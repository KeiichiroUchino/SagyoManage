namespace Jpsys.SagyoManage.ReportAppendix
{
    partial class PrinterChoiceDialog
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grbDuplexAllign = new System.Windows.Forms.GroupBox();
            this.rbtDuplexHorizontal = new System.Windows.Forms.RadioButton();
            this.rbtDuplexVertical = new System.Windows.Forms.RadioButton();
            this.chkUseMono = new System.Windows.Forms.CheckBox();
            this.chkUseDuplex = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboPrinterPaperSources = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textPaperName = new System.Windows.Forms.TextBox();
            this.btnShowPaperSetting = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboInstalledPrinters = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.plnBasic = new System.Windows.Forms.Panel();
            this.plnButtons = new System.Windows.Forms.Panel();
            this.plnRangeCopies = new System.Windows.Forms.Panel();
            this.grbPageRange = new System.Windows.Forms.GroupBox();
            this.nudRangeTo = new System.Windows.Forms.NumericUpDown();
            this.nudRangeFrom = new System.Windows.Forms.NumericUpDown();
            this.rbtRangeSelect = new System.Windows.Forms.RadioButton();
            this.rbtRangeCurrent = new System.Windows.Forms.RadioButton();
            this.rbtAllRange = new System.Windows.Forms.RadioButton();
            this.grbPageCopies = new System.Windows.Forms.GroupBox();
            this.chkCollated = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudCopies = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.grbDuplexAllign.SuspendLayout();
            this.plnBasic.SuspendLayout();
            this.plnButtons.SuspendLayout();
            this.plnRangeCopies.SuspendLayout();
            this.grbPageRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRangeTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRangeFrom)).BeginInit();
            this.grbPageCopies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCopies)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grbDuplexAllign);
            this.groupBox1.Controls.Add(this.chkUseMono);
            this.groupBox1.Controls.Add(this.chkUseDuplex);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboPrinterPaperSources);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textPaperName);
            this.groupBox1.Controls.Add(this.btnShowPaperSetting);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboInstalledPrinters);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 176);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "プリンタ";
            // 
            // grbDuplexAllign
            // 
            this.grbDuplexAllign.Controls.Add(this.rbtDuplexHorizontal);
            this.grbDuplexAllign.Controls.Add(this.rbtDuplexVertical);
            this.grbDuplexAllign.Enabled = false;
            this.grbDuplexAllign.Location = new System.Drawing.Point(52, 140);
            this.grbDuplexAllign.Name = "grbDuplexAllign";
            this.grbDuplexAllign.Size = new System.Drawing.Size(316, 32);
            this.grbDuplexAllign.TabIndex = 7;
            this.grbDuplexAllign.TabStop = false;
            // 
            // rbtDuplexHorizontal
            // 
            this.rbtDuplexHorizontal.AutoSize = true;
            this.rbtDuplexHorizontal.Location = new System.Drawing.Point(172, 12);
            this.rbtDuplexHorizontal.Name = "rbtDuplexHorizontal";
            this.rbtDuplexHorizontal.Size = new System.Drawing.Size(111, 16);
            this.rbtDuplexHorizontal.TabIndex = 2;
            this.rbtDuplexHorizontal.Text = "用紙を上に開く(&H)";
            this.rbtDuplexHorizontal.UseVisualStyleBackColor = true;
            // 
            // rbtDuplexVertical
            // 
            this.rbtDuplexVertical.AutoSize = true;
            this.rbtDuplexVertical.Checked = true;
            this.rbtDuplexVertical.Location = new System.Drawing.Point(12, 12);
            this.rbtDuplexVertical.Name = "rbtDuplexVertical";
            this.rbtDuplexVertical.Size = new System.Drawing.Size(111, 16);
            this.rbtDuplexVertical.TabIndex = 1;
            this.rbtDuplexVertical.TabStop = true;
            this.rbtDuplexVertical.Text = "用紙を横に開く(&V)";
            this.rbtDuplexVertical.UseVisualStyleBackColor = true;
            // 
            // chkUseMono
            // 
            this.chkUseMono.AutoSize = true;
            this.chkUseMono.Location = new System.Drawing.Point(24, 176);
            this.chkUseMono.Name = "chkUseMono";
            this.chkUseMono.Size = new System.Drawing.Size(99, 16);
            this.chkUseMono.TabIndex = 6;
            this.chkUseMono.TabStop = false;
            this.chkUseMono.Text = "白黒で印刷(&M)";
            this.chkUseMono.UseVisualStyleBackColor = true;
            this.chkUseMono.Visible = false;
            // 
            // chkUseDuplex
            // 
            this.chkUseDuplex.AutoSize = true;
            this.chkUseDuplex.Location = new System.Drawing.Point(24, 124);
            this.chkUseDuplex.Name = "chkUseDuplex";
            this.chkUseDuplex.Size = new System.Drawing.Size(342, 16);
            this.chkUseDuplex.TabIndex = 5;
            this.chkUseDuplex.Text = "両面に印刷(&D)　　※プリンタの機器で、機能しない場合があります。";
            this.chkUseDuplex.UseVisualStyleBackColor = true;
            this.chkUseDuplex.CheckedChanged += new System.EventHandler(this.chkUseDuplex_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(25, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "給紙方法:";
            // 
            // comboPrinterPaperSources
            // 
            this.comboPrinterPaperSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPrinterPaperSources.Location = new System.Drawing.Point(96, 56);
            this.comboPrinterPaperSources.Name = "comboPrinterPaperSources";
            this.comboPrinterPaperSources.Size = new System.Drawing.Size(308, 20);
            this.comboPrinterPaperSources.TabIndex = 1;
            this.comboPrinterPaperSources.SelectionChangeCommitted += new System.EventHandler(this.comboPrinterPaperSources_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "用紙:";
            // 
            // textPaperName
            // 
            this.textPaperName.Location = new System.Drawing.Point(97, 92);
            this.textPaperName.Name = "textPaperName";
            this.textPaperName.ReadOnly = true;
            this.textPaperName.Size = new System.Drawing.Size(163, 19);
            this.textPaperName.TabIndex = 2;
            this.textPaperName.Text = "textBox1";
            // 
            // btnShowPaperSetting
            // 
            this.btnShowPaperSetting.Location = new System.Drawing.Point(264, 90);
            this.btnShowPaperSetting.Name = "btnShowPaperSetting";
            this.btnShowPaperSetting.Size = new System.Drawing.Size(75, 23);
            this.btnShowPaperSetting.TabIndex = 3;
            this.btnShowPaperSetting.Text = "用紙設定";
            this.btnShowPaperSetting.Click += new System.EventHandler(this.btnShowPaperSetting_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "プリンタ名:";
            // 
            // comboInstalledPrinters
            // 
            this.comboInstalledPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInstalledPrinters.Location = new System.Drawing.Point(95, 22);
            this.comboInstalledPrinters.Name = "comboInstalledPrinters";
            this.comboInstalledPrinters.Size = new System.Drawing.Size(308, 20);
            this.comboInstalledPrinters.TabIndex = 0;
            this.comboInstalledPrinters.SelectionChangeCommitted += new System.EventHandler(this.comboInstalledPrinters_SelectionChangeCommitted);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(388, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(276, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // plnBasic
            // 
            this.plnBasic.Controls.Add(this.groupBox1);
            this.plnBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.plnBasic.Location = new System.Drawing.Point(0, 0);
            this.plnBasic.Name = "plnBasic";
            this.plnBasic.Size = new System.Drawing.Size(478, 192);
            this.plnBasic.TabIndex = 0;
            // 
            // plnButtons
            // 
            this.plnButtons.Controls.Add(this.btnOk);
            this.plnButtons.Controls.Add(this.btnCancel);
            this.plnButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plnButtons.Location = new System.Drawing.Point(0, 290);
            this.plnButtons.Name = "plnButtons";
            this.plnButtons.Size = new System.Drawing.Size(478, 39);
            this.plnButtons.TabIndex = 0;
            // 
            // plnRangeCopies
            // 
            this.plnRangeCopies.Controls.Add(this.grbPageRange);
            this.plnRangeCopies.Controls.Add(this.grbPageCopies);
            this.plnRangeCopies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plnRangeCopies.Location = new System.Drawing.Point(0, 192);
            this.plnRangeCopies.Name = "plnRangeCopies";
            this.plnRangeCopies.Size = new System.Drawing.Size(478, 98);
            this.plnRangeCopies.TabIndex = 1;
            // 
            // grbPageRange
            // 
            this.grbPageRange.Controls.Add(this.nudRangeTo);
            this.grbPageRange.Controls.Add(this.nudRangeFrom);
            this.grbPageRange.Controls.Add(this.rbtRangeSelect);
            this.grbPageRange.Controls.Add(this.rbtRangeCurrent);
            this.grbPageRange.Controls.Add(this.rbtAllRange);
            this.grbPageRange.Location = new System.Drawing.Point(16, 4);
            this.grbPageRange.Name = "grbPageRange";
            this.grbPageRange.Size = new System.Drawing.Size(240, 88);
            this.grbPageRange.TabIndex = 0;
            this.grbPageRange.TabStop = false;
            this.grbPageRange.Text = "ページ範囲";
            // 
            // nudRangeTo
            // 
            this.nudRangeTo.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nudRangeTo.Location = new System.Drawing.Point(184, 57);
            this.nudRangeTo.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudRangeTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRangeTo.Name = "nudRangeTo";
            this.nudRangeTo.Size = new System.Drawing.Size(44, 19);
            this.nudRangeTo.TabIndex = 4;
            this.nudRangeTo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRangeTo.Enter += new System.EventHandler(this.nudRangeTo_Enter);
            // 
            // nudRangeFrom
            // 
            this.nudRangeFrom.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nudRangeFrom.Location = new System.Drawing.Point(120, 57);
            this.nudRangeFrom.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudRangeFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRangeFrom.Name = "nudRangeFrom";
            this.nudRangeFrom.Size = new System.Drawing.Size(44, 19);
            this.nudRangeFrom.TabIndex = 3;
            this.nudRangeFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRangeFrom.Enter += new System.EventHandler(this.nudRangeFrom_Enter);
            // 
            // rbtRangeSelect
            // 
            this.rbtRangeSelect.AutoSize = true;
            this.rbtRangeSelect.Location = new System.Drawing.Point(12, 60);
            this.rbtRangeSelect.Name = "rbtRangeSelect";
            this.rbtRangeSelect.Size = new System.Drawing.Size(93, 16);
            this.rbtRangeSelect.TabIndex = 2;
            this.rbtRangeSelect.Text = "ページ指定(&G)";
            this.rbtRangeSelect.UseVisualStyleBackColor = true;
            // 
            // rbtRangeCurrent
            // 
            this.rbtRangeCurrent.AutoSize = true;
            this.rbtRangeCurrent.Enabled = false;
            this.rbtRangeCurrent.Location = new System.Drawing.Point(12, 40);
            this.rbtRangeCurrent.Name = "rbtRangeCurrent";
            this.rbtRangeCurrent.Size = new System.Drawing.Size(103, 16);
            this.rbtRangeCurrent.TabIndex = 1;
            this.rbtRangeCurrent.Text = "現在のページ(&U)";
            this.rbtRangeCurrent.UseVisualStyleBackColor = true;
            // 
            // rbtAllRange
            // 
            this.rbtAllRange.AutoSize = true;
            this.rbtAllRange.Checked = true;
            this.rbtAllRange.Location = new System.Drawing.Point(12, 20);
            this.rbtAllRange.Name = "rbtAllRange";
            this.rbtAllRange.Size = new System.Drawing.Size(66, 16);
            this.rbtAllRange.TabIndex = 0;
            this.rbtAllRange.TabStop = true;
            this.rbtAllRange.Text = "すべて(&L)";
            this.rbtAllRange.UseVisualStyleBackColor = true;
            // 
            // grbPageCopies
            // 
            this.grbPageCopies.Controls.Add(this.chkCollated);
            this.grbPageCopies.Controls.Add(this.label4);
            this.grbPageCopies.Controls.Add(this.nudCopies);
            this.grbPageCopies.Location = new System.Drawing.Point(264, 4);
            this.grbPageCopies.Name = "grbPageCopies";
            this.grbPageCopies.Size = new System.Drawing.Size(200, 88);
            this.grbPageCopies.TabIndex = 1;
            this.grbPageCopies.TabStop = false;
            // 
            // chkCollated
            // 
            this.chkCollated.AutoSize = true;
            this.chkCollated.Checked = true;
            this.chkCollated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCollated.Location = new System.Drawing.Point(76, 56);
            this.chkCollated.Name = "chkCollated";
            this.chkCollated.Size = new System.Drawing.Size(110, 16);
            this.chkCollated.TabIndex = 2;
            this.chkCollated.Text = "部単位で印刷(&O)";
            this.chkCollated.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "部数(&C):";
            // 
            // nudCopies
            // 
            this.nudCopies.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nudCopies.Location = new System.Drawing.Point(76, 24);
            this.nudCopies.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCopies.Name = "nudCopies";
            this.nudCopies.Size = new System.Drawing.Size(112, 19);
            this.nudCopies.TabIndex = 1;
            this.nudCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PrinterChoiceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(478, 329);
            this.Controls.Add(this.plnRangeCopies);
            this.Controls.Add(this.plnButtons);
            this.Controls.Add(this.plnBasic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrinterChoiceDialog";
            this.Text = "プリンタの設定";
            this.Load += new System.EventHandler(this.PrinterChoiceDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbDuplexAllign.ResumeLayout(false);
            this.grbDuplexAllign.PerformLayout();
            this.plnBasic.ResumeLayout(false);
            this.plnButtons.ResumeLayout(false);
            this.plnRangeCopies.ResumeLayout(false);
            this.grbPageRange.ResumeLayout(false);
            this.grbPageRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRangeTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRangeFrom)).EndInit();
            this.grbPageCopies.ResumeLayout(false);
            this.grbPageCopies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCopies)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboPrinterPaperSources;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textPaperName;
        private System.Windows.Forms.Button btnShowPaperSetting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboInstalledPrinters;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel plnBasic;
        private System.Windows.Forms.Panel plnButtons;
        private System.Windows.Forms.Panel plnRangeCopies;
        private System.Windows.Forms.GroupBox grbPageCopies;
        private System.Windows.Forms.GroupBox grbPageRange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudCopies;
        private System.Windows.Forms.RadioButton rbtRangeSelect;
        private System.Windows.Forms.RadioButton rbtRangeCurrent;
        private System.Windows.Forms.RadioButton rbtAllRange;
        private System.Windows.Forms.CheckBox chkCollated;
        private System.Windows.Forms.NumericUpDown nudRangeTo;
        private System.Windows.Forms.NumericUpDown nudRangeFrom;
        private System.Windows.Forms.CheckBox chkUseMono;
        private System.Windows.Forms.CheckBox chkUseDuplex;
        private System.Windows.Forms.GroupBox grbDuplexAllign;
        private System.Windows.Forms.RadioButton rbtDuplexHorizontal;
        private System.Windows.Forms.RadioButton rbtDuplexVertical;
    }
}