namespace Jpsys.SagyoManage.Frame
{
    partial class PasswordChangeFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.edtConfirmPassword = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtNewPassword = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtOperatorName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.chkShowPassword = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtConfirmPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtNewPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtOperatorName)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.edtConfirmPassword);
            this.groupBox1.Controls.Add(this.edtNewPassword);
            this.groupBox1.Controls.Add(this.edtOperatorName);
            this.groupBox1.Controls.Add(this.chkShowPassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.ForeColor = System.Drawing.Color.DarkGreen;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "新しいパスワードを設定してください";
            // 
            // edtConfirmPassword
            // 
            this.edtConfirmPassword.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtConfirmPassword.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.edtConfirmPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtConfirmPassword.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtConfirmPassword.DropDown.AllowDrop = false;
            this.edtConfirmPassword.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.edtConfirmPassword.HighlightText = true;
            this.edtConfirmPassword.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.edtConfirmPassword.Location = new System.Drawing.Point(139, 92);
            this.edtConfirmPassword.MaxLength = 255;
            this.edtConfirmPassword.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtConfirmPassword.Name = "edtConfirmPassword";
            this.edtConfirmPassword.PasswordChar = '●';
            this.gcShortcut1.SetShortcuts(this.edtConfirmPassword, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl",
                "PreviousControl"}));
            this.edtConfirmPassword.Size = new System.Drawing.Size(258, 28);
            this.edtConfirmPassword.TabIndex = 1;
            // 
            // edtNewPassword
            // 
            this.edtNewPassword.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtNewPassword.AllowSpace = GrapeCity.Win.Editors.AllowSpace.None;
            this.edtNewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtNewPassword.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtNewPassword.DropDown.AllowDrop = false;
            this.edtNewPassword.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.edtNewPassword.HighlightText = true;
            this.edtNewPassword.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.edtNewPassword.Location = new System.Drawing.Point(139, 58);
            this.edtNewPassword.MaxLength = 255;
            this.edtNewPassword.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtNewPassword.Name = "edtNewPassword";
            this.edtNewPassword.PasswordChar = '●';
            this.gcShortcut1.SetShortcuts(this.edtNewPassword, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl",
                "PreviousControl"}));
            this.edtNewPassword.Size = new System.Drawing.Size(258, 28);
            this.edtNewPassword.TabIndex = 0;
            // 
            // edtOperatorName
            // 
            this.edtOperatorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtOperatorName.BackColor = System.Drawing.SystemColors.Control;
            this.edtOperatorName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtOperatorName.DropDown.AllowDrop = false;
            this.edtOperatorName.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.edtOperatorName.HighlightText = true;
            this.edtOperatorName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtOperatorName.Location = new System.Drawing.Point(139, 24);
            this.edtOperatorName.MaxLength = 14;
            this.edtOperatorName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtOperatorName.Name = "edtOperatorName";
            this.edtOperatorName.ReadOnly = true;
            this.gcShortcut1.SetShortcuts(this.edtOperatorName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl",
                "PreviousControl"}));
            this.edtOperatorName.Size = new System.Drawing.Size(258, 28);
            this.edtOperatorName.TabIndex = 158;
            this.edtOperatorName.TabStop = false;
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkShowPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkShowPassword.Location = new System.Drawing.Point(139, 126);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkShowPassword.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkShowPassword.Size = new System.Drawing.Size(123, 22);
            this.chkShowPassword.TabIndex = 2;
            this.chkShowPassword.TabStop = false;
            this.chkShowPassword.Text = "パスワードを表示";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.CausesValidation = false;
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(6, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 24);
            this.label3.TabIndex = 157;
            this.label3.Text = "ユーザー名";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 24);
            this.label1.TabIndex = 155;
            this.label1.Text = "確認用";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 24);
            this.label2.TabIndex = 153;
            this.label2.Text = "新しいパスワード";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 181);
            this.panel1.TabIndex = 171;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 181);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(409, 40);
            this.panel2.TabIndex = 172;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOk.Location = new System.Drawing.Point(191, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "変更";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(297, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 177);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(409, 4);
            this.splitter2.TabIndex = 173;
            this.splitter2.TabStop = false;
            // 
            // PasswordChangeFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(409, 221);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "PasswordChangeFrame";
            this.Text = "パスワード変更";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtConfirmPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtNewPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtOperatorName)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkShowPassword;
        private GrapeCity.Win.Editors.GcTextBox edtConfirmPassword;
        private GrapeCity.Win.Editors.GcTextBox edtNewPassword;
        private GrapeCity.Win.Editors.GcTextBox edtOperatorName;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
    }
}