using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;

using System.Configuration;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.FrameLib.ViewSupport;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.Frame.Command;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.BizProperty;

namespace Jpsys.SagyoManage.Frame
{
    public partial class SystemPropertyFrame : Form,IFrameBase
    {
        #region ユーザー定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "基本情報設定";

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
        /// 管理クラス
        /// </summary>
        private SQLServerDAL.SystemProperty _SystemProperty;

        /// <summary>
        /// 現在編集中の管理情報を保持する領域
        /// </summary>
        private SystemPropertyInfo _SystemPropertyInfo = null;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList = null;

        /// <summary>
        /// タブページのIndexを表す列挙体です。
        /// </summary>
        private enum TabPageIndex : int
        {
            /// <summary>
            /// 基本情報設定
            /// </summary>
            BasicInformation = 0,
            /// <summary>
            /// 設定情報設定
            /// </summary>
            SettingInformation = 1,
        }
        
        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

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
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public SystemPropertyFrame()
        {
            //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
            SystemParametersInfo((uint)SystemParametersInfoActionFlag.SPI_SETKEYBOARDCUES, 0,
                true, (uint)(SystemParametersInfoFlag.SPIF_UPDATEINIFILE | SystemParametersInfoFlag.SPIF_SENDWININICHANGE));

            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        private void InitSystemPropertyFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //画面配色の初期化
            this.InitFrameColor();

            //画面配色の設定
            this.statusStrip1.BackColor = FrameUtilites.GetFrameFooterBackColor();

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

            this.systemNameList = this._DalUtil.SystemGlobalName.GetList();

            //管理クラスインスタンス作成
            this._SystemProperty = new SQLServerDAL.SystemProperty(this.appAuth);
            
            //保持するModelのクラスをインスタンス化する
            this.InitInfo();

            //入力項目のクリア
            this.ClearInputs();

            //データを取得し、値をセットする
            this.DoGetData(false);
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
            // 操作メニュー
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            // 操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            // メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);
            // 画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }
        
        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            #region 項目初期化

            //基本情報タブ
            this.edtCompanyName.Text = string.Empty;
            this.edtSenderName.Text = string.Empty;
            this.edtMailTitle.Text = string.Empty;
            this.edtEmail.Text = string.Empty;
            this.edtMailBody.Text = string.Empty;

            //設定情報タブ
            this.edtSendGridApiKey.Text = string.Empty;
            this.edtPDFTempFolderPath.Text = string.Empty;
            this.numDateSwitchingTimeH.Value = 0;
            this.numDateSwitchingTimeM.Value = 0;

            #endregion
        }

        /// <summary>
        /// 保持するModelのクラスがNULLの場合はインスタンス化する
        /// </summary>
        private void InitInfo()
        {
            if (_SystemPropertyInfo == null)
            {
                _SystemPropertyInfo = new SystemPropertyInfo();
            }
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
                _commandSet.Save, this.btnUpdate, actionMenuItems.GetMenuItemBy(ActionMenuItems.Update), this.toolStripSave);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            this.DoGetData(true);
        }

        void Save_Execute(object sender, EventArgs e)
        {
            this.DoUpdate();
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
            //validatingEventRaiserCollection.Add(
            //    ControlValidatingEventRaiser.Create(this.numHatchiCode, ctl => ctl.Text, this.numHatchiCode_Validating));
        }

        #region IFrameBase メンバ

        public void InitFrame()
        {
            this.InitSystemPropertyFrame();
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
        /// 編集取消の確認画面を表示するかどうかを指定して画面の情報からデータを取得します。
        /// </summary>
        /// <param name="showCancelConfirm">取消確認画面を表示するかどうか(true:表示する)</param>
        private void DoGetData(bool showCancelConfirm)
        {
            if (showCancelConfirm)
            {
                //取消確認の実行確認ダイアログ
                DialogResult d_result =
                    MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2102008"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                //Noだったら抜ける
                if (d_result == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                //データを取得後、画面表示します
                this._SystemPropertyInfo = this._SystemProperty.GetInfo();

                if (this._SystemPropertyInfo == null)
                {
                    this._SystemPropertyInfo = new SystemPropertyInfo();
                }
                else
                {
                    this.SetScreen();
                }

                //フォーカス設定
                this.tabSystemProperty.SelectedIndex = (int)TabPageIndex.BasicInformation;
                this.edtCompanyName.Focus();
            }
            catch (CanRetryException ex)
            {
                //他から更新されている場合の例外ハンドラ
                FrameUtilites.ShowExceptionMessage(ex, this);
                this.isConfirmClose = false;
                this.DoClose();
            }
            catch (Exception)
            {
                this.isConfirmClose = false;
                this.DoClose();
                throw;
            }
        }

        /// <summary>
        /// 画面に画面コントロールの値をセットします。
        /// </summary>
        private void SetScreen()
        {
            if (_SystemPropertyInfo == null)
            {
                //基本情報がない場合は入力項目の初期化を行う
                this.ClearInputs();
            }
            else
            {
                #region 基本情報タブ

                //基本情報タブ
                this.edtCompanyName.Text = _SystemPropertyInfo.CompanyName;
                this.edtSenderName.Text = _SystemPropertyInfo.SenderName;
                this.edtMailTitle.Text = _SystemPropertyInfo.MailTitle;
                this.edtEmail.Text = _SystemPropertyInfo.Email;
                this.edtMailBody.Text = _SystemPropertyInfo.MailBody;

                //設定情報タブ
                this.edtSendGridApiKey.Text = _SystemPropertyInfo.SendGridApiKey;
                this.edtPDFTempFolderPath.Text = _SystemPropertyInfo.PDFTempFolderPath;

                var time = _SystemPropertyInfo.DateSwitchingTime.ToString("0000");
                this.numDateSwitchingTimeH.Value = Convert.ToDecimal(time.Substring(0, 2));
                this.numDateSwitchingTimeM.Value = Convert.ToDecimal(time.Substring(2, 2));

                #endregion

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
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.isConfirmClose)
            {
                //編集または新規の場合はメッセージを表示    
                DialogResult result =
                MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MQ2102001"),
                        "確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                        );

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
        /// 画面情報からデータを更新します。
        /// </summary>
        private void DoUpdate()
        {
            //実行確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                FrameUtilites.GetDefineMessage("MQ2102007"),
                this.Text,
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
                //画面から値を取得
                this.GetScreen();

                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._SystemProperty.Save(tx, this._SystemPropertyInfo);
                    });

                    //操作ログ(保存)の条件取得(空文字)
                    string log_jyoken = string.Empty;

                    //保存完了メッセージ格納用
                    string msg = string.Empty;

                    //操作ログ出力
                    if (this.currentMode == FrameEditMode.New)
                    {
                        //操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                           FrameLogWriter.LoggingOperateKind.NewItem,
                               log_jyoken);

                        //登録完了のメッセージ
                        msg =
                           FrameUtilites.GetDefineMessage("MI2001002");
                    }
                    else
                    {
                        //操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.UpdateItem,
                               log_jyoken);

                        //更新完了のメッセージ
                        msg =
                           FrameUtilites.GetDefineMessage("MI2001003");
                    }

                    //登録完了メッセージ
                    MessageBox.Show(
                        msg,
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //登録処理後の画面初期化
                    this.DoGetData(false);

                    //画面を閉じる
                    this.isConfirmClose = false;
                    this.DoClose();
                }
                catch (CanRetryException ex)
                {
                    //他から更新されている場合の例外ハンドラ
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
        /// 入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputs()
        {
            bool rt_val = true;

            //基本情報
            if (rt_val)
            {
                rt_val = this.CheckInputsMainInfo();
            }

            //設定情報
            if (rt_val)
            {
                rt_val = this.CheckInputsSettingInfo();
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// 「基本情報」タブの入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputsMainInfo()
        {
            bool rt_val = true;
            string msg = string.Empty;
            Control ctl = null;
            //フォームコントロールの属しているタブ
            TabPageIndex tbp = TabPageIndex.BasicInformation;

            //会社名の必須入力チェック
            if (rt_val && this.edtCompanyName.Text.Trim().Length == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "会社名" });
                ctl = this.edtCompanyName;
            }

            //メールアドレス送信者名の必須入力チェック
            if (rt_val && this.edtSenderName.Text.Trim().Length == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "メールアドレス送信者名" });
                ctl = this.edtSenderName;
            }

            //配送指示メールタイトルの必須入力チェック
            if (rt_val && this.edtMailTitle.Text.Trim().Length == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "配送指示メールタイトル" });
                ctl = this.edtMailTitle;
            }

            //送信元メールアドレスの必須入力チェック
            if (rt_val && this.edtEmail.Text.Trim().Length == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "送信元メールアドレス" });
                ctl = this.edtEmail;
            }

            //メール本文の必須入力チェック
            if (rt_val && this.edtMailBody.Text.Trim().Length == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "メール本文" });
                ctl = this.edtMailBody;
            }

            //入力された場合のメールアドレス正規表現チェック
            if (rt_val && 0 < this.edtEmail.Text.Trim().Length && !Regex.IsMatch(this.edtEmail.Text, DefaultProperty.EMAIL_REGEX))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203015", new string[] { "メールアドレス" });
                ctl = this.edtEmail;
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabSystemProperty.SelectedIndex = (int)tbp;
                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// 「設定情報」タブの入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputsSettingInfo()
        {
            bool rt_val = true;
            string msg = string.Empty;
            Control ctl = null;
            //フォームコントロールの属しているタブ
            TabPageIndex tbp = TabPageIndex.SettingInformation;

            //PDF一時フォルダパスの必須入力チェック
            if (rt_val && this.edtPDFTempFolderPath.Text.Trim().Length == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "PDF一時フォルダパス" });
                ctl = this.edtPDFTempFolderPath;
            }

            //日付切替時間（時）の必須入力チェック
            if (rt_val && this.numDateSwitchingTimeH.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "日付切替時間の時" });
                ctl = this.numDateSwitchingTimeH;
            }

            //日付切替時間（分）の必須入力チェック
            if (rt_val && this.numDateSwitchingTimeM.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "日付切替時間の分" });
                ctl = this.numDateSwitchingTimeM;
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabSystemProperty.SelectedIndex = (int)tbp;
                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// 荷サイズ名称の重複チェックを行います。
        /// (true：重複あり)
        /// </summary>
        /// <returns>重複チェック結果（true：重複あり、false：重複なし）</returns>
        private bool CheckDoubleNiSize(Dictionary<int, string> niSizeList, string name, int idx)
        {
            bool rt_val = false;

            foreach (var item in niSizeList)
            {
                if (item.Key != idx && item.Value.Equals(name))
                {
                    rt_val = true;
                }
            }

            return rt_val;
        }

        /// <summary>
        /// 画面から必要な値を画面コントロールにセットします。
        /// </summary>
        private void GetScreen()
        {
            #region 基本情報タブ

            //基本情報タブ
            _SystemPropertyInfo.CompanyName = this.edtCompanyName.Text;
            _SystemPropertyInfo.SenderName = this.edtSenderName.Text;
            _SystemPropertyInfo.MailTitle = this.edtMailTitle.Text;
            _SystemPropertyInfo.Email = this.edtEmail.Text;
            _SystemPropertyInfo.MailBody = this.edtMailBody.Text;

            //設定情報タブ
            _SystemPropertyInfo.SendGridApiKey = this.edtSendGridApiKey.Text;
            _SystemPropertyInfo.PDFTempFolderPath = this.edtPDFTempFolderPath.Text;

            var time = this.numDateSwitchingTimeH.Value.Value.ToString("00")
                + this.numDateSwitchingTimeM.Value.Value.ToString("00");

            _SystemPropertyInfo.DateSwitchingTime = Convert.ToInt32(time);

            #endregion
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
                default:
                    break;
            }
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

        private void SystemPropertyFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォーム終了時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }
        private void tabSystemProperty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.ActiveControl.GetType().Equals(typeof(TabControl)))
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void SystemPropertyFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }
    }
}
