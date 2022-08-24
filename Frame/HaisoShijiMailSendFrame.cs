using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.SQLServerDAL;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaisoShijiMailSendFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配送指示メール送信処理";

        /// <summary>
        /// 本文境界線メイン
        /// </summary>
        private const string SPLITTER_MAIN = "-------------------------------------------";

        /// <summary>
        /// 本文境界線サブ
        /// </summary>
        private const string SPLITTER_SUB = "- - - - - - - - - - - - - - - - - - - - - -";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示している印刷操作メニューを保持する領域
        /// </summary>
        private AddActionMenuItem _AddActionMenuItem;

        /// <summary>
        /// 取得した配送指示メール送信条件を保持する領域
        /// </summary>
        private HaisoShijiMailSendSearchParameter _HaisoShijiMailSendSearchParameter = null;

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
        private const int COL_CODE = 0;

        /// <summary>
        /// 乗務員名列番号
        /// </summary>
        private const int COL_NAME = 1;

        /// <summary>
        /// 乗務員カナ名列番号
        /// </summary>
        private const int COL_KNAME = 2;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_SELECT_CHECKBOX = 3;

        /// <summary>
        /// 乗務員Id列番号
        /// </summary>
        private const int COL_ID = 4;

        /// <summary>
        /// 乗務員リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_LIST = 5;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialStaffStyleInfoArr;

        /// <summary>
        /// 乗務員情報のリストを保持する領域
        /// </summary>
        private IList<StaffInfo> _StaffInfoList = null;

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
        public HaisoShijiMailSendFrame()
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
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            this._AddActionMenuItem = new AddActionMenuItem();

            this._AddActionMenuItem.SetCreatingItem(ActionMenuItems.Send);
            this._AddActionMenuItem.SetCreatingItem(ActionMenuItems.Separator);
            this._AddActionMenuItem.SetCreatingItem(ActionMenuItems.Close);

            this._AddActionMenuItem.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***送信
            commandSet.Send.Execute += Send_Execute;
            commandSet.Bind(commandSet.Send,
               this.btnSend, _AddActionMenuItem.GetMenuItemBy(ActionMenuItems.Send), this.toolStripSend);

            //***終了
            commandSet.Close.Execute += CloseCommand_Execute;
            commandSet.Bind(commandSet.Close,
               this.btnClose, _AddActionMenuItem.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        private void Send_Execute(object sender, EventArgs e)
        {
            // 送信
            this.DoSend();
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
        }

        /// <summary>
        /// 乗務員リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStaffStyleInfo()
        {
            // Id列非表示
            this.fpStaffListGrid.Sheets[0].Columns[COL_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpStaffListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialStaffStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_LIST];

            for (int i = 0; i < COL_MAXCOLNUM_LIST; i++)
            {
                this.initialStaffStyleInfoArr[i] =
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
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_LIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //乗務員印刷条件
            this.dteHizukeYMDFrom.Value = DateTime.Today.AddDays(1);
            this.dteHizukeYMDTo.Value = DateTime.Today.AddDays(1);
            this.edtTitlePrefix.Text = string.Empty;
            this.edtAttentions.Text = string.Empty;

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
            this.dteHizukeYMDFrom.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 乗務員リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckStaffList(int rowIndex)
        {
            this.fpStaffListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpStaffListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value));
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

                //検索結果一覧の取得を指示
                this._StaffInfoList =
                    this._DalUtil.Staff.GetList(new StaffSearchParameter() { DriverFlag = true });

                //乗務員リスト取得
                wk_list = this._StaffInfoList
                    .Where(x => x.ToraDONDisableFlag == false
                        && x.SendFlag == true
                        && Regex.IsMatch(x.Email, HaisoShijiMailSendToListFrame.EMAIL_REGEX))
                    .ToList();

                //件数取得
                rowCount = wk_list.Count;

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_LIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStaffStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    StaffInfo wk_info = wk_list[i];

                    datamodel.SetValue(i, COL_CODE, wk_info.ToraDONStaffCode);
                    datamodel.SetValue(i, COL_NAME, wk_info.ToraDONStaffName);
                    datamodel.SetValue(i, COL_KNAME, wk_info.ToraDONStaffNameKana);
                    datamodel.SetValue(i, COL_SELECT_CHECKBOX, false);
                    datamodel.SetValue(i, COL_ID, wk_info.ToraDONStaffId);
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
        /// 指定した印刷条件で印刷プレビューを表示します。
        /// </summary>
        /// <returns>処理結果</returns>
        private void DoSend()
        {
            this.SendMail();
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

            this.CreateSendDataAsync(this._HaisoShijiMailSendSearchParameter).ContinueWith(t =>
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
                    KanriInfo kanriInfo = this._DalUtil.Kanri.GetInfo();

                    //送信者情報取得
                    var sender = new EmailAddress(kanriInfo.Email, kanriInfo.CompanyName);

                    //メールタイトル取得
                    var titleBase = this._HaisoShijiMailSendSearchParameter.TitlePrefix + kanriInfo.HaisoShijiMailTitle;

                    //メール本文のひな型取得
                    var bodyTemplate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(this._HaisoShijiMailSendSearchParameter.Attentions))
                    {
                        bodyTemplate = this._HaisoShijiMailSendSearchParameter.Attentions + "\n";
                    }
                    bodyTemplate = bodyTemplate + "$body";

                    //日付範囲取得
                    var ymd = this._HaisoShijiMailSendSearchParameter.HizukeYMDFrom.ToString("MM/dd")
                            + "～"
                            + this._HaisoShijiMailSendSearchParameter.HizukeYMDTo.ToString("MM/dd");

                    //受信者リスト
                    List<EmailAddress> recipientList = new List<EmailAddress>();
                    //受信メールタイトルリスト
                    List<string> titles = new List<string>();
                    //本文置換リスト
                    List<Dictionary<string, string>> substitutions = new List<Dictionary<string, string>>();

                    //乗務員グループ取得
                    var driverList = sendData.GroupBy(x => new { DriverId = x.DriverId });

                    //乗務員の件数回、処理を繰り返す
                    foreach (var drivergroup in driverList)
                    {
                        //乗務員ベース情報取得
                        HaisoShijiMailSendInfo driverinfo = drivergroup.First();

                        //受信者リスト追加
                        recipientList.Add(new EmailAddress(driverinfo.Email, driverinfo.StaffName));

                        //受信メールタイトルリスト追加
                        titles.Add(titleBase + ymd + " " + driverinfo.StaffName);

                        //車両グループ取得
                        var carList = sendData.Where(x => x.DriverId == driverinfo.DriverId).GroupBy(x => new { CarId = x.CarId });

                        //本文ワーク
                        StringBuilder sb = new StringBuilder();

                        //乗務員初回フラグ
                        bool isDriverFirst = true;

                        //車両の件数回、処理を繰り返す
                        foreach (var cargroup in carList)
                        {
                            //乗務員の初回時
                            if (isDriverFirst)
                            {
                                //開始罫線取得
                                sb.AppendLine(SPLITTER_MAIN);

                                //乗務員の初回終了
                                isDriverFirst = false;
                            }

                            //車両初回フラグ
                            bool isCarFirst = true;

                            //配車の件数回、処理を繰り返す
                            foreach (var carinfo in cargroup)
                            {
                                //車両の初回時
                                if (isCarFirst)
                                {
                                    //車番取得
                                    sb.AppendLine("車両：" + carinfo.LicPlateCarNo);
                                    sb.AppendLine(SPLITTER_MAIN);

                                    //車両の初回終了
                                    isCarFirst = false;
                                }

                                //配車情報取得
                                //数量小数部桁数
                                int decimalpart = UserProperty.GetInstance().JuchuNumberDecimalDigits;
                                sb.AppendLine("積日 " + carinfo.TaskStartDateTime.ToString("yyyy/MM/dd HH:mm") + " " + carinfo.StartPointName);
                                sb.AppendLine("品目 " + (carinfo.ItemName.Equals(string.Empty) ? string.Empty : (carinfo.ItemName + " "))
                                        + carinfo.Number.ToString("###,##0." + new string('0', decimalpart)) + " "
                                        + (carinfo.FigName.Equals(string.Empty) ? string.Empty : (carinfo.FigName + " ")) + " "
                                        + (0 < carinfo.Weight ? (Convert.ToDecimal(carinfo.Weight).ToString("##,###") + "kg") : string.Empty));
                                sb.AppendLine("発日 " + carinfo.StartYMD.ToString("yyyy/MM/dd HH:mm"));
                                sb.AppendLine("着日 " + carinfo.TaskEndDateTime.ToString("yyyy/MM/dd HH:mm") + " " + carinfo.EndPointName);
                                //sb.AppendLine("再使日 " + carinfo.ReuseYMD.ToString("yyyy/MM/dd HH:mm"));
                                sb.AppendLine(SPLITTER_SUB);
                                sb.AppendLine("積地 " + carinfo.StartPointAddress1 + carinfo.StartPointAddress2);
                                sb.AppendLine("着地 " + carinfo.EndPointAddress1 + carinfo.EndPointAddress2);
                                sb.AppendLine("得意先 " + carinfo.TokuisakiName);
                                sb.AppendLine("備考 " + carinfo.Biko);
                                sb.AppendLine(SPLITTER_MAIN);
                            }
                        }

                        //本文置換リスト取得
                        substitutions.Add(new Dictionary<string, string> { { "$body", sb.ToString() } });
                    }

                    //一括送信
                    Response resultResponse = FrameLib.MailHelper.SendMail(sender, recipientList, titles, bodyTemplate, substitutions);

                    //送信処理判定
                    if (resultResponse.StatusCode != HttpStatusCode.Accepted)
                    {
                        throw new CanRetryException("メール送信処理に失敗しました。\r\n\r\n結果コード："
                            + ((int)resultResponse.StatusCode).ToString() + " " + resultResponse.StatusCode.ToString()
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
                    //画面を使用可能に
                    this.ControlEnableChangeForPrinting(true);
                    //画面タイトルをもとに戻す
                    this.Text = WINDOW_TITLE;
                    //マウスカーソルを元に戻す
                    this.Cursor = Cursors.Default;
                    //条件項目にフォーカス
                    this.SetFirstFocus();
                }
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
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

            //乗務員の必須チェック
            if (rt_val)
            {
                bool exists = false;

                for (int i = 0; i < this.fpStaffListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpStaffListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { "乗務員" });
                    ctl = this.fpStaffListGrid;
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

            this._AddActionMenuItem.ActionMenuItemEnable(ActionMenuItems.Send, val);
            this._AddActionMenuItem.ActionMenuItemEnable(ActionMenuItems.Close, val);
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
                ""
                ;
        }

        /// <summary>
        /// 画面から更新に必要な情報をメンバにセットします。
        /// </summary>
        private void GetSendParameterFromScreen()
        {
            this._HaisoShijiMailSendSearchParameter = new HaisoShijiMailSendSearchParameter();

            this._HaisoShijiMailSendSearchParameter.HizukeYMDFrom = this.HizukeYMDFrom.Value;
            this._HaisoShijiMailSendSearchParameter.HizukeYMDTo = new DateTime(
                this.HizukeYMDTo.Value.Year
                , this.HizukeYMDTo.Value.Month
                , this.HizukeYMDTo.Value.Day
                , 23
                , 59
                , 59);
            this._HaisoShijiMailSendSearchParameter.TitlePrefix = this.edtTitlePrefix.Text;
            this._HaisoShijiMailSendSearchParameter.Attentions = this.edtAttentions.Text;

            //乗務員リスト
            IList<SendMailStaffInfo> stafflist = new List<SendMailStaffInfo>();
            SheetView sheet0 = this.fpStaffListGrid.ActiveSheet;

            //チェックボックスを全削除
            for (int i = 0; i < sheet0.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet0.GetValue(i, COL_SELECT_CHECKBOX)))
                {
                    SendMailStaffInfo info = new SendMailStaffInfo();
                    info.StaffCode = Convert.ToInt32(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_CODE].Value);
                    info.StaffName = Convert.ToString(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_NAME].Value);
                    info.StaffNameKana = Convert.ToString(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_KNAME].Value);
                    info.CheckedFlag = Convert.ToBoolean(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value);
                    info.StaffId = Convert.ToDecimal(
                        this.fpStaffListGrid.Sheets[0].Cells[i, COL_ID].Value);
                    stafflist.Add(info);
                }
            }

            this._HaisoShijiMailSendSearchParameter.SendMailStaffList = stafflist;
        }

        /// <summary>
        /// 送信データを非同期で取得します。
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public Task<IList<HaisoShijiMailSendInfo>> CreateSendDataAsync(
            HaisoShijiMailSendSearchParameter para)
        {
            return
                Task.Factory.StartNew(() =>
                {
                    //戻り値
                    IList<HaisoShijiMailSendInfo> ret_list = new List<HaisoShijiMailSendInfo>();

                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        ret_list = new HaisoShijiMailSend(this.appAuth).GetHaisha(para);
                    });

                    return ret_list;
                });
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
                string Staffcodestring =
                    string.Format("個別指定 {0}", string.Join(",", this._HaisoShijiMailSendSearchParameter.SendMailStaffList.Select(x => x.StaffCode).ToList()));

                string rt_str = string.Format("[乗務員] {0}", Staffcodestring);

                if (255 < rt_str.Length)
                {
                    rt_str = rt_str.Substring(0, 255);
                }

                return string.Format("[乗務員] {0}", Staffcodestring);
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

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            //チェックボックス全削除
            for (int i = 0; i < this.fpStaffListGrid.Sheets[0].RowCount; i++)
            {
                this.fpStaffListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = true;
            }
        }

        private void btnAllRemove_Click(object sender, EventArgs e)
        {
            //チェックボックス全削除
            for (int i = 0; i < this.fpStaffListGrid.Sheets[0].RowCount; i++)
            {
                this.fpStaffListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = false;
            }
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
