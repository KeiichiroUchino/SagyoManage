using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.Model;
using jp.co.jpsys.util;

using System.Runtime.InteropServices ;
using System.Threading.Tasks;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.BizProperty;  // for DllImport

namespace Jpsys.SagyoManage.Boot
{
    public partial class LoginFrame : Form
    {

        [DllImport("user32.dll")]

        extern static int GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]

        extern static IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]

        extern static bool AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);

        #region ユーザ定義

        private static string WINDOW_TITLE = "ログイン";

        /// <summary>
        /// 売上のテーブル名
        /// </summary>
        private const string TABLENAME = "Juchu";

        /// <summary>
        /// 売上テーブルの列名「数量」
        /// </summary>
        private const string COLUMNNAME_Number = "Number";
        /// <summary>
        /// 売上テーブルの列名「単価」
        /// </summary>
        private const string COLUMNNAME_AtPrice = "AtPrice";

        /// <summary>
        /// ログイン処理を継続してOKかどうかのフラグ
        /// </summary>
        private bool loginOkFlag = false;

        /// <summary>
        /// ログイン成功後のチェック処理でエラーがある場合のメッセージ
        /// </summary>
        private string loginCheckNgMessage = string.Empty;

        #endregion

        public LoginFrame()
        {
            InitializeComponent();
        }

        public void InitLoginFrame()
        {
            this.Text = WINDOW_TITLE;

            this.lblCompanyName.Text =
                 ConfigurationManager.AppSettings["DisplayCompanyName"].Trim();

            this.lblSystemName.Text =
                 (ConfigurationManager.AppSettings["DisplaySystemName"].Trim() + " " +
                    ConfigurationManager.AppSettings["DisplaySystemVersion"]).Trim();

            this.Icon =
                global::Jpsys.SagyoManage.Boot.
                    Properties.Resources.AppIcon;

            this.pnlTitle.BackColor = FrameUtilites.GetFrameTitleBackColor();

            //保存したログインIDを画面へセット

            this.txtUserId.Text = Properties.Settings.Default.UserId;
            //this.txtPassword.Text = Properties.Settings.Default.Password;

            if (!Properties.Settings.Default.UserId.Equals(string.Empty))
            {
                this.chkLoginSave.Checked = true;
            }

            if (this.txtUserId.Text.Trim().Length > 0)
            {
                this.ActiveControl = this.txtPassword;
            }
        }

        /// <summary>
        /// 入力されたログインID・パスワードを指定して、ログインしようとしている
        /// ユーザがNSK管理用ログインかどうか確認し、そのIDを偽装する対象のオペレータ
        /// 情報を取得します。OperataInfoがnullの場合はNSK管理者ではありません。
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="loginPassword">ログインパスワード</param>
        /// <returns>偽装されたID</returns>
        private OperatorInfo CheckNskAdmin(string loginId, string loginPassword)
        {
            OperatorInfo rt_val = null;

            ////大文字に変換してNskAdminIDを取得
            //string wk_adminid = ConfigurationManager.AppSettings["NSKAdminID"].Trim().ToUpper();
            ////普通にNSKAdminPasswordを取得
            //string wk_pass = ConfigurationManager.AppSettings["NSKAdminPassword"].Trim();

            //--NSKAdminIDを取得・大文字に変換
            var wk_adminid =
                ConfigurationManager.AppSettings["NSKAdminID"] ?? string.Empty;
            wk_adminid = wk_adminid.Trim().ToUpper();
            //--NSKAdminPasswordを取得
            var wk_pass =
                ConfigurationManager.AppSettings["NSKAdminPassword"] ?? string.Empty;
            wk_pass = wk_pass.Trim();

            //追加 T.Kuroki 20140624 暗号化された偽装用IDを取得しておく。暗号化があった場合にはそちらを優先する。
            //--EncryptedNSKAdminIDを取得
            var wk_encadminid =
                ConfigurationManager.AppSettings["EncryptedNSKAdminID"] ?? string.Empty;
            wk_encadminid = wk_encadminid.Trim();
            //--EncryptedNSKAdminPasswordを取得
            var wk_encpass =
                ConfigurationManager.AppSettings["EncryptedNSKAdminPassword"] ?? string.Empty;
            wk_encpass = wk_encpass.Trim();

            //偽装IDの設定がそもそもない場合は偽装IDの取得をスルー
            if (wk_adminid.Length == 0 &&
                wk_pass.Length == 0 &&
                wk_encadminid.Length == 0 &&
                wk_encpass.Length == 0)
            {
                return null;
            }

            //暗号化されたIDかPASSのキーがあったらこっちを優先する。
            if (wk_encadminid.Length != 0 || wk_encpass.Length != 0)
            {
                //復号化キー
                string dec_key = SQLHelper.DEC_KEY;

                //比較用の変数に再設定
                //--IDは大文字比較をするのでUpperCaseしておく。
                wk_adminid = NSKUtil.DecryptString(wk_encadminid, dec_key).ToUpper();
                wk_pass = NSKUtil.DecryptString(wk_encpass, dec_key);

            }

            //大文字で比較
            if (wk_adminid == loginId.ToUpper() && wk_pass == loginPassword)
            {
                ////同じだったらIDを偽装
                ////偽装用のIDでオペレータ情報を取得
                //Operator fake_op = new Operator();
                //rt_val = fake_op.GetOperatorInfo(wk_fakeid);

                //OpereatorInfoのインスタンスをつくり最低限必要な値のみ設定して返却する
                rt_val = new OperatorInfo()
                {
                    OperatorId = 0,
                    OperatorCode = string.Empty,
                    OperatorName = string.Empty,
                    AuthorityId = 0,
                };

                //偽装IDでオペレータ情報を取得できない場合はエラー・・
                if (rt_val == null)
                {
                    throw new ApplicationException("偽装IDが不正です。設定ファイルを確認してください。");
                }
            }

            return rt_val;
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        private void Login()
        {
            //偽装IDとして処理するかどうかのフラグ
            bool use_fake_if = false;


            //画面からログインIDとパスワードを取得
            string loginId = this.txtUserId.Text.Trim();
            string password = this.txtPassword.Text.Trim();

            //ログインするオペレータの情報を保持する
            OperatorInfo op_info = null;
            //後でOperatorのビジネスロジックを使用するためにインスタンス化しておく
            Operator op_bll = new Operator();

            //NSKAdminチェック
            op_info = this.CheckNskAdmin(loginId, password);

            //NSKAdminチェックの結果がnullだった場合には通常のログイン処理を行う。
            if (op_info == null)
            {
                //ログインIDとパスワードを指定して、ログイン処理にてオペレーター情報を取得
                op_info = op_bll.Login(loginId, password);

                ////他でログインしている場合
                //if (op_info != null && op_info.LoginYMD != DateTime.MinValue)
                //{
                //    this.loginCheckNgMessage = "すでにログイン中です。\r\n心当たりがない場合はシステム管理者にお問い合わせください。";
                //    op_info = null;
                //}

                //※ログインIDのモデル上のプロパティ名は"OperatorCode"
                if ((op_info != null) &&
                    (op_info.OperatorCode.ToLower() == loginId.ToLower()) && (op_info.Password == password))
                {
                    //ログイン成功
                    loginOkFlag = true;
                }
            }
            else
            {
                //nullでない場合は、ログインID偽装がOKと判断しログインを成功とする
                loginOkFlag = true;
                //同様にログイン時にIDを偽装することにする。
                use_fake_if = true;

                op_info.AdminKbn = 1;
            }

            //ログイン成功時・・
            if (loginOkFlag)
            {
                //一致すればログイン成功

                //UserPropertyにオペレーター情報をセット
                Property.UserProperty.GetInstance().LoginOperetorId = op_info.OperatorId;
                Property.UserProperty.GetInstance().LoginCode = op_info.OperatorCode;
                Property.UserProperty.GetInstance().LoginOperatorName = op_info.OperatorName.Trim();
                Property.UserProperty.GetInstance().AuthorityId = op_info.AuthorityId;
                Jpsys.SagyoManage.Property.UserProperty.GetInstance().AdminKbn = op_info.AdminKbn;

                //システム情報保持
                //SystemSettings SystemSettings = new SystemSettings();
                //Property.UserProperty.GetInstance().SystemSettingsInfo = SystemSettings.GetInfo();
                //SystemName SystemName = new SystemName();
                //Property.UserProperty.GetInstance().SystemNameList = SystemName.GetList();
                //DefaultSettings DefaultSettings = new DefaultSettings();
                //Property.UserProperty.GetInstance().DefaultSettingsInfo = DefaultSettings.GetInfo();
                //Property.UserProperty.GetInstance().JuchuNumberIntDigits = SQLHelper.GetIntegerPartDigitsFromDatabase(TABLENAME, COLUMNNAME_Number);
                //Property.UserProperty.GetInstance().JuchuNumberDecimalDigits = SQLHelper.GetDecimalPartDigitsFromDatabase(TABLENAME, COLUMNNAME_Number);
                //Property.UserProperty.GetInstance().JuchuAtPriceIntDigits = SQLHelper.GetIntegerPartDigitsFromDatabase(TABLENAME, COLUMNNAME_AtPrice);
                //Property.UserProperty.GetInstance().JuchuAtPriceDecimalDigits = SQLHelper.GetDecimalPartDigitsFromDatabase(TABLENAME, COLUMNNAME_AtPrice);

                //ログインID偽装ではない場合
                if (!use_fake_if)
                {
                    //ログイン時間を利用者マスタに登録
                    op_info.LoginYMD = DateTime.Now;

                    try
                    {
                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            op_bll.SaveLoginYMD(tx, op_info);
                        });
                    }
                    catch (Exception ex)
                    {
                        //例外ハンドラ
                        FrameUtilites.ShowExceptionMessage(ex, this);
                    }
                }

                ////トラDONのマスタ情報を非同期で同期
                //this.MergeToraDonTables().ContinueWith(t => { },
                //    TaskScheduler.FromCurrentSynchronizationContext());

                ////念のため排他情報を非同期で削除
                //this.ClearHaitaWorkAsync().ContinueWith(t => { },
                //    TaskScheduler.FromCurrentSynchronizationContext());
            }

            //ログインIDが偽装されている場合は、NSKの管理用アカウントと認識しフラグを立て
            //るようにする
            Property.UserProperty.GetInstance().NSKAdminLoginFlag = use_fake_if;

            if (this.loginOkFlag)
            {
                //「ユーザーIDを保存する」にチェックが入っていれば保存する

                if (this.chkLoginSave.Checked)
                {
                    Properties.Settings.Default.UserId = loginId;
                    Properties.Settings.Default.Password = string.Empty;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.UserId = string.Empty;
                    Properties.Settings.Default.Password = string.Empty;
                    Properties.Settings.Default.Save();
                }
                
                //ダイアログリザルトをOKで画面を閉じる
                this.DoClose(DialogResult.OK);
            }
            else
            {
                if (this.loginCheckNgMessage.Length != 0)
                {
                    //--失敗メッセージを表示
                    MessageBox.Show(this,
                        this.loginCheckNgMessage,
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //--失敗メッセージを表示
                    MessageBox.Show(this,
                        "ログインID、またはパスワードを認識できません。",
                        "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        ///// <summary>
        ///// トラDONのマスタ情報を同期します。
        ///// </summary>
        ///// <returns>実行結果</returns>
        //private Task<bool> MergeToraDonTables()
        //{
        //    return
        //        Task.Factory.StartNew(() =>
        //        {
        //            try
        //            {
        //                SQLHelper.ActionWithTransaction(tx =>
        //                {
        //                    MergeToraDon margeToraDon = new MergeToraDon();
        //                    margeToraDon.MergeToraDonAllTables(tx);
        //                });
        //            }
        //            catch(Exception e)
        //            {
        //                //何もしない
        //                ;
        //            }

        //            return true;
        //        });
        //}

        ///// <summary>
        ///// 配車排他管理情報を削除します。
        ///// </summary>
        ///// <returns>実行結果</returns>
        //private Task<bool> ClearHaitaWorkAsync()
        //{
        //    return
        //        Task.Factory.StartNew(() =>
        //        {
        //            SQLHelper.ActionWithTransaction(tx =>
        //            {
        //                HaishaExclusiveManage haishaExclusiveManage = new HaishaExclusiveManage();
        //                haishaExclusiveManage.Delete(tx, new HaishaExclusiveManageInfo()
        //                {
        //                    OperatorId = Property.UserProperty.GetInstance().LoginOperetorId
        //                });
        //            });

        //            return true;
        //        });
        //}

        /// <summary>
        /// ダイアログリザルトを指定して画面を閉じる
        /// </summary>
        /// <param name="dialogResult">ダイアログリザルト(OK:ログイン)</param>
        private void DoClose(DialogResult dialogResult)
        {
            this.DialogResult = dialogResult;
            this.Close();
        }

        /// <summary>
        /// 画面をクリア
        /// </summary>
        private void DoClear()
        {
            this.txtUserId.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
        }

        /// <summary>
        /// キーイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessKeyEvent(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SendKeys.SendWait("{TAB}");
                    break;
                case Keys.F12:
                    SendKeys.SendWait("+{TAB}");
                    break;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                this.Login();
            }
            catch (Model.DALExceptions.CanRetryException ex)
            {
                FrameLib.FrameUtilites.ShowExceptionMessage(ex, this);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.DoClear(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DoClose(DialogResult.Cancel);
        }

        private void LoginFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void LoginFrame_Load(object sender, EventArgs e)
        {

        }

        private void LoginFrame_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.Activate();
            this.TopMost = false;
            this.Activate();
        }
    }
}
