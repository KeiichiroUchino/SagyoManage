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
using jp.co.jpsys.util;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.Frame;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class ChangeMasterCodeFrame : Form, IFrameBase
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
        /// 引数「変更前コード」の保持領域
        /// </summary>
        private int _OldCode =0;

        /// <summary>
        /// 引数「対象テーブルのコード桁数」の保持領域
        /// </summary>
        private int _CodeDigitCount = 0;

        #endregion

        /// <summary>
        /// 「変更後コード」の保持領域
        /// </summary>
        private int _NewCode = 0;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        #endregion

        #region コンストランクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public ChangeMasterCodeFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 引数を受け取るコンストラクタです。
        /// </summary>
        /// <param name="mastertype">マスタ種類</param>
        /// <param name="oldcode">旧コード</param>
        /// <param name="codedigitcount">コード桁数</param>
        public ChangeMasterCodeFrame(DefaultProperty.MasterCodeKbn mastertype, int oldcode, int codedigitcount)
        {
            InitializeComponent();

            //ローカル変数に保持
            this._MasterKbn = mastertype;
            this._OldCode = oldcode;
            this._CodeDigitCount = codedigitcount;
        }


        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitChangeMasterCodeFrame()
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

            this.numNewCode.Focus();
        }

        /// <summary>
        /// 本画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.numOldCode.Value = 0;
            this.numNewCode.Value =0;

            //最大値を計算し、設定
            this.numOldCode.Fields.IntegerPart.MaxDigits = this._CodeDigitCount;
            this.numNewCode.Fields.IntegerPart.MaxDigits = this._CodeDigitCount;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 画面に値をセットします。
        /// </summary>
        private void SetScreen()
        {
            this.numOldCode.Value= this._OldCode;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose(DialogResult result)
        {
            this.DialogResult = result;
        }

        /// <summary>
        /// 入力されたコードに変更します。
        /// </summary>
        private void DoChangeCode()
        {
            if (this.ValidateChildren() && this.CheckInputs())
            {
                try　
                {
                    //画面コントローラにコード変更を指示
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
                    this.numNewCode.Focus();
                }
                catch (Model.DALExceptions.CanRetryException ex)
                {
                    //他から更新されている場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// メンバに設定されたコードに変更します。
        /// (名称マスタ以外)
        /// </summary>
        /// <param name="masterKbn">対象テーブル(列挙値)</param>
        private void ChangeCode()
        {
            //「変更後コード」の保持
            this._NewCode = Convert.ToInt32(this.numNewCode.Value);

            switch (this._MasterKbn)
            {
                case DefaultProperty.MasterCodeKbn.None:
                    break;
                case DefaultProperty.MasterCodeKbn.PointGroup:
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._DalUtil.PointGroup.ChangeCode(
                            tx
                            , this._DalUtil.PointGroup.GetInfo(_OldCode)
                            , this._NewCode
                            );
                    });
                    break;
                case DefaultProperty.MasterCodeKbn.TokuisakiGroup:
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._DalUtil.TokuisakiGroup.ChangeCode(
                            tx
                            , this._DalUtil.TokuisakiGroup.GetInfo(_OldCode)
                            , this._NewCode
                            );
                    });
                    break;
                case DefaultProperty.MasterCodeKbn.Homen:
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._DalUtil.Homen.ChangeCode(
                            tx
                            , this._DalUtil.Homen.GetInfo(_OldCode)
                            , this._NewCode
                            );
                    });
                    break;
                case DefaultProperty.MasterCodeKbn.Hanro:
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._DalUtil.Hanro.ChangeCode(
                            tx
                            , this._DalUtil.Hanro.GetInfo(_OldCode)
                            , this._NewCode
                            );
                    });
                    break;
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


            if (this._OldCode.ToString().Trim().Length != 0 && this._NewCode.ToString().Trim().Length != 0)
            {
                //操作ログ(保存)の条件取得
                rt_val =
                    FrameUtilites.GetDefineLogMessage(
                    "C10003", new string[] { mastName, this._OldCode.ToString(), this._NewCode.ToString()});
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
        /// ログメッセージの作成
        /// </summary>
        /// <param name="logmessagekey"></param>
        /// <param name="mastertype"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateLogJyoken(string logmessagekey, DefaultProperty.MasterCodeKbn mastertype, int code)
        {
            string rt_val = string.Empty;

            rt_val = FrameUtilites.GetDefineLogMessage(
                        logmessagekey, new string[] { 
                            DefaultProperty.GetMasterTypeNameString(mastertype),
                            code.ToString(),
                            });
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
            if (rt_val && Convert.ToInt32(this.numNewCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "新コード" });
                ctl = this.numNewCode;
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            return rt_val;
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitChangeMasterCodeFrame();
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

        private void ChangeMasterCodeFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
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
