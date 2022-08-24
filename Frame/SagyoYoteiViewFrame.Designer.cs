namespace Jpsys.SagyoManage.Frame
{
    partial class SagyoYoteiViewFrame
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
            GrapeCity.Win.Editors.Fields.DateYearDisplayField dateYearDisplayField3 = new GrapeCity.Win.Editors.Fields.DateYearDisplayField();
            GrapeCity.Win.Editors.Fields.DateLiteralDisplayField dateLiteralDisplayField5 = new GrapeCity.Win.Editors.Fields.DateLiteralDisplayField();
            GrapeCity.Win.Editors.Fields.DateMonthDisplayField dateMonthDisplayField3 = new GrapeCity.Win.Editors.Fields.DateMonthDisplayField();
            GrapeCity.Win.Editors.Fields.DateLiteralDisplayField dateLiteralDisplayField6 = new GrapeCity.Win.Editors.Fields.DateLiteralDisplayField();
            GrapeCity.Win.Editors.Fields.DateDayDisplayField dateDayDisplayField3 = new GrapeCity.Win.Editors.Fields.DateDayDisplayField();
            GrapeCity.Win.Editors.Fields.DateYearField dateYearField3 = new GrapeCity.Win.Editors.Fields.DateYearField();
            GrapeCity.Win.Editors.Fields.DateLiteralField dateLiteralField5 = new GrapeCity.Win.Editors.Fields.DateLiteralField();
            GrapeCity.Win.Editors.Fields.DateMonthField dateMonthField3 = new GrapeCity.Win.Editors.Fields.DateMonthField();
            GrapeCity.Win.Editors.Fields.DateLiteralField dateLiteralField6 = new GrapeCity.Win.Editors.Fields.DateLiteralField();
            GrapeCity.Win.Editors.Fields.DateDayField dateDayField3 = new GrapeCity.Win.Editors.Fields.DateDayField();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.dteSagyoYMD = new Jpsys.SagyoManage.Frame.NskDateTime(this.components);
            this.spinButton1 = new GrapeCity.Win.Editors.SpinButton();
            this.dropDownButton6 = new GrapeCity.Win.Editors.DropDownButton();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAllFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label168 = new System.Windows.Forms.Label();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.sagyoYoteiViewTpl1 = new Jpsys.SagyoManage.Frame.SagyoYoteiViewTpl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripClose = new System.Windows.Forms.ToolStripStatusLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton2 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton3 = new GrapeCity.Win.Editors.DropDownButton();
            this.pnlMid = new System.Windows.Forms.Panel();
            this.sbtnSeikyusakiCode = new GrapeCity.Win.Editors.SideButton();
            this.mrsSagyoYoteiList1 = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dteSagyoYMD)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpDetail.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlMid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mrsSagyoYoteiList1)).BeginInit();
            this.SuspendLayout();
            // 
            // dteSagyoYMD
            // 
            this.dteSagyoYMD.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dteSagyoYMD.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            dateYearDisplayField3.ShowLeadingZero = true;
            dateLiteralDisplayField5.Text = "/";
            dateMonthDisplayField3.ShowLeadingZero = true;
            dateLiteralDisplayField6.Text = "/";
            dateDayDisplayField3.ShowLeadingZero = true;
            this.dteSagyoYMD.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.DateDisplayField[] {
            dateYearDisplayField3,
            dateLiteralDisplayField5,
            dateMonthDisplayField3,
            dateLiteralDisplayField6,
            dateDayDisplayField3});
            this.dteSagyoYMD.DropDownCalendar.Weekdays = new GrapeCity.Win.Editors.WeekdaysStyle(new GrapeCity.Win.Editors.DayOfWeekStyle("日", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Red, false, false), ((GrapeCity.Win.Editors.WeekFlags)((((((GrapeCity.Win.Editors.WeekFlags.FirstWeek | GrapeCity.Win.Editors.WeekFlags.SecondWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.ThirdWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FourthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FifthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.LastWeek)))), new GrapeCity.Win.Editors.DayOfWeekStyle("月", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("火", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("水", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("木", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("金", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, false, false), GrapeCity.Win.Editors.WeekFlags.None), new GrapeCity.Win.Editors.DayOfWeekStyle("土", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.Editors.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Blue, false, false), ((GrapeCity.Win.Editors.WeekFlags)((((((GrapeCity.Win.Editors.WeekFlags.FirstWeek | GrapeCity.Win.Editors.WeekFlags.SecondWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.ThirdWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FourthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.FifthWeek) 
                    | GrapeCity.Win.Editors.WeekFlags.LastWeek)))));
            this.dteSagyoYMD.EditMode = GrapeCity.Win.Editors.EditMode.Overwrite;
            dateLiteralField5.Text = "/";
            dateLiteralField6.Text = "/";
            this.dteSagyoYMD.Fields.AddRange(new GrapeCity.Win.Editors.Fields.DateField[] {
            dateYearField3,
            dateLiteralField5,
            dateMonthField3,
            dateLiteralField6,
            dateDayField3});
            this.dteSagyoYMD.HighlightText = GrapeCity.Win.Editors.HighlightText.Field;
            this.dteSagyoYMD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dteSagyoYMD.Location = new System.Drawing.Point(56, 16);
            this.dteSagyoYMD.MaxDate = new System.DateTime(2999, 12, 31, 23, 59, 59, 0);
            this.dteSagyoYMD.MinDate = new System.DateTime(1950, 1, 1, 0, 0, 0, 0);
            this.dteSagyoYMD.Name = "dteSagyoYMD";
            this.dteSagyoYMD.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.dteSagyoYMD, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.dteSagyoYMD)),
                ((object)(this.dteSagyoYMD)),
                ((object)(this.dteSagyoYMD)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextFieldThenControl",
                "PreviousFieldThenControl",
                "NextFieldThenControl",
                "PreviousControl"}));
            this.dteSagyoYMD.ShowRecommendedValue = true;
            this.dteSagyoYMD.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.spinButton1,
            this.dropDownButton6});
            this.dteSagyoYMD.Size = new System.Drawing.Size(110, 28);
            this.dteSagyoYMD.Spin.AllowSpin = false;
            this.dteSagyoYMD.TabAction = GrapeCity.Win.Editors.TabAction.Field;
            this.dteSagyoYMD.TabIndex = 3;
            this.dteSagyoYMD.Value = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
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
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.groupBox1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 24);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1420, 62);
            this.pnlTop.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.chkAllFlag);
            this.groupBox1.Controls.Add(this.label168);
            this.groupBox1.Controls.Add(this.dteSagyoYMD);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1400, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "検索条件";
            // 
            // chkAllFlag
            // 
            this.chkAllFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllFlag.AutoSize = true;
            this.chkAllFlag.Checked = true;
            this.chkAllFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllFlag.ForeColor = System.Drawing.Color.Black;
            this.chkAllFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAllFlag.Location = new System.Drawing.Point(1169, 54);
            this.chkAllFlag.Name = "chkAllFlag";
            this.chkAllFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkAllFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkAllFlag.Size = new System.Drawing.Size(99, 22);
            this.chkAllFlag.TabIndex = 1067;
            this.chkAllFlag.TabStop = false;
            this.chkAllFlag.Text = "使用停止表示";
            this.chkAllFlag.UseVisualStyleBackColor = true;
            // 
            // label168
            // 
            this.label168.AutoSize = true;
            this.label168.CausesValidation = false;
            this.label168.ForeColor = System.Drawing.Color.Black;
            this.label168.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label168.Location = new System.Drawing.Point(6, 21);
            this.label168.Name = "label168";
            this.label168.Size = new System.Drawing.Size(44, 18);
            this.label168.TabIndex = 1055;
            this.label168.Text = "作業日";
            this.label168.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpDetail
            // 
            this.grpDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDetail.Controls.Add(this.mrsSagyoYoteiList1);
            this.grpDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.grpDetail.Location = new System.Drawing.Point(8, 6);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(1400, 613);
            this.grpDetail.TabIndex = 0;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "検索結果";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripClose});
            this.statusStrip1.Location = new System.Drawing.Point(0, 753);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1420, 22);
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
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.CausesValidation = false;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1420, 42);
            this.label5.TabIndex = 82;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 711);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1420, 42);
            this.pnlBottom.TabIndex = 2;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(1420, 24);
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
            // pnlMid
            // 
            this.pnlMid.Controls.Add(this.grpDetail);
            this.pnlMid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMid.Location = new System.Drawing.Point(0, 86);
            this.pnlMid.Name = "pnlMid";
            this.pnlMid.Size = new System.Drawing.Size(1420, 625);
            this.pnlMid.TabIndex = 204;
            // 
            // sbtnSeikyusakiCode
            // 
            this.sbtnSeikyusakiCode.ButtonWidth = 22;
            this.sbtnSeikyusakiCode.Name = "sbtnSeikyusakiCode";
            this.sbtnSeikyusakiCode.Text = "...";
            this.sbtnSeikyusakiCode.Click += new System.EventHandler(this.sbtn_Click);
            // 
            // mrsSagyoYoteiList1
            // 
            this.mrsSagyoYoteiList1.AllowUserToResize = false;
            this.mrsSagyoYoteiList1.AllowUserToZoom = false;
            this.mrsSagyoYoteiList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mrsSagyoYoteiList1.Location = new System.Drawing.Point(9, 15);
            this.mrsSagyoYoteiList1.MultiSelect = false;
            this.mrsSagyoYoteiList1.Name = "mrsSagyoYoteiList1";
            this.mrsSagyoYoteiList1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mrsSagyoYoteiList1.ScrollBarStyle = System.Windows.Forms.FlatStyle.Standard;
            this.mrsSagyoYoteiList1.Size = new System.Drawing.Size(1385, 580);
            this.mrsSagyoYoteiList1.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.mrsSagyoYoteiList1.TabIndex = 6;
            this.mrsSagyoYoteiList1.Template = this.sagyoYoteiViewTpl1;
            this.mrsSagyoYoteiList1.Text = "gcMultiRow1";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(226, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 30);
            this.button1.TabIndex = 1068;
            this.button1.Text = "翌日";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(172, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 30);
            this.button2.TabIndex = 1069;
            this.button2.Text = "前日";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SagyoYoteiViewFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1420, 775);
            this.Controls.Add(this.pnlMid);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SagyoYoteiViewFrame";
            this.Text = "SagyoYoteiViewFrameFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JuchuIchiranFrame_FormClosing);
            this.Shown += new System.EventHandler(this.JuchuIchiranFrame_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.JuchuIchiranFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dteSagyoYMD)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlMid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mrsSagyoYoteiList1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton2;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Panel pnlMid;
        private System.Windows.Forms.ToolStripStatusLabel toolStripClose;
        private GrapeCity.Win.Editors.SideButton sbtnSeikyusakiCode;
        private System.Windows.Forms.Label label168;
        private NskDateTime dteSagyoYMD;
        private GrapeCity.Win.Editors.SpinButton spinButton1;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton6;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkAllFlag;
        private SagyoYoteiViewTpl sagyoYoteiViewTpl1;
        private GrapeCity.Win.MultiRow.GcMultiRow mrsSagyoYoteiList1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}