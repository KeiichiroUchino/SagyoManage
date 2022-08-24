using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.Boot
{
    public partial class ErrorMessgeFrame : Form
    {
        private string _message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="content">エラー内容</param>
        public ErrorMessgeFrame(string message)
        {
            InitializeComponent();

            this._message = message;
        }


        private void ErrorMessgeBox_Load(object sender, EventArgs e)
        {
            this.Text = "エラー";
            this.pictureBox1.Image = SystemIcons.Error.ToBitmap();
            this.txtMessage.Text = this._message;
            //フォントをメッセージボックスフォントに設定
            this.lblMsg.Font = System.Drawing.SystemFonts.MessageBoxFont;
            this.txtMessage.Font = System.Drawing.SystemFonts.MessageBoxFont;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.A | Keys.Control:
                    if (this.ActiveControl is TextBox)
                    {
                        TextBox txt = (TextBox)this.ActiveControl;
                        txt.SelectionStart = 0;
                        txt.SelectionLength = txt.Text.Length;
                        return true;
                    }
                    break;
                //このほかにもショートカットキーなどをここに記述できる
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
