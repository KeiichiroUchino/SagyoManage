namespace Jpsys.SagyoManage.Frame
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class CarWariateMeisaiTpl
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField numberSignDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField();
            GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField numberIntegerPartDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField();
            GrapeCity.Win.MultiRow.InputMan.SideButton sideButton1 = new GrapeCity.Win.MultiRow.InputMan.SideButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell13 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.HeaderMei2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.LicPlateCarNo = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.CarCode = new GrapeCity.Win.MultiRow.InputMan.GcNumberCell(false);
            this.RowNo = new GrapeCity.Win.MultiRow.RowHeaderCell();
            this.CarName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.IsEdited = new GrapeCity.Win.MultiRow.CheckBoxCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.IsEdited);
            this.Row.Cells.Add(this.LicPlateCarNo);
            this.Row.Cells.Add(this.CarCode);
            this.Row.Cells.Add(this.RowNo);
            this.Row.Cells.Add(this.CarName);
            this.Row.Height = 20;
            this.Row.Width = 366;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell13);
            this.columnHeaderSection1.Cells.Add(this.HeaderMei2);
            this.columnHeaderSection1.Height = 20;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 366;
            // 
            // columnHeaderCell13
            // 
            this.columnHeaderCell13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell13.Location = new System.Drawing.Point(31, 0);
            this.columnHeaderCell13.Name = "columnHeaderCell13";
            this.columnHeaderCell13.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell13.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell13.Size = new System.Drawing.Size(335, 20);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle6.Border = border4;
            cellStyle6.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell13.Style = cellStyle6;
            this.columnHeaderCell13.TabIndex = 0;
            this.columnHeaderCell13.Value = "車両";
            // 
            // HeaderMei2
            // 
            this.HeaderMei2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderMei2.Location = new System.Drawing.Point(0, 0);
            this.HeaderMei2.Name = "HeaderMei2";
            this.HeaderMei2.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.HeaderMei2.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.HeaderMei2.Size = new System.Drawing.Size(31, 20);
            cellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border5.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle7.Border = border5;
            cellStyle7.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle7.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.HeaderMei2.Style = cellStyle7;
            this.HeaderMei2.TabIndex = 1;
            this.HeaderMei2.Value = "No.";
            // 
            // LicPlateCarNo
            // 
            this.LicPlateCarNo.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LicPlateCarNo.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LicPlateCarNo.ExitOnArrowKey = true;
            this.LicPlateCarNo.Location = new System.Drawing.Point(105, 0);
            this.LicPlateCarNo.MaxLength = 256;
            this.LicPlateCarNo.Name = "LicPlateCarNo";
            this.LicPlateCarNo.ReadOnly = true;
            this.LicPlateCarNo.Selectable = false;
            this.LicPlateCarNo.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.LicPlateCarNo.Size = new System.Drawing.Size(37, 20);
            cellStyle2.BackColor = System.Drawing.SystemColors.Control;
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle2.Border = border1;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle2.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.LicPlateCarNo.Style = cellStyle2;
            this.LicPlateCarNo.TabIndex = 0;
            this.LicPlateCarNo.TabStop = false;
            this.LicPlateCarNo.Value = "12345";
            // 
            // CarCode
            // 
            this.CarCode.AllowDeleteToNull = true;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.CarCode.DisplayFields.Add(numberSignDisplayField1);
            this.CarCode.DisplayFields.Add(numberIntegerPartDisplayField1);
            this.CarCode.ExitOnArrowKey = true;
            this.CarCode.Fields.DecimalPart.MaxDigits = 0;
            this.CarCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.CarCode.Fields.IntegerPart.MaxDigits = 9;
            this.CarCode.Location = new System.Drawing.Point(31, 0);
            this.CarCode.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.CarCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CarCode.Name = "CarCode";
            this.CarCode.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "SetZero"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.Subtract, "SwitchSign"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.OemMinus, "SwitchSign"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return))), "ApplyRecommendedValue")});
            sideButton1.ButtonWidth = 22;
            sideButton1.Text = "...";
            this.CarCode.SideButtons.Add(sideButton1);
            this.CarCode.Size = new System.Drawing.Size(74, 20);
            this.CarCode.Spin.SpinOnKeys = false;
            cellStyle3.BackColor = System.Drawing.Color.LightYellow;
            cellStyle3.Format = "#";
            cellStyle3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle3.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.CarCode.Style = cellStyle3;
            this.CarCode.TabIndex = 1;
            this.CarCode.Value = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            // 
            // RowNo
            // 
            this.RowNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RowNo.Location = new System.Drawing.Point(0, 0);
            this.RowNo.Name = "RowNo";
            this.RowNo.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.RowNo.ShowIndicator = false;
            this.RowNo.Size = new System.Drawing.Size(31, 20);
            cellStyle4.BackColor = System.Drawing.Color.White;
            border2.Bottom = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            border2.Left = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            border2.Right = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            border2.Top = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle4.Border = border2;
            cellStyle4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            cellStyle4.ImageAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            cellStyle4.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            cellStyle4.TextAngle = 0F;
            cellStyle4.TextEffect = GrapeCity.Win.MultiRow.TextEffect.Flat;
            cellStyle4.TextImageRelation = GrapeCity.Win.MultiRow.MultiRowTextImageRelation.ImageBeforeText;
            cellStyle4.TextIndent = 0;
            cellStyle4.WordWrap = GrapeCity.Win.MultiRow.MultiRowTriState.False;
            this.RowNo.Style = cellStyle4;
            this.RowNo.TabIndex = 2;
            this.RowNo.TabStop = false;
            this.RowNo.ValueFormat = "%1%";
            // 
            // CarName
            // 
            this.CarName.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CarName.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CarName.ExitOnArrowKey = true;
            this.CarName.Location = new System.Drawing.Point(142, 0);
            this.CarName.MaxLength = 256;
            this.CarName.Name = "CarName";
            this.CarName.ReadOnly = true;
            this.CarName.Selectable = false;
            this.CarName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.CarName.Size = new System.Drawing.Size(224, 20);
            cellStyle5.BackColor = System.Drawing.SystemColors.Control;
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle5.Border = border3;
            cellStyle5.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle5.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.CarName.Style = cellStyle5;
            this.CarName.TabIndex = 3;
            this.CarName.TabStop = false;
            this.CarName.Value = "123456789012345678901234567890";
            // 
            // IsEdited
            // 
            this.IsEdited.Location = new System.Drawing.Point(167, 0);
            this.IsEdited.Name = "IsEdited";
            this.IsEdited.Selectable = false;
            this.IsEdited.Size = new System.Drawing.Size(32, 21);
            cellStyle1.BackColor = System.Drawing.Color.White;
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.IsEdited.Style = cellStyle1;
            this.IsEdited.TabIndex = 4;
            this.IsEdited.TabStop = false;
            // 
            // CarWariateMeisaiTpl
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 40;
            this.Width = 366;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell LicPlateCarNo;
        private GrapeCity.Win.MultiRow.InputMan.GcNumberCell CarCode;
        private GrapeCity.Win.MultiRow.RowHeaderCell RowNo;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell13;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell HeaderMei2;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell CarName;
        private GrapeCity.Win.MultiRow.CheckBoxCell IsEdited;
    }
}
