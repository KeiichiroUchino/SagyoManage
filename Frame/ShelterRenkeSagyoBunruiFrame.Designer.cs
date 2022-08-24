namespace Jpsys.SagyoManage.Frame
{
    partial class ShelterRenkeSagyoBunruiFrame
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
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtImportFilePath = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.pnl = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStripImportCsv = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripClose = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton2 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton3 = new GrapeCity.Win.Editors.DropDownButton();
            this.rbtSagyoDaiBunrui = new System.Windows.Forms.RadioButton();
            this.rbtSagyoChuBunrui = new System.Windows.Forms.RadioButton();
            this.rbtSagyoShoBunrui = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.edtImportFilePath)).BeginInit();
            this.pnl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // edtImportFilePath
            // 
            this.edtImportFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtImportFilePath.BackColor = System.Drawing.SystemColors.Control;
            this.edtImportFilePath.DropDown.AllowDrop = false;
            this.edtImportFilePath.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.edtImportFilePath.HighlightText = true;
            this.edtImportFilePath.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtImportFilePath.Location = new System.Drawing.Point(9, 112);
            this.edtImportFilePath.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtImportFilePath.Multiline = true;
            this.edtImportFilePath.Name = "edtImportFilePath";
            this.edtImportFilePath.ReadOnly = true;
            this.edtImportFilePath.ScrollBarMode = GrapeCity.Win.Editors.ScrollBarMode.Automatic;
            this.edtImportFilePath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gcShortcut1.SetShortcuts(this.edtImportFilePath, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl"}));
            this.edtImportFilePath.Size = new System.Drawing.Size(549, 62);
            this.edtImportFilePath.TabIndex = 330;
            this.edtImportFilePath.TabStop = false;
            this.edtImportFilePath.Text = "C:\\Users\\takada-ko\\Documents\\USER\\KinkouKasetu\\2016\\SRC\\Framework";
            // 
            // pnl
            // 
            this.pnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl.Controls.Add(this.groupBox1);
            this.pnl.Location = new System.Drawing.Point(0, 24);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(681, 203);
            this.pnl.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnSelectFile);
            this.groupBox1.Controls.Add(this.edtImportFilePath);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(675, 197);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "取込ファイル";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectFile.Location = new System.Drawing.Point(564, 112);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(105, 30);
            this.btnSelectFile.TabIndex = 331;
            this.btnSelectFile.TabStop = false;
            this.btnSelectFile.Text = "参照";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.CausesValidation = false;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(9, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 18);
            this.label4.TabIndex = 61;
            this.label4.Text = "取込ファイル *";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripImportCsv
            // 
            this.toolStripImportCsv.ForeColor = System.Drawing.Color.White;
            this.toolStripImportCsv.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripImportCsv.Name = "toolStripImportCsv";
            this.toolStripImportCsv.Size = new System.Drawing.Size(82, 20);
            this.toolStripImportCsv.Text = "F8：取込実行";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripClose,
            this.toolStripImportCsv});
            this.statusStrip1.Location = new System.Drawing.Point(0, 269);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(681, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 203;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripClose
            // 
            this.toolStripClose.ForeColor = System.Drawing.Color.White;
            this.toolStripClose.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripClose.Name = "toolStripClose";
            this.toolStripClose.Size = new System.Drawing.Size(58, 20);
            this.toolStripClose.Text = "F1：終了";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(564, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "CSV取込";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.CausesValidation = false;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(681, 1);
            this.label5.TabIndex = 82;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 219);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(681, 50);
            this.pnlBottom.TabIndex = 2;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(681, 24);
            this.menuStripTop.TabIndex = 9;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // dropDownButton1
            // 
            this.dropDownButton1.Name = "dropDownButton1";
            // 
            // dropDownButton2
            // 
            this.dropDownButton2.Name = "dropDownButton2";
            // 
            // dropDownButton3
            // 
            this.dropDownButton3.Name = "dropDownButton3";
            // 
            // rbtSagyoDaiBunrui
            // 
            this.rbtSagyoDaiBunrui.AutoSize = true;
            this.rbtSagyoDaiBunrui.ForeColor = System.Drawing.Color.Black;
            this.rbtSagyoDaiBunrui.Location = new System.Drawing.Point(6, 25);
            this.rbtSagyoDaiBunrui.Name = "rbtSagyoDaiBunrui";
            this.rbtSagyoDaiBunrui.Size = new System.Drawing.Size(86, 22);
            this.rbtSagyoDaiBunrui.TabIndex = 332;
            this.rbtSagyoDaiBunrui.TabStop = true;
            this.rbtSagyoDaiBunrui.Text = "作業大分類";
            this.rbtSagyoDaiBunrui.UseVisualStyleBackColor = true;
            // 
            // rbtSagyoChuBunrui
            // 
            this.rbtSagyoChuBunrui.AutoSize = true;
            this.rbtSagyoChuBunrui.ForeColor = System.Drawing.Color.Black;
            this.rbtSagyoChuBunrui.Location = new System.Drawing.Point(98, 25);
            this.rbtSagyoChuBunrui.Name = "rbtSagyoChuBunrui";
            this.rbtSagyoChuBunrui.Size = new System.Drawing.Size(86, 22);
            this.rbtSagyoChuBunrui.TabIndex = 333;
            this.rbtSagyoChuBunrui.TabStop = true;
            this.rbtSagyoChuBunrui.Text = "作業中分類";
            this.rbtSagyoChuBunrui.UseVisualStyleBackColor = true;
            // 
            // rbtSagyoShoBunrui
            // 
            this.rbtSagyoShoBunrui.AutoSize = true;
            this.rbtSagyoShoBunrui.ForeColor = System.Drawing.Color.Black;
            this.rbtSagyoShoBunrui.Location = new System.Drawing.Point(190, 25);
            this.rbtSagyoShoBunrui.Name = "rbtSagyoShoBunrui";
            this.rbtSagyoShoBunrui.Size = new System.Drawing.Size(86, 22);
            this.rbtSagyoShoBunrui.TabIndex = 334;
            this.rbtSagyoShoBunrui.TabStop = true;
            this.rbtSagyoShoBunrui.Text = "作業小分類";
            this.rbtSagyoShoBunrui.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtSagyoDaiBunrui);
            this.groupBox2.Controls.Add(this.rbtSagyoChuBunrui);
            this.groupBox2.Controls.Add(this.rbtSagyoShoBunrui);
            this.groupBox2.Location = new System.Drawing.Point(9, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 64);
            this.groupBox2.TabIndex = 337;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "取込対象";
            // 
            // ShelterRenkeSagyoBunruiFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(681, 291);
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ShelterRenkeSagyoBunruiFrame";
            this.Text = "ShelterRenkeSagyoBunruiFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JuchuNyuryokuFrame_FormClosing);
            this.Shown += new System.EventHandler(this.JuchuNyuryokuFrame_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.edtImportFilePath)).EndInit();
            this.pnl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripImportCsv;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton2;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectFile;
        private GrapeCity.Win.Editors.GcTextBox edtImportFilePath;
        private System.Windows.Forms.RadioButton rbtSagyoDaiBunrui;
        private System.Windows.Forms.RadioButton rbtSagyoChuBunrui;
        private System.Windows.Forms.RadioButton rbtSagyoShoBunrui;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}