using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using System.Globalization;
using jp.co.jpsys.util;
using Jpsys.HaishaManageV10.ComLib;
using System.Runtime.InteropServices;
using Jpsys.HaishaManageV10.ReportDAL;
using Jpsys.HaishaManageV10.ReportModel;
using System.Threading.Tasks;
using Jpsys.HaishaManageV10.ReportFrame;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using AdvanceSoftware.VBReport8;
using Jpsys.HaishaManageV10.VBReport;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class SeikyuDataSakuseiPrtToFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル初期値
        /// </summary>
        private const string WINDOW_TITLE = "請求データ作成";

        /// <summary>
        /// 帳票指示画面で使用するCrystalReportのレポートファイルの名前を配列で保持
        /// </summary>
        private static string[] REPORT_CLASSIES =
            new string[] { "SeikyuDataSakuseiPrtToRpt" };

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示している印刷操作メニューを保持する領域
        /// </summary>
        private AddPrintActionMenuItem printActionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 請求データ作成クラス
        /// </summary>
        private SeikyuDataSakuseiRpt _SeikyuDataSakuseiRpt;

        /// <summary>
        /// 管理情報クラス
        /// </summary>
        private KanriInfo _KanriInfo;

        /// <summary>
        /// 請求データ連携条件情報
        /// </summary>
        private SeikyuDataSakuseiConditionInfo _Param = new SeikyuDataSakuseiConditionInfo();

        /// <summary>
        /// 画面の日付条件範囲限度（月数）
        /// </summary>
        private const int HIZUKE_SPAN_MONTHS = 13;

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
        public SeikyuDataSakuseiPrtToFrame()
        {
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

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // 請求データ作成クラスインスタンス作成
            this._SeikyuDataSakuseiRpt = new SeikyuDataSakuseiRpt(this.appAuth);

            // 管理情報取得
            this._KanriInfo = this._DalUtil.Kanri.GetInfo();

            //日付ラベル取得
            string lblHizuke = "日付";
            switch (this._KanriInfo.SeikyuRenkeiHizukeKbn)
            {
                case (int)DefaultProperty.SeikyuRenkeiHizukeKbn.AddUpYMD:
                case (int)DefaultProperty.SeikyuRenkeiHizukeKbn.StartYMD:
                case (int)DefaultProperty.SeikyuRenkeiHizukeKbn.ChakuYMD:
                    lblHizuke = lblHizuke + "（" +
                        DefaultProperty.GetSeikyuRenkeiHizukeKbnMeisho((DefaultProperty.SeikyuRenkeiHizukeKbn)this._KanriInfo.SeikyuRenkeiHizukeKbn) +
                        "）";
                    break;
                default:
                    break;
            }
            lblHizuke = lblHizuke + " *";
            this.lblHizuke.Text = lblHizuke;

                // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            //コンボボックスの初期化
            this.InitCombo();

            // 入力項目のクリア
            this.ClearInputs();

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
            this.InitPrintActionMenuItems();
        }

        /// <summary>
        /// 印刷操作メニューを初期化します。
        /// </summary>
        private void InitPrintActionMenuItems()
        {
            this.printActionMenuItems = new AddPrintActionMenuItem();

            this.printActionMenuItems.SetCreatingItem(PrintActionMenuItems.ExecProc);
            this.printActionMenuItems.SetCreatingItem(PrintActionMenuItems.Separator);
            this.printActionMenuItems.SetCreatingItem(PrintActionMenuItems.DelPrintSetting);
            this.printActionMenuItems.SetCreatingItem(PrintActionMenuItems.Separator);
            this.printActionMenuItems.SetCreatingItem(PrintActionMenuItems.Close);

            this.printActionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***実行
            this.commandSet.ExecProc.Execute += FaxCommand_Execute;
            this.commandSet.Bind(
                this.commandSet.ExecProc,
                this.btnExecProc,
                this.printActionMenuItems.GetMenuItemBy(PrintActionMenuItems.ExecProc),
                this.toolStripFax
                );

            //***終了
            this.commandSet.Close.Execute += CloseCommand_Execute;
            this.commandSet.Bind(
                this.commandSet.Close,
                this.btnClose,
                this.printActionMenuItems.GetMenuItemBy(PrintActionMenuItems.Close),
                this.toolStripEnd
                );

            //***印刷設定の削除
            this.commandSet.DelPrintSetting.Execute += DelPrintSettingCommand_Execute;
            this.commandSet.Bind(
                this.commandSet.DelPrintSetting,
                this.printActionMenuItems.GetMenuItemBy(PrintActionMenuItems.DelPrintSetting)
                );
        }

        private void DelPrintSettingCommand_Execute(object sender, EventArgs e)
        {
            // 印刷設定の削除
            this.DoDelPrintSetting();
        }

        private void FaxCommand_Execute(object sender, EventArgs e)
        {
            // 実行
            this.DoExecProc();
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

            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteHizukeYMDFrom, ctl => ctl.Text, this.dteHizukeYMDFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteHizukeYMDTo, ctl => ctl.Text, this.dteHizukeYMDTo_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.cmbBranchOffice, ctl => ctl.Text, this.cmbBranchOffice_Validating));
        }

        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitBranchOfficeCombo();
        }

        /// <summary>
        /// 営業所コンボボックスを初期化します。
        /// </summary>
        private void InitBranchOfficeCombo()
        {
            // 営業所コンボ設定
            Dictionary<int, String> datasource = new Dictionary<int, String>();

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

            int key = 0;
            String value = "";

            foreach (ToraDONBranchOfficeInfo item in list)
            {
                key = item.BranchOfficeCode;
                value = item.BranchOfficeCode.ToString() + " " + item.BranchOfficeShortName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbBranchOffice, datasource, true, null, false);
        }

        #endregion

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.cmbBranchOffice.SelectedValue = null;
            this.cmbBranchOffice.Tag = null;
            this.dteHizukeYMDFrom.Value = DateTimeExtensions.FirstDayOfMonth(DateTime.Today);
            this.dteHizukeYMDTo.Value = DateTimeExtensions.FirstDayOfMonth(DateTime.Today);

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
            this.cmbBranchOffice.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 請求データ作成を実行します。
        /// </summary>
        /// <param name="printDestType"></param>
        private void DoExecProc()
        {
            if (!this.ValidateChildren())
            {
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                return;
            }

            if (!this.CheckInputs())
                return;

            //印刷プレビュー可否フラグ
            bool previewFlag = false;

            // 確認MSG
            DialogResult res2 =
            MessageBox.Show(
                "請求確認リストをプレビュー表示します。よろしいですか？",
                this.Text,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button2
                );

            previewFlag = res2 == DialogResult.Yes;

            //画面を使用不可に
            this.ControlEnableChangeForPrinting(false);
            if (previewFlag)
            {
                //画面タイトルを変更する
                this.Text = "請求確認リスト　" + "印刷中...";
            }
            else
            {
                //画面タイトルを変更する
                this.Text = "請求データ作成中...";
            }
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;
            
            //すべてのマスタ同期
            SQLHelper.ActionWithTransaction(tx =>
            {
                MergeToraDon margeToraDon = new MergeToraDon(this.appAuth);
                margeToraDon.MergeToraDonAllTables(tx);
            });

            //画面からパラメーターを取得
            this._Param =
                this.CreateParameterFromScreen();

            //請求データ取得（排他管理用）
            SeikyuDataInfo haitaInfo = this._DalUtil.SeikyuData.GetInfo(Convert.ToDecimal(this.cmbBranchOffice.Tag));

            if (haitaInfo == null || haitaInfo.BranchOfficeId == 0)
            {
                haitaInfo = new SeikyuDataInfo()
                {
                    BranchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag)
                };
            }

            this.CreateReportDataAsync(this._Param).ContinueWith(t =>
            {
                // ハンドルが関連付けられていない場合（画面が閉じられた場合など）、処理しない
                if (!this.IsHandleCreated)
                    return;

                try
                {
                    // エラーが発生した場合の処理
                    if (t.IsFaulted)
                    {
                        var innerException = t.Exception.InnerException;

                        if (innerException is CanRetryException)
                        {
                            // リトライ可能な例外ハンドラ
                            FrameUtilites.ShowExceptionMessage(innerException, this);
                            return;
                        }
                        else if (innerException is MustCloseFormException)
                        {
                            // 画面の終了が要求される例外ハンドラ
                            FrameUtilites.ShowExceptionMessage(innerException, this);
                            this.DoClose();
                            return;
                        }
                        else
                        {
                            // その他の例外ハンドラ
                            FrameUtilites.ShowExceptionMessage(innerException, this);
                            return;
                        }
                    }

                    var reportData = t.Result;

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

                    if (previewFlag)
                    {
                        // 出力先パス生成
                        string fileName = Path.GetTempPath() + WINDOW_TITLE + "_"
                             + DateTime.Now.ToString("yyyyMMddHHmmssttt") + ".xlsx";

                        // 砂時計
                        Cursor.Current = Cursors.WaitCursor;

                        try
                        {
                            //Excel作成
                            this.CreateSeikyuKakuninList(ReportPrintDestType.PrintToScreen, fileName, reportData, this._Param);
                        }
                        finally
                        {
                            // デフォルトに戻す
                            Cursor.Current = Cursors.Default;
                        }

                        // 操作ログ(プレビュー)出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.PrintPreview, this.CreateLog());
                    }

                    #endregion

                    #region 車両ID、乗務員ID、傭車ID等、必須チェック（未入力データは作成されないはずだが念のため）

                    //車両ID、乗務員ID、傭車ID等、未登録データ取得
                    var errList = reportData
                        .Where(x => (x.CarKbn != (int)DefaultProperty.CarKbn.Yosha && x.CarId == 0) //車両チェック
                            || (x.CarKbn != (int)DefaultProperty.CarKbn.Yosha && x.CarId == 0 && x.DriverId == 0) //乗務員チェック
                            || (x.CarKbn == (int)DefaultProperty.CarKbn.Yosha && x.CarOfChartererId == 0)) //傭車先チェック
                        .ToList();

                    //未登録データが存在する場合
                    if (errList != null && 0 < errList.Count)
                    {
                        DialogResult res3 =
                            MessageBox.Show(
                                "車両・乗務員、または、傭車先が未入力のデータが存在します。\r\n" +
                                "車両・乗務員、または、傭車先を入力後もう一度処理を行ってください。",
                                WINDOW_TITLE,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                                );

                        return;
                    }

                    // 確認MSG
                    DialogResult res4 =
                        MessageBox.Show(
                            "請求データを作成します。よろしいですか？",
                            WINDOW_TITLE,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.None,
                            MessageBoxDefaultButton.Button2
                            );

                    if (res4 == DialogResult.No)
                    {
                        return;
                    }

                    #endregion

                    #region トラDONの最終月次集計月チェック

                    //トラDONの最終月次集計月を取得
                    DateTime lastSummaryOfMonthDate = this.GetToraDONLastSummaryOfMonthDate();

                    if (this.dteHizukeYMDTo.Value.Value <= lastSummaryOfMonthDate)
                    {
                        MessageBox.Show(
                                FrameUtilites.GetDefineMessage("ME2303010", new string[] { lastSummaryOfMonthDate.ToString("yyyy/MM/dd") }),
                                WINDOW_TITLE,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                        return;
                    }

                    #endregion

                    #region 請求残高集計処理済データ存在チェック

                    //締切済売上データ取得
                    IList<String> fixSaleList = this._SeikyuDataSakuseiRpt.GetClmFixDateList(this._Param);

                    //締切済売上データが存在する場合
                    if (fixSaleList != null && 0 < fixSaleList.Count)
                    {
                        String msg = "下記請求先が請求残高集計処理済です。\r\n\r\n【請求先】\r\n" + String.Join("\r\n", fixSaleList)
                            + "\r\n\r\nトラDONシステムで集計処理を取消後、請求データを作成してください。";

                        DialogResult res5 =
                            MessageBox.Show(
                                msg,
                                WINDOW_TITLE,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );

                        return;
                    }

                    #endregion

                    #region 支払残高集計処理済データ存在チェック

                    //支払済売上データ取得
                    fixSaleList = this._SeikyuDataSakuseiRpt.GetCharterPayFixDateList(this._Param);

                    //締切済売上データが存在する場合
                    if (fixSaleList != null && 0 < fixSaleList.Count)
                    {
                        String msg = "下記取引先が支払残高集計処理済です。\r\n\r\n【取引先】\r\n" + String.Join("\r\n", fixSaleList)
                            + "\r\n\r\nトラDONシステムで集計処理を取消後、請求データを作成してください。";

                        DialogResult res5 =
                            MessageBox.Show(
                                msg,
                                WINDOW_TITLE,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );

                        return;
                    }

                    #endregion

                    #region 請求データ作成処理

                    //待機状態へ
                    this.Cursor = Cursors.WaitCursor;

                    //請求データ作成処理
                    try
                    {
                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            //日報、売上を「営業所ID、発日、車両ID、車番、車種ID、乗務員ID、傭車先ID」ごとに1：Nで作成
                            this._SeikyuDataSakuseiRpt.CreateSeikyuData(tx, this._Param, haitaInfo);
                        });

                        //操作ログ（処理の実行）出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.ProcessTask,
                                this.CreateLog());

                        //登録完了メッセージ
                        MessageBox.Show(
                            "請求データ作成が完了しました。",
                            WINDOW_TITLE,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (CanRetryException ex)
                    {
                        //再実行可能な例外ハンドラ
                        FrameUtilites.ShowExceptionMessage(ex, this);
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
                    }
                    catch (Exception ex)
                    {
                        FrameUtilites.ShowExceptionMessage(ex, this);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }

                    #endregion

                }
                finally
                {
                    //画面を使用可能に
                    this.ControlEnableChangeForPrinting(true);
                    //画面タイトルを変更する
                    this.Text = WINDOW_TITLE;
                    //カーソルを元に戻す
                    this.Cursor = Cursors.Default;
                    //画面の最初の項目にフォーカスを移動
                    this.SetFirstFocus();
                }
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// トラDONの最終月次集計月を取得します。
        /// </summary>
        /// <returns>トラDONの最終月次集計月</returns>
        private DateTime GetToraDONLastSummaryOfMonthDate()
        {
            //戻り値
            DateTime rt_val = DateTime.MaxValue;

            //トラDON管理情報取得
            ToraDonSystemPropertyInfo ToraDonKanriInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();

            if (DateTime.MinValue < ToraDonKanriInfo.LastSummaryOfMonthDate)
            {
                rt_val = NSKUtil.MonthLastDay(ToraDonKanriInfo.LastSummaryOfMonthDate);
            }

            return rt_val;
        }

        /// <summary>
        /// 請求確認リスト（Excel）を作成します。
        /// </summary>
        /// <param name="printDestType"></param>
        /// <param name="fileName">作成するファイル名（パス付き）</param>
        /// <param name="reportData">出力情報</param>
        /// <param name="param">印刷条件</param>
        /// <returns></returns>
        private bool CreateSeikyuKakuninList(ReportPrintDestType printDestType, string fileName,
            IList<SeikyuKakuninListRptInfo> reportData, SeikyuDataSakuseiConditionInfo param)
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
                vbrList = new SeikyuKakuninList().SeikyuKakuninListData(vbrList, reportData, param);

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

            //営業所の必須チェック
            if (rt_val && Convert.ToInt32(this.cmbBranchOffice.SelectedValue) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "営業所" });
                ctl = this.cmbBranchOffice;
            }

            // 日付（範囲開始）
            if (rt_val && this.HizukeYMDFrom == DateTime.MinValue)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { "日付（範囲開始）" });
                ctl = this.dteHizukeYMDFrom;
            }
            else
            {
                // 日付（範囲終了）
                if (rt_val && this.HizukeYMDTo == DateTime.MinValue)
                {
                    this.dteHizukeYMDTo.Value = this.HizukeYMDFrom;
                }

                if (this.HizukeYMDFrom > this.HizukeYMDTo)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.dteHizukeYMDTo;
                }
                else
                {
                    if (this.HizukeYMDFrom.AddMonths(HIZUKE_SPAN_MONTHS) < this.HizukeYMDTo)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage("MW2202026", new string[] { "HIZUKE_SPAN_MONTHS" });
                        ctl = this.dteHizukeYMDTo;
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

            return rt_val;
        }

        /// <summary>
        /// 画面からパラメーターを取得します。
        /// </summary>
        private SeikyuDataSakuseiConditionInfo CreateParameterFromScreen()
        {
            return new SeikyuDataSakuseiConditionInfo()
            {
                BranchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag),
                HizukeYMDFrom = this.HizukeYMDFrom.Date,
                HizukeYMDTo = this.HizukeYMDTo.Date,
                SeikyuRenkeiHizukeKbn = (DefaultProperty.SeikyuRenkeiHizukeKbn)this._KanriInfo.SeikyuRenkeiHizukeKbn,
                PrintJokenString = this.GetPrintJokenString(),
            };
        }

        /// <summary>
        /// レポートデータを非同期で取得します。
        /// </summary>
        /// <returns></returns>
        private Task<IList<SeikyuKakuninListRptInfo>> CreateReportDataAsync(
            SeikyuDataSakuseiConditionInfo param)
        {
            return
                Task.Factory.StartNew(() =>
                {
                    //戻り値
                    IList<SeikyuKakuninListRptInfo> ret_list = new List<SeikyuKakuninListRptInfo>();

                    //対象データ作成／取得
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        //トラDONに連携する請求データを作成
                        ret_list = this._SeikyuDataSakuseiRpt.GetRptInfoList(tx, param);
                    });

                    return ret_list;
                });
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
            this.pnlMain.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.printActionMenuItems.PrintActionMenuItemEnable(PrintActionMenuItems.DelPrintSetting, val);
            this.printActionMenuItems.PrintActionMenuItemEnable(PrintActionMenuItems.ExecProc, val);
            this.printActionMenuItems.PrintActionMenuItemEnable(PrintActionMenuItems.Close, val);
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string CreateLog()
        {
            //操作ログ(保存)の条件取得
            string log_jyoken =
                "[営業所] " + BranchOfficeString +
                "\r\n[日付] " + HizukeYMDString;

            return log_jyoken;
        }

        /// <summary>
        /// 請求確認リストに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string GetPrintJokenString()
        {
            //操作ログ(保存)の条件取得
            string log_jyoken =
                "[営業所] " + BranchOfficeString +
                "　[日付] " + HizukeYMDString;

            return log_jyoken;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// 印刷設定を削除します。
        /// </summary>
        private void DoDelPrintSetting()
        {
            // 確認MSG
            DialogResult res =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2101009"),
                    this.Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                    );

            if (res == DialogResult.Yes)
            {
                // レポートファイル名の配列をループで展開
                foreach (string item in REPORT_CLASSIES)
                {
                    FrameUtilites.DelPrintSetting(item);
                }

                // 完了MSG
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MI2001010"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
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

        #region 検証（Validate）処理

        /// <summary>
        /// 日付（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHizukeYMDFrom(CancelEventArgs e)
        {
            //日付（範囲開始）が設定された場合
            if (this.dteHizukeYMDFrom.Value != null)
            {
                this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value.Date;
            }
            else
            {
                this.dteHizukeYMDFrom.Value = DateTimeExtensions.FirstDayOfMonth(DateTime.Today);
                this.dteHizukeYMDTo.Value = DateTimeExtensions.FirstDayOfMonth(DateTime.Today);
            }
        }

        /// <summary>
        /// 日付（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHizukeYMDTo(CancelEventArgs e)
        {
            //日付（範囲終了）が空の場合
            if (this.dteHizukeYMDTo.Value == null)
            {
                this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value.Date;
            }

            //日付（範囲開始）が存在する場合
            if (this.dteHizukeYMDFrom.Value != null)
            {
                if (this.dteHizukeYMDFrom.Value.Value > this.dteHizukeYMDTo.Value.Value)
                {
                    MessageBox.Show(
                        "開始＞終了です。",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    this.dteHizukeYMDTo.Focus();
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                }
            }
        }

        /// <summary>
        /// 営業所コンボボックスの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCmbBranchOffice(CancelEventArgs e)
        {
            //営業所情報を取得
            ToraDONBranchOfficeSearchParameter para = new ToraDONBranchOfficeSearchParameter();
            para.BranchOfficeCode = Convert.ToInt32(this.cmbBranchOffice.SelectedValue);
            ToraDONBranchOfficeInfo info = this._DalUtil.ToraDONBranchOffice.GetList(para).FirstOrDefault();

            if (info == null)
            {
                this.cmbBranchOffice.Tag = decimal.Zero;

                //イベントをキャンセル
                e.Cancel = true;
                return;
            }

            this.cmbBranchOffice.Tag = info.BranchOfficeId;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 日付（範囲開始）の値を取得します。
        /// </summary>
        public DateTime HizukeYMDFrom
        {
            get { return Convert.ToDateTime(this.dteHizukeYMDFrom.Value).Date; }
        }

        /// <summary>
        /// 日付（範囲開始）の値を取得します。
        /// </summary>
        public DateTime HizukeYMDTo
        {
            get { return Convert.ToDateTime(this.dteHizukeYMDTo.Value).Date; }
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

                return rt_str;
            }
        }

        /// <summary>
        /// 画面「日付」の条件指定を文字型で取得します。
        /// </summary>
        private String HizukeYMDString
        {
            get { return string.Format("{0}", string.Format("{0}～{1}",
                Convert.ToDateTime(this.dteHizukeYMDFrom.Value).ToString("yyyy/MM/dd"),
                Convert.ToDateTime(this.dteHizukeYMDTo.Value).ToString("yyyy/MM/dd"))); }
        }

        #endregion

        #region プライベートクラス

        /// <summary>
        /// 選択請求先情報
        /// </summary>
        private class SelectSeikyusakiResult
        {
            public Int32 SeikyusakiCode { get; set; }
            public Decimal SeikyusakiId { get; set; }
        }

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

        private void SeikyuDataSakuseiPrtToFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void SeikyuDataSakuseiPrtToFrame_KeyDown(object sender, KeyEventArgs e)
        {
            // 画面 KeyDown
            this.ProcessKeyEvent(e);
        }

        private void dteHizukeYMDFrom_Validating(object sender, CancelEventArgs e)
        {
            // 日付（範囲開始）
            this.ValidateHizukeYMDFrom(e);
        }

        private void dteHizukeYMDTo_Validating(object sender, CancelEventArgs e)
        {
            // 日付（範囲終了）
            this.ValidateHizukeYMDTo(e);
        }

        private void cmbBranchOffice_Validating(object sender, CancelEventArgs e)
        {
            //営業所コンボボックス
            this.ValidateCmbBranchOffice(e);
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
    }
}
