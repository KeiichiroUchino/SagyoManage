using AdvanceSoftware.VBReport8;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.ReportDAL;
using Jpsys.HaishaManageV10.ReportModel;
using Jpsys.HaishaManageV10.VBReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaisoShijiShoPrtToFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配送指示書";

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
        private HaisoShijiShoRptSearchParameter _HaisoShijiShoConditionInfo = null;

        /// <summary>
        /// 取得した配送指示書情報を保持しておく領域
        /// </summary>
        private List<HaisoShijiShoRptInfo> reportDataList;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを
        /// 利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        #region 乗務員リスト

        //--Spread列定義

        /// <summary>
        /// 乗務員コード列番号
        /// </summary>
        private const int COL_STAFF_CODE = 0;

        /// <summary>
        /// 乗務員名列番号
        /// </summary>
        private const int COL_STAFF_NAME = 1;

        /// <summary>
        /// 乗務員カナ名列番号
        /// </summary>
        private const int COL_STAFF_KNAME = 2;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_STAFF_SELECT_CHECKBOX = 3;

        /// <summary>
        /// 乗務員Id列番号
        /// </summary>
        private const int COL_STAFF_ID = 4;

        /// <summary>
        /// 乗務員リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_STAFFLIST = 5;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialStaffStyleInfoArr;

        /// <summary>
        /// 乗務員情報のリストを保持する領域
        /// </summary>
        private IList<StaffInfo> _StaffInfoList = null;

        /// <summary>
        /// 乗務員コード（範囲終了）初期値
        /// </summary>
        private const decimal STAFF_CODEFROM_DEFAULT = 0;

        /// <summary>
        /// 乗務員コード（範囲終了）初期値
        /// </summary>
        private const decimal STAFF_CODETO_DEFAULT = 99999;

        /// <summary>
        /// 乗務員名（範囲開始）初期値
        /// </summary>
        private const string STAFF_NAMEFROM_DEFAULT = "***** 最初から *****";

        /// <summary>
        /// 乗務員コード（範囲終了）初期値
        /// </summary>
        private const string STAFF_NAMETO_DEFAULT = "***** 最後まで *****";

        #endregion

        #region 車種リスト

        //--Spread列定義

        /// <summary>
        /// 車種コード列番号
        /// </summary>
        private const int COL_CARKIND_CODE = 0;

        /// <summary>
        /// 車種名列番号
        /// </summary>
        private const int COL_CARKIND_NAME = 1;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_CARKIND_SELECT_CHECKBOX = 2;

        /// <summary>
        /// 車種Id列番号
        /// </summary>
        private const int COL_CARKIND_ID = 3;

        /// <summary>
        /// 車種リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_CARKINDLIST = 4;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialCarKindStyleInfoArr;

        /// <summary>
        /// 車種情報のリストを保持する領域
        /// </summary>
        private IList<CarKindInfo> _CarKindInfoList = null;

        /// <summary>
        /// 車種コード（範囲終了）初期値
        /// </summary>
        private const decimal CARKIND_CODEFROM_DEFAULT = 0;

        /// <summary>
        /// 車種コード（範囲終了）初期値
        /// </summary>
        private const decimal CARKIND_CODETO_DEFAULT = 999;

        /// <summary>
        /// 車種名（範囲開始）初期値
        /// </summary>
        private const string CARKIND_NAMEFROM_DEFAULT = "***** 最初から *****";

        /// <summary>
        /// 車種コード（範囲終了）初期値
        /// </summary>
        private const string CARKIND_NAMETO_DEFAULT = "***** 最後まで *****";

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
        public HaisoShijiShoPrtToFrame()
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
        private void InitHaisoShijiShoFrame()
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

            // 乗務員リストのセット
            this.SetStaffListSheet();

            // 車種リストのセット
            this.SetCarKindListSheet();

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
                this.numStaffCodeFrom,
                this.numStaffCodeTo,
                this.numCarKindCodeFrom,
                this.numCarKindCodeTo
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
                ControlValidatingEventRaiser.Create(this.numStaffCodeFrom, ctl => ctl.Text, this.numStaffCodeFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numStaffCodeTo, ctl => ctl.Text, this.numStaffCodeTo_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCodeFrom, ctl => ctl.Text, this.numCarKindCodeFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCodeTo, ctl => ctl.Text, this.numCarKindCodeTo_Validating));
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
            // 乗務員リストスタイル情報初期化
            this.InitStaffStyleInfo();
            // 車種リストスタイル情報初期化
            this.InitCarKindStyleInfo();
        }

        /// <summary>
        /// 乗務員リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStaffStyleInfo()
        {
            // Id列非表示
            this.fpStaffListGrid.Sheets[0].Columns[COL_STAFF_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpStaffListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialStaffStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_STAFFLIST];

            for (int i = 0; i < COL_MAXCOLNUM_STAFFLIST; i++)
            {
                this.initialStaffStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// 車種リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitCarKindStyleInfo()
        {
            // Id列非表示
            this.fpCarKindListGrid.Sheets[0].Columns[COL_CARKIND_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpCarKindListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialCarKindStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_CARKINDLIST];

            for (int i = 0; i < COL_MAXCOLNUM_CARKINDLIST; i++)
            {
                this.initialCarKindStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //乗務員リストの初期化
            this.InitStaffListSheet();

            //車種リストの初期化
            this.InitCarKindListSheet();
        }

        /// <summary>
        /// 乗務員リストを初期化します。
        /// </summary>
        private void InitStaffListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpStaffListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_STAFFLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 車種リストを初期化します。
        /// </summary>
        private void InitCarKindListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpCarKindListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_CARKINDLIST);
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
            this.edtAttentions.Text = string.Empty;

            //乗務員印刷条件
            this.radStaffHaniShitei.Checked = true;
            this.numStaffCodeFrom.Tag = null;
            this.numStaffCodeFrom.Value = STAFF_CODEFROM_DEFAULT;
            this.edtStaffNameFrom.Text = STAFF_NAMEFROM_DEFAULT;
            this.numStaffCodeTo.Tag = null;
            this.numStaffCodeTo.Value = STAFF_CODETO_DEFAULT;
            this.edtStaffNameTo.Text = STAFF_NAMETO_DEFAULT;

            //車種印刷条件
            this.radCarKindHaniShitei.Checked = true;
            this.numCarKindCodeFrom.Tag = null;
            this.numCarKindCodeFrom.Value = CARKIND_CODEFROM_DEFAULT;
            this.edtCarKindNameFrom.Text = CARKIND_NAMEFROM_DEFAULT;
            this.numCarKindCodeTo.Tag = null;
            this.numCarKindCodeTo.Value = CARKIND_CODETO_DEFAULT;
            this.edtCarKindNameTo.Text = CARKIND_NAMETO_DEFAULT;

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
                this.reportDataList = new List<HaisoShijiShoRptInfo>();
            }
        }

        #endregion

        #region プライベート メソッド

        /// <summary>コントロールの活性／非活性を初期化します。
        /// </summary>
        private void InitControlsEnabled()
        {
            this.ChangeStaffCondition();
            this.ChangeCarKindCondition();
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
        /// 乗務員指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeStaffCondition()
        {
            this.numStaffCodeFrom.Enabled = this.radStaffHaniShitei.Checked;
            this.numStaffCodeTo.Enabled = this.radStaffHaniShitei.Checked;
            this.fpStaffListGrid.Enabled = this.radStaffKobetsuShitei.Checked;

            // 乗務員コード（範囲開始）が非活性の場合
            if (!this.numStaffCodeFrom.Enabled)
            {
                // 乗務員コード（範囲開始）空白化
                this.numStaffCodeFrom.Tag = null;
                this.numStaffCodeFrom.Value = null;
                this.edtStaffNameFrom.Text = string.Empty;
            }
            else
            {
                // 乗務員コード（範囲開始）初期化
                this.numStaffCodeFrom.Tag = null;
                this.numStaffCodeFrom.Value = STAFF_CODEFROM_DEFAULT;
                this.edtStaffNameFrom.Text = STAFF_NAMEFROM_DEFAULT;
            }

            // 乗務員コード（範囲終了）が非活性の場合
            if (!this.numStaffCodeTo.Enabled)
            {
                // 乗務員コード（範囲終了）空白化
                this.numStaffCodeTo.Tag = null;
                this.numStaffCodeTo.Value = null;
                this.edtStaffNameTo.Text = string.Empty;
            }
            else
            {
                // 乗務員コード（範囲終了）初期化
                this.numStaffCodeTo.Tag = null;
                this.numStaffCodeTo.Value = STAFF_CODETO_DEFAULT;
                this.edtStaffNameTo.Text = STAFF_NAMETO_DEFAULT;
            }

            // 乗務員リストが非活性の場合
            if(!this.fpStaffListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpStaffListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpStaffListGrid.Sheets[0].ClearSelection();
            }
        }

        /// <summary>
        /// 車種指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeCarKindCondition()
        {
            this.numCarKindCodeFrom.Enabled = this.radCarKindHaniShitei.Checked;
            this.numCarKindCodeTo.Enabled = this.radCarKindHaniShitei.Checked;
            this.fpCarKindListGrid.Enabled = this.radCarKindKobetsuShitei.Checked;

            // 車種コード（範囲開始）が非活性の場合
            if (!this.numCarKindCodeFrom.Enabled)
            {
                // 車種コード（範囲開始）空白化
                this.numCarKindCodeFrom.Tag = null;
                this.numCarKindCodeFrom.Value = null;
                this.edtCarKindNameFrom.Text = string.Empty;
            }
            else
            {
                // 車種コード（範囲開始）初期化
                this.numCarKindCodeFrom.Tag = null;
                this.numCarKindCodeFrom.Value = CARKIND_CODEFROM_DEFAULT;
                this.edtCarKindNameFrom.Text = CARKIND_NAMEFROM_DEFAULT;
            }

            // 車種コード（範囲終了）が非活性の場合
            if (!this.numCarKindCodeTo.Enabled)
            {
                // 車種コード（範囲終了）空白化
                this.numCarKindCodeTo.Tag = null;
                this.numCarKindCodeTo.Value = null;
                this.edtCarKindNameTo.Text = string.Empty;
            }
            else
            {
                // 車種コード（範囲終了）初期化
                this.numCarKindCodeTo.Tag = null;
                this.numCarKindCodeTo.Value = CARKIND_CODETO_DEFAULT;
                this.edtCarKindNameTo.Text = CARKIND_NAMETO_DEFAULT;
            }

            // 車種リストが非活性の場合
            if (!this.fpCarKindListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpCarKindListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpCarKindListGrid.Sheets[0].Cells[i, COL_CARKIND_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpCarKindListGrid.Sheets[0].ClearSelection();
            }
        }

        /// <summary>
        /// 乗務員リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckStaffList(int rowIndex)
        {
            this.fpStaffListGrid.Sheets[0].Cells[rowIndex, COL_STAFF_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpStaffListGrid.Sheets[0].Cells[rowIndex, COL_STAFF_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 車種リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckCarKindList(int rowIndex)
        {
            this.fpCarKindListGrid.Sheets[0].Cells[rowIndex, COL_CARKIND_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpCarKindListGrid.Sheets[0].Cells[rowIndex, COL_CARKIND_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 乗務員リストに値を設定します。
        /// </summary>
        private void SetStaffListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitStaffListSheet();

                //件数
                int rowCount = 0;

                IList<StaffInfo> wk_list = null;

                if (this._HaisoShijiShoConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._StaffInfoList =
                        this._DalUtil.Staff.GetList(new StaffSearchParameter() { DriverFlag = true });

                    //乗務員リスト取得
                    wk_list = this._StaffInfoList
                        .Where(x => x.ToraDONDisableFlag == false)
                        .ToList();

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    ////乗務員リスト件数取得
                    rowCount = this._HaisoShijiShoConditionInfo.HaisoShijishoConditionStaffList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_STAFFLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_STAFFLIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_STAFFLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStaffStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._HaisoShijiShoConditionInfo == null)
                    {
                        StaffInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_STAFF_CODE, wk_info.ToraDONStaffCode);
                        datamodel.SetValue(i, COL_STAFF_NAME, wk_info.ToraDONStaffName);
                        datamodel.SetValue(i, COL_STAFF_KNAME, wk_info.ToraDONStaffNameKana);
                        datamodel.SetValue(i, COL_STAFF_ID, wk_info.ToraDONStaffId);
                    }
                    else
                    {
                        HaisoShijishoConditionStaffInfo wk_info = this._HaisoShijiShoConditionInfo.HaisoShijishoConditionStaffList[i];

                        datamodel.SetValue(i, COL_STAFF_NAME, wk_info.StaffName);
                        datamodel.SetValue(i, COL_STAFF_KNAME, wk_info.StaffNameKana);
                        datamodel.SetValue(i, COL_STAFF_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_STAFF_ID, wk_info.StaffId);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpStaffListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpStaffListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpStaffListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpStaffListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 車種リストに値を設定します。
        /// </summary>
        private void SetCarKindListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitCarKindListSheet();

                //件数
                int rowCount = 0;

                IList<CarKindInfo> wk_list = null;

                if (this._HaisoShijiShoConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._CarKindInfoList =
                        this._DalUtil.CarKind.GetList();

                    //車種リスト取得
                    wk_list = this._CarKindInfoList
                        .Where(x => x.ToraDONDisableFlag == false)
                        .ToList();

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    //車種リスト件数取得
                    rowCount = this._HaisoShijiShoConditionInfo.HaisoShijishoConditionCarKindList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_CARKINDLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_CARKINDLIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_CARKINDLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialCarKindStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._HaisoShijiShoConditionInfo == null)
                    {
                        CarKindInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_CARKIND_CODE, wk_info.ToraDONCarKindCode);
                        datamodel.SetValue(i, COL_CARKIND_NAME, wk_info.ToraDONCarKindName);
                        datamodel.SetValue(i, COL_CARKIND_ID, wk_info.ToraDONCarKindId);
                    }
                    else
                    {
                        HaisoShijishoConditionCarKindInfo wk_info = this._HaisoShijiShoConditionInfo.HaisoShijishoConditionCarKindList[i];

                        datamodel.SetValue(i, COL_CARKIND_NAME, wk_info.CarKindName);
                        datamodel.SetValue(i, COL_CARKIND_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_CARKIND_ID, wk_info.CarKindId);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpCarKindListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpCarKindListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpCarKindListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpCarKindListGrid.ShowActiveCell(
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
                this.reportDataList = new HaisoShijiShoRpt(this.appAuth).GetHaisha(this._HaisoShijiShoConditionInfo);

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

                //Excel作成
                createResult = this.CreateHaisoShijiSho(printDestType, fileName);

                //操作ログ取得
                string log_joken = this.CreateLogText();

                //操作ログ出力
                //プレビュー時
                FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                    FrameLogWriter.LoggingOperateKind.PrintPreview,
                    log_joken);
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
        }

        /// <summary>
        /// 配送指示書（Excel）を作成します。
        /// </summary>
        /// <param name="printDestType"></param>
        /// <param name="fileName">作成するファイル名（パス付き）</param>
        /// <returns></returns>
        private bool CreateHaisoShijiSho(ReportPrintDestType printDestType, string fileName)
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
                vbrList = new HaisoShijiSho().HaisoShijiShoData(vbrList, this.reportDataList, this._HaisoShijiShoConditionInfo);

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

            // 乗務員範囲指定の場合
            if (this.radStaffHaniShitei.Checked)
            {
                if (rt_val && this.StaffCodeTo < this.StaffCodeFrom)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.numStaffCodeTo;
                }
            }
            // 乗務員個別指定の場合
            else if (rt_val)
            {
                int cnt = 0;
                //チェック件数取得
                for (int i = 0; i < this.fpStaffListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_SELECT_CHECKBOX].Value)
                        cnt++;
                }
                //チェックが0件の場合
                if (rt_val && cnt == 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { "乗務員" });
                    ctl = this.fpStaffListGrid;
                }
            }

            // 車種範囲指定の場合
            if (this.radCarKindHaniShitei.Checked)
            {
                if (rt_val && this.CarKindCodeTo < this.CarKindCodeFrom)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.numCarKindCodeTo;
                }
            }
            // 車種個別指定の場合
            else if (rt_val)
            {
                int cnt = 0;
                //チェック件数取得
                for (int i = 0; i < this.fpCarKindListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpCarKindListGrid.Sheets[0].Cells[i, COL_CARKIND_SELECT_CHECKBOX].Value)
                        cnt++;
                }
                //チェックが0件の場合
                if (rt_val && cnt == 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { "車種" });
                    ctl = this.fpCarKindListGrid;
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
                this.StaffString + "\r\n" +
                this.CarKindString + "\r\n" +
                ""
                ;
        }

        /// <summary>
        /// 画面から更新に必要な情報をメンバにセットします。
        /// </summary>
        private void GetFromScreen()
        {
            this._HaisoShijiShoConditionInfo = new HaisoShijiShoRptSearchParameter();

            this._HaisoShijiShoConditionInfo.HizukeYMDFrom = this.HizukeYMDFrom.Value;
            this._HaisoShijiShoConditionInfo.HizukeYMDTo = new DateTime(
                this.HizukeYMDTo.Value.Year
                , this.HizukeYMDTo.Value.Month
                , this.HizukeYMDTo.Value.Day
                , 23
                , 59
                , 59);
            this._HaisoShijiShoConditionInfo.Attentions = this.edtAttentions.Text;

            //乗務員印刷条件
            this._HaisoShijiShoConditionInfo.Staff_HaniShiteiChecked = this.radStaffHaniShitei.Checked;
            this._HaisoShijiShoConditionInfo.Staff_KobetsuShiteiChecked = this.radStaffKobetsuShitei.Checked;
            this._HaisoShijiShoConditionInfo.StaffCodeFrom = this.StaffCodeFrom;
            this._HaisoShijiShoConditionInfo.StaffCodeTo = this.StaffCodeTo;
            //乗務員リスト
            IList<HaisoShijishoConditionStaffInfo> stafflist = new List<HaisoShijishoConditionStaffInfo>();
            SheetView sheet0 = this.fpStaffListGrid.ActiveSheet;

            //チェックボックスを全削除
            for (int i = 0; i < sheet0.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet0.GetValue(i, COL_STAFF_SELECT_CHECKBOX)))
                {
                    HaisoShijishoConditionStaffInfo info = new HaisoShijishoConditionStaffInfo();
                    info.StaffCode = Convert.ToInt32(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_CODE].Value);
                    info.StaffName = Convert.ToString(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_NAME].Value);
                    info.StaffNameKana = Convert.ToString(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_KNAME].Value);
                    info.CheckedFlag = Convert.ToBoolean(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_SELECT_CHECKBOX].Value);
                    info.StaffId = Convert.ToDecimal(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_STAFF_ID].Value);
                    stafflist.Add(info);
                }
            }

            this._HaisoShijiShoConditionInfo.HaisoShijishoConditionStaffList = stafflist;

            //車種印刷条件
            this._HaisoShijiShoConditionInfo.CarKind_HaniShiteiChecked = this.radCarKindHaniShitei.Checked;
            this._HaisoShijiShoConditionInfo.CarKind_KobetsuShiteiChecked = this.radCarKindKobetsuShitei.Checked;
            this._HaisoShijiShoConditionInfo.CarKindCodeFrom = this.CarKindCodeFrom;
            this._HaisoShijiShoConditionInfo.CarKindCodeTo = this.CarKindCodeTo;
            //車種リスト
            IList<HaisoShijishoConditionCarKindInfo> carkindlist = new List<HaisoShijishoConditionCarKindInfo>();

            SheetView sheet1 = this.fpCarKindListGrid.ActiveSheet;

            //チェックボックスを全削除
            for (int i = 0; i < sheet1.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet1.GetValue(i, COL_CARKIND_SELECT_CHECKBOX)))
                {
                    HaisoShijishoConditionCarKindInfo info = new HaisoShijishoConditionCarKindInfo();
                    info.CarKindCode = Convert.ToInt32(
                        this.fpCarKindListGrid.Sheets[0].Cells[i, COL_CARKIND_CODE].Value);
                    info.CarKindName = Convert.ToString(
                        this.fpCarKindListGrid.Sheets[0].Cells[i, COL_CARKIND_NAME].Value);
                    info.CheckedFlag = Convert.ToBoolean(
                        this.fpCarKindListGrid.Sheets[0].Cells[i, COL_CARKIND_SELECT_CHECKBOX].Value);
                    info.CarKindId = Convert.ToDecimal(
                        this.fpCarKindListGrid.Sheets[0].Cells[i, COL_CARKIND_ID].Value);
                    carkindlist.Add(info);
                }
            }

            this._HaisoShijiShoConditionInfo.HaisoShijishoConditionCarKindList = carkindlist;
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
            if (this.ActiveControl == this.numStaffCodeFrom
                || this.ActiveControl == this.numStaffCodeTo)
            {
                this.ShowCmnSearchDriver();
            }
            else if (this.ActiveControl == this.numCarKindCodeFrom
                || this.ActiveControl == this.numCarKindCodeTo)
            {
                this.ShowCmnSearchCarKind();
            }

        }

        /// <summary>
        /// 乗務員検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchDriver()
        {
            using (CmnSearchDriverFrame f = new CmnSearchDriverFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONStaffCode);

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
                f.ShowAllFlag = true;

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
        /// 乗務員コード（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateStaffCodeFrom(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.StaffCodeFrom == 0)
                {
                    is_clear = true;
                    return;
                }

                StaffInfo info =
                    this._DalUtil.Staff.GetInfo(this.StaffCodeFrom);

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
                    this.numStaffCodeFrom.Tag = info.ToraDONStaffId;
                    this.edtStaffNameFrom.Text = info.ToraDONStaffName;

                    if (0 == Convert.ToDecimal(this.numStaffCodeTo.Tag)
                        || this.StaffCodeTo == STAFF_CODEFROM_DEFAULT)
                    {
                        this.numStaffCodeTo.Tag = info.ToraDONStaffId;
                        this.numStaffCodeTo.Value = info.ToraDONStaffCode;
                        this.edtStaffNameTo.Text = info.ToraDONStaffName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numStaffCodeFrom.Tag = null;
                    this.numStaffCodeFrom.Value = STAFF_CODEFROM_DEFAULT;
                    this.edtStaffNameFrom.Text = STAFF_NAMEFROM_DEFAULT;
                }
            }
        }

        /// <summary>
        /// 乗務員コード（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateStaffCodeTo(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合、もしくは、コードが最大値の場合は抜ける
                if (this.StaffCodeTo == 0
                    || this.StaffCodeTo == STAFF_CODETO_DEFAULT)
                {
                    is_clear = true;
                    return;
                }

                StaffInfo info =
                    this._DalUtil.Staff.GetInfo(this.StaffCodeTo);

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
                    this.numStaffCodeTo.Tag = info.ToraDONStaffId;
                    this.edtStaffNameTo.Text = info.ToraDONStaffName;

                    if (this.StaffCodeTo < this.StaffCodeFrom)
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
                    this.numStaffCodeTo.Tag = null;
                    this.numStaffCodeTo.Value = STAFF_CODETO_DEFAULT;
                    this.edtStaffNameTo.Text = STAFF_NAMETO_DEFAULT;
                }
            }
        }

        /// <summary>
        /// 車種コード（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindCodeFrom(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.CarKindCodeFrom == 0)
                {
                    is_clear = true;
                    return;
                }

                CarKindInfo info =
                    this._DalUtil.CarKind.GetInfo(this.CarKindCodeFrom);

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
                    this.numCarKindCodeFrom.Tag = info.ToraDONCarKindId;
                    this.edtCarKindNameFrom.Text = info.ToraDONCarKindName;

                    if (0 == Convert.ToDecimal(this.numCarKindCodeTo.Tag)
                        || this.CarKindCodeTo == CARKIND_CODEFROM_DEFAULT)
                    {
                        this.numCarKindCodeTo.Tag = info.ToraDONCarKindId;
                        this.numCarKindCodeTo.Value = info.ToraDONCarKindCode;
                        this.edtCarKindNameTo.Text = info.ToraDONCarKindName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarKindCodeFrom.Tag = null;
                    this.numCarKindCodeFrom.Value = CARKIND_CODEFROM_DEFAULT;
                    this.edtCarKindNameFrom.Text = CARKIND_NAMEFROM_DEFAULT;
                }
            }
        }

        /// <summary>
        /// 車種コード（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindCodeTo(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合、もしくは、コードが最大値の場合は抜ける
                if (this.CarKindCodeTo == 0
                    || this.CarKindCodeTo == CARKIND_CODETO_DEFAULT)
                {
                    is_clear = true;
                    return;
                }

                CarKindInfo info =
                    this._DalUtil.CarKind.GetInfo(this.CarKindCodeTo);

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
                    this.numCarKindCodeTo.Tag = info.ToraDONCarKindId;
                    this.edtCarKindNameTo.Text = info.ToraDONCarKindName;

                    if (this.CarKindCodeTo < this.CarKindCodeFrom)
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
                    this.numCarKindCodeTo.Tag = null;
                    this.numCarKindCodeTo.Value = CARKIND_CODETO_DEFAULT;
                    this.edtCarKindNameTo.Text = CARKIND_NAMETO_DEFAULT;
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
        /// 乗務員コード（範囲開始）の値を取得します。
        /// </summary>
        private int StaffCodeFrom
        {
            get { return Convert.ToInt32(this.numStaffCodeFrom.Value); }
        }

        /// <summary>
        /// 乗務員コード（範囲終了）の値を取得します。
        /// </summary>
        private int StaffCodeTo
        {
            get { return Convert.ToInt32(this.numStaffCodeTo.Value); }
        }

        /// <summary>
        /// 車種コード（範囲開始）の値を取得します。
        /// </summary>
        private int CarKindCodeFrom
        {
            get { return Convert.ToInt32(this.numCarKindCodeFrom.Value); }
        }

        /// <summary>
        /// 車種コード（範囲終了）の値を取得します。
        /// </summary>
        private int CarKindCodeTo
        {
            get { return Convert.ToInt32(this.numCarKindCodeTo.Value); }
        }

        /// <summary>
        /// 画面「日付」の条件指定を文字型で取得します。
        /// </summary>
        private String HizukeString
        {
            get { return string.Format("[日付] {0}～{1}", this.HizukeYMDFrom.Value.ToString("yyyy/MM/dd"), this.HizukeYMDTo.Value.ToString("yyyy/MM/dd")); }
        }

        /// <summary>
        /// 画面「乗務員」の条件指定を文字型で取得します。
        /// </summary>
        private String StaffString
        {
            get
            {
                string Staffcodestring = string.Empty;
                if (this.radStaffHaniShitei.Checked)
                {
                    Staffcodestring = string.Format("範囲指定 {0}～{1}", this.StaffCodeFrom, this.StaffCodeTo);
                }
                else
                {
                    Staffcodestring = string.Format("個別指定 {0}", string.Join(",", this._HaisoShijiShoConditionInfo.HaisoShijishoConditionStaffList.Select(x => x.StaffCode).ToList()));
                }
                return string.Format("[乗務員] {0}", Staffcodestring);
            }
        }

        /// <summary>
        /// 画面「車種」の条件指定を文字型で取得します。
        /// </summary>
        private String CarKindString
        {
            get
            {
                string CarKindcodestring = string.Empty;
                if (this.radCarKindHaniShitei.Checked)
                {
                    CarKindcodestring = string.Format("範囲指定 {0}～{1}", this.CarKindCodeFrom, this.CarKindCodeTo);
                }
                else
                {
                    CarKindcodestring = string.Format("個別指定 {0}", string.Join(",", this._HaisoShijiShoConditionInfo.HaisoShijishoConditionCarKindList.Select(x => x.CarKindCode).ToList()));
                }
                return string.Format("[車種] {0}", CarKindcodestring);
            }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaisoShijiShoFrame();
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

        private void HaisoShijiShoFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void WK_HaisoShijiShoFrame_KeyDown(object sender, KeyEventArgs e)
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

        private void sbtnStaffCode_Click(object sender, EventArgs e)
        {
            // 乗務員検索ボタン
            this.ShowCmnSearchDriver();
        }

        private void sbtnCarKindCode_Click(object sender, EventArgs e)
        {
            // 車種検索ボタン
            this.ShowCmnSearchCarKind();
        }

        private void numStaffCodeFrom_Validating(object sender, CancelEventArgs e)
        {
            // 乗務員コード（範囲開始）
            this.ValidateStaffCodeFrom(e);
        }

        private void numStaffCodeTo_Validating(object sender, CancelEventArgs e)
        {
            // 乗務員コード（範囲終了）
            this.ValidateStaffCodeTo(e);
        }

        private void numCarKindCodeFrom_Validating(object sender, CancelEventArgs e)
        {
            // 車種コード（範囲開始）
            this.ValidateCarKindCodeFrom(e);
        }

        private void numCarKindCodeTo_Validating(object sender, CancelEventArgs e)
        {
            // 車種コード（範囲終了）
            this.ValidateCarKindCodeTo(e);
        }

        private void radStaff_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStaffCondition();
        }

        private void radCarKind_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCarKindCondition();
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

        private void fpStaffListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckStaffList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpStaffListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpCarKindListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckCarKindList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpCarKindListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void fpStaffListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckStaffList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
        }

        private void fpCarKindListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckCarKindList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
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
            sfd.FileName = "配送指示書_" + this.dteHizukeYMDFrom.Value.ToString("yyyyMMdd") + "_" + this.dteHizukeYMDTo.Value.ToString("yyyyMMdd") + ".xlsx";
            //はじめに表示されるフォルダを指定する
            sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
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
