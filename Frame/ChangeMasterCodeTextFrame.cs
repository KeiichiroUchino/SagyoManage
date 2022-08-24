using System;
using System.Windows.Forms;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.FrameLib.ViewSupport;
using System.ComponentModel;
using Jpsys.SagyoManage.ComLib;

namespace Jpsys.SagyoManage.Frame
{
    public partial class ChangeMasterCodeTextFrame : Form, IFrameBase
    {
        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "コード変更";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        #region 引数の保持領域

        /// <summary>
        /// 引数「マスタ種類」を表す列挙値の保持領域
        /// </summary>
        private DefaultProperty.MasterCodeKbn _MasterKbn;

        /// <summary>
        /// 引数「変更前番号」の保持領域
        /// </summary>
        private String _OldText = string.Empty;

        /// <summary>
        /// 引数「対象テーブルのコード桁数」の保持領域
        /// </summary>
        private int _CodeDigitCount = 0;

        /// <summary>
        /// 引数「親ID」の保持領域
        /// </summary>
        private Decimal _OldParentId = 0;

        #endregion

        /// <summary>
        /// 「変更後番号」の保持領域
        /// </summary>
        private String _NewText = string.Empty;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        #endregion

        #region コンストランクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public ChangeMasterCodeTextFrame()
        {
            InitializeComponent();
        }

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        /// <summary>
        /// 引数を受け取るコンストラクタです。
        /// </summary>
        /// <param name="mastertype">マスタ種類</param>
        /// <param name="oldtext">旧コード</param>
        /// <param name="codedigitcount">新コード桁数</param>
        /// <param name="parentId">親ID</param>
        public ChangeMasterCodeTextFrame(DefaultProperty.MasterCodeKbn mastertype, String oldtext, int codedigitcount, decimal parentId = 0)
        {
            InitializeComponent();

            //ローカル変数に保持
            this._MasterKbn = mastertype;
            this._OldText = oldtext;
            this._CodeDigitCount = codedigitcount;
            this._OldParentId = parentId;
        }

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitChangeMasterCodeTextFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //バインドの設定
            this.SetupValidatingEventRaiser();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //入力項目のクリア
            this.ClearInputs();

            //旧コードを設定
            this.SetScreen();

            this.edtNewText.Focus();
        }

        /// <summary>
        /// 本画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.edtOldText.Text = string.Empty;
            this.edtNewText.Text = string.Empty;

            //最大値を計算し、設定
            this.edtOldText.MaxLength = this._CodeDigitCount;
            this.edtNewText.MaxLength = this._CodeDigitCount;
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtNewText, ctl => ctl.Text, this.edtNewText_Validating));
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 画面に値をセットします。
        /// </summary>
        private void SetScreen()
        {
            this.edtOldText.Text= this._OldText;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose(DialogResult result)
        {
            this.DialogResult = result;
        }

        /// <summary>
        /// 入力されたコード番号に変更します。
        /// </summary>
        private void DoChangeCode()
        {
            if (this.ValidateChildren() && this.CheckInputs())
            {
                try
                {
                    //画面コントローラに番号変更を指示
                    this.ChangeCode();

                    //操作ログ(保存)の条件取得
                    string log_jyoken = this.GetLogJyoken();

                    //操作ログ出力
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.ChangeCode,
                        log_jyoken);

                    //変更完了メッセージ
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MI2001006"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //何もなければ画面を閉じる
                    this.DoClose(DialogResult.OK);
                }
                catch (Model.DALExceptions.UniqueConstraintException ex)
                {
                    //他から更新されている場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    this.edtNewText.Focus();
                }
                catch (Model.DALExceptions.CanRetryException ex)
                {
                    //コード番号重複時の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    this.edtNewText.Focus();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// メンバに設定されたコード番号に変更します。
        /// </summary>
        private void ChangeCode()
        {
            //「変更後コード」の保持
            this._NewText = this.edtNewText.Text;

            switch (this._MasterKbn)
            {
                case DefaultProperty.MasterCodeKbn.None:
                    break;
                //case DefaultProperty.MasterCodeKbn.ItemLBunrui:
                //    SQLHelper.ActionWithTransaction(tx =>
                //    {
                //        this._DalUtil.ItemLBunrui.ChangeCode(
                //            tx
                //            , this._DalUtil.ItemLBunrui.GetInfo(_OldText)
                //            , this._NewText
                //            );
                //    });
                //    break;
                //case DefaultProperty.MasterCodeKbn.ItemMBunrui:
                //    SQLHelper.ActionWithTransaction(tx =>
                //    {
                //        this._DalUtil.ItemMBunrui.ChangeCode(
                //            tx
                //            , this._DalUtil.ItemMBunrui.GetInfo(_OldParentId, _OldText)
                //            , this._NewText
                //            );
                //    });
                //    break;
                //case DefaultProperty.MasterCodeKbn.PointLBunrui:
                //    SQLHelper.ActionWithTransaction(tx =>
                //    {
                //        this._DalUtil.PointLBunrui.ChangeCode(
                //            tx
                //            , this._DalUtil.PointLBunrui.GetInfo(_OldText)
                //            , this._NewText
                //            );
                //    });
                //    break;
                //case DefaultProperty.MasterCodeKbn.PointMBunrui:
                //    SQLHelper.ActionWithTransaction(tx =>
                //    {
                //        this._DalUtil.PointMBunrui.ChangeCode(
                //            tx
                //            , this._DalUtil.PointMBunrui.GetInfo(_OldParentId, _OldText)
                //            , this._NewText
                //            );
                //    });
                //    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ログ出力の条件を取得します。
        /// </summary>
        /// <returns>ログ出力の条件</returns>
        private string GetLogJyoken()
        {
            string rt_val = string.Empty;

            string mastName = DefaultProperty.GetMasterTypeNameString(this._MasterKbn);

            if (this._OldText.Trim().Length != 0 && this._NewText.Trim().Length != 0)
            {
                //操作ログ(保存)の条件取得
                rt_val =
                    FrameUtilites.GetDefineLogMessage(
                    "C10003", new string[] { mastName, this._OldText, this._NewText });
            }
            else
            {
                //リトライ可能な例外
                throw new Model.DALExceptions.CanRetryException(
                   string.Format("コード変更が未実装です。"),
                   MessageBoxIcon.Warning);
            }

            return rt_val;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputs()
        {
            bool rt_val = true;
            
            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            //新コードの必須チェック
            if (rt_val)
            {
                string code = string.Empty;

                switch (this._MasterKbn)
                {
                    case DefaultProperty.MasterCodeKbn.ItemLBunrui:
                        code = this.NewText.PadLeft(2, '0');
                        break;
                    case DefaultProperty.MasterCodeKbn.ItemMBunrui:
                        code = this.NewText.PadLeft(3, '0');
                        break;
                    case DefaultProperty.MasterCodeKbn.PointLBunrui:
                        code = this.NewText.PadLeft(2, '0');
                        break;
                    case DefaultProperty.MasterCodeKbn.PointMBunrui:
                        code = this.NewText.PadLeft(3, '0');
                        break;
                    default:
                        code = this.NewText;
                    break;
                }

                if (String.IsNullOrWhiteSpace(code))
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "新コード" });
                    ctl = this.edtNewText;
                }
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            return rt_val;
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 大分類コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateNewText(CancelEventArgs e)
        {
            bool isClear = true;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.NewText.Equals(string.Empty))
                {
                    return;
                }

                switch (this._MasterKbn)
                {
                    case DefaultProperty.MasterCodeKbn.ItemLBunrui:
                        this.edtNewText.Text = this.NewText.PadLeft(2, '0');
                        break;
                    case DefaultProperty.MasterCodeKbn.ItemMBunrui:
                        this.edtNewText.Text = this.NewText.PadLeft(3, '0');
                        break;
                    case DefaultProperty.MasterCodeKbn.PointLBunrui:
                        this.edtNewText.Text = this.NewText.PadLeft(2, '0');
                        break;
                    case DefaultProperty.MasterCodeKbn.PointMBunrui:
                        this.edtNewText.Text = this.NewText.PadLeft(3, '0');
                        break;
                    default:
                        break;
                }

                isClear = false;
            }
            finally
            {
                if (isClear)
                {
                    this.edtNewText.Text = string.Empty;
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
            this.InitChangeMasterCodeTextFrame();
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

        private void ChangeMasterCodeTextFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private String NewText
        {
            get { return StringHelper.ConvertToString(this.edtNewText.Text.Trim()); }
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

        private void edtNewText_Validating(object sender, CancelEventArgs e)
        {
            //新コード
            this.ValidateNewText(e);
        }

        /// <summary>
        /// ＯＫボタンをクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DoChangeCode();
        }

        /// <summary>
        /// キャンセルボタンをクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DoClose(DialogResult.Cancel);
        }
    }
}
