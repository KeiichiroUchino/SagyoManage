using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.Frame.Command;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.FrameLib.ViewSupport;
using Jpsys.SagyoManage.FrameLib.MultiRow;
using Jpsys.SagyoManage.FrameLib.WinForms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using GrapeCity.Win.Editors;
using System.ComponentModel;
using jp.co.jpsys.util;
using System.Drawing;
using InputManCell = GrapeCity.Win.MultiRow.InputMan;
using Jpsys.SagyoManage.ComLib;
using System.IO;
using System.Text;
using System.Data.SqlClient;

namespace Jpsys.SagyoManage.Frame
{
    public partial class ShelterRenkeSagyoBunruiFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public ShelterRenkeSagyoBunruiFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "外部システム連携（Shelter）作業分類";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// 現在の画面の編集モードを保持する領域
        /// </summary>
        private FrameEditMode currentMode = FrameEditMode.Default;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 作業大分類クラス
        /// </summary>
        private SagyoDaiBunrui _SagyoDaiBunrui;

        /// <summary>
        /// 作業中分類クラス
        /// </summary>
        private SagyoChuBunrui _SagyoChuBunrui;

        /// <summary>
        /// 作業小分類クラス
        /// </summary>
        private SagyoShoBunrui _SagyoShoBunrui;

        /// <summary>
        /// 作業大分類パラメータリスト
        /// </summary>
        private List<SagyoDaiBunruiInfo> _SagyoDaiBunruiInfoList;

        /// <summary>
        /// 作業中分類パラメータリスト
        /// </summary>
        private List<SagyoChuBunruiInfo> _SagyoChuBunruiInfoList;

        /// <summary>
        /// 作業小分類パラメータリスト
        /// </summary>
        private List<SagyoShoBunruiInfo> _SagyoShoBunruiInfoList;

        /// <summary>
        /// 画面表示可能かどうかの値を保持する領域（true：表示可）
        /// </summary>
        private bool canShowFrame = true;

        /// <summary>
        /// 画面を表示する時のエラーメッセージを保持する領域
        /// </summary>
        private string showFrameMsg = string.Empty;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = false;

        /// <summary>
        /// 再表示を行うかどうかの値を保持する領域
        /// (true:再表示する)
        /// (false:再表示しない)
        /// </summary>
        private bool isReDraw = false;

        /// <summary>
        /// 一覧のインスタンス
        /// </summary>
        public KeiyakuViewFrame KeiyakuIchiran { get; set; }

        /// <summary>
        /// 一覧にて選択されたID
        /// </summary>
        public decimal SelectDtlId { get; set; } = decimal.Zero;

        #endregion

        #region CSV列定義

        private enum CsvSagyoDaiBunrui : int
        {
            ShelterId,
            SagyoDaiBunruiCode,
            SagyoDaiBunruiName,
        };

        private enum CsvSagyoChuBunrui : int
        {
            ShelterId,
            SagyoDaiBunruiId,
            SagyoChuBunruiCode,
            SagyoChuBunruiName,
        };

        private enum CsvSagyoShoBunrui : int
        {
            ShelterId,
            SagyoDaiBunruiId,
            SagyoChuBunruiId,
            SagyoShoBunruiCode,
            SagyoShoBunruiName,
        };

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitJuchuNyuryokuFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //画面配色の初期化
            this.InitFrameColor();

            //親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //メニューの初期化
            this.InitMenuItem();

            //バインドの設定
            this.SettingCommands();
            this.SetupValidatingEventRaiser();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //作業分類クラスインスタンス作成
            this._SagyoDaiBunrui = new SagyoDaiBunrui(this.appAuth);
            this._SagyoChuBunrui = new SagyoChuBunrui(this.appAuth);
            this._SagyoShoBunrui = new SagyoShoBunrui(this.appAuth);

            //入力項目のクリア
            this.ClearInputs();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);
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
        /// メニュー関連の初期化をします。
        /// </summary>
        private void InitMenuItem()
        {
            // 操作メニュー
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.ImportCsv);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.edtImportFilePath.Text = string.Empty;

            this.rbtSagyoDaiBunrui.Checked = true;

            //メンバをクリア
            this.isConfirmClose = false;
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
        }

        #endregion


        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***CSV取込
            _commandSet.ImportCsv.Execute += Save_Execute;
            _commandSet.Bind(
                _commandSet.ImportCsv, this.btnSave, actionMenuItems.GetMenuItemBy(ActionMenuItems.ImportCsv), this.toolStripImportCsv);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            this.DoClear(true);
        }

        void Save_Execute(object sender, EventArgs e)
        {
            this.DoUpdate();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        private void JuchuNyuryokuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void JuchuNyuryokuFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);

            //画面表示時のメッセージ表示
            if (this.showFrameMsg.Trim().Length != 0)
            {
                MessageBox.Show(
                    this.showFrameMsg,
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                if (!this.canShowFrame)
                {
                    //画面を閉じる（終了確認はしない）
                    this.isConfirmClose = false;
                    this.DoClose();
                }
            }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitJuchuNyuryokuFrame();
        }

        /// <summary>
        /// 本インスタンスのNameプロパティをインターフェース経由で
        /// 取得・設定します。
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
        /// 本インスタンスのTextプロパティをインターフェース経由で
        /// 取得・設定します。
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

        #region プライベートメソッド

        /// <summary>
        /// 取り込むファイルのパスを選択します。
        /// </summary>
        private void SelectInportFilePath()
        {
            //対象フォルダのパスの変数
            string filePath = string.Empty;

            using (OpenFileDialog fbd = new OpenFileDialog())
            {
                //フォルダのパスの指定(マイドキュメントを指定)
                //fbd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                fbd.Title = "ファイルを選択してください";

                fbd.Filter = "CSV ファイル (*.csv)|*.csv";

                //OKのときはパスを取得
                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    filePath = fbd.FileName;
                }
                else
                {
                    //それ以外は戻る
                    return;
                }
            }

            //ファイパスをセット
            this.edtImportFilePath.Text = filePath;
            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Editable);
        }

        /// <summary>
        /// CSVファイルの取込処理を行います
        /// </summary>
        private bool DoImportCsvFile(SqlTransaction transaction)
        {
            //メンバをクリア
            this._SagyoDaiBunruiInfoList = new List<SagyoDaiBunruiInfo>();
            this._SagyoChuBunruiInfoList = new List<SagyoChuBunruiInfo>();
            this._SagyoShoBunruiInfoList = new List<SagyoShoBunruiInfo>();

            try
            {
                string text = File.ReadAllText(this.edtImportFilePath.Text, Encoding.GetEncoding("shift_jis"));
                var lines = FrameUtilites.CsvToList(text);

                //CSVデータの取り込み
                return this.LoadCsvData(lines, transaction);

            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        /// CSVを読み込みます。
        /// </summary>
        private bool LoadCsvData(List<List<string>> csvlines, SqlTransaction transaction)
        {
            //***テキストがあるかチェック
            if (csvlines.Count < 2)
            {
                MessageBox.Show(
                    "取込対象となるデータが存在しません。",
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            ////1行目の1項目のチェック
            //if (csvlines.FirstOrDefault().FirstOrDefault() != "AIRIS")
            //{
            //    MessageBox.Show(
            //        "選択したファイルは「自動車検査登録情報提供サービス」から提供されたファイルはありません。",
            //        this.Text,
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    return false;
            //}

            //IDリスト
            List<string> idList = new List<string>();

            //CSVファイルの行ごとでループを回す
            for (int i = 0; i < csvlines.Count(); i++)
            {
                //CSVファイルの1行目は読み飛ばす
                if (i == 0)
                    continue;

                //1行を取得
                List<string> line = csvlines[i];

                try
                {
                    if (this.rbtSagyoDaiBunrui.Checked)
                    {
                        if (!string.IsNullOrWhiteSpace(line[(int)CsvSagyoDaiBunrui.ShelterId]))
                        {
                            decimal chkId = 0;

                            if (decimal.TryParse(line[(int)CsvSagyoDaiBunrui.ShelterId], out chkId))
                            {
                                idList.Add(chkId.ToString());
                            }
                        }

                    }
                    else if (this.rbtSagyoChuBunrui.Checked)
                    {
                        if (!string.IsNullOrWhiteSpace(line[(int)CsvSagyoChuBunrui.ShelterId]))
                        {
                            decimal chkId = 0;

                            if (decimal.TryParse(line[(int)CsvSagyoChuBunrui.ShelterId], out chkId))
                            {
                                idList.Add(chkId.ToString());
                            }
                        }
                    }
                    else 
                    {
                        if (!string.IsNullOrWhiteSpace(line[(int)CsvSagyoShoBunrui.ShelterId]))
                        {
                            decimal chkId = 0;

                            if (decimal.TryParse(line[(int)CsvSagyoShoBunrui.ShelterId], out chkId))
                            {
                                idList.Add(chkId.ToString());
                            }
                        }
                    }

                }
                catch (CanRetryException err)
                {

                    MessageBox.Show(
                        err.Message,
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    err.Msgicon);

                    return false;
                }
            }

            // 画面コントローラに取得を指示
            // 作業分類一覧取得(行ロック)
            List<SagyoDaiBunruiInfo> sagyoDaiBunruiList = null;
            List<SagyoChuBunruiInfo> sagyoChuBunruiList = null;
            List<SagyoShoBunruiInfo> sagyoShoBunruiList = null;

            //作業分類の取得
            var dList = _DalUtil.SagyoDaiBunrui.GetList();
            var cList = _DalUtil.SagyoChuBunrui.GetList();

            if (this.rbtSagyoDaiBunrui.Checked)
            {
                sagyoDaiBunruiList = this._SagyoDaiBunrui.BulkGetListInternal(idList.Distinct().ToList(), transaction, true);
            }
            else if (this.rbtSagyoChuBunrui.Checked)
            {
                sagyoChuBunruiList = this._SagyoChuBunrui.BulkGetListInternal(idList.Distinct().ToList(), transaction, true);
            }
            else
            {
                sagyoShoBunruiList = this._SagyoShoBunrui.BulkGetListInternal(idList.Distinct().ToList(), transaction, true);
            }

            //エラーリスト
            List<string> errList = new List<string>();

            //CSVファイルの行ごとでループを回す
            for (int i = 0; i < csvlines.Count(); i++)
            {
                //CSVファイルの1行目は読み飛ばす
                if (i == 0)
                    continue;

                //1行を取得
                List<string> line = csvlines[i];

                try
                {
                    if (this.rbtSagyoDaiBunrui.Checked)
                    {
                        SagyoDaiBunruiInfo info = null;

                        if (line.Count() != Enum.GetNames(typeof(CsvSagyoDaiBunrui)).Length)
                        {
                            errList.Add(string.Format("CSVファイルの{0}行目は、列数が不正です。", i + 1));
                            continue;
                        }

                        //ShelterIdの必須チェック
                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoDaiBunrui.ShelterId, errList);

                        //ShelterIdの型チェック＆取得
                        var shelterId = FrameUtilites.ConvertToDecimalFromString(line, i, (int)CsvSagyoDaiBunrui.ShelterId, errList, true);

                        //ShelterIdCSVファイル重複チェック
                        if (idList.Where(x => x == shelterId.ToString()).Count() > 1)
                        {
                            errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値は、CSVファイル内に重複するShelterIdが存在します。", (int)CsvSagyoDaiBunrui.ShelterId + 1, i + 1));
                        }

                        var upList = sagyoDaiBunruiList.Where(x => x.ShelterId == shelterId);
                        //var addList = this._TokuisakiInfoList.Where(x => x.TokuisakiCode == tokuisakiCd);

                        //登録済データに存在する場合
                        if (upList.Count() > 0)
                        {
                            info = upList.First();
                        }

                        //登録済データに存在しない場合は新規
                        else
                        {
                            info = new SagyoDaiBunruiInfo();
                        }

                        info.ShelterId = shelterId;

                        //作業分類コード必須チェック
                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoDaiBunrui.SagyoDaiBunruiCode, errList);
                        var cd = FrameUtilites.ConvertToIntFromString(line, i, (int)CsvSagyoDaiBunrui.SagyoDaiBunruiCode, errList);

                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoDaiBunrui.SagyoDaiBunruiName, errList);

                        info.SagyoDaiBunruiCode = cd;
                        info.SagyoDaiBunruiName = FrameUtilites.SubstringShiftJIS(line[(int)CsvSagyoDaiBunrui.SagyoDaiBunruiName], 1, 70);

                        this._SagyoDaiBunruiInfoList.Add(info);

                    }
                    else if (this.rbtSagyoChuBunrui.Checked)
                    {
                        SagyoChuBunruiInfo info = null;

                        if (line.Count() != Enum.GetNames(typeof(CsvSagyoChuBunrui)).Length)
                        {
                            errList.Add(string.Format("CSVファイルの{0}行目は、列数が不正です。", i + 1));
                            continue;
                        }

                        //ShelterIdの必須チェック
                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoChuBunrui.ShelterId, errList);

                        //ShelterIdの型チェック＆取得
                        var shelterId = FrameUtilites.ConvertToDecimalFromString(line, i, (int)CsvSagyoChuBunrui.ShelterId, errList, true);

                        //ShelterIdCSVファイル重複チェック
                        if (idList.Where(x => x == shelterId.ToString()).Count() > 1)
                        {
                            errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値は、CSVファイル内に重複するShelterIdが存在します。", (int)CsvSagyoChuBunrui.ShelterId + 1, i + 1));
                        }

                        var upList = sagyoChuBunruiList.Where(x => x.ShelterId == shelterId);
                        //var addList = this._TokuisakiInfoList.Where(x => x.TokuisakiCode == tokuisakiCd);

                        //登録済データに存在する場合
                        if (upList.Count() > 0)
                        {
                            info = upList.First();
                        }

                        //登録済データに存在しない場合は新規
                        else
                        {
                            info = new SagyoChuBunruiInfo();
                        }

                        info.ShelterId = shelterId;

                        //作業分類コード必須チェック
                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoChuBunrui.SagyoChuBunruiCode, errList);
                        var cd = FrameUtilites.ConvertToIntFromString(line, i, (int)CsvSagyoChuBunrui.SagyoChuBunruiCode, errList);

                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoChuBunrui.SagyoChuBunruiName, errList);

                        var dInfo = dList.Where(x => x.ShelterId ==
                                            FrameUtilites.ConvertToIntFromString(line, i, (int)CsvSagyoChuBunrui.SagyoDaiBunruiId, errList));
                        if (dInfo != null && dInfo.Count() > 0)
                        {
                            info.SagyoDaiBunruiId = dInfo.First().SagyoDaiBunruiId;
                        }
                        else
                        {
                            info.SagyoDaiBunruiId = 0;
                        }

                        info.SagyoChuBunruiCode = cd;
                        info.SagyoChuBunruiName = FrameUtilites.SubstringShiftJIS(line[(int)CsvSagyoChuBunrui.SagyoChuBunruiName], 1, 70);

                        this._SagyoChuBunruiInfoList.Add(info);
                    }
                    else
                    {
                        SagyoShoBunruiInfo info = null;

                        if (line.Count() != Enum.GetNames(typeof(CsvSagyoShoBunrui)).Length)
                        {
                            errList.Add(string.Format("CSVファイルの{0}行目は、列数が不正です。", i + 1));
                            continue;
                        }

                        //ShelterIdの必須チェック
                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoShoBunrui.ShelterId, errList);

                        //ShelterIdの型チェック＆取得
                        var shelterId = FrameUtilites.ConvertToDecimalFromString(line, i, (int)CsvSagyoShoBunrui.ShelterId, errList, true);

                        //ShelterIdCSVファイル重複チェック
                        if (idList.Where(x => x == shelterId.ToString()).Count() > 1)
                        {
                            errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値は、CSVファイル内に重複するShelterIdが存在します。", (int)CsvSagyoChuBunrui.ShelterId + 1, i + 1));
                        }

                        var upList = sagyoShoBunruiList.Where(x => x.ShelterId == shelterId);
                        //var addList = this._TokuisakiInfoList.Where(x => x.TokuisakiCode == tokuisakiCd);

                        //登録済データに存在する場合
                        if (upList.Count() > 0)
                        {
                            info = upList.First();
                        }

                        //登録済データに存在しない場合は新規
                        else
                        {
                            info = new SagyoShoBunruiInfo();
                        }

                        info.ShelterId = shelterId;

                        //作業分類コード必須チェック
                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoShoBunrui.SagyoShoBunruiCode, errList);
                        var cd = FrameUtilites.ConvertToIntFromString(line, i, (int)CsvSagyoShoBunrui.SagyoShoBunruiCode, errList);

                        FrameUtilites.isNullCheckCsvColumn(line, i, (int)CsvSagyoShoBunrui.SagyoShoBunruiName, errList);

                        var dInfo = dList.Where(x => x.ShelterId ==
                                            FrameUtilites.ConvertToIntFromString(line, i, (int)CsvSagyoShoBunrui.SagyoDaiBunruiId, errList));
                        if (dInfo != null && dInfo.Count() > 0)
                        {
                            info.SagyoDaiBunruiId = dInfo.First().SagyoDaiBunruiId;
                        }
                        else
                        {
                            info.SagyoDaiBunruiId = 0;
                        }

                        var cInfo = cList.Where(x => x.ShelterId ==
                                            FrameUtilites.ConvertToIntFromString(line, i, (int)CsvSagyoShoBunrui.SagyoChuBunruiId, errList));
                        if (cInfo != null && cInfo.Count() > 0)
                        {
                            info.SagyoChuBunruiId = cInfo.First().SagyoChuBunruiId;
                        }
                        else
                        {
                            info.SagyoChuBunruiId = 0;
                        }

                        info.SagyoShoBunruiCode = cd;
                        info.SagyoShoBunruiName = FrameUtilites.SubstringShiftJIS(line[(int)CsvSagyoShoBunrui.SagyoShoBunruiName], 1, 70);

                        this._SagyoShoBunruiInfoList.Add(info);

                    }


                    
                }
                catch (CanRetryException err)
                {

                    MessageBox.Show(
                        err.Message,
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    err.Msgicon);

                    return false;
                }
            }

            //エラーが1件以上ある場合はエラーファイル出力。
            if (errList.Count() > 0)
            {
                try
                {
                    //ファイルパス
                    string path = Path.GetDirectoryName(this.edtImportFilePath.Text);

                    if (Directory.Exists(path))
                    {
                        try
                        {
                            string fileName = "作業分類情報エラーファイル_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

                            FrameUtilites.LinesToErrText(errList, path + "\\" + fileName);

                            //処理完了メッセージ
                            MessageBox.Show(
                                "CSVファイルの取込に失敗したため、エラーファイルを出力しました。"
                                 + Environment.NewLine
                                 + "ファイル名：" + fileName,
                                this.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (IOException err)
                        {
                            //IOExceptionが発生したらメッセージを表示。そうでない場合はthrowする
                            MessageBox.Show(
                                FrameUtilites.GetDefineMessage("ME2302008", new string[] { "エラーファイル" }) +
                                    "\r\n" + err.Message,
                                this.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                                );
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                    }
                    else
                    {
                        //フォルダが無い場合のメッセージ
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201007", new string[] { "指定した", "フォルダ" }),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 画面情報からデータを更新します。
        /// </summary>
        private void DoUpdate()
        {
            //実行確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                FrameUtilites.GetDefineMessage("MQ2102007"),
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (this.ValidateChildren(ValidationConstraints.None))
                {
                    try
                    {
                        bool shoriFlg = true;

                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            //CSVデータ取得
                            shoriFlg = this.DoImportCsvFile(tx);
                            if (!shoriFlg) return;

                            // 登録・更新
                            if (this.rbtSagyoDaiBunrui.Checked)
                            {
                                this._SagyoDaiBunrui.BulkSave(tx, this._SagyoDaiBunruiInfoList);
                            }
                            else if (this.rbtSagyoChuBunrui.Checked)
                            {
                                this._SagyoChuBunrui.BulkSave(tx, this._SagyoChuBunruiInfoList);
                            }
                            else
                            {
                                this._SagyoShoBunrui.BulkSave(tx, this._SagyoShoBunruiInfoList);
                            }
                        });

                        //更新がない場合は終了
                        if (!shoriFlg) return;

                        //操作ログ(保存)の条件取得
                        //string log_jyoken = FrameUtilites.GetDefineLogMessage(
                        //    "C10002", new string[] { "契約登録", this.edtKeiyakuCode.Text });

                        //保存完了メッセージ格納用
                        string msg = string.Empty;

                        //操作ログ出力
                        //FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        //    FrameLogWriter.LoggingOperateKind.UpdateItem,
                        //        log_jyoken);

                        //更新完了のメッセージ
                        msg =
                            FrameUtilites.GetDefineMessage("MI2001003");

                        //登録完了メッセージ
                        MessageBox.Show(
                            msg,
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        ////初期状態へ移行
                        if (FrameEditMode.New == this.currentMode)
                        {
                            //データ表示
                            this.DoClear(false);
                        }
                        else if (FrameEditMode.Editable == this.currentMode)
                        {
                            if (this.KeiyakuIchiran != null)
                            {
                                //呼び出し元のPGの再表示を指示
                                this.KeiyakuIchiran.ReSetScreen(this.SelectDtlId);
                            }

                            // 画面を閉じる
                            this.isConfirmClose = false;
                            this.DoClose();
                        }

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
                        this.btnSelectFile.Focus();
                    }
                }
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            if (this.isReDraw)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool CheckInputs()
        {

            //戻り値用
            bool rt_val = true;

            string msg = string.Empty;
            MessageBoxIcon icon = MessageBoxIcon.None;
            Control ctl = null;

            //コードの必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtImportFilePath.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtImportFilePath;
            }

            if (!rt_val)
            {
                //アイコンの種類によってMassageBoxTitleを変更
                string msg_title = string.Empty;

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        msg_title = "エラー";
                        break;
                    case MessageBoxIcon.Warning:
                        msg_title = "警告";
                        break;
                    default:
                        break;
                }

                MessageBox.Show(
                    msg, msg_title, MessageBoxButtons.OK, icon);

                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示

            if (this.isConfirmClose)
            {
                DialogResult result =
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MQ2102001"),
                        this.Text,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    //Yesの場合は閉じる
                    e.Cancel = false;
                    if (this.KeiyakuIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.KeiyakuIchiran.ReSetScreen(this.SelectDtlId);
                    }
                }
                else if (result == DialogResult.No)
                {
                    //Noの場合は終了をキャンセル
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 画面情報をクリアして初期状態に変更します。
        /// </summary>
        /// <param name="showCancelConfirm">確認画面を表示するかどうか(true:表示する)</param>
        private void DoClear(bool showCancelConfirm)
        {
            if (showCancelConfirm)
            {
                //取消確認の実行確認ダイアログ
                DialogResult d_result =
                    MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2102008"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                //Noだったら抜ける
                if (d_result == DialogResult.No)
                {
                    return;
                }
            }

            //入力項目をクリア
            this.ClearInputs();

            if (FrameEditMode.New == this.currentMode)
            {
                this.ChangeMode(FrameEditMode.New);
            }
            else if (FrameEditMode.Editable == this.currentMode)
            {
                this.ChangeMode(FrameEditMode.Editable);
            }
        }


        /// <summary>
        /// 画面モードを表す列挙体を指定して、現在の画面の
        /// 表示モードを切り替えます。
        /// </summary>
        /// <param name="mode">変更したい画面モード</param>
        private void ChangeMode(FrameEditMode mode)
        {
            switch (mode)
            {
                case FrameEditMode.Default:
                    //初期状態
                    // コントロールの使用可否
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.btnSave.Enabled = false;
                    // ファンクションの使用可否
                    _commandSet.ImportCsv.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:

                    //編集モード
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.btnSave.Enabled = true;
                    // ファンクションの使用可否
                    _commandSet.ImportCsv.Enabled = true;
                    _commandSet.Close.Enabled = true;

                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            this.currentMode = mode;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            //取込ファイルの選択
            this.SelectInportFilePath();
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
    }
}
