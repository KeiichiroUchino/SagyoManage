namespace Jpsys.SagyoManage.Frame
{
    partial class GekkanSagyoKeikakuhyouPrtToFrame
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
            GrapeCity.Win.Editors.Fields.DateYearDisplayField dateYearDisplayField1 = new GrapeCity.Win.Editors.Fields.DateYearDisplayField();
            GrapeCity.Win.Editors.Fields.DateLiteralDisplayField dateLiteralDisplayField1 = new GrapeCity.Win.Editors.Fields.DateLiteralDisplayField();
            GrapeCity.Win.Editors.Fields.DateMonthDisplayField dateMonthDisplayField1 = new GrapeCity.Win.Editors.Fields.DateMonthDisplayField();
            GrapeCity.Win.Editors.Fields.DateYearField dateYearField1 = new GrapeCity.Win.Editors.Fields.DateYearField();
            GrapeCity.Win.Editors.Fields.DateLiteralField dateLiteralField1 = new GrapeCity.Win.Editors.Fields.DateLiteralField();
            GrapeCity.Win.Editors.Fields.DateMonthField dateMonthField1 = new GrapeCity.Win.Editors.Fields.DateMonthField();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.dteShiteiYM = new Jpsys.SagyoManage.Frame.NskDateTime(this.components);
            this.spinButton1 = new GrapeCity.Win.Editors.SpinButton();
            this.dropDownButton6 = new GrapeCity.Win.Editors.DropDownButton();
            this.pnl = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label168 = new System.Windows.Forms.Label();
            this.toolStripPrintToPrinter = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripClose = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPrintToScreen = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnPrintToScreen = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnPrintToPrinter = new System.Windows.Forms.Button();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton2 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton3 = new GrapeCity.Win.Editors.DropDownButton();
            ((System.ComponentModel.ISupportInitialize)(this.dteShiteiYM)).BeginInit();
            this.pnl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dteShiteiYM
            // 
            this.dteShiteiYM.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dteShiteiYM.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            dateYearDisplayField1.ShowLeadingZero = true;
            dateLiteralDisplayField1.Text = "/";
            dateMonthDisplayField1.ShowLeadingZero = true;
            this.dteShiteiYM.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.DateDisplayField[] {
            dateYearDisplayField1,
            dateLiteralDisplayField1,
            dateMonthDisplayField1});
            this.dteShiteiYM.DropDownCalendar.Weekdays = new GrapeCity.Win.Editors.WeekdaysStyle(new GrapeCity.Win.Editors.DayOfWeekStyle("日", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Red, false, false), ((GrapeCity.Win.Editors.WeekFlags)((((((GrapeCity.Win.Editors.WeekFlags.FirstWeek | GrapeCity.Win.Editors.WeekFlags.SecondWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.ThirdWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FourthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FifthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.LastWeek)))), new GrapeCity.Win.Editors.DayOfWeekStyle("月", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("火", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("水", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("木", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("金", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("土", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Blue, false, false), ((GrapeCity.Win.Editors.WeekFlags)((((((GrapeCity.Win.Editors.WeekFlags.FirstWeek | GrapeCity.Win.Editors.WeekFlags.SecondWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.ThirdWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FourthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FifthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.LastWeek)))));
            this.dteShiteiYM.EditMode = GrapeCity.Win.Editors.EditMode.Overwrite;
            dateLiteralField1.Text = "/";
            this.dteShiteiYM.Fields.AddRange(new GrapeCity.Win.Editors.Fields.DateField[] {
            dateYearField1,
            dateLiteralField1,
            dateMonthField1});
            this.dteShiteiYM.HighlightText = GrapeCity.Win.Editors.HighlightText.Field;
            this.dteShiteiYM.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dteShiteiYM.Location = new System.Drawing.Point(65, 35);
            this.dteShiteiYM.MaxDate = new System.DateTime(2999, 12, 31, 23, 59, 59, 0);
            this.dteShiteiYM.MinDate = new System.DateTime(1950, 1, 1, 0, 0, 0, 0);
            this.dteShiteiYM.Name = "dteShiteiYM";
            this.dteShiteiYM.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.dteShiteiYM, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.dteShiteiYM)),
                ((object)(this.dteShiteiYM)),
                ((object)(this.dteShiteiYM)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextFieldThenControl",
                "PreviousFieldThenControl",
                "NextFieldThenControl",
                "PreviousControl"}));
            this.dteShiteiYM.ShowRecommendedValue = true;
            this.dteShiteiYM.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.spinButton1,
            this.dropDownButton6});
            this.dteShiteiYM.Size = new System.Drawing.Size(95, 28);
            this.dteShiteiYM.Spin.AllowSpin = false;
            this.dteShiteiYM.TabAction = GrapeCity.Win.Editors.TabAction.Field;
            this.dteShiteiYM.TabIndex = 1058;
            this.dteShiteiYM.Value = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            // 
            // spinButton1
            // 
            this.spinButton1.Name = "spinButton1";
            this.spinButton1.SpinUp += new System.EventHandler(this.ymd_Up);
            this.spinButton1.SpinDown += new System.EventHandler(this.ymd_Down);
            // 
            // dropDownButton6
            // 
            this.dropDownButton6.Name = "dropDownButton6";
            // 
            // pnl
            // 
            this.pnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl.Controls.Add(this.groupBox1);
            this.pnl.Location = new System.Drawing.Point(0, 24);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(378, 94);
            this.pnl.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label168);
            this.groupBox1.Controls.Add(this.dteShiteiYM);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "印刷条件を指定して下さい";
            // 
            // label168
            // 
            this.label168.AutoSize = true;
            this.label168.CausesValidation = false;
            this.label168.ForeColor = System.Drawing.Color.Black;
            this.label168.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label168.Location = new System.Drawing.Point(8, 40);
            this.label168.Name = "label168";
            this.label168.Size = new System.Drawing.Size(51, 18);
            this.label168.TabIndex = 1059;
            this.label168.Text = "指定月*";
            this.label168.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripPrintToPrinter
            // 
            this.toolStripPrintToPrinter.ForeColor = System.Drawing.Color.White;
            this.toolStripPrintToPrinter.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripPrintToPrinter.Name = "toolStripPrintToPrinter";
            this.toolStripPrintToPrinter.Size = new System.Drawing.Size(58, 20);
            this.toolStripPrintToPrinter.Text = "F7：印刷";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripClose,
            this.toolStripPrintToPrinter,
            this.toolStripPrintToScreen});
            this.statusStrip1.Location = new System.Drawing.Point(0, 174);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(378, 22);
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
            // toolStripPrintToScreen
            // 
            this.toolStripPrintToScreen.ForeColor = System.Drawing.Color.White;
            this.toolStripPrintToScreen.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripPrintToScreen.Name = "toolStripPrintToScreen";
            this.toolStripPrintToScreen.Size = new System.Drawing.Size(118, 20);
            this.toolStripPrintToScreen.Text = "F8：印刷プレビュー";
            // 
            // btnPrintToScreen
            // 
            this.btnPrintToScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintToScreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrintToScreen.Location = new System.Drawing.Point(261, 14);
            this.btnPrintToScreen.Name = "btnPrintToScreen";
            this.btnPrintToScreen.Size = new System.Drawing.Size(105, 30);
            this.btnPrintToScreen.TabIndex = 2;
            this.btnPrintToScreen.TabStop = false;
            this.btnPrintToScreen.Text = "プレビュー";
            this.btnPrintToScreen.UseVisualStyleBackColor = true;
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
            this.label5.Size = new System.Drawing.Size(378, 1);
            this.label5.TabIndex = 82;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnPrintToPrinter);
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.btnPrintToScreen);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 124);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(378, 50);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnPrintToPrinter
            // 
            this.btnPrintToPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintToPrinter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrintToPrinter.Location = new System.Drawing.Point(150, 14);
            this.btnPrintToPrinter.Name = "btnPrintToPrinter";
            this.btnPrintToPrinter.Size = new System.Drawing.Size(105, 30);
            this.btnPrintToPrinter.TabIndex = 83;
            this.btnPrintToPrinter.TabStop = false;
            this.btnPrintToPrinter.Text = "印刷";
            this.btnPrintToPrinter.UseVisualStyleBackColor = true;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(378, 24);
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
            // GekkanSagyoKeikakuhyouPrtToFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(378, 196);
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GekkanSagyoKeikakuhyouPrtToFrame";
            this.Text = "GekkanSagyoKeikakuhyouPrtToFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JuchuNyuryokuFrame_FormClosing);
            this.Shown += new System.EventHandler(this.JuchuNyuryokuFrame_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dteShiteiYM)).EndInit();
            this.pnl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPrintToPrinter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnPrintToScreen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton2;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label168;
        private NskDateTime dteShiteiYM;
        private GrapeCity.Win.Editors.SpinButton spinButton1;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton6;
        private System.Windows.Forms.Button btnPrintToPrinter;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPrintToScreen;
    }
}