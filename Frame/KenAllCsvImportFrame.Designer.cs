namespace Jpsys.HaishaManageV10.Frame
{
    partial class KenAllCsvImportFrame
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
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripReference = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripImportCsv = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnImportCsv = new System.Windows.Forms.Button();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtCsvPath = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.sbtnCsvPath = new GrapeCity.Win.Editors.SideButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblPost = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radModeAraigae = new System.Windows.Forms.RadioButton();
            this.radModeTsuiki = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtCsvPath)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStripTop.Size = new System.Drawing.Size(773, 24);
            this.menuStripTop.TabIndex = 202;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripReference,
            this.toolStripImportCsv});
            this.statusStrip1.Location = new System.Drawing.Point(0, 236);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(773, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 204;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripEnd
            // 
            this.toolStripEnd.ForeColor = System.Drawing.Color.White;
            this.toolStripEnd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripEnd.Name = "toolStripEnd";
            this.toolStripEnd.Size = new System.Drawing.Size(58, 20);
            this.toolStripEnd.Text = "F1：終了";
            // 
            // toolStripReference
            // 
            this.toolStripReference.ForeColor = System.Drawing.Color.White;
            this.toolStripReference.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripReference.Name = "toolStripReference";
            this.toolStripReference.Size = new System.Drawing.Size(58, 20);
            this.toolStripReference.Text = "F5：参照";
            // 
            // toolStripImportCsv
            // 
            this.toolStripImportCsv.ForeColor = System.Drawing.Color.White;
            this.toolStripImportCsv.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripImportCsv.Name = "toolStripImportCsv";
            this.toolStripImportCsv.Size = new System.Drawing.Size(119, 20);
            this.toolStripImportCsv.Text = "Shift+F8：CSV取込";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label12);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnImportCsv);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 192);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(773, 44);
            this.pnlBottom.TabIndex = 205;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.CausesValidation = false;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(773, 1);
            this.label12.TabIndex = 1031;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(12, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "終了";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnImportCsv
            // 
            this.btnImportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCsv.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnImportCsv.Location = new System.Drawing.Point(656, 6);
            this.btnImportCsv.Name = "btnImportCsv";
            this.btnImportCsv.Size = new System.Drawing.Size(105, 30);
            this.btnImportCsv.TabIndex = 0;
            this.btnImportCsv.TabStop = false;
            this.btnImportCsv.Text = "CSV取込";
            this.btnImportCsv.UseVisualStyleBackColor = true;
            // 
            // edtCsvPath
            // 
            this.edtCsvPath.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtCsvPath.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtCsvPath.DropDown.AllowDrop = false;
            this.edtCsvPath.HighlightText = true;
            this.edtCsvPath.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtCsvPath.Location = new System.Drawing.Point(88, 121);
            this.edtCsvPath.MaxLength = 2147483647;
            this.edtCsvPath.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtCsvPath.Name = "edtCsvPath";
            this.gcShortcut1.SetShortcuts(this.edtCsvPath, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl"}));
            this.edtCsvPath.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sbtnCsvPath});
            this.edtCsvPath.Size = new System.Drawing.Size(630, 28);
            this.edtCsvPath.TabIndex = 14;
            this.edtCsvPath.Text = "C:\\TEMP\\";
            // 
            // sbtnCsvPath
            // 
            this.sbtnCsvPath.ButtonWidth = 22;
            this.sbtnCsvPath.Name = "sbtnCsvPath";
            this.sbtnCsvPath.Text = "...";
            this.sbtnCsvPath.Click += new System.EventHandler(this.sbtnCsvPath_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.groupBox1);
            this.pnlMain.Controls.Add(this.lblPost);
            this.pnlMain.Controls.Add(this.edtCsvPath);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.label22);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(773, 168);
            this.pnlMain.TabIndex = 0;
            // 
            // lblPost
            // 
            this.lblPost.AutoSize = true;
            this.lblPost.Location = new System.Drawing.Point(88, 37);
            this.lblPost.Name = "lblPost";
            this.lblPost.Size = new System.Drawing.Size(356, 18);
            this.lblPost.TabIndex = 1090;
            this.lblPost.TabStop = true;
            this.lblPost.Text = "日本郵便トップ > 郵便番号検索 > 郵便番号データダウンロード";
            this.lblPost.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPost_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(85, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(557, 18);
            this.label2.TabIndex = 1089;
            this.label2.Text = "下記サイトからダウンロードした「KEN_ALL.CSV」（全国一括）ファイルの取込を行ってください。";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.CausesValidation = false;
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label22.Location = new System.Drawing.Point(12, 126);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(59, 18);
            this.label22.TabIndex = 1041;
            this.label22.Text = "取込元  *";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radModeAraigae);
            this.groupBox1.Controls.Add(this.radModeTsuiki);
            this.groupBox1.Location = new System.Drawing.Point(88, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.groupBox1.Size = new System.Drawing.Size(211, 48);
            this.groupBox1.TabIndex = 1091;
            this.groupBox1.TabStop = false;
            // 
            // radModeAraigae
            // 
            this.radModeAraigae.AutoSize = true;
            this.radModeAraigae.Checked = true;
            this.radModeAraigae.Location = new System.Drawing.Point(15, 17);
            this.radModeAraigae.Name = "radModeAraigae";
            this.radModeAraigae.Size = new System.Drawing.Size(74, 22);
            this.radModeAraigae.TabIndex = 0;
            this.radModeAraigae.TabStop = true;
            this.radModeAraigae.Text = "洗い替え";
            this.radModeAraigae.UseVisualStyleBackColor = true;
            // 
            // radModeTsuiki
            // 
            this.radModeTsuiki.AutoSize = true;
            this.radModeTsuiki.Location = new System.Drawing.Point(130, 17);
            this.radModeTsuiki.Name = "radModeTsuiki";
            this.radModeTsuiki.Size = new System.Drawing.Size(50, 22);
            this.radModeTsuiki.TabIndex = 1;
            this.radModeTsuiki.Text = "追記";
            this.radModeTsuiki.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 18);
            this.label1.TabIndex = 1041;
            this.label1.Text = "モード  *";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.CausesValidation = false;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(305, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(308, 18);
            this.label3.TabIndex = 1089;
            this.label3.Text = "（※追記の場合、同じデータは重複して登録されます）";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // KenAllCsvImportFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(773, 258);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripTop);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "KenAllCsvImportFrame";
            this.Text = "KenAllCsvImportFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KenAllCsvImportFrame_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KenAllCsvImportFrame_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edtCsvPath)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripReference;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnClose;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripImportCsv;
        private System.Windows.Forms.Button btnImportCsv;
        private GrapeCity.Win.Editors.GcTextBox edtCsvPath;
        private GrapeCity.Win.Editors.SideButton sbtnCsvPath;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.LinkLabel lblPost;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radModeAraigae;
        private System.Windows.Forms.RadioButton radModeTsuiki;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}