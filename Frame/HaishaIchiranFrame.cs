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
using System.Configuration;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishaIchiranFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaIchiranFrame()
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

        #region 配車一覧定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配車一覧";

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
        private HaishaIchiran _HaishaIchiran;

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
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        /// <summary>
        /// Category（配車情報）を保持する領域
        /// </summary>
        private Dictionary<decimal, Category> CarDictionary;

        /// <summary>
        /// 追加中FLGを保持する領域
        /// </summary>
        private bool AddingFlg;

        /// <summary>
        /// 初期化FLGを保持する領域
        /// </summary>
        private bool InitFlg;

        /// <summary>
        /// マウスホイール動作中FLGを保持する領域
        /// </summary>
        private bool WheelScrollFlg;

        /// <summary>
        /// 表示日数
        /// </summary>
        private const int DEFAULT_DISP_DAYS = 14;

        /// <summary>
        /// 自動更新（自動検索）起動間隔デフォルト値(5分)
        /// </summary>
        private const int DEFAULT_AUTO_SEARCH_TIME = 300000;

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

        private C1.C1Schedule.Label HaisoLabel;       // 通常配送の表示に使うラベル

        private Status StatusOKa; // 往荷ステータス
        private Status StatusFukuKa; // 復荷ステータス
        private Status StatusNon; // 該当なしステータス

        private Timer timer; // 自動更新用タイマー

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitHaishaIchiranFrame()
        {
            // 初期化フラグON
            this.InitFlg = true;

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
            this.InitMenuHaishaIchiran();

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

            // 設定ステータス（往路・復路）の設定
            this.StatusOKa = new Status(Color.Transparent, "往路", "", new C1Brush(Color.Blue));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusOKa);
            this.StatusFukuKa = new Status(Color.Transparent, "往路", "", new C1Brush(Color.Red));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusFukuKa);
            this.StatusNon = new Status(Color.Transparent, "該当無", "", new C1Brush(Color.White));
            c1ScheduleHaisha.DataStorage.StatusStorage.Statuses.Add(this.StatusNon);

            HaisoLabel = new C1.C1Schedule.Label(Color.Cornsilk, "配送", "配送キャプション");
            
            // 汎用データアクセスクラスインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // 配車一覧クラスインスタンス作成
            this._HaishaIchiran = new HaishaIchiran(this.appAuth);

            // 管理情報取得
            this._KanriInfo = this._DalUtil.Kanri.GetInfo();

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numCarCd, this.ShowCmnSearchCar},
                {this.numCarKindCd, this.ShowCmnSearchCarKind},
                {this.numStaffCd, this.ShowCmnSearchStaff},
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

            // 画面表示設定
            this.InitScreen();

            // タイマーの初期化
            this.InitTimer();

            // 初期化フラグOFF
            this.InitFlg = false;
        }

        /// <summary>
        /// 自動更新（再検索）用のタイマー設定をします。
        /// </summary>
        private void InitTimer()
        {
            KanriInfo kanriInfo = this._DalUtil.Kanri.GetInfo();

            // タイマーの間隔(ミリ秒)
            timer = new Timer();
            timer.Tick += new EventHandler(AutoSearch);
            timer.Interval = kanriInfo.AutoSearchSeconds * 1000;
            timer.Enabled = false;
        }

        /// <summary>
        /// 自動配車情報検索イベント
        /// </summary>
        /// <param name="e"></param>
        private void AutoSearch(object sender, EventArgs e) 
        {
            // 配車情報検索
            this.DoGetHaishaSearchData();
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
        private void InitMenuHaishaIchiran()
        {
            // 操作メニュー
            this.InitActionMenuHaishaIchiran();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuHaishaIchiran()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {

            // 配車情報表示切替
            this.dteTargetDate.Value = DateTime.Now;
            this.cmbTimeInterval.SelectedValue = (int)DefaultProperty.HaishaJohoJikanKankakuKbn.OneDay;
            this.cmbDateRange.SelectedValue = (int)DateRange.FourteenDays;
            this.chkMihainomiFlag.Checked = this._KanriInfo.HaishaNyuryokuMihainomiFlag;

            // 配車情報検索条件
            this.numCarCd.Text = string.Empty;
            this.numCarKindCd.Text = string.Empty;
            this.edtLicPlateCarNo.Text = string.Empty;
            this.edtCarKindNM.Text = string.Empty;
            this.numStaffCd.Text = string.Empty;
            this.edtStaffNM.Text = string.Empty;
            this.chkAutoSearchFlag.Checked = false;
            this.chkKyujitshHyojiFlag.Checked = true;
            this.chkAllFlag.Checked = false;
            this.cmbHaishaBranchOffice.SelectedIndex = 0;

            //メンバをクリア
            this.AddingFlg = false;
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

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            // コード検索
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarCd, ctl => ctl.Text, this.numCarCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCd, ctl => ctl.Text, this.numCarKindCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numStaffCd, ctl => ctl.Text, this.numStaffCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteTargetDate, ctl => ctl.Text, this.dteTargetDate_Validating));

            // 時間間隔
            cmbTimeInterval.SelectedIndexChanged += CmbTimeInterval_SelectedIndexChanged;

            // 表示範囲
            cmbDateRange.SelectedIndexChanged += CmbDateRange_SelectedIndexChanged;

            // 車両区分
            cmbCarKbn.SelectedIndexChanged += cmbCarKbn_SelectedIndexChanged;

            //ホイールイベントの追加  
            c1ScheduleHaisha.MouseWheel += c1ScheduleHaisha_MouseWheel;

            // 配車用営業所
            cmbHaishaBranchOffice.SelectedIndexChanged += cmbHaishaBranchOffice_SelectedIndexChanged;

            // 未配のみ
            chkMihainomiFlag.CheckedChanged += chkMihainomiFlag_CheckedChanged;

        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        void dteTargetDate_Down(object sender, EventArgs e)
        {
            this.dteTargetDate_Spin(-1);
        }

        void dteTargetDate_Up(object sender, EventArgs e)
        {
            this.dteTargetDate_Spin(1);
        }

        private void c1ScheduleHaisha_BeforeAppointmentDrop(object sender, CancelAppointmentEventArgs e)
        {
            // 追加中の場合は終了
            if (this.AddingFlg) return;

            // 操作をキャンセル
            e.Cancel = true;
        }

        private void c1ScheduleHaisha_BeforeAppointmentEdit(object sender, CancelAppointmentEventArgs e)
        {
            // 追加中の場合は終了
            if (this.AddingFlg) return;

            // 操作をキャンセル
            e.Cancel = true;
        }

        private void c1ScheduleHaisha_BeforeAppointmentResize(object sender, CancelAppointmentEventArgs e)
        {

            // 追加中の場合は終了
            if (this.AddingFlg) return;

            // 操作をキャンセル
            e.Cancel = true;
        }

        private void c1ScheduleHaisha_BeforeAppointmentDelete(object sender, CancelAppointmentEventArgs e)
        {
            // 操作をキャンセル
            e.Cancel = true;
        }

        /// <summary>
        /// 配車情報検索イベント
        /// </summary>
        /// <param name="e"></param>
        private void btnSearchHaisha_Click(object sender, EventArgs e)
        {
            // 配車情報表示
            DoGetHaishaSearchData();

        }

        /// <summary>
        /// フォーム初回起動イベント
        /// </summary>
        /// <param name="e"></param>
        private void HaishaIchiranFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        /// <summary>
        /// フォームキーダウンイベント
        /// </summary>
        /// <param name="e"></param>
        private void HaishaIchiranFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        /// <summary>
        /// フォームクローズイベント
        /// </summary>
        /// <param name="e"></param>
        private void HaishaIchiranFrame_FormClosing(object sender, FormClosingEventArgs e)
        {

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
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
        }

        /// <summary>
        /// 社員コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numStaffCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateStaffCd(e);
        }

        /// <summary>
        /// 配車情報のDoubleClickイベント(配車詳細入力画面表示)
        /// </summary>
        /// <param name="e"></param>
        private void c1ScheduleHaisha_DoubleClick(object sender, EventArgs e)
        {
            if (0 == c1ScheduleHaisha.SelectedAppointments.Count) return;
            var appoint = c1ScheduleHaisha.SelectedAppointments[0];
            if (null != appoint.Tag)
            {
                // 休日以外の場合
                if (((HaishaIchiranAppointInfo)appoint.Tag).KyujitsuFlg) return;

                // appoint情報を取得
                HaishaIchiranAppointInfo appInfo = (HaishaIchiranAppointInfo)appoint.Tag;
                this.DoUpdateEntry(appInfo.HaishaInfo, appInfo.JuchuInfo);
            }
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
                if (0 == c1ScheduleHaisha.SelectedAppointments.Count) return;
                var appoint = c1ScheduleHaisha.SelectedAppointments[0];
                if (null != appoint.Tag)
                {
                    // 休日以外の場合
                    if (((HaishaIchiranAppointInfo)appoint.Tag).KyujitsuFlg) return;

                    // appoint情報を取得
                    HaishaIchiranAppointInfo appInfo = (HaishaIchiranAppointInfo)appoint.Tag;
                    this.DoUpdateEntry(appInfo.HaishaInfo, appInfo.JuchuInfo);
                }
            }
        }

        /// <summary>
        /// 自動更新チェックボックスの変更イベント
        /// </summary>
        /// <param name="e"></param>
        private void chkAutoSearchFlag_CheckedChanged(object sender, EventArgs e)
        {
            // 初期化中は処理中断
            if (this.InitFlg) return;

            if (this.chkAutoSearchFlag.Checked)
            {
                // タイマー起動
                timer.Enabled = true;
            }
            else 
            {
                // タイマー停止
                timer.Enabled = false;
            }
        }

        /// <summary>
        /// 未配のみチェックボックスの変更イベント
        /// </summary>
        /// <param name="e"></param>
        private void chkMihainomiFlag_CheckedChanged(object sender, EventArgs e)
        {
            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // 配車情報を再描画
                this.HaishaShowSchedule();
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
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
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddSearchableControls(
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
            this.ActiveControl = this.dteTargetDate;
        }

        /// <summary>
        /// 表示開始日付Changeイベント
        /// </summary>
        private void dteTargetDate_Spin(int d)
        {
            try
            {
                this.dteTargetDate.Value = Convert.ToDateTime(this.dteTargetDate.Value).AddDays(d);
            }
            catch
            {
            }
        }


        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaishaIchiranFrame();
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
        private void InitScreen()
        {

            //描画を停止
            this.c1ScheduleHaisha.SuspendLayout();

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //入力項目のクリア
                this.ClearInputs();

                // 配車情報表示
                this.ShowSchedule(true, this.GetScreen());

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
                this.c1ScheduleHaisha.ResumeLayout();
            }
        }

        /// <summary>
        /// 配車一覧画面の検索条件を取得します。
        /// </summary>
        /// <param name="keyFlg">キー検索フラグ</param>
        private HaishaIchiranSearchParameter GetScreen(bool keyFlg = false)
        {

            // 検索条件設定
            var para = new HaishaIchiranSearchParameter();

            // 配車情報検索条件
            para.DispStratYMD = this.dteTargetDate.Value.Value.Date;
            para.DispEndYMD = this.dteTargetDate.Value.Value.AddDays(DEFAULT_DISP_DAYS).Date;
            para.CarCode = (int?)this.numCarCd.Value;
            para.CarKindCode = (int?)this.numCarKindCd.Value;
            para.StaffCd = (int?)this.numStaffCd.Value;
            para.CarKbn = (int?)cmbCarKbn.SelectedValue;
            para.HaishaBranchOfficeId = (decimal?)cmbHaishaBranchOffice.SelectedValue;
            para.DisableFlag = this.chkAllFlag.Checked;

            // キー検索フラグ
            para.KeyFlg = keyFlg;

            return para;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputs()
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            //返却
            return rt_val;
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
            if (SearchFunctions.ContainsKey(this.splitContainer1.ActiveControl))
            {
                SearchFunctions[this.splitContainer1.ActiveControl]();
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
                    ((GrapeCity.Win.Editors.GcNumber)this.splitContainer1.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONCarKindCode);

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
        /// 検索条件に一致する配車情報を取得します。
        /// </summary>
        private void DoGetHaishaSearchData()
        {
            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
            {
                //描画を停止
                this.c1ScheduleHaisha.SuspendLayout();

                //マウスカーソルを待機中(砂時計)に変更
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    // 配車情報を検索
                    this.ShowSchedule(true, this.GetScreen());
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

        #endregion

        #region 検証（Validate）処理

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

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numCarKindCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 車両情報を取得
                CarKindSearchParameter para = new CarKindSearchParameter();
                para.ToraDONCarKindCode = Convert.ToInt32(this.numCarKindCd.Value);
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
                    this.numCarKindCd.Tag = info.ToraDONCarKindId;
                    this.numCarKindCd.Value = info.ToraDONCarKindCode;
                    this.edtCarKindNM.Text = info.ToraDONCarKindName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numCarKindCd.Tag = null;
                    this.numCarKindCd.Value = null;
                    this.edtCarKindNM.Text = string.Empty;
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

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numStaffCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 乗務員情報を取得
                StaffSearchParameter para = new StaffSearchParameter();
                para.ToraDONStaffCode = Convert.ToInt32(this.numStaffCd.Value);
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
                    this.numStaffCd.Tag = info.ToraDONStaffId;
                    this.numStaffCd.Value = info.ToraDONStaffCode;
                    this.edtStaffNM.Text = info.ToraDONStaffName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numStaffCd.Tag = null;
                    this.numStaffCd.Value = null;
                    this.edtStaffNM.Text = string.Empty;
                }
            }
        }

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        #endregion

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            // タイマーを切る
            timer.Enabled = false;

            this.Close();
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

            foreach (SystemNameInfo item in list)
            {
                datasource.Add(item.SystemNameCode, item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbTimeInterval, datasource, false, null, false);

            this.cmbTimeInterval.SelectedIndex = 0;
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

            // 配車入力用
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbHaishaBranchOffice, datasource, true, DefaultProperty.CMB_BRANCHOFFICE_ALL_NAME, false);

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
        /// DBからデータを取得しスケジュールを表示する。
        /// コンボや、日付が変わった時に呼ばれます。
        /// </summary>
        /// <param name="isClearCategories">カテゴリを再描画するかどうか</param>
        private void ShowSchedule(bool isClearCategories, HaishaIchiranSearchParameter para = null)
        {
            //描画を停止
            this.c1ScheduleHaisha.Visible = false;

            try
            {
                // 配車情報を検索
                this.ShowHaishaList(isClearCategories, para);
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
        /// 車両別スケジュールを表示する
        /// </summary>
        /// <param name="isClearCategories"></param>
        private void ShowHaishaList(bool isClearCategories, HaishaIchiranSearchParameter para = null)
        {

            // 表示開始日付
            var selectedDate = dteTargetDate.Value.Value;
            c1ScheduleHaisha.GoToDate(selectedDate);

            // 休日情報の取得
            List<HaishaKyujitsuCalendarInfo> kyujitsuCalendarList = _HaishaIchiran.GetKyujitsuCalendar(para);

            // 配車情報を取得
            this._DataOfCurrentDay = _HaishaIchiran.GetHaisha(para);

            // 未配車両の一覧取得（「未配車両のみ」にチェックが入っていない場合は空）
            var haishaCarDictionar = this.GEtHaishaCarDictionar(para.DispStratYMD, kyujitsuCalendarList);

            // フラグが立っているときはカテゴリをリセット
            if (isClearCategories)
            {
                c1ScheduleHaisha.DataStorage.CategoryStorage.Categories.Clear();
                CarDictionary = new Dictionary<decimal, Category>();

                // 車両一覧を取得
                var cars = _HaishaIchiran.GetCar(para);

                foreach (HaishaIchiranCarInfo carRow in cars)
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

                    // 未配車両の一覧にない物のみ一覧に設定
                    if (!haishaCarDictionar.ContainsKey(carRow.CarId))
                    {
                        c1ScheduleHaisha.DataStorage.CategoryStorage.Categories.Add(category);
                    }
                    
                    CarDictionary.Add(carRow.CarId, category);
                }
            }

            // 予定のリセット
            c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Clear();

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

                    // appoint.Duration = TimeSpan.FromMinutes(DEFAULT_JUC_DUARATION);
                    appoint.Categories.Add(CarDictionary[info.CarId]);

                    // 往復区分でステータスを指定
                    appoint.BusyStatus = StatusNon;

                    // 配車情報詳細を保持
                    HaishaIchiranAppointInfo appointInfo = new HaishaIchiranAppointInfo();
                    appointInfo.KyujitsuFlg = true;
                    appointInfo.KyujitsuFontColor = _KyujitsuForeColorDic[info.KyujitsuKbn];

                    appoint.Tag = appointInfo;

                }
            }



            // データがなければ画面をクリアして抜ける
            if (this._DataOfCurrentDay.Count == 0)
            {
                return;
            }

            //// 予定のリセット
            //c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Clear();
            //int scheduleInterval = (int)c1ScheduleHaisha.CalendarInfo.TimeInterval;

            foreach (HaishaSearchResultInfo info in this._DataOfCurrentDay)
            {

                // スケジュールを追加
                var appoint = this.c1ScheduleHaisha.DataStorage.AppointmentStorage.Appointments.Add();

                HaishaNyuryokuInfo item = info.HaishaInfo;

                // ラベル情報（背景色など）
                appoint.Label = HaisoLabel;

                // 開始日時
                appoint.Start = item.TaskStartDateTime;

                // 終了日時
                appoint.End = item.ReuseYMD;

                // 件名
                appoint.Subject = item.TokuisakiName + " " + item.StartPointName + " → " + item.EndPointName;
                //if (item.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                //{
                //    // 傭車の場合
                //    appoint.Subject = "【" + item.TokuisakiName + "】" + item.ItemName + "（" + item.TorihikiShortName + "）";
                //}
                //else
                //{
                //    // 自社車両の場合
                //    appoint.Subject = "【" + item.TokuisakiName + "】" + item.ItemName + "（" + item.StaffName + "）";
                //}

                appoint.Categories.Add(CarDictionary[item.CarId]);

                // 往復区分でステータスを指定
                appoint.BusyStatus = GetStatus(item.OfukuKbn);

                // 配車情報詳細を保持
                HaishaIchiranAppointInfo appointInfo = new HaishaIchiranAppointInfo();
                appointInfo.HaishaInfo = item;
                appointInfo.JuchuInfo = info.JuchuInfo;
                appointInfo.KyujitsuFlg = false;

                appoint.Tag = appointInfo;
            }

        }

        /// <summary>
        /// 日付が変更された場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dteTargetDate_Validating(object sender, EventArgs e)
        {
            // 初期化中は処理中断
            if (this.InitFlg) return;

            if (!dteTargetDate.Value.HasValue)
                return;

            //描画を停止
            this.c1ScheduleHaisha.SuspendLayout();

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // 配車情報を検索
                this.ShowSchedule(true, this.GetScreen());
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

        /// <summary>
        /// 時間間隔の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbTimeInterval_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                //マウスカーソルを待機中(砂時計)に変更
                Cursor.Current = Cursors.WaitCursor;

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
        /// 車両区分の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCarKbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // 配車情報を再描画
                this.HaishaShowSchedule();
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 配車用営業所の変更があった場合、スケジュールを再描画する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHaishaBranchOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // 配車情報を再描画
                this.HaishaShowSchedule();
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
            
            try
            {
                //マウスカーソルを待機中(砂時計)に変更
                Cursor.Current = Cursors.WaitCursor;

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

        /// <summary>
        /// 配車一覧の表示期間を設定します
        /// </summary>
        /// <param name="BaseDate">基準日</param>
        /// <param name="DateRange">期間</param>
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
        private HaishaShosaiNyuryokuFrame.HaishaDialogResult DoUpdateEntry(HaishaNyuryokuInfo haishainfo, JuchuInfo juchuInfo)
        {
            // 選択行のTagから受注情報を取得

            try
            {
                // 編集フラグOFF
                haishainfo.UppdateFlg = false;

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
                HaishaIchiranAppointInfo appInfo = (HaishaIchiranAppointInfo)e.Appointment.Tag;

                // 休日の予定の場合
                if (appInfo != null && appInfo.KyujitsuFlg)
                {
                    // 16進数へ変換
                    String htmlColor = String.Format("#{0:X2}{1:X2}{2:X2}"
                        , appInfo.KyujitsuFontColor.R
                        , appInfo.KyujitsuFontColor.G
                        , appInfo.KyujitsuFontColor.B);

                    e.Text = "<p style='color:" + htmlColor + "'>" + e.Text + "</p>";

                }
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

            if (!dteTargetDate.Value.HasValue)
                return;
            this.ShowSchedule(true, this.GetScreen());

            // 追加中フラグOFF
            this.AddingFlg = false;

            // 描画を再開
            this.c1ScheduleHaisha.ResumeLayout();
        }

        /// <summary>
        /// 対象日が配車済みとなっている車両の一覧を取得します。
        /// </summary>
        /// <param name="day">対象日</param>
        /// <param name="kyujitsuCalendarList">休日検索結果</param>
        /// <returns></returns>
        private Dictionary<decimal, decimal> GEtHaishaCarDictionar(DateTime day, List<HaishaKyujitsuCalendarInfo> kyujitsuCalendarList)
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
                if (info.HizukeYMD.Date == day.Date)
                {
                    if (!haishaCarDictionar.ContainsKey(info.CarId)) haishaCarDictionar.Add(info.CarId, info.CarId);
                }
            }

            // 配車の検索結果
            foreach (HaishaSearchResultInfo info in this._DataOfCurrentDay)
            {

                HaishaNyuryokuInfo item = info.HaishaInfo;

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
    }
}
