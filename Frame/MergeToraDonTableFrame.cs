using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.BizProperty.SettingsModel;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.SQLServerDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class MergeToraDonTableFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル初期値
        /// </summary>
        private const string WINDOW_TITLE = "トラDONマスタ同期";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示している操作メニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// トラDONマスタ同期クラス
        /// </summary>
        private MergeToraDon _MergeToraDon;

        #region 対象テーブルリスト

        //--Spread列定義

        /// <summary>
        /// 対象テーブル名列番号
        /// </summary>
        private const int COL_TARGETTABLE_NAME = 0;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_SELECT_CHECKBOX = 1;

        /// <summary>
        /// 対象テーブル区分列番号
        /// </summary>
        private const int COL_TARGETTABLE_KBN = 2;

        /// <summary>
        /// 対象テーブルリスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_TARGETTABLELIST = 3;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialTargetTableStyleInfoArr;

        #endregion

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

        //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, [MarshalAs(UnmanagedType.Bool)] bool pvParam, UInt32 fWinIni);
        [Flags]
        public enum SystemParametersInfoActionFlag : uint
        {
            SPI_SETKEYBOARDCUES = 0x100B,
        }
        [Flags]
        public enum SystemParametersInfoFlag : uint
        {
            SPIF_UPDATEINIFILE = 0x0001,
            SPIF_SENDWININICHANGE = 0x0002,
        }

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public MergeToraDonTableFrame()
        {
            //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
            SystemParametersInfo((uint)SystemParametersInfoActionFlag.SPI_SETKEYBOARDCUES, 0,
                true, (uint)(SystemParametersInfoFlag.SPIF_UPDATEINIFILE | SystemParametersInfoFlag.SPIF_SENDWININICHANGE));

            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitSeikyuDataSakuseiFrame()
        {
            //画面タイトルの設定
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

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // トラDONマスタ同期クラスインスタンス作成
            this._MergeToraDon = new MergeToraDon(this.appAuth);

            // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            // Spread 関連の初期化
            this.InitSheet();

            // 入力項目のクリア
            this.ClearInputs();

            // 対象テーブルリストのセット
            this.SetTargetTableListSheet();

            // コントロール活性化
            this.InitControlsEnabled();

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

            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***終了
            this.commandSet.Close.Execute += CloseCommand_Execute;
            this.commandSet.Bind(
                this.commandSet.Close,
                this.btnClose,
                this.actionMenuItems.GetMenuItemBy(ActionMenuItems.Close),
                this.toolStripEnd
                );
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
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStyleInfo()
        {
            //SpreadのStyle情報を格納するメンバ変数の初期化
            this.InitSpreadStyleInfo();
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitSpreadStyleInfo()
        {
            // 対象テーブルリストスタイル情報初期化
            this.InitTargetTableStyleInfo();
        }

        /// <summary>
        /// 対象テーブルリストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitTargetTableStyleInfo()
        {
            // Id列非表示
            this.fpTargetTableListGrid.Sheets[0].Columns[COL_TARGETTABLE_KBN].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpTargetTableListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialTargetTableStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_TARGETTABLELIST];

            for (int i = 0; i < COL_MAXCOLNUM_TARGETTABLELIST; i++)
            {
                this.initialTargetTableStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        #endregion

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //対象テーブルリストの初期化
            this.InitTargetTableListSheet();
        }

        /// <summary>
        /// 対象テーブルを初期化します。
        /// </summary>
        private void InitTargetTableListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpTargetTableListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_TARGETTABLELIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.radAll.Checked = true;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #region プライベート メソッド

        /// <summary>コントロールの活性／非活性を初期化します。
        /// </summary>
        private void InitControlsEnabled()
        {
            this.ChangeTargetTableCondition();
        }

        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            this.groupBox2.Focus();
            this.ActiveControl = this.groupBox2;
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 対象テーブル指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeTargetTableCondition()
        {
            this.fpTargetTableListGrid.Enabled = this.radKobetsuShitei.Checked;

            // 対象テーブルリストが非活性の場合
            if (!this.fpTargetTableListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpTargetTableListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpTargetTableListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpTargetTableListGrid.Sheets[0].ClearSelection();

                //ボタン非活性
                this.btnSelectAll.Enabled = false;
                this.btnCancelSelected.Enabled = false;
            }
            else
            {
                //ボタン非活性
                this.btnSelectAll.Enabled = true;
                this.btnCancelSelected.Enabled = true;
            }
        }

        /// <summary>
        /// 対象テーブルリストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckTargetTableList(int rowIndex)
        {
            this.fpTargetTableListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpTargetTableListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 対象テーブルリストに値を設定します。
        /// </summary>
        private void SetTargetTableListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(
                        MergeToraDon.MARGETORADONTABLEKBN_TOTAL_COUNT, COL_MAXCOLNUM_TARGETTABLELIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(
                        MergeToraDon.MARGETORADONTABLEKBN_TOTAL_COUNT, COL_MAXCOLNUM_TARGETTABLELIST);

                for (int i = 0; i < MergeToraDon.MARGETORADONTABLEKBN_TOTAL_COUNT; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_TARGETTABLELIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialTargetTableStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    try
                    {
                        DefaultProperty.MergeToraDonTableKbn kbn = (DefaultProperty.MergeToraDonTableKbn)i;
                    }
                    //テーブル対象外の場合
                    catch (Exception)
                    {
                        //スキップ
                        continue;
                    }

                    datamodel.SetValue(i, COL_TARGETTABLE_NAME,
                        DefaultProperty.GetMergeToraDonTableKbnMeisho((DefaultProperty.MergeToraDonTableKbn)i + 1));
                    datamodel.SetValue(i, COL_TARGETTABLE_KBN, i + 1);
                }

                //Spreadにデータモデルをセット
                this.fpTargetTableListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpTargetTableListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpTargetTableListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpTargetTableListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// トラDONのマスタ同期を実行します。
        /// </summary>
        private void DoMerge()
        {
            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
            {
                //実行確認ダイアログ
                DialogResult d_result =
                    MessageBox.Show(
                    "同期処理を開始します。よろしいですか？",
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                //Noだったら抜ける
                if (d_result == DialogResult.No)
                {
                    return;
                }

                //待機状態へ
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        if (this.radAll.Checked)
                        {
                            this._MergeToraDon.MergeToraDonAllTables(tx);
                        }
                        else
                        {
                            this._MergeToraDon.MergeToraDonTables(this.GetSelectTargetTableList(), tx);
                        }
                    });

                    //操作ログ出力
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.UpdateItem,
                           this.CreateLog());

                    //更新完了メッセージ
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MI2001003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //フォーカスを移動
                    this.SetFirstFocus();
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    this.SetFirstFocus();
                }
                catch (MustCloseFormException ex)
                {
                    //画面の終了が要求される例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //画面を閉じます
                    this.DoClose();
                }
                catch (Model.DALExceptions.UniqueConstraintException ex)
                {
                    //同一コードが存在する場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    this.SetFirstFocus();
                }
                catch (Exception ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    this.SetFirstFocus();
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
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


            // 個別指定の場合
            if (this.radKobetsuShitei.Checked)
            {
                int cnt = 0;
                //チェック件数取得
                for (int i = 0; i < this.fpTargetTableListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpTargetTableListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value)
                        cnt++;
                }
                //チェックが0件の場合
                if (rt_val && cnt == 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { "対象テーブル" });
                    ctl = this.fpTargetTableListGrid;
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

            return rt_val;
        }

        /// <summary>
        ///ファイル選択ダイアログを表示します。
        /// </summary>
        private string ShowSaveFileDialog()
        {
            string rt_str = string.Empty;

            //初期フォルダパスの初期値を設定
            this.saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //初期フォルダパスを設定
            HaishaCsvSettingInfo info = BizCommon.GetHaishaCsvSettingInfo();
            if (info != null)
            {
                //ディレクトリが存在する場合
                if (Directory.Exists(Path.GetDirectoryName(info.HaishaCsvExportFilePath)))
                {
                    this.saveFileDialog1.InitialDirectory = info.HaishaCsvExportFilePath;
                }
            }

            //初期ファイル名を設定
            this.saveFileDialog1.FileName = WINDOW_TITLE + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

            //ファイルダイアログを開く
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //ファイルパスを取得
                rt_str = this.saveFileDialog1.FileName;

                //CSVファイルパスを設定ファイルに保存
                this.SaveCsvPath(System.IO.Path.GetDirectoryName(this.saveFileDialog1.FileName));
            }

            return rt_str;
        }

        /// <summary>
        /// 配車情報CSVファイルパス情報を保存します。
        /// </summary>
        private void SaveCsvPath(string dir)
        {
            HaishaCsvSettingInfo info = new HaishaCsvSettingInfo();
            info.HaishaCsvExportFilePath = dir;
            BizCommon.SaveHaishaCsvSettingInfo(info);
        }

        /// <summary>
        /// 印刷中の処理においてコントロールの有効無効を切り替えます。
        /// 印刷のための処理中に不用意にコントロールが操作されることを
        /// 防ぐためです。
        /// </summary>
        /// <param name="val">コントロールの有効・無効(true:有効）</param>
        private void ControlEnableChangeForMerging(bool val)
        {
            this.menuStripTop.Enabled = val;
            this.pnlMain.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Close, val);
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string CreateLog()
        {
            //操作ログ(保存)の条件取得
            string log_jyoken =
                "[範囲指定] " + TargetTableKbnString;

            if (!this.radAll.Checked)
            {
                log_jyoken = log_jyoken +
                    "\r\n[対象テーブル]" + TargetTableListString;
            }

            return log_jyoken;
        }

        /// <summary>
        /// 画面「対象テーブル一覧」で選択されている対象テーブル情報リストを取得します。
        /// </summary>
        /// <returns>選択されている対象テーブル情報リスト</returns>
        private List<int> GetSelectTargetTableList()
        {
            SheetView sheet0 = this.fpTargetTableListGrid.ActiveSheet;

            List<int> selectList = new List<int>();
            for (int i = 0; i < sheet0.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet0.GetValue(i, COL_SELECT_CHECKBOX)))
                {
                    //情報を取得
                    selectList.Add((int)sheet0.Cells[i, COL_TARGETTABLE_KBN].Value);
                }
            }
            return selectList;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// リスト一括選択
        /// </summary>
        /// <param name="checkedflag">選択可否（true：選択、false：解除）</param>
        private void SetSelectCheckBox(bool checkedflag)
        {
            for (int i = 0; i < this.fpTargetTableListGrid.Sheets[0].RowCount; i++)
            {
                this.fpTargetTableListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = checkedflag;
            }
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
                default:
                    break;
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 画面「対象テーブル」の条件指定を文字型で取得します。
        /// </summary>
        private String TargetTableKbnString
        {
            get { return this.radAll.Checked ? "すべて" : "個別指定"; }
        }

        /// <summary>
        /// 画面「選択対象テーブル」の条件指定を文字型で取得します。
        /// </summary>
        private String TargetTableListString
        {
            get { return string.Join(",", this.GetSelectTargetTableList()); }
        }

        #endregion

        #region プライベートクラス

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitSeikyuDataSakuseiFrame();
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

        private void MergeToraDonTableFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void MergeToraDonTableFrame_KeyDown(object sender, KeyEventArgs e)
        {
            // 画面 KeyDown
            this.ProcessKeyEvent(e);
        }

        private void radTargetTable_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeTargetTableCondition();
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void fpTargetTableListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.F12:
                    SendKeys.SendWait("+{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckTargetTableList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ymd_Down(object sender, EventArgs e)
        {
            if (this.ActiveControl.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)this.ActiveControl).Value = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(-1);
                }
                catch
                {
                    ;
                }
            }
        }

        private void ymd_Up(object sender, EventArgs e)
        {
            if (this.ActiveControl.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)this.ActiveControl).Value = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(1);
                }
                catch
                {
                    ;
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.SetSelectCheckBox(true);
        }

        private void btnCancelSelected_Click(object sender, EventArgs e)
        {
            this.SetSelectCheckBox(false);
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            this.DoMerge();
        }

        private void fpTargetTableListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckTargetTableList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpTargetTableListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }
    }
}
