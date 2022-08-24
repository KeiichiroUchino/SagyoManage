using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.ComLib.Text;
using System.Text;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.BizProperty.SettingsModel;
using Jpsys.HaishaManageV10.ComLib;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class KenAllCsvImportFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "郵便辞書設定（KEN_ALL）";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// KEN_ALL_CSV情報リスト
        /// </summary>
        private IList<KenAllCsvInfo> _KenAllCsvList;

        /// <summary>
        /// 排他制御情報
        /// </summary>
        private ExclusiveControlInfo _ExclusiveControlInfo;

        /// <summary>
        /// CSVデータなしメッセージ
        /// </summary>
        private const string CSV_NO_RECODE = "CSV_NO_RECODE";

        /// <summary>
        /// CSV取込完了フラグ
        /// </summary>
        private bool _ImportCsvResultFlag = false;

        #region コマンド

        private CommandSet commandSet;

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public KenAllCsvImportFrame()
        {
            InitializeComponent();
        }

        #endregion
        
        #region 初期化処理

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitKenAllCsvImportFrame()
        {
            // 画面タイトルの設定
            this.Text = WINDOW_TITLE;

            // 画面配色の初期化
            this.InitFrameColor();

            // 親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // ファイルダイアログ設定
            this.SetOpenFileDiarog();

            // 入力項目のクリア
            this.ClearInputs();

            //排他制御情報取得
            this._ExclusiveControlInfo =
                this._DalUtil.ExclusiveControl.GetInfoBySearchParameter(
                new ExclusiveControlSearchParameter()
                {
                    ExclusiveControlKbn = (int)DefaultProperty.ExclusiveControlKbn.KenAll,
                    KinoKbn = 0,
                    ShoriValue = 0
                });

            if (this._ExclusiveControlInfo.ExclusiveControlKbn == 0)
            {
                this._ExclusiveControlInfo.ExclusiveControlKbn = (int)DefaultProperty.ExclusiveControlKbn.KenAll;
                this._ExclusiveControlInfo.KinoKbn = 0;
                this._ExclusiveControlInfo.ShoriValue = 0;
            }

            // フォーカス設定
            this.SetFirstFocus();
        }

        /// <summary>
        /// 画面配色を初期化します。
        /// </summary>
        private void InitFrameColor()
        {
            //表のヘッダー背景色、表の選択行背景色の設定
            FrameUtilites.SetFrameGridBackColor(this);

            //ステータスバーの背景色設定
            this.statusStrip1.BackColor = FrameUtilites.GetFrameFooterBackColor();
        }

        /// <summary>
        /// メニュー関連を初期化します。
        /// </summary>
        private void InitMenuItem()
        {
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            this.actionMenuItems = new AddActionMenuItem();

            this.actionMenuItems.SetCreatingItem(ActionMenuItems.ImportCsv);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***CSV取込
            commandSet.ImportCsv.Execute += ImportCsvCommand_Execute;
            commandSet.Bind(commandSet.ImportCsv,
               this.btnImportCsv, actionMenuItems.GetMenuItemBy(ActionMenuItems.ImportCsv), this.toolStripImportCsv);

            //***終了
            commandSet.Close.Execute += CloseCommand_Execute;
            commandSet.Bind(commandSet.Close,
               this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        private void ImportCsvCommand_Execute(object sender, EventArgs e)
        {
            // CSV取込
            this.DoImportCsv();
        }

        private void CloseCommand_Execute(object sender, EventArgs e)
        {
            // 終了
            this.DoClose();
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            this.searchStateBinder.AddSearchableControls(
                this.edtCsvPath
                );

            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
        }

        /// <summary>
        /// ファイルダイアログの設定を行います。
        /// </summary>
        private void SetOpenFileDiarog()
        {
            //ファイル名設定
            if (this.openFileDialog1.FileName.Equals(string.Empty))
            {
                this.openFileDialog1.FileName = DefaultProperty.KEN_ALL_CSVFILE_DEFAULT_NAME;
            }
            //ファイルの種類設定
            this.openFileDialog1.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            this.openFileDialog1.FilterIndex = 2;
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            this.openFileDialog1.RestoreDirectory = true;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            KenAllCsvSettingInfo info = BizCommon.GetKenAllCsvSettingInfo();
            if (info != null)
            {
                this.edtCsvPath.Text = info.KenAllCsvImportFilePath;
            }
            else
            {
                this.edtCsvPath.Text = DefaultProperty.KEN_ALL_CSVFILE_DEFAULT_NAME;
            }

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #endregion

        #region プライベート メソッド
        
        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// KEN_ALL_CSVファイルパス情報を保存します。
        /// </summary>
        private void SaveCsvPath()
        {
            KenAllCsvSettingInfo info = new KenAllCsvSettingInfo();
            info.KenAllCsvImportFilePath = Convert.ToString(this.edtCsvPath.Text);
            BizCommon.SaveKenAllCsvSettingInfo(info);
        }

        /// <summary>
        /// 指定したファイルをCSV取込します。
        /// </summary>
        /// <returns>処理結果</returns>
        private void DoImportCsv()
        {
            //全コントロールの入力チェック
            if (!this.ValidateChildren())
                return;

            //画面を使用不可に
            this.ControlEnabledForImporting(false);
            //画面タイトルを変更する
            this.Text = WINDOW_TITLE + "　" + "取込中...";
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //KEN_ALL_CSV情報初期化
                this._KenAllCsvList = null;

                //入力チェック
                if (!this.CheckInputs())
                    return;

                //郵便番号データ取込
                SQLHelper.ActionWithTransaction(tx =>
                {
                    this._DalUtil.WK_PostalZipData.Save(tx, this._ExclusiveControlInfo, this._KenAllCsvList, this.radModeAraigae.Checked);
                });

                //ログ出力
                FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                    FrameLogWriter.LoggingOperateKind.UpdateItem,
                    this.Createlog());

                //登録完了メッセージ
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MI2001003"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                //洗い替えの場合
                if (this.radModeAraigae.Checked)
                {
                    //終了
                    this.DoClose();
                }
                //追記の場合
                else
                {
                    //排他制御情報取得
                    this._ExclusiveControlInfo =
                        this._DalUtil.ExclusiveControl.GetInfoBySearchParameter(
                        new ExclusiveControlSearchParameter()
                        {
                            ExclusiveControlKbn = (int)DefaultProperty.ExclusiveControlKbn.KenAll,
                            KinoKbn = 0,
                            ShoriValue = 0
                        });

                    if (this._ExclusiveControlInfo.ExclusiveControlKbn == 0)
                    {
                        this._ExclusiveControlInfo.ExclusiveControlKbn = (int)DefaultProperty.ExclusiveControlKbn.KenAll;
                        this._ExclusiveControlInfo.KinoKbn = 0;
                        this._ExclusiveControlInfo.ShoriValue = 0;
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                MessageBox.Show(
                    "ファイルオープンに失敗しました。" +
                        "\r\n" + e.Message,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
            finally
            {
                //画面を使用可能に
                this.ControlEnabledForImporting(true);
                //画面タイトルをもとに戻す
                this.Text = WINDOW_TITLE;
                //かードルをもとに戻す
                this.Cursor = Cursors.Default;
                //条件項目にフォーカス
                this.SetFirstFocus();
            }
        }

        /// <summary>
        /// 指定したファイルをCSV取込します。
        /// </summary>
        /// <returns>エラーリスト</returns>
        private IList<string> GetKenAllCsvList()
        {
            IList<string> errList = new List<string>();

            IList<KenAllCsvInfo> kenAllCsvList = new List<KenAllCsvInfo>();

            //レコード件数
            int cnt = 0;

            //CSVファイルを開く
            using (var sr = new System.IO.StreamReader(this.edtCsvPath.Text,System.Text.Encoding.GetEncoding("shift_jis")))
            {
                //ストリームの末尾まで繰り返す
                while (!sr.EndOfStream)
                {
                    //レコード件数加算
                    cnt++;

                    //ファイルから一行読み込む
                    var line = sr.ReadLine();

                    ArrayList rowList = new ArrayList();

                    //読み込んだ一行をカンマ毎に分けて配列に格納する
                    rowList = CsvHelper.CsvToArrayList(line);

                    //無効行の場合
                    if (rowList == null || rowList.Count == 0)
                    {
                        continue;
                    }

                    //項目リスト取得
                    ArrayList items = (ArrayList)rowList[0];

                    //項目数不足の場合
                    if (items.Count < KenAllCsvInfo.CSV_MCURUMN_COUNT)
                    {
                        continue;
                    }

                    //KEN_ALL_CSV情報に格納
                    kenAllCsvList.Add(this.GetKenAllCsvInfo(rowList));
                }

                this._KenAllCsvList = kenAllCsvList;
            }

            if (cnt == 0)
            {
                errList.Add(CSV_NO_RECODE);
            }

            return errList;
        }

        /// <summary>
        /// 指定したファイルをCSV取込します。
        /// </summary>
        /// <param name="rowList">読込行情報</param>
        /// <returns>KEN_ALL_CSV情報</returns>
        private KenAllCsvInfo GetKenAllCsvInfo(ArrayList rowList)
        {
            //戻り値
            KenAllCsvInfo ret_info = new KenAllCsvInfo();

            //項目リスト取得
            ArrayList items = (ArrayList)rowList[0];

            try
            {
                //利用者ID
                ret_info.OperatorId = this.appAuth.OperatorId;
                //全国地方公共団体コード
                ret_info.JisCode = items[(int)KenAllCsvInfo.CsvCurumnIndices.JisCode].ToString();
                //（旧）郵便番号（5桁）
                ret_info.Post5Code = items[(int)KenAllCsvInfo.CsvCurumnIndices.Post5Code].ToString();
                //郵便番号（7桁）
                ret_info.PostCode = items[(int)KenAllCsvInfo.CsvCurumnIndices.PostCode].ToString();
                //都道府県名カナ
                ret_info.PrefectureKana = items[(int)KenAllCsvInfo.CsvCurumnIndices.PrefectureKana].ToString();
                //市区町村名カナ
                ret_info.CityKana = items[(int)KenAllCsvInfo.CsvCurumnIndices.CityKana].ToString();
                //町域名カナ
                ret_info.TownAreaKana = items[(int)KenAllCsvInfo.CsvCurumnIndices.TownAreaKana].ToString();
                //都道府県名
                ret_info.Prefecture = items[(int)KenAllCsvInfo.CsvCurumnIndices.Prefecture].ToString();
                //市区町村名
                ret_info.City = items[(int)KenAllCsvInfo.CsvCurumnIndices.City].ToString();
                //町域名
                ret_info.TownArea = items[(int)KenAllCsvInfo.CsvCurumnIndices.TownArea].ToString();
                //一町域が二以上の郵便番号で表される場合の表示
                ret_info.IsOneTownByMultiPostCode = StringExtensions.ToInt32(items[(int)KenAllCsvInfo.CsvCurumnIndices.IsOneTownByMultiPostCode].ToString());
                //小字毎に番地が起番されている町域の表示
                ret_info.IsNeedSmallAreaAddress = StringExtensions.ToInt32(items[(int)KenAllCsvInfo.CsvCurumnIndices.IsNeedSmallAreaAddress].ToString());
                //丁目を有する町域の場合の表示
                ret_info.IsChome = StringExtensions.ToInt32(items[(int)KenAllCsvInfo.CsvCurumnIndices.IsChome].ToString());
                //一つの郵便番号で二以上の町域を表す場合の表示
                ret_info.IsMultiTownByOnePostCode = StringExtensions.ToInt32(items[(int)KenAllCsvInfo.CsvCurumnIndices.IsMultiTownByOnePostCode].ToString());
                //更新の表示
                ret_info.Updated = StringExtensions.ToInt32(items[(int)KenAllCsvInfo.CsvCurumnIndices.Updated].ToString());
                //表示抑制
                ret_info.UpdateReason = StringExtensions.ToInt32(items[(int)KenAllCsvInfo.CsvCurumnIndices.UpdateReason].ToString());
            }
            catch
            {
                ret_info = new KenAllCsvInfo();
            }

            return ret_info;
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns>ログに出力する文字</returns>
        private string Createlog()
        {
            //操作ログ(保存)の条件取得
            string log_joken = FilePathString;

            return log_joken;
        }

        /// <summary>
        ///ファイル選択ダイアログを表示します。
        /// </summary>
        private void ShowOpenFileDialog()
        {
            //ダイアログを表示する
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                this.edtCsvPath.Text = this.openFileDialog1.FileName;

                //CSVファイルパスを設定ファイルに保存
                this.SaveCsvPath();
            }
        }

        /// <summary>
        /// 詳細の入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputs()
        {
            bool rt_val = true;

            string msg = string.Empty;
            Control ctl = null;

            //CSVエラーリスト初期化
            IList<string> errList = new List<string>();

            //取込元存在チェック
            if (rt_val && (this.edtCsvPath == null || this.edtCsvPath.Text.Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { "取込元" });
                ctl = this.edtCsvPath;
            }
            //取込元が存在する場合
            else if (rt_val)
            {
                //ファイルの存在チェック
                if (!System.IO.File.Exists(this.edtCsvPath.Text))
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203070", new string[] { "取込元", this.edtCsvPath.Text });
                    ctl = this.edtCsvPath;
                }
                else
                {
                    try
                    {
                        //KEN_ALL_CSVファイル取得
                        errList = this.GetKenAllCsvList();
                    }
                    catch (Exception)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage("ME2303005", new string[] { "CSVファイル", "読込" });
                        ctl = this.edtCsvPath;
                    }

                    if (0 < errList.Count)
                    {
                        if (errList[0].Equals(CSV_NO_RECODE))
                        {
                            rt_val = false;
                            msg = "CSVファイルに有効なデータが存在しません。";
                            ctl = this.edtCsvPath;
                        }
                        else
                        {
                            rt_val = false;
                            StringBuilder sbErrs = new StringBuilder();
                            sbErrs.AppendLine("【CSVファイル】に下記エラーが存在します。");
                            sbErrs.AppendLine(string.Empty);
                            int i = 0;
                            foreach (string err in errList)
                            {
                                sbErrs.AppendLine(err);
                                i++;

                                if (15 < i)
                                {
                                    sbErrs.AppendLine("…");
                                    break;
                                }
                            }
                            msg = sbErrs.ToString();
                        }
                    }
                }
            }

            if (!rt_val)
            {
                MessageBox.Show(
                    msg,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

                if (ctl != null)
                {
                    ctl.Focus();
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                }
            }

            //return rt_val && zeroSaveFlag;
            return rt_val;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        #endregion

        #region 取込関係

        /// <summary>
        /// 取込中の処理においてコントロールの有効無効を切り替えます。
        /// 取込のための処理中に不用意にコントロールが操作されることを
        /// 防ぐためです。
        /// </summary>
        /// <param name="val">コントロールの有効・無効(true:有効）</param>
        private void ControlEnabledForImporting(bool val)
        {
            this.menuStripTop.Enabled = val;
            this.pnlMain.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.ImportCsv, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Close, val);
        }

        #endregion

        #region 検索処理

        /// <summary>
        /// キーイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessKeyEvent(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    // F5は共通検索画面
                    this.ShowCmnSearch();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// フォーカスしているコントロールに対応する共通検索画面を表示します。
        /// </summary>
        private void ShowCmnSearch()
        {
            if (this.ActiveControl == this.edtCsvPath)
            {
                this.ShowOpenFileDialog();
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 「ファイルパス」の条件指定を文字型で取得します。
        /// </summary>
        private String FilePathString
        {
            get { return string.Format("[ファイル] {0}", this.edtCsvPath.Text); }
        }

        /// <summary>
        /// CSV取込の実行結果を取得します。
        /// </summary>
        public bool ImportCsvResultFlag
        {
            get { return this._ImportCsvResultFlag; }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitKenAllCsvImportFrame();
        }

        /// <summary>
        /// 本クラスの Name プロパティを取得・設定します。
        /// </summary>
        public string FrameName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// 本クラスの Text プロパティを取得・設定します。
        /// </summary>
        public string FrameText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        #endregion
        
        /// <summary>
        /// OS規程の操作によりフォームを閉じる操作をした場合に、フォームの
        /// 自動入力検証(AutoValidate)処理を行わないようにするために、Windowsメッセージ
        /// の処理メソッドをオーバーライドして、該当する操作時に自動入力検証を
        /// 無効にします。
        /// 無効後はいずれかの処理で必ず自動入力検証をONにしてください。
        /// </summary>
        /// <param name="m">処理対象のWindows Message</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_CLOSE = 0x10;
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xf060;

            switch (m.Msg)
            {
                case WM_SYSCOMMAND:
                    if (m.WParam.ToInt32() == SC_CLOSE)
                    {
                        //Xボタン、コントロールメニューの「閉じる」、 
                        //コントロールボックスのダブルクリック、 
                        //Atl+F4などにより閉じられようとしている 
                        //このときValidatingイベントを発生させない。 
                        this.AutoValidate = AutoValidate.Disable;
                    }

                    break;
                case WM_CLOSE:
                    //Application.Exit以外で閉じられようとしている 
                    //このときValidatingイベントを発生させない。 
                    this.AutoValidate = AutoValidate.Disable;
                    break;
            }

            base.WndProc(ref m);
        }

        private void KenAllCsvImportFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void KenAllCsvImportFrame_KeyDown(object sender, KeyEventArgs e)
        {
            // 画面 KeyDown
            this.ProcessKeyEvent(e);
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void sbtnCsvPath_Click(object sender, EventArgs e)
        {
            this.ShowOpenFileDialog();
        }

        private void lblPost_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lblPost.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.post.japanpost.jp/zipcode/dl/oogaki-zip.html");
        }
    }
}
