using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using GrapeCity.Win.CalendarGrid;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.ComLib;
using GrapeCity.Win.Calendar;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class KyujitsuCalendarFrame : Form,IFrameBase
    {
        #region ユーザー定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "休日カレンダ登録";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// 現在の画面の編集モードを保持する領域
        /// </summary>
        private FrameEditMode currentMode = FrameEditMode.Default;

        /// <summary>
        /// メニューに表示している操作メニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 社員クラス
        /// </summary>
        private Staff _Staff;

        /// <summary>
        /// 休日カレンダクラス
        /// </summary>
        private KyujitsuCalendar _KyujitsuCalendar;

        /// <summary>
        /// 休日カレンダ明細クラス
        /// </summary>
        private KyujitsuCalendarMeisai _KyujitsuCalendarMeisai;

        /// <summary>
        /// 現在編集中の休日カレンダ情報を保持する領域
        /// </summary>
        private KyujitsuCalendarInfo _KyujitsuCalendarInfo = null;

        /// <summary>
        /// 現在編集中のメモリストを保持する領域
        /// </summary>
        private SortedDictionary<DateTime, String> _MemoDic = null;

        /// <summary>
        /// 現在登録中の休日カレンダ明細情報を保持する領域
        /// </summary>
        private SortedDictionary<DateTime, String> _KyujitsuCalendarMeisaiDic = null;

        /// <summary>
        /// 休日区分配色文字色リストを保持する領域
        /// </summary>
        private SortedDictionary<int, Color> _KyujitsuForeColorDic = new SortedDictionary<int, Color>();

        /// <summary>
        /// 休日区分配色背景色リストを保持する領域
        /// </summary>
        private SortedDictionary<int, Color> _KyujitsuBackColorDic = new SortedDictionary<int, Color>();

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList =
            UserProperty.GetInstance().SystemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.KyujitsuKbn)).ToList();

        /// <summary>
        /// 選択年度の自分以外の休日合計リストを保持する領域
        /// </summary>
        private Dictionary<DateTime, Decimal> anotherKyujitsuList = null;

        #region 社員一覧

        //--Spread列定義

        /// <summary>
        /// 社員コード列番号
        /// </summary>
        private const int COL_STAFF_CODE = 0;

        /// <summary>
        /// 社員名列番号
        /// </summary>
        private const int COL_STAFF_NAME = 1;

        /// <summary>
        /// 社員名カナ列番号
        /// </summary>
        private const int COL_STAFF_KANA = 2;

        /// <summary>
        /// 社員リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_STAFFLIST = 3;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialStaffStyleInfoArr;

        /// <summary>
        /// 社員情報のリストを保持する領域
        /// </summary>
        private IList<StaffInfo> _StaffInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx_Result = 0;

        #endregion

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public KyujitsuCalendarFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region メモ一覧

        //--Spread列定義

        /// <summary>
        /// 日付列番号
        /// </summary>
        private const int COL_HIZUKE = 0;

        /// <summary>
        /// メモ列番号
        /// </summary>
        private const int COL_MEMO = 1;

        /// <summary>
        /// メモリスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_MEMOLIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialMemoStyleInfoArr;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 休日カレンダ登録画面を初期化します。
        /// </summary>
        private void InitKyujitsuCalendarFrame()
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

            //社員クラスインスタンス作成
            this._Staff = new Staff(this.appAuth);

            //休日カレンダクラスインスタンス作成
            this._KyujitsuCalendar = new KyujitsuCalendar(this.appAuth);

            //休日カレンダ明細クラスインスタンス作成
            this._KyujitsuCalendarMeisai = new KyujitsuCalendarMeisai(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //社員リストのセット
            this.SetStaffListSheet();

            //現在の画面モードを初期化に変更
            this.ChangeMode(FrameEditMode.Default);

            //フォーカス設定
            this.ActiveControl = this.edtSearch;
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
            //社員スタイル情報初期化
            this.InitStaffStyleInfo();

            //メモリストスタイル情報初期化
            this.InitMemoStyleInfo();

            //配色関連コントロール初期化
            this.InitColorContlor();
        }

        /// <summary>
        /// 配色関連のコントロールの初期化をします。
        /// </summary>
        private void InitColorContlor()
        {
            //休日区分の配色リスト取得
            IList<HaishokuExInfo> haishokuList = this._DalUtil.Haishoku.GetListEx(
                new HaishokuExSearchParameter()
                {
                    TableKey = (int)DefaultProperty.HaishokuTableKbn.SystemName,
                    FunctionKey = (int)DefaultProperty.SystemNameKbn.KyujitsuKbn
                });

            //休日区分件数繰り返す
            foreach (SystemNameInfo info in systemNameList)
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

            switch (systemNameList.Count)
            {
                case (int)DefaultProperty.KyujitsuKbn.One:
                    this.lblKbn1.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.One) + "：";
                    this.lblKbnColor1.Text = string.Empty;
                    this.lblKbnColor1.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    this.lblKbn2.Visible = false;
                    this.lblKbnColor2.Visible = false;
                    this.lblKbnColor2.Visible = false;
                    this.lblKbn3.Visible = false;
                    this.lblKbnColor3.Visible = false;
                    this.lblKbnColor3.Visible = false;
                    this.lblKbn4.Visible = false;
                    this.lblKbnColor4.Visible = false;
                    this.lblKbnColor4.Visible = false;
                    this.lblKbn5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    break;
                case (int)DefaultProperty.KyujitsuKbn.Two:
                    this.lblKbn1.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.One) + "：";
                    this.lblKbnColor1.Text = string.Empty;
                    this.lblKbnColor1.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    this.lblKbn2.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Two) + "：";
                    this.lblKbnColor2.Text = string.Empty;
                    this.lblKbnColor2.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Two)];
                    this.lblKbn3.Visible = false;
                    this.lblKbnColor3.Visible = false;
                    this.lblKbnColor3.Visible = false;
                    this.lblKbn4.Visible = false;
                    this.lblKbnColor4.Visible = false;
                    this.lblKbnColor4.Visible = false;
                    this.lblKbn5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    break;
                case (int)DefaultProperty.KyujitsuKbn.Three:
                    this.lblKbn1.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.One) + "：";
                    this.lblKbnColor1.Text = string.Empty;
                    this.lblKbnColor1.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    this.lblKbn2.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Two) + "：";
                    this.lblKbnColor2.Text = string.Empty;
                    this.lblKbnColor2.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Two)];
                    this.lblKbn3.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Three) + "：";
                    this.lblKbnColor3.Text = string.Empty;
                    this.lblKbnColor3.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Three)];
                    this.lblKbn4.Visible = false;
                    this.lblKbnColor4.Visible = false;
                    this.lblKbnColor4.Visible = false;
                    this.lblKbn5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    break;
                case (int)DefaultProperty.KyujitsuKbn.Four:
                    this.lblKbn1.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.One) + "：";
                    this.lblKbnColor1.Text = string.Empty;
                    this.lblKbnColor1.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    this.lblKbn2.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Two) + "：";
                    this.lblKbnColor2.Text = string.Empty;
                    this.lblKbnColor2.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Two)];
                    this.lblKbn3.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Three) + "：";
                    this.lblKbnColor3.Text = string.Empty;
                    this.lblKbnColor3.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Three)];
                    this.lblKbn4.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Four) + "：";
                    this.lblKbnColor4.Text = string.Empty;
                    this.lblKbnColor4.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Four)];
                    this.lblKbn5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    this.lblKbnColor5.Visible = false;
                    break;
                case (int)DefaultProperty.KyujitsuKbn.Five:
                    this.lblKbn1.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.One) + "：";
                    this.lblKbnColor1.Text = string.Empty;
                    this.lblKbnColor1.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    this.lblKbn2.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Two) + "：";
                    this.lblKbnColor2.Text = string.Empty;
                    this.lblKbnColor2.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Two)];
                    this.lblKbn3.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Three) + "：";
                    this.lblKbnColor3.Text = string.Empty;
                    this.lblKbnColor3.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Three)];
                    this.lblKbn4.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Four) + "：";
                    this.lblKbnColor4.Text = string.Empty;
                    this.lblKbnColor4.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Four)];
                    this.lblKbn5.Text = this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.Five) + "：";
                    this.lblKbnColor5.Text = string.Empty;
                    this.lblKbnColor5.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.Five)];
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 社員のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStaffStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpStaffListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialStaffStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_STAFFLIST];

            for (int i = 0; i < COL_MAXCOLNUM_STAFFLIST; i++)
            {
                this.initialStaffStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// メモリストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitMemoStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpMemoListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialMemoStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_MEMOLIST];

            for (int i = 0; i < COL_MAXCOLNUM_MEMOLIST; i++)
            {
                this.initialMemoStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.edtSearch.Text = string.Empty;
            this.dteSearchYear.Value = DateTime.Today;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.anotherKyujitsuList = new Dictionary<DateTime, Decimal>();
            this.calKyujitsuCalendar.Holidays.Clear();

            this.dteYear.Value = new DateTime(DateTime.Today.Year, 1, 1);
            this.numStaffCode.Value = null;
            this.edtStaffName.Text = string.Empty;

            this.edtMemo.Text = string.Empty;
            this.numKyujitsuGokei.Value = null;
            this.numNenkanNissu.Value = null;

            this.InitSheet();

            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// メニュー関連を初期化します。
        /// </summary>
        private void InitMenuItem()
        {
            //操作メニュー
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
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Delete);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 社員リストを初期化します。
        /// </summary>
        private void InitStaffListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpStaffListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_STAFFLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// メモリストを初期化します。
        /// </summary>
        private void InitMemoListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpMemoListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_MEMOLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// Spread関連、Calender関連の初期化をします。
        /// </summary>
        private void InitSearchSheet()
        {
            //社員リストの初期化
            this.InitStaffListSheet();
        }

        /// <summary>
        /// Spread関連、Calender関連の初期化をします。
        /// </summary>
        private void InitSheet()
        {
            //カレンダー初期化
            DateTime date = Convert.ToDateTime(dteSearchYear.Value);
            this.calKyujitsuCalendar.FirstDateInView = Convert.ToDateTime(dteSearchYear.Value);
            this.calKyujitsuCalendar.MaxDate = new DateTime(date.Year, 12, 31);
            this.calKyujitsuCalendar.MinDate = new DateTime(date.Year, 1, 1);
            this.calKyujitsuCalendar.CurrentDate = new DateTime(date.Year, 1, 1);

            int days = DateTimeHelper.GetPeriod(
                this.calKyujitsuCalendar.MinDate, this.calKyujitsuCalendar.MaxDate, false);

            for (int i = 0; i < days; i++)
            {
                //セル取得
                var cell = this.calKyujitsuCalendar.Content[this.calKyujitsuCalendar.MinDate.AddDays(i)];
                if (cell != null)
                {
                    //セル初期化
                    cell.Tag = null;
                    cell.CellStyle.BackColor = Color.Empty;
                    cell.CellStyle.ForeColor = Color.Empty;
                }
            }

            //メモリストの初期化
            this.InitMemoListSheet();
        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***編集取消
            _commandSet.EditCancel.Execute += EditCancel_Execute;
            _commandSet.Bind(
                _commandSet.EditCancel, this.btnCancel, actionMenuItems.GetMenuItemBy(ActionMenuItems.EditCancel), this.toolStripCancel);

            //***保存
            _commandSet.Save.Execute += Save_Execute;
            _commandSet.Bind(
                _commandSet.Save, this.btnSave, actionMenuItems.GetMenuItemBy(ActionMenuItems.Update), this.toolStripSave);

            //***削除
            _commandSet.Delete.Execute += Delete_Execute;
            _commandSet.Bind(
                _commandSet.Delete, this.btnDelete, actionMenuItems.GetMenuItemBy(ActionMenuItems.Delete), this.toolStripRemove);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            this.DoClear(true);
        }

        void Save_Execute(object sender, EventArgs e)
        {
            this.DoUpdate();
        }

        void Delete_Execute(object sender, EventArgs e)
        {
            this.DoDelData();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        #endregion

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtMemo, ctl => ctl.Text, this.edtMemo_Validating));
        }

        #region IFrameBase メンバ

        public void InitFrame()
        {
            this.InitKyujitsuCalendarFrame();
        }

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
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = true;
                    this.pnlTop.Enabled = false;
                    this.pnlMain.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = false;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.New:
                    //新規作成モード
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = false;
                    this.pnlTop.Enabled = true;
                    this.pnlMain.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    this.pnlLeft.Enabled = false;
                    this.pnlTop.Enabled = true;
                    this.pnlMain.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = true;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.ViewOnly:
                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// 社員リストに値を設定します。
        /// </summary>
        private void SetStaffListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSearchSheet();

                //検索結果一覧の取得を指示
                this._StaffInfoList =
                    this._Staff.GetList();

                IList<StaffInfo> wk_list = this.GetMatchedList(this._StaffInfoList);

                //件数取得
                int rowCount = wk_list.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputs();

                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_STAFFLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_STAFFLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    StaffInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_STAFFLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStaffStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.ToraDONDisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_STAFF_CODE, wk_info.ToraDONStaffCode);
                    datamodel.SetValue(j, COL_STAFF_NAME, wk_info.ToraDONStaffName);
                    datamodel.SetValue(j, COL_STAFF_KANA, wk_info.ToraDONStaffNameKana);

                    datamodel.SetTag(j, COL_STAFF_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpStaffListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpStaffListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpStaffListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpStaffListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpStaffListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpStaffListGrid.Sheets[0].ColumnCount - 1);

                    this.DoGetData(false);
                }
                else
                {
                    //入力項目をクリア
                    this.ClearInputs();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 検索テーブル情報の一覧を指定して、抽出条件に合致する検索テーブル情報の一覧を取得します。
        /// </summary>
        /// <param name="list">検索テーブル情報の一覧</param>
        private List<StaffInfo> GetMatchedList(IList<StaffInfo> list)
        {
            List<StaffInfo> rt_list = new List<StaffInfo>();

            //検索条件「名称」
            string joken_Mei = this.edtSearch.Text.Trim().ToLower();

            //全角に変換
            joken_Mei =
                Microsoft.VisualBasic.Strings.StrConv(
                    joken_Mei, Microsoft.VisualBasic.VbStrConv.Wide, 0x411);

            //カナの一致のタイプをチェックボックスから列挙値に変換する
            FrameLib.SpecialStringContaintType kana_type;

            //包含一致
            kana_type = SpecialStringContaintType.Contains;

            foreach (StaffInfo item in list)
            {
                //「コード」+「名称」+「カナ」であいまい検索
                string item_mei =
                    Convert.ToInt32(item.ToraDONStaffCode) + Environment.NewLine
                    + item.ToraDONStaffName.Trim() + Environment.NewLine
                    + item.ToraDONStaffNameKana.Trim() + Environment.NewLine;

                //半角を全角に変換する
                string zenkaku =
                    Microsoft.VisualBasic.Strings.StrConv(
                        item_mei, Microsoft.VisualBasic.VbStrConv.Wide, 0x411);

                //検索条件に合致しているかどうかの値(true:一致)
                bool match = true;

                //名称のチェック
                if (match)
                {
                    if (joken_Mei.Length > 0 &&
                        !FrameUtilites.SpecialContaintString(zenkaku.ToLower(), joken_Mei, kana_type))
                    {
                        match = false;
                    }
                }

                //全ての抽出条件に合致していればリストに追加
                if (match)
                {
                    rt_list.Add(item);
                }
            }

            return rt_list;
        }

        /// <summary>
        /// メモリストに値を設定します。
        /// </summary>
        private void SetMemoListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //メモリスト初期化
                this.InitMemoListSheet();

                var wk_list = this._MemoDic.Where(x => !x.Value.Equals(string.Empty));

                //件数取得
                if (wk_list == null
                    || wk_list.Count() == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count(), COL_MAXCOLNUM_MEMOLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count(), COL_MAXCOLNUM_MEMOLIST);

                int i = 0;

                foreach (KeyValuePair<DateTime, string> item in wk_list)
                {
                    if (!item.Value.Equals(string.Empty))
                    {
                        //スタイルモデルのセット
                        for (int k = 0; k < COL_MAXCOLNUM_MEMOLIST; k++)
                        {
                            StyleInfo sInfo = new StyleInfo(this.initialMemoStyleInfoArr[k]);
                            stylemodel.SetDirectInfo(i, k, sInfo);
                        }

                        datamodel.SetValue(i, COL_HIZUKE, item.Key.ToString("yyyy/MM/dd"));
                        datamodel.SetValue(i, COL_MEMO, item.Value);
                        i++;
                    }
                }

                //Spreadにデータモデルをセット
                this.fpMemoListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpMemoListGrid.Sheets[0].Models.Style = stylemodel;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 登録済リスト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpStaffListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetData();
                    break;
                default:
                    break;
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
        /// キーイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessKeyEvent(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    DateTime currentDate = Convert.ToDateTime(this.calKyujitsuCalendar.CurrentDate).Date;
                    this.ChangeKyujitsuKbn(currentDate);
                    break;
                case Keys.F1:
                    this.DoClose();
                    break;
                case Keys.F6:
                    this.DoClear(true);
                    break;
                case Keys.F8:
                    this.DoUpdate();
                    break;
                case Keys.F9:
                    this.DoDelData();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //選択社員情報をリストから取得して設定
            StaffInfo info = this.GetStaffInfoByStaffListOnSelection();

            if (info.ToraDONStaffId == 0)
            {
                return;
            }

            int year = 0;

            if (Convert.ToDateTime(this.dteSearchYear.Value) == DateTime.MinValue)
            {
                year = DateTime.Today.Year;
            }
            else
            {
                year = this.dteSearchYear.Value.Value.Year;
            }

            try
            {
                //カレンダー情報取得
                this._KyujitsuCalendarInfo =
                    this._KyujitsuCalendar.GetInfoByNendoAndStaffId(year, info.ToraDONStaffId);

                //画面設定
                this.SetScreen(info, year);

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditMode.Editable);

                    //フォーカス設定
                    this.calKyujitsuCalendar.Focus();
                }
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
        }

        /// <summary>
        /// 社員リストにて選択中の社員情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の社員情報</returns>
        private StaffInfo GetStaffInfoByStaffListOnSelection()
        {
            //返却用
            StaffInfo rt_info = new StaffInfo();

            SheetView sheet0 = this.fpStaffListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagから社員情報を取得
                    rt_info = (StaffInfo)sheet0.Cells[select_row, COL_STAFF_CODE].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// 詳細上部の入力値をチェックします。
        /// </summary>
        /// <returns>チェック結果（true：正常）</returns>
        private bool CheckInputs()
        {
            bool rt_val = true;

            string msg = string.Empty;
            Control ctl = null;

            if (rt_val && Convert.ToDateTime(this.dteYear.Value) == DateTime.MinValue)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "対象年" });
                ctl = this.dteYear;
            }

            if (rt_val && Convert.ToDecimal(this.numStaffCode.Tag) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "社員" });
                ctl = this.numStaffCode;
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// 選択年度の自分以外の休日合計リストを取得します。
        /// </summary>
        private void GetAnotherKyujitsuList(int nendo, decimal id)
        {
            this.anotherKyujitsuList = new Dictionary<DateTime, Decimal>();

            IList<KyujitsuCalendarMeisaiInfo> wk_list =
                    this._KyujitsuCalendarMeisai.GetList(
                        new KyujitsuCalendarMeisaiSearchParameter()
                        {
                            Nendo = nendo,
                            ExcludeToraDONStaffId = id
                        });

            foreach (KyujitsuCalendarMeisaiInfo info in wk_list)
            {
                if (this.anotherKyujitsuList.ContainsKey(info.HizukeYMD))
                {
                    decimal val = this.anotherKyujitsuList[info.HizukeYMD];

                    this.anotherKyujitsuList[info.HizukeYMD] = val += info.KyujitsuNissu;
                }
                else
                {
                    this.anotherKyujitsuList.Add(info.HizukeYMD, info.KyujitsuNissu);
                }
            }
        }

        /// <summary>
        /// 画面に画面コントローラの値をセットします。
        /// </summary>
        private void SetScreen(StaffInfo info, int year)
        {
            //編集画面初期化
            this.ClearInputs();

            if (year == 0)
            {
                this.dteYear.Value = new DateTime(DateTime.Today.Year, 1, 1);
            }
            else
            {
                this.dteYear.Value = new DateTime(year, 1, 1);
            }

            if (this._KyujitsuCalendarInfo != null
                && 0 < this._KyujitsuCalendarInfo.ToraDONStaffId)
            {
                this.numStaffCode.Tag = this._KyujitsuCalendarInfo.ToraDONStaffId;
                this.numStaffCode.Value = this._KyujitsuCalendarInfo.ToraDONStaffCode;
                this.edtStaffName.Text = this._KyujitsuCalendarInfo.ToraDONStaffName;
            }
            else
            {
                if (0 < info.ToraDONStaffId)
                {
                    this.numStaffCode.Tag = info.ToraDONStaffId;
                    this.numStaffCode.Value = info.ToraDONStaffCode;
                    this.edtStaffName.Text = info.ToraDONStaffName;
                }
            }

            this.calKyujitsuCalendar.SuspendLayout();

            try
            {
                //処理年取得
                DateTime yearDate = Convert.ToDateTime(this.dteYear.Value);

                //休日合計リストの取得
                this.GetAnotherKyujitsuList(yearDate.Year, Convert.ToDecimal(this.numStaffCode.Tag));

                //休日カレンダ初期化
                this.calKyujitsuCalendar.FirstDateInView = new DateTime(yearDate.Year, 1, 1);
                this.calKyujitsuCalendar.MaxDate = new DateTime(yearDate.Year, 12, 31);
                this.calKyujitsuCalendar.MinDate = new DateTime(yearDate.Year, 1, 1);

                //メモリスト初期化
                this._MemoDic = new SortedDictionary<DateTime, String>();
                this._KyujitsuCalendarMeisaiDic = new SortedDictionary<DateTime, String>();

                //年間休日日数
                decimal nenkanNissu = 0;

                //休日カレンダ情報が存在する場合
                if (this._KyujitsuCalendarInfo != null
                    && this._KyujitsuCalendarInfo.KyujitsuCalendarMeisaiList != null
                    && 0 < this._KyujitsuCalendarInfo.KyujitsuCalendarMeisaiList.Count())
                {
                    foreach (var item in this._KyujitsuCalendarInfo.KyujitsuCalendarMeisaiList)
                    {
                        this.calKyujitsuCalendar.Content[item.HizukeYMD].Tag = item.KyujitsuKbn;

                        //休日の場合
                        if (0 < item.KyujitsuKbn)
                        {
                            this.SetKyujitsuKbn(item.HizukeYMD, item.KyujitsuKbn);
                            nenkanNissu += item.KyujitsuNissu;
                        }

                        //メモが存在する場合
                        if (!item.Memo.Trim().Equals(string.Empty))
                        {
                            this._MemoDic[item.HizukeYMD] = item.Memo.Trim();
                            this._KyujitsuCalendarMeisaiDic[item.HizukeYMD] = item.Memo.Trim();
                        }
                    }

                    //初日のメモを設定
                    this.edtMemo.Text = this.GetMemo(this.calKyujitsuCalendar.MinDate);
                }
                else
                {
                    this._KyujitsuCalendarInfo = new KyujitsuCalendarInfo();
                }

                //メモリストが存在する場合
                if (0 < this._MemoDic.Count)
                {
                    //メモリスト設定
                    this.SetMemoListSheet();
                }

                //年間休日日数を設定
                this.numNenkanNissu.Value = nenkanNissu;

                //初日の休日合計を設定
                this.numKyujitsuGokei.Value = this.GetKyujitsuGokei(this.calKyujitsuCalendar.MinDate);

                this.calKyujitsuCalendar.CurrentCellPosition =
                    new CalendarCellPosition(this.calKyujitsuCalendar.FirstDateInView, 0, 0);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                this.calKyujitsuCalendar.ResumeLayout();
            }
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

            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
            {
                //待機状態へ
                this.Cursor = Cursors.WaitCursor;

                //画面から値を取得
                this.GetScreen();

                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._KyujitsuCalendar.Save(tx, this._KyujitsuCalendarInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "休日カレンダ", this._KyujitsuCalendarInfo.Nendo.ToString() });

                    //保存完了メッセージ格納用
                    string msg = string.Empty;

                    //操作ログ出力
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.UpdateItem,
                            log_jyoken);

                    //更新完了のメッセージ
                    msg =
                        FrameUtilites.GetDefineMessage("MI2001003");

                    //登録完了メッセージ
                    MessageBox.Show(
                        msg,
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //初期状態へ移行
                    this.DoClear(false);
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
                catch (Model.DALExceptions.UniqueConstraintException ex)
                {
                    //同一コードが存在する場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            //年度
            this._KyujitsuCalendarInfo.Nendo = this.dteYear.Value.Value.Year;

            //トラDON社員
            this._KyujitsuCalendarInfo.ToraDONStaffId = Convert.ToDecimal(this.numStaffCode.Tag);

            //休日カレンダ明細情報リスト
            IList<KyujitsuCalendarMeisaiInfo> wk_list = new List<KyujitsuCalendarMeisaiInfo>();

            int days = DateTimeHelper.GetPeriod(
                this.calKyujitsuCalendar.MinDate, this.calKyujitsuCalendar.MaxDate, false);

            for (int i = 0; i < days; i++)
            {
                //対象日付取得
                DateTime targetYmd = this.calKyujitsuCalendar.MinDate.AddDays(i);

                int KyujitsuKbn = 0;

                //休日の場合
                if (this.calKyujitsuCalendar.Holidays.Contains(targetYmd))
                {
                    //休日区分取得
                    KyujitsuKbn = Convert.ToInt32(this.calKyujitsuCalendar.Content[targetYmd].Tag);
                }
                
                //メモ取得
                string memo = this.GetMemo(targetYmd);

                //旧メモ存在フラグ
                bool isMemoBK = this.GetMemo_BK(targetYmd);

                if (0 < KyujitsuKbn || 0 < memo.Length || isMemoBK)
                {
                    wk_list.Add(
                        new KyujitsuCalendarMeisaiInfo()
                        {
                            KyujitsuCalendarId = this._KyujitsuCalendarInfo.KyujitsuCalendarId,
                            HizukeYMD = targetYmd,
                            KyujitsuKbn = KyujitsuKbn,
                            Memo = memo
                        });
                }
            }

            //休日カレンダ明細取得
            this._KyujitsuCalendarInfo.KyujitsuCalendarMeisaiList = wk_list;
        }

        /// <summary>
        /// 画面情報からデータを削除します。
        /// </summary>
        private void DoDelData()
        {
            //削除確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2101004"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            //Yesだったら
            if (d_result == DialogResult.Yes)
            {
                //削除確認画面を表示
                using (DeleteMessage f = new DeleteMessage())
                {
                    f.InitFrame();

                    DialogResult wk_d_result = f.ShowDialog(this);

                    //削除OK
                    if (wk_d_result == DialogResult.OK)
                    {
                        try
                        {
                            //削除
                            SQLHelper.ActionWithTransaction(tx =>
                            {
                                this._KyujitsuCalendar.Delete(tx, this._KyujitsuCalendarInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "休日カレンダ", this._KyujitsuCalendarInfo.Nendo.ToString() });

                            //操作ログ出力（削除）
                            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                                FrameLogWriter.LoggingOperateKind.DelItem,
                                log_jyoken);

                            //削除完了メッセージ
                            MessageBox.Show(
                                FrameUtilites.GetDefineMessage("MI2001004"),
                                this.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            //初期状態へ移行
                            this.DoClear(false);
                        }
                        catch (CanRetryException ex)
                        {
                            //他から更新されている場合の例外ハンドラ
                            FrameUtilites.ShowExceptionMessage(ex, this);
                        }
                    }
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

            //Spread関連をクリア
            this.InitSearchSheet();

            //社員リストをセット
            this.SetStaffListSheet();

            //休日設定の初期化
            this.calKyujitsuCalendar.Holidays.Clear();

            //メモリストの初期化
            this.InitMemoListSheet();

            //画面モード切換え
            this.ChangeMode(FrameEditMode.Default);

            this.dteYear.Focus();
        }

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.currentMode == FrameEditMode.Editable)
            {
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
        }

        /// <summary>
        /// 対象日付のメモを取得します。
        /// </summary>
        /// <returns>対象日付のメモ</returns>
        private string GetMemo(DateTime ymd)
        {
            string rt_val = string.Empty;

            try
            {
                rt_val = StringHelper.ConvertToString(this._MemoDic[ymd]).Trim();
            }
            catch
            {
                ;
            }

            return rt_val;
        }

        /// <summary>
        /// 対象日付の旧メモを取得します。
        /// </summary>
        /// <returns>対象日付の旧メモ</returns>
        private bool GetMemo_BK(DateTime ymd)
        {
            bool rt_val = false;

            try
            {
                object wk_dic = this._KyujitsuCalendarMeisaiDic[ymd];

                if (wk_dic != null)
                {
                    rt_val = true;
                }
            }
            catch
            {
                ;
            }

            return rt_val;
        }

        /// <summary>
        /// 対象日付のメモのラベルを取得します。
        /// </summary>
        /// <returns>対象日付のメモのラベル</returns>
        private string GetlblMemo(DateTime ymd)
        {
            string rt_val = string.Empty;

            try
            {
                rt_val = ymd.ToString("M月d日") + "のメモ";
            }
            catch
            {
                ;
            }

            return rt_val;
        }

        /// <summary>
        /// 対象日付のメモのラベルを取得します。
        /// </summary>
        /// <returns>対象日付のメモのラベル</returns>
        private string GetlblKyujitsuGokei(DateTime ymd)
        {
            string rt_val = string.Empty;

            try
            {
                rt_val = ymd.ToString("M月d日") + "の休日合計";
            }
            catch
            {
                ;
            }

            return rt_val;
        }

        /// <summary>
        /// 対象日付の休日合計を取得します。
        /// </summary>
        /// <returns>対象日付の休日合計</returns>
        private decimal GetKyujitsuGokei(DateTime ymd)
        {
            decimal rt_val = 0;

            if (this.anotherKyujitsuList.ContainsKey(ymd))
            {
                rt_val = this.anotherKyujitsuList[ymd] + this.GetKyujitsuNissu(ymd);
            }
            else
            {
                rt_val = this.GetKyujitsuNissu(ymd);
            }

            return rt_val;
        }

        /// <summary>
        /// 対象日付の休日日数を取得します。
        /// </summary>
        /// <returns>対象日付の休日日数</returns>
        private decimal GetKyujitsuNissu(DateTime ymd)
        {
            int wk_kbn = Convert.ToInt32(this.calKyujitsuCalendar.Content[ymd].Tag);

            if (wk_kbn == 0)
            {
                return 0;
            }

            var wk_list = this.systemNameList.Where(x => x.SystemNameCode == wk_kbn);

            if (wk_list == null || wk_list.Count() == 0)
            {
                return 0;
            }

            return ((SystemNameInfo)wk_list.ToList()[0]).DecimalValue01;
        }

        /// <summary>
        /// 対象日付の休日区分を変更します。
        /// </summary>
        private void ChangeKyujitsuKbn(DateTime ymd)
        {
            if (currentMode != FrameEditMode.Editable)
            {
                return;
            }

            int kbn = Convert.ToInt32(this.calKyujitsuCalendar.Content[ymd].Tag);

            switch (kbn)
            {
                case 0:
                case ((int)DefaultProperty.KyujitsuKbn.One):
                case ((int)DefaultProperty.KyujitsuKbn.Two):
                case ((int)DefaultProperty.KyujitsuKbn.Three):
                case ((int)DefaultProperty.KyujitsuKbn.Four):
                case ((int)DefaultProperty.KyujitsuKbn.Five):
                    if (kbn == systemNameList.Count)
                    {
                        this.InitCalendarGridCell(ymd);
                    }
                    else
                    {
                        this.SetCalendarGridCell(ymd, kbn + 1);
                    }
                    break;
                default:
                    var cell2 = this.calKyujitsuCalendar.Content[ymd];
                    cell2.Tag = (int)DefaultProperty.KyujitsuKbn.One;
                    cell2.CellStyle.BackColor = this._KyujitsuBackColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    cell2.CellStyle.ForeColor = this._KyujitsuForeColorDic[((int)DefaultProperty.KyujitsuKbn.One)];
                    //休日以外の場合は追加
                    if (!this.calKyujitsuCalendar.Holidays.Contains(ymd))
                    {
                        this.calKyujitsuCalendar.Holidays.Add(ymd, this.GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn.One));
                    }
                    break;
            }

            //休日合計取得
            this.numKyujitsuGokei.Value = this.GetKyujitsuGokei(ymd);
        }

        /// <summary>
        /// 対象日付の休日区分を設定します。
        /// </summary>
        private void SetKyujitsuKbn(DateTime ymd, int kbn)
        {
            switch (kbn)
            {
                case ((int)DefaultProperty.KyujitsuKbn.One):
                case ((int)DefaultProperty.KyujitsuKbn.Two):
                case ((int)DefaultProperty.KyujitsuKbn.Three):
                case ((int)DefaultProperty.KyujitsuKbn.Four):
                case ((int)DefaultProperty.KyujitsuKbn.Five):
                    if (systemNameList.Count < kbn)
                    {
                        this.InitCalendarGridCell(ymd);
                    }
                    else
                    {
                        this.SetCalendarGridCell(ymd, kbn);
                    }
                    break;
                default:
                    this.InitCalendarGridCell(ymd);
                    break;
            }
        }

        /// <summary>
        /// 対象日付の休日区分を初期化します。
        /// </summary>
        private void InitCalendarGridCell(DateTime ymd)
        {
            var cell = this.calKyujitsuCalendar.Content[ymd];

            //初期化前休日日数取得
            decimal kyujituNissu = this.GetKyujitsuNissu(ymd);

            //休日区分初期化
            cell.CellStyle.BackColor = Color.Empty;
            cell.CellStyle.ForeColor = Color.Empty;
            cell.Tag = null;
            //すでに休日設定になっている場合は削除
            if (this.calKyujitsuCalendar.Holidays.Contains(ymd))
            {
                this.calKyujitsuCalendar.Holidays.Remove(
                    (this.calKyujitsuCalendar.Holidays.GetHolidays(ymd))[0]);
            }
            //年間休日日数減算
            this.numNenkanNissu.Value = Convert.ToDecimal(this.numNenkanNissu.Value) - kyujituNissu;
        }

        /// <summary>
        /// 対象日付の休日区分をセットします。
        /// </summary>
        private void SetCalendarGridCell(DateTime ymd, int kbn)
        {
            decimal beforeNissu = this.GetKyujitsuNissu(ymd);
            var cell = this.calKyujitsuCalendar.Content[ymd];
            cell.Tag = kbn;
            cell.CellStyle.BackColor = this._KyujitsuBackColorDic[(kbn)];
            cell.CellStyle.ForeColor = this._KyujitsuForeColorDic[(kbn)];
            decimal afterNissu = this.GetKyujitsuNissu(ymd);
            //休日以外の場合は追加
            if (!this.calKyujitsuCalendar.Holidays.Contains(ymd))
            {
                this.calKyujitsuCalendar.Holidays.Add(ymd, this.GetKyujitsuKbnMeisho((DefaultProperty.KyujitsuKbn)kbn));
            }
            //年間休日日数加算
            this.numNenkanNissu.Value = Convert.ToDecimal(this.numNenkanNissu.Value) + (afterNissu - beforeNissu);
        }

        /// <summary>
        /// 休日区分の名称を取得します。
        /// </summary>
        /// <param name="kbn">休日区分</param>
        /// <returns>休日区分の名称</returns>
        public string GetKyujitsuKbnMeisho(DefaultProperty.KyujitsuKbn kbn)
        {
            SystemNameInfo info = systemNameList.Where(x => x.SystemNameCode == (int)kbn).ToList()[0];

            return info.SystemName;
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// メモの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateMemo(CancelEventArgs e)
        {
            if (this.calKyujitsuCalendar.CurrentCellPosition == null)
            {
                //何もしない
                return;
            }
            //メモリストに設定
            this._MemoDic[this.calKyujitsuCalendar.CurrentCellPosition.Date] = this.edtMemo.Text.Trim();

            //メモリスト再描画
            this.SetMemoListSheet();
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

        private void edtMemo_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateMemo(e);
        }

        private void calKyujitsuCalendar_CellDoubleClick(object sender, CalendarCellEventArgs e)
        {
            this.ChangeKyujitsuKbn(e.CellPosition.Date);
        }

        private void calKyujitsuCalendar_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void calKyujitsuCalendar_CellEnter(object sender, CalendarCellMoveEventArgs e)
        {
            if (this._MemoDic == null)
            {
                return;
            }

            this.edtMemo.Text = this.GetMemo(e.CellPosition.Date);
            this.lblMemo.Text = this.GetlblMemo(e.CellPosition.Date);
            this.lblKyujitsuGokei.Text = this.GetlblKyujitsuGokei(e.CellPosition.Date);
            this.numKyujitsuGokei.Value = this.GetKyujitsuGokei(e.CellPosition.Date);
        }

        private void fpStaffListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpStaffListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpStaffListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //社員リスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetData();
            }
        }

        private void fpStaffListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpStaffListGridPreviewKeyDown(e);
        }

        private void fpStaffListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetStaffListSheet();
        }

        private void dteSearchYear_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(this.dteSearchYear.Value) == DateTime.MinValue)
            {
                this.dteSearchYear.Value = DateTime.Today;
            }
        }

        private void KyujitsuCalendarFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }
    }
}
