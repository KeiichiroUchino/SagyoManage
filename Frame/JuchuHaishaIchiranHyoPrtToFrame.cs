using AdvanceSoftware.VBReport8;
using jp.co.jpsys.util;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.ReportModel;
using Jpsys.HaishaManageV10.VBReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class JuchuHaishaIchiranHyoPrtToFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "受注配車一覧表";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// 印刷条件を保持する領域
        /// </summary>
        private JuchuHaishaIchiranHyoSearchParameter _JuchuHaishaIchiranHyoSearchParameter = null;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 受注配車一覧表クラス
        /// </summary>
        private ReportDAL.JuchuHaishaIchiranHyoRpt _dal;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを
        /// 利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList =
            UserProperty.GetInstance().SystemNameList;

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

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
        public JuchuHaishaIchiranHyoPrtToFrame()
        {
            InitializeComponent();
        }

        #endregion
        
        #region 初期化処理

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitShanaiTankaMisetteiListFrame()
        {
            // 画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //画面配色の初期化
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

            // 受注配車一覧表クラスインスタンス作成
            this._dal = new ReportDAL.JuchuHaishaIchiranHyoRpt(this.appAuth);

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

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するための
            // ファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numCarCd, this.ShowCmnSearchCar},
                {this.numCarKindCd, this.ShowCmnSearchCarKind},
                {this.numDriverCd, this.ShowCmnSearchDriver},
                {this.numCarOfChartererCd, this.ShowCmnSearchTorihikisaki},
                {this.numTokuisakiCd, this.ShowCmnSearchTokuisaki},
                {this.numClmClassCd, this.ShowCmnSearchClmClass},
                {this.numContractCd, this.ShowCmnSearchContract},
                {this.numHanroCd, this.ShowCmnSearchHanro},
                {this.numJuchuTantoCd, this.ShowCmnSearchJuchuTantosha},
            };

            //コンボボックスの初期化
            this.InitCombo();

            // 入力項目のクリア
            this.ClearInputs();

            //条件項目にフォーカス
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

            this.actionMenuItems.SetCreatingItem(ActionMenuItems.PrintToScreen);
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

            //***プレビュー
            commandSet.PrintToScreen.Execute += PrintToScreenCommand_Execute;
            commandSet.Bind(commandSet.PrintToScreen,
               this.btnPreview, actionMenuItems.GetMenuItemBy(ActionMenuItems.PrintToScreen), this.toolStripPreview);

            //***終了
            commandSet.Close.Execute += CloseCommand_Execute;
            commandSet.Bind(commandSet.Close,
               this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        private void PrintToScreenCommand_Execute(object sender, EventArgs e)
        {
            // プレビュー
            this.DoPreview();
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
                this.numCarCd, this.numCarKindCd, this.numDriverCd, this.numCarOfChartererCd, this.numTokuisakiCd, this.numJuchuTantoCd, this.numClmClassCd, this.numContractCd, this.numHanroCd);

            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.cmbBranchOffice, ctl => ctl.Text, this.cmbBranchOffice_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarCd, ctl => ctl.Text, this.numCarCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCd, ctl => ctl.Text, this.numCarKindCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numDriverCd, ctl => ctl.Text, this.numDriverCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarOfChartererCd, ctl => ctl.Text, this.numCarOfChartererCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTokuisakiCd, ctl => ctl.Text, this.numTokuisakiCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numClmClassCd, ctl => ctl.Text, this.numClmClassCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numContractCd, ctl => ctl.Text, this.numContractCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numHanroCd, ctl => ctl.Text, this.numHanroCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numJuchuTantoCd, ctl => ctl.Text, this.numJuchuTantoCd_Validating));
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.cmbFilterDateKbns.SelectedIndex = 0;
            DateTime today = DateTime.Today;
            this.dteTaishoYMDFrom.Value = new DateTime(today.Year, today.Month, 1);
            this.dteTaishoYMDTo.Value = NSKUtil.MonthLastDay(today);
            this.cmbBranchOffice.SelectedValue = null;
            this.cmbBranchOffice.Tag = null;
            this.numTokuisakiCd.Tag = null;
            this.numTokuisakiCd.Value = null;
            this.edtTokuisakiNm.Text = string.Empty;
            this.numCarCd.Value = 0;
            this.numCarCd.Tag = null;
            this.edtLicPlateCarNo.Text = string.Empty;
            this.edtCarKindNM.Text = string.Empty;
            this.numCarKindCd.Value = 0;
            this.numCarKindCd.Tag = null;
            this.edtCarKindName.Text = string.Empty;
            this.numDriverCd.Value = 0;
            this.numDriverCd.Tag = null;
            this.edtDriverNM.Text = string.Empty;
            this.numCarOfChartererCd.Value = 0;
            this.numCarOfChartererCd.Tag = null;
            this.edtCarOfChartererNm.Text = string.Empty;
            this.numTokuisakiCd.Value = 0;
            this.numTokuisakiCd.Tag = null;
            this.edtTokuisakiNm.Text = string.Empty;
            this.numClmClassCd.Value = 0;
            this.numClmClassCd.Tag = null;
            this.edtClmClassNm.Text = string.Empty;
            this.numContractCd.Value = 0;
            this.numContractCd.Tag = null;
            this.edtContractNm.Text = string.Empty;
            this.numHanroCd.Value = 0;
            this.numHanroCd.Tag = null;
            this.edtHanroNm.Text = string.Empty;
            this.numJuchuTantoCd.Value = 0;
            this.numJuchuTantoCd.Tag = null;
            this.edtJuchuTantoNm.Text = string.Empty;
            this.numJuchuSlipNoFrom.Value = 0;
            this.numJuchuSlipNoTo.Value = 0;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitNyukinMeisaiKbnCombo();
            this.InitBranchOfficeCombo();
        }

        /// <summary>
        /// 日付指定区分コンボボックスを初期化します。
        /// </summary>
        private void InitNyukinMeisaiKbnCombo()
        {
            // 日付指定区分コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<SystemNameInfo> list = this._DalUtil.SystemGlobalName.GetList((Int32)DefaultProperty.SystemNameKbn.FilterDateKbns);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.SystemNameCode)
                    .ToList();
            }
            else
            {
                list = new List<SystemNameInfo>();
            }

            String key = "";
            String value = "";

            foreach (SystemNameInfo item in list)
            {
                key = item.SystemNameCode.ToString();
                value = item.SystemNameCode.ToString() + " " + item.SystemName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbFilterDateKbns, datasource, false, null, false);
            this.cmbFilterDateKbns.SelectedIndex = 0;
        }

        /// <summary>
        /// 営業所コンボボックスを初期化します。
        /// </summary>
        private void InitBranchOfficeCombo()
        {
            // 営業所コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<ToraDONBranchOfficeInfo> list = this._DalUtil.ToraDONBranchOffice.GetList(null);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.BranchOfficeCode)
                    .ToList();
            }
            else
            {
                list = new List<ToraDONBranchOfficeInfo>();
            }

            String key = "";
            String value = "";

            foreach (ToraDONBranchOfficeInfo item in list)
            {
                key = item.BranchOfficeCode.ToString();
                value = item.BranchOfficeCode.ToString() + " " + item.BranchOfficeShortName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbBranchOffice, datasource, true, DefaultProperty.CMB_BRANCHOFFICE_ALL_NAME, false);
        }

        #endregion

        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            this.dteTaishoYMDFrom.Focus();
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// 指定した印刷条件で印刷プレビューを表示します。
        /// </summary>
        /// <returns>処理結果</returns>
        private void DoPreview()
        {
            this.Print(ReportPrintDestType.PrintToScreen);
        }

        /// <summary>
        /// 詳細の入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputs()
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            //日付（範囲開始）の必須入力チェック
            if (rt_val && DateTime.MinValue == Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "日付（範囲開始）" });
                ctl = this.dteTaishoYMDFrom;
            }

            //日付（範囲終了）の必須入力チェック
            if (rt_val && DateTime.MinValue == Convert.ToDateTime(this.dteTaishoYMDTo.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "日付（範囲終了）" });
                ctl = this.dteTaishoYMDTo;
            }

            //日付の範囲チェック
            if (rt_val && Convert.ToDateTime(this.dteTaishoYMDTo.Value) < Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2202018", new string[] { "日付" });
                ctl = this.dteTaishoYMDTo;
            }


            //伝票№の範囲チェック
            decimal JuchuSlipNoFrom = Convert.ToDecimal(this.numJuchuSlipNoFrom.Value);
            decimal JuchuSlipNoTo = Convert.ToDecimal(this.numJuchuSlipNoTo.Value);

            //どちらも入力されている場合にのみチェックを行う
            if ((0 != JuchuSlipNoFrom) && (0 != JuchuSlipNoTo))
            {
                if (rt_val && Convert.ToDecimal(this.numJuchuSlipNoTo.Value) < Convert.ToDecimal(this.numJuchuSlipNoFrom.Value))
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "伝票№" });
                    ctl = this.numJuchuSlipNoTo;
                }
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// 画面から更新に必要な情報をメンバにセットします。
        /// </summary>
        private JuchuHaishaIchiranHyoSearchParameter GetFromScreen()
        {
            JuchuHaishaIchiranHyoSearchParameter para = new JuchuHaishaIchiranHyoSearchParameter();

            // 日付指定区分
            para.FilterDateKbns = Convert.ToInt32(this.cmbFilterDateKbns.SelectedValue);
            // 対象日付（From）
            para.TaishoYMDFrom = Convert.ToDateTime(this.dteTaishoYMDFrom.Value);
            // 対象日付（To）
            para.TaishoYMDTo = new DateTime(
                this.dteTaishoYMDTo.Value.Value.Year
                , this.dteTaishoYMDTo.Value.Value.Month
                , this.dteTaishoYMDTo.Value.Value.Day
                , 23
                , 59
                , 59);
            // 営業所ID
            para.BranchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag);
            // 車両ID
            para.CarId = Convert.ToDecimal(this.numCarCd.Tag);
            // 車種ID
            para.CarKindId = Convert.ToDecimal(this.numCarKindCd.Tag);
            // 乗務員ID
            para.DriverId = Convert.ToDecimal(this.numDriverCd.Tag);
            // 傭車先ID
            para.CarOfChartererId = Convert.ToDecimal(this.numCarOfChartererCd.Tag);
            // 得意先ID
            para.TokuisakiId = Convert.ToDecimal(this.numTokuisakiCd.Tag);
            // 請求部門ID
            para.ClmClassId = Convert.ToDecimal(this.numClmClassCd.Tag);
            // 請負ID
            para.ContractId = Convert.ToDecimal(this.numContractCd.Tag);
            // 販路ID
            para.HanroId = Convert.ToDecimal(this.numHanroCd.Tag);
            // 受注担当ID
            para.JuchuTantoId = Convert.ToDecimal(this.numJuchuTantoCd.Tag);
            // 伝票№（From）
            para.JuchuSlipNoFrom = Convert.ToDecimal(this.numJuchuSlipNoFrom.Value);
            // 伝票№（To）
            para.JuchuSlipNoTo = Convert.ToDecimal(this.numJuchuSlipNoTo.Value);
            // トラDONバージョン区分
            para.TraDonVersionKbn = UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn;
            // 印刷条件
            para.PrintJokenString = this.GetPrintJokenString(" ");

            return para;
        }

        #endregion

        #region 印刷関係

        /// <summary>
        /// 印刷中の処理においてコントロールの有効無効を切り替えます。
        /// 印刷のための処理中に不用意にコントロールが操作されることを
        /// 防ぐためです。
        /// </summary>
        /// <param name="val">コントロールの有効・無効(true:有効）</param>
        private void ControlEnabledForPrinting(bool val)
        {
            this.menuStripTop.Enabled = val;
            this.pnlMain.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.PrintToScreen, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Close, val);
        }

        /// <summary>
        /// 印刷先を指定して印刷を実行します。
        /// </summary>
        /// <param name="printDestType"></param>
        private void Print(ReportPrintDestType printDestType)
        {
            if (!this.ValidateChildren(ValidationConstraints.None))
                return;

            if (!this.CheckInputs())
                return;

            //画面を使用不可に
            this.ControlEnabledForPrinting(false);
            //画面タイトルを変更する
            this.Text = WINDOW_TITLE + "　" + "印刷中...";
            //画面からコントロールへデータの取得
            this._JuchuHaishaIchiranHyoSearchParameter = this.GetFromScreen();

            this.CreateReportDataAsync(this._JuchuHaishaIchiranHyoSearchParameter).ContinueWith(t =>
            {
                var reportData = t.Result;

                //ハンドルが関連付けられていない場合、処理しない
                if (!this.IsHandleCreated)
                    return;

                try
                {
                    //データがない場合はエラーメッセージを表示
                    if (reportData.Count == 0)
                    {
                        //画面タイトルを変更する
                        this.Text = WINDOW_TITLE;

                        //明細が無い場合メッセージを表示
                        MessageBox.Show(
                                FrameUtilites.GetDefineMessage("MW2201015"),
                                this.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                        return;
                    }

                    #region 印刷処理

                    // 出力先パス生成
                    string fileName = Path.GetTempPath() + WINDOW_TITLE + "_"
                         + DateTime.Now.ToString("yyyyMMddHHmmssttt") + ".xlsx";

                    // 砂時計
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        //Excel作成
                        this.CreateJuchuHaishaIchiranHyo(ReportPrintDestType.PrintToScreen, fileName,
                            reportData, this._JuchuHaishaIchiranHyoSearchParameter);
                    }
                    finally
                    {
                        // デフォルトに戻す
                        Cursor.Current = Cursors.Default;
                    }

                    //操作ログ(保存)の条件取得
                    string log_jyoken = this.GetPrintJokenString("\r\n");

                    //操作ログ出力
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.PrintPreview,
                        log_jyoken);

                    #endregion
                }
                catch (Exception ex)
                {
                    //例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
                finally
                {
                    //画面を使用可能に
                    this.ControlEnabledForPrinting(true);
                    //画面タイトルをもとに戻す
                    this.Text = WINDOW_TITLE;
                    //条件項目にフォーカス
                    this.SetFirstFocus();
                }
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// レポートデータを非同期で取得します。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private Task<List<JuchuHaishaIchiranHyoRptInfo>> CreateReportDataAsync(JuchuHaishaIchiranHyoSearchParameter para)
        {
            return
                Task.Factory.StartNew(() =>
                {
                    return this._dal.GetReportData(para).ToList();
                });
        }

        /// <summary>
        /// 受注配車一覧表（Excel）を作成します。
        /// </summary>
        /// <param name="printDestType"></param>
        /// <param name="fileName">作成するファイル名（パス付き）</param>
        /// <param name="reportData">出力情報</param>
        /// <param name="param">印刷条件</param>
        /// <returns></returns>
        private bool CreateJuchuHaishaIchiranHyo(ReportPrintDestType printDestType, string fileName,
            IList<JuchuHaishaIchiranHyoRptInfo> reportData, JuchuHaishaIchiranHyoSearchParameter param)
        {
            CellReport vbrList = new CellReport();

            try
            {
                //【1】===========================================================//
                // 帳票作成に必要な開始処理
                //================================================================//
                // 計算式の再計算を行なう設定
                // 詳細は[2-VB-Reportの機能]-[3-データ設定/操作]-[5-計算式(自動計算)]
                vbrList.ApplyFormula = true;
                vbrList.Report.Start();

                // Excelファイルを作成して戻り値をセット
                vbrList = new JuchuHaishaIchiranHyo().JuchuHaishaIchiranHyoData(vbrList, reportData, param);

                //【2】===========================================================//
                // Report.Endで帳票処理を終了します。
                //================================================================//
                vbrList.Report.End();

                #region 今回、直接印刷、保存は、プレビュー画面側で行う為コメントヘ

                //【3】===========================================================//
                // Report.End後にReport.SaveAsでExcelファイル出力を行ないます。
                // 第1引数で出力先のファイルパス、
                // 第2引数でExcelファイル形式
                // を指定します。なお、Excelファイル形式はデザインファイルと同じ
                // 形式を設定してください。(xls形式→xlsx形式は不可)
                //================================================================//
                vbrList.Report.SaveAs(fileName, AdvanceSoftware.VBReport8.ExcelVersion.ver2013);

                //【4】===========================================================//
                // 出力が失敗した場合、ErrorNoにてエラー内容が取得できます。
                //================================================================//
                if (vbrList.ErrorNo != AdvanceSoftware.VBReport8.ErrorNo.NoError)
                {
                    MessageBox.Show(
                        vbrList.ErrorMessage + " (" + vbrList.ErrorNo.ToString() + ") ",
                        WINDOW_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                    return false;
                }

                #endregion

                switch (printDestType)
                {
                    case ReportPrintDestType.PrintToPrinter:

                        #region 今回、直接印刷、保存は、プレビュー画面側で行う為コメントヘ

                        ////================================================================//
                        //// ビューアコントロールに帳票ドキュメントを設定します。
                        ////================================================================//
                        //new ReportPreviewVBFrame(WINDOW_TITLE, vbrList, printDestType);

                        #endregion

                        break;
                    case ReportPrintDestType.PrintToScreen:

                        //================================================================//
                        // ビューアコントロールに帳票ドキュメントを設定します。
                        //================================================================//
                        using (ReportPreviewVBFrame f = new ReportPreviewVBFrame(WINDOW_TITLE, vbrList, printDestType))
                        {
                            f.ShowDialog();
                        }

                        break;

                    case ReportPrintDestType.PrintToFax:
                        break;
                    case ReportPrintDestType.ExportExcel:
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
                // vbﾚﾎﾟｰﾄのﾘｿｰｽを開放
                vbrList.Dispose();
                vbrList = null;
            }

            return true;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 印刷条件に出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string GetPrintJokenString(string separateStr)
        {
            string rt_str = TaishoYMDString + separateStr + this.BranchOfficeString;
            if (0 < TokuisakiString.Length)
            {
                rt_str = rt_str + separateStr + TokuisakiString;
            }
            if (0 < HanroString.Length)
            {
                rt_str = rt_str + separateStr + HanroString;
            }
            if (0 < CarString.Length)
            {
                rt_str = rt_str + separateStr + CarString;
            }
            if (0 < CarKindString.Length)
            {
                rt_str = rt_str + separateStr + CarKindString;
            }
            if (0 < ClmClassString.Length)
            {
                rt_str = rt_str + separateStr + ClmClassString;
            }
            if (0 < JuchuTantoString.Length)
            {
                rt_str = rt_str + separateStr + JuchuTantoString;
            }
            if (0 < StaffString.Length)
            {
                rt_str = rt_str + separateStr + StaffString;
            }
            if (0 < ContractString.Length)
            {
                rt_str = rt_str + separateStr + ContractString;
            }
            if (0 < DempyoNoString.Length)
            {
                rt_str = rt_str + separateStr + DempyoNoString;
            }
            if (0 < CarOfChartererString.Length)
            {
                rt_str = rt_str + separateStr + CarOfChartererString;
            }
            return rt_str;
        }

        /// <summary>
        /// 画面「対象期間」の条件指定を文字型で取得します。
        /// </summary>
        private String TaishoYMDString
        {
            get { return string.Format("[日付({0})] {1}～{2}",
                DefaultProperty.GetFilterDateKbnsMeisho((DefaultProperty.FilterDateKbns)Convert.ToInt32(this.cmbFilterDateKbns.SelectedValue)),
                Convert.ToDateTime(this.dteTaishoYMDFrom.Value).ToString("yyyy/MM/dd"),
                Convert.ToDateTime(this.dteTaishoYMDTo.Value).ToString("yyyy/MM/dd"));
            }
        }

        /// <summary>
        /// 画面「営業所」の条件指定を文字型で取得します。
        /// </summary>
        private String BranchOfficeString
        {
            get
            {
                string rt_str = string.Empty;

                if (0 < Convert.ToDecimal(this.cmbBranchOffice.Tag))
                {
                    ToraDONBranchOfficeInfo info = this._DalUtil.ToraDONBranchOffice.GetInfoById(Convert.ToDecimal(this.cmbBranchOffice.Tag));

                    if (info != null && 0 < info.BranchOfficeId)
                    {
                        rt_str = info.BranchOfficeShortName;
                    }
                }
                else
                {
                    rt_str = DefaultProperty.BRANCHOFFICE_ALL_NAME;
                }

                return string.Format("[営業所] {0}", rt_str);
            }
        }

        /// <summary>
        /// 画面「得意先」の条件指定を文字型で取得します。
        /// </summary>
        private String TokuiskaiString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numTokuisakiCd.Value)
                {
                    rt_str = string.Format("[得意先] {0}", this.numTokuisakiCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「得意先」の条件指定を文字型で取得します。
        /// </summary>
        private String TokuisakiString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numTokuisakiCd.Value)
                {
                    rt_str = string.Format("[得意先] {0}", this.numTokuisakiCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「販路」の条件指定を文字型で取得します。
        /// </summary>
        private String HanroString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numHanroCd.Value)
                {
                    rt_str = string.Format("[販路] {0}", this.numHanroCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「車両」の条件指定を文字型で取得します。
        /// </summary>
        private String CarString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numCarCd.Value)
                {
                    rt_str = string.Format("[車両] {0}", this.numCarCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「車種」の条件指定を文字型で取得します。
        /// </summary>
        private String CarKindString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numCarKindCd.Value)
                {
                    rt_str = string.Format("[車種] {0}", this.numCarKindCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「請求部門」の条件指定を文字型で取得します。
        /// </summary>
        private String ClmClassString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numClmClassCd.Value)
                {
                    rt_str = string.Format("[請求部門] {0}", this.numClmClassCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「受注担当」の条件指定を文字型で取得します。
        /// </summary>
        private String JuchuTantoString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numJuchuTantoCd.Value)
                {
                    rt_str = string.Format("[受注担当] {0}", this.numJuchuTantoCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「乗務員」の条件指定を文字型で取得します。
        /// </summary>
        private String StaffString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numDriverCd.Value)
                {
                    rt_str = string.Format("[乗務員] {0}", this.numDriverCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「請負」の条件指定を文字型で取得します。
        /// </summary>
        private String ContractString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numContractCd.Value)
                {
                    rt_str = string.Format("[請負] {0}", this.numContractCd.Value.ToString());
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「伝票No」の条件指定を文字型で取得します。
        /// </summary>
        private String DempyoNoString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numJuchuSlipNoFrom.Value || 0 < this.numJuchuSlipNoTo.Value)
                {
                    rt_str = string.Format("[伝票No] {0}～{1}",
                        0 < this.numJuchuSlipNoFrom.Value ? this.numJuchuSlipNoFrom.Value.ToString() : string.Empty,
                        0 < this.numJuchuSlipNoTo.Value ? this.numJuchuSlipNoTo.Value.ToString() : string.Empty);
                }
                return rt_str;
            }
        }

        /// <summary>
        /// 画面「傭車先」の条件指定を文字型で取得します。
        /// </summary>
        private String CarOfChartererString
        {
            get
            {
                string rt_str = string.Empty;
                if (0 < this.numCarOfChartererCd.Value)
                {
                    rt_str = string.Format("[傭車先] {0}", this.numCarOfChartererCd.Value.ToString());
                }
                return rt_str;
            }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitShanaiTankaMisetteiListFrame();
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

        #region 検索処理

        /// <summary>
        /// キーイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessKeyEvent(KeyEventArgs e)
        {
        switch (e.KeyCode)
            {
                // F5は共通検索画面
                case Keys.F5:
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
            if (SearchFunctions.ContainsKey(ActiveControl))
            {
                SearchFunctions[ActiveControl]();
            }
        }

        /// <summary>
        /// 車両検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCar()
        {
            using (CmnSearchCarFrame f = new CmnSearchCarFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONCarCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 車種検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCarKind()
        {
            using (CmnSearchCarKindFrame f = new CmnSearchCarKindFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONCarKindCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 運転者検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchDriver()
        {
            using (CmnSearchDriverFrame f = new CmnSearchDriverFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 受注担当者検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchJuchuTantosha()
        {
            using (CmnSearchJuchuTantoshaFrame f = new CmnSearchJuchuTantoshaFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 取引先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTorihikisaki()
        {
            using (CmnSearchTorihikisakiFrame f = new CmnSearchTorihikisakiFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;
                f.ShowYoshaFlag = true;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONTorihikiCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 得意先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONTokuisakiCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 請求部門検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchClmClass()
        {
            using (CmnSearchClmClassFrame f = new CmnSearchClmClassFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.ParamTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ClmClassCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 請負検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchContract()
        {
            using (CmnSearchContractFrame f = new CmnSearchContractFrame())
            {
                //パラメータをセット
                f.ParamTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ContractCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 販路検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchHanro()
        {
            using (CmnSearchHanroFrame f = new CmnSearchHanroFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.HanroCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 営業所入力後の処理を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCmbBranchOffice(CancelEventArgs e)
        {
            if (null == this.cmbBranchOffice.SelectedValue)
            {
                //イベントをキャンセル
                e.Cancel = true;
                this.cmbBranchOffice.Tag = null;
                return;
            }

            //営業所情報を取得
            ToraDONBranchOfficeSearchParameter para = new ToraDONBranchOfficeSearchParameter();
            para.BranchOfficeCode = Convert.ToInt32(this.cmbBranchOffice.SelectedValue);
            ToraDONBranchOfficeInfo info = this._DalUtil.ToraDONBranchOffice.GetList(para).FirstOrDefault();

            if (info == null)
            {
                //イベントをキャンセル
                e.Cancel = true;
                this.cmbBranchOffice.Tag = null;
                return;
            }

            this.cmbBranchOffice.Tag = info.BranchOfficeId;
        }

        /// <summary>
        /// 対象期間（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTaishoYMDFrom(CancelEventArgs e)
        {
            //対象期間（範囲開始）が設定された場合
            if (this.dteTaishoYMDFrom.Value != null)
            {
                //月初日が設定された場合
                if (this.dteTaishoYMDFrom.Value.Value.ToString("dd").Equals("01"))
                {
                    //対象期間（範囲終了）に月末日を設定
                    this.dteTaishoYMDTo.Value = this.dteTaishoYMDFrom.Value.Value.AddMonths(1).PreviousDay();
                }
                else
                {
                    //対象期間（範囲終了）に一か月後の対象期間を設定
                    this.dteTaishoYMDTo.Value = this.dteTaishoYMDFrom.Value.Value.AddMonths(1);
                }
            }
            //対象期間（範囲開始）が空の場合
            else
            {
                //システム対象期間を設定
                this.dteTaishoYMDFrom.Value = DateTime.Today;
                this.dteTaishoYMDTo.Value = DateTime.Today.AddMonths(1);
            }
        }

        /// <summary>
        /// 対象期間（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTaishoYMDTo(CancelEventArgs e)
        {
            //対象期間（範囲終了）が空の場合
            if (this.dteTaishoYMDTo.Value == null)
            {
                if (this.dteTaishoYMDFrom.Value != null)
                {
                    //システム対象期間を設定
                    this.dteTaishoYMDTo.Value = this.dteTaishoYMDFrom.Value;
                }
                else
                {
                    //システム対象期間を設定
                    this.dteTaishoYMDTo.Value = DateTime.Today;
                }
            }

            //対象期間（範囲開始）が存在する場合
            if (this.dteTaishoYMDFrom.Value != null)
            {
                if (this.dteTaishoYMDFrom.Value.Value > this.dteTaishoYMDTo.Value.Value)
                {
                    MessageBox.Show(
                        "開始＞終了です。",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    this.dteTaishoYMDTo.Focus();
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                }
            }
        }

        /// <summary>
        /// 車両コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numCarCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 車両情報を取得
                CarSearchParameter para = new CarSearchParameter();
                para.ToraDONCarCode = Convert.ToInt32(this.numCarCd.Value);
                CarInfo info = _DalUtil.Car.GetList(para, null).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.numCarCd.Tag = info.ToraDONCarId;
                    this.numCarCd.Value = info.ToraDONCarCode;
                    this.edtLicPlateCarNo.Text = info.LicPlateCarNo;
                    this.edtCarKindNM.Text = info.CarKindName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numCarCd.Tag = null;
                    this.numCarCd.Value = null;
                    this.edtLicPlateCarNo.Text = string.Empty;
                    this.edtCarKindNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車種コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindCd(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numCarKindCd.Value))
                {
                    is_clear = true;
                    return;
                }

                CarKindInfo info =
                    this._DalUtil.CarKind.GetInfo(Convert.ToInt32(this.numCarKindCd.Value));

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.ToraDONDisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numCarKindCd.Tag = info.ToraDONCarKindId;
                        this.numCarKindCd.Value = info.ToraDONCarKindCode;
                        this.edtCarKindName.Text = info.ToraDONCarKindName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarKindCd.Tag = null;
                    this.numCarKindCd.Value = null;
                    this.edtCarKindName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 乗務員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateDriverCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numDriverCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 乗務員情報を取得
                StaffSearchParameter para = new StaffSearchParameter();
                para.ToraDONStaffCode = Convert.ToInt32(this.numDriverCd.Value);
                StaffInfo info = _DalUtil.Staff.GetList(para, null).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.numDriverCd.Tag = info.ToraDONStaffId;
                    this.numDriverCd.Value = info.ToraDONStaffCode;
                    this.edtDriverNM.Text = info.ToraDONStaffName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numDriverCd.Tag = null;
                    this.numDriverCd.Value = null;
                    this.edtDriverNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 傭車先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarOfChartererCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が場合は抜ける
                if (0 == this.numCarOfChartererCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 取引先情報を取得
                TorihikisakiSearchParameter para = new TorihikisakiSearchParameter();
                para.ToraDONTorihikiCode = Convert.ToInt32(this.numCarOfChartererCd.Value);
                TorihikisakiInfo info = _DalUtil.Torihikisaki.GetList(para, null).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.numCarOfChartererCd.Tag = info.ToraDONTorihikiId;
                    this.numCarOfChartererCd.Value = info.ToraDONTorihikiCode;
                    this.edtCarOfChartererNm.Text = info.ToraDONTorihikiName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numCarOfChartererCd.Tag = null;
                    this.numCarOfChartererCd.Value = null;
                    this.edtCarOfChartererNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 得意先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numTokuisakiCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 得意先情報を取得
                TokuisakiSearchParameter para = new TokuisakiSearchParameter();
                para.ToraDONTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);
                TokuisakiInfo info = _DalUtil.Tokuisaki.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.numTokuisakiCd.Tag = info.ToraDONTokuisakiId;
                    this.numTokuisakiCd.Value = info.ToraDONTokuisakiCode;
                    this.edtTokuisakiNm.Text = info.ToraDONTokuisakiShortName;

                    //関連をクリア
                    this.numClmClassCd.Tag = null;
                    this.numClmClassCd.Value = null;
                    this.edtClmClassNm.Text = string.Empty;
                    this.numContractCd.Tag = null;
                    this.numContractCd.Value = null;
                    this.edtContractNm.Text = string.Empty;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numTokuisakiCd.Tag = null;
                    this.numTokuisakiCd.Value = null;
                    this.edtTokuisakiNm.Text = string.Empty;
                    this.numClmClassCd.Tag = null;
                    this.numClmClassCd.Value = null;
                    this.edtClmClassNm.Text = string.Empty;
                    this.numContractCd.Tag = null;
                    this.numContractCd.Value = null;
                    this.edtContractNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 請求部門コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateClmClassCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numClmClassCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 請求部門情報を取得
                ToraDONClmClassSearchParameter para = new ToraDONClmClassSearchParameter();
                para.ClmClassCode = Convert.ToInt32(this.numClmClassCd.Value);
                ToraDONClmClassInfo info = _DalUtil.ToraDONClmClass.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    //入力中の得意先
                    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
                    tokui_para.ToraDONTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);
                    TokuisakiInfo tokuisakiInfo = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

                    //請求部門の得意先と入力されている得意先が不一致か？
                    if (tokuisakiInfo != null && (info.TokuisakiId != 0 && info.TokuisakiId != tokuisakiInfo.ToraDONTokuisakiId))
                    {
                        //編集をキャンセル
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2203076"),
                        "警告",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    }

                    this.numClmClassCd.Tag = info.ClmClassId;
                    this.numClmClassCd.Value = info.ClmClassCode;
                    this.edtClmClassNm.Text = info.ClmClassName;

                    //関連をクリア
                    this.numContractCd.Tag = null;
                    this.numContractCd.Value = null;
                    this.edtContractNm.Text = string.Empty;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numClmClassCd.Tag = null;
                    this.numClmClassCd.Value = null;
                    this.edtClmClassNm.Text = string.Empty;
                    this.numContractCd.Tag = null;
                    this.numContractCd.Value = null;
                    this.edtContractNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 請負コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateContractCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numContractCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 請負情報を取得
                ToraDONContractSearchParameter para = new ToraDONContractSearchParameter();
                para.ContractCode = Convert.ToInt32(this.numContractCd.Value);
                ToraDONContractInfo info = _DalUtil.ToraDONContract.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    //入力中の得意先
                    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
                    tokui_para.ToraDONTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);
                    TokuisakiInfo tokuisakiInfo = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

                    // 請求部門情報を取得
                    ToraDONClmClassSearchParameter clm_para = new ToraDONClmClassSearchParameter();
                    clm_para.ClmClassCode = Convert.ToInt32(this.numClmClassCd.Value);
                    ToraDONClmClassInfo clmclassInfo = _DalUtil.ToraDONClmClass.GetList(clm_para).FirstOrDefault();

                    //請求部門の得意先と入力されている得意先が不一致か？
                    if (tokuisakiInfo != null
                        && (info.TokuisakiId != 0 && info.TokuisakiId != tokuisakiInfo.ToraDONTokuisakiId)
                        && (info.ClmClassId != 0 && info.ClmClassId != clmclassInfo.ClmClassId))
                    {
                        //編集をキャンセル
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2203076"),
                        "警告",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    }

                    this.numContractCd.Tag = info.ContractId;
                    this.numContractCd.Value = info.ContractCode;
                    this.edtContractNm.Text = info.ContractName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numContractCd.Tag = null;
                    this.numContractCd.Value = null;
                    this.edtContractNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 販路コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHanroCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numHanroCd.Value)
                {
                    isClear = true;
                    return;
                }

                // 販路情報を取得
                HanroSearchParameter para = new HanroSearchParameter();
                para.HanroCode = Convert.ToInt32(this.numHanroCd.Value);
                HanroInfo info = _DalUtil.Hanro.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.numHanroCd.Tag = info.HanroId;
                    this.numHanroCd.Value = info.HanroCode;
                    this.edtHanroNm.Text = info.HanroName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numHanroCd.Tag = null;
                    this.numHanroCd.Value = null;
                    this.edtHanroNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 受注担当コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateJuchuTantoCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == this.numJuchuTantoCd.Value)
                {
                    isClear = true;
                    return;
                }


                // 受注担当情報を取得
                StaffSearchParameter para = new StaffSearchParameter();
                para.ToraDONStaffCode = Convert.ToInt32(this.numJuchuTantoCd.Value);
                StaffInfo info = _DalUtil.Staff.GetList(para, null).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.numJuchuTantoCd.Tag = info.ToraDONStaffId;
                    this.numJuchuTantoCd.Value = info.ToraDONStaffCode;
                    this.edtJuchuTantoNm.Text = info.ToraDONStaffName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numJuchuTantoCd.Tag = null;
                    this.numJuchuTantoCd.Value = null;
                    this.edtJuchuTantoNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 営業所のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbBranchOffice_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateCmbBranchOffice(e);
        }

        private void numCarCd_Validating(object sender, CancelEventArgs e)
        {
            //車両コード
            this.ValidateCarCd(e);
        }

        private void numCarKindCd_Validating(object sender, CancelEventArgs e)
        {
            //車種コード
            this.ValidateCarKindCd(e);
        }

        private void numDriverCd_Validating(object sender, CancelEventArgs e)
        {
            //乗務員コード
            this.ValidateDriverCd(e);
        }

        private void numCarOfChartererCd_Validating(object sender, CancelEventArgs e)
        {
            //傭車先コード
            this.ValidateCarOfChartererCd(e);
        }

        private void numTokuisakiCd_Validating(object sender, CancelEventArgs e)
        {
            //得意先コード
            this.ValidateTokuisakiCd(e);
        }

        private void numClmClassCd_Validating(object sender, CancelEventArgs e)
        {
            //請求部門コード
            this.ValidateClmClassCd(e);
        }

        private void numContractCd_Validating(object sender, CancelEventArgs e)
        {
            //請負コード
            this.ValidateContractCd(e);
        }

        private void numHanroCd_Validating(object sender, CancelEventArgs e)
        {
            //販路コード
            this.ValidateHanroCd(e);
        }

        private void numJuchuTantoCd_Validating(object sender, CancelEventArgs e)
        {
            //受注担当コード
            this.ValidateJuchuTantoCd(e);
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

        private void dteTaishoYMDFrom_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(this.dteTaishoYMDFrom.Value) == DateTime.MinValue)
            {
                DateTime today = DateTime.Today;
                this.dteTaishoYMDFrom.Value = new DateTime(today.Year, today.Month, 1);
            }
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void JuchuHaishaIchiranHyoPrtToFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        /// <summary>
        /// 日付項目Changeイベント（Down）
        /// </summary>
        private void ymd_Down(object sender, EventArgs e)
        {
            try
            {
                ((NskDateTime)this.ActiveControl).Value
                    = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(-1);
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// 日付項目Changeイベント（Up）
        /// </summary>
        private void ymd_Up(object sender, EventArgs e)
        {
            try
            {
                ((NskDateTime)this.ActiveControl).Value
                    = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(1);
            }
            catch
            {
                ;
            }
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
