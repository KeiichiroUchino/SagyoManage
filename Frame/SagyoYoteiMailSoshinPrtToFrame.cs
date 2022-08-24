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
using System.ComponentModel;
using jp.co.jpsys.util;
using System.Drawing;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.SagyoManage.ReportModel;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using Jpsys.SagyoManage.Frame.EventData;
using System.Collections;
using Jpsys.SagyoManage.ReportFrame;
using Jpsys.SagyoManage.ReportDAL;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace Jpsys.SagyoManage.Frame
{
    public partial class SagyoYoteiMailSoshinPrtToFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoYoteiMailSoshinPrtToFrame()
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
        private const string WINDOW_TITLE = "作業予定メール送信";

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
        /// 契約一覧クラス
        /// </summary>
        private SagyoAnken _SagyoAnken;

        /// <summary>
        /// 検索した社員情報リストを保持する領域
        /// </summary>
        private List<StaffInfo> _StaffInfoList = new List<StaffInfo>();

        ///// <summary>
        ///// 登録済みリストがダブルクリックされたかどうかを表す値を保持します。
        ///// </summary>
        //private bool isDoubleClickedListSheet = false;

        /// <summary>
        /// 登録済みの一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx = 0;

        /// <summary>
        /// 再表示する際、データがない場合メッセージを表示するかしないかの値を保持します。
        /// 初期値は、true:メッセージを表示。
        /// </summary>
        private bool _ReloadMessageFlg = true;

        /// <summary>
        /// 取得したメール送信条件を保持する領域
        /// </summary>
        private HibetsuSagyoKeikakuhyouRptInfoSearchParameter _HibetsuSagyoKeikakuhyouRptInfoSearchParameter = null;

        /// <summary>
        /// 作業計画書（日単位）クラス
        /// </summary>
        private HibetsuSagyoKeikakuhyouRpt _HibetsuSagyoKeikakuhyouRpt;

        #region Spread列定義

        #region 社員一覧

        /// <summary>
        /// 社員一覧の列名を表します。
        /// </summary>
        private enum SpreadColKeys
        {
            StaffId,
            StaffCode,
            StaffName,
            StaffKbn,
            Check,
        }

        /// <summary>
        /// 社員一覧最大列数
        /// </summary>
        private int COL_MAXCOLUM_LIST = Enum.GetNames(typeof(SpreadColKeys)).Length;

        /// <summary>
        /// フィルタ情報を保持する領域
        /// </summary>
        private FarPoint.Win.Spread.HideRowFilter hrFilter;
        private string[] filterStrings;
        private FarPoint.Win.Spread.Model.SortIndicator[] sortIndicators;

        #endregion

        #endregion

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitJuchuIchiranFrame()
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

            //契約一覧クラスインスタンス作成
            this._SagyoAnken = new SagyoAnken(this.appAuth);

            //条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //データを取得
            this.DoGetData();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);
        }

        /// <summary>
        /// 入力画面より再表示を指示
        /// </summary>
        /// <param name="id"></param>
        public void ReSetScreen(decimal id, bool adjustWidth = false, bool dtlFlg = true)
        {
            //入力画面より再表示を指示、件数が0件の場合でもメッセージを出さない
            this._ReloadMessageFlg = false;
            //フィルタ情報を保持
            this.GetFilterInfo();
            this.DoGetData(id);
            //フィルタ情報を復元
            this.SetFilterInfo();
            this._ReloadMessageFlg = true;
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
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Send);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            //行数の初期化
            sheet0.Models.Data.RowCount = 0;

            //フィルター・ソートをクリアする
            this.fpListGrid.ActiveSheet.RowFilter.ResetFilter();
            FrameUtilites.ResetSortInfo(this.fpListGrid.ActiveSheet);

            //マウスポインタを矢印キーにする
            this.fpListGrid.SetCursor(FarPoint.Win.Spread.CursorType.Normal, System.Windows.Forms.Cursors.Arrow);

            this.selectrowidx = 0;
        }

        /// <summary>
        /// 画面送信条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.dteKeiyakuYMDFrom.Value = DateTime.Today.Date.AddDays(1);
            this.dteKeiyakuYMDTo.Value = DateTime.Today.Date.AddDays(1);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            //validatingEventRaiserCollection.Add(
            //    ControlValidatingEventRaiser.Create(this.dteKeiyakuYMDFrom, ctl => ctl.Text, this.dteKeiyakuYMDFrom_Validating));
        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***送信
            _commandSet.Send.Execute += Send_Execute;
            _commandSet.Bind(_commandSet.Send,
               this.btnSend, actionMenuItems.GetMenuItemBy(ActionMenuItems.Send), this.toolStripSend);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
        }

        private void Send_Execute(object sender, EventArgs e)
        {
            //送信
            this.SendMail();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
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
                    ((NskDateTime)c).Value = Convert.ToDateTime(((NskDateTime)c).Value).AddDays(days);
                }
                catch
                {
                    ;
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
            this.InitJuchuIchiranFrame();
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
                    this.pnlTop.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    this.pnlTop.Enabled = false;
              
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;

                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// メール送信条件を取得します
        /// </summary>
        /// <returns>日別作業計画情報一覧の検索条件</returns>
        private void GetSendParameterFromScreen()
        {
            HibetsuSagyoKeikakuhyouRptInfoSearchParameter para = new HibetsuSagyoKeikakuhyouRptInfoSearchParameter();

            // 作業日(開始)
            if (this.dteKeiyakuYMDFrom.Value != null)
            {
                para.SagyoYMDFrom = this.dteKeiyakuYMDFrom.Value.Value;
            }
            else
            {
                para.SagyoYMDFrom = DateTime.MinValue;
            }

            // 作業日(終了)
            if (this.dteKeiyakuYMDFrom.Value != null)
            {
                para.SagyoYMDTo = this.dteKeiyakuYMDTo.Value.Value;
            }
            else
            {
                para.SagyoYMDTo = DateTime.MinValue;
            }

            this._HibetsuSagyoKeikakuhyouRptInfoSearchParameter = para;

            IList<decimal> stafflist = new List<decimal>();
            SheetView sheet0 = this.fpListGrid.ActiveSheet;

            //チェックボックスを全削除
            for (int i = 0; i < sheet0.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet0.GetValue(i, (int)SpreadColKeys.Check)))
                {
                    stafflist.Add(((StaffInfo)sheet0.GetTag(i, (int)SpreadColKeys.StaffId)).StaffId);
                }
            }

            this._HibetsuSagyoKeikakuhyouRptInfoSearchParameter.StaffList = stafflist;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen(decimal id)
        {
            this.SetJuchuListMrow(id);
        }

        /// <summary>
        /// 画面コントローラのメンバに読み込んだ旅券売上情報を画面にセットします。
        /// </summary>
        private void SetJuchuListMrow(decimal id)
        {
            //SheetView変数定義
            SheetView sheet = this.fpListGrid.ActiveSheet;

            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            var list = this._StaffInfoList
                        .OrderBy(x => x.StaffCode)
                        .ToList();

            try
            {
                //登録済みの一覧を初期化
                this.InitSheet();

                //件数取得
                int rowCount = list.Count;
                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを抜き出す
                var datamodel = sheet.Models.Data;
                //Spreadのスタイルモデルを抜き出す
                var stylemodel = sheet.Models.Style;

                datamodel.RowCount = rowCount;

                int row = 0;

                foreach (var item in list)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLUM_LIST; k++)
                    {
                        //ヘッダーのスタイルを取得
                        StyleInfo sInfo = new StyleInfo(stylemodel.GetDirectInfo(-1, k, null));
                        stylemodel.SetDirectInfo(row, k, sInfo);
                    }

                    datamodel.SetValue(row, (int)SpreadColKeys.StaffId, item.StaffId);
                    datamodel.SetValue(row, (int)SpreadColKeys.StaffCode, item.StaffCode);

                    datamodel.SetValue(row, (int)SpreadColKeys.StaffName, item.StaffName);
                    datamodel.SetValue(row, (int)SpreadColKeys.StaffKbn, item.StaffKbnName);
                    datamodel.SetValue(row, (int)SpreadColKeys.Check, false);

                    //タグに情報を保持しておく
                    datamodel.SetTag(row, (int)SpreadColKeys.StaffId, item);

                    row++;

                }

                //選択行にフォーカスを合わせて選択状態にする
                sheet.SetActiveCell(selectrowidx, 0, true);
                sheet.AddSelection(selectrowidx, -1, 1, sheet.ColumnCount - 1);

                //フォーカスを移動
                this.fpListGrid.Focus();

                //選択行にスクロールバーを移動させる
                this.fpListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //デフォルトに戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 検索条件項目をチェックします。
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
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// データを取得し、画面上にセットします。
        /// </summary>
        private void DoGetData(decimal id = 0)
        {
            try
            {
                //待機状態へ
                this.Cursor = Cursors.WaitCursor;

                if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
                {

                    // 画面コントローラに取得を指示
                    // 社員一覧取得
                    this._StaffInfoList = this._DalUtil.Staff.GetList().Where(x=>!string.IsNullOrWhiteSpace(x.MailAddress) && !x.DisableFlag).ToList();

                    // 0件なら処理しない
                    if (this._StaffInfoList.Count == 0 && this._ReloadMessageFlg)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201015"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        this.dteKeiyakuYMDFrom.Focus();
                    }
                    else
                    {
                        //取得したデータを画面にセット
                        this.SetScreen(id);
                    }
                }
            }
            finally
            {
                //カーソを元に戻す
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 指定した条件でメール送信を行います。
        /// </summary>
        /// <returns>処理結果</returns>
        private void SendMail()
        {
            if (!this.ValidateChildren(ValidationConstraints.None) || !this.CheckInputs())
                return;

            //実行確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                FrameUtilites.GetDefineMessage("MQ2101007"),
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            // 画面を使用不可に
            this.ControlEnableChangeForPrinting(false);
            // 画面タイトルを変更
            this.Text = WINDOW_TITLE + "　" + "送信中...";
            // 砂時計
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();

            //送信条件を取得
            this.GetSendParameterFromScreen();

            string filePath = string.Empty;

            this.CreateSendDataAsync(this._HibetsuSagyoKeikakuhyouRptInfoSearchParameter).ContinueWith(async t =>
            {
                //ハンドルが関連付けられていない場合、処理しない
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

                    var sendData = t.Result;

                    //データがない場合はエラーメッセージを表示
                    if (sendData == null || sendData.Count == 0)
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

                    //管理マスタ取得
                    SystemPropertyInfo sysInfo = this._DalUtil.SystemProperty.GetInfo();

                    //印刷用データをイベントデータ作成用のレポートデータの配列を作る
                    IList[] wk_list = { (IList)sendData };

                    var ymdF = DateTime.Now.ToString("yyyyMMddHHmmss");
                    filePath = sysInfo.PDFTempFolderPath + "\\作業計画書（日単位）_" + ymdF+".pdf";

                    //PDFファイル出力処理
                    this.DoPrintReport(wk_list, filePath);

                    //送信者情報取得
                    var sender = new EmailAddress(sysInfo.Email, sysInfo.SenderName);

                    //日付範囲取得
                    var ymd = "(" + this._HibetsuSagyoKeikakuhyouRptInfoSearchParameter.SagyoYMDFrom.ToString("MM/dd")
                            + "～"
                            + this._HibetsuSagyoKeikakuhyouRptInfoSearchParameter.SagyoYMDTo.ToString("MM/dd") 
                            + ")";

                    //メールタイトル取得
                    var titleBase = sysInfo.MailTitle + " " + ymd;

                    //メール本文のひな型取得
                    var bodyTemplate = string.Empty;
                    bodyTemplate = sysInfo.MailBody + "\n";
                    bodyTemplate = bodyTemplate + "$body";

                    //受信者リスト
                    List<EmailAddress> recipientList = new List<EmailAddress>();
                    //受信メールタイトルリスト
                    List<string> titles = new List<string>();
                    //本文置換リスト
                    List<Dictionary<string, string>> substitutions = new List<Dictionary<string, string>>();

                    StaffSearchParameter para = new StaffSearchParameter();
                    para.StaffList = this._HibetsuSagyoKeikakuhyouRptInfoSearchParameter.StaffList;

                    //社員グループ取得
                    var staffList = _DalUtil.Staff.GetList(para);

                    //乗務員の件数回、処理を繰り返す
                    foreach (var info in staffList)
                    {
                        //受信者リスト追加
                        recipientList.Add(new EmailAddress(info.MailAddress, info.StaffName));

                        //受信メールタイトルリスト追加
                        //titles.Add(titleBase + ymd + " " + driverinfo.StaffName);
                        titles.Add(titleBase);

                        //本文ワーク
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(info.StaffName + "様");

                        //本文置換リスト取得
                        substitutions.Add(new Dictionary<string, string> { { "$body", sb.ToString() } });
                    }

                    //一括送信
                    FrameLib.MailHelper mailHelper = new FrameLib.MailHelper();
                    var response = await mailHelper.SendMail(
                        sysInfo.SendGridApiKey,
                        sender,
                        recipientList,
                        titles,
                        bodyTemplate,
                        filePath,
                        substitutions);

                    //送信処理判定
                    if (response.StatusCode != HttpStatusCode.Accepted)
                    {
                        throw new CanRetryException("メール送信処理に失敗しました。\r\n\r\n結果コード："
                            + ((int)response.StatusCode).ToString() + " " + response.StatusCode.ToString()
                            + "\r\n\r\nSendGrid管理者画面より送信状況をご確認の上、\r\n再度メール送信処理を行ってください。", MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MI2001001"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                            );
                    }

                    //操作ログ取得
                    string log_joken = this.CreateLogText();

                    //操作ログ出力
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.ProcessTask,
                        log_joken);
                }
                catch (Exception e)
                {
                    FrameUtilites.ShowExceptionMessage(e, this);
                    return;
                }
                finally
                {
                    if (!string.IsNullOrEmpty(@filePath) && !File.Exists(@filePath))
                    {
                        //一時ファイル削除
                        File.Delete(@filePath);
                    }
                    //画面を使用可能に
                    this.ControlEnableChangeForPrinting(true);
                    //画面タイトルをもとに戻す
                    this.Text = WINDOW_TITLE;
                    //マウスカーソルを元に戻す
                    this.Cursor = Cursors.Default;
                    //条件項目にフォーカス
                    //this.SetFirstFocus();
                }
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string CreateLogText()
        {
            //★TODO ログ文字列確認
            return "";
        }

        private void fpListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckStaffList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        /// <summary>
        /// リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckStaffList(int rowIndex)
        {
            this.fpListGrid.Sheets[0].Cells[rowIndex, (int)SpreadColKeys.Check].Value =
                !Convert.ToBoolean((this.fpListGrid.Sheets[0].Cells[rowIndex, (int)SpreadColKeys.Check].Value));
        }

        /// <summary>
        /// 処理モード、Idを指定して、作業案件入力画面を開きます。
        /// </summary>
        /// <param name="processmode">処理モード</param>
        /// <param name="id">Id</param>
        private void ShowMitsumoriFrame(SagyoAnkenInfo info)
        {

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowidx = select_row;
            }

            SagyoAnkenTorokuFrame f = null;

            if (info.SagyoAnkenId > 0)
            {
                f = new SagyoAnkenTorokuFrame(info.SagyoAnkenId, info.KeiyakuId);
            }
            else
            {
                f = new SagyoAnkenTorokuFrame(info.KeiyakuId);
            }

            f.InitFrame();
            //f.SagyoAnkenIchiran = this;
            f.ShowDialog(this);

            //自身のMdiParentプロパティを入力画面のMdiParentプロパティに
            //セットする
            if (this.MdiParent != null)
            {
                ((Form)f).MdiParent = this.MdiParent;
            }
        }

        /// <summary>
        /// 登録済みリストにて現在選択中の行のindexを取得します。
        /// 未選択の場合は-1を返却します。
        /// </summary>
        /// <returns>現在選択中の行</returns>
        private int GetListSelectionRowIndex()
        {
            //返却値
            int rt_val = -1;

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                rt_val = sheet0.GetSelection(0).Row;
            }

            return rt_val;
        }

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
        /// 作業場所検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchSagyoBasho()
        {
            using (CmnSearchSagyoBashoFrame f = new CmnSearchSagyoBashoFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.SagyoBashoCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 開始日のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteKeiyakuYMDFrom_Validating(object sender, CancelEventArgs e)
        {
            this.dteKeiyakuYMDTo.Value = this.dteKeiyakuYMDFrom.Value;
        }

        #endregion

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

        private void JuchuIchiranFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void JuchuIchiranFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void JuchuIchiranFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            this.DoGetData();
        }

        /// <summary>
        /// フィルタ情報を保持します。
        /// </summary>
        private void GetFilterInfo()
        {
            //SheetView変数定義
            SheetView sheet = this.fpListGrid.ActiveSheet;

            // フィルタ設定の保存
            hrFilter = sheet.RowFilter as FarPoint.Win.Spread.HideRowFilter;
            filterStrings = FrameUtilites.GetFilterInfo(sheet);

            // ソート設定の保存
            sortIndicators = FrameUtilites.GetSortInfo(sheet);
        }

        /// <summary>
        /// フィルタ情報を復元します。
        /// </summary>
        private void SetFilterInfo()
        {
            //SheetView変数定義
            SheetView sheet = this.fpListGrid.ActiveSheet;

            // 保存したフィルタ設定の復元
            FrameUtilites.SetFilterInfo(sheet, filterStrings, hrFilter);

            // 保存したソート設定の復元
            FrameUtilites.SetSortInfo(sheet, sortIndicators);
        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            //チェックボックス全選択
            for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
            {
                this.fpListGrid.Sheets[0].Cells[i, (int)SpreadColKeys.Check].Value = true;
            }
        }

        private void btnAllRemove_Click(object sender, EventArgs e)
        {
            //チェックボックス全解除
            for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
            {
                this.fpListGrid.Sheets[0].Cells[i, (int)SpreadColKeys.Check].Value = false;
            }
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
            this.pnlTop.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Send, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Close, val);
        }

        /// <summary>
        /// 送信データを非同期で取得します。
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Task<IList<HibetsuSagyoKeikakuhyouRptInfo>> CreateSendDataAsync(
            HibetsuSagyoKeikakuhyouRptInfoSearchParameter para)
        {
            return
                Task.Factory.StartNew(() =>
                {
                    //戻り値
                    IList<HibetsuSagyoKeikakuhyouRptInfo> ret_list = new List<HibetsuSagyoKeikakuhyouRptInfo>();

                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        ret_list = new HibetsuSagyoKeikakuhyouRpt(this.appAuth).GetList(para);
                    });

                    return ret_list;
                });
        }

        #region 印刷処理


        /// <summary>
        /// 帳票データの配列と印刷先を表す列挙体を指定して、指定された印刷先に
        /// 応じて印字処理を続行します。
        /// </summary>
        /// <param name="reportDataArray">帳票データの配列</param>
        /// <param name="filePath">PDF出力先</param>
        private void DoPrintReport(IList[] reportDataArray, string filePath)
        {
            try
            {
                //レポートの必要に応じて、レポートデータの配列からデータを抜き出す。
                IList wk_list = reportDataArray[0];

                //レポートオブジェクト作成
                HibetsuSagyoKeikakuhyouToRpt rpt = new HibetsuSagyoKeikakuhyouToRpt();
                //レポートをロード
                rpt.Load();
                //レポートに印刷データをセット
                rpt.SetDataSource(wk_list);

                ////経理部長検印欄フラグの設定
                //bool flgKeiriPrint = setKeiriKeninranPrint(rpt);

                //#region データをセットした後でクリレポのテキストに直接セットします。

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

                //#endregion

                // PDF形式でファイル出力
                try
                {
                    // 出力先ファイル名を指定
                    CrystalDecisions.Shared.DiskFileDestinationOptions fileOption;
                    fileOption = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                    fileOption.DiskFileName = filePath;

                    // 外部ファイル出力をPDF出力として定義する
                    CrystalDecisions.Shared.ExportOptions option;
                    option = rpt.ExportOptions;
                    option.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                    option.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                    option.FormatOptions = new CrystalDecisions.Shared.PdfRtfWordFormatOptions();
                    option.DestinationOptions = fileOption;

                    // pdfとして外部ファイル出力を行う
                    rpt.Export();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ////画面を使用可能に
                //this.ControlEnableChangeForPrinting(true);
                //画面タイトルを変更する
                this.Text = WINDOW_TITLE;

            }
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        public string Createlog()
        {
            string txtlog = string.Empty;

            //操作ログ(保存)の条件取得　★TODO　ログ内容確認
            string log_jyoken = string.Format("ログ内容検討");

            return log_jyoken;
        }

        #endregion
    }
}
