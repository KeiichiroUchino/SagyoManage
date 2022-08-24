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
    public partial class SagyoBashoTorokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoBashoTorokuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 受注№を指定して、本クラスの初期化を行います。
        /// </summary>
        /// <param name="JuchuSlipNo"></param>
        /// <param name="JuchuId"></param>
        public SagyoBashoTorokuFrame(decimal id)
        {
            InitializeComponent();

            //メンバにセット
            this.SelectDtlId = id;
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検索可能状態

        private SearchStateBinder _searchStateBinder;

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;


        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "作業場所登録";

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
        /// 作業場所クラス
        /// </summary>
        private SagyoBasho _SagyoBasho;

        /// <summary>
        /// 作業場所パラメータ
        /// </summary>
        private SagyoBashoInfo _SagyoBashoInfo;

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
        public SagyoBashoViewFrame SagyoBashoIchiran { get; set; }

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
            this.SettingSearchStateBinder();
            this.SettingCommands();
            this.SetupValidatingEventRaiser();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.edtTokuisakiCd, this.ShowCmnSearchTokuisaki},
            };

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //作業場所クラスインスタンス作成
            this._SagyoBasho = new SagyoBasho(this.appAuth);

            //入力項目のクリア
            this.ClearInputs();

            // モード設定（画面間パラメータの設定有無で判定）
            if (decimal.Zero < this.SelectDtlId)
            {
                //データ表示
                this.DoGetData();
                if (this._SagyoBashoInfo.ShelterId == decimal.Zero)
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
                this._SagyoBashoInfo = new SagyoBashoInfo();
            }
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SettingSearchStateBinder()
        {
            this._searchStateBinder = new SearchStateBinder(this);
            this._searchStateBinder.AddSearchableControls(
                this.edtTokuisakiCd);
            this._searchStateBinder.AddStateObject(this.toolStripSearch);
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
            this.edtSagyoBashoCode.Text = string.Empty;
            this.edtSagyoBashoName.Text = string.Empty;
            this.edtTokuisakiCd.Tag = null;
            this.edtTokuisakiCd.Text = string.Empty;
            this.edtTokuisakiNm.Text = string.Empty;

            this.edtAddress1.Text = string.Empty;
            this.edtAddress2.Text = string.Empty;

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

            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtTokuisakiCd, ctl => ctl.Text, this.edtTokuisakiCd_Validating));
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

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
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
                            this._SagyoBasho.Save(tx, this._SagyoBashoInfo);
                        });

                        //操作ログ(保存)の条件取得
                        string log_jyoken = FrameUtilites.GetDefineLogMessage(
                            "C10002", new string[] { "作業場所登録", this.edtSagyoBashoCode.Text });

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
                                if (this.SagyoBashoIchiran != null)
                            {
                                //呼び出し元のPGの再表示を指示
                                this.SagyoBashoIchiran.ReSetScreen(this.SelectDtlId);
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
                            this.edtSagyoBashoCode.Focus();
                        }
                        else
                        {
                            //フォーカスを移動
                            this.edtSagyoBashoName.Focus();
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
                        this.edtSagyoBashoCode.Focus();
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
                        this._SagyoBasho.Delete(tx, this._SagyoBashoInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken = FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "作業場所削除", this.edtSagyoBashoCode.Text });

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

                    if (this.SagyoBashoIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.SagyoBashoIchiran.ReSetScreen(this.SelectDtlId);
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
                        this.edtSagyoBashoCode.Focus();
                    }
                    else
                    {
                        //フォーカスを移動
                        this.edtSagyoBashoName.Focus();
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
                    this.edtSagyoBashoCode.Focus();
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
            this._SagyoBashoInfo.SagyoBashoCode = this.edtSagyoBashoCode.Text;
            this._SagyoBashoInfo.SagyoBashoName = this.edtSagyoBashoName.Text;
            this._SagyoBashoInfo.TokuisakiId =Convert.ToDecimal(this.edtTokuisakiCd.Tag);
            this._SagyoBashoInfo.DisableFlag = this.chkDisableFlag.Checked;

            this._SagyoBashoInfo.SagyoBashoAddress1 = this.edtAddress1.Text;
            this._SagyoBashoInfo.SagyoBashoAddress2 = this.edtAddress2.Text;

            if (FrameEditMode.New == this.currentMode)
            {
                this._SagyoBashoInfo.ShelterId = decimal.Zero;
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
            if (rt_val && string.IsNullOrWhiteSpace(this.edtSagyoBashoCode.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "コード" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtSagyoBashoCode;
            }

            //作業場所名称の必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtSagyoBashoName.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "作業場所名称" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtSagyoBashoName;
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
                    if (this.SagyoBashoIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.SagyoBashoIchiran.ReSetScreen(this.SelectDtlId);
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
            // 作業場所情報取得
            this._SagyoBashoInfo = this._SagyoBasho.GetInfoById(this.SelectDtlId);

            // 0件なら処理しない
            if (null == this._SagyoBashoInfo)
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
                    this.edtSagyoBashoCode.ReadOnly = false;

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
                    this.edtSagyoBashoCode.ReadOnly = true;
                    this.edtSagyoBashoCode.ActiveBackColor = SystemColors.Control;

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
                    this.edtSagyoBashoCode.ReadOnly = true;
                    this.edtSagyoBashoName.ReadOnly = true;
                    this.edtTokuisakiCd.ReadOnly = true;
                    this.edtTokuisakiNm.ReadOnly = true;
                    this.edtAddress1.ReadOnly = true;
                    this.edtAddress2.ReadOnly = true;

                    this.sideButton1.Enabled = false;

                    this.edtSagyoBashoCode.ActiveBackColor = SystemColors.Control;
                    this.edtSagyoBashoName.ActiveBackColor = SystemColors.Control;
                    this.edtTokuisakiCd.ActiveBackColor = SystemColors.Control;
                    this.edtTokuisakiNm.ActiveBackColor = SystemColors.Control;
                    this.edtAddress1.ActiveBackColor = SystemColors.Control;
                    this.edtAddress2.ActiveBackColor = SystemColors.Control;

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
            this.edtSagyoBashoCode.Text = this._SagyoBashoInfo.SagyoBashoCode;
            this.edtSagyoBashoName.Text = this._SagyoBashoInfo.SagyoBashoName;
            this.edtTokuisakiCd.Text  = this._SagyoBashoInfo.TokuisakiCode;
            this.edtTokuisakiCd.Tag = this._SagyoBashoInfo.TokuisakiId;
            this.edtTokuisakiNm.Text = this._SagyoBashoInfo.TokuisakiName;
            this.chkDisableFlag.Checked = this._SagyoBashoInfo.DisableFlag;

            this.edtAddress1.Text = this._SagyoBashoInfo.SagyoBashoAddress1;
            this.edtAddress2.Text = this._SagyoBashoInfo.SagyoBashoAddress2;
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

                    if (!this.edtTokuisakiCd.ReadOnly)
                    {
                        //キーボードイベントの抑止
                        e.Handled = true;

                        this.ShowCmnSearch();
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
            if (SearchFunctions.ContainsKey(ActiveControl))
            {
                SearchFunctions[ActiveControl]();
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 得意先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcTextBox)this.ActiveControl).Text =
                        f.SelectedInfo.TokuisakiCode;

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 得意先コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void edtTokuisakiCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateTokuisakiCd(e);
        }

        /// <summary>
        /// 得意先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (string.IsNullOrWhiteSpace(this.edtTokuisakiCd.Text))
                {
                    isClear = true;
                    return;
                }

                //得意先情報を取得
                TokuisakiSearchParameter para = new TokuisakiSearchParameter();
                para.TokuisakiCode = this.edtTokuisakiCd.Text;
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
                    if (info.DisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        isClear = true;
                    }
                    else
                    {
                        this.edtTokuisakiCd.Tag = info.TokuisakiId;
                        this.edtTokuisakiNm.Text = info.TokuisakiName;
                    }
                }
            }
            finally
            {
                if (isClear)
                {
                    this.edtTokuisakiCd.Tag = null;
                    this.edtTokuisakiCd.Text = string.Empty;
                    this.edtTokuisakiNm.Text = string.Empty;
                }
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
    }
}
