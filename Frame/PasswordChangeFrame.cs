using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.SQLServerDAL;

namespace Jpsys.SagyoManage.Frame
{
    public partial class PasswordChangeFrame : Form, IFrameBase
    {

        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "パスワード変更";

        //--ローカル変数
        /// <summary>
        /// 権限別メニュービジネスロジック
        /// </summary>
        private Operator bll;

        /// <summary>
        /// 認証済みアプリケーション情報保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// 現在編集中のオペレータ情報を保持する領域
        /// </summary>
        private OperatorInfo operatorInfo = null;

        #endregion
        
        #region コンストラクタ

        public PasswordChangeFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        private void InitPasswordChangeFrame()
        {

            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //親フォームがあるときは、そのフォームの中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //認証アプリケーション情報を設定
            UserProperty.GetInstance().Refresh();
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //画面コントローラのインスタンスを作成
            this.bll = new Operator(this.appAuth);

            //入力項目を初期化するメソッド
            this.ClearInputs();

            //現在編集中のオペレータ情報を取得
            this.operatorInfo = this.bll.GetInfo(UserProperty.GetInstance().LoginCode);
        }

        #endregion

        #region プライベートメソッド

        private void ClearInputs()
        {
            //社員名を
            this.edtOperatorName.Text =
                UserProperty.GetInstance().LoginOperatorName;

            //パスワードの入力画面をクリアする
            this.edtNewPassword.Text = "";
            this.edtConfirmPassword.Text = "";
            
            //パスワードの表示の設定をクリアする
            this.chkShowPassword.Checked = false;
            this.edtNewPassword.PasswordChar = '*';
            this.edtConfirmPassword.PasswordChar = '*';
        }

        /// <summary>
        /// パスワードの表示を変更した時の処理を一括して行います。
        /// </summary>
        private void ChangeShowPassword()
        {
            if (this.chkShowPassword.Checked)
            {
                this.edtNewPassword.PasswordChar = new char();
                this.edtConfirmPassword.PasswordChar = new char();
            }
            else
            {
                this.edtNewPassword.PasswordChar = '*';
                this.edtConfirmPassword.PasswordChar = '*';
            }
        }

        /// <summary>
        /// 入力中の項目に不備がないか確認します。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputs()
        {
            bool rt_val = true;
            string msg = "";
            Control ctl = null;

            if (this.edtNewPassword.Text != this.edtConfirmPassword.Text)
            {
                msg = "パスワードが一致しません。";
                ctl = this.edtConfirmPassword;
                rt_val = false;
            }

            if (rt_val &&
                this.edtNewPassword.Text == operatorInfo.Password)
            {
                msg = "現在と同じパスワードは設定できません。";
                ctl = this.edtNewPassword;
                rt_val = false;
            }


            if (!rt_val)
            {
                MessageBox.Show(msg,
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                ctl.Focus();

            }

            return rt_val;
        }


        private void ChangePassword()
        {
            if (this.ValidateChildren() && this.CheckInputs())
            {
                try
                {
                    this.operatorInfo.Password = this.edtNewPassword.Text;

                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this.bll.Save(tx,this.operatorInfo);
                    });
                    

                    //操作ログ(削除)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "利用者名", this.edtOperatorName.Text });

                    //更新時
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.UpdateItem,
                        log_jyoken);

                    MessageBox.Show("パスワードを変更しました。",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.Close();
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
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
            this.InitPasswordChangeFrame();
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

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeShowPassword();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.ChangePassword();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
