using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using System.Globalization;
using jp.co.jpsys.util;
using Jpsys.HaishaManageV10.ComLib;
using System.Runtime.InteropServices;
using Jpsys.HaishaManageV10.ReportDAL;
using Jpsys.HaishaManageV10.ReportModel;
using System.Threading.Tasks;
using Jpsys.HaishaManageV10.ReportFrame;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using AdvanceSoftware.VBReport8;
using Jpsys.HaishaManageV10.VBReport;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishaExclusiveManageDeleteFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル初期値
        /// </summary>
        private const string WINDOW_TITLE = "配車排他管理情報削除";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 配車排他管理クラス
        /// </summary>
        private HaishaExclusiveManage _HaishaExclusiveManage;

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        /// <summary>
        /// 管理情報クラス
        /// </summary>
        private KanriInfo _KanriInfo;

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

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public HaishaExclusiveManageDeleteFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitSeikyuDataSakuseiFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            // 画面配色の初期化
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

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // 配車排他管理クラスインスタンス作成
            this._HaishaExclusiveManage = new HaishaExclusiveManage(this.appAuth);

            // 管理情報取得
            this._KanriInfo = this._DalUtil.Kanri.GetInfo();

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.edtOperatorCode, this.ShowCmnSearchOperator},
            };

            // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // 入力項目のクリア
            this.ClearInputs();

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
            // 操作メニュー
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            this.actionMenuItems = new AddActionMenuItem();

            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Delete);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);
                 
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***削除
            commandSet.Delete.Execute += Delete_Execute;
            commandSet.Bind(
                commandSet.Delete, this.btnDelete, actionMenuItems.GetMenuItemBy(ActionMenuItems.Delete), this.toolStripRemove);

            //***終了
            commandSet.Close.Execute += Close_Execute;
            commandSet.Bind(
                commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        void Delete_Execute(object sender, EventArgs e)
        {
            this.DoDelData();
        }

        void Close_Execute(object sender, EventArgs e)
        {
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
                ControlValidatingEventRaiser.Create(this.edtOperatorCode, ctl => ctl.Text, this.edtOperatorCode_Validating));
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.edtOperatorCode.Tag = null;
            this.edtOperatorCode.Text = string.Empty;
            this.edtOperatorName.Text = string.Empty;
            this.dteLastEntryDateTime.Value = null;

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
            this.edtOperatorCode.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
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

            //利用者の必須チェック
            if (rt_val && String.IsNullOrWhiteSpace(this.edtOperatorCode.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "利用者" });
                ctl = this.edtOperatorCode;
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
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// 画面情報からデータを削除します。
        /// </summary>
        private void DoDelData()
        {
            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
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
                                    this._HaishaExclusiveManage.Delete(tx, new HaishaExclusiveManageInfo()
                                    {
                                        OperatorId = Convert.ToDecimal(this.edtOperatorCode.Tag)
                                    }
                                        );
                                });

                                //操作ログ(削除)の条件取得
                                string log_jyoken =
                                    FrameUtilites.GetDefineLogMessage(
                                    "C10002", new string[] { "配車排他管理情報削除", this.edtOperatorCode.Text });

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

                                //画面を閉じる
                                this.DoClose();
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
            if (SearchFunctions.ContainsKey(ActiveControl))
            {
                SearchFunctions[ActiveControl]();
            }
        }

        /// <summary>
        /// 利用者検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchOperator()
        {
            using (CmnSearchOperatorFrame f = new CmnSearchOperatorFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcTextBox)this.ActiveControl).Text =
                        f.SelectedInfo.OperatorCode;

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 利用者コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateOperatorCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (String.IsNullOrWhiteSpace(this.edtOperatorCode.Text))
                {
                    is_clear = true;
                    return;
                }

                OperatorInfo info =
                    this._DalUtil.Operator.GetInfo(this.edtOperatorCode.Text.Trim());

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
                    this.edtOperatorCode.Tag = info.OperatorId;
                    this.edtOperatorCode.Text = info.OperatorCode;
                    this.edtOperatorName.Text = info.OperatorName;
                    this.dteLastEntryDateTime.Value = info.LastHaishaEntryDateTime == DateTime.MinValue ? (DateTime?)null
                        : info.LastHaishaEntryDateTime;
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.edtOperatorCode.Tag = null;
                    this.edtOperatorCode.Text = null;
                    this.edtOperatorName.Text = string.Empty;
                    this.dteLastEntryDateTime.Value = null;
                }
            }
        }

        #endregion

        #region プロパティ

        #endregion

        #region プライベートクラス

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitSeikyuDataSakuseiFrame();
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

        private void HaishaExclusiveManageDeleteFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void HaishaExclusiveManageDeleteFrame_KeyDown(object sender, KeyEventArgs e)
        {
            // 画面 KeyDown
            this.ProcessKeyEvent(e);
        }

        private void edtOperatorCode_Validating(object sender, CancelEventArgs e)
        {
            //利用者
            this.ValidateOperatorCode(e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }
    }
}
