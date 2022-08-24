namespace Jpsys.HaishaManageV10.Frame
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class TokuisakiGroupMeisaiTpl
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
            this.TokuisakiCode = new GrapeCity.Win.MultiRow.InputMan.GcNumberCell(false);
            this.TokuisakiName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.RowNumber = new GrapeCity.Win.MultiRow.RowHeaderCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.TokuisakiCode);
            this.Row.Cells.Add(this.TokuisakiName);
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
            // TokuisakiCode
            // 
            this.TokuisakiCode.AllowDeleteToNull = true;
            this.TokuisakiCode.ContentPadding = new System.Windows.Forms.Padding(1, 1, 3, 1);
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            this.TokuisakiCode.DisplayFields.Add(numberSignDisplayField1);
            this.TokuisakiCode.DisplayFields.Add(numberIntegerPartDisplayField1);
            this.TokuisakiCode.DropDown.AllowDrop = false;
            this.TokuisakiCode.ExitOnArrowKey = true;
            this.TokuisakiCode.Fields.DecimalPart.MaxDigits = 0;
            this.TokuisakiCode.Fields.IntegerPart.GroupSeparator = '\0';
            this.TokuisakiCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.TokuisakiCode.Fields.IntegerPart.MaxDigits = 8;
            this.TokuisakiCode.Fields.IntegerPart.MinDigits = 1;
            this.TokuisakiCode.HighlightText = true;
            this.TokuisakiCode.Location = new System.Drawing.Point(40, 0);
            this.TokuisakiCode.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.TokuisakiCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TokuisakiCode.Name = "TokuisakiCode";
            sideButton1.ButtonWidth = 22;
            sideButton1.Name = "sbtnTokuisakiCode";
            sideButton1.Text = "...";
            this.TokuisakiCode.SideButtons.Add(sideButton1);
            this.TokuisakiCode.Size = new System.Drawing.Size(96, 20);
            this.TokuisakiCode.Spin.AllowSpin = false;
            cellStyle1.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle1.EditingForeColor = System.Drawing.Color.Black;
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle1.Padding = new System.Windows.Forms.Padding(0);
            cellStyle1.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.TokuisakiCode.Style = cellStyle1;
            this.TokuisakiCode.TabIndex = 1;
            this.TokuisakiCode.UseNegativeColor = false;
            this.TokuisakiCode.Value = new decimal(new int[] {
            12345678,
            0,
            0,
            0});
            this.TokuisakiCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // TokuisakiName
            // 
            this.TokuisakiName.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TokuisakiName.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TokuisakiName.ContentPadding = new System.Windows.Forms.Padding(3, 1, 1, 1);
            this.TokuisakiName.DropDown.AllowDrop = false;
            this.TokuisakiName.Location = new System.Drawing.Point(136, 0);
            this.TokuisakiName.MaxLength = 60;
            this.TokuisakiName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.TokuisakiName.Name = "TokuisakiName";
            this.TokuisakiName.ReadOnly = true;
            this.TokuisakiName.Selectable = false;
            this.TokuisakiName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.TokuisakiName.Size = new System.Drawing.Size(344, 20);
            cellStyle2.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle2.EditingForeColor = System.Drawing.Color.Black;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.TokuisakiName.Style = cellStyle2;
            this.TokuisakiName.TabIndex = 3;
            this.TokuisakiName.TabStop = false;
            this.TokuisakiName.Value = "フィード・ワン";
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
            // TokuisakiGroupMeisaiTpl
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
        private GrapeCity.Win.MultiRow.InputMan.GcNumberCell TokuisakiCode;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell HeaderMei3;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell TokuisakiName;
        private GrapeCity.Win.MultiRow.RowHeaderCell RowNumber;

    }
}
