namespace Jpsys.HaishaManageV10.Frame
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class HanroGroupMeisaiTpl
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle9 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle10 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField numberSignDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField();
            GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField numberIntegerPartDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField();
            GrapeCity.Win.MultiRow.InputMan.SideButton sideButton1 = new GrapeCity.Win.MultiRow.InputMan.SideButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.HeaderMei1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.HeaderMei3 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.HanroCode = new GrapeCity.Win.MultiRow.InputMan.GcNumberCell(false);
            this.HanroName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.RowNumber = new GrapeCity.Win.MultiRow.RowHeaderCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.HanroCode);
            this.Row.Cells.Add(this.HanroName);
            this.Row.Cells.Add(this.RowNumber);
            cellStyle4.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle4.EditingForeColor = System.Drawing.Color.Black;
            cellStyle4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.Row.DefaultCellStyle = cellStyle4;
            cellStyle5.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Row.DefaultHeaderCellStyle = cellStyle5;
            this.Row.Height = 20;
            this.Row.Width = 480;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell1);
            this.columnHeaderSection1.Cells.Add(this.HeaderMei1);
            this.columnHeaderSection1.Cells.Add(this.HeaderMei3);
            cellStyle9.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.columnHeaderSection1.DefaultCellStyle = cellStyle9;
            cellStyle10.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.columnHeaderSection1.DefaultHeaderCellStyle = cellStyle10;
            this.columnHeaderSection1.Height = 20;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 480;
            // 
            // columnHeaderCell1
            // 
            this.columnHeaderCell1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell1.Location = new System.Drawing.Point(0, 0);
            this.columnHeaderCell1.Name = "columnHeaderCell1";
            this.columnHeaderCell1.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell1.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell1.Size = new System.Drawing.Size(40, 20);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle6.Border = border2;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell1.Style = cellStyle6;
            this.columnHeaderCell1.TabIndex = 1;
            // 
            // HeaderMei1
            // 
            this.HeaderMei1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderMei1.Location = new System.Drawing.Point(40, 0);
            this.HeaderMei1.Name = "HeaderMei1";
            this.HeaderMei1.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.HeaderMei1.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.HeaderMei1.Size = new System.Drawing.Size(96, 20);
            cellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle7.Border = border3;
            cellStyle7.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.HeaderMei1.Style = cellStyle7;
            this.HeaderMei1.TabIndex = 2;
            this.HeaderMei1.Value = "コード";
            // 
            // HeaderMei3
            // 
            this.HeaderMei3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderMei3.Location = new System.Drawing.Point(136, 0);
            this.HeaderMei3.Name = "HeaderMei3";
            this.HeaderMei3.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.HeaderMei3.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.HeaderMei3.Size = new System.Drawing.Size(344, 20);
            cellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle8.Border = border4;
            cellStyle8.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.HeaderMei3.Style = cellStyle8;
            this.HeaderMei3.TabIndex = 4;
            this.HeaderMei3.Value = "得意先名";
            // 
            // HanroCode
            // 
            this.HanroCode.AllowDeleteToNull = true;
            this.HanroCode.ContentPadding = new System.Windows.Forms.Padding(1, 1, 3, 1);
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            this.HanroCode.DisplayFields.Add(numberSignDisplayField1);
            this.HanroCode.DisplayFields.Add(numberIntegerPartDisplayField1);
            this.HanroCode.DropDown.AllowDrop = false;
            this.HanroCode.ExitOnArrowKey = true;
            this.HanroCode.Fields.DecimalPart.MaxDigits = 0;
            this.HanroCode.Fields.IntegerPart.GroupSeparator = '\0';
            this.HanroCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.HanroCode.Fields.IntegerPart.MaxDigits = 8;
            this.HanroCode.Fields.IntegerPart.MinDigits = 1;
            this.HanroCode.HighlightText = true;
            this.HanroCode.Location = new System.Drawing.Point(40, 0);
            this.HanroCode.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.HanroCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.HanroCode.Name = "HanroCode";
            sideButton1.ButtonWidth = 22;
            sideButton1.Name = "sbtnHanroCode";
            sideButton1.Text = "...";
            this.HanroCode.SideButtons.Add(sideButton1);
            this.HanroCode.Size = new System.Drawing.Size(96, 20);
            this.HanroCode.Spin.AllowSpin = false;
            cellStyle1.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle1.EditingForeColor = System.Drawing.Color.Black;
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle1.Padding = new System.Windows.Forms.Padding(0);
            cellStyle1.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.HanroCode.Style = cellStyle1;
            this.HanroCode.TabIndex = 1;
            this.HanroCode.UseNegativeColor = false;
            this.HanroCode.Value = new decimal(new int[] {
            12345678,
            0,
            0,
            0});
            this.HanroCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // HanroName
            // 
            this.HanroName.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HanroName.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HanroName.ContentPadding = new System.Windows.Forms.Padding(3, 1, 1, 1);
            this.HanroName.DropDown.AllowDrop = false;
            this.HanroName.Location = new System.Drawing.Point(136, 0);
            this.HanroName.MaxLength = 60;
            this.HanroName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.HanroName.Name = "HanroName";
            this.HanroName.ReadOnly = true;
            this.HanroName.Selectable = false;
            this.HanroName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.HanroName.Size = new System.Drawing.Size(344, 20);
            cellStyle2.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle2.EditingForeColor = System.Drawing.Color.Black;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.HanroName.Style = cellStyle2;
            this.HanroName.TabIndex = 3;
            this.HanroName.TabStop = false;
            this.HanroName.Value = "フィード・ワン";
            // 
            // RowNumber
            // 
            this.RowNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RowNumber.Location = new System.Drawing.Point(0, 0);
            this.RowNumber.Name = "RowNumber";
            this.RowNumber.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.RowNumber.ShowIndicator = false;
            this.RowNumber.Size = new System.Drawing.Size(40, 20);
            cellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle3.Border = border1;
            cellStyle3.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.RowNumber.Style = cellStyle3;
            this.RowNumber.TabIndex = 0;
            this.RowNumber.TabStop = false;
            this.RowNumber.ValueFormat = "%1%";
            // 
            // HanroGroupMeisaiTpl
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 40;
            this.Width = 480;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell HeaderMei1;
        private GrapeCity.Win.MultiRow.InputMan.GcNumberCell HanroCode;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell HeaderMei3;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell HanroName;
        private GrapeCity.Win.MultiRow.RowHeaderCell RowNumber;

    }
}
