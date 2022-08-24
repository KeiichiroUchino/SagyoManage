namespace Jpsys.SagyoManage.Frame
{          
    [System.ComponentModel.ToolboxItem(true)]
    partial class SystemPropertyTaxRateTpl
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

        #region MultiRow Template Designer generated code

		/// <summary> 
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
        private void InitializeComponent()
        {
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.InputMan.DateYearDisplayField dateYearDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.DateYearDisplayField();
            GrapeCity.Win.MultiRow.InputMan.DateLiteralDisplayField dateLiteralDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.DateLiteralDisplayField();
            GrapeCity.Win.MultiRow.InputMan.DateMonthDisplayField dateMonthDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.DateMonthDisplayField();
            GrapeCity.Win.MultiRow.InputMan.DateLiteralDisplayField dateLiteralDisplayField2 = new GrapeCity.Win.MultiRow.InputMan.DateLiteralDisplayField();
            GrapeCity.Win.MultiRow.InputMan.DateDayDisplayField dateDayDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.DateDayDisplayField();
            GrapeCity.Win.MultiRow.InputMan.WeekdaysStyle weekdaysStyle1 = new GrapeCity.Win.MultiRow.InputMan.WeekdaysStyle();
            GrapeCity.Win.MultiRow.InputMan.DateYearField dateYearField1 = new GrapeCity.Win.MultiRow.InputMan.DateYearField();
            GrapeCity.Win.MultiRow.InputMan.DateLiteralField dateLiteralField1 = new GrapeCity.Win.MultiRow.InputMan.DateLiteralField();
            GrapeCity.Win.MultiRow.InputMan.DateMonthField dateMonthField1 = new GrapeCity.Win.MultiRow.InputMan.DateMonthField();
            GrapeCity.Win.MultiRow.InputMan.DateLiteralField dateLiteralField2 = new GrapeCity.Win.MultiRow.InputMan.DateLiteralField();
            GrapeCity.Win.MultiRow.InputMan.DateDayField dateDayField1 = new GrapeCity.Win.MultiRow.InputMan.DateDayField();
            GrapeCity.Win.MultiRow.InputMan.DropDownButton dropDownButton1 = new GrapeCity.Win.MultiRow.InputMan.DropDownButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.RangeValidator rangeValidator1 = new GrapeCity.Win.MultiRow.RangeValidator();
            GrapeCity.Win.MultiRow.FocusProcess focusProcess1 = new GrapeCity.Win.MultiRow.FocusProcess();
            GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField numberSignDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField();
            GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField numberIntegerPartDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.RangeValidator rangeValidator2 = new GrapeCity.Win.MultiRow.RangeValidator();
            GrapeCity.Win.MultiRow.FocusProcess focusProcess2 = new GrapeCity.Win.MultiRow.FocusProcess();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.StartDate = new GrapeCity.Win.MultiRow.InputMan.GcDateTimeCell(false);
            this.TaxRate = new GrapeCity.Win.MultiRow.InputMan.GcNumberCell(false);
            // 
            // Row
            // 
            this.Row.Cells.Add(this.StartDate);
            this.Row.Cells.Add(this.TaxRate);
            cellStyle3.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle3.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Row.DefaultCellStyle = cellStyle3;
            cellStyle4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Row.DefaultHeaderCellStyle = cellStyle4;
            this.Row.Height = 28;
            this.Row.Width = 180;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell2);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell1);
            cellStyle7.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.columnHeaderSection1.DefaultCellStyle = cellStyle7;
            cellStyle8.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.columnHeaderSection1.DefaultHeaderCellStyle = cellStyle8;
            this.columnHeaderSection1.Height = 28;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 180;
            // 
            // columnHeaderCell2
            // 
            this.columnHeaderCell2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell2.Location = new System.Drawing.Point(0, 0);
            this.columnHeaderCell2.Name = "columnHeaderCell2";
            this.columnHeaderCell2.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell2.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell2.Size = new System.Drawing.Size(100, 28);
            cellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle5.Border = border2;
            cellStyle5.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell2.Style = cellStyle5;
            this.columnHeaderCell2.TabIndex = 0;
            this.columnHeaderCell2.Value = "適用開始日";
            // 
            // columnHeaderCell1
            // 
            this.columnHeaderCell1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell1.Location = new System.Drawing.Point(100, 0);
            this.columnHeaderCell1.Name = "columnHeaderCell1";
            this.columnHeaderCell1.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell1.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell1.Size = new System.Drawing.Size(80, 28);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle6.Border = border3;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell1.Style = cellStyle6;
            this.columnHeaderCell1.TabIndex = 1;
            this.columnHeaderCell1.Value = "税率 [%] *";
            // 
            // StartDate
            // 
            dateYearDisplayField1.ShowLeadingZero = true;
            dateLiteralDisplayField1.Text = "/";
            dateMonthDisplayField1.ShowLeadingZero = true;
            dateLiteralDisplayField2.Text = "/";
            dateDayDisplayField1.ShowLeadingZero = true;
            this.StartDate.DisplayFields.Add(dateYearDisplayField1);
            this.StartDate.DisplayFields.Add(dateLiteralDisplayField1);
            this.StartDate.DisplayFields.Add(dateMonthDisplayField1);
            this.StartDate.DisplayFields.Add(dateLiteralDisplayField2);
            this.StartDate.DisplayFields.Add(dateDayDisplayField1);
            weekdaysStyle1.Saturday = new GrapeCity.Win.MultiRow.InputMan.DayOfWeekStyle("土", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.MultiRow.InputMan.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Blue, false, false), ((GrapeCity.Win.Editors.WeekFlags)((((((GrapeCity.Win.Editors.WeekFlags.FirstWeek | GrapeCity.Win.Editors.WeekFlags.SecondWeek) 
                | GrapeCity.Win.Editors.WeekFlags.ThirdWeek) 
                | GrapeCity.Win.Editors.WeekFlags.FourthWeek) 
                | GrapeCity.Win.Editors.WeekFlags.FifthWeek) 
                | GrapeCity.Win.Editors.WeekFlags.LastWeek))));
            weekdaysStyle1.Sunday = new GrapeCity.Win.MultiRow.InputMan.DayOfWeekStyle("日", GrapeCity.Win.Editors.ReflectTitle.None, new GrapeCity.Win.MultiRow.InputMan.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Red, false, false), ((GrapeCity.Win.Editors.WeekFlags)((((((GrapeCity.Win.Editors.WeekFlags.FirstWeek | GrapeCity.Win.Editors.WeekFlags.SecondWeek) 
                | GrapeCity.Win.Editors.WeekFlags.ThirdWeek) 
                | GrapeCity.Win.Editors.WeekFlags.FourthWeek) 
                | GrapeCity.Win.Editors.WeekFlags.FifthWeek) 
                | GrapeCity.Win.Editors.WeekFlags.LastWeek))));
            this.StartDate.DropDownCalendar.Weekdays = weekdaysStyle1;
            this.StartDate.EditMode = GrapeCity.Win.Editors.EditMode.Overwrite;
            dateLiteralField1.Text = "/";
            dateLiteralField2.Text = "/";
            this.StartDate.Fields.Add(dateYearField1);
            this.StartDate.Fields.Add(dateLiteralField1);
            this.StartDate.Fields.Add(dateMonthField1);
            this.StartDate.Fields.Add(dateLiteralField2);
            this.StartDate.Fields.Add(dateDayField1);
            this.StartDate.HighlightText = GrapeCity.Win.Editors.HighlightText.Field;
            this.StartDate.Location = new System.Drawing.Point(0, 0);
            this.StartDate.Name = "StartDate";
            this.StartDate.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.Up, "PreviousFieldThenCellThenControl"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.Down, "NextFieldThenCellThenControl")});
            this.StartDate.SideButtons.Add(dropDownButton1);
            this.StartDate.Size = new System.Drawing.Size(100, 28);
            this.StartDate.Spin.AllowSpin = false;
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle1.Border = border1;
            cellStyle1.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle1.EditingForeColor = System.Drawing.Color.Black;
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            cellStyle1.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.StartDate.Style = cellStyle1;
            this.StartDate.TabIndex = 0;
            focusProcess1.DoActionReason = ((GrapeCity.Win.MultiRow.ValidateReasons)((((((GrapeCity.Win.MultiRow.ValidateReasons.CellValidating | GrapeCity.Win.MultiRow.ValidateReasons.RowValidating) 
            | GrapeCity.Win.MultiRow.ValidateReasons.EndEdit) 
            | GrapeCity.Win.MultiRow.ValidateReasons.CancelEdit) 
            | GrapeCity.Win.MultiRow.ValidateReasons.CancelRow) 
            | GrapeCity.Win.MultiRow.ValidateReasons.EditedFormattedValueChanged)));
            rangeValidator1.Actions.Add(focusProcess1);
            rangeValidator1.MaxValue = new System.DateTime(2099, 12, 31, 0, 0, 0, 0);
            rangeValidator1.MinValue = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.StartDate.Validators.Add(rangeValidator1);
            this.StartDate.Value = new System.DateTime(2011, 10, 3, 0, 0, 0, 0);
            // 
            // TaxRate
            // 
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.TaxRate.DisplayFields.Add(numberSignDisplayField1);
            this.TaxRate.DisplayFields.Add(numberIntegerPartDisplayField1);
            this.TaxRate.DropDown.AllowDrop = false;
            this.TaxRate.EditMode = GrapeCity.Win.Editors.EditMode.Overwrite;
            this.TaxRate.ExitOnArrowKey = true;
            this.TaxRate.Fields.DecimalPart.MaxDigits = 0;
            this.TaxRate.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.TaxRate.Fields.IntegerPart.MaxDigits = 2;
            this.TaxRate.HighlightText = true;
            this.TaxRate.Location = new System.Drawing.Point(100, 0);
            this.TaxRate.Name = "TaxRate";
            this.TaxRate.Size = new System.Drawing.Size(80, 28);
            this.TaxRate.Spin.AllowSpin = false;
            cellStyle2.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle2.EditingForeColor = System.Drawing.Color.Black;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            cellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            cellStyle2.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.TaxRate.Style = cellStyle2;
            this.TaxRate.TabIndex = 1;
            focusProcess2.DoActionReason = ((GrapeCity.Win.MultiRow.ValidateReasons)((((((GrapeCity.Win.MultiRow.ValidateReasons.CellValidating | GrapeCity.Win.MultiRow.ValidateReasons.RowValidating) 
            | GrapeCity.Win.MultiRow.ValidateReasons.EndEdit) 
            | GrapeCity.Win.MultiRow.ValidateReasons.CancelEdit) 
            | GrapeCity.Win.MultiRow.ValidateReasons.CancelRow) 
            | GrapeCity.Win.MultiRow.ValidateReasons.EditedFormattedValueChanged)));
            rangeValidator2.Actions.Add(focusProcess2);
            rangeValidator2.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            rangeValidator2.MinValue = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.TaxRate.Validators.Add(rangeValidator2);
            this.TaxRate.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.TaxRate.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // SystemPropertyTaxRateTpl
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 56;
            this.Width = 180;

        }
        

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.InputMan.GcDateTimeCell StartDate;
        private GrapeCity.Win.MultiRow.InputMan.GcNumberCell TaxRate;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell2;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell1;
        
    }
}
