using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using System.IO;
using GrapeCity.Win.MultiRow;
using jp.co.jpsys.util;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.SQLServerDAL;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class MenuAuthSettingFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public MenuAuthSettingFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "メニュー権限設定";

        #region 権限別メニュー設定一覧の列定義

        /// <summary>
        /// 権限別メニュー設定一覧の列を表す列挙体です。
        /// </summary>
        private enum MrowCellKeyDtl : int
        {
            /// <summary>
            /// 大分類名(分類名)
            /// </summary>
            TBunruiName = 0,

            /// <summary>
            /// 中分類名(メニュー名)
            /// </summary>
            MBunruiName = 1,

            /// <summary>
            /// 使用許可フラグ
            /// </summary>
            AllowUseFlag = 2
        }

        #endregion

        #region 権限一覧(Spread)の列定義

        //--権限一覧(Spread)の列定義
        /// <summary>
        /// 権限名列番号
        /// </summary>
        private const int COL_AUTHNAME = 0;
        /// <summary>
        /// 権限一覧の最大列数
        /// </summary>
        private const int COL_MAXCOLUM_AUTHLIST = 1;

        /// <summary>
        /// 権限名称テーブルリストを保持する領域
        /// </summary>
        private IList<KengenNameInfo> _KengenNameInfoList = null;

        #endregion

        #region 対象ユーザ一覧(Spread)の列定義

        //--対象ユーザ一覧(Spread)の列定義
        /// <summary>
        /// 対象ユーザ名列番号
        /// </summary>
        private const int COL_OPERATORNAME = 0;
        /// <summary>
        /// 対象ユーザ一覧の最大列数
        /// </summary>
        private const int COL_MAXCOLUM_OPERATORHLIST = 1;

        #endregion

        //--ローカル変数
        /// <summary>
        /// 権限別メニュービジネスロジック
        /// </summary>
        private MenuAuthSetting bll;

        /// <summary>
        /// データアクセスオブジェクトを利用するためのクラス
        /// </summary>
        private CommonDALUtil dalUtil;

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
        /// 現在編集中の権限別メニュー情報を保持する領域
        /// </summary>
        private IList<MenuAuthSettingInfo> menuAuthSettingList = null;

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion
        
        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitMenuAuthSettingFrame()
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

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //ビジネスロジックのインスタンスを作成
            bll = new MenuAuthSetting(this.appAuth);

            //データアクセスオブジェクトを利用するためのクラスのインスタンス作成
            this.dalUtil = new CommonDALUtil(this.appAuth);

            //入力項目の初期化
            this.ClearInputs();

            //Mrow関連の初期化
            this.InitMrow();

            //スプレッドを初期化するメソッド
            this.InitSheet();

            //権限一覧(Spread)を画面に表示するメソッド
            this.SetAuthListSheet();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);
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

            // 画面内のメニューに操作メニューを追加
            this.actionMenuItems.AddMenu(this.mnuStripTop);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
        }

        /// <summary>
        /// Mrow関連を初期化します。
        /// </summary>
        private void InitMrow()
        {
            this.InitMenuAuthorizeListMrow(true);
        }

        /// <summary>
        /// メニュー使用不可情報一覧(Mrow)を初期化します。
        /// </summary>
        /// <param name="allowSetTemplete"></param>
        private void InitMenuAuthorizeListMrow(bool allowSetTemplete)
        {

            //描画を停止
            this.mrsMenuAuthorizeList.SuspendLayout();

            try
            {
                //Mrowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsMenuAuthorizeList.Template;
                tpl.Row.DefaultCellStyle = dynamicellstyle;

                //テンプレートを再設定
                this.mrsMenuAuthorizeList.Template = tpl;

                //F2にセルの編集のショートカット割り当てるように変更
                //---規定値でF2は編集モードになっています T.Kuroki@NSK

                //--Escのショートカットキーを無効に設定
                this.mrsMenuAuthorizeList.ShortcutKeyManager.Unregister(Keys.Escape);

                //--セルの編集の制御を追加
                //---Enter制御を追加
                this.mrsMenuAuthorizeList.ShortcutKeyManager.Unregister(Keys.Enter);
                this.mrsMenuAuthorizeList.ShortcutKeyManager.Register(
                                        SelectionActions.MoveToNextCell, Keys.Enter);

                //---F12制御を追加
                this.mrsMenuAuthorizeList.ShortcutKeyManager.Register(
                                        SelectionActions.MoveToPreviousCell, Keys.F12);

                //--DeleteキーのClearアクションを解除（MultiRowのKeyDownイベントにて処理）
                this.mrsMenuAuthorizeList.ShortcutKeyManager.Unregister(Keys.Delete);

                //--EscapeキーのClearアクションを解除（MultiRowのKeyDownイベントにて処理）
                this.mrsMenuAuthorizeList.ShortcutKeyManager.Unregister(Keys.Escape);

                //--単一セル選択モード
                this.mrsMenuAuthorizeList.MultiSelect = false;

                //--ヘッダの列幅変更を禁止する
                //---列ヘッダと行ヘッダ個別で列幅変更の可否は調整できません。 T.Kuroki@NSK
                this.mrsMenuAuthorizeList.AllowUserToResize = false;

                //--ユーザでの行追加を不許可
                this.mrsMenuAuthorizeList.AllowUserToAddRows = false;
                //--ユーザでの行削除は不許可
                this.mrsMenuAuthorizeList.AllowUserToDeleteRows = false;

                //*** s.arimrua 2010/05/12 追加
                //フォーカスが無い時はセルのハイライトを表示しない
                this.mrsMenuAuthorizeList.HideSelection = true;

                //--分割ボタンを表示しない
                this.mrsMenuAuthorizeList.SplitMode = SplitMode.None;

                //行をクリア
                //---MultiRow.Clear() メソッドは未対応
                this.mrsMenuAuthorizeList.Rows.Clear();

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                this.mrsMenuAuthorizeList.ResumeLayout();
            }
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
                case FrameEditMode.Default:
                    //初期状態
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = true;
                    this.pnlCenter.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.btnUpdate.Enabled = false;
                    //--ファンクションの使用可否
                    this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Update, false);
                    this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.EditCancel, false);

                    break;
                case FrameEditMode.New:
                    //新規作成モード
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = false;
                    this.pnlCenter.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.btnUpdate.Enabled = true;
                    //--ファンクションの使用可否
                    this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.EditCancel, true);
                    this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Update, true);

                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = false;
                    this.pnlCenter.Enabled = true;
                    this.btnCancel.Enabled = true;
                    this.btnUpdate.Enabled = true;
                    //--ファンクションの使用可否
                    this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.EditCancel, true);
                    this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Update, true);

                    break;
                case FrameEditMode.ViewOnly:
                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        #region スプレッド関係の初期化

        /// <summary>
        /// スプレッドを初期化するメソッド
        /// </summary>
        private void InitSheet()
        {
            this.InitAuthListSheet();
            this.InitOperatorListSheet();
        }

        /// <summary>
        /// 権限一覧の初期化を行います
        /// </summary>
        private void InitAuthListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpAuthListGrid.Sheets[0];

            //桁数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLUM_AUTHLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 対象ユーザ一覧の初期化を行います
        /// </summary>
        private void InitOperatorListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpOperatorListGrid.Sheets[0];

            //桁数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLUM_OPERATORHLIST);
            sheet0.Models.Data = dataModel;
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
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close));
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            if (this.ConfirmClear())
            {
                this.DoClear();
            }
        }

        void Save_Execute(object sender, EventArgs e)
        {
            if (this.ConfirmUpdate())
            {
                this.DoUpdate();
            }
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 画面の情報からデータを取得します。
        /// </summary>
        private void DoGetData()
        {
            //検索セクションの入力チェック
            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputsSearch())
            {
                //対象ユーザ一覧(Spread)を画面に表示するメソッド
                this.SetOperatorListSheet();

                //権限設定一覧(Mrow)へデータセット
                this.SetScreen();

                //明細が無い場合メッセージを表示して抜ける。
                if (this.menuAuthSettingList.Count == 0)
                {
                    MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201015"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                    this.fpAuthListGrid.Focus();

                    return;
                }

                //画面モードを変更
                this.ChangeMode(FrameEditMode.Editable);

                //権限設定一覧にフォーカス
                this.mrsMenuAuthorizeList.Focus();

            }
        }

        /// <summary>
        /// 画面に画面コントローラーの値をセットします。
        /// </summary>
        private void SetScreen()
        {
            //明細の情報をセット
            this.SetScreenDtlMrow();
        }

        /// <summary>
        /// 画面コントローラーからメニュー使用不可一覧(Mrow)の情報読み込み
        /// Mrowにセットします。
        /// </summary>
        private void SetScreenDtlMrow()
        {
            //Mrowをクリア
            this.mrsMenuAuthorizeList.Rows.Clear();

            //MultiRowの描画を停止
            this.mrsMenuAuthorizeList.SuspendLayout();

            //計算を停止
            //this.mrsMenuAuthorizeList.AutoCalculate = false;

            try
            {
                //権限一覧(Spread)にて選択中の権限IDを取得して設定
                int _AuthID = this.GetKengenByAuthListOnSelection();

                //データ取得
                this.menuAuthSettingList =
                    bll.GetMenuAuthSettingList((decimal)_AuthID);

                //件数取得
                int rowcount = this.menuAuthSettingList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //行数を入れる
                this.mrsMenuAuthorizeList.RowCount = rowcount;

                //ループしてセット
                for (int i = 0; i < rowcount; i++)
                {
                    //１件分を取得
                    MenuAuthSettingInfo info =
                        this.menuAuthSettingList[i];

                    //使用許可フラグでチェックを切り替える
                    if (info.AllowUseFlag)
                    {
                        this.mrsMenuAuthorizeList.SetValue(
                            i, (int)MrowCellKeyDtl.AllowUseFlag, true);
                    }
                    else
                    {
                        this.mrsMenuAuthorizeList.SetValue(
                            i, (int)MrowCellKeyDtl.AllowUseFlag, false);
                    }

                    //他の値をセット
                    this.mrsMenuAuthorizeList.SetValue(
                        i, (int)MrowCellKeyDtl.TBunruiName, info.TBunruiName);
                    this.mrsMenuAuthorizeList.SetValue(
                        i, (int)MrowCellKeyDtl.MBunruiName, info.MBunruiName);

                    //1件分のMrowのTagにinfoをセット
                    this.mrsMenuAuthorizeList.Rows[i].Tag = info;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsMenuAuthorizeList.ResumeLayout();
            }
        }


        /// <summary>
        /// 検索セクションの入力項目をチェックします。
        /// (true:正常に入力されている。)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputsSearch()
        {
            bool rt_val = true;
            string msg = string.Empty;
            Control ctl = null;

            ////権限の必須入力
            //if (rt_val && (decimal)this.numSearchAuthorityId.Value <= 0)
            //{
            //    rt_val = false;
            //    msg = FrameUtilites.GetDefineMessage(
            //        "MW2203004", new string[] { "権限" });
            //    ctl = this.numSearchAuthorityId;
            //}

            if (!rt_val)
            {
                MessageBox.Show(
                    msg, "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// 保存の確認をします。
        /// （true:メッセージの選択「はい」）
        /// </summary>
        /// <returns>メッセージの選択結果（true:はい）</returns>
        private bool ConfirmUpdate()
        {
            bool rt_val = true;

            //保存確認メッセージ
            DialogResult wk_result =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2101010"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            //Noの場合、戻り値にfalseをセット
            if (wk_result == DialogResult.No)
            {
                rt_val = false;
            }

            return rt_val;
        }

        /// <summary>
        /// 画面情報からデータを更新します。
        /// </summary>
        private void DoUpdate()
        {
            this.mrsMenuAuthorizeList.EndEdit();

            //Mrowの確定
            //EditingActions.EndEdit.Execute(this.mrsMenuAuthorizeList);

            //入力チェックと更新処理
            if (this.ValidateChildren(ValidationConstraints.None))
            {
                //画面からコントローラーに値をセット
                this.GetScreenDtlMrow();

                //更新対象がなければエラー
                if (this.menuAuthSettingList.Count == 0)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("ME2303027"),
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                    return;
                }

                //権限一覧(Spread)にて選択中の権限IDを取得して設定
                int _AuthID = this.GetKengenByAuthListOnSelection();

                bll.SetMenuAuthSettingList(this.menuAuthSettingList, (decimal)_AuthID);

                //ログメッセージを取得
                string msg = string.Format("権限：{0}",
                    _AuthID.ToString());

                //操作ログ出力
                FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                    FrameLogWriter.LoggingOperateKind.ProcessTask,
                    msg);

                //保存完了メッセージ
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MI2001012"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DoClear();
            }
        }

        /// <summary>
        /// メニュー使用不可情報一覧(Mrow)から必要な項目を画面コントロールに
        /// セットします。
        /// </summary>
        private void GetScreenDtlMrow()
        {
            //画面コントローラーへセットするためのリスト
            List<MenuAuthSettingInfo> set_list =
                new List<MenuAuthSettingInfo>();

            //計算を停止
            //this.mrsMenuAuthorizeList.AutoCalculate = false;
            //MultiRowの描画を停止
            this.mrsMenuAuthorizeList.SuspendLayout();

            try
            {
                //行数を取得
                int rowcount = this.mrsMenuAuthorizeList.RowCount;

                //権限一覧(Spread)にて選択中の行Indexを取得して設定
                int _authId = this.GetKengenByAuthListOnSelection();

                //ループして明細のリストを作る
                for (int i = 0; i < rowcount; i++)
                {
                    //明細1件分のinfoを作る
                    MenuAuthSettingInfo info =
                        new MenuAuthSettingInfo();

                    //1行分のtagがnullではなかったら、読み込み行なのでTagから
                    //明細１件分を復元する
                    if (this.mrsMenuAuthorizeList.Rows[i].Tag != null)
                    {
                        info = (MenuAuthSettingInfo)this.mrsMenuAuthorizeList.Rows[i].Tag;
                    }

                    //他の値を取得
                    info.AuthId = _authId;
                    info.AllowUseFlag =
                        Convert.ToBoolean(
                            this.mrsMenuAuthorizeList.GetValue(i, (int)MrowCellKeyDtl.AllowUseFlag));

                    //画面コントローラーセット用のリストにセット
                    set_list.Add(info);

                    //ログ出力
                    //--処理区分によってログの出力を分ける
                    string log_msg = string.Empty;
                }

                //画面コントローラーへセット
                this.menuAuthSettingList = set_list;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsMenuAuthorizeList.ResumeLayout();
            }
        }

        /// <summary>
        /// 編集の取消しの確認をします。
        /// （true:メッセージの選択「はい」）
        /// </summary>
        /// <returns>メッセージの選択結果（true:はい）</returns>
        private bool ConfirmClear()
        {
            bool rt_val = true;

            //取消確認メッセージ
            DialogResult wk_result =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2102008"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            //Noの場合、戻り値にfalseをセット
            if (wk_result == DialogResult.No)
            {
                rt_val = false;
            }

            return rt_val;
        }

        /// <summary>
        /// 画面情報をクリアして初期状態に変更します。
        /// </summary>
        private void DoClear()
        {
            //Mrowをクリア
            this.InitMenuAuthorizeListMrow(false);
            ////入力項目をクリア
            //this.ClearInputs();
            //対象ユーザ一覧(Spread)の初期化
            this.InitOperatorListSheet();
            //画面モード切替
            this.ChangeMode(FrameEditMode.Default);
            //画面コントローラをクリア
            this.menuAuthSettingList.Clear();
            //初期フォーカスのセット
            this.fpAuthListGrid.Focus();
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
            if (this.currentMode == FrameEditMode.Editable ||
                    this.currentMode == FrameEditMode.New)
            {
                //編集または新規の場合はメッセージを表示    
                DialogResult result =
                MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MQ2102001"),
                        this.Text,
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

        #region スプレッド関係

        /// <summary>
        /// 画面の権限一覧(Spread)にデータを設定します。
        /// </summary>
        private void SetAuthListSheet()
        {
            //DefaultPropertyに設定されている権限のMAXを取得します
            int idx = (int)BizProperty.DefaultProperty.AUTH_MAX;

            //Spreadのデーターモデルを作る
            DefaultSheetDataModel datamodel =
                new DefaultSheetDataModel(idx, COL_MAXCOLUM_AUTHLIST);

            this._KengenNameInfoList =
                dalUtil.KengenName.GetList();

            int i = 0;
            foreach (var item in this._KengenNameInfoList)
            {
                //DataModelにデータをセットします
                datamodel.SetValue(i, COL_AUTHNAME, item.KengenName);
                datamodel.SetTag(i, (i + 1), item.KengenName);

                i += 1;
            }

            //DataModelを権限一覧(Spread)に設定します
            this.fpAuthListGrid.Sheets[0].Models.Data = datamodel;
        }

        /// <summary>
        /// 画面の対象ユーザ一覧(Spread)にデータを設定します。
        /// </summary>
        private void SetOperatorListSheet()
        {

            //権限一覧(Spread)にて選択中の権限IDを取得して設定
            decimal _Kengen = (decimal)this.GetKengenByAuthListOnSelection();

            //指定された権限のオペレータリストを取得します
            IList<OperatorInfo> _operatorList =
                this.dalUtil.Operator.GetList(new OperatorSearchParameter() { }).Where(x => x.AuthorityId == _Kengen).ToList();

            //オペレータリストの件数を取得
            int idx = _operatorList.Count;

            //Spreadのデーターモデルを作る
            DefaultSheetDataModel datamodel =
                new DefaultSheetDataModel(idx, COL_MAXCOLUM_OPERATORHLIST);

            //データモデルにセット
            for (int i = 0; i < idx; i++)
            {
                //オペレータリストを１件分を取得しておく
                OperatorInfo wk_info = _operatorList[i];

                //DataModelにセットする
                datamodel.SetValue(i, COL_OPERATORNAME, wk_info.OperatorName);
                datamodel.SetTag(i, COL_OPERATORNAME, wk_info);
            }
            //DataModelを対象のユーザ一覧(Spread)に設定します
            this.fpOperatorListGrid.Sheets[0].Models.Data = datamodel;

        }

        /// <summary>
        /// 権限一覧(Spread)にて選択中の権限IDを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の権限一覧の権限ID</returns>
        private int GetKengenByAuthListOnSelection()
        {
            //返却行
            int rt_val = 0;

            SheetView sheet0 = fpAuthListGrid.Sheets[0];
            if (sheet0.SelectionCount > 0)
            {
                //選択行の権限IDを取得
                rt_val = sheet0.GetSelection(0).Row + 1;
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// 権限一覧(Spread)上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpAuthListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:

                    //対象ユーザ一覧(Spread)を画面に表示するメソッド
                    this.SetAuthListSheet();

                    //メニュー定義情報を表示
                    this.DoGetData();

                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitMenuAuthSettingFrame();
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
        /// フォームが閉じられる前のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAuthSettingFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォーム終了時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        /// <summary>
        /// 権限一覧(Spread)でクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpAuthListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpAuthListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }
        /// <summary>
        /// 権限一覧(Spread)でダブルクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpAuthListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !e.ColumnHeader)
            {
                this.DoGetData();
            }
        }
        /// <summary>
        /// 権限一覧(Spread)でキーダウン(Enter)押したときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpAuthListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //権限一覧(Spread) - PreviewKeyDown
            this.ProcessFpAuthListGridPreviewKeyDown(e);
        }
    }
}
