using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.MultiRow;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.BizProperty;
using GrapeCity.Win.MultiRow;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using System.ComponentModel;
using C1.C1Schedule;
using C1.Win.C1Schedule;
using System.Drawing;
using System.Text;
using jp.co.jpsys.util;
using System.Threading.Tasks;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishaNyuryokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaNyuryokuFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;

        #endregion

        #region 配車入力定義

        /// <summary>
        /// 取得した配車入力（抽出条件）を保持する領域
        /// </summary>
        public HaishaNyuryokuConditionInfo HaishaNyuryokuConditionInfo = null;

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配車入力";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 汎用データアクセスオブジェクト
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 自画面固有データアクセスクラス
        /// </summary>
        private HaishaNyuryoku _HaishaNyuryoku;

        /// <summary>
        /// 管理情報を保持する領域
        /// </summary>
        private KanriInfo _KanriInfo;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList =
            UserProperty.GetInstance().SystemNameList;

        /// <summary>
        /// 配車入力情報の更新情報を保持する。
        /// </summary>
        private Dictionary<decimal, HaishaNyuryokuInfo> _HaishaNyuryokuUpdata;

        /// <summary>
        /// 配車入力情報の追加情報を保持する。
        /// </summary>
        private Dictionary<decimal, HaishaNyuryokuInfo> _HaishaNyuryokuAdd;

        /// <summary>
        /// 配車入力情報の削除情報を保持する。
        /// </summary>
        private Dictionary<decimal, HaishaNyuryokuInfo> _HaishaNyuryokuDel;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// (false:終了確認しない)
        /// </summary>
        private bool isConfirmClose;

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        /// <summary>
        /// Category（JucCategory）を保持する領域
        /// </summary>
        private Category JucCategory;

        /// <summary>
        /// Category（配車情報）を保持する領域
        /// </summary>
        private Dictionary<decimal, Category> CarDictionary;

        /// <summary>
        /// Category（配車情報）の詳細情報を保持する領域
        /// </summary>
        private List<HaishaCarInfo> CarInfoDictionary;

        /// <summary>
        /// 受注一覧のページ数を保持する領域
        /// </summary>
        private int MaxPageNumber;

        /// <summary>
        /// 受注一覧の現在表示ページ数を保持する領域
        /// </summary>
        private int NowPageNumber;

        /// <summary>
        /// 追加中FLGを保持する領域
        /// </summary>
        private bool AddingFlg;

        /// <summary>
        /// 初期化FLGを保持する領域
        /// </summary>
        private bool InitFlg = true;

        /// <summary>
        /// 配車情報初期化FLGを保持する領域
        /// </summary>
        private bool InitHaishaFlg;

        /// <summary>
        /// マウスホイール動作中FLGを保持する領域
        /// </summary>
        private bool WheelScrollFlg;

        /// <summary>
        /// 配車IDの仮採番の情報を保持する領域
        /// </summary>
        private decimal HaishaIdNumbering;

        /// <summary>
        /// 受注一覧の時間範囲(受注一覧の1マスの立幅の指定に使用)
        /// </summary>
        private const int DEFAULT_JUC_DUARATION = 30;

        /// <summary>
        /// 受注一覧の表示件数
        /// </summary>
        private const int DEFAULT_JUC_DISPRESULTS = 12;

        /// <summary>
        /// 表示日数
        /// </summary>
        private const int DEFAULT_DISP_DAYS = 14;

        /// <summary>
        /// 休日区分配色文字色リストを保持する領域
        /// </summary>
        private SortedDictionary<int, Color> _KyujitsuForeColorDic = new SortedDictionary<int, Color>();

        /// <summary>
        /// 休日区分配色背景色リストを保持する領域
        /// </summary>
        private SortedDictionary<int, Color> _KyujitsuBackColorDic = new SortedDictionary<int, Color>();

        /// <summary>
        /// 配車情報検索結果リストを保持する領域
        /// </summary>
        private List<HaishaSearchResultInfo> _DataOfCurrentDay;

        /// <summary>
        /// 利用者補を保持する領域
        /// </summary>
        private HaitaOperatorExInfo OperatorExInfo;

        ///// <summary> TODO 削除予定
        ///// 管理マスタレコードを保持する。
        ///// </summary>
        //private ToraDonSystemPropertyInfo _ToraDonSystemPropertyInfo;

        /// <summary>
        /// 表示範囲の列挙型
        /// </summary>
        private enum DateRange
        {
            ThreeHours = 91,
            SixHours = 92,
            TwelveHours = 93,
            OneDay = 1,
            TwoDays = 2,
            ThreeDays = 3,
            FourDays = 4,
            FiveDays = 5,
            SevenDays = 7,
            FourteenDays = 14,
        }

        /// <summary>
        /// 予定区分の列挙型
        /// </summary>
        private enum AppointKbn
        {
            Editable = 1,
            NoEditable = 2,
            Kyujitsu = 3,
            Sakujo = 4,
        }

        /// <summary>
        /// スケジュール種類の列挙型
        /// </summary>
        private enum ScheduleType
        {
            Haisha = 1,
        }

        /// <summary>
        /// 処理モードの列挙型
        /// </summary>
        private enum HaishaNyuryokuProcessMode
        {
            JuchuSearch = 0,
            HaishaSearch = 1,
            Save = 2,
        }

        private C1.C1Schedule.Label HaisoLabel;       // 通常配送の表示に使うラベル
        private C1.C1Schedule.Label TsuikaHaisoLabel;  // 追加された配送の表示に使うラベル
        private C1.C1Schedule.Label ShuseiHaisoLabel; // 変更された配送の表示に使うラベル
        private C1.C1Schedule.Label DeleteHaisoLabel; // 削除された配送（受注）の表示に使うラベル
        private C1.C1Schedule.Label NotHaisoLabel; // 見配車の表示に使うラベル
        private C1.C1Schedule.Label NoEditLabel; // 編集不可の表示に使うラベル

        private Status StatusOKa; // 往荷ステータス
        private Status StatusFukuKa; // 復荷ステータス
        private Status StatusOfuku; // 往復ステータス
        private Status StatusNon; // 該当なしステータス

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitHaishaNyuryokuFrame()
        {
            // 画面タイトルの設定
            this.Text = WINDOW_TITLE;

            // 画面配色の初期化
            this.InitFrameColor();

            // 親フォームがあるときは、そのフォームを中心に表示するように変更
            if (null == this.ParentForm)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            // 画面最大化
            this.WindowState = FormWindowState.Maximized;

            // メニューの初期化
            this.InitMenuHaishaNyuryoku();

            // バインドの設定
            this.SettingCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // 認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 設定ステータス（往路・復路・往復）の設定
            this.StatusOKa = new Status(Color.Transparent, "往路", "", new C1Brush(Color.Blue));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusOKa);
            this.StatusFukuKa = new Status(Color.Transparent, "往路", "", new C1Brush(Color.Red));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusFukuKa);
            this.StatusNon = new Status(Color.Transparent, "該当無", "", new C1Brush(Color.White));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusNon);
            this.StatusOfuku = new Status(Color.Transparent, "往復", "", new C1Brush(Color.Yellow));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusOfuku);

            HaisoLabel = new C1.C1Schedule.Label(Color.Cornsilk, "配送", "配送キャプション");
            TsuikaHaisoLabel = new C1.C1Schedule.Label(Color.LightPink, "追加", "追加キャプション");
            ShuseiHaisoLabel = new C1.C1Schedule.Label(Color.PaleGreen, "変更有", "変更有キャプション");
            DeleteHaisoLabel = new C1.C1Schedule.Label(Color.Gainsboro, "削除", "削除キャプション");
            NotHaisoLabel = new C1.C1Schedule.Label(Color.MintCream, "未配車", "未配車キャプション");
            NoEditLabel = new C1.C1Schedule.Label(Color.DimGray, "編集不可", "編集不可キャプション");

            // 汎用データアクセスクラスインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // 配車入力クラスインスタンス作成
            this._HaishaNyuryoku = new HaishaNyuryoku(this.appAuth);

            // 管理情報取得
            this._KanriInfo = this._DalUtil.Kanri.GetInfo();

            // 追加・更新情報保持領域インスタンス作成
            this._HaishaNyuryokuUpdata = new Dictionary<decimal, HaishaNyuryokuInfo>();
            this._HaishaNyuryokuAdd = new Dictionary<decimal, HaishaNyuryokuInfo>();
            this._HaishaNyuryokuDel = new Dictionary<decimal, HaishaNyuryokuInfo>();

            ////管理情報取得 TODO 削除予定
            //this._ToraDonSystemPropertyInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numCarCd, this.ShowCmnSearchCar},
                {this.numCarKindCd, this.ShowCmnSearchCarKind},
                {this.numTokuisakiCd, this.ShowCmnSearchTokuisaki},
                {this.numHomenCode, this.ShowCmnSearchHomen},
                {this.numStaffCd, this.ShowCmnSearchStaff},
                {this.numJSearchCarKindCd, this.ShowCmnSearchCarKind},
                {this.numJuchuTantoCd, this.ShowCmnJuchuTantosha},
            };

            // 休日用配色情報を取得
            this.GetKyukaHaishoku();

            // 時間間隔コンボボックス初期化
            this.InitTimeIntervalCombo();
            // 表示範囲コンボボックス初期化
            this.InitDateRangeCombo();
            // 車両区分コンボボックス初期化
            this.InitTimeScheduleTypeCombo();
            // 営業所コンボボックス初期化
            this.InitBranchOfficeCombo();
            // 排他用検索条件
            this.DispHaitaJoken();
            // 表示条件の前回値取得
            this.GetOperatorExInfo();

            // 画面表示設定
            this.InitScreen(true);

        }

        /// <summary>
        /// 画面配色を初期化します。
        /// </summary>
        private void InitFrameColor()
        {
            //ステータスバーの背景色設定
            this.statusStrip1.BackColor = FrameUtilites.GetFrameFooterBackColor();
        }

        /// <summary>
        /// メニュー関連の初期化をします。
        /// </summary>
        private void InitMenuHaishaNyuryoku()
        {
            // 操作メニュー
            this.InitActionMenuHaishaNyuryoku();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuHaishaNyuryoku()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {

            // 受注情報検索条件
            if (this._KanriInfo.HaishaNyuryokuHizukeJokenSubtractDays == null)
            {
                this.dteJuchuStartYMD.Value = null;
            }
            else
            {
                this.dteJuchuStartYMD.Value = DateTime.Today.AddDays((double)this._KanriInfo.HaishaNyuryokuHizukeJokenSubtractDays * -1);
            }
            if (this._KanriInfo.HaishaNyuryokuHizukeJokenAddDays == null)
            {
                this.dteJuchuEndYMD.Value = null;
            }
            else
            {
                this.dteJuchuEndYMD.Value = DateTime.Today.AddDays((double)this._KanriInfo.HaishaNyuryokuHizukeJokenAddDays);
            }
            this.numTokuisakiCd.Text = string.Empty;
            this.edtJuchuTokuisakiNM.Text = string.Empty;
            this.numHomenCode.Text = string.Empty;
            this.edtHomenName.Text = string.Empty;
            this.numJSearchCarKindCd.Text = string.Empty;
            this.edtJSearchCarKindNM.Text = string.Empty;
            this.numJuchuTantoCd.Text = string.Empty;
            this.edtJuchuTantoNM.Text = string.Empty;
            this.chkAllFlag.Checked = false;

            // 排他管理しない場合は初期値は空白
            if (UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn == (int)DefaultProperty.EigyoshoKanriKbn.None
                && UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn == (int)DefaultProperty.HaishaNyuryokuJokenKbn.None)
            {
                this.cmbBranchOffice.SelectedIndex = 0;
            }
            else
            {
                // 選択された営業所を初期値として設定
                if (Convert.ToDecimal(this.HaishaNyuryokuConditionInfo.BranchOfficeId).CompareTo(decimal.Zero) != 0)
                {
                    this.cmbBranchOffice.SelectedValue = this.HaishaNyuryokuConditionInfo.BranchOfficeId;
                    this.cmbBranchOffice.Enabled = false;
                }
                else 
                {
                    this.cmbBranchOffice.SelectedIndex = 0;
                }
            }

            //配車入力の営業所初期値区分によって制御を分岐
            switch (UserProperty.GetInstance().DefaultSettingsInfo.HN_EigyosyoShokichiKbn)
            {
                case (int)DefaultProperty.HN_EigyosyoShokichiKbn.SelectedOffice:

                    // 排他管理しない場合は初期値は空白
                    if (UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn == (int)DefaultProperty.EigyoshoKanriKbn.None
                        && UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn == (int)DefaultProperty.HaishaNyuryokuJokenKbn.None)
                    {
                        if (this.OperatorExInfo != null)
                        {
                            // 前回値あり
                            if (decimal.Zero.CompareTo(this.OperatorExInfo.PrevCarOfiice) < 0)
                            {
                                this.cmbHaishaBranchOffice.SelectedValue = this.OperatorExInfo.PrevCarOfiice;
                            }
                            else
                            {
                                this.cmbHaishaBranchOffice.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            // 前回値なし
                            this.cmbHaishaBranchOffice.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        // 選択された営業所を初期値として設定
                        if (Convert.ToDecimal(this.HaishaNyuryokuConditionInfo.BranchOfficeId).CompareTo(decimal.Zero) != 0)
                        {
                            this.cmbHaishaBranchOffice.SelectedValue = this.HaishaNyuryokuConditionInfo.BranchOfficeId;
                        }
                        else
                        {
                            this.cmbHaishaBranchOffice.SelectedIndex = 0;
                        }
                    }

                    break;
                default:

                    if (this.OperatorExInfo != null)
                    {
                        // 前回値あり
                        if (decimal.Zero.CompareTo(this.OperatorExInfo.PrevCarOfiice) < 0)
                        {
                            this.cmbHaishaBranchOffice.SelectedValue = this.OperatorExInfo.PrevCarOfiice;
                        }
                        else
                        {
                            this.cmbHaishaBranchOffice.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        // 前回値なし
                        this.cmbHaishaBranchOffice.SelectedIndex = 0;
                    }
                    
                    break;
            }

            // 配車情報表示切替
            if (this.OperatorExInfo != null) 
            {
                // 前回値あり
                this.cmbTimeInterval.SelectedValue = this.OperatorExInfo.PrevTimeInterval;
                this.cmbDateRange.SelectedValue = this.OperatorExInfo.PrevDisplayRange;

                if (this.OperatorExInfo.PrevCarType > 0) 
                {
                    this.cmbCarKbn.SelectedValue = this.OperatorExInfo.PrevCarType;
                }
                else 
                {
                    this.cmbCarKbn.SelectedIndex = 0;
                }
                
                this.chkMihainomiFlag.Checked = this.OperatorExInfo.PrevHaishaNyuryokuMihainomiFlag;
            }
            else 
            {
                // 前回値なし
                this.cmbTimeInterval.SelectedValue = (int)DefaultProperty.HaishaJohoJikanKankakuKbn.OneDay;
                this.cmbDateRange.SelectedValue = (int)DateRange.FourteenDays;
                this.cmbCarKbn.SelectedValue = ScheduleType.Haisha;
                this.chkMihainomiFlag.Checked = this._KanriInfo.HaishaNyuryokuMihainomiFlag;
            }
            this.dteTargetDate.Value = DateTime.Today;

            // 配車情報検索条件
            this.numCarCd.Text = string.Empty;
            this.numCarKindCd.Text = string.Empty;
            this.edtLicPlateCarNo.Text = string.Empty;
            this.edtCarKindNM.Text = string.Empty;
            this.numStaffCd.Text = string.Empty;
            this.edtStaffNM.Text = string.Empty;
            this.chkKyujitshHyojiFlag.Checked = true;

            // 詳細情報
            this.groupBoxJuchuShosai.Visible = false;
            this.groupBoxHaishaShosai.Visible = false;
            this.groupBoxKihonJoho.Visible = true;

            //メンバをクリア
            this.isConfirmClose = true;
            this.AddingFlg = false;
            this.InitHaishaFlg = false;
            this.WheelScrollFlg = false;

        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***保存
            _commandSet.Save.Execute += Save_Execute;
            _commandSet.Bind(
                _commandSet.Save, this.btnSave, actionMenuItems.GetMenuItemBy(ActionMenuItems.Update), this.toolStripSave);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

            //***編集取消
            _commandSet.EditCancel.Execute += EditCancel_Execute;
            _commandSet.Bind(
                _commandSet.EditCancel, this.btnCancel, actionMenuItems.GetMenuItemBy(ActionMenuItems.EditCancel), this.toolStripCancel);

        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            // コード検索
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTokuisakiCd, ctl => ctl.Text, this.numTokuisakiCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numHomenCode, ctl => ctl.Text, this.numHomenCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarCd, ctl => ctl.Text, this.numCarCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCd, ctl => ctl.Text, this.numCarKindCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numStaffCd, ctl => ctl.Text, this.numStaffCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numJSearchCarKindCd, ctl => ctl.Text, this.numCarKindCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numJuchuTantoCd, ctl => ctl.Text, this.numStaffCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteJuchuStartYMD, ctl => ctl.Text, this.dteJuchuStartYMD_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteJuchuEndYMD, ctl => ctl.Text, this.dteJuchuEndYMD_Validating));

            // 表示開始日付
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteTargetDate, ctl => ctl.Text, this.dteTargetDate_ValueChanged));

            // 車両区分
            cmbCarKbn.SelectedIndexChanged += cmbCarKbn_SelectedIndexChanged;

            // 時間間隔
            cmbTimeInterval.SelectedIndexChanged += CmbTimeInterval_SelectedIndexChanged;

            // 表示範囲
            cmbDateRange.SelectedIndexChanged += CmbDateRange_SelectedIndexChanged;

            //ホイールイベントの追加  
            c1ScheduleHaisha.MouseWheel += c1ScheduleHaisha_MouseWheel;

            // 配車用営業所
            cmbHaishaBranchOffice.SelectedIndexChanged += cmbHaishaBranchOffice_SelectedIndexChanged;

            // 受注用営業所
            cmbBranchOffice.SelectedIndexChanged += cmbBranchOffice_Validating;

            // 未配のみ
            chkMihainomiFlag.CheckedChanged += chkMihainomiFlag_CheckedChanged;

        }

        void Save_Execute(object sender, EventArgs e)
        {
            this.DoUpdate();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            this.DoClear(true);
        }

        private void ymd_Down(object sender, EventArgs e)
        {
            // 配車一覧の場合
            if (this.splitContainer1 == this.splitContainer3.ActiveControl)
            {
                try
                {
                    ((NskDateTime)this.splitContainer1.ActiveControl).Value
                        = Convert.ToDateTime(((NskDateTime)this.splitContainer1.ActiveControl).Value).AddDays(-1);
                }
                catch
                {
                    ;
                }
            }

            // 受注一覧の場合
            else if (this.splitContainer2 == this.splitContainer3.ActiveControl)
            {
                try
                {
                    ((NskDateTime)this.splitContainer2.ActiveControl).Value
                        = Convert.ToDateTime(((NskDateTime)this.splitContainer2.ActiveControl).Value).AddDays(-1);
                }
                catch
                {
                    ;
                }
            }
        }

        private void ymd_Up(object sender, EventArgs e)
        {
            // 受注一覧の場合
            if (this.splitContainer1 == this.splitContainer3.ActiveControl)
            {
                try
                {
                    ((NskDateTime)this.splitContainer1.ActiveControl).Value
                        = Convert.ToDateTime(((NskDateTime)this.splitContainer1.ActiveControl).Value).AddDays(1);
                }
                catch
                {
                    ;
                }
            }

            // 配車一覧の場合
            else if (this.splitContainer2 == this.splitContainer3.ActiveControl)
            {
                if (this.splitContainer2.ActiveControl.GetType().Equals(typeof(NskDateTime)))
                {
                    try
                    {
                        ((NskDateTime)this.splitContainer2.ActiveControl).Value
                            = Convert.ToDateTime(((NskDateTime)this.splitContainer2.ActiveControl).Value).AddDays(1);
                    }
                    catch
                    {
                        ;
                    }
                }
            }
        }

        /// <summary>
        /// 次ページイベント
        /// </summary>
        /// <param name="e"></param>
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            this.DoNewPage(1);
        }

        /// <summary>
        /// 前ページイベント
        /// </summary>
        /// <param name="e"></param>
        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            this.DoNewPage(-1);
        }

        /// <summary>
        /// マウスのホイール移動イベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_MouseWheel(object sender, MouseEventArgs e)
        {
            // MouseWheelイベントは2回連続で呼び出されるため、偶数回は処理しない。
            if (this.WheelScrollFlg)
            {
                this.WheelScrollFlg = false;
                return;
            };
            this.WheelScrollFlg = true;

            // グループ（カテゴリ）の移動
            // ※マウス ホイールの 1 目盛りの回転で増分される差分値で割った値を設定
            c1ScheduleHaisha.NavigateToScheduleGroup((e.Delta / SystemInformation.MouseWheelScrollDelta) * -1);

        }

        /// <summary>
        /// 新規登録ボタンのClickイベント（受注入力画面表示）
        /// </summary>
        /// <param name="e"></param>
        private void btnShinkiToroku_Click(object sender, EventArgs e)
        {
            // 受注入力画面を表示
            this.DoJuchuNyuryokuEntryAdd();

            // 現在ページ数
            int page = NowPageNumber;

            // 受注情報表示
            this.DoGetJuchuSearchData();

            if (page > MaxPageNumber) page--;

            // ページ数を表示
            DoNewPage(page);

            // 配車情報を再描画
            c1ScheduleHaisha.Refresh();
        }

        /// <summary>
        /// 一括配車ボタンのClickイベント
        /// </summary>
        /// <param name="e"></param>
        private void btnIkkatsuHaisha_Click(object sender, EventArgs e)
        {
            if (0 < this.c1ScheduleJuchu.DataStorage.AppointmentStorage.Appointments.Count)
            {
                DialogResult wk_result = MessageBox.Show(
                    "車両登録のある受注情報を一括して配車します。よろしいですか？",
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (wk_result == DialogResult.No)
                {
                    //Noの場合は処理中断
                    return;
                }
            }

            // 追加中フラグON
            this.AddingFlg = true;

            // 表示されている受注情報をもとに一括配車
            for (int i = 0; i < this.c1ScheduleJuchu.DataStorage.AppointmentStorage.Appointments.Count; i++)
            {
                var app = this.c1ScheduleJuchu.DataStorage.AppointmentStorage.Appointments[i];
                if (null != app.Tag)
                {
                    AppointInfo appointInfo = (AppointInfo)app.Tag;
                    JuchuInfo item = appointInfo.JuchuInfo;

                    // 車両が未設定の場合、または車両が使用停止の場合、または受注が編集不可の場合は一括配車対象外
                    if (item.CarId.CompareTo(decimal.Zero) == 0
                        || _HaishaNyuryoku.GetCheckDisable(item.CarId)
                        || appointInfo.AppointKbn == (int)AppointKbn.NoEditable)
                    {
                        continue;
                    } 

                    // スケジュールを追加
                    var appoint = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

                    // 各種詳細情報
                    appoint.Tag = app.Tag;

                    // 配車情報に設定
                    JudgeExclusive(appoint, true);
                }
            }

            // 現在ページ数
            int page = NowPageNumber;

            // 受注情報表示
            this.DoGetJuchuSearchData();

            if (page > MaxPageNumber) page--;

            // ページ数を表示
            DoNewPage(page);

            // 追加中フラグOFF
            this.AddingFlg = false;

        }

        /// <summary>
        /// 受注情報検索イベント
        /// </summary>
        private void SearchJuchu()
        {
            //描画を停止
            this.c1ScheduleJuchu.SuspendLayout();
            // 受注情報表示
            this.DoGetJuchuSearchData();

            // 詳細情報は非表示
            this.groupBoxJuchuShosai.Visible = false;
            this.groupBoxHaishaShosai.Visible = false;
            this.groupBoxKihonJoho.Visible = true;

            // 描画を再開
            this.c1ScheduleJuchu.ResumeLayout();
        }

        /// <summary>
        /// 配車情報検索イベント
        /// </summary>
        /// <param name="e"></param>
        private void btnSearchHaisha_Click(object sender, EventArgs e)
        {
            // 追加中フラグON
            this.AddingFlg = true;

            // 配車情報表示
            this.DoGetHaishaSearchData();

            // 詳細情報は非表示
            this.groupBoxJuchuShosai.Visible = false;
            this.groupBoxHaishaShosai.Visible = false;
            this.groupBoxKihonJoho.Visible = true;

            // 追加中フラグOFF
            this.AddingFlg = false;
        }

        /// <summary>
        /// フォーム初回起動イベント
        /// </summary>
        /// <param name="e"></param>
        private void HaishaNyuryokuFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        /// <summary>
        /// フォームキーダウンイベント
        /// </summary>
        /// <param name="e"></param>
        private void HaishaNyuryokuFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        /// <summary>
        /// フォームクローズイベント
        /// </summary>
        /// <param name="e"></param>
        private void HaishaNyuryokuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            // フォーム終了の確認をします
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
            else 
            {
                // 表示条件を保存
                this.UpdateOperatorEx();
            }
        }

        /// <summary>
        /// 検索コード部品サイドボタンの押下イベント
        /// </summary>
        /// <param name="e"></param>
        private void sbtn_Click(object sender, EventArgs e)
        {
            //共通検索画面起動
            this.ShowCmnSearch();
        }

        /// <summary>
        /// 積日FromのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteJuchuStartYMD_Validating(object sender, CancelEventArgs e)
        {
            // 受注情報検索
            this.SearchJuchu();
        }

        /// <summary>
        /// 積日ToのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteJuchuEndYMD_Validating(object sender, CancelEventArgs e)
        {
            // 受注情報検索
            this.SearchJuchu();
        }

        /// <summary>
        /// 得意先コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numTokuisakiCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateTokuisakiCd(e);

            // 受注情報検索
            this.SearchJuchu();
        }

        /// <summary>
        /// 方面コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numHomenCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateHomenCd(e);

            // 受注情報検索
            this.SearchJuchu();
        }

        /// <summary>
        /// 車両コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numCarCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateCarCd(e);
        }

        /// <summary>
        /// 車種コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numCarKindCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateCarKindCd(e);

            // 受注情報の場合
            if (this.splitContainer2 == this.splitContainer3.ActiveControl)
            {
                // 受注情報検索
                this.SearchJuchu();
            }
        }

        /// <summary>
        /// 社員コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numStaffCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateStaffCd(e);

            // 受注情報の場合
            if (this.splitContainer2 == this.splitContainer3.ActiveControl)
            {
                // 受注情報検索
                this.SearchJuchu();
            }
        }

        /// <summary>
        /// 営業所（受注）のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbBranchOffice_Validating(object sender, EventArgs e)
        {
            // 初期化中は処理中断
            if (this.InitFlg) return;

            // 受注情報検索
            this.SearchJuchu();
        }

        /// <summary>
        /// 受注一覧のClickイベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleJuchu_Click(object sender, EventArgs e)
        {
            // 受注一覧クリック時選択時

            if (0 == c1ScheduleJuchu.SelectedAppointments.Count) return;
            var appoint = c1ScheduleJuchu.SelectedAppointments[0];
            if (null != appoint.Tag)
            {
                // 休日以外の場合
                if (((AppointInfo)appoint.Tag).AppointKbn != (int)AppointKbn.Kyujitsu) 
                {
                    this.groupBoxJuchuShosai.Visible = true;
                    this.groupBoxHaishaShosai.Visible = false;
                    this.groupBoxKihonJoho.Visible = false;

                    // 受注情報詳細の設定
                    JuchuInfo item = ((AppointInfo)appoint.Tag).JuchuInfo;

                    this.numBranchOfficeCd.Value = item.BranchOfficeCode;
                    this.edtBranchOfficeNM.Text = item.BranchOfficeShortName;
                    this.numJuchuShosaiTokuisakiCd.Value = item.TokuisakiCode;
                    this.edtJuchuShosaiTokuisakiNM.Text = item.TokuisakiName;
                    this.numJuchuStartPointCd.Value = item.StartPointCode;
                    this.edtJuchuStartPointNM.Text = item.StartPointName;
                    this.numJuchuEndPointCd.Value = item.EndPointCode;
                    this.edtJuchuEndPointNM.Text = item.EndPointName;
                    this.dteChakubi.Value = item.TaskEndDateTime;

                    if (item.ReuseYMD == DateTime.MinValue) this.dteReuseYMD.Value = null;
                    else this.dteReuseYMD.Value = item.ReuseYMD;

                    this.numItemCd.Value = item.ItemCode;
                    this.dteItemNm.Text = item.ItemName;
                    this.numJuchuNumber.Value = item.Number;
                    this.numJuchuCarCd.Value = item.CarCode;
                    this.edtJuchuLicPlateCarNo.Text = item.LicPlateCarNo;
                    this.edtJuchuCarBranchOfficeNm.Text = item.CarKbn == (int)DefaultProperty.CarKbn.Yosha
                            ? FrameUtilites.GetSystemName(DefaultProperty.SystemNameKbn.CarKbn, (int)DefaultProperty.CarKbn.Yosha)
                            : item.CarBranchOfficeShortName;
                    this.numJuchuCarKindCd.Value = item.CarKindCode;
                    this.edtJuchuCarKindNm.Text = item.CarKindName;
                    this.numJuchuStaffCd.Value = item.StaffCd;
                    this.edtJuchuStaffNm.Text = item.StaffName;
                    this.numJuchuCarOfChartererCd.Value = item.TorihikiCd;
                    this.edtJuchuCarOfChartererNm.Text = item.TorihikiShortName;
                    this.edtMagoYoshasaki.Text = item.MagoYoshasaki;
                    this.dteTsumibi.Value = item.TaskStartDateTime;
                    this.numJuchuKingaku.Value = item.PriceInPrice;
                }
            }
        }

        /// <summary>
        /// 配車情報のClickイベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_Click(object sender, EventArgs e)
        {
            // 配車情報クリック時選択時

            if (0 == c1ScheduleHaisha.SelectedAppointments.Count) return;
            var appoint = c1ScheduleHaisha.SelectedAppointments[0];
            if (null != appoint.Tag)
            {
                // 休日の場合
                if (((AppointInfo)appoint.Tag).AppointKbn == (int)AppointKbn.Kyujitsu) 
                {
                    this.groupBoxJuchuShosai.Visible = false;
                    this.groupBoxHaishaShosai.Visible = false;
                    this.groupBoxKihonJoho.Visible = true;
                }
                else
                {
                    // 詳細情報へ表示
                    this.SetHaishaShosai(appoint);
                }
            }
        }

        /// <summary>
        /// 受注一覧のDoubleClickイベント(受注入力画面表示)
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleJuchu_DoubleClick(object sender, EventArgs e)
        {
            if (0 == c1ScheduleJuchu.SelectedAppointments.Count) return;
            var appoint = c1ScheduleJuchu.SelectedAppointments[0];
            if (null != appoint.Tag)
            {
                // 休日以外の場合
                if (((AppointInfo)appoint.Tag).AppointKbn != (int)AppointKbn.Kyujitsu) 
                {
                    // appoint情報を取得
                    AppointInfo appInfo = (AppointInfo)appoint.Tag;

                    // 受注入力画面を表示
                    this.DoJuchuNyuryokuEntryUpdate(appInfo.JuchuInfo.JuchuSlipNo, appInfo.JuchuInfo.JuchuId);

                    // 現在ページ数
                    int page = NowPageNumber;

                    // 受注情報表示
                    this.DoGetJuchuSearchData();

                    if (page > MaxPageNumber) page--;

                    // ページ数を表示
                    DoNewPage(page);

                    // 配車情報を再描画
                    c1ScheduleHaisha.Refresh();
                }
            }
        }

        /// <summary>
        /// 配車情報のDoubleClickイベント(配車詳細入力画面表示)
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_DoubleClick(object sender, EventArgs e)
        {
            this.DispShosaiNyuryoku();
        }

        /// <summary>
        /// 配車情報の右クリックイベント(配車詳細入力画面表示)
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_MouseClick(object sender, MouseEventArgs e)
        {
            // 右クリックの場合
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.DispShosaiNyuryoku();
            }
        }

        /// <summary>
        /// 配車情報の追加（受注一覧からのDragDrop）イベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_DragDrop(object sender, DragEventArgs e)
        {
            // 追加中フラグON
            this.AddingFlg = true;

            var appoint = (Appointment)e.Data.GetData(typeof(Appointment));
            if (null != appoint.Tag)
            {

                // 配車情報に設定
                JudgeExclusive(appoint, false);

            }

            // 追加中フラグOFF
            this.AddingFlg = false;
        }

        /// <summary>
        /// 配車情報の予定変更イベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_AppointmentChanged(object sender, AppointmentEventArgs e)
        {
            // 初期化中の場合はリターン
            if (this.InitFlg) return;

            // 追加中の場合はリターン
            if (this.AddingFlg) return;

            var appoint = e.Appointment;
            if (null != appoint.Tag)
            {
                // 編集可の場合
                if (((AppointInfo)appoint.Tag).AppointKbn == (int)AppointKbn.Editable)
                {

                    // appoint情報を取得
                    AppointInfo appInfo = (AppointInfo)appoint.Tag;
                    // 配車情報詳細の設定
                    HaishaNyuryokuInfo item = appInfo.HaishaInfo;

                    // 配車情報がない（追加時の場合）は終了
                    if (item == null) return;

                    // 受注情報詳細を配車情報に設定
                    JuchuInfo JuchuItem = appInfo.JuchuInfo;

                    // 配置した場所のCarIdを取得
                    decimal carId = CarDictionary.First(x => x.Value == appoint.Categories.First()).Key;

                    // Category（配車情報）の詳細情報を取得
                    HaishaCarInfo carInf = CarInfoDictionary.Where(x => x.CarId == carId).First();

                    // カテゴリの変更があった場合のみ、乗務員・傭車先情報を再設定する。
                    if (item.CarCode != carInf.CarCode)
                    {
                        // 傭車、かつ受注の傭車先が未設定、かつ車番に紐づく傭車先がある場合
                        // または、傭車以外、かつ受注の乗務員が未設定、かつかつ車番に紐づく乗務員がある場合
                        if ((carInf.CarKbn == (int)DefaultProperty.CarKbn.Yosha && JuchuItem.CarOfChartererId == decimal.Zero && carInf.TorihikiId != decimal.Zero)
                            || (carInf.CarKbn != (int)DefaultProperty.CarKbn.Yosha && JuchuItem.DriverId == decimal.Zero && carInf.DriverId != decimal.Zero))
                        {
                            // 乗務員・傭車先情報を配車詳細情報に設定し、乗務員名・傭車先名を取得
                            SetJishaSharyoYousha(item, carInf, JuchuItem);

                            // 件名
                            e.Appointment.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;
                        }

                        // 受注の乗務員が設定済み、かつ傭車以外、かつ配車の乗務員が未設定の場合
                        else if (JuchuItem.DriverId != decimal.Zero && carInf.CarKbn != (int)DefaultProperty.CarKbn.Yosha && item.DriverId == decimal.Zero) 
                        {
                            item.DriverId = JuchuItem.DriverId;
                            item.StaffCd = JuchuItem.StaffCd;
                            item.StaffName = JuchuItem.StaffName;
                        }

                        // 車両区分
                        item.CarKbn = carInf.CarKbn;
                        if (carInf.CarKbn == (int)DefaultProperty.CarKbn.Yosha) 
                        {
                            item.LicPlateCarNo = carInf.LicPlateCarNo;
                            item.CarLicPlateCarNo = string.Empty;
                        }
                        else
                        {
                            item.LicPlateCarNo = string.Empty;
                            item.CarLicPlateCarNo = carInf.LicPlateCarNo;
                            item.PriceInCharterPrice = decimal.Zero;
                            item.TollFeeInCharterPrice = decimal.Zero;
                            item.CharterTaxDispKbn = (int)DefaultProperty.ZeiKbn.Sotozei;
                            item.CharterAddUpYMD = DateTime.MinValue;
                            item.CharterFixFlag = false;
                        }
                    }

                    // 元の発日を取得
                    DateTime hatsuYMD = item.StartYMD;

                    // 元の発日がリサイズ期間外の場合
                    if (hatsuYMD < e.Appointment.Start || e.Appointment.End < hatsuYMD)
                    {
                        // リサイズ後の開始日を取得
                        hatsuYMD = e.Appointment.Start;
                    }

                    // 元の着日を取得
                    DateTime chakuYMD = item.TaskEndDateTime;

                    // 元の着日がリサイズ期間外の場合
                    if (chakuYMD < hatsuYMD || e.Appointment.End < chakuYMD)
                    {
                        // リサイズ後の開始日を取得
                        chakuYMD = e.Appointment.End;
                    }

                    // 発着日を更新
                    item.TaskStartDateTime = appoint.Start;
                    item.StartYMD = hatsuYMD;
                    item.TaskEndDateTime = chakuYMD;
                    item.ReuseYMD = appoint.End;
                    item.CarId = CarDictionary.First(x => x.Value == appoint.Categories.First()).Key;
                    item.CarCode = carInf.CarCode;
                    item.CarKbn = carInf.CarKbn;
                    this.dteTaskStartYMD.Value = appoint.Start;
                    //this.dteTaskEndYMD.Value = appoint.End;
                    this.dteHaishaReuseYMD.Value = appoint.End;

                    // 保持領域に格納
                    this.SetHoldingArea(item, appInfo.AddFlg);

                    // 詳細情報へ表示
                    this.SetHaishaShosai(appoint);

                    // 追加でない場合の色変え
                    if (!appInfo.AddFlg) appoint.Label = ShuseiHaisoLabel;
                }
            }
        }

        /// <summary>
        /// 受注一覧のサイズ変更イベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleJuchu_BeforeAppointmentResize(object sender, CancelAppointmentEventArgs e)
        {
            // サイズ変更処理をキャンセル
            e.Cancel = true;
        }

        /// <summary>
        /// 配車情報の予定削除イベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_AppointmentDeleted(object sender, AppointmentEventArgs e)
        {
            // 初期化中は処理中断
            if (this.InitFlg) return;

            // 配車情報のクリアは処理中断
            if (this.InitHaishaFlg) return;

            // 現在ページ数
            int page = NowPageNumber;

            // 受注情報表示
            this.DoGetJuchuSearchData();

            if (page > MaxPageNumber) page--;

            // ページ数を表示
            this.DoNewPage(page);

            // 配車情報を再描画
            c1ScheduleHaisha.Refresh();
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddSearchableControls(
                this.numTokuisakiCd,
                this.numHomenCode,
                this.numCarCd,
                this.numCarKindCd,
                this.numStaffCd);
            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            // 売上日付
            this.ActiveControl = this.dteJuchuStartYMD;
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaishaNyuryokuFrame();
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
        /// 表示内容を初回表示状態にします。
        /// </summary>
        private void InitScreen(bool clearFlg)
        {
            // 初期化フラグON
            this.InitFlg = true;

            //描画を停止
            this.c1ScheduleJuchu.SuspendLayout();
            this.c1ScheduleHaisha.SuspendLayout();

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //入力項目のクリア
                if (clearFlg) this.ClearInputs();

                // 追加・更新・削除情報を初期化
                this._HaishaNyuryokuUpdata.Clear();
                this._HaishaNyuryokuAdd.Clear();
                this._HaishaNyuryokuDel.Clear();

                // 受注情報
                this.ShowJuchuList(this.GetScreen());

                // 配車情報表示
                this.ShowSchedule(this.GetScreen());

                // 仮採番番号を初期化
                HaishaIdNumbering = decimal.Zero;

            }
            catch (CanRetryException ex)
            {
                //データがない場合の例外ハンドラ
                FrameUtilites.ShowExceptionMessage(ex, this);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                //既定項目にフォーカス
                this.SetFirstFocus();

                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;

                // 描画を再開
                this.c1ScheduleJuchu.ResumeLayout();
                this.c1ScheduleHaisha.ResumeLayout();

                // 初期化フラグOFF
                this.InitFlg = false;
            }
        }

        /// <summary>
        /// 配車入力画面の検索条件を取得します。
        /// </summary>
        /// <param name="keyFlg">キー検索フラグ</param>
        /// <param name="searchKbn">受注情報検索区分（0:未使用、1:削除情報、2:追加、3:更新）</param>
        private HaishaNyuryokuSearchParameter GetScreen(bool keyFlg = false, int searchKbn = 0)
        {

            // 検索条件設定
            var para = new HaishaNyuryokuSearchParameter();

            // 受注情報検索条件
            para.JuchuStartYMD = this.dteJuchuStartYMD.Value;

            if (this.dteJuchuEndYMD.Value != null)
            {
                para.JuchuEndYMD = new DateTime(
                    this.dteJuchuEndYMD.Value.Value.Year
                    , this.dteJuchuEndYMD.Value.Value.Month
                    , this.dteJuchuEndYMD.Value.Value.Day
                    , 23
                    , 59
                    , 59);
            }
            else
            {
                para.JuchuEndYMD = null;
            }
            para.TokuisakiCd = this.numTokuisakiCd.Value;
            para.HomenCode = this.numHomenCode.Value;
            para.BranchOfficeId = (decimal?)cmbBranchOffice.SelectedValue;
            para.JuchuCarKindId = Convert.ToDecimal(this.numJSearchCarKindCd.Tag);
            para.JuchuTAntoshaId = Convert.ToDecimal(this.numJuchuTantoCd.Tag);
            para.HaishaJuchuListSortKbn = UserProperty.GetInstance().SystemSettingsInfo.HaishaJuchuListSortKbn;

            // 配車情報表示条件
            para.PrevTimeInterval = Convert.ToInt32(this.cmbTimeInterval.SelectedValue);
            para.PrevDisplayRange = Convert.ToInt32(this.cmbDateRange.SelectedValue);
            para.PrevCarType = Convert.ToInt32(this.cmbCarKbn.SelectedValue);
            para.PrevCarOfiice = Convert.ToDecimal(this.cmbCarKbn.SelectedValue);
            para.PrevHaishaNyuryokuMihainomiFlag = this.chkMihainomiFlag.Checked;

            // 配車情報検索条件
            para.DispStratYMD = this.dteTargetDate.Value.Value;
            para.DispEndYMD = this.dteTargetDate.Value.Value.AddDays(DEFAULT_DISP_DAYS);
            para.CarCode = (int?)numCarCd.Value;
            para.CarKindCode = (int?)numCarKindCd.Value;
            para.StaffCd = (int?)numStaffCd.Value;
            para.CarKbn = (int?)cmbCarKbn.SelectedValue;
            para.HaishaBranchOfficeId = (decimal?)cmbHaishaBranchOffice.SelectedValue;
            para.DisableFlag = this.chkAllFlag.Checked;

            // キー検索フラグ
            para.KeyFlg = keyFlg;

            // 排他用検索条件
            para.HaishaNyuryokuConditionInfo = this.HaishaNyuryokuConditionInfo;

            switch (searchKbn)
            {
                case 1:
                    // 削除対象の受注情報検索の場合
                    para.HaishaNyuryokuInfo = this._HaishaNyuryokuDel;
                    break;
                case 2:
                    // 追加対象の受注情報検索の場合
                    para.HaishaNyuryokuInfo = this._HaishaNyuryokuAdd;
                    break;
                case 3:
                    // 追加対象の受注情報検索の場合
                    para.HaishaNyuryokuInfo = this._HaishaNyuryokuUpdata;
                    break;
                default:
                    break;
            }

            return para;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputs(HaishaNyuryokuProcessMode mode)
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;


            /*********************
             * エラーチェック
             ********************/
            switch (mode)
            {
                // 受注検索
                case HaishaNyuryokuProcessMode.JuchuSearch:

                    // 日付の範囲チェック
                    if (rt_val && this.dteJuchuStartYMD.Value != null && this.dteJuchuEndYMD.Value != null)
                    {
                        if (this.dteJuchuStartYMD.Value > this.dteJuchuEndYMD.Value)
                        {
                            rt_val = false;
                            msg = FrameUtilites.GetDefineMessage(
                                "MW2202018", new string[] { "日付" });
                            ctl = this.dteJuchuStartYMD;
                        }
                    }

                    break;

                // 配車検索
                case HaishaNyuryokuProcessMode.HaishaSearch:
                    break;

                // 保存ボタン押下
                case HaishaNyuryokuProcessMode.Save:

                    if (rt_val)
                    {
                        // 追加・更新対象リスト
                        List<HaishaNyuryokuInfo> list = new List<HaishaNyuryokuInfo>();

                        // 追加対象となる配車
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuAdd)
                        {
                            list.Add(info.Value);
                        }

                        // 更新対象となる配車
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuUpdata)
                        {
                            list.Add(info.Value);
                        }

                        // 更新・追加対象となる配車
                        foreach (HaishaNyuryokuInfo info in list)
                        {
                            decimal startYmd = this.CnvDayDecimal(info.StartYMD, true).Value;
                            decimal startHm = this.CnvDayDecimal(info.StartYMD, true).Value;
                            string ymdhmS = startYmd.ToString("00000000") + startHm.ToString("0000");

                            decimal sekisaiYmd = this.CnvDayDecimal(info.TaskStartDateTime, true).Value;
                            decimal sekisaiHm = this.CnvDayDecimal(info.TaskStartDateTime, true).Value;
                            string ymdhmTsumi = sekisaiYmd.ToString("00000000") + sekisaiHm.ToString("0000");

                            // 積載日チェック
                            if (ymdhmTsumi.CompareTo(ymdhmS) > 0)
                            {
                                rt_val = false;
                                msg = FrameUtilites.GetDefineMessage(
                                    "MW2202032", new string[] { "積載日付には発日以前の日付" });
                                ctl = this.c1ScheduleHaisha;
                            }

                            // 乗務員の必須入力チェック
                            if (rt_val && (info.StaffCd == null || info.StaffCd == 0)
                                && info.CarKbn != (int)DefaultProperty.CarKbn.Yosha)
                            {
                                rt_val = false;
                                msg = FrameUtilites.GetDefineMessage(
                                    "MW2203001", new string[] { "乗務員" });

                                msg = "乗務員を入力してください。" + Environment.NewLine
                                    + "【車両コード：" + info.CarCode + "  積日："
                                    + info.TaskStartDateTime.ToString("yyyy/MM/dd") + "】";
                                ctl = this.c1ScheduleHaisha;
                            }

                            // 傭車先の必須入力チェック
                            if (rt_val && info.TorihikiCd == 0
                                && info.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                            {
                                rt_val = false;
                                msg = "傭車先を入力してください。" + Environment.NewLine
                                    + "【車両コード：" + info.CarCode + "  積日："
                                    + info.TaskStartDateTime.ToString("yyyy/MM/dd") + "】";
                                ctl = this.c1ScheduleHaisha;
                            }

                            // 傭車計上日の必須入力チェック
                            if (rt_val && (info.CharterAddUpYMD == DateTime.MinValue)
                                && info.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                            {
                                rt_val = false;
                                msg = "傭車計上日を入力してください。" + Environment.NewLine
                                    + "【車両コード：" + info.CarCode + "  積日："
                                    + info.TaskStartDateTime.ToString("yyyy/MM/dd") + "】";
                                ctl = this.c1ScheduleHaisha;
                            }
                        }
                        
                    }

                    if (rt_val)
                    {
                        // 請求済、支払済、月次締処理済みのチェック

                        msg = CheckFix();
                        if (!string.IsNullOrWhiteSpace(msg)) 
                        {
                            rt_val = false;
                            ctl = this.c1ScheduleHaisha;
                        }
                    }

                    break;
            }


            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();

                //返却
                return rt_val;
            }

            /*********************
             * 警告チェック
             ********************/

            switch (mode)
            {
                // 受注検索
                case HaishaNyuryokuProcessMode.JuchuSearch:
                    break;

                // 配車検索
                case HaishaNyuryokuProcessMode.HaishaSearch:
                    break;

                // 保存ボタン押下
                case HaishaNyuryokuProcessMode.Save:

                    if (rt_val)
                    {
                        // 追加・更新対象リスト
                        List<HaishaNyuryokuInfo> list = new List<HaishaNyuryokuInfo>();

                        // 追加対象となる配車
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuAdd)
                        {
                            list.Add(info.Value);
                        }

                        // 更新対象となる配車
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuUpdata)
                        {
                            list.Add(info.Value);
                        }

                        if (list.Count() > 0) 
                        {
                            DateTime minYMD = list.Min(x => x.TaskStartDateTime);
                            DateTime maxYMD = list.Max(x => x.ReuseYMD);

                            // 発日
                            DateTime stratYMD = new DateTime(minYMD.Year, minYMD.Month, minYMD.Day, 0, 0, 0);
                            // 着日
                            DateTime endYMD = new DateTime(maxYMD.Year, maxYMD.Month, maxYMD.Day, 23, 59, 59);

                            // 休日情報の取得
                            var kyujitsuList = _HaishaNyuryoku.GetCheckKyujitsuCalendar(stratYMD, endYMD, null);

                            // 対象となる配車
                            foreach (HaishaNyuryokuInfo info in list)
                            {
                                //配車休日チェックフラグがtrueの場合のみ
                                if (UserProperty.GetInstance().SystemSettingsInfo.HaishaKyujitsuCheckFlag)
                                {
                                    bool checkFlag = false;

                                    // 積日
                                    DateTime sYMD = new DateTime(info.TaskStartDateTime.Year, info.TaskStartDateTime.Month, info.TaskStartDateTime.Day, 0, 0, 0);

                                    // 積日が休日に含まれるか
                                    var kl = kyujitsuList.Where(x => x.DriverId == info.DriverId
                                     && x.HizukeYMD == sYMD);

                                    checkFlag = kl.Count() > 0;

                                    if (!checkFlag)
                                    {
                                        // 発日
                                        DateTime hYMD = new DateTime(info.StartYMD.Year, info.StartYMD.Month, info.StartYMD.Day, 0, 0, 0);
                                        // 着日
                                        DateTime eYMD = new DateTime(info.ReuseYMD.Year, info.ReuseYMD.Month, info.ReuseYMD.Day, 23, 59, 59);

                                        // 発日～着日が休日に含まれるか
                                        var kl2 = kyujitsuList.Where(x => x.DriverId == info.DriverId
                                         && x.HizukeYMD >= hYMD
                                         && x.HizukeYMD <= eYMD);

                                        checkFlag = kl2.Count() > 0;
                                    }

                                    if (checkFlag)
                                    {
                                        msg = "配車予定期間に乗務員の休日が含まれておりますがよろしいでしょうか？"
                                            + Environment.NewLine
                                            + "【車両コード：" + info.CarCode + "  積日："
                                            + info.TaskStartDateTime.ToString("yyyy/MM/dd HH:mm") + "】";
                                        ctl = this.c1ScheduleHaisha;

                                        DialogResult wk_result = MessageBox.Show(
                                            msg,
                                            "確認",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question,
                                            MessageBoxDefaultButton.Button2);

                                        if (wk_result == DialogResult.No)
                                        {
                                            //Noの場合は処理中断
                                            return false;
                                        }
                                    }
                                }

                                //配車発着日チェックフラグがtrueの場合のみ
                                if (UserProperty.GetInstance().SystemSettingsInfo.HaishaYMDCheckFlag)
                                {
                                    // 受注時の積着日と異なる場合
                                    if (info.JuchuTaskStartDateTime != info.TaskStartDateTime
                                        || info.JuchuTaskEndDateTime != info.TaskEndDateTime
                                        || (info.JuchuReuseYMD != DateTime.MinValue && info.JuchuReuseYMD != info.ReuseYMD))
                                    {
                                        msg = "受注時の積着日と異なっておりますがよろしいでしょうか？"
                                            + Environment.NewLine
                                            + "【車両コード：" + info.CarCode + "】"
                                            + Environment.NewLine
                                            + "積日　 受注時：" + info.JuchuTaskStartDateTime.ToString("yyyy/MM/dd HH:mm") + " → "
                                            + "配車時：" + info.TaskStartDateTime.ToString("yyyy/MM/dd HH:mm")
                                            + Environment.NewLine
                                            + "着日　 受注時：" + info.JuchuTaskEndDateTime.ToString("yyyy/MM/dd HH:mm") + " → "
                                            + "配車時：" + info.TaskEndDateTime.ToString("yyyy/MM/dd HH:mm");

                                        // 再使用可能日時(設定されている場合のみ)
                                        if (info.JuchuReuseYMD != DateTime.MinValue)
                                        {
                                            msg += Environment.NewLine 
                                                + "再使日 受注時：" + info.JuchuReuseYMD.ToString("yyyy/MM/dd HH:mm") + " → "
                                                + "配車時：" + info.ReuseYMD.ToString("yyyy/MM/dd HH:mm");
                                        }

                                        ctl = this.c1ScheduleHaisha;

                                        DialogResult wk_result = MessageBox.Show(
                                            msg,
                                            "確認",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question,
                                            MessageBoxDefaultButton.Button2);

                                        if (wk_result == DialogResult.No)
                                        {
                                            //Noの場合は処理中断
                                            return false;
                                        }
                                    }
                                }
                            }
                        }

                        // リストをクリア
                        list.Clear();

                        // 更新対象となる配車
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuUpdata)
                        {
                            list.Add(info.Value);
                        }

                        // 削除対象となる配車
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuDel)
                        {
                            list.Add(info.Value);
                        }

                        if (this._DataOfCurrentDay.Count() > 0)
                        {
                            DateTime stratYMD = this._DataOfCurrentDay.Max(x => x.HaishaInfo.TaskStartDateTime);
                            DateTime endYMD = this._DataOfCurrentDay.Min(x => x.HaishaInfo.ReuseYMD);

                            // 休日情報の取得
                            var kyujitsuList = _HaishaNyuryoku.GetCheckKyujitsuCalendar(stratYMD, endYMD, null);

                            // 未変更データのチェック
                            foreach (HaishaSearchResultInfo data in this._DataOfCurrentDay)
                            {
                                // 配車情報
                                HaishaNyuryokuInfo info = data.HaishaInfo;

                                //配車休日チェックフラグがtrueの場合のみ
                                if (UserProperty.GetInstance().SystemSettingsInfo.HaishaKyujitsuCheckFlag)
                                {
                                    bool checkFlag = false;

                                    // 更新・削除の対象に入っている場合はスキップ
                                    if (list.Where(x => x.HaishaId == info.HaishaId).Count() > 0) continue;

                                    // 積日
                                    DateTime sYMD = new DateTime(info.TaskStartDateTime.Year, info.TaskStartDateTime.Month, info.TaskStartDateTime.Day, 0, 0, 0);

                                    // 積日が休日に含まれるか
                                    var kl = kyujitsuList.Where(x => x.DriverId == info.DriverId
                                     && x.HizukeYMD == sYMD);

                                    checkFlag = kl.Count() > 0;

                                    if (!checkFlag)
                                    {
                                        // 発日
                                        DateTime hYMD = new DateTime(info.StartYMD.Year, info.StartYMD.Month, info.StartYMD.Day, 0, 0, 0);
                                        // 着日
                                        DateTime eYMD = new DateTime(info.ReuseYMD.Year, info.ReuseYMD.Month, info.ReuseYMD.Day, 23, 59, 59);

                                        // 対象期間に休日が含まれるか
                                        var kl2 = kyujitsuList.Where(x => x.DriverId == info.DriverId
                                         && x.HizukeYMD >= sYMD
                                         && x.HizukeYMD <= eYMD);

                                        checkFlag = kl2.Count() > 0;

                                        if (kl.Count() > 0)
                                        {
                                            msg = "配車予定期間に乗務員の休日が含まれておりますがよろしいでしょうか？"
                                                + Environment.NewLine
                                                + "【車両コード：" + info.CarCode + "  積日："
                                                + info.TaskStartDateTime.ToString("yyyy/MM/dd HH:mm") + "】";
                                            ctl = this.c1ScheduleHaisha;

                                            DialogResult wk_result = MessageBox.Show(
                                                msg,
                                                "確認",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question,
                                                MessageBoxDefaultButton.Button2);

                                            if (wk_result == DialogResult.No)
                                            {
                                                //Noの場合は処理中断
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            //返却
            return true;
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
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    // 受注一覧がアクティブの場合
                    if (this.splitContainer2 == this.splitContainer3.ActiveControl
                        && this.splitContainer2.ActiveControl == this.c1ScheduleJuchu)
                    {
                        // 上下左右キーの場合、入力をキャンセル
                        e.Handled = true;
                    }
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
            if (this.splitContainer1 == this.splitContainer3.ActiveControl) 
            {
                if (SearchFunctions.ContainsKey(this.splitContainer1.ActiveControl)) 
                { 
                    SearchFunctions[this.splitContainer1.ActiveControl](); 
                }
            } 
            else 
            {
                if (SearchFunctions.ContainsKey(this.splitContainer2.ActiveControl)) 
                {
                    SearchFunctions[this.splitContainer2.ActiveControl]();
                }
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
                    ((GrapeCity.Win.Editors.GcNumber)this.splitContainer1.ActiveControl).Value =
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
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得

                    // 受注の場合
                    if (this.splitContainer2 == this.splitContainer3.ActiveControl)
                    {
                        ((GrapeCity.Win.Editors.GcNumber)this.splitContainer2.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONCarKindCode);
                    }
                    // 配車の場合
                    else
                    {
                        ((GrapeCity.Win.Editors.GcNumber)this.splitContainer1.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONCarKindCode);
                    }

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
                    ((GrapeCity.Win.Editors.GcNumber)this.splitContainer2.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONTokuisakiCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 社員検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchStaff()
        {
            using (CmnSearchDriverFrame f = new CmnSearchDriverFrame())
            {
                f.InitFrame();
                f.ShowDialog();

                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.splitContainer1.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 方面検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchHomen()
        {
            using (CmnSearchHomenFrame f = new CmnSearchHomenFrame())
            {
                f.InitFrame();
                f.ShowDialog();

                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.splitContainer2.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.HomenCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 受注担当者検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnJuchuTantosha()
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
                    ((GrapeCity.Win.Editors.GcNumber)this.splitContainer2.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 検索条件に一致する受注情報を取得します。
        /// </summary>
        private void DoGetJuchuSearchData()
        {
            if (this.CheckInputs(HaishaNyuryokuProcessMode.JuchuSearch))
            {
                //マウスカーソルを待機中(砂時計)に変更
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // 受注情報を検索
                    this.ShowJuchuList(this.GetScreen());
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    //マウスカーソルを元に戻す
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 検索条件に一致する配車情報を取得します。
        /// </summary>
        private void DoGetHaishaSearchData()
        {
            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs(HaishaNyuryokuProcessMode.HaishaSearch))
            {
                //描画を停止
                this.c1ScheduleHaisha.SuspendLayout();

                //マウスカーソルを待機中(砂時計)に変更
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // 配車情報を検索
                    this.ShowSchedule(this.GetScreen());
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    //マウスカーソルを元に戻す
                    Cursor.Current = Cursors.Default;

                    // 描画を再開
                    this.c1ScheduleHaisha.ResumeLayout();
                }
            }
        }

        /// <summary>
        /// 受注情報をID指定で取得します。
        /// </summary>
        private List<JuchuInfo> GetJuchuSearchDataKey(int searchKbn)
        {
            List<JuchuInfo> result = null;

            try
            {
                // 受注情報を検索
                result = _HaishaNyuryoku.GetJuchu(this.GetScreen(true, searchKbn));
            }
            catch (CanRetryException ex)
            {
                //データがない場合の例外ハンドラ
                FrameUtilites.ShowExceptionMessage(ex, this);
            }
            catch (Exception)
            {
                throw;
            }

            if (result.Count() > 0)return result;
            
            return null;
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 得意先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetTokuisakiName(e);
        }

        /// <summary>
        /// 方面コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHomenCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetHomenName(e);
        }

        /// <summary>
        /// 車両コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetCarName(e);
        }

        /// <summary>
        /// 車種コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetCarKindName(e);
        }

        /// <summary>
        /// 社員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateStaffCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetStaffName(e);
        }

        /// <summary>
        /// 得意先コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetTokuisakiName(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numTokuisakiCd.Value))
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
                    this.edtJuchuTokuisakiNM.Text = info.ToraDONTokuisakiShortName;

                }
            }
            finally
            {
                if (isClear)
                {
                    this.numTokuisakiCd.Tag = null;
                    this.numTokuisakiCd.Value = null;
                    this.edtJuchuTokuisakiNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 方面コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetHomenName(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numHomenCode.Value))
                {
                    isClear = true;
                    return;
                }


                // 得意先情報を取得
                HomenSearchParameter para = new HomenSearchParameter();
                para.HomenCode = Convert.ToInt32(this.numHomenCode.Value);
                HomenInfo info = _DalUtil.Homen.GetList(para).FirstOrDefault();

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
                    this.numHomenCode.Tag = info.HomenId;
                    this.numHomenCode.Value = info.HomenCode;
                    this.edtHomenName.Text = info.HomenName;

                }
            }
            finally
            {
                if (isClear)
                {
                    this.numHomenCode.Tag = null;
                    this.numHomenCode.Value = null;
                    this.edtHomenName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車両コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void SetCarName(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numCarCd.Value))
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
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numCarCd.Tag = null;
                    this.numCarCd.Value = null;
                    this.edtLicPlateCarNo.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車種コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void SetCarKindName(CancelEventArgs e)
        {
            bool isClear = false;
            GrapeCity.Win.Editors.GcNumber activeConCd;
            GrapeCity.Win.Editors.GcTextBox activeConNm;

            // 受注の場合
            if (this.splitContainer2 == this.splitContainer3.ActiveControl)
            {
                activeConCd = this.numJSearchCarKindCd;
                activeConNm = this.edtJSearchCarKindNM;
            }

            // 配車の場合
            else
            {
                activeConCd = this.numCarKindCd;
                activeConNm = this.edtCarKindNM;
            }

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(activeConCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 車両情報を取得
                CarKindSearchParameter para = new CarKindSearchParameter();
                para.ToraDONCarKindCode = Convert.ToInt32(activeConCd.Value);
                CarKindInfo info = _DalUtil.CarKind.GetList(para).FirstOrDefault();

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
                    activeConCd.Tag = info.ToraDONCarKindId;
                    activeConCd.Value = info.ToraDONCarKindCode;
                    activeConNm.Text = info.ToraDONCarKindName;
                }
            }
            finally
            {
                if (isClear)
                {
                    activeConCd.Tag = null;
                    activeConCd.Value = null;
                    activeConNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 社員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void SetStaffName(CancelEventArgs e)
        {
            bool isClear = false;
            GrapeCity.Win.Editors.GcNumber activeConCd;
            GrapeCity.Win.Editors.GcTextBox activeConNm;

            // 受注の場合
            if (this.splitContainer2 == this.splitContainer3.ActiveControl)
            {
                activeConCd = this.numJuchuTantoCd;
                activeConNm = this.edtJuchuTantoNM;
            }

            // 配車の場合
            else
            {
                activeConCd = this.numStaffCd;
                activeConNm = this.edtStaffNM;
            }

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(activeConCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 乗務員情報を取得
                StaffSearchParameter para = new StaffSearchParameter();
                para.ToraDONStaffCode = Convert.ToInt32(activeConCd.Value);
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
                    activeConCd.Tag = info.ToraDONStaffId;
                    activeConCd.Value = info.ToraDONStaffCode;
                    activeConNm.Text = info.ToraDONStaffName;                   
                }
            }
            finally
            {
                if (isClear)
                {
                    activeConCd.Tag = null;
                    activeConCd.Value = null;
                    activeConNm.Text = string.Empty;
                }
            }
        }

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        #endregion

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
                    try
                    {
                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            // 削除
                            this._HaishaNyuryoku.DeleteHaishaExclusiveManage(tx);
                        });
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }

                    // 抽出条件指定より遷移した場合
                    if (HaishaNyuryokuConditionInfo != null)
                    {
                        //配車入力生成
                        HaishaNyuryokuConditionFrame f = new HaishaNyuryokuConditionFrame(this.HaishaNyuryokuConditionInfo);
                        //配車入力初期化
                        f.InitFrame();
                        //配車入力表示
                        f.Show();
                    }

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
        /// 画面情報からデータを登録します。
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

            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs(HaishaNyuryokuProcessMode.Save))
            {
                try
                {
                    //画面から値を取得
                    this.GetScreen();

                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        // 更新
                        this._HaishaNyuryoku.UpdateHaisha(tx, this._HaishaNyuryokuUpdata);

                        // 削除
                        this._HaishaNyuryoku.DeleteHaisha(tx, this._HaishaNyuryokuDel);

                        // 追加
                        this._HaishaNyuryoku.AddHaisha(tx, this._HaishaNyuryokuAdd);

                    });

                    // 操作ログ(保存)の条件取得
                    StringBuilder sb = new StringBuilder();
                    bool f = false;
                    string log_jyoken = string.Empty;

                    // 更新対象となる配車
                    if (this._HaishaNyuryokuUpdata.Count() > 0) 
                    {
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuUpdata)
                        {
                            if (f) sb.AppendLine(",");
                            sb.AppendLine(info.Key.ToString());
                            f = true;
                        }
                        log_jyoken =
                            FrameUtilites.GetDefineLogMessage(
                            "C10002",
                            new string[] { "更新対象配車ID", sb.ToString() });

                        // 操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.UpdateItem,
                                log_jyoken);
                    }

                    // 削除対象となる配車
                    if (this._HaishaNyuryokuDel.Count() > 0) 
                    {
                        sb = new StringBuilder();
                        f = false;
                        foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuDel)
                        {
                            if (f) sb.AppendLine(",");
                            sb.AppendLine(info.Key.ToString());
                            f = true;
                        }
                        log_jyoken =
                            FrameUtilites.GetDefineLogMessage(
                            "C10002",
                            new string[] { "削除対象配車ID", sb.ToString() });

                        // 操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.UpdateItem,
                                log_jyoken);
                    }

                    // 削除対象となる配車
                    if (this._HaishaNyuryokuAdd.Count() > 0)
                    {
                        // 追加対象件数
                        log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002",
                        new string[] { "追加対象件数", this._HaishaNyuryokuAdd.Count() + "件" });

                        // 操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.UpdateItem,
                                log_jyoken);

                        //登録完了メッセージ
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MI2001003"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    // 画面表示設定
                    this.InitScreen(false);

                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
                catch (MustCloseFormException ex)
                {
                    //画面の終了が要求される例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //画面を閉じます
                    this.DoClose();
                }
            }
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
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

            // 画面表示設定
            this.InitScreen(true);

        }

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

        /// <summary>
        /// 時間間隔コンボボックスを初期化します。
        /// </summary>
        private void InitTimeIntervalCombo()
        {
            // 時間間隔コンボ設定
            Dictionary<int, String> datasource = new Dictionary<int, String>();

            // 時間間隔のコンボ要素を取得
            var list = this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.HaishaJohoJikanKankakuKbn))
                .OrderBy(x => x.SystemNameCode);

            foreach(SystemNameInfo item in list)
            {
                datasource.Add(item.SystemNameCode, item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbTimeInterval, datasource, false, null, false);

            this.cmbTimeInterval.SelectedIndex = 0;
        }

        /// <summary>
        /// 表示範囲コンボボックスを初期化します。
        /// </summary>
        private void InitDateRangeCombo()
        {
            // 表示範囲コンボ設定
            Dictionary<int, String> datasource = new Dictionary<int, String>();

            // 選択中の時間間隔のコンボ要素を取得
            SystemNameInfo jikanInfo = this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.HaishaJohoJikanKankakuKbn)
                && x.SystemNameCode == (int)this.cmbTimeInterval.SelectedValue).First();

            // 時間間隔のコンボ要素を取得
            var list = this.systemNameList
                .Where(x => x.SystemNameKbn == Convert.ToInt32(jikanInfo.StringValue01))
                .OrderBy(x => x.SystemNameCode);

            foreach (SystemNameInfo item in list)
            {
                datasource.Add(item.IntegerValue01, item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbDateRange, datasource, false, null, false);

            this.cmbDateRange.SelectedIndex = datasource.Count - 1;
        }

        /// <summary>
        /// 車両区分コンボボックスを初期化します。
        /// </summary>
        private void InitTimeScheduleTypeCombo()
        {
            // スケジュール種類コンボ設定
            Dictionary<int, String> datasource = new Dictionary<int, String>();

            // 時間間隔のコンボ要素を取得
            var list = this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.CarKbn) && x.SystemNameCode < (int)DefaultProperty.CarKbn.Sonota)
                .OrderBy(x => x.SystemNameCode);

            foreach (SystemNameInfo item in list)
            {
                datasource.Add(item.SystemNameCode, item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbCarKbn, datasource, true, null, false);

            this.cmbCarKbn.SelectedIndex = 0;
        }

        /// <summary>
        /// 営業所コンボボックスを初期化します。
        /// </summary>
        private void InitBranchOfficeCombo()
        {
            // 営業所コンボ設定
            Dictionary<decimal, String> datasource = new Dictionary<decimal, String>();

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

            decimal key = 0;
            String value = "";

            foreach (ToraDONBranchOfficeInfo item in list)
            {
                key = item.BranchOfficeId;
                value = item.BranchOfficeCode.ToString() + " " + item.BranchOfficeShortName.ToString();

                datasource.Add(key, value);
            }

            // 受注情報用
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbBranchOffice, datasource, true, DefaultProperty.CMB_BRANCHOFFICE_ALL_NAME, false);

            // 配車入力用
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbHaishaBranchOffice, datasource, true, DefaultProperty.CMB_BRANCHOFFICE_ALL_NAME, false);

        }

        /// <summary>
        /// DBからデータを取得しスケジュールを表示する。
        /// コンボや、日付が変わった時に呼ばれます。
        /// </summary>
        /// <param name="isClearCategories">カテゴリを再描画するかどうか</param>
        private void ShowSchedule(HaishaNyuryokuSearchParameter para = null)
        {
            //描画を停止
            this.c1ScheduleHaisha.Visible = false;

            try
            {
                // 配車情報を検索
                this.ShowHaishaList(para);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // 描画を再開
                this.c1ScheduleHaisha.Visible = true;
            }
        }

        /// <summary>
        /// 受注一覧を表示する
        /// </summary>
        private void ShowJuchuList(HaishaNyuryokuSearchParameter para = null)
        {

            // システム日付
            DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime Date = dateNow;

            // カレント日付の設定
            this.c1ScheduleJuchu.GoToDate(dateNow);

            // 受注一覧のページ数をクリア
            MaxPageNumber = 0;

            // 現在のページ数を設定
            NowPageNumber = 0;

            // 受注情報を取得
            List<JuchuInfo> juchuList = _HaishaNyuryoku.GetJuchu(para);

            // 画面をクリア
            c1ScheduleJuchu.DataStorage.CategoryStorage.Categories.Clear();

            // Categoryをクリア
            JucCategory = new Category();
            // c1Scheduleのカテゴリをクリア
            this.c1ScheduleJuchu.DataStorage.CategoryStorage.Categories.Clear();
            // c1ScheduleのAppointmentをクリア
            this.c1ScheduleJuchu.DataStorage.AppointmentStorage.Appointments.Clear();

            // カテゴリ（縦軸）に追加
            JucCategory.Text = string.Empty;
            this.c1ScheduleJuchu.DataStorage.CategoryStorage.Categories.Add(JucCategory);

            // 配車
            IEnumerable<JuchuInfo> list = null;

            // 削除対象リスト
            List<HaishaNyuryokuInfo> delList = new List<HaishaNyuryokuInfo>();

            // 削除対象となる配車
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuDel)
            {
                delList.Add(info.Value);
            }

            // 削除対象がある場合、削除分を受注一覧に追加
            if (this._HaishaNyuryokuDel.Count > 0)
            {
                List<JuchuInfo> jlist = GetJuchuSearchDataKey(1);

                foreach (JuchuInfo info in jlist)
                {
                    // 削除対象件数
                    int delCount = delList.Where(x => x.JuchuId == info.JuchuId).Count();

                    // 配車件数
                    int haishaCount = _HaishaNyuryoku.GetCheckHsishaSelectCount(info.JuchuId);

                    // 削除対象件数と配車対象件数が一致する場合のみ、受注一覧へ追加
                    if (delCount == haishaCount) juchuList.Add(info);
                }
            }

            // 配車受注一覧ソート区分
            switch (para.HaishaJuchuListSortKbn)
            {
                case (int)DefaultProperty.HaishaJuchuListSortKbn.TsumiYMD:

                    // 積日(発日)で再ソート
                    list = juchuList.OrderBy(x => x.TaskStartDateTime);
                    break;
                case (int)DefaultProperty.HaishaJuchuListSortKbn.ChakuYMD:

                    // 着日で再ソート
                    list = juchuList.OrderBy(x => x.TaskEndDateTime);
                    break;
                case (int)DefaultProperty.HaishaJuchuListSortKbn.OfukuKbn:

                    //顧客が田村運輸の場合
                    if (SQLHelper.IsCustomer(DefaultProperty.EditionNameKbn.TamuraUnyu.ToString()))
                    {
                        // 往復区分、積日(発日)で再ソート
                        list = juchuList.OrderBy(x => x.TaskStartDateTime.Date).ThenBy(x => x.OfukuKbnSortOrder).ThenBy(x => x.OfukuKbn);
                    }
                    else
                    {
                        // 往復区分、積日(発日)で再ソート
                        list = juchuList.OrderBy(x => x.OfukuKbnSortOrder).ThenBy(x => x.OfukuKbn).ThenBy(x => x.TaskStartDateTime);
                    }
                    break;
                default:

                    // 積日(発日)で再ソート
                    list = juchuList.OrderBy(x => x.TaskStartDateTime);
                    break;
            }

            // 追加・更新対象リスト
            List<HaishaNyuryokuInfo> addUpList = new List<HaishaNyuryokuInfo>();

            // 追加対象となる配車
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuAdd)
            {
                addUpList.Add(info.Value);
            }

            // 更新対象となる配車
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuUpdata)
            {
                addUpList.Add(info.Value);
            }

            //管理情報取得
            ToraDonSystemPropertyInfo toraDonSystemPropertyInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();

            // 往路・往復件数
            int ouroOfukuCount = 0;

            // 復路件数
            int fkuroCount = 0;

            // 受注件数
            int i = 0;
            foreach (JuchuInfo item in list)
            {

                bool skipFlg = false;

                // 更新・追加のListに存在する場合はスキップ
                if (addUpList.Where(x => x.JuchuId == item.JuchuId).Count() > 0) continue;

                if (skipFlg) continue;

                // 表示件数を超えた場合、
                if (i % DEFAULT_JUC_DISPRESULTS == 0 && i != 0)
                {
                    Date = Date.AddDays(1).AddMinutes(-1 * DEFAULT_JUC_DUARATION * DEFAULT_JUC_DISPRESULTS);
                    MaxPageNumber++;
                }

                // スケジュールを追加
                var appoint = this.c1ScheduleJuchu.DataStorage.AppointmentStorage.Appointments.Add();

                // 設定箇所を設定
                appoint.Start = Date;
                appoint.Duration = TimeSpan.FromMinutes(DEFAULT_JUC_DUARATION);

                // 件名
                // 配車受注一覧ソート区分
                switch (para.HaishaJuchuListSortKbn)
                {
                    case (int)DefaultProperty.HaishaJuchuListSortKbn.TsumiYMD:

                        // 積日(発日)
                        appoint.Subject = "積日:" + item.TaskStartDateTime.Year + "/" + item.TaskStartDateTime.Month + "/" + item.TaskStartDateTime.Day
                            + " " + "積:" + item.StartPointName + " 着:" + item.EndPointName;

                        break;
                    case (int)DefaultProperty.HaishaJuchuListSortKbn.ChakuYMD:

                        // 着日
                        appoint.Subject = "着日:" + item.TaskEndDateTime.Year + "/" + item.TaskEndDateTime.Month + "/" + item.TaskEndDateTime.Day
                            + " " + "積:" + item.StartPointName + " 着:" + item.EndPointName;

                        break;
                    default:

                        // 積日(発日)
                        appoint.Subject = "積日:" + item.TaskStartDateTime.Year + "/" + item.TaskStartDateTime.Month + "/" + item.TaskStartDateTime.Day
                            + " " + "積:" + item.StartPointName + " 着:" + item.EndPointName;

                        break;
                }

                // カテゴリ情報を設定
                appoint.Categories.Add(JucCategory);

                // 往復区分でステータスを指定
                appoint.BusyStatus = GetStatus(item.OfukuKbn);


                // 往復区分でステータスを指定
                switch (item.OfukuKbn)
                {
                    case (int)DefaultProperty.OfukuKbn.OKa:

                        // 往荷
                        ouroOfukuCount++;

                        break;
                    case (int)DefaultProperty.OfukuKbn.FukuKa:

                        // 復荷
                        fkuroCount++;

                        break;

                    case (int)DefaultProperty.OfukuKbn.Ofuku:

                        // 往復
                        ouroOfukuCount++;

                        break;

                    default:
                        break;

                }

                // 受注情報詳細を保持
                AppointInfo appointInfo = new AppointInfo();
                appointInfo.AppointKbn = (int)AppointKbn.Editable;
                appointInfo.JuchuInfo = item;
                // 追加フラグ
                appointInfo.AddFlg = true;
                appoint.Tag = appointInfo;

                // 削除対象がある場合、削除分を受注一覧に追加
                if (item.DelTargetFlg)
                {
                    
                    // 削除対象配車ID
                    appointInfo.DelHaishaId = decimal.Zero;

                    // 配車対象となる配車ID
                    foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuDel)
                    {
                        if (info.Value.JuchuId.CompareTo(item.JuchuId) == 0)
                        {
                            appointInfo.DelHaishaId = info.Value.DelHaishaId;
                            break;
                        }
                    }
                    
                    // 登録済みフラグ
                    appointInfo.RegFlg = false;

                    // ラベル情報（背景色など）
                    appoint.Label = DeleteHaisoLabel;

                    // 予定区分　削除
                    appointInfo.AppointKbn = (int)AppointKbn.Sakujo;
                }

                // 行ロックがかかっている場合
                else if(JudgeExclusive(null, null, item, toraDonSystemPropertyInfo))
                {
                    // ラベル情報（背景色など）
                    appoint.Label = NoEditLabel;

                    // 予定区分　編集不可
                    appointInfo.AppointKbn = (int)AppointKbn.NoEditable;
                }
                else
                {
                    // ラベル情報（背景色など）
                    appoint.Label = NotHaisoLabel;

                    // 予定区分　通常
                    appointInfo.AppointKbn = (int)AppointKbn.Editable;
                }

                Date = Date.AddMinutes(DEFAULT_JUC_DUARATION);

                // カウントアップ
                i++;
            }

            this.labelOuroOfuku.Text = "往路・往復：" + ouroOfukuCount.ToString("###,###,##0") + "件";
            this.labelFukuro.Text = "復路：" + fkuroCount.ToString("###,###,##0") + "件";

            // 文字色を反映させるため、リフレッシュ
            this.c1ScheduleJuchu.Refresh();

            // 改ページボタンの切り替え
            this.DoChangeNewPageBtn();

        }

        /// <summary>
        /// 車両別スケジュールを表示する
        /// </summary>
        /// <param name="isClearCategories"></param>
        private void ShowHaishaList(HaishaNyuryokuSearchParameter para = null)
        {
            // 表示開始日付
            var selectedDate = dteTargetDate.Value.Value;
            c1ScheduleHaisha.GoToDate(selectedDate);

            c1ScheduleHaisha.DataStorage.CategoryStorage.Categories.Clear();
            CarDictionary = new Dictionary<decimal, Category>();

            // 車両一覧を取得(全量)
            CarInfoDictionary = _HaishaNyuryoku.GetCar(null);

            // 車両一覧を取得（条件指定）
            var carList = _HaishaNyuryoku.GetCar(para);

            // 休日情報の取得
            List<HaishaKyujitsuCalendarInfo> kyujitsuCalendarList = _HaishaNyuryoku.GetKyujitsuCalendar(para);

            // 配車情報を取得
            this._DataOfCurrentDay = _HaishaNyuryoku.GetHaisha(para);

            // 未配車両の一覧取得（「未配車両のみ」にチェックが入っていない場合は空）
            var haishaCarDictionar = this.GEtHaishaCarDictionar(para.DispStratYMD, kyujitsuCalendarList);

            // 利用可能台数
            int carInt = 0;

            foreach (HaishaCarInfo carRow in CarInfoDictionary)
            {
                var category = new Category();
                if (carRow.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                {
                    category.Text = "(" + carRow.CarCode.ToString();
                    if (!string.IsNullOrWhiteSpace(carRow.TorihikiShortName)) category.Text += "：" + carRow.TorihikiShortName;
                    category.Text += ")";
                }
                else
                {
                    category.Text = carRow.CarCode.ToString();
                    if (!string.IsNullOrWhiteSpace(carRow.StaffName)) category.Text += "：" + carRow.StaffName;
                }

                // 背景色
                if (carRow.HaishaNyuryokuCarBackColor == null)
                {
                    category.Color = Color.Empty;
                }
                else
                {
                    category.Color = ColorTranslator.FromOle(carRow.HaishaNyuryokuCarBackColor.Value);
                }

                // 検索条件に一致する物、かつ未配車両の一覧にない物のみ一覧に設定
                if (carList.Where(x => x.CarId == carRow.CarId).Count() > 0
                    && !haishaCarDictionar.ContainsKey(carRow.CarId)) 
                {
                    // 利用可能台数
                    if (!carRow.HaishaNyuryokuCarCountExclusionFlag && carRow.DisableFlag == NSKUtil.BoolToInt(false)) carInt++;

                    // カテゴリ設定
                    c1ScheduleHaisha.DataStorage.CategoryStorage.Categories.Add(category);

                }
                
                CarDictionary.Add(carRow.CarId, category);
            }

            this.labelRiyoukanoDaisu.Text = "利用可能台数：" + carInt.ToString("###,###,##0") + "台";

            // 予定のリセット
            this.InitHaishaFlg = true;
            c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Clear();
            this.InitHaishaFlg = false;

            // 表示車両がない場合は終了
            if (c1ScheduleHaisha.DataStorage.CategoryStorage.Categories.Count() == 0) return;

            // 主な乗務員の休日を表示にチェックが入っている場合
            if (this.chkKyujitshHyojiFlag.Checked) 
            {
                foreach (HaishaKyujitsuCalendarInfo info in kyujitsuCalendarList)
                {
                    // スケジュールを追加
                    var appoint = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

                    // ラベル情報（背景色など）
                    appoint.Label = new C1.C1Schedule.Label(this._KyujitsuBackColorDic[info.KyujitsuKbn], "休日", "休日キャプション");

                    // 開始日時
                    appoint.Start = info.HizukeYMD;

                    // 終了日時
                    appoint.End = info.HizukeYMD.AddDays(1);

                    // 自社車両の場合
                    appoint.Subject = "休日";

                    appoint.Categories.Add(CarDictionary[info.CarId]);

                    // 往復区分でステータスを指定
                    appoint.BusyStatus = StatusNon;

                    // 配車情報詳細を保持
                    AppointInfo appointInfo = new AppointInfo();
                    appointInfo.AppointKbn = (int)AppointKbn.Kyujitsu;
                    appointInfo.KyujitsuFontColor = _KyujitsuForeColorDic[info.KyujitsuKbn];
                    appointInfo.KyujitsuBackColor = this._KyujitsuBackColorDic[info.KyujitsuKbn];

                    // 追加フラグ
                    appointInfo.AddFlg = false;

                    // 登録済みフラグ
                    appointInfo.RegFlg = true;
                    appoint.Tag = appointInfo;

                }
            }

            //管理情報取得
            ToraDonSystemPropertyInfo toraDonSystemPropertyInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();

            // 排他情報（操作対象となる配車の一覧）を取得
            List<HaitaInfo> haitaInfo = _HaishaNyuryoku.GetExclusiveInfoSelect(para);

            foreach (HaishaSearchResultInfo info in this._DataOfCurrentDay)
            {

                HaishaNyuryokuInfo item = info.HaishaInfo;

                // 更新リストにあるデータは表示しない
                if (_HaishaNyuryokuUpdata.ContainsKey(item.HaishaId)) continue;

                // 削除リストにあるデータは表示しない
                if (_HaishaNyuryokuDel.ContainsKey(item.HaishaId)) continue;

                // スケジュールを追加
                var appoint = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

                // 操作可能か判定（排他）
                var result = JudgeExclusive(haitaInfo, item, info.JuchuInfo, toraDonSystemPropertyInfo);

                // 排他対象か判定
                if (!result)
                {
                    // ラベル情報（背景色など）通常
                    appoint.Label = HaisoLabel;
                }
                else
                {
                    // ラベル情報（背景色など）操作不可
                    appoint.Label = NoEditLabel;
                }

                // 開始日時
                appoint.Start = item.TaskStartDateTime;

                // 終了日時
                appoint.End = item.ReuseYMD;

                // 件名
                appoint.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;

                appoint.Categories.Add(CarDictionary[item.CarId]);

                // 往復区分でステータスを指定
                appoint.BusyStatus = GetStatus(item.OfukuKbn);

                // 配車情報詳細を保持
                AppointInfo appointInfo = new AppointInfo();

                // 排他対象か判定
                if (!result)
                {
                    // 予定区分　通常
                    appointInfo.AppointKbn = (int)AppointKbn.Editable;
                }
                else
                {
                    // 予定区分　編集不可
                    appointInfo.AppointKbn = (int)AppointKbn.NoEditable;
                }

                appointInfo.HaishaInfo = item;
                appointInfo.JuchuInfo = info.JuchuInfo;

                // 追加フラグ
                appointInfo.AddFlg = false;

                // 登録済みフラグ
                appointInfo.RegFlg = true;
                appoint.Tag = appointInfo;
            }

            // 追加対象の受注情報を検索
            List<JuchuInfo> jlistAdd = GetJuchuSearchDataKey(2);

            // 追加中のものは、再度描画する。
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in _HaishaNyuryokuAdd)
            {

                HaishaNyuryokuInfo item = info.Value;

                // スケジュールを追加
                var appoint = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

                // 開始日時
                appoint.Start = item.TaskStartDateTime;

                // 終了日時
                appoint.End = item.ReuseYMD;

                // 件名
                appoint.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;

                appoint.Categories.Add(CarDictionary[item.CarId]);

                // ラベル情報（背景色など）
                appoint.Label = TsuikaHaisoLabel;

                // 往復区分でステータスを指定
                appoint.BusyStatus = GetStatus(item.OfukuKbn);

                // 配車情報詳細を保持
                AppointInfo appointInfo = new AppointInfo();
                appointInfo.AppointKbn = (int)AppointKbn.Editable;
                appointInfo.HaishaInfo = item;

                // 受注情報詳細
                if (jlistAdd != null) 
                {
                    var list = jlistAdd.Where(x => x.JuchuId == item.JuchuId);
                    if (list.Count() != 0) appointInfo.JuchuInfo = jlistAdd.Where(x => x.JuchuId == item.JuchuId).First();
                }

                // 追加フラグ
                appointInfo.AddFlg = true;

                // 登録済みフラグ
                appointInfo.RegFlg = item.RegFlg;

                // 削除対象配車ID
                appointInfo.DelHaishaId = item.DelHaishaId;

                appoint.Tag = appointInfo;

            }

            // 追加対象の受注情報を検索
            List<JuchuInfo> jlistUp = GetJuchuSearchDataKey(3);

            // 更新中のものは、再度描画する。
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in _HaishaNyuryokuUpdata)
            {

                HaishaNyuryokuInfo item = info.Value;

                // スケジュールを追加
                var appoint = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

                // 開始日時
                appoint.Start = item.TaskStartDateTime;

                // 終了日時
                appoint.End = item.ReuseYMD;

                // 件名
                appoint.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;

                appoint.Categories.Add(CarDictionary[item.CarId]);

                // ラベル情報（背景色など）
                appoint.Label = ShuseiHaisoLabel;

                // 往復区分でステータスを指定
                appoint.BusyStatus = GetStatus(item.OfukuKbn);

                // 配車情報詳細を保持
                AppointInfo appointInfo = new AppointInfo();
                appointInfo.AppointKbn = (int)AppointKbn.Editable;
                appointInfo.HaishaInfo = item;

                // 受注情報詳細
                if (jlistUp != null)
                {
                    var list = jlistUp.Where(x => x.JuchuId == item.JuchuId);
                    if (list.Count() != 0) appointInfo.JuchuInfo = jlistUp.Where(x => x.JuchuId == item.JuchuId).First();
                }

                // 追加フラグ
                appointInfo.AddFlg = true;

                // 登録済みフラグ
                appointInfo.RegFlg = item.RegFlg;

                // 削除対象配車ID
                appointInfo.DelHaishaId = item.DelHaishaId;

                appoint.Tag = appointInfo;

            }
        }

        /// <summary>
        /// 車両区分の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCarKbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 配車情報を再描画
            this.HaishaShowSchedule();
        }

        /// <summary>
        /// 配車用営業所の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHaishaBranchOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 配車情報を再描画
            this.HaishaShowSchedule();
        }

        /// <summary>
        /// 日付が変更された場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dteTargetDate_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(this.dteTargetDate.Value) == DateTime.MinValue)
            {
                this.dteTargetDate.Value = DateTime.Today;
            }

            // 配車情報を再描画
            this.HaishaShowSchedule();
        }

        /// <summary>
        /// 時間間隔の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbTimeInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // 表示範囲コンボボックスを初期化
                this.InitDateRangeCombo();
                this.DoHaishaSettingUpdate();
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 表示範囲の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbDateRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                this.DoHaishaSettingUpdate();
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 未配のみチェックボックスの変更イベント
        /// </summary>
        /// <param name="e"></param>
        private void chkMihainomiFlag_CheckedChanged(object sender, EventArgs e)
        {
            // 配車情報を再描画
            this.HaishaShowSchedule();
        }

        /// <summary>
        /// 配車情報（スケジュール上の予定）の削除イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_BeforeAppointmentDelete(object sender, CancelAppointmentEventArgs e)
        {
            // 初期化中は処理中断
            if (this.InitFlg) return;

            if (0 == c1ScheduleHaisha.SelectedAppointments.Count) return;
            var appoint = c1ScheduleHaisha.SelectedAppointments[0];
            if (null != appoint.Tag)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 受注情報の追加（配車一覧からのDragDrop）イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void c1ScheduleJuchu_BeforeAppointmentDrop(object sender, CancelAppointmentEventArgs e)
        {
            //描画を停止
            this.c1ScheduleJuchu.SuspendLayout();

            // appoint情報を取得
            AppointInfo appInfo = (AppointInfo)e.Appointment.Tag;

            // 編集可以外の場合は終了
            if (((AppointInfo)e.Appointment.Tag).AppointKbn != (int)AppointKbn.Editable) 
            {
                e.Cancel = true;

                // 往復区分制御
                if (e.Appointment.BusyStatus.Text.Equals("予定あり"))
                {
                    e.Appointment.BusyStatus = GetStatus(appInfo.JuchuInfo.OfukuKbn);
                }

                return; 
            }

            // 配車情報詳細の設定
            HaishaNyuryokuInfo item = appInfo.HaishaInfo;

            // 保持領域より削除
            if (item != null)
            {
                DialogResult result =
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MQ2102004"),
                        this.Text,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    //Yesの場合は削除

                    if (appInfo.AddFlg)
                    {
                        // 追加予定の場合
                        this._HaishaNyuryokuAdd.Remove(item.HaishaId);

                        // 既存データを一度削除した場合は、もともと保持している配車IDで削除リストに追加
                        if (appInfo.RegFlg)
                        {
                            // 登録済みの予定の場合
                            this._HaishaNyuryokuDel.Add(appInfo.DelHaishaId, item);
                        }
                    }
                    else
                    {
                        // 削除対象配車ID
                        if (appInfo.RegFlg) item.DelHaishaId = item.HaishaId;

                        // 更新予定の場合
                        this._HaishaNyuryokuUpdata.Remove(item.HaishaId);

                        // 登録済みの予定の場合
                        this._HaishaNyuryokuDel.Add(item.HaishaId, item);
                    }

                    // 予定を削除
                    e.Appointment.Delete();

                }
                else if (result == DialogResult.No)
                {
                    // Noの場合は処理をキャンセル
                    e.Cancel = true;
                }

                // 配車情報詳細を非表示
                groupBoxHaishaShosai.Visible = false;
            }
            else
            {
                e.Cancel = true;

                // 往復区分制御
                if (e.Appointment.BusyStatus.Text.Equals("予定あり"))
                {
                    e.Appointment.BusyStatus = GetStatus(appInfo.JuchuInfo.OfukuKbn);
                }
            }

            // 削除対象がある場合、削除分を受注一覧に追加
            if (appInfo.JuchuInfo.DelTargetFlg)
            {
                // ラベル情報（背景色など）
                e.Appointment.Label = DeleteHaisoLabel;
            }
            else
            {
                // ラベル情報（背景色など）
                e.Appointment.Label = NotHaisoLabel;
            }

            // 描画を再開
            this.c1ScheduleJuchu.ResumeLayout();
        }

        /// <summary>
        /// 表示範囲の変更があった場合、スケジュールを再描画する
        /// </summary>
        private void DoHaishaSettingUpdate()
        {
            if (null == this.cmbTimeInterval.SelectedValue || null == this.cmbDateRange.SelectedValue) return;

            // 選択中の時間間隔のコンボ要素を取得
            SystemNameInfo jikanInfo = this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.HaishaJohoJikanKankakuKbn)
                && x.SystemNameCode == (int)this.cmbTimeInterval.SelectedValue).First();

            // スケジュールに時間間隔を設定
            c1ScheduleHaisha.CalendarInfo.TimeInterval = (TimeScaleEnum)jikanInfo.IntegerValue01;

            // 表示範囲を更新
            if (90 < (int)this.cmbDateRange.SelectedValue)
            {
                // 表示範囲が1日未満の場合
                this.c1ScheduleHaisha.CalendarInfo.StartDayTime = new TimeSpan(0, 0, 0);
                TimeSpan TimeSpan = new TimeSpan(0, 0, 0);
                if (DateRange.ThreeHours == (DateRange)this.cmbDateRange.SelectedValue) TimeSpan = new TimeSpan(3, 0, 0);
                if (DateRange.SixHours == (DateRange)this.cmbDateRange.SelectedValue) TimeSpan = new TimeSpan(6, 0, 0);
                if (DateRange.TwelveHours == (DateRange)this.cmbDateRange.SelectedValue) TimeSpan = new TimeSpan(12, 0, 0);
                this.c1Calendar1.CalendarInfo.EndDayTime = TimeSpan;
                this.c1ScheduleHaisha.ShowWorkTimeOnly = true;
                this.DoChangeDateRange((DateTime)this.dteTargetDate.Value, 1);
            }
            else
            {
                // 表示範囲が1日以上の場合
                this.c1ScheduleHaisha.CalendarInfo.StartDayTime = new TimeSpan(0, 0, 0);
                this.c1Calendar1.CalendarInfo.EndDayTime = new TimeSpan(0, 0, 0);
                this.c1ScheduleHaisha.ShowWorkTimeOnly = false;
                this.DoChangeDateRange((DateTime)this.dteTargetDate.Value, (int)this.cmbDateRange.SelectedValue);
            }

            // 時間間隔に一日を指定した場合、自動的にスケジュールのレイアウトが日単位に変更されるので時間単位に再設定
            if (C1.Win.C1Schedule.TimeLineStyleEnum.Days == this.c1ScheduleHaisha.TimeLineStyle)
            {
                this.c1ScheduleHaisha.TimeLineStyle = C1.Win.C1Schedule.TimeLineStyleEnum.Hours;
            }

        }

        private void DoChangeDateRange(DateTime BaseDate, int DateRange)
        {
            DateTime[] DateList = new DateTime[DateRange];
            for (int i = 0; i < DateRange; i++)
            {
                DateList[i] = BaseDate.AddDays(i);
            }
            c1Calendar1.SelectedDates = DateList;
        }

        /// <summary>
        /// 改ページ処理
        /// </summary>
        /// <param name="pageNum">増減ページ数</param>
        private void DoNewPage(int pageNum)
        {
            // 最大ページ数を超えた場合
            if (MaxPageNumber < NowPageNumber + pageNum)return;

            // 最小ページ数を超えた場合
            if (0 > NowPageNumber + pageNum)return;

            // 改ページ処理（日付の切り替え）
            DateTime CurrentDate = c1ScheduleJuchu.CurrentDate.AddDays(pageNum);
            c1ScheduleJuchu.GoToDate(CurrentDate);

            // 現在のページ数を設定
            NowPageNumber = NowPageNumber + pageNum;

            // 改ページボタンの切り替え
            this.DoChangeNewPageBtn();
        }

        /// <summary>
        /// 改ページボタン切替処理
        /// </summary>
        private void DoChangeNewPageBtn()
        {

            // 改ページボタンを活性化
            this.btnPreviousPage.Enabled = true;
            this.btnNextPage.Enabled = true;

            // 最大ページ数の場合
            if (NowPageNumber == MaxPageNumber)
            {
                // 次ページを非活性
                this.btnNextPage.Enabled = false;
            }

            // 最小ページ数の場合
            if (NowPageNumber == 0)
            {
                // 前ページを非活性
                this.btnPreviousPage.Enabled = false;
            }

            // ページ数を描画
            this.labelPage.Text = (NowPageNumber + 1) + "/" + (MaxPageNumber + 1);
        }

        /// <summary>
        /// DateTimeから日付の数値に変換します。
        /// </summary>
        /// <param name="numDay">DateTime</param>
        /// <param name="nullFlg">null許容（true:null許容、false:null不可）</param>
        private decimal? CnvDayDecimal(DateTime? dateTime, bool nullFlg)
        {
            if (dateTime == null)
            {
                // nullありの場合はnullで終了
                if (nullFlg) return null;
                dateTime = new DateTime(1950, 1, 1, 0, 0, 0);
            }

            return decimal.Parse(
                string.Concat(
                dateTime.Value.Year,
                dateTime.Value.Month.ToString("00"),
                dateTime.Value.Day.ToString("00")));

        }

        /// <summary>
        /// 予定情報を保持領域に格納します。。
        /// </summary>
        /// <param name="numDay">DateTime</param>
        /// <param name="addFlg">追加フラグ（true:追加、false:更新）</param>
        private void SetHoldingArea(HaishaNyuryokuInfo item, bool addFlg)
        {

            if (addFlg)
            {
                // 追加予定の場合
                if (!this._HaishaNyuryokuAdd.ContainsKey(item.HaishaId))
                {
                    this._HaishaNyuryokuAdd.Add(item.HaishaId, item);
                }
            }
            else
            {
                // 更新予定の場合
                if (!this._HaishaNyuryokuUpdata.ContainsKey(item.HaishaId))
                {
                    this._HaishaNyuryokuUpdata.Add(item.HaishaId, item);
                }
            }            
        }

        /// <summary>
        /// 往復区分を元に、予定に設定するステータスを取得します。
        /// </summary>
        /// <param name="ofukuKbn">往復区分</param>
        private Status GetStatus(int ofukuKbn)
        {
            Status result = null;

            // 往復区分でステータスを指定
            switch (ofukuKbn)
            {
                case (int)DefaultProperty.OfukuKbn.OKa:

                    // 往荷
                    result = this.StatusOKa;

                    break;
                case (int)DefaultProperty.OfukuKbn.FukuKa:

                    // 復荷
                    result = this.StatusFukuKa;

                    break;

                case (int)DefaultProperty.OfukuKbn.Ofuku:

                    // 往復
                    result = this.StatusOfuku;

                    break;

                default:

                    // 該当無
                    result = StatusNon;

                    break;

            }

            return result;
        }


        /// <summary>
        /// 配車情報を設定して配車詳細入力画面を表示します。
        /// </summary>
        /// <param name="info">配車情報</param>
        private HaishaShosaiNyuryokuFrame.HaishaDialogResult DoUpdateEntry(HaishaNyuryokuInfo haishainfo, JuchuInfo juchuInfo, int apintKbn)
        {
            // 選択行のTagから受注情報を取得

            try
            {
                if (apintKbn == (int)AppointKbn.Editable)
                {
                    // 編集フラグON
                    haishainfo.UppdateFlg = true;
                }
                else
                {
                    // 編集フラグOFF
                    haishainfo.UppdateFlg = false;
                }

                using (HaishaShosaiNyuryokuFrame f = new HaishaShosaiNyuryokuFrame(haishainfo, juchuInfo))
                {
                    this.Cursor = Cursors.WaitCursor;

                    f.InitFrame();
                    f.ShowDialog();

                    if (f.DialogResult == DialogResult.OK)
                    {
                        return f.ResultProcessing;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this.Cursor == Cursors.WaitCursor)
                {
                    this.Cursor = Cursors.Default;
                }
            }
            return HaishaShosaiNyuryokuFrame.HaishaDialogResult.Cancel;
        }

        /// <summary>
        /// 配車詳細情報を表示します。
        /// </summary>
        /// <param name="appoint">予定情報</param>
        private void SetHaishaShosai(Appointment appoint)
        {
            this.groupBoxJuchuShosai.Visible = false;
            this.groupBoxHaishaShosai.Visible = true;
            this.groupBoxKihonJoho.Visible = false;

            // appoint情報を取得
            AppointInfo appInfo = (AppointInfo)appoint.Tag;

            // 配車情報詳細の設定
            HaishaNyuryokuInfo item = appInfo.HaishaInfo;

            // 受注情報詳細の設定
            JuchuInfo itemJuchu = appInfo.JuchuInfo;

            // 発着日
            var taskStart = item.TaskStartDateTime;
            var taskEnd = item.ReuseYMD;
            appoint.Start = taskStart;
            appoint.End = taskEnd;

            // カテゴリの再設定。
            appoint.Categories.Clear();
            appoint.Categories.Add(CarDictionary[item.CarId]);

            this.edtHaishaBranchOfficeNM.Text = itemJuchu.BranchOfficeShortName;
            this.numNumber.Value = item.Number;
            this.edtFig.Text = item.FigName;
            this.numWeight.Value = item.Weight;
            this.numPrice.Value = item.PriceInPrice;
            this.dteStartYMD.Value = item.StartYMD;
            this.dteTaskStartYMD.Value = taskStart;
            this.dteTaskEndYMD.Value = item.TaskEndDateTime;
            this.dteHaishaReuseYMD.Value = item.ReuseYMD;

            this.edtHanro.Text = item.HanroName;
            this.edtHaishaStartPointNM.Text = item.StartPointName;
            this.edtHaishaEndPointNM.Text = item.EndPointName;
            this.edtBiko.Text = item.Biko;

            this.edtHaishaStaffNm.Text = item.StaffName;
            this.edtHaishaCarOfChartererNm.Text = item.TorihikiShortName;
            this.edtHaishaMagoYoshasaki.Text = item.MagoYoshasaki;

            // Category（配車情報）の詳細情報を取得
            HaishaCarInfo carInf = CarInfoDictionary.Where(x => x.CarId == item.CarId).First();

            // 乗務員・傭車先情報を配車詳細情報に設定し、乗務員名・傭車先名を取得
            string name = string.Empty;

            // 乗務員名・傭車先名を設定
            if (carInf.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
            {
                name = item.TorihikiShortName;
            }
            else 
            {
                name = item.StaffName;
            }

            // 件名
            appoint.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;


            // 受注情報
            this.lblNumber.Text = "/ " + itemJuchu.Number.ToString("###,###,##0.000#");
            this.lblWeight.Text = "/ " + itemJuchu.Weight.ToString("###,###,##0") + "kg";
            this.lblPrice.Text = "/ " + itemJuchu.Price.ToString("###,###,##0");

            decimal weightSum = decimal.Zero;
            // 発日時点の、同一車両の重量合計を算出
            for (int j = 0; j < this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Count; j++)
            {
                var app = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments[j];
                if (null != app.Tag)
                {
                    // 休日以外の場合
                    if (((AppointInfo)app.Tag).AppointKbn != (int)AppointKbn.Kyujitsu) 
                    {
                        // 配車情報詳細
                        HaishaNyuryokuInfo itemSub = ((AppointInfo)app.Tag).HaishaInfo;

                        // 発着日時
                        var start = itemSub.TaskStartDateTime;
                        var end = itemSub.TaskEndDateTime;

                        // 発日時点で同一車両の重複する予定
                        if (item.CarId == itemSub.CarId
                            && taskStart >= start
                            && taskStart <= end)
                        {
                            decimal weight = decimal.Zero;
                            if (itemSub.Weight != null) weight = itemSub.Weight.Value;

                            weightSum = weightSum + weight;
                        }
                    }
                }
            }

            // 重量合計
            this.numWeightSum.Value = weightSum;

        }

        /// <summary>
        /// 予定情報を分割します。
        /// </summary>
        /// <param name="appoint">予定情報</param>
        private void CopyAppointment(Appointment appoint)
        {
            // 追加中フラグON
            this.AddingFlg = true;

            // スケジュールを追加
            var appointAdd = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

            // 開始日時
            appointAdd.Start = appoint.Start;

            // 終了日時
            appointAdd.End = appoint.End;

            // 件名
            appointAdd.Subject = appoint.Subject;

            // 車両（カテゴリ）
            appointAdd.Categories.Add(appoint.Categories[0]);

            // 往復区分でステータスを指定
            appointAdd.BusyStatus = appoint.BusyStatus;

            // 予定情報（Appointment）の詳細情報をコピー
            var info = (AppointInfo)appoint.Tag;
            var infoAdd = info.Clone();

            // 配車詳細情報をコピー
            infoAdd.HaishaInfo = info.HaishaInfo.Clone();

            // 数量
            decimal number = info.HaishaInfo.Number;
            decimal pow = (decimal)Math.Pow(10, Property.UserProperty.GetInstance().JuchuNumberDecimalDigits);
            info.HaishaInfo.Number = (Math.Truncate((number / 2) * pow) / pow) + ((number * pow % 2)/ pow);
            infoAdd.HaishaInfo.Number = (Math.Truncate((number / 2) * pow) / pow);

            // 重量
            decimal weight = info.HaishaInfo.Weight.Value;
            info.HaishaInfo.Weight = Math.Truncate(weight / 2) + (weight % 2);
            infoAdd.HaishaInfo.Weight = Math.Truncate(weight / 2);

            // 金額_金額
            decimal priceinprice = info.HaishaInfo.PriceInPrice;
            info.HaishaInfo.PriceInPrice = Math.Truncate(priceinprice / 2) + (priceinprice % 2);
            infoAdd.HaishaInfo.PriceInPrice = Math.Truncate(priceinprice / 2);

            // 金額_通行料
            decimal tollfeeinprice = info.HaishaInfo.TollFeeInPrice;
            info.HaishaInfo.TollFeeInPrice = Math.Truncate(tollfeeinprice / 2) + (tollfeeinprice % 2);
            infoAdd.HaishaInfo.TollFeeInPrice = Math.Truncate(tollfeeinprice / 2);

            // 金額_附帯業務料
            decimal futaigyomuryoinprice = info.HaishaInfo.FutaigyomuryoInPrice;
            info.HaishaInfo.FutaigyomuryoInPrice = Math.Truncate(futaigyomuryoinprice / 2) + (futaigyomuryoinprice % 2);
            infoAdd.HaishaInfo.FutaigyomuryoInPrice = Math.Truncate(futaigyomuryoinprice / 2);

            // 金額_乗務員売上金額
            decimal jomuinuriagekingaku = info.HaishaInfo.JomuinUriageKingaku;
            info.HaishaInfo.JomuinUriageKingaku = Math.Truncate(jomuinuriagekingaku / 2) + (jomuinuriagekingaku % 2);
            infoAdd.HaishaInfo.JomuinUriageKingaku = Math.Truncate(jomuinuriagekingaku / 2);

            // 外税対象額
            decimal priceOutTaxCalc = info.HaishaInfo.PriceOutTaxCalc;
            info.HaishaInfo.PriceOutTaxCalc = Math.Truncate(priceOutTaxCalc / 2) + (priceOutTaxCalc % 2);
            infoAdd.HaishaInfo.PriceOutTaxCalc = Math.Truncate(priceOutTaxCalc / 2);

            // 外税額
            decimal priceOutTax = info.HaishaInfo.PriceOutTax;
            info.HaishaInfo.PriceOutTax = Math.Truncate(priceOutTax / 2) + (priceOutTax % 2);
            infoAdd.HaishaInfo.PriceOutTax = Math.Truncate(priceOutTax / 2);

            // 内税対象額
            decimal priceInTaxCalc = info.HaishaInfo.PriceInTaxCalc;
            info.HaishaInfo.PriceInTaxCalc = Math.Truncate(priceInTaxCalc / 2) + (priceInTaxCalc % 2);
            infoAdd.HaishaInfo.PriceInTaxCalc = Math.Truncate(priceInTaxCalc / 2);

            // 内税額
            decimal priceInTax = info.HaishaInfo.PriceInTax;
            info.HaishaInfo.PriceInTax = Math.Truncate(priceInTax / 2) + (priceInTax % 2);
            infoAdd.HaishaInfo.PriceInTax = Math.Truncate(priceInTax / 2);

            // 非課税対象額
            decimal priceNoTaxCalc = info.HaishaInfo.PriceNoTaxCalc;
            info.HaishaInfo.PriceNoTaxCalc = Math.Truncate(priceNoTaxCalc / 2) + (priceNoTaxCalc % 2);
            infoAdd.HaishaInfo.PriceNoTaxCalc = Math.Truncate(priceNoTaxCalc / 2);

            // 金額
            info.HaishaInfo.Price = info.HaishaInfo.PriceInPrice
                + info.HaishaInfo.TollFeeInPrice
                + info.HaishaInfo.FutaigyomuryoInPrice;
            infoAdd.HaishaInfo.Price = infoAdd.HaishaInfo.PriceInPrice
                + infoAdd.HaishaInfo.TollFeeInPrice
                + infoAdd.HaishaInfo.FutaigyomuryoInPrice;

            // 運賃
            info.HaishaInfo.Fee = info.HaishaInfo.Price;
            infoAdd.HaishaInfo.Fee = infoAdd.HaishaInfo.Price;
            // 運賃_金額
            info.HaishaInfo.PriceInFee = info.HaishaInfo.PriceInPrice;
            infoAdd.HaishaInfo.PriceInFee = infoAdd.HaishaInfo.PriceInPrice;
            // 運賃_通行料
            info.HaishaInfo.TollFeeInFee = info.HaishaInfo.TollFeeInPrice;
            infoAdd.HaishaInfo.TollFeeInFee = infoAdd.HaishaInfo.TollFeeInPrice;
            // 運賃_附帯業務料
            info.HaishaInfo.FutaigyomuryoInFee = info.HaishaInfo.FutaigyomuryoInPrice;
            infoAdd.HaishaInfo.FutaigyomuryoInFee = infoAdd.HaishaInfo.FutaigyomuryoInPrice;
            // 運賃外税対象額
            info.HaishaInfo.FeeOutTaxCalc = info.HaishaInfo.PriceOutTaxCalc;
            infoAdd.HaishaInfo.FeeOutTaxCalc = infoAdd.HaishaInfo.PriceOutTaxCalc;
            // 運賃外税額
            info.HaishaInfo.FeeOutTax = info.HaishaInfo.PriceOutTax;
            infoAdd.HaishaInfo.FeeOutTax = infoAdd.HaishaInfo.PriceOutTax;
            // 運賃内税対象額
            info.HaishaInfo.FeeInTaxCalc = info.HaishaInfo.PriceInTaxCalc;
            infoAdd.HaishaInfo.FeeInTaxCalc = infoAdd.HaishaInfo.PriceInTaxCalc;
            // 運賃内税額
            info.HaishaInfo.FeeInTax = info.HaishaInfo.PriceInTax;
            infoAdd.HaishaInfo.FeeInTax = infoAdd.HaishaInfo.PriceInTax;
            // 運賃非課税対象額
            info.HaishaInfo.FeeNoTaxCalc = info.HaishaInfo.PriceNoTaxCalc;
            infoAdd.HaishaInfo.FeeNoTaxCalc = infoAdd.HaishaInfo.PriceNoTaxCalc;
            // 運賃_附帯業務料
            info.HaishaInfo.FutaigyomuryoInFee = info.HaishaInfo.FutaigyomuryoInPrice;
            infoAdd.HaishaInfo.FutaigyomuryoInFee = infoAdd.HaishaInfo.FutaigyomuryoInPrice;

            // 傭車金額_金額
            decimal priceincharterprice = info.HaishaInfo.PriceInCharterPrice;
            info.HaishaInfo.PriceInCharterPrice = Math.Truncate(priceincharterprice / 2) + (priceincharterprice % 2);
            infoAdd.HaishaInfo.PriceInCharterPrice = Math.Truncate(priceincharterprice / 2);

            // 傭車金額_通行料
            decimal tollfeeincharterprice = info.HaishaInfo.TollFeeInCharterPrice;
            info.HaishaInfo.TollFeeInCharterPrice = Math.Truncate(tollfeeincharterprice / 2) + (tollfeeincharterprice % 2);
            infoAdd.HaishaInfo.TollFeeInCharterPrice = Math.Truncate(tollfeeincharterprice / 2);

            // 傭車外税対象額
            decimal charterPriceOutTaxCalc = info.HaishaInfo.CharterPriceOutTaxCalc;
            info.HaishaInfo.CharterPriceOutTaxCalc = Math.Truncate(charterPriceOutTaxCalc / 2) + (charterPriceOutTaxCalc % 2);
            infoAdd.HaishaInfo.CharterPriceOutTaxCalc = Math.Truncate(charterPriceOutTaxCalc / 2);

            // 傭車外税額
            decimal charterPriceOutTax = info.HaishaInfo.CharterPriceOutTax;
            info.HaishaInfo.CharterPriceOutTax = Math.Truncate(charterPriceOutTax / 2) + (charterPriceOutTax % 2);
            infoAdd.HaishaInfo.CharterPriceOutTax = Math.Truncate(charterPriceOutTax / 2);

            // 傭車内税対象額
            decimal charterPriceInTaxCalc = info.HaishaInfo.CharterPriceInTaxCalc;
            info.HaishaInfo.CharterPriceInTaxCalc = Math.Truncate(charterPriceInTaxCalc / 2) + (charterPriceInTaxCalc % 2);
            infoAdd.HaishaInfo.CharterPriceInTaxCalc = Math.Truncate(charterPriceInTaxCalc / 2);

            // 傭車内税額
            decimal charterPriceInTax = info.HaishaInfo.CharterPriceInTax;
            info.HaishaInfo.CharterPriceInTax = Math.Truncate(charterPriceInTax / 2) + (charterPriceInTax % 2);
            infoAdd.HaishaInfo.CharterPriceInTax = Math.Truncate(charterPriceInTax / 2);

            // 傭車内税額
            decimal charterPriceNoTaxCalc = info.HaishaInfo.CharterPriceNoTaxCalc;
            info.HaishaInfo.CharterPriceNoTaxCalc = Math.Truncate(charterPriceNoTaxCalc / 2) + (charterPriceNoTaxCalc % 2);
            infoAdd.HaishaInfo.CharterPriceNoTaxCalc = Math.Truncate(charterPriceNoTaxCalc / 2);

            // 傭車金額
            info.HaishaInfo.CharterPrice = info.HaishaInfo.PriceInCharterPrice + info.HaishaInfo.TollFeeInCharterPrice;
            infoAdd.HaishaInfo.CharterPrice = infoAdd.HaishaInfo.PriceInCharterPrice + infoAdd.HaishaInfo.TollFeeInCharterPrice;

            // 配車IDに仮IDを採番
            infoAdd.HaishaInfo.HaishaId = HaishaIdNumbering;
            HaishaIdNumbering++;

            // 受注詳細情報をコピー
            if (info.JuchuInfo != null) infoAdd.JuchuInfo = info.JuchuInfo.Clone();

            // 追加フラグ
            infoAdd.AddFlg = true;

            // 登録済みフラグ
            infoAdd.RegFlg = false;

            // 予定情報（Appointment）の詳細情報を設定
            appointAdd.Tag = infoAdd;

            // ラベル情報（分割先）
            appointAdd.Label = TsuikaHaisoLabel;

            // ラベル情報（分割元）
            if (!info.AddFlg) appoint.Label = ShuseiHaisoLabel;

            // 保持領域に格納(分割元)
            this.SetHoldingArea(info.HaishaInfo, info.AddFlg);

            // 保持領域に格納（分割先）
            this.SetHoldingArea(infoAdd.HaishaInfo, infoAdd.AddFlg);

            // 追加中フラグOFF
            this.AddingFlg = false;

        }
        /// <summary>
        /// 受注入力画面を表示します。（更新）
        /// </summary>
        /// <param name="JuchuSlipNo">受注伝票番号</param>
        /// <param name="JuchuId">受注ID</param>
        private void DoJuchuNyuryokuEntryUpdate(decimal JuchuSlipNo, decimal JuchuId)
        {
            // 選択行のTagから受注情報を取得

            try
            {
                using (JuchuNyuryokuFrame f = new JuchuNyuryokuFrame(JuchuSlipNo, JuchuId))
                {
                    this.Cursor = Cursors.WaitCursor;

                    f.InitFrame();
                    f.ShowDialog();

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this.Cursor == Cursors.WaitCursor)
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 受注入力画面を表示します。(新規)
        /// </summary>
        private void DoJuchuNyuryokuEntryAdd()
        {
            // 選択行のTagから受注情報を取得

            try
            {
                using (JuchuNyuryokuFrame f = new JuchuNyuryokuFrame())
                {
                    this.Cursor = Cursors.WaitCursor;

                    f.InitFrame();
                    f.ShowDialog();

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this.Cursor == Cursors.WaitCursor)
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 配車詳細入力画面を表示し、返却された値をもとに処理します。
        /// </summary>
        private void DispShosaiNyuryoku()
        {
            // 追加中フラグON
            this.AddingFlg = true;

            if (0 == c1ScheduleHaisha.SelectedAppointments.Count) return;
            var appoint = c1ScheduleHaisha.SelectedAppointments[0];
            if (null != appoint.Tag)
            {
                // 休日以外の場合
                if (((AppointInfo)appoint.Tag).AppointKbn != (int)AppointKbn.Kyujitsu) 
                {
                    // appoint情報を取得
                    AppointInfo appInfo = (AppointInfo)appoint.Tag;
                    switch (this.DoUpdateEntry(appInfo.HaishaInfo, appInfo.JuchuInfo, ((AppointInfo)appoint.Tag).AppointKbn))
                    {
                        case HaishaShosaiNyuryokuFrame.HaishaDialogResult.Update:
                            // 更新の場合

                            // 配車情報詳細の設定
                            HaishaNyuryokuInfo itemHaisha = appInfo.HaishaInfo;

                            // 詳細情報へ表示
                            this.SetHaishaShosai(appoint);

                            // 追加でない場合の色変え
                            if (!appInfo.AddFlg) appoint.Label = ShuseiHaisoLabel;

                            // 保持領域に格納
                            this.SetHoldingArea(itemHaisha, appInfo.AddFlg);

                            break;
                        case HaishaShosaiNyuryokuFrame.HaishaDialogResult.Copy:
                            // 分割の場合

                            // 予定の分割
                            this.CopyAppointment(appoint);

                            // 詳細情報へ表示
                            this.SetHaishaShosai(appoint);

                            break;

                        case HaishaShosaiNyuryokuFrame.HaishaDialogResult.Delete:
                            // 削除の場合

                            // 予定情報（Appointment）の詳細情報
                            var info = (AppointInfo)appoint.Tag;
                            var item = info.HaishaInfo;

                            if (info.AddFlg)
                            {
                                // 追加予定の場合
                                this._HaishaNyuryokuAdd.Remove(item.HaishaId);

                                // 既存データを一度削除した場合は、もともと保持している配車IDで削除リストに追加
                                if (appInfo.RegFlg)
                                {
                                    // 登録済みの予定の場合
                                    this._HaishaNyuryokuDel.Add(appInfo.DelHaishaId, item);
                                }
                            }
                            else
                            {
                                // 削除対象配車ID
                                if (appInfo.RegFlg) item.DelHaishaId = item.HaishaId;

                                // 更新予定の場合
                                this._HaishaNyuryokuUpdata.Remove(item.HaishaId);

                                // 登録済みの予定の場合
                                this._HaishaNyuryokuDel.Add(item.HaishaId, item);
                            }

                            // 削除
                            appoint.Delete();
                            break;
                    }
                }
            }

            // 追加中フラグOFF
            this.AddingFlg = false;
        }

        /// <summary>
        /// 配車情報の追加（受注一覧からのDragDrop）イベント
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_BeforeAppointmentDrop(object sender, CancelAppointmentEventArgs e)
        {
            if (null != e.Appointment.Tag)
            {
                // 編集可の場合
                if (((AppointInfo)e.Appointment.Tag).AppointKbn == (int)AppointKbn.Editable
                    || ((AppointInfo)e.Appointment.Tag).AppointKbn == (int)AppointKbn.Sakujo)
                {
                    // 表示車両がない場合は終了
                    if (c1ScheduleHaisha.DataStorage.CategoryStorage.Categories.Count() == 0)
                    {
                        string msg = "車両が表示されていないため、配車できません。";
                        MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        e.Cancel = true;
                        return;
                    }

                    // appoint情報を取得
                    AppointInfo appInfo = (AppointInfo)e.Appointment.Tag;

                    // 配置した場所のCarIdを取得
                    decimal carId = CarDictionary.First(x => x.Value == e.Appointment.Categories.First()).Key;

                    // Category（配車情報）の詳細情報を取得
                    HaishaCarInfo carInf = CarInfoDictionary.Where(x => x.CarId == carId).First();

                    // 追加データの場合は受注情報より
                    if (appInfo.AddFlg)
                    {

                        // 受注情報詳細を配車情報に設定
                        JuchuInfo JuchuItem = appInfo.JuchuInfo;

                        // 受注が傭車指定の場合、かつ配車先が傭車以外の場合はDrop処理をキャンセル
                        if (JuchuItem.CarKbn == (int)DefaultProperty.CarKbn.Yosha
                            && carInf.CarKbn != (int)DefaultProperty.CarKbn.Yosha)
                        {
                            string msg = FrameUtilites.GetDefineMessage("MW2202016",
                                new string[] { "この受注情報", "傭車車両の指定があるため、傭車車両以外への配車は" });
                            MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            e.Cancel = true;
                            return;
                        }
                        // 受注が自社車両指定の場合、かつ配車先が自社車両以外の場合はDrop処理をキャンセル
                        else if (JuchuItem.CarKbn != 0
                            && JuchuItem.CarKbn != (int)DefaultProperty.CarKbn.Yosha
                            && carInf.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                        {
                            string msg = FrameUtilites.GetDefineMessage("MW2202016",
                                new string[] { "この受注情報", "自社車両の指定があるため、自社車両以外への配車は" });
                            MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            e.Cancel = true;
                            return;
                        }
                    }

                    // 受注一覧からDropされた場合
                    if (appInfo.HaishaInfo == null)
                    {
                        // 受注情報詳細を配車情報に設定
                        JuchuInfo JuchuItem = appInfo.JuchuInfo;

                        // 再使用可能日時が未設定の場合は着日を設定
                        if (JuchuItem.ReuseYMD == DateTime.MinValue)
                        {
                            JuchuItem.ReuseYMD = JuchuItem.TaskEndDateTime;
                        }

                        //配車発着日初期値区分によって制御を分岐
                        switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaYMDDefaultKbn)
                        {
                            // 受注優先の場合
                            case (int)DefaultProperty.HaishaYMDDefaultKbn.Juchu:

                                // 出発日
                                e.Appointment.Start = JuchuItem.TaskStartDateTime;

                                // 到着日（再使用可能日時）
                                e.Appointment.End = JuchuItem.ReuseYMD;

                                break;

                            // ドラッグ優先の場合
                            case (int)DefaultProperty.HaishaYMDDefaultKbn.Drag:

                                // 期間
                                int jikan = Int32.Parse(JuchuItem.KoteiJikan.ToString("000000").Substring(0, 2));
                                e.Appointment.Duration = new TimeSpan(JuchuItem.KoteiNissu * 24 + jikan, 0, 0);
                                break;
                            default:
                                break;
                        }
                    }

                    // 傭車以外の場合は休日チェック
                    if (carInf.CarKbn != (int)DefaultProperty.CarKbn.Yosha) 
                    {
                        // 発日
                        DateTime hatsuYMD = appInfo.HaishaInfo == null
                            ? e.Appointment.Start
                            : appInfo.HaishaInfo.StartYMD;

                        // 発日がリサイズ期間外の場合
                        if (hatsuYMD < e.Appointment.Start || e.Appointment.End < hatsuYMD)
                        {
                            // リサイズ後の開始日を発日に設定
                            hatsuYMD = e.Appointment.Start;
                        }

                        // 休日チェック
                        this.CheckKyujitsu(e,
                        e.Appointment.Start,
                        hatsuYMD,
                         e.Appointment.End,
                         this.GetDriverId(carInf, appInfo.JuchuInfo));
                    }
                }
                else 
                {
                    // 休日の場合は移動不可
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 乗務員・傭車先情報を配車詳細情報に設定し、乗務員名・傭車先名を返却します。
        /// </summary>
        /// <param name="item">配車詳細情報</param>
        /// <param name="carInf">配車車両情報</param>
        /// <param name="JuchuItem">受注情報</param>
        private string SetJishaSharyoYousha(HaishaNyuryokuInfo item, HaishaCarInfo carInf, JuchuInfo JuchuItem)
        {
            // 乗務員名・傭車先名
            string name = string.Empty;

            // 配車先が傭車の場合
            if (carInf.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
            {

                // 受注に紐づく傭車先がある場合
                if (JuchuItem.CarOfChartererId != decimal.Zero)
                {
                    item.CarOfChartererId = JuchuItem.CarOfChartererId;
                    item.TorihikiCd = JuchuItem.TorihikiCd;
                    item.TorihikiShortName = JuchuItem.TorihikiShortName;
                    item.LicPlateCarNo = JuchuItem.LicPlateCarNo;
                    name = JuchuItem.TorihikiShortName;
                }

                // 受注に紐づく傭車先がない場合
                else
                {
                    // 配車先に紐づく傭車先がある場合
                    if (carInf.TorihikiId != decimal.Zero)
                    {
                        item.CarOfChartererId = carInf.TorihikiId;
                        item.TorihikiCd = carInf.TorihikiCd;
                        item.TorihikiShortName = carInf.TorihikiShortName;
                        name = carInf.TorihikiShortName;
                    }

                    // 配車先に紐づく傭車先がない場合
                    else
                    {
                        // 受注で指定がある場合
                        item.CarOfChartererId = decimal.Zero;
                        item.TorihikiCd = 0;
                        item.TorihikiShortName = string.Empty;
                        item.LicPlateCarNo = string.Empty;
                        name = string.Empty;
                    }

                }

                // 車両区分
                item.CarKbn = (int)DefaultProperty.CarKbn.Yosha;

                // 乗務員情報をクリア
                item.DriverId = decimal.Zero;
                item.StaffCd = 0;
                item.StaffName = string.Empty;
            }

            // 自社車両の場合
            else
            {

                // 受注に紐づく乗務員がいる場合
                if (JuchuItem.DriverId != decimal.Zero)
                {
                    item.DriverId = JuchuItem.DriverId;
                    item.StaffCd = JuchuItem.StaffCd;
                    item.StaffName = JuchuItem.StaffName;
                    name = JuchuItem.StaffName;

                }

                // 受注に紐づく乗務員がいない場合
                else
                {
                    // 配車先に紐づく乗務員がいる場合
                    if (carInf.DriverId != decimal.Zero)
                    {
                        item.DriverId = carInf.DriverId;
                        item.StaffCd = carInf.StaffCd;
                        item.StaffName = carInf.StaffName;
                        name = carInf.StaffName;
                    }

                    // 配車先に紐づく乗務員がいない場合
                    else
                    {
                        item.DriverId = decimal.Zero;
                        item.StaffCd = 0;
                        item.StaffName = string.Empty;
                        name = string.Empty;
                    }
                }

                // 車両区分
                item.CarKbn = (int)DefaultProperty.CarKbn.Jisha;

                // 車番
                item.LicPlateCarNo = string.Empty;

                // 傭車先をクリア
                item.CarOfChartererId = decimal.Zero;
                item.TorihikiCd = 0;
                item.TorihikiName = string.Empty;
                item.TorihikiShortName = string.Empty;
            }

            // 受注に紐づく車種がある場合
            if (JuchuItem.CarKindId != decimal.Zero)
            {
                item.CarKindId = JuchuItem.CarKindId;
                item.CarKindCode = JuchuItem.CarKindCode;
                item.CarKindName = JuchuItem.CarKindName;
            }

            // 受注に紐づく車種がない場合
            else
            {
                // 配車先に紐づく車種がある場合
                if (carInf.CarKindId != decimal.Zero)
                {
                    item.CarKindId = carInf.CarKindId;
                    item.CarKindCode = carInf.CarKindCd;
                    item.CarKindName = carInf.CarKindName;
                }

                // 配車先に紐づく車種がない場合
                else
                {
                    item.CarKindId = decimal.Zero;
                    item.CarKindCode = 0;
                    item.CarKindName = string.Empty;
                }
            }

            // 車両情報コードを設定
            item.CarCode = carInf.CarCode;

            return name;
        }


        /// <summary>
        /// 乗務員・傭車先情報を配車詳細情報に設定し、乗務員IDを返却します。
        /// </summary>
        /// <param name="carInf">配車車両情報</param>
        /// <param name="JuchuItem">受注情報</param>
        private decimal GetDriverId(HaishaCarInfo carInf, JuchuInfo JuchuItem)
        {
            // 乗務員Id
            decimal driverId = decimal.Zero;

            // 配車先が自社車両の場合
            if (carInf.CarKbn != (int)DefaultProperty.CarKbn.Yosha)
            {

                // 受注に紐づく乗務員がいる場合
                if (JuchuItem.DriverId != decimal.Zero)
                {
                    driverId = JuchuItem.DriverId;
                }

                // 受注に紐づく乗務員がいない場合
                else
                {
                    // 配車先に紐づく乗務員がいる場合
                    if (carInf.DriverId != decimal.Zero)
                    {
                        driverId = carInf.DriverId;
                    }

                    // 配車先に紐づく乗務員がいない場合
                    else
                    {
                        driverId = decimal.Zero;
                    }
                }
            }

            return driverId;
        }

        /// <summary>
        /// 休日用の配色情報を取得
        /// </summary>
        private void GetKyukaHaishoku()
        {
            //休日区分の配色リスト取得
            IList<HaishokuExInfo> haishokuList = this._DalUtil.Haishoku.GetListEx(
                new HaishokuExSearchParameter()
                {
                    TableKey = (int)DefaultProperty.HaishokuTableKbn.SystemName,
                    FunctionKey = (int)DefaultProperty.SystemNameKbn.KyujitsuKbn
                });

            //休日区分一覧
            IList<SystemNameInfo> kyujitsuKbnList =
            UserProperty.GetInstance().SystemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.KyujitsuKbn)).ToList();

            //休日区分件数繰り返す
            foreach (SystemNameInfo info in kyujitsuKbnList)
            {
                Color backcolor = FrameUtilites.GetKyujitsuKbnBackColor((DefaultProperty.KyujitsuKbn)info.SystemNameCode);
                Color forecolor = FrameUtilites.GetKyujitsuKbnForeColor((DefaultProperty.KyujitsuKbn)info.SystemNameCode);

                var wk_color_list = haishokuList.Where(x => x.TargetKey == Convert.ToDecimal(info.SystemNameCode));

                if (wk_color_list != null && 0 < wk_color_list.Count())
                {
                    try
                    {
                        if (((HaishokuExInfo)(wk_color_list.ToList()[0])).BackColor != null)
                        {
                            backcolor = ColorTranslator.FromOle(((HaishokuExInfo)(wk_color_list.ToList()[0])).BackColor.Value);
                        }
                        if (((HaishokuExInfo)(wk_color_list.ToList()[0])).ForeColor != null)
                        {
                            forecolor = ColorTranslator.FromOle(((HaishokuExInfo)(wk_color_list.ToList()[0])).ForeColor.Value);
                        }
                    }
                    finally
                    {
                        //何もしない
                        ;
                    }
                }

                this._KyujitsuBackColorDic.Add(info.SystemNameCode, backcolor);
                this._KyujitsuForeColorDic.Add(info.SystemNameCode, forecolor);
            }
        }

        /// <summary>
        /// 予定のフォーマット指定時のイベント
        /// </summary>
        private void c1ScheduleHaisha_BeforeAppointmentFormat(object sender, BeforeAppointmentFormatEventArgs e)
        {

            if (e.Appointment != null)
            {
                // appoint情報を取得
                AppointInfo appInfo = (AppointInfo)e.Appointment.Tag;

                if (appInfo != null)
                {

                    switch (appInfo.AppointKbn)
                    {
                        //　編集可の場合
                        case (int)AppointKbn.Editable:
                            // 追加の場合はラベルを再設定
                            if (appInfo.AddFlg)
                            {
                                // ラベル情報（背景色など）
                                if (e.Appointment.Label == null)
                                {
                                    e.Appointment.Label = TsuikaHaisoLabel;
                                }
                            }

                            break;

                        //　休日の場合
                        case (int)AppointKbn.Kyujitsu:

                            // 16進数へ変換
                            String htmlColor = String.Format("#{0:X2}{1:X2}{2:X2}"
                                , appInfo.KyujitsuFontColor.R
                                , appInfo.KyujitsuFontColor.G
                                , appInfo.KyujitsuFontColor.B);

                            e.Text = "<p style='color:" + htmlColor + "'>" + e.Text + "</p>";

                            // ラベル情報（背景色など）
                            if (e.Appointment.Label == null)
                            {
                                e.Appointment.Label = new C1.C1Schedule.Label(appInfo.KyujitsuBackColor, "休日", "休日キャプション");
                            }

                            break;

                        // 編集不可の場合
                        case (int)AppointKbn.NoEditable:

                            e.Text = "<p style='color:white'>" + e.Text + "</p>";

                            // ラベル情報（背景色など）
                            if (e.Appointment.Label == null)
                            {
                                e.Appointment.Label = NoEditLabel;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// 予定のリサイズイベント
        /// </summary>
        private void c1ScheduleHaisha_BeforeAppointmentResize(object sender, CancelAppointmentEventArgs e)
        {
            // 変更以外の場合
            if (((AppointInfo)e.Appointment.Tag).AppointKbn != (int)AppointKbn.Editable)
            {
                // 変更以外の場合は変更不可
                e.Cancel = true;
            }
            else 
            {
                // 配置した場所のCarIdを取得
                decimal carId = CarDictionary.First(x => x.Value == e.Appointment.Categories.First()).Key;

                // Category（配車情報）の詳細情報を取得
                HaishaCarInfo carInf = CarInfoDictionary.Where(x => x.CarId == carId).First();

                // 傭車以外の場合は休日チェック
                if (carInf.CarKbn != (int)DefaultProperty.CarKbn.Yosha)
                {
                    // 発日を取得
                    DateTime hatsuYMD = ((AppointInfo)e.Appointment.Tag).HaishaInfo.StartYMD;

                    // 発日がリサイズ期間外の場合
                    if (hatsuYMD < e.Appointment.Start || e.Appointment.End < hatsuYMD)
                    {
                        // リサイズ後の開始日を発日に設定
                        hatsuYMD = e.Appointment.Start;
                    }

                    // 休日チェック
                    this.CheckKyujitsu(e,
                        e.Appointment.Start,
                        hatsuYMD,
                         e.Appointment.End,
                         ((AppointInfo)e.Appointment.Tag).HaishaInfo.DriverId);
                }
            }
        }

        /// <summary>
        /// 受注情報検索条件欄の排他用条件の表示
        /// </summary>
        private void DispHaitaJoken()
        {
            //配車入力条件区分と営業所管理区分によって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.None:


                    if (UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn == (int)DefaultProperty.EigyoshoKanriKbn.Use)
                    {
                        // 営業所名を表示
                        groupBox3.Text += ("（" + this.HaishaNyuryokuConditionInfo.BranchOfficeName + "）");
                    }

                    break;
                default:

                    string str = "（";

                    // 営業所名を表示
                    str += this.HaishaNyuryokuConditionInfo.BranchOfficeName + " ";

                    // 個別の場合
                    if (this.HaishaNyuryokuConditionInfo.ShiteiKobetsuChecked)
                    {
                        str += this.HaishaNyuryokuConditionInfo.CheckCodeList.Replace(Environment.NewLine, string.Empty);
                    }
                    // グループの場合
                    else
                    {
                        str += this.HaishaNyuryokuConditionInfo.GroupCode + ":" + this.HaishaNyuryokuConditionInfo.GroupName;
                    }

                    string strBk = str;

                    // バイト数で文字列をカット
                    str = this.LeftCutByte(str, 30);

                    if (!strBk.Equals(str)) str += "…";

                    str += "）";
                    groupBox3.Text += str;

                    break;
            }
        }

        /// <summary>
        /// エンコードで指定したバイト数以下で文字列を切り詰めるて返却する。
        /// </summary>
        private string LeftCutByte(string s, int maxByteCount)
        {
            Encoding encoding = Encoding.GetEncoding("shift_jis");

            var bytes = encoding.GetBytes(s);
            if (bytes.Length <= maxByteCount) return s;

            var result = s.Substring(0,
                encoding.GetString(bytes, 0, maxByteCount).Length);

            while (encoding.GetByteCount(result) > maxByteCount)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// 予定の期間内に休日があるかチェックする
        /// </summary>
        private void CheckKyujitsu(CancelAppointmentEventArgs e, DateTime stratYMD, DateTime hatsuYMD, DateTime endYMD, Decimal? driverId)
        {
            //配車休日チェックフラグがfalseの場合は終了
            if (!UserProperty.GetInstance().SystemSettingsInfo.HaishaKyujitsuCheckFlag) return;

            // ドライバーが未設定の場合は終了
            if (driverId == null || driverId.Value.CompareTo(decimal.Zero) == 0) return;

            bool checkFlag = false;

            // 積日
            DateTime sYMD = new DateTime(stratYMD.Year, stratYMD.Month, stratYMD.Day, 0, 0, 0);

            // 休日情報の取得
            var list = _HaishaNyuryoku.GetCheckKyujitsuCalendar(sYMD, sYMD, driverId);

            checkFlag = list.Count() > 0;

            // 積日が休日に含まれるか
            var kl = list.Where(x => x.DriverId == driverId.Value
             && x.HizukeYMD == sYMD);

            if (!checkFlag)
            {
                // 発日
                DateTime hYMD = new DateTime(hatsuYMD.Year, hatsuYMD.Month, hatsuYMD.Day, 0, 0, 0);
                // 着日
                DateTime eYMD = new DateTime(endYMD.Year, endYMD.Month, endYMD.Day, 23, 59, 59);

                // 休日情報の取得
                var list2 = _HaishaNyuryoku.GetCheckKyujitsuCalendar(hYMD, endYMD, driverId);

                checkFlag = list2.Count() > 0;
            }

            if (!checkFlag) return;

            DialogResult wk_result =
            MessageBox.Show(
            "配車予定期間に乗務員の休日が含まれておりますがよろしいでしょうか？",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (wk_result == DialogResult.No)
            {
                //Noの場合は処理中断
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// 配車情報を再描画
        /// </summary>
        private void HaishaShowSchedule()
        {
            // 初期化中は処理中断
            if (this.InitFlg) return;

            // 追加中フラグON
            this.AddingFlg = true;

            //描画を停止
            this.c1ScheduleHaisha.SuspendLayout();

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try {

                if (!dteTargetDate.Value.HasValue)
                    return;
                this.ShowSchedule(this.GetScreen());
            }
            finally
            {
                // 追加中フラグOFF
                this.AddingFlg = false;

                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;

                // 描画を再開
                this.c1ScheduleHaisha.ResumeLayout();
            }
        }

        /// <summary>
        /// 排他対象か判定
        /// </summary>
        /// <param name="haitaInfo">排他情報</param>
        /// <param name="haishaitem">配車情報</param>
        /// <param name="jutyuitem">受注情報</param>
        /// <returns></returns>
        private Boolean JudgeExclusive(List<HaitaInfo> haitaInfo, HaishaNyuryokuInfo haishaitem, JuchuInfo jutyuitem,
            ToraDonSystemPropertyInfo toraDonSystemPropertyInfo)
        {
            // 排他入力情報が排他情報と一致するか
            if (haitaInfo != null && haitaInfo.Where(x => x.HaishaId == haishaitem.HaishaId).Count() == 0)
            {
                return true;
            }

            // 最終月次集計月を取得する
            DateTime wk_date = toraDonSystemPropertyInfo.LastSummaryOfMonthDate;
            DateTime last_sum_of_monthday;
            if (wk_date > DateTime.MinValue)
            {
                last_sum_of_monthday = NSKUtil.MonthLastDay(wk_date);
            }
            else
            {
                last_sum_of_monthday = wk_date;
            }

            //TODO 性能問題のためコメント化中 START
            //if (haishaitem != null) 
            //{

            //    // 請求済みの場合(請求締切日)
            //    if (haishaitem.MinClmFixDate != DateTime.MinValue)
            //    {
            //        return true;
            //    }

            //    // 支払済みの場合(傭車支払締切日)
            //    if (haishaitem.MinCharterPayFixDate != DateTime.MinValue)
            //    {
            //        return true;
            //    }
            //}
            //TODO 性能問題のためコメント化中 END

            // 受注情報の月次締処理済みの場合
            if (haishaitem == null && (this.IsMonthlyTotaled(jutyuitem.AddUpYMD, jutyuitem.FixFlag, last_sum_of_monthday) ||
                this.IsMonthlyTotaled(jutyuitem.CharterAddUpYMD, jutyuitem.CharterFixFlag, last_sum_of_monthday)))
            {
                return true;
            }

            // 受注情報のトラDon月次締処理済みの場合
            else if (haishaitem == null && (this.IsMonthlyTotaled(jutyuitem.AddUpDate, jutyuitem.ToraDonFixFlag, last_sum_of_monthday) ||
                this.IsMonthlyTotaled(jutyuitem.CharterAddUpDate, jutyuitem.ToraDonCharterFixFlag, last_sum_of_monthday)))
            {
                return true;
            }

            // 配車情報の月次締処理済みの場合
            else if (haishaitem != null && (this.IsMonthlyTotaled(haishaitem.AddUpYMD, haishaitem.FixFlag, last_sum_of_monthday) ||
                this.IsMonthlyTotaled(haishaitem.CharterAddUpYMD, haishaitem.CharterFixFlag, last_sum_of_monthday)))
            {
                return true;
            }

            //TODO 性能問題のためコメント化中 START
            //// 配車情報のトラDon月次締処理済みの場合
            //else if (haishaitem != null && (this.IsMonthlyTotaled(haishaitem.AddUpDate, haishaitem.ToraDonFixFlag, last_sum_of_monthday) ||
            //    this.IsMonthlyTotaled(haishaitem.CharterAddUpDate, haishaitem.ToraDonCharterFixFlag, last_sum_of_monthday)))
            //{
            //    return true;
            //}
            //TODO 性能問題のためコメント化中 END

            else
            {
                return false;
            }
        }

        private bool IsMonthlyTotaled(DateTime addUpDate, bool fixFlag, DateTime lastSumOfMonthDay)
        {
            //戻り値用
            bool rt_val = false;

            if (lastSumOfMonthDay != DateTime.MinValue && addUpDate != DateTime.MinValue)
            {
                //確定済みで、計上日が最終月次集計月(末日)以前の場合は集計済みとする
                if (addUpDate <= lastSumOfMonthDay && fixFlag)
                {
                    rt_val = true;
                }
            }

            return rt_val;
        }

        /// <summary>
        /// 受注一覧のフォーマット指定時のイベント
        /// </summary>
        private void c1ScheduleJuchu_BeforeAppointmentFormat(object sender, BeforeAppointmentFormatEventArgs e)
        {
            if (e.Appointment != null)
            {
                // appoint情報を取得
                AppointInfo appInfo = (AppointInfo)e.Appointment.Tag;

                if (appInfo != null)
                {

                    switch (appInfo.AppointKbn)
                    {
                        //　編集可の場合
                        case (int)AppointKbn.Editable:
                            // 追加の場合はラベルを再設定
                            if (appInfo.AddFlg)
                            {
                                // ラベル情報（背景色など）
                                if (e.Appointment.Label == null)
                                {
                                    e.Appointment.Label = NotHaisoLabel;
                                }
                            }

                            break;

                        //　削除の場合
                        case (int)AppointKbn.Sakujo:
                            // 追加の場合はラベルを再設定
                            if (appInfo.AddFlg)
                            {
                                // ラベル情報（背景色など）
                                if (e.Appointment.Label == null)
                                {
                                    e.Appointment.Label = DeleteHaisoLabel;
                                }
                            }

                            break;

                        // 編集不可の場合
                        case (int)AppointKbn.NoEditable:

                            e.Text = "<p style='color:white'>" + e.Text + "</p>";

                            // ラベル情報（背景色など）
                            if (e.Appointment.Label == null)
                            {
                                e.Appointment.Label = NoEditLabel;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 受注情報から配車情報を設定
        /// </summary>
        /// <param name="appoint">予定情報</param>
        /// <param name="bulkFlg">一括フラグ</param>
        /// <returns></returns>
        private void JudgeExclusive(Appointment appoint, bool bulkFlg)
        {
            // appoint情報を取得
            AppointInfo appInfo = (AppointInfo)appoint.Tag;

            // 休日以外の場合
            if (appInfo.AppointKbn != (int)AppointKbn.Kyujitsu)
            {

                // 配車情報詳細の設定
                HaishaNyuryokuInfo item = null;

                // 追加データの場合は受注情報より
                if (appInfo.AddFlg && appInfo.HaishaInfo == null)
                {
                    // 受注情報詳細を配車情報に設定
                    JuchuInfo JuchuItem = appInfo.JuchuInfo;
                    item = new HaishaNyuryokuInfo();

                    item.JuchuId = JuchuItem.JuchuId;
                    item.CarId = JuchuItem.CarId;
                    item.CarLicPlateCarNo = JuchuItem.CarLicPlateCarNo;

                    item.Number = JuchuItem.Number;
                    item.FigId = JuchuItem.FigId;
                    item.Weight = JuchuItem.Weight;
                    item.Price = JuchuItem.Price;
                    item.PriceInPrice = JuchuItem.PriceInPrice;
                    item.TollFeeInPrice = JuchuItem.TollFeeInPrice;
                    item.PriceOutTaxCalc = JuchuItem.PriceOutTaxCalc;
                    item.PriceOutTax = JuchuItem.PriceOutTax;
                    item.PriceInTaxCalc = JuchuItem.PriceInTaxCalc;
                    item.PriceInTax = JuchuItem.PriceInTax;
                    item.PriceNoTaxCalc = JuchuItem.PriceNoTaxCalc;
                    item.TaxDispKbn = JuchuItem.TaxDispKbn;
                    item.AddUpYMD = JuchuItem.AddUpYMD;
                    item.FixFlag = JuchuItem.FixFlag;
                    item.CharterPrice = JuchuItem.CharterPrice;
                    item.PriceInCharterPrice = JuchuItem.PriceInCharterPrice;
                    item.TollFeeInCharterPrice = JuchuItem.TollFeeInCharterPrice;
                    item.CharterPriceOutTaxCalc = JuchuItem.CharterPriceOutTaxCalc;
                    item.CharterPriceOutTax = JuchuItem.CharterPriceOutTax;
                    item.CharterPriceInTaxCalc = JuchuItem.CharterPriceInTaxCalc;
                    item.CharterPriceInTax = JuchuItem.CharterPriceInTax;
                    item.CharterPriceNoTaxCalc = JuchuItem.CharterPriceNoTaxCalc;
                    item.CharterTaxDispKbn = JuchuItem.CharterTaxDispKbn;
                    item.CharterAddUpYMD = JuchuItem.CharterAddUpYMD;
                    item.CharterFixFlag = JuchuItem.CharterFixFlag;
                    item.Fee = JuchuItem.Fee;
                    item.PriceInFee = JuchuItem.PriceInFee;
                    item.TollFeeInFee = JuchuItem.TollFeeInFee;
                    item.FeeOutTaxCalc = JuchuItem.FeeOutTaxCalc;
                    item.FeeOutTax = JuchuItem.FeeOutTax;
                    item.FeeInTaxCalc = JuchuItem.FeeInTaxCalc;
                    item.FeeInTax = JuchuItem.FeeInTax;
                    item.FeeNoTaxCalc = JuchuItem.FeeNoTaxCalc;
                    item.StartPointId = JuchuItem.StartPointId;
                    item.StartPointName = JuchuItem.StartPointName;
                    item.EndPointId = JuchuItem.EndPointId;
                    item.EndPointName = JuchuItem.EndPointName;
                    item.BranchOfficeCode = JuchuItem.BranchOfficeCode;
                    item.TokuisakiCode = JuchuItem.TokuisakiCode;
                    item.TokuisakiName = JuchuItem.TokuisakiName;
                    item.StartPointCode = JuchuItem.StartPointCode;
                    item.EndPointCode = JuchuItem.EndPointCode;
                    item.Weight = JuchuItem.Weight;
                    item.FigCode = JuchuItem.FigCode;
                    item.FigName = JuchuItem.FigName;
                    item.HanroName = JuchuItem.HanroName;
                    item.OfukuKbn = JuchuItem.OfukuKbn;
                    item.ItemId = JuchuItem.ItemId;
                    item.ItemCode = JuchuItem.ItemCode;
                    item.ItemName = JuchuItem.ItemName;
                    item.OwnerId = JuchuItem.OwnerId;
                    item.OwnerCode = JuchuItem.OwnerCode;
                    item.OwnerName = JuchuItem.OwnerName;
                    item.TaikijikanNumber = JuchuItem.TaikijikanNumber;
                    item.TaikijikanFigId = JuchuItem.TaikijikanFigId;
                    item.TaikijikanAtPrice = JuchuItem.TaikijikanAtPrice;
                    item.TaikijikanryoInPrice = JuchuItem.TaikijikanryoInPrice;
                    item.NizumiryoInPrice = JuchuItem.NizumiryoInPrice;
                    item.NioroshiryoInPrice = JuchuItem.NioroshiryoInPrice;
                    item.FutaigyomuryoInPrice = JuchuItem.FutaigyomuryoInPrice;
                    item.TaikijikanryoInFee = JuchuItem.TaikijikanryoInFee;
                    item.NizumiryoInFee = JuchuItem.NizumiryoInFee;
                    item.NioroshiryoInFee = JuchuItem.NioroshiryoInFee;
                    item.FutaigyomuryoInFee = JuchuItem.FutaigyomuryoInFee;
                    item.NizumijikanNumber = JuchuItem.NizumijikanNumber;
                    item.NizumijikanFigId = JuchuItem.NizumijikanFigId;
                    item.NizumijikanAtPrice = JuchuItem.NizumijikanAtPrice;
                    item.NioroshijikanNumber = JuchuItem.NioroshijikanNumber;
                    item.NioroshijikanFigId = JuchuItem.NioroshijikanFigId;
                    item.NioroshijikanAtPrice = JuchuItem.NioroshijikanAtPrice;
                    item.FutaigyomujikanNumber = JuchuItem.FutaigyomujikanNumber;
                    item.FutaigyomujikanFigId = JuchuItem.FutaigyomujikanFigId;
                    item.FutaigyomujikanAtPrice = JuchuItem.FutaigyomujikanAtPrice;
                    item.JomuinUriageDogakuFlag = JuchuItem.JomuinUriageDogakuFlag;
                    item.JomuinUriageKingaku = JuchuItem.JomuinUriageKingaku;
                    item.AtPrice = JuchuItem.AtPrice;
                    item.JuchuTaskStartDateTime = JuchuItem.TaskStartDateTime;
                    item.JuchuTaskEndDateTime = JuchuItem.TaskEndDateTime;
                    item.BranchOfficeId = JuchuItem.BranchOfficeId;

                    item.ReuseYMD = JuchuItem.ReuseYMD;
                    item.MagoYoshasaki = JuchuItem.MagoYoshasaki;
                    item.JuchuReuseYMD = JuchuItem.ReuseYMD;

                    // 登録済みフラグ
                    item.RegFlg = appInfo.RegFlg;

                    // 新規の為、内部持ち回り用に配車IDを仮採番
                    item.HaishaId = HaishaIdNumbering;
                    HaishaIdNumbering++;

                    // 配車情報に上書き
                    appInfo.HaishaInfo = item;

                    // 削除対象配車ID
                    item.DelHaishaId = appInfo.DelHaishaId;

                    // 一括配車の場合
                    if (bulkFlg) 
                    {

                        // 再使用可能日時が未設定の場合は着日を設定
                        if (JuchuItem.ReuseYMD == DateTime.MinValue)
                        {
                            JuchuItem.ReuseYMD = JuchuItem.TaskEndDateTime;
                        }

                        // 出発日
                        appoint.Start = JuchuItem.TaskStartDateTime;

                        // 到着日（再使用可能日時）
                        appoint.End = JuchuItem.ReuseYMD;

                        appoint.Categories.Add(CarDictionary[item.CarId]);

                    }

                    // 元の着日を取得
                    DateTime chakuYMD = JuchuItem.TaskEndDateTime;

                    // 元の着日がリサイズ期間外の場合
                    if (chakuYMD < appoint.Start || appoint.End < chakuYMD)
                    {
                        // リサイズ後の開始日を取得
                        chakuYMD = appoint.End;
                    }

                    // 積載日付
                    item.TaskStartDateTime = appoint.Start;

                    // 到着日
                    item.TaskEndDateTime = chakuYMD;

                    // 再使用可能日時
                    item.ReuseYMD = appoint.End;

                    // 出発日
                    item.StartYMD = appoint.Start;

                    // 一括配車以外の場合
                    if (!bulkFlg) 
                    {
                        // 配置した場所のCarIdを取得
                        item.CarId = CarDictionary.First(x => x.Value == appoint.Categories.First()).Key;
                    }

                    // Category（配車情報）の詳細情報を取得
                    HaishaCarInfo carInf = CarInfoDictionary.Where(x => x.CarId == item.CarId).First();

                    // 乗務員・傭車先情報を配車詳細情報に設定し、乗務員名・傭車先名を取得
                    string name = SetJishaSharyoYousha(item, carInf, JuchuItem);

                    // 件名
                    appoint.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;

                    // 往復区分でステータスを指定
                    appoint.BusyStatus = GetStatus(item.OfukuKbn);

                    // ラベル情報（背景色など）
                    appoint.Label = TsuikaHaisoLabel;

                    // 予定区分
                    appInfo.AppointKbn = (int)AppointKbn.Editable;

                    // 削除対象の場合はクリアしない
                    if (!JuchuItem.DelTargetFlg)
                    {
                        // 削除リストにある場合は、削除リストから削除
                        this._HaishaNyuryokuDel.Remove(appInfo.DelHaishaId);
                    }

                    // 保持領域に格納
                    this.SetHoldingArea(item, appInfo.AddFlg);

                    // 受注情報の営業所と配車情報の営業所コンボボックスが一致しない場合、配車情報を再描画
                    decimal jucB = JuchuItem.BranchOfficeId;
                    decimal haiB = (decimal?)cmbHaishaBranchOffice.SelectedValue ?? 0;
                    if (haiB.CompareTo(decimal.Zero) != 0
                        && jucB.CompareTo(haiB) != 0) this.HaishaShowSchedule();
                }
            }

            // 一括配車以外の場合
            if (!bulkFlg) 
            {
                // 現在ページ数
                int page = this.NowPageNumber;

                // 受注情報表示
                this.DoGetJuchuSearchData();

                if (page > MaxPageNumber) page--;

                // ページ数を表示
                DoNewPage(page);
            }

        }

        /// <summary>
        /// 対象日が配車済みとなっている車両の一覧を取得します。
        /// </summary>
        /// <param name="day">対象日</param>
        /// <param name="kyujitsuCalendarList">休日検索結果</param>
        /// <returns></returns>
        private Dictionary<decimal,decimal> GEtHaishaCarDictionar(DateTime day, List<HaishaKyujitsuCalendarInfo> kyujitsuCalendarList)
        {
            Dictionary<decimal, decimal> haishaCarDictionar = new Dictionary<decimal, decimal>();

            // 「未配車両のみ」にチェックが入っていない場合は空を設定して終了
            if (!this.chkMihainomiFlag.Checked) return haishaCarDictionar;

            // 対象日の末尾
            DateTime endDey = new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);

            // 休日の場合
            foreach (HaishaKyujitsuCalendarInfo info in kyujitsuCalendarList)
            {
                // 対象日が休日か判定
                if (info.HizukeYMD == day)
                {
                    if (!haishaCarDictionar.ContainsKey(info.CarId)) haishaCarDictionar.Add(info.CarId, info.CarId);
                }
            }

            // 配車の検索結果
            foreach (HaishaSearchResultInfo info in this._DataOfCurrentDay)
            {

                HaishaNyuryokuInfo item = info.HaishaInfo;

                // 更新リストにあるデータは表示しない
                if (_HaishaNyuryokuUpdata.ContainsKey(item.HaishaId)) continue;

                // 削除リストにあるデータは表示しない
                if (_HaishaNyuryokuDel.ContainsKey(item.HaishaId)) continue;

                // 配車済みか判定
                if (item.TaskStartDateTime<= day && endDey < item.ReuseYMD) 
                {
                    if (!haishaCarDictionar.ContainsKey(item.CarId)) haishaCarDictionar.Add(item.CarId, item.CarId);
                }

                // 配車済みか判定(積日と一致しているか)
                if (day.Date == item.TaskStartDateTime.Date)
                {
                    if (!haishaCarDictionar.ContainsKey(item.CarId)) haishaCarDictionar.Add(item.CarId, item.CarId);
                }

            }

            // 新規追加
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in _HaishaNyuryokuAdd)
            {

                HaishaNyuryokuInfo item = info.Value;

                // 配車済みか判定
                if (item.TaskStartDateTime <= day && endDey < item.ReuseYMD)
                {
                    if (!haishaCarDictionar.ContainsKey(item.CarId)) haishaCarDictionar.Add(item.CarId, item.CarId);
                }

                // 配車済みか判定(積日と一致しているか)
                if (day.Date == item.TaskStartDateTime.Date)
                {
                    if (!haishaCarDictionar.ContainsKey(item.CarId)) haishaCarDictionar.Add(item.CarId, item.CarId);
                }
            }

            // 更新中
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in _HaishaNyuryokuUpdata)
            {

                HaishaNyuryokuInfo item = info.Value;

                // 配車済みか判定
                if (item.TaskStartDateTime <= day && endDey < item.ReuseYMD)
                {
                    if (!haishaCarDictionar.ContainsKey(item.CarId)) haishaCarDictionar.Add(item.CarId, item.CarId);
                }

                // 配車済みか判定(積日と一致しているか)
                if (day.Date == item.TaskStartDateTime.Date)
                {
                    if (!haishaCarDictionar.ContainsKey(item.CarId)) haishaCarDictionar.Add(item.CarId, item.CarId);
                }
            }

            return haishaCarDictionar;
        }

        /// <summary>
        /// 請求済、支払済、月次締処理済みのチェックを実施します。
        /// </summary>
        /// <returns></returns>
        private string CheckFix()
        {

            // 追加・更新・削除対象リスト
            List<HaishaNyuryokuInfo> list = new List<HaishaNyuryokuInfo>();

            //メッセージ表示用
            string msg = string.Empty;

            // 更新対象となる配車
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuUpdata)
            {
                list.Add(info.Value);
            }

            // 削除対象となる配車
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in this._HaishaNyuryokuDel)
            {
                list.Add(info.Value);
            }

            // 対象データがない場合は終了
            if (list.Count() == 0) return string.Empty;

            // 検索条件設定
            var para = new HaishaNyuryokuSearchParameter();
            // 配車IDリスト
            para.HaishaIdList = list.Select(x => x.HaishaId).ToList();

            //TODO 性能問題のためコメント化中 START
            //// 請求日、支払日のリストを取得します。
            //var FixDatList = _HaishaNyuryoku.GetFixDat(para);

            //// 請求済みの場合(請求締切日)
            //if (FixDatList.Where(x=>x.MinClmFixDate != DateTime.MinValue).Count() > 0)
            //{
            //    return FrameUtilites.GetDefineMessage("ME2303011");
            //}

            //// 支払済みの場合(傭車支払締切日)
            //if (FixDatList.Where(x => x.MinCharterPayFixDate != DateTime.MinValue).Count() > 0)
            //{
            //    return FrameUtilites.GetDefineMessage("ME2303012");
            //}
            //TODO 性能問題のためコメント化中 END

            //管理情報取得
            ToraDonSystemPropertyInfo toraDonSystemPropertyInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();

            // 最終月次集計月を取得する
            DateTime wk_date = toraDonSystemPropertyInfo.LastSummaryOfMonthDate;
            DateTime last_sum_of_monthday;
            if (wk_date > DateTime.MinValue)
            {
                last_sum_of_monthday = NSKUtil.MonthLastDay(wk_date);
            }
            else
            {
                last_sum_of_monthday = wk_date;
            }

            //TODO 性能問題のためコメント化中 START
            // トラDonの月次締処理済みの場合
            //foreach (HaishaNyuryokuInfo info in FixDatList) 
            //{
            //    // 配車情報の月次締処理済みの場合
            //    if ((this.IsMonthlyTotaled(info.AddUpDate, info.ToraDonFixFlag, last_sum_of_monthday) ||
            //        this.IsMonthlyTotaled(info.CharterAddUpDate, info.ToraDonCharterFixFlag, last_sum_of_monthday)))
            //    {
            //        return FrameUtilites.GetDefineMessage("ME2303013",
            //            new string[] { last_sum_of_monthday.ToString("yyyy/MM/dd") });
            //    }
            //}
            //TODO 性能問題のためコメント化中 END

            // 更新・追加対象となる配車
            foreach (HaishaNyuryokuInfo info in list) 
            {

                // 配車情報の月次締処理済みの場合
                if ((this.IsMonthlyTotaled(info.AddUpYMD, info.FixFlag, last_sum_of_monthday) ||
                    this.IsMonthlyTotaled(info.CharterAddUpYMD, info.CharterFixFlag, last_sum_of_monthday)))
                {
                    return FrameUtilites.GetDefineMessage("ME2303013", 
                        new string[] { last_sum_of_monthday.ToString("yyyy/MM/dd") });
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// パネル切替イベント
        /// </summary>
        /// <param name="e"></param>
        private void btnIPanelKirikae_Click(object sender, EventArgs e)
        {
            this.groupBoxJuchuShosai.Visible = false;
            this.groupBoxHaishaShosai.Visible = false;
            this.groupBoxKihonJoho.Visible = true;
        }

        /// <summary>
        /// 利用者補マスタの情報の取得をします。
        /// </summary>
        private void GetOperatorExInfo()
        {
            List<HaitaOperatorExInfo> list = this._HaishaNyuryoku.GetOperatorEx();
            if (list.Count() > 0)
            {
                this.OperatorExInfo = list.First();
            }
            else
            {
                this.OperatorExInfo = null;
            }
        }

        /// <summary>
        /// 利用者補マスタの更新をします。
        /// </summary>
        /// <returns>実行結果</returns>
        private Task<bool> UpdateOperatorEx()
        {
            return
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            // 利用者補テーブルの更新
                            this._HaishaNyuryoku.UpdateOperatorE(tx, this.GetScreen());
                        });
                    }
                    catch (Exception e)
                    {
                        // 何もしない
                        ;
                    }

                    return true;
                });
        }
    }
}
