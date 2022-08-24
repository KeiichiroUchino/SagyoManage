using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.Frame.Command;
using System.Runtime.InteropServices;
using Jpsys.HaishaManageV10.ReportModel;
using AdvanceSoftware.VBReport8;
using Jpsys.HaishaManageV10.VBReport;
using System.IO;
using Jpsys.HaishaManageV10.ReportDAL;
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class YoshaIraishoPrtToFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "傭車依頼書";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示している印刷操作メニューを保持する領域
        /// </summary>
        private AddPrintActionMenuItem _AddPrintActionMenuItem;
        
        /// <summary>
        /// 取得した配送指示書（印刷条件）を保持する領域
        /// </summary>
        private YoshaIraishoPrtSearchParameter _YoshaIraishoConditionInfo = null;

        /// <summary>
        /// 取得した配送指示書情報を保持しておく領域
        /// </summary>
        private List<YoshaIraishoPrtInfo> reportDataList;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを
        /// 利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        #region 得意先リスト

        //--Spread列定義

        /// <summary>
        /// 得意先コード列番号
        /// </summary>
        private const int COL_TOKUI_CODE = 0;

        /// <summary>
        /// 得意先名列番号
        /// </summary>
        private const int COL_TOKUI_NAME = 1;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_TOKUI_SELECT_CHECKBOX = 2;

        /// <summary>
        /// 得意先Id列番号
        /// </summary>
        private const int COL_TOKUI_ID = 3;

        /// <summary>
        /// 得意先リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_TOKUILIST = 4;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialTokuisakiStyleInfoArr;

        /// <summary>
        /// 得意先情報のリストを保持する領域
        /// </summary>
        private IList<TokuisakiInfo> _TokuiInfoList = null;

        /// <summary>
        /// 得意先コード（範囲終了）初期値
        /// </summary>
        private const decimal TOKUI_CODEFROM_DEFAULT = 0;

        /// <summary>
        /// 得意先コード（範囲終了）初期値
        /// </summary>
        private const decimal TOKUI_CODETO_DEFAULT = 99999999;

        /// <summary>
        /// 得意先名（範囲開始）初期値
        /// </summary>
        private const string TOKUI_NAMEFROM_DEFAULT = "***** 最初から *****";

        /// <summary>
        /// 得意先コード（範囲終了）初期値
        /// </summary>
        private const string TOKUI_NAMETO_DEFAULT = "***** 最後まで *****";

        #endregion

        #region 傭車リスト

        //--Spread列定義

        /// <summary>
        /// 傭車コード列番号
        /// </summary>
        private const int COL_CAR_OF_CHARTERER_CODE = 0;

        /// <summary>
        /// 傭車名（取引先名）
        /// </summary>
        private const int COL_CAR_OF_CHARTERER_NAME = 1;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_CAR_OF_CHARTERER_SELECT_CHECKBOX = 2;

        /// <summary>
        /// 傭車Id列番号
        /// </summary>
        private const int COL_CAR_OF_CHARTERER_ID = 3;

        /// <summary>
        /// 傭車リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST = 4;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialCarStyleInfoArr;

        /// <summary>
        /// 傭車情報のリストを保持する領域
        /// </summary>
        private IList<CarInfo> _CarInfoList = null;

        /// <summary>
        /// 傭車コード（範囲終了）初期値
        /// </summary>
        private const decimal CAR_CODEFROM_DEFAULT = 0;

        /// <summary>
        /// 傭車コード（範囲終了）初期値
        /// </summary>
        private const decimal CAR_CODETO_DEFAULT = 99999;

        /// <summary>
        /// 傭車名（範囲開始）初期値
        /// </summary>
        private const string CAR_NAMEFROM_DEFAULT = "***** 最初から *****";

        /// <summary>
        /// 傭車コード（範囲終了）初期値
        /// </summary>
        private const string CAR_NAMETO_DEFAULT = "***** 最後まで *****";

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
        public YoshaIraishoPrtToFrame()
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
        private void InitYoshaIraishoFrame()
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

            // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.OperetorName = UserProperty.GetInstance().LoginOperatorName;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するための
            // ファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            // Spread 関連の初期化
            this.InitSheet();

            // 入力項目のクリア
            this.ClearInputs();

            // 得意先リストのセット
            this.SetTokuisakiListSheet();

            // 傭車リストのセット
            this.SetCarOfChartererListSheet();

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
            this.InitPrintActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitPrintActionMenuItems()
        {
            this._AddPrintActionMenuItem = new AddPrintActionMenuItem();

            this._AddPrintActionMenuItem.SetCreatingItem(PrintActionMenuItems.PrintToScreen);
            this._AddPrintActionMenuItem.SetCreatingItem(PrintActionMenuItems.Separator);
            this._AddPrintActionMenuItem.SetCreatingItem(PrintActionMenuItems.Close);

            this._AddPrintActionMenuItem.AddMenu(this.menuStripTop);
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
               this.btnPreview, _AddPrintActionMenuItem.GetMenuItemBy(PrintActionMenuItems.PrintToScreen), this.toolStripPreview);

            //***終了
            commandSet.Close.Execute += CloseCommand_Execute;
            commandSet.Bind(commandSet.Close,
               this.btnClose, _AddPrintActionMenuItem.GetMenuItemBy(PrintActionMenuItems.Close), this.toolStripEnd);
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
                this.numTokuisakiCodeFrom,
                this.numTokuisakiCodeTo,
                this.numCarOfChartererCodeFrom,
                this.numCarOfChartererCodeTo
                );

            this.searchStateBinder.AddStateObject(this.toolStripReference);
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
                ControlValidatingEventRaiser.Create(this.numTokuisakiCodeFrom, ctl => ctl.Text, this.numTokuisakiCodeFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTokuisakiCodeTo, ctl => ctl.Text, this.numTokuisakiCodeTo_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarOfChartererCodeFrom, ctl => ctl.Text, this.numCarOfChartererCodeFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarOfChartererCodeTo, ctl => ctl.Text, this.numCarOfChartererCodeTo_Validating));
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
            // 得意先リストスタイル情報初期化
            this.InitTokuisakiStyleInfo();
            // 傭車リストスタイル情報初期化
            this.InitCarOfChartererStyleInfo();
        }

        /// <summary>
        /// 得意先リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitTokuisakiStyleInfo()
        {
            // Id列非表示
            this.fpTokuisakiListGrid.Sheets[0].Columns[COL_TOKUI_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpTokuisakiListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialTokuisakiStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_TOKUILIST];

            for (int i = 0; i < COL_MAXCOLNUM_TOKUILIST; i++)
            {
                this.initialTokuisakiStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// 傭車リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitCarOfChartererStyleInfo()
        {
            // Id列非表示
            this.fpCarOfChartererListGrid.Sheets[0].Columns[COL_CAR_OF_CHARTERER_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpCarOfChartererListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialCarStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST];

            for (int i = 0; i < COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST; i++)
            {
                this.initialCarStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //得意先リストの初期化
            this.InitTokuisakiListSheet();

            //傭車リストの初期化
            this.InitCarOfChartererListSheet();
        }

        /// <summary>
        /// 得意先リストを初期化します。
        /// </summary>
        private void InitTokuisakiListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpTokuisakiListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_TOKUILIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 傭車リストを初期化します。
        /// </summary>
        private void InitCarOfChartererListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpCarOfChartererListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //印刷条件
            this.dteHizukeYMDFrom.Value = DateTime.Today.AddDays(1);
            this.dteHizukeYMDTo.Value = DateTime.Today.AddDays(1);

            //得意先印刷条件
            this.radTokuisakiHaniShitei.Checked = true;
            this.numTokuisakiCodeFrom.Tag = null;
            this.numTokuisakiCodeFrom.Value = TOKUI_CODEFROM_DEFAULT;
            this.edtTokuisakiNameFrom.Text = TOKUI_NAMEFROM_DEFAULT;
            this.numTokuisakiCodeTo.Tag = null;
            this.numTokuisakiCodeTo.Value = TOKUI_CODETO_DEFAULT;
            this.edtTokuisakiNameTo.Text = TOKUI_NAMETO_DEFAULT;

            //傭車印刷条件
            this.radCarOfChartererHaniShitei.Checked = true;
            this.numCarOfChartererCodeFrom.Tag = null;
            this.numCarOfChartererCodeFrom.Value = CAR_CODEFROM_DEFAULT;
            this.edtCarOfChartererNameFrom.Text = CAR_NAMEFROM_DEFAULT;
            this.numCarOfChartererCodeTo.Tag = null;
            this.numCarOfChartererCodeTo.Value = CAR_CODETO_DEFAULT;
            this.edtCarOfChartererNameTo.Text = CAR_NAMETO_DEFAULT;

            //備考
            this.edtBiko01.Text = string.Empty;
            this.edtBiko02.Text = string.Empty;
            this.edtBiko03.Text = string.Empty;
            this.edtBiko04.Text = string.Empty;
            this.edtBiko05.Text = string.Empty;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        /// <summary>
        /// 印刷データを保持しているメンバをクリアします。
        /// </summary>
        public void ReportDataListClear()
        {
            if (this.reportDataList != null)
            {
                //this.reportDataList.Clear();
                this.reportDataList = new List<YoshaIraishoPrtInfo>();
            }
        }

        #endregion

        #region プライベート メソッド

        /// <summary>コントロールの活性／非活性を初期化します。
        /// </summary>
        private void InitControlsEnabled()
        {
            this.ChangeTokuisakiCondition();
            this.ChangeCarCondition();
        }
        
        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            this.dteHizukeYMDFrom.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 得意先指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeTokuisakiCondition()
        {
            this.numTokuisakiCodeFrom.Enabled = this.radTokuisakiHaniShitei.Checked;
            this.numTokuisakiCodeTo.Enabled = this.radTokuisakiHaniShitei.Checked;
            this.fpTokuisakiListGrid.Enabled = this.radTokuisakiKobetsuShitei.Checked;

            // 得意先コード（範囲開始）が非活性の場合
            if (!this.numTokuisakiCodeFrom.Enabled)
            {
                // 得意先コード（範囲開始）空白化
                this.numTokuisakiCodeFrom.Tag = null;
                this.numTokuisakiCodeFrom.Value = null;
                this.edtTokuisakiNameFrom.Text = string.Empty;
            }
            else
            {
                // 得意先コード（範囲開始）初期化
                this.numTokuisakiCodeFrom.Tag = null;
                this.numTokuisakiCodeFrom.Value = TOKUI_CODEFROM_DEFAULT;
                this.edtTokuisakiNameFrom.Text = TOKUI_NAMEFROM_DEFAULT;
            }

            // 得意先コード（範囲終了）が非活性の場合
            if (!this.numTokuisakiCodeTo.Enabled)
            {
                // 得意先コード（範囲終了）空白化
                this.numTokuisakiCodeTo.Tag = null;
                this.numTokuisakiCodeTo.Value = null;
                this.edtTokuisakiNameTo.Text = string.Empty;
            }
            else
            {
                // 得意先コード（範囲終了）初期化
                this.numTokuisakiCodeTo.Tag = null;
                this.numTokuisakiCodeTo.Value = TOKUI_CODETO_DEFAULT;
                this.edtTokuisakiNameTo.Text = TOKUI_NAMETO_DEFAULT;
            }

            // 得意先リストが非活性の場合
            if(!this.fpTokuisakiListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpTokuisakiListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpTokuisakiListGrid.Sheets[0].Cells[i, COL_TOKUI_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpTokuisakiListGrid.Sheets[0].ClearSelection();
            }
        }

        /// <summary>
        /// 傭車指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeCarCondition()
        {
            this.numCarOfChartererCodeFrom.Enabled = this.radCarOfChartererHaniShitei.Checked;
            this.numCarOfChartererCodeTo.Enabled = this.radCarOfChartererHaniShitei.Checked;
            this.fpCarOfChartererListGrid.Enabled = this.radCarOfChartererKobetsuShitei.Checked;

            // 傭車コード（範囲開始）が非活性の場合
            if (!this.numCarOfChartererCodeFrom.Enabled)
            {
                // 傭車コード（範囲開始）空白化
                this.numCarOfChartererCodeFrom.Tag = null;
                this.numCarOfChartererCodeFrom.Value = null;
                this.edtCarOfChartererNameFrom.Text = string.Empty;
            }
            else
            {
                // 傭車コード（範囲開始）初期化
                this.numCarOfChartererCodeFrom.Tag = null;
                this.numCarOfChartererCodeFrom.Value = CAR_CODEFROM_DEFAULT;
                this.edtCarOfChartererNameFrom.Text = CAR_NAMEFROM_DEFAULT;
            }

            // 傭車コード（範囲終了）が非活性の場合
            if (!this.numCarOfChartererCodeTo.Enabled)
            {
                // 傭車コード（範囲終了）空白化
                this.numCarOfChartererCodeTo.Tag = null;
                this.numCarOfChartererCodeTo.Value = null;
                this.edtCarOfChartererNameTo.Text = string.Empty;
            }
            else
            {
                // 傭車コード（範囲終了）初期化
                this.numCarOfChartererCodeTo.Tag = null;
                this.numCarOfChartererCodeTo.Value = CAR_CODETO_DEFAULT;
                this.edtCarOfChartererNameTo.Text = CAR_NAMETO_DEFAULT;
            }

            // 傭車リストが非活性の場合
            if (!this.fpCarOfChartererListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpCarOfChartererListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpCarOfChartererListGrid.Sheets[0].Cells[i, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpCarOfChartererListGrid.Sheets[0].ClearSelection();
            }
        }

        /// <summary>
        /// 得意先リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckTokuisakiList(int rowIndex)
        {
            this.fpTokuisakiListGrid.Sheets[0].Cells[rowIndex, COL_TOKUI_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpTokuisakiListGrid.Sheets[0].Cells[rowIndex, COL_TOKUI_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 傭車リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckCarOfChartererList(int rowIndex)
        {
            this.fpCarOfChartererListGrid.Sheets[0].Cells[rowIndex, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpCarOfChartererListGrid.Sheets[0].Cells[rowIndex, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 得意先リストに値を設定します。
        /// </summary>
        private void SetTokuisakiListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitTokuisakiListSheet();

                //件数
                int rowCount = 0;

                IList<TokuisakiInfo> wk_list = null;

                if (this._YoshaIraishoConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._TokuiInfoList =
                        this._DalUtil.Tokuisaki.GetList();

                    //得意先リスト取得
                    wk_list = this._TokuiInfoList
                        .Where(x => x.ToraDONDisableFlag == false)
                        .ToList();

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    ////得意先リスト件数取得
                    rowCount = this._YoshaIraishoConditionInfo.YoshaIraishoConditionTokuisakiList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_TOKUILIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_TOKUILIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_TOKUILIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialTokuisakiStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._YoshaIraishoConditionInfo == null)
                    {
                        TokuisakiInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_TOKUI_CODE, wk_info.ToraDONTokuisakiCode);
                        datamodel.SetValue(i, COL_TOKUI_NAME, wk_info.ToraDONTokuisakiShortName);
                        datamodel.SetValue(i, COL_TOKUI_ID, wk_info.ToraDONTokuisakiId);
                    }
                    else
                    {
                        YoshaIraishoConditionTokuisakiInfo wk_info = this._YoshaIraishoConditionInfo.YoshaIraishoConditionTokuisakiList[i];

                        datamodel.SetValue(i, COL_TOKUI_NAME, wk_info.TokuisakiName);
                        datamodel.SetValue(i, COL_TOKUI_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_TOKUI_ID, wk_info.TokuisakiId);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpTokuisakiListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpTokuisakiListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpTokuisakiListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpTokuisakiListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 傭車リストに値を設定します。
        /// </summary>
        private void SetCarOfChartererListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitCarOfChartererListSheet();

                //件数
                int rowCount = 0;

                IList<CarInfo> wk_list = null;

                if (this._YoshaIraishoConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._CarInfoList =
                        this._DalUtil.Car.GetList();

                    //傭車リスト取得（傭車のみ）
                    wk_list = this._CarInfoList
                        .Where(x => x.ToraDONDisableFlag == false 
                            && x.CarKbn == (int)DefaultProperty.CarKbn.Yosha
                            && x.YoshasakiCode > 0)
                        .ToList();

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    //傭車リスト件数取得
                    rowCount = this._YoshaIraishoConditionInfo.YoshaIraishoConditionCarList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_CAR_OF_CHARTERER_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialCarStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._YoshaIraishoConditionInfo == null)
                    {
                        CarInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_CAR_OF_CHARTERER_CODE, wk_info.YoshasakiCode);
                        datamodel.SetValue(i, COL_CAR_OF_CHARTERER_NAME, wk_info.YoshasakiName);
                        datamodel.SetValue(i, COL_CAR_OF_CHARTERER_ID, wk_info.YoshasakiId);
                    }
                    else
                    {
                        YoshaIraishoConditionCarInfo wk_info = this._YoshaIraishoConditionInfo.YoshaIraishoConditionCarList[i];

                        datamodel.SetValue(i, COL_CAR_OF_CHARTERER_NAME, wk_info.YoshasakiName);
                        datamodel.SetValue(i, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_CAR_OF_CHARTERER_ID, wk_info.YoshasakiId);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpCarOfChartererListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpCarOfChartererListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpCarOfChartererListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpCarOfChartererListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 指定した印刷条件で印刷プレビューを表示します。
        /// </summary>
        /// <returns>処理結果</returns>
        private void DoPreview()
        {
            this.Print(ReportPrintDestType.PrintToScreen);
        }

        /// <summary>
        /// 指定した印刷条件でプレビューします。
        /// </summary>
        /// <returns>処理結果</returns>
        private void Print(ReportPrintDestType printDestType)
        {

            if (!this.ValidateChildren(ValidationConstraints.None))
                return;

            if (!this.CheckInputs())
                return;

            // ディレクトリ指定
            string fileName = ShowFileDialog();

            // パスを指定しない場合は終了
            if (string.IsNullOrWhiteSpace(fileName)) return;

            // 作成結果格納用
            bool createResult = false;

            // 画面を使用不可に
            this.ControlEnableChangeForPrinting(false);
            // 画面タイトルを変更
            this.Text = WINDOW_TITLE + "　" + "印刷中...";
            // 砂時計
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //印刷データを保持しているメンバをクリア
                this.ReportDataListClear();

                //画面から更新に必要な情報をメンバにセット
                this.GetFromScreen();

                //印刷用のデータを取得
                this.reportDataList = new YoshaIraishoPrt(this.appAuth).GetHaisha(this._YoshaIraishoConditionInfo);

                //配送指示書データが存在しない場合
                if (this.reportDataList == null
                    || this.reportDataList.Count == 0)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201015"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    return;
                }

                // Excel作成
                createResult = this.CreateYoshaIraisho(printDestType, fileName);

                // 操作ログの出力
                string log_joken = this.CreateLogText();

                //操作ログ出力
                switch (printDestType)
                {
                    case ReportPrintDestType.PrintToScreen:
                        //プレビュー時
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.PrintPreview,
                            log_joken);
                        break;
                    case ReportPrintDestType.PrintToPrinter:
                        ////直接印刷時時
                        //FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        //    FrameLogWriter.LoggingOperateKind.PrintPrinting,
                        //    log_joken);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // 画面を使用可能に
                this.ControlEnableChangeForPrinting(true);
                // 画面タイトルを変更する
                this.Text = WINDOW_TITLE;
                // デフォルトに戻す
                Cursor.Current = Cursors.Default;
                //条件項目にフォーカス
                this.SetFirstFocus();
            }

            #region 今回、直接印刷、保存は、プレビュー画面側で行う為コメントヘ

           
            #endregion
        }

        /// <summary>
        /// 配送指示書（Excel）を作成します。
        /// </summary>
        /// <param name="printDestType"></param>
        /// <param name="fileName">作成するファイル名（パス付き）</param>
        /// <returns></returns>
        private bool CreateYoshaIraisho(ReportPrintDestType printDestType, string fileName)
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
                vbrList = new YoshaIraisho(this.appAuth).YoshaIraishoData(vbrList, this.reportDataList, this._YoshaIraishoConditionInfo);

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

            // 日付（範囲開始）
            if (rt_val && this.HizukeYMDFrom == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { "日付（範囲開始）" });
                ctl = this.dteHizukeYMDFrom;
            }
            else
            {
                // 日付（範囲終了）
                if (rt_val && this.HizukeYMDTo == null)
                {
                    this.dteHizukeYMDTo.Value = this.HizukeYMDFrom.Value;
                }

                if (this.dteHizukeYMDFrom.Value.Value > this.dteHizukeYMDTo.Value.Value)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.dteHizukeYMDTo;
                }
                else
                {
                    //一週間の範囲
                    if (this.dteHizukeYMDFrom.Value.Value.AddDays(6) < this.dteHizukeYMDTo.Value.Value)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage("MW2203014", new string[] { "日付","１週間" });
                        ctl = this.dteHizukeYMDTo;
                    }
                }
            }

            // 得意先範囲指定の場合
            if (this.radTokuisakiHaniShitei.Checked)
            {
                if (rt_val && this.TokuisakiCodeTo < this.TokuisakiCodeFrom)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.numTokuisakiCodeTo;
                }
            }
            // 得意先個別指定の場合
            else if (rt_val)
            {
                int cnt = 0;
                //チェック件数取得
                for (int i = 0; i < this.fpTokuisakiListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpTokuisakiListGrid.Sheets[0].Cells[i, COL_TOKUI_SELECT_CHECKBOX].Value)
                        cnt++;
                }
                //チェックが0件の場合
                if (rt_val && cnt == 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { "得意先" });
                    ctl = this.fpTokuisakiListGrid;
                }
            }

            // 傭車範囲指定の場合
            if (this.radCarOfChartererHaniShitei.Checked)
            {
                if (rt_val && this.CarOfChartererCodeTo < this.CarOfChartererCodeFrom)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.numCarOfChartererCodeTo;
                }
            }
            // 傭車個別指定の場合
            else if (rt_val)
            {
                int cnt = 0;
                //チェック件数取得
                for (int i = 0; i < this.fpCarOfChartererListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpCarOfChartererListGrid.Sheets[0].Cells[i, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX].Value)
                        cnt++;
                }
                //チェックが0件の場合
                if (rt_val && cnt == 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { "傭車" });
                    ctl = this.fpCarOfChartererListGrid;
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

            this._AddPrintActionMenuItem.PrintActionMenuItemEnable(PrintActionMenuItems.PrintToScreen, val);
            this._AddPrintActionMenuItem.PrintActionMenuItemEnable(PrintActionMenuItems.Close, val);
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string CreateLogText()
        {
            return 
                this.HizukeString + "\r\n" +
                this.TokuisakiString + "\r\n" +
                this.CarString + "\r\n" +
                //this.KaishiMaeNissuString + "\r\n" +
                ""
                ;
        }

        /// <summary>
        /// 画面から更新に必要な情報をメンバにセットします。
        /// </summary>
        private void GetFromScreen()
        {
            this._YoshaIraishoConditionInfo = new YoshaIraishoPrtSearchParameter();

            this._YoshaIraishoConditionInfo.HizukeYMDFrom = this.HizukeYMDFrom.Value;
            this._YoshaIraishoConditionInfo.HizukeYMDTo = new DateTime(
                this.HizukeYMDTo.Value.Year
                , this.HizukeYMDTo.Value.Month
                , this.HizukeYMDTo.Value.Day
                , 23
                , 59
                , 59);

            //得意先印刷条件
            this._YoshaIraishoConditionInfo.Tokuisaki_HaniShiteiChecked = this.radTokuisakiHaniShitei.Checked;
            this._YoshaIraishoConditionInfo.Tokuisaki_KobetsuShiteiChecked = this.radTokuisakiKobetsuShitei.Checked;
            this._YoshaIraishoConditionInfo.TokuisakiCodeFrom = this.TokuisakiCodeFrom;
            this._YoshaIraishoConditionInfo.TokuisakiCodeTo = this.TokuisakiCodeTo;
            //得意先リスト
            IList<YoshaIraishoConditionTokuisakiInfo> tokuilist = new List<YoshaIraishoConditionTokuisakiInfo>();
            SheetView sheet0 = this.fpTokuisakiListGrid.ActiveSheet;

            //チェックボックスを全削除
            for (int i = 0; i < sheet0.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet0.GetValue(i, COL_TOKUI_SELECT_CHECKBOX)))
                {
                    YoshaIraishoConditionTokuisakiInfo info = new YoshaIraishoConditionTokuisakiInfo();
                    info.TokuisakiCode = Convert.ToInt32(
                        this.fpTokuisakiListGrid.Sheets[0].Cells[i, COL_TOKUI_CODE].Value);
                    info.TokuisakiName = Convert.ToString(
                        this.fpTokuisakiListGrid.Sheets[0].Cells[i, COL_TOKUI_NAME].Value);
                    info.CheckedFlag = Convert.ToBoolean(
                        this.fpTokuisakiListGrid.Sheets[0].Cells[i, COL_TOKUI_SELECT_CHECKBOX].Value);
                    info.TokuisakiId = Convert.ToDecimal(
                        this.fpTokuisakiListGrid.Sheets[0].Cells[i, COL_TOKUI_ID].Value);
                    tokuilist.Add(info);
                }
            }

            this._YoshaIraishoConditionInfo.YoshaIraishoConditionTokuisakiList = tokuilist;

            //傭車印刷条件
            this._YoshaIraishoConditionInfo.Car_HaniShiteiChecked = this.radCarOfChartererHaniShitei.Checked;
            this._YoshaIraishoConditionInfo.Car_KobetsuShiteiChecked = this.radCarOfChartererKobetsuShitei.Checked;
            this._YoshaIraishoConditionInfo.CarCodeFrom = this.CarOfChartererCodeFrom;
            this._YoshaIraishoConditionInfo.CarCodeTo = this.CarOfChartererCodeTo;
            //傭車リスト
            IList<YoshaIraishoConditionCarInfo> carList = new List<YoshaIraishoConditionCarInfo>();

            SheetView sheet1 = this.fpCarOfChartererListGrid.ActiveSheet;

            //チェックボックスを全削除
            for (int i = 0; i < sheet1.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet1.GetValue(i, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX)))
                {
                    YoshaIraishoConditionCarInfo info = new YoshaIraishoConditionCarInfo();
                    info.YoshasakiCode = Convert.ToInt32(
                        this.fpCarOfChartererListGrid.Sheets[0].Cells[i, COL_CAR_OF_CHARTERER_CODE].Value);
                    info.YoshasakiName = Convert.ToString(
                        this.fpCarOfChartererListGrid.Sheets[0].Cells[i, COL_CAR_OF_CHARTERER_NAME].Value);
                    info.CheckedFlag = Convert.ToBoolean(
                        this.fpCarOfChartererListGrid.Sheets[0].Cells[i, COL_CAR_OF_CHARTERER_SELECT_CHECKBOX].Value);
                    info.YoshasakiId = Convert.ToDecimal(
                        this.fpCarOfChartererListGrid.Sheets[0].Cells[i, COL_CAR_OF_CHARTERER_ID].Value);
                    carList.Add(info);
                }
            }

            this._YoshaIraishoConditionInfo.YoshaIraishoConditionCarList = carList;

            //備考
            this._YoshaIraishoConditionInfo.Biko01 = this.edtBiko01.Text;
            this._YoshaIraishoConditionInfo.Biko02 = this.edtBiko02.Text;
            this._YoshaIraishoConditionInfo.Biko03 = this.edtBiko03.Text;
            this._YoshaIraishoConditionInfo.Biko04 = this.edtBiko04.Text;
            this._YoshaIraishoConditionInfo.Biko05 = this.edtBiko05.Text;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
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
            if (this.ActiveControl == this.numTokuisakiCodeFrom
                || this.ActiveControl == this.numTokuisakiCodeTo)
            {
                this.ShowCmnSearchTokuisaki();
            }
            else if (this.ActiveControl == this.numCarOfChartererCodeFrom
                || this.ActiveControl == this.numCarOfChartererCodeTo)
            {
                this.ShowCmnSearchCar();
            }

        }

        /// <summary>
        /// 得意先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONTokuisakiCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 傭車検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCar()
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
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONTorihikiCode);

                    this.OnCmnSearchComplete();
                }
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
                //日付（範囲終了）に同日を設定
                this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value;
            }
            //日付（範囲開始）が空の場合
            else
            {
                //システム日付を設定
                this.dteHizukeYMDFrom.Value = DateTime.Today;
                this.dteHizukeYMDTo.Value = DateTime.Today;
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
                //システム日付を設定
                this.dteHizukeYMDTo.Value = DateTime.Today;
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
        /// 得意先コード（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCodeFrom(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.TokuisakiCodeFrom == 0)
                {
                    is_clear = true;
                    return;
                }

                TokuisakiInfo info =
                    this._DalUtil.Tokuisaki.GetInfo(this.TokuisakiCodeFrom);

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
                    this.numTokuisakiCodeFrom.Tag = info.ToraDONTokuisakiId;
                    this.edtTokuisakiNameFrom.Text = info.ToraDONTokuisakiShortName;

                    if (0 == Convert.ToDecimal(this.numTokuisakiCodeTo.Tag)
                        || this.TokuisakiCodeTo == TOKUI_CODEFROM_DEFAULT)
                    {
                        this.numTokuisakiCodeTo.Tag = info.ToraDONTokuisakiId;
                        this.numTokuisakiCodeTo.Value = info.ToraDONTokuisakiCode;
                        this.edtTokuisakiNameTo.Text = info.ToraDONTokuisakiShortName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numTokuisakiCodeFrom.Tag = null;
                    this.numTokuisakiCodeFrom.Value = TOKUI_CODEFROM_DEFAULT;
                    this.edtTokuisakiNameFrom.Text = TOKUI_NAMEFROM_DEFAULT;
                }
            }
        }

        /// <summary>
        /// 得意先コード（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCodeTo(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合、もしくは、コードが最大値の場合は抜ける
                if (this.TokuisakiCodeTo == 0
                    || this.TokuisakiCodeTo == TOKUI_CODETO_DEFAULT)
                {
                    is_clear = true;
                    return;
                }

                TokuisakiInfo info =
                    this._DalUtil.Tokuisaki.GetInfo(this.TokuisakiCodeTo);

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
                    this.numTokuisakiCodeTo.Tag = info.ToraDONTokuisakiId;
                    this.edtTokuisakiNameTo.Text = info.ToraDONTokuisakiShortName;

                    if (this.TokuisakiCodeTo < this.TokuisakiCodeFrom)
                    {
                        MessageBox.Show(
                            "開始＞終了です。",
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numTokuisakiCodeTo.Tag = null;
                    this.numTokuisakiCodeTo.Value = TOKUI_CODETO_DEFAULT;
                    this.edtTokuisakiNameTo.Text = TOKUI_NAMETO_DEFAULT;
                }
            }
        }

        /// <summary>
        /// 傭車コード（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarOfChartererCodeFrom(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.CarOfChartererCodeFrom == 0)
                {
                    is_clear = true;
                    return;
                }

                TorihikisakiSearchParameter para = new TorihikisakiSearchParameter();
                para.ToraDONTorihikiCode = Convert.ToInt32(this.CarOfChartererCodeFrom);
                TorihikisakiInfo info = _DalUtil.Torihikisaki.GetList(para, null).FirstOrDefault();

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
                    this.numCarOfChartererCodeFrom.Tag = info.ToraDONTorihikiId;
                    this.edtCarOfChartererNameFrom.Text = info.ToraDONTorihikiName;

                    if (0 == Convert.ToDecimal(this.numCarOfChartererCodeTo.Tag)
                        || this.CarOfChartererCodeTo == CAR_CODEFROM_DEFAULT)
                    {
                        this.numCarOfChartererCodeTo.Tag = info.ToraDONTorihikiId;
                        this.numCarOfChartererCodeTo.Value = info.ToraDONTorihikiCode;
                        this.edtCarOfChartererNameTo.Text = info.ToraDONTorihikiName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarOfChartererCodeFrom.Tag = null;
                    this.numCarOfChartererCodeFrom.Value = CAR_CODEFROM_DEFAULT;
                    this.edtCarOfChartererNameFrom.Text = CAR_NAMEFROM_DEFAULT;
                }
            }
        }

        /// <summary>
        /// 傭車コード（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarOfChartererCodeTo(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合、もしくは、コードが最大値の場合は抜ける
                if (this.CarOfChartererCodeTo == 0
                    || this.CarOfChartererCodeTo == CAR_CODETO_DEFAULT)
                {
                    is_clear = true;
                    return;
                }

                TorihikisakiSearchParameter para = new TorihikisakiSearchParameter();
                para.ToraDONTorihikiCode = Convert.ToInt32(this.CarOfChartererCodeTo);
                TorihikisakiInfo info = _DalUtil.Torihikisaki.GetList(para, null).FirstOrDefault();

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
                    this.numCarOfChartererCodeTo.Tag = info.ToraDONTorihikiId;
                    this.edtCarOfChartererNameTo.Text = info.ToraDONTorihikiName;

                    if (this.CarOfChartererCodeTo < this.CarOfChartererCodeFrom)
                    {
                        MessageBox.Show(
                            "開始＞終了です。",
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarOfChartererCodeTo.Tag = null;
                    this.numCarOfChartererCodeTo.Value = CAR_CODETO_DEFAULT;
                    this.edtCarOfChartererNameTo.Text = CAR_NAMETO_DEFAULT;
                }
            }
        }
        
        #endregion

        #region プロパティ

        /// <summary>
        /// 日付（範囲開始）の値を取得します。
        /// </summary>
        public DateTime? HizukeYMDFrom
        {
            get { return this.dteHizukeYMDFrom.Value.GetDate(); }
        }

        /// <summary>
        /// 日付（範囲終了）の値を取得します。
        /// </summary>
        public DateTime? HizukeYMDTo
        {
            get { return this.dteHizukeYMDTo.Value.GetDate(); }
        }
        
        /// <summary>
        /// 得意先コード（範囲開始）の値を取得します。
        /// </summary>
        private int TokuisakiCodeFrom
        {
            get { return Convert.ToInt32(this.numTokuisakiCodeFrom.Value); }
        }

        /// <summary>
        /// 得意先コード（範囲終了）の値を取得します。
        /// </summary>
        private int TokuisakiCodeTo
        {
            get { return Convert.ToInt32(this.numTokuisakiCodeTo.Value); }
        }

        /// <summary>
        /// 傭車コード（範囲開始）の値を取得します。
        /// </summary>
        private int CarOfChartererCodeFrom
        {
            get { return Convert.ToInt32(this.numCarOfChartererCodeFrom.Value); }
        }

        /// <summary>
        /// 傭車コード（範囲終了）の値を取得します。
        /// </summary>
        private int CarOfChartererCodeTo
        {
            get { return Convert.ToInt32(this.numCarOfChartererCodeTo.Value); }
        }

        /// <summary>
        /// 画面「日付」の条件指定を文字型で取得します。
        /// </summary>
        private String HizukeString
        {
            get { return string.Format("[日付] {0}～{1}", this.HizukeYMDFrom.Value.ToString("yyyy/MM/dd"), this.HizukeYMDTo.Value.ToString("yyyy/MM/dd")); }
        }

        /// <summary>
        /// 画面「得意先」の条件指定を文字型で取得します。
        /// </summary>
        private String TokuisakiString
        {
            get
            {
                string Tokuisakicodestring = string.Empty;
                if (this.radTokuisakiHaniShitei.Checked)
                {
                    Tokuisakicodestring = string.Format("範囲指定 {0}～{1}", this.TokuisakiCodeFrom, this.TokuisakiCodeTo);
                }
                else
                {
                    Tokuisakicodestring = string.Format("個別指定 {0}", string.Join(",", this._YoshaIraishoConditionInfo.YoshaIraishoConditionTokuisakiList.Select(x => x.TokuisakiCode).ToList()));
                }
                return string.Format("[得意先] {0}", Tokuisakicodestring);
            }
        }

        /// <summary>
        /// 画面「傭車」の条件指定を文字型で取得します。
        /// </summary>
        private String CarString
        {
            get
            {
                string Carcodestring = string.Empty;
                if (this.radCarOfChartererHaniShitei.Checked)
                {
                    Carcodestring = string.Format("範囲指定 {0}～{1}", this.CarOfChartererCodeFrom, this.CarOfChartererCodeTo);
                }
                else
                {
                    Carcodestring = string.Format("個別指定 {0}", string.Join(",", this._YoshaIraishoConditionInfo.YoshaIraishoConditionCarList.Select(x => x.YoshasakiCode).ToList()));
                }
                return string.Format("[傭車] {0}", Carcodestring);
            }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitYoshaIraishoFrame();
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

        private void YoshaIraishoFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void WK_YoshaIraishoFrame_KeyDown(object sender, KeyEventArgs e)
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

        private void sbtnTokuisakiCode_Click(object sender, EventArgs e)
        {
            // 得意先検索ボタン
            this.ShowCmnSearchTokuisaki();
        }

        private void sbtnCarCode_Click(object sender, EventArgs e)
        {
            // 傭車検索ボタン
            this.ShowCmnSearchCar();
        }

        private void numTokuisakiCodeFrom_Validating(object sender, CancelEventArgs e)
        {
            // 得意先コード（範囲開始）
            this.ValidateTokuisakiCodeFrom(e);
        }

        private void numTokuisakiCodeTo_Validating(object sender, CancelEventArgs e)
        {
            // 得意先コード（範囲終了）
            this.ValidateTokuisakiCodeTo(e);
        }

        private void numCarOfChartererCodeFrom_Validating(object sender, CancelEventArgs e)
        {
            // 傭車コード（範囲開始）
            this.ValidateCarOfChartererCodeFrom(e);
        }

        private void numCarOfChartererCodeTo_Validating(object sender, CancelEventArgs e)
        {
            // 傭車コード（範囲終了）
            this.ValidateCarOfChartererCodeTo(e);
        }

        private void radTokuisaki_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeTokuisakiCondition();
        }

        private void radCarOfCharterer_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCarCondition();
        }

        void dteHizukeYMDFrom_Down(object sender, EventArgs e)
        {
            this.dteHizukeYMDFrom_Spin(-1);
        }

        void dteHizukeYMDFrom_Up(object sender, EventArgs e)
        {
            this.dteHizukeYMDFrom_Spin(1);
        }

        void dteHizukeYMDTo_Down(object sender, EventArgs e)
        {
            this.dteHizukeYMDTo_Spin(-1);
        }

        void dteHizukeYMDTo_Up(object sender, EventArgs e)
        {
            this.dteHizukeYMDTo_Spin(1);
        }

        /// <summary>
        /// 日付＿開始Changeイベント
        /// </summary>
        private void dteHizukeYMDFrom_Spin(int d)
        {
            try
            {
                this.dteHizukeYMDFrom.Value = Convert.ToDateTime(this.dteHizukeYMDFrom.Value).AddDays(d);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 日付＿終了Changeイベント
        /// </summary>
        private void dteHizukeYMDTo_Spin(int d)
        {
            try
            {
                this.dteHizukeYMDTo.Value = Convert.ToDateTime(this.dteHizukeYMDTo.Value).AddDays(d);
            }
            catch
            {
            }
        }

        private void fpTokuisakiListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckTokuisakiList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpTokuisakiListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpCarOfChartererListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckCarOfChartererList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpCarOfChartererListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void fpTokuisakiListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckTokuisakiList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
        }

        private void fpCarOfChartererListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckCarOfChartererList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
        }

        private void numKaishiMaeDays_ValueChanged(object sender, EventArgs e)
        {
            //this.lblNissu.Text = (14 - Convert.ToInt32(this.numKaishiMaeNissu.Value)).ToString() + "日";
        }

        /// <summary>
        /// 出力先指定のダイアログ
        /// </summary>
        /// <returns></returns>
        private string ShowFileDialog()
        {
            //SaveFileDialogクラスのインスタンスを作成
            SaveFileDialog sfd = new SaveFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            sfd.FileName = "傭車依頼書_" + dteHizukeYMDFrom.Value.ToString("yyyyMMdd") + "_" + dteHizukeYMDTo.Value.ToString("yyyyMMdd") + ".xlsx";
            //はじめに表示されるフォルダを指定する
            sfd.InitialDirectory =  System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            sfd.Filter = "Excelブック(*.xlsx)|*.xlsx|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに選択されるものを指定する
            sfd.FilterIndex = 1;
            //タイトルを設定する
            sfd.Title = "保存先のファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            sfd.RestoreDirectory = true;
            //既に存在するファイル名を指定したとき警告する
            //デフォルトでTrueなので指定する必要はない
            sfd.OverwritePrompt = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            sfd.CheckPathExists = true;

            //ダイアログを表示する
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }

            return string.Empty;
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
