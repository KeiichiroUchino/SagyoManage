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
    public partial class EmphasizeErrorMessage : Form, IFrameBase
    {
        #region ユーザ定義

        private String _Title = string.Empty;

        private String _ErrorMessage = string.Empty;

        private ButtonShubetsu _ButtonShubetsu = ButtonShubetsu.OK;

        private bool _ButtonTabStop = true;

        /// <summary>
        /// ボタン種類を表す列挙体です。
        /// </summary>
        public enum ButtonShubetsu : int
        {
            /// <summary>
            /// 0:OK
            /// </summary>
            OK = 0,
            /// <summary>
            /// 1:OKCancel
            /// </summary>
            OKCancel = 1,
            /// <summary>
            /// 4:YesNo
            /// </summary>
            YesNo = 4,
        }

        #endregion

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public EmphasizeErrorMessage()
        {
            InitializeComponent();
        }

        #region 初期化処理

        private void InitEmphasizeErrorMessage()
        {
            //タイトル設定
            this.Text = _Title;

            //エラーメッセージ設定
            this.lblErrorMessage.Text = _ErrorMessage;

            //親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //ボタン設定
            this.SetButtons();

            //フォーカス削除
            if (!this._ButtonTabStop)
            {
                this.btnOk.TabStop = this._ButtonTabStop;
                this.btnCancel.TabStop = this._ButtonTabStop;
            }
        }
        
        #endregion

        #region プライベートクラス

        /// <summary>
        /// メニュー関連を初期化します。
        /// </summary>
        private void SetButtons()
        {
            //ボタンの初期表示位置を取得
            Point btn1Point = this.btnOk.Location;
            Point btn2Point = this.btnCancel.Location;
            if (this._ButtonShubetsu == ButtonShubetsu.OK)
            {
                //キャンセルボタン非表示
                this.btnCancel.Visible = false;

                //OKボタン表示位置変更
                this.btnOk.Location = btn2Point;
            }
            else if (this._ButtonShubetsu == ButtonShubetsu.OKCancel)
            {
                this.ActiveControl = this.btnCancel;
                //this.btnCancel.Focus();
            }
            else if (this._ButtonShubetsu == ButtonShubetsu.YesNo)
            {
                //ボタン表示名変更
                this.btnOk.Text = "Yes";
                this.btnCancel.Text = "No";
                this.ActiveControl = this.btnCancel;
                //this.btnCancel.Focus();
            }
        }

        #endregion

        #region IFrameBase メンバ

        public void InitFrame()
        {
            this.InitEmphasizeErrorMessage();
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

        private void EmphasizeErrorMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        #region プロパティ

        public String Title
        {
            set { this._Title = value; }
        }

        public ButtonShubetsu SetButtonShubetsu
        {
            set { this._ButtonShubetsu = value; }
        }

        public String ErrorMessage
        {
            set { this._ErrorMessage = value; }
        }

        public bool ButtonTabStop
        {
            set { this._ButtonTabStop = value; }
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            //「OK」ボタンがクリックされたとき
            if (this._ButtonShubetsu == ButtonShubetsu.OK
                || this._ButtonShubetsu == ButtonShubetsu.OKCancel)
            {
                this.DialogResult = DialogResult.OK;
            }
            else if (this._ButtonShubetsu == ButtonShubetsu.YesNo)
            {
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //「キャンセル」ボタンがクリックされたとき
            if (this._ButtonShubetsu == ButtonShubetsu.OKCancel)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else if (this._ButtonShubetsu == ButtonShubetsu.YesNo)
            {
                this.DialogResult = DialogResult.No;
            }
        }
    }
}
