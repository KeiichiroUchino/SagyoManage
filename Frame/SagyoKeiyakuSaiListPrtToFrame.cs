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
using Jpsys.SagyoManage.ReportModel;
using System.Threading;
using Jpsys.SagyoManage.ReportDAL;
using System.Collections;
using Jpsys.SagyoManage.Frame.EventData;
using Jpsys.SagyoManage.ReportFrame;

namespace Jpsys.SagyoManage.Frame
{
    public partial class SagyoKeiyakuSaiListPrtToFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoKeiyakuSaiListPrtToFrame()
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
        private const string WINDOW_TITLE = "作業契約差異リスト";

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
        /// 印刷指示にて指示された印刷の出力先を保持する領域
        /// </summary>
        private ReportPrintDestType currentPrintDest;

        /// <summary>
        /// 印字用データ一覧を保持する領域
        /// </summary>
        private IList<GekkanSagyoKeikakuhyouRptInfo> _RptList;

        /// <summary>
        /// 前回抽出情報を保持する領域
        /// </summary>
        private GekkanSagyoKeikakuhyouRptInfoSearchParameter _LastTimeArgInfo;

        /// <summary>
        /// 抽出情報を保持する領域
        /// </summary>
        private GekkanSagyoKeikakuhyouRptInfoSearchParameter _ArgInfo;

        /// <summary>
        /// 帳票印字に必要なデータを取得・作成するスレッド
        /// </summary>
        private Thread reportDataCreateThread = null;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

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
        /// 作業計画書（月単位）クラス
        /// </summary>
        private SagyoKeiyakuSaiListRpt _SagyoKeiyakuSaiListRpt;

        //--デリゲート（疑似コールバック用）
        /// <summary>
        /// 帳票印刷データの作成終了イベントを処理するコールバック用のデリゲート
        /// </summary>
        /// <param name="reportDataArray"></param>
        /// <param name="printDestType"></param>
        private delegate void ReportDataCreatedCallBack(IList[] reportDataArray, ReportPrintDestType printDestType);

        #endregion

        #region イベントデリゲート

        /// <summary>
        /// 帳票印刷データ作成終了時のイベントデリゲートです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ReportDataCreatedEventHandler(object sender,
            ReportDataCreatedEventArgs e);

        #endregion

        #region 基本イベントメンバ

        /// <summary>
        /// 帳票印刷データ作成終了時のイベントメンバーです。
        /// </summary>
        public event ReportDataCreatedEventHandler ReportDataCreated;

        #endregion

        #region CSV列定義

        private enum CsvIndex : int
        {
            ShelterId,
            KeiyakuCode,
            SagyoBashoCode,
            KeiyakuName,
            SagyoDaiBunruiCode,
            SagyoChuBunruiCode,
            SagyoShoBunruiCode,
            KeiyakuStartYMD,
            KeiyakuEndYMD,
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

            //画面コントローラのインスタンスを作成
            this._SagyoKeiyakuSaiListRpt = new SagyoKeiyakuSaiListRpt();

            //画面コントローラから帳票データ取得終了イベントのイベントハンドラを作る
            this.ReportDataCreated +=
                new ReportDataCreatedEventHandler(_ReportDataCreated);

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

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

            //印刷指示画面ファイルメニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.DelPrintSetting);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.PrintToPrinter);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.PrintToScreen);
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
            this.dteShiteiYM.Value = DateTime.Today.Date;
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

            //***印刷
            _commandSet.PrintToPrinter.Execute += PrintToPrinter_Execute;
            _commandSet.Bind(
                _commandSet.PrintToPrinter, this.btnPrintToPrinter, actionMenuItems.GetMenuItemBy(ActionMenuItems.PrintToPrinter), this.toolStripPrintToPrinter);

            //***プレビュー
            _commandSet.PrintToScreen.Execute += PrintToScreen_Execute;
            _commandSet.Bind(
                _commandSet.PrintToScreen, this.btnPrintToScreen, actionMenuItems.GetMenuItemBy(ActionMenuItems.PrintToScreen), this.toolStripPrintToScreen);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
        }

        void PrintToPrinter_Execute(object sender, EventArgs e)
        {
            //印刷ボタンクリック
            this.DoPrintToPrinter();
        }

        void PrintToScreen_Execute(object sender, EventArgs e)
        {
            //プレビューボタンクリック
            this.DoPrintToScreen();
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

        public void GetJyutyuCheckList()
        {
            //スレッドを定義
            this.reportDataCreateThread =
                new Thread(new ThreadStart(this.StartReportCreate));

            //スレッドの名前を定義
            this.reportDataCreateThread.Name = "StartReportCreate";

            //スレッドを開始
            this.reportDataCreateThread.Start();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 帳票印刷データを取得してプリンタに印刷する処理を開始します。
        /// </summary>
        private void DoPrintToPrinter()
        {
            //入力チェック
            if (this.ValidateChildren() && CheckInputs())
            {
                //印刷先を保持
                this.currentPrintDest = ReportPrintDestType.PrintToPrinter;
                //画面を使用不可に
                this.ControlEnableChangeForPrinting(false);
                //画面タイトルを変更する
                this.Text = WINDOW_TITLE + "　" + "印刷中...";
                //メンバをクリアする
                this.ReportListClear();
                //画面からコントロールへデータの取得
                this.GetScreen();
                //画面コントローラのメソッドを呼び出す。
                this.GetJyutyuCheckList();
            }
        }

        /// <summary>
        /// 帳票印刷データを取得して画面にプレビューする処理を開始します。
        /// </summary>
        private void DoPrintToScreen()
        {
            //入力チェック
            if (this.ValidateChildren() && CheckInputs())
            {
                //印刷先を保持
                this.currentPrintDest = ReportPrintDestType.PrintToScreen;
                //画面を使用不可に
                this.ControlEnableChangeForPrinting(false);
                //画面タイトルを変更する
                this.Text = WINDOW_TITLE + "　" + "印刷中...";
                //メンバをクリアする
                this.ReportListClear();
                //画面からデータの取得
                this.GetScreen();
                //画面コントローラのメソッドを呼び出す。
                this.GetJyutyuCheckList();
            }
        }

        /// <summary>
        /// 画面入力項目から画面クラスに必要な値を設定します。
        /// </summary>
        private void GetScreen()
        {
            //コントローラーにセット
            GekkanSagyoKeikakuhyouRptInfoSearchParameter arginfo =
                new GekkanSagyoKeikakuhyouRptInfoSearchParameter();

            // 作業日(開始)
            if (this.dteShiteiYM.Value != null)
            {
                arginfo.SagyoYM = this.dteShiteiYM.Value.Value;
            }
            else
            {
                arginfo.SagyoYM = DateTime.MinValue;
            }

            //抽出情報を保持
            this._ArgInfo = arginfo;

            //コントローラーにセット
            GekkanSagyoKeikakuhyouRptInfoSearchParameter lastArginfo =
                new GekkanSagyoKeikakuhyouRptInfoSearchParameter();

            // 作業日(開始)
            if (this.dteShiteiYM.Value != null)
            {
                lastArginfo.SagyoYM = this.dteShiteiYM.Value.Value.AddYears(-1);
            }
            else
            {
                lastArginfo.SagyoYM = DateTime.MinValue;
            }

            //受注チェックリスト抽出情報を保持
            this._LastTimeArgInfo = lastArginfo;
        }

        /// <summary>
        /// 印刷中の処理においてコントロールの有効無効を切り替えます。
        /// 印刷のための処理中に不用意にコントロールが操作されることを
        /// 防ぐためです。
        /// </summary>
        /// <param name="val">コントロールの有効・無効(true:有効）</param>
        private void ControlEnableChangeForPrinting(bool val)
        {
            this.menuStripTop.Enabled = val;
            this.pnl.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.DelPrintSetting, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Close, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.PrintToScreen, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.PrintToPrinter, val);
        }

        /// <summary>
        /// メンバをクリアします。
        /// </summary>
        public void ReportListClear()
        {
            if (this._RptList != null)
            {
                this._RptList.Clear();
            }
        }

        private void StartReportCreate()
        {
            lock (this)
            {
                //メンバをクリア
                this.ReportListClear();

                IList<GekkanSagyoKeikakuhyouRptInfo> comparisonList = new List<GekkanSagyoKeikakuhyouRptInfo>();
                comparisonList = this._SagyoKeiyakuSaiListRpt.GetList(this._LastTimeArgInfo);

                IList<GekkanSagyoKeikakuhyouRptInfo> nowList = new List<GekkanSagyoKeikakuhyouRptInfo>();
                nowList = this._SagyoKeiyakuSaiListRpt.GetList(this._ArgInfo);

                this._RptList = new List<GekkanSagyoKeikakuhyouRptInfo>();

                //全件出力の場合
                if (this.chkAllFlag.Checked)
                {
                    List<GekkanSagyoKeikakuhyouRptInfo> list = new List<GekkanSagyoKeikakuhyouRptInfo>();

                    list.AddRange(comparisonList);
                    list.AddRange(nowList);

                    this._RptList = list;
                }

                //差異分のみの場合
                else 
                {
                    foreach (var comItem in comparisonList)
                    {
                        int i = nowList.Where(x => x.SagyoBashoId == comItem.SagyoBashoId).Count();
                        if (i == 0)
                        {
                            this._RptList.Add(comItem);
                        }
                    }
                }

                //イベントデータ作成用のレポートデータの配列を作る
                IList[] wk_list = { (IList)this._RptList };

                //イベントデータのインスタンス作成
                ReportDataCreatedEventArgs eArgs =
                    new ReportDataCreatedEventArgs(wk_list);

                //イベントデータ発生関数を呼ぶ
                this.OnReportDataCreated(eArgs);
            }
        }

        /// <summary>
        /// 帳票印刷データ作成終了時のイベント発生関数です。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnReportDataCreated(ReportDataCreatedEventArgs e)
        {
            if (this.ReportDataCreated != null)
                this.ReportDataCreated(this, e);
        }

        private void _ReportDataCreated(object sender, ReportDataCreatedEventArgs e)
        {
            //帳票印刷データの取得終了
            //疑似コールバックとして、印刷を実行
            ReportDataCreatedCallBack d = null;
            if (0 < this._RptList.Count)
            {
                d += DoPrintReport;
            }
            else
            {
                d += DoPFrameReset;
            }
            this.Invoke(d, new object[] { e.ReportDataListArray, this.currentPrintDest });
        }

        /// <summary>
        /// 帳票データの配列と印刷先を表す列挙体を指定して、指定された印刷先に
        /// 応じて印字処理を続行します。
        /// </summary>
        /// <param name="reportDataArray">帳票データの配列</param>
        /// <param name="printDestType">印刷先を表す列挙体</param>
        private void DoPrintReport(IList[] reportDataArray, ReportPrintDestType printDestType)
        {
            try
            {
                //レポートの必要に応じて、レポートデータの配列からデータを抜き出す。
                IList wk_list = reportDataArray[0];

                //レポートオブジェクト作成
                SagyoKeiyakuSaiListToRpt rpt = new SagyoKeiyakuSaiListToRpt();
                //レポートをロード
                rpt.Load();
                ////レポートに印刷データをセット
                rpt.SetDataSource(wk_list);

                ////経理部長検印欄フラグの設定
                //bool flgKeiriPrint = setKeiriKeninranPrint(rpt);

                #region データをセットした後でクリレポのテキストに直接セットします。

                ////検印欄
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol1"]).Text = this.getKeninranDisp(this.radAri1.Checked);
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol2"]).Text = this.getKeninranDisp(this.radAri2.Checked);
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol3"]).Text = this.getKeninranDisp(this.radAri3.Checked);
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol4"]).Text = this.getKeninranDisp(this.radAri4.Checked);
                //if (!flgKeiriPrint)
                //{
                //    ((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol5"]).Text = this.getKeninranDisp(this.radAri5.Checked);
                //}
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol6"]).Text = this.getKeninranDisp(this.radAri6.Checked);
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninCol7"]).Text = this.getKeninranDisp(this.radAri7.Checked);

                ////検印タイトル
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName1"]).Text = this._SystemPropertyInfo.StampTitle1;
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName2"]).Text = this._SystemPropertyInfo.StampTitle2;
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName3"]).Text = this._SystemPropertyInfo.StampTitle3;
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName4"]).Text = this._SystemPropertyInfo.StampTitle4;
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName5"]).Text = this._SystemPropertyInfo.StampTitle5;
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName6"]).Text = this._SystemPropertyInfo.StampTitle6;
                //((TextObject)rpt.ReportDefinition.ReportObjects["txtKeninName7"]).Text = this._SystemPropertyInfo.StampTitle7;

                #endregion

                //印刷コントローラのインスタンスを作成
                ReportPrintingControl reportControl = new ReportPrintingControl(this, rpt, printDestType);

                //印刷コントローラに印刷を指示
                reportControl.RunPrintProcess();

                //操作ログ(保存)の条件取得
                string log_jyoken = this.Createlog();

                //操作ログ出力
                switch (printDestType)
                {
                    case ReportPrintDestType.PrintToScreen:
                        //プレビュー時
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.PrintPreview,
                            log_jyoken);
                        break;
                    case ReportPrintDestType.PrintToPrinter:
                        //直接印刷時時
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.PrintPrinting,
                            log_jyoken);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //画面を使用可能に
                this.ControlEnableChangeForPrinting(true);
                //画面タイトルを変更する
                this.Text = WINDOW_TITLE;

                //フォーカスを移動します。
                this.SetFirstFocus();
            }
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        public string Createlog()
        {
            string txtlog = string.Empty;

            //操作ログ(保存)の条件取得
            string log_jyoken = "ログ確認";

            return log_jyoken;
        }

        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        public void SetFirstFocus()
        {
            this.dteShiteiYM.Focus();
        }

        /// <summary>
        /// 帳票データ０件時の処理
        /// 画面を使用可能にする。
        /// </summary>
        /// <param name="reportDataArray">帳票データの配列</param>
        /// <param name="printDestType">印刷先を表す列挙体</param>
        private void DoPFrameReset(IList[] reportDataArray, ReportPrintDestType printDestType)
        {
            //明細が無い場合メッセージを表示
            MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2201015"),
                    WINDOW_TITLE,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            //画面を使用可能に
            this.ControlEnableChangeForPrinting(true);
            //画面タイトルを変更する
            this.Text = WINDOW_TITLE;

            //条件項目にフォーカス
            SetFirstFocus();
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

            if (!rt_val)
            {
                //アイコンの種類によってMassageBoxTitleを変更
                string msg_title = string.Empty;

                //指定月の必須チェック
                if (rt_val && this.dteShiteiYM.Value == null)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                            "MW2203004", new string[] { "指定月" });
                    icon = MessageBoxIcon.Warning;
                    ctl = this.dteShiteiYM;
                }

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
                }
                else if (result == DialogResult.No)
                {
                    //Noの場合は終了をキャンセル
                    e.Cancel = true;
                }
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
                    // ファンクションの使用可否
                    _commandSet.PrintToPrinter.Enabled = true;
                    _commandSet.PrintToScreen.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:

                    //編集モード
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    // ファンクションの使用可否
                    _commandSet.PrintToPrinter.Enabled = true;
                    _commandSet.PrintToScreen.Enabled = true;

                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            this.currentMode = mode;
        }

        private void ymd_Down(object sender, EventArgs e)
        {
            this.AddDayForNskDateTime(this.ActiveControl, -1);
        }

        private void ymd_Up(object sender, EventArgs e)
        {
            this.AddDayForNskDateTime(this.ActiveControl, 1);
        }

        private void AddDayForNskDateTime(Control c, int days)
        {
            if (c.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)c).Value = Convert.ToDateTime(((NskDateTime)c).Value).AddMonths(days);
                }
                catch
                {
                    ;
                }
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
    }
}
