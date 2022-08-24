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
using GrapeCity.Win.Editors;
using System.ComponentModel;
using jp.co.jpsys.util;
using System.Drawing;
using InputManCell = GrapeCity.Win.MultiRow.InputMan;
using Jpsys.SagyoManage.ComLib;

namespace Jpsys.SagyoManage.Frame
{
    public partial class TokuisakiTorokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public TokuisakiTorokuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 受注№を指定して、本クラスの初期化を行います。
        /// </summary>
        /// <param name="JuchuSlipNo"></param>
        /// <param name="JuchuId"></param>
        public TokuisakiTorokuFrame(decimal id)
        {
            InitializeComponent();

            //メンバにセット
            this.SelectDtlId = id;
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
        private const string WINDOW_TITLE = "得意先登録";

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
        /// 得意先クラス
        /// </summary>
        private Tokuisaki _Tokuisaki;

        /// <summary>
        /// 得意先パラメータ
        /// </summary>
        private TokuisakiInfo _TokuisakiInfo;

        /// <summary>
        /// 画面表示可能かどうかの値を保持する領域（true：表示可）
        /// </summary>
        private bool canShowFrame = true;

        /// <summary>
        /// 画面を表示する時のエラーメッセージを保持する領域
        /// </summary>
        private string showFrameMsg = string.Empty;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        /// <summary>
        /// 再表示を行うかどうかの値を保持する領域
        /// (true:再表示する)
        /// (false:再表示しない)
        /// </summary>
        private bool isReDraw = false;

        /// <summary>
        /// 一覧のインスタンス
        /// </summary>
        public TokuisakiViewFrame TokuisakiIchiran { get; set; }

        /// <summary>
        /// 一覧にて選択されたID
        /// </summary>
        public decimal SelectDtlId { get; set; } = decimal.Zero;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitJuchuNyuryokuFrame()
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

            //得意先クラスインスタンス作成
            this._Tokuisaki = new Tokuisaki(this.appAuth);

            //入力項目のクリア
            this.ClearInputs();

            // モード設定（画面間パラメータの設定有無で判定）
            if (decimal.Zero < this.SelectDtlId)
            {
                //データ表示
                this.DoGetData();
                if (this._TokuisakiInfo.ShelterId == decimal.Zero)
                {
                    //現在の画面モードを修正状態に変更
                    this.ChangeMode(FrameEditMode.Editable);
                }
                else
                {
                    //現在の画面モードを参照のみに変更
                    this.ChangeMode(FrameEditMode.ViewOnly);
                }
            }
            else
            {
                //現在の画面モードを初期状態に変更
                this.ChangeMode(FrameEditMode.New);
                this._TokuisakiInfo = new TokuisakiInfo();
            }
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
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Delete);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.edtTokuisakiCode.Text = string.Empty;
            this.edtTokuisakiName.Text = string.Empty;
            this.edtAddress1.Text = string.Empty;
            this.edtAddress2.Text = string.Empty;
            this.edtTel.Text = string.Empty;
            this.edtFax.Text = string.Empty;

            this.chkDisableFlag.Checked = false;

            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
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
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);

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

        void DeleteAllDtl_Execute(object sender, EventArgs e)
        {
            this.DoDelData();
        }

        private void JuchuNyuryokuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void JuchuNyuryokuFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);

            //画面表示時のメッセージ表示
            if (this.showFrameMsg.Trim().Length != 0)
            {
                MessageBox.Show(
                    this.showFrameMsg,
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                if (!this.canShowFrame)
                {
                    //画面を閉じる（終了確認はしない）
                    this.isConfirmClose = false;
                    this.DoClose();
                }
            }
        }

        private void JuchuNyuryokuFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        void GcTextBoxEditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            // ※ GcTextBoxCell で上・下キーが押されたら強制的にセル移動させる
            if (e.KeyCode == Keys.Up)
            {
                SelectionActions.MoveToPreviousCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
            }
            else if (e.KeyCode == Keys.Down)
            {
                SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
            }
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
            this.InitJuchuNyuryokuFrame();
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

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
                {
                    //画面から値を取得
                    this.GetScreen();

                    try
                    {
                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            // 登録・更新
                            this._Tokuisaki.Save(tx, this._TokuisakiInfo);
                        });

                        //操作ログ(保存)の条件取得
                        string log_jyoken = FrameUtilites.GetDefineLogMessage(
                            "C10002", new string[] { "得意先登録", this.edtTokuisakiCode.Text });

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

                        ////初期状態へ移行
                        if (FrameEditMode.New == this.currentMode)
                        {
                            //データ表示
                            this.DoClear(false);
                        }
                        //else if (FrameEditMode.Editable == this.currentMode)
                        else
                        {
                            if (this.TokuisakiIchiran != null)
                            {
                                //呼び出し元のPGの再表示を指示
                                this.TokuisakiIchiran.ReSetScreen(this.SelectDtlId);
                            }

                            // 画面を閉じる
                            this.isConfirmClose = false;
                            this.DoClose();
                        }

                    }
                    catch (CanRetryException ex)
                    {
                        //データがない場合の例外ハンドラ
                        FrameUtilites.ShowExceptionMessage(ex, this);
                        if (FrameEditMode.New == this.currentMode)
                        {
                            //フォーカスを移動
                            this.edtTokuisakiCode.Focus();
                        }
                        else
                        {
                            //フォーカスを移動
                            this.edtTokuisakiName.Focus();
                        }
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
                        this.edtTokuisakiCode.Focus();
                    }
                }
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 画面に読み込んだデータを削除します。
        /// </summary>
        private void DoDelData()
        {
            //実行確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                FrameUtilites.GetDefineMessage("MQ2102003"),
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            if (this.ValidateChildren(ValidationConstraints.None))
            {
                //画面から値を取得
                this.GetScreen();

                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._Tokuisaki.Delete(tx, this._TokuisakiInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken = FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "得意先削除", this.edtTokuisakiCode.Text });

                    //保存完了メッセージ格納用
                    string msg = string.Empty;

                    //更新完了のメッセージ
                    msg =
                        FrameUtilites.GetDefineMessage("MI2001004");

                    //登録完了メッセージ
                    MessageBox.Show(
                        msg,
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //一覧画面に戻った際に再検索を行う。
                    this.isReDraw = true;

                    if (this.TokuisakiIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.TokuisakiIchiran.ReSetScreen(this.SelectDtlId);
                    }

                    // 画面を閉じる
                    this.isConfirmClose = false;
                    this.DoClose();

                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);

                    if (FrameEditMode.New == this.currentMode)
                    {
                        //フォーカスを移動
                        this.edtTokuisakiCode.Focus();
                    }
                    else
                    {
                        //フォーカスを移動
                        this.edtTokuisakiName.Focus();
                    }
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
                    this.edtTokuisakiCode.Focus();
                }
            }
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            if (this.isReDraw)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            this._TokuisakiInfo.TokuisakiCode = this.edtTokuisakiCode.Text;

            this._TokuisakiInfo.TokuisakiName = this.edtTokuisakiName.Text;
            this._TokuisakiInfo.TokuisakiAddress1 = this.edtAddress1.Text;
            this._TokuisakiInfo.TokuisakiAddress2 = this.edtAddress2.Text;
            this._TokuisakiInfo.TokuisakiTel = this.edtTel.Text;
            this._TokuisakiInfo.TokuisakiFax = this.edtFax.Text;

            this._TokuisakiInfo.DisableFlag = this.chkDisableFlag.Checked;

            if (FrameEditMode.New == this.currentMode)
            {
                this._TokuisakiInfo.ShelterId = decimal.Zero;
            }
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool CheckInputs()
        {

            //戻り値用
            bool rt_val = true;

            string msg = string.Empty;
            MessageBoxIcon icon = MessageBoxIcon.None;
            Control ctl = null;

            //コードの必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtTokuisakiCode.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "コード" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtTokuisakiCode;
            }

            //得意先名称の必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtTokuisakiName.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "得意先名称" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtTokuisakiName;
            }

            if (!rt_val)
            {
                //アイコンの種類によってMassageBoxTitleを変更
                string msg_title = string.Empty;

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        msg_title = "エラー";
                        break;
                    case MessageBoxIcon.Warning:
                        msg_title = "警告";
                        break;
                    default:
                        break;
                }

                MessageBox.Show(
                    msg, msg_title, MessageBoxButtons.OK, icon);

                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示

            if (this.isConfirmClose && FrameEditMode.ViewOnly != this.currentMode)
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
                    if (this.TokuisakiIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.TokuisakiIchiran.ReSetScreen(this.SelectDtlId);
                    }
                }
                else if (result == DialogResult.No)
                {
                    //Noの場合は終了をキャンセル
                    e.Cancel = true;
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

            if (FrameEditMode.New == this.currentMode)
            {
                this.ChangeMode(FrameEditMode.New);
            }
            else if (FrameEditMode.Editable == this.currentMode || FrameEditMode.ViewOnly == this.currentMode)
            {
                //受注情報を再表示
                this.DoGetData();
                this.ChangeMode(FrameEditMode.Editable);
            }
        }

        /// <summary>
        /// データを取得し、画面上にセットします。
        /// </summary>
        private void DoGetData()
        {
            // 画面コントローラに取得を指示
            // 得意先情報取得
            this._TokuisakiInfo = this._Tokuisaki.GetInfoById(this.SelectDtlId);

            // 0件なら処理しない
            if (null == this._TokuisakiInfo)
            {
                if (FrameEditMode.Editable == this.currentMode)
                {
                    this.showFrameMsg = FrameUtilites.GetDefineMessage("MW2201015");
                    //画面表示不可
                    this.canShowFrame = false;
                }
                else
                {
                    return;
                }
            }

            this.SetScreen();

        }

        /// <summary>
        /// 画面モードを表す列挙体を指定して、現在の画面の
        /// 表示モードを切り替えます。
        /// </summary>
        /// <param name="mode">変更したい画面モード</param>
        private void ChangeMode(FrameEditMode mode)
        {
            switch (mode)
            {
                case FrameEditMode.New:
                    //初期状態
                    // コントロールの使用可否
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtTokuisakiCode.ReadOnly = false;

                    // ファンクションの使用可否

                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Delete.Enabled = false;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = false;

                    break;
                case FrameEditMode.Editable:

                    //編集モード
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtTokuisakiCode.ReadOnly = true;
                    this.edtTokuisakiCode.ActiveBackColor = SystemColors.Control;

                    // ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Delete.Enabled = true;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = true;

                    break;
                case FrameEditMode.ViewOnly:

                    //参照モード
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtTokuisakiCode.ReadOnly = true;
                    this.edtTokuisakiName.ReadOnly = true;
                    this.edtAddress1.ReadOnly = true;
                    this.edtAddress2.ReadOnly = true;
                    this.edtTel.ReadOnly = true;
                    this.edtFax.ReadOnly = true;

                    this.edtTokuisakiCode.ActiveBackColor = SystemColors.Control;
                    this.edtTokuisakiName.ActiveBackColor = SystemColors.Control;
                    this.edtAddress1.ActiveBackColor = SystemColors.Control;
                    this.edtAddress2.ActiveBackColor = SystemColors.Control;
                    this.edtTel.ActiveBackColor = SystemColors.Control;
                    this.edtFax.ActiveBackColor = SystemColors.Control;

                    //this.chkDisableFlag.Enabled = false;
                    //this.btnDelete.Enabled = true;
                    //this.btnCancel.Enabled = false;
                    //this.btnSave.Enabled = false;

                    // ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Delete.Enabled = true;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = false;

                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            this.edtTokuisakiCode.Text = this._TokuisakiInfo.TokuisakiCode;
            this.edtTokuisakiName.Text = this._TokuisakiInfo.TokuisakiName;
            this.edtAddress1.Text  = this._TokuisakiInfo.TokuisakiAddress1;
            this.edtAddress2.Text = this._TokuisakiInfo.TokuisakiAddress2;
            this.edtTel.Text = this._TokuisakiInfo.TokuisakiTel;
            this.edtFax.Text = this._TokuisakiInfo.TokuisakiFax;

            this.chkDisableFlag.Checked = this._TokuisakiInfo.DisableFlag;
        }

        #region セルのフォーカス移動支援

        /// <summary>
        /// セルの移動順番がもう一つのセルの移動順番よりも大きいかどうかを取得します。
        /// ※移動順番はTab移動の順番こと。 ≠ TabIndex（行の上下位置も考慮する）
        /// </summary>
        /// <param name="multiRow">比較対象のセルのGCMultiRow</param>
        /// <param name="positionA">一つ目のセルのCellPostion</param>
        /// <param name="positionB">二つ目のセルのCellPostion</param>
        /// <returns></returns>
        private static bool CellMovingOrderIsGreater(GcMultiRow multiRow, CellPosition positionA, CellPosition positionB)
        {
            bool forward = false;

            var cellA = multiRow[positionA.RowIndex, positionA.CellIndex];
            var cellB = multiRow[positionB.RowIndex, positionB.CellIndex];

            if (cellA.RowIndex > cellB.RowIndex)
            {
                forward = true;
            }
            else
            {
                if (cellA.RowIndex == cellB.RowIndex &&
                    cellA.TabIndex > cellB.TabIndex)
                {
                    forward = true;
                }
            }

            return forward;
        }

        /// <summary>
        /// 移動順番が次のセルを探索
        /// </summary>
        /// <param name="multiRow">探索対象のGCMultiRow</param>
        /// <param name="startPosition">探索を開始するCellPosition</param>
        /// <param name="predicate">探索対象に含めるかの述語</param>
        /// <returns></returns>
        private static CellPosition FindNextMovingOrderCellAsCellPosition(
            GcMultiRow multiRow, CellPosition startPosition, Predicate<Cell> predicate)
        {
            CellPosition position = CellPosition.Empty;

            Cell startCell = multiRow[startPosition.RowIndex, startPosition.CellIndex];

            for (int i = startPosition.RowIndex; i < multiRow.RowCount; i++)
            {
                Row row = multiRow.Rows[i];

                foreach (Cell cell in row.Cells.OrderBy(cell => cell.TabIndex))
                {
                    if (cell.RowIndex == startPosition.RowIndex && cell.TabIndex <= startCell.TabIndex)
                    {
                        continue;
                    }

                    if (cell.Selectable && predicate(cell))
                    {
                        return new CellPosition(cell.RowIndex, cell.CellIndex);
                    }
                }
            }

            return position;
        }

        /// <summary>
        /// 移動順番が前のセルを探索
        /// </summary>
        /// <param name="multiRow">探索対象のGCMultiRow</param>
        /// <param name="startPosition">探索を開始するCellPosition</param>
        /// <param name="predicate">探索対象に含めるかの述語</param>
        /// <returns></returns>
        private static CellPosition FindPreviousMovingOrderCellAsCellPosition(
            GcMultiRow multiRow, CellPosition startPosition, Predicate<Cell> predicate)
        {
            CellPosition position = CellPosition.Empty;

            Cell startCell = multiRow[startPosition.RowIndex, startPosition.CellIndex];

            for (int i = startPosition.RowIndex; i > -1; i--)
            {
                Row row = multiRow.Rows[i];

                foreach (Cell cell in row.Cells.OrderByDescending(cell => cell.TabIndex))
                {

                    if (cell.RowIndex == startPosition.RowIndex && cell.TabIndex >= startCell.TabIndex)
                    {
                        continue;
                    }

                    if (cell.Selectable && predicate(cell))
                    {
                        return new CellPosition(cell.RowIndex, cell.CellIndex);
                    }
                }
            }

            return position;
        }

        #endregion


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
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 検証（Validate）処理


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
    }
}
