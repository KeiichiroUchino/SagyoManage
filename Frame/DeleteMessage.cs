using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;

namespace Jpsys.SagyoManage.Frame
{
    public partial class DeleteMessage : Form, IFrameBase
    {
        #region ユーザ定義

        private const string WINDOW_TITLE = "削除確認画面";

        #endregion

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public DeleteMessage()
        {
            InitializeComponent();
        }

        #region 初期化処理

        private void InitDeleteMessage()
        {
            this.Text = WINDOW_TITLE;

            ////画面のアイコンを設定
            //this.Icon =
            //    global ::Kyoshingumi.CargoManage.Common.Frame.Properties.Resources.AppIcon;

            //親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //削除可能チェックボックスはfalseにしておく。
            this.chkDel.Checked = false;

            //OKボタンは無効
            this.btnOk.Enabled = false;
        }
        
        #endregion

        #region プライベートクラス

        /// <summary>
        /// 現在のチェックボックスの値によって削除ボタンのEnabledプロパティを切替えます。
        /// </summary>
        private void ChangeDelButtonEnabled()
        {
            this.btnOk.Enabled = this.chkDel.Checked;
        }

        #endregion

        #region IFrameBase メンバ

        public void InitFrame()
        {
            this.InitDeleteMessage();
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

        private void DeleteMessage_FormClosing(object sender, FormClosingEventArgs e)
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

        private void chkDel_CheckedChanged(object sender, EventArgs e)
        {
            //「削除する」チェックボックスの値が変わったとき
            this.ChangeDelButtonEnabled();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //「OK」ボタンがクリックされたとき
            if (this.chkDel.Checked)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //「キャンセル」ボタンがクリックされたとき
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
