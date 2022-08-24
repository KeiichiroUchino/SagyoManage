namespace Jpsys.SagyoManage.Frame
{
    [System.ComponentModel.ToolboxItem(true)]
    partial class SagyoinWariateMeisaiTpl
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
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle9 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border6 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField numberSignDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberSignDisplayField();
            GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField numberIntegerPartDisplayField1 = new GrapeCity.Win.MultiRow.InputMan.NumberIntegerPartDisplayField();
            GrapeCity.Win.MultiRow.InputMan.SideButton sideButton1 = new GrapeCity.Win.MultiRow.InputMan.SideButton();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell13 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.HeaderMei2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell29 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.StaffName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.StaffCode = new GrapeCity.Win.MultiRow.InputMan.GcNumberCell(false);
            this.RowNo = new GrapeCity.Win.MultiRow.RowHeaderCell();
            this.ShuJitsuFlg = new GrapeCity.Win.MultiRow.CheckBoxCell();
            this.StaffKbnName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.IsEdited = new GrapeCity.Win.MultiRow.CheckBoxCell();
            // 
            // Row
            // 
            this.Row.Cells.Add(this.IsEdited);
            this.Row.Cells.Add(this.StaffName);
            this.Row.Cells.Add(this.StaffCode);
            this.Row.Cells.Add(this.RowNo);
            this.Row.Cells.Add(this.ShuJitsuFlg);
            this.Row.Cells.Add(this.StaffKbnName);
            this.Row.Height = 20;
            this.Row.Width = 375;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell13);
            this.columnHeaderSection1.Cells.Add(this.HeaderMei2);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell29);
            this.columnHeaderSection1.Height = 20;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 375;
            // 
            // columnHeaderCell13
            // 
            this.columnHeaderCell13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell13.Location = new System.Drawing.Point(31, 0);
            this.columnHeaderCell13.Name = "columnHeaderCell13";
            this.columnHeaderCell13.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell13.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell13.Size = new System.Drawing.Size(304, 20);
            cellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle7.Border = border4;
            cellStyle7.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle7.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell13.Style = cellStyle7;
            this.columnHeaderCell13.TabIndex = 0;
            this.columnHeaderCell13.Value = "作業員";
            // 
            // HeaderMei2
            // 
            this.HeaderMei2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeaderMei2.Location = new System.Drawing.Point(0, 0);
            this.HeaderMei2.Name = "HeaderMei2";
            this.HeaderMei2.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.HeaderMei2.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.HeaderMei2.Size = new System.Drawing.Size(31, 20);
            cellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border5.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle8.Border = border5;
            cellStyle8.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle8.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.HeaderMei2.Style = cellStyle8;
            this.HeaderMei2.TabIndex = 1;
            this.HeaderMei2.Value = "No.";
            // 
            // columnHeaderCell29
            // 
            this.columnHeaderCell29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell29.Location = new System.Drawing.Point(335, 0);
            this.columnHeaderCell29.Name = "columnHeaderCell29";
            this.columnHeaderCell29.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell29.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell29.Size = new System.Drawing.Size(40, 20);
            cellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border6.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle9.Border = border6;
            cellStyle9.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle9.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell29.Style = cellStyle9;
            this.columnHeaderCell29.TabIndex = 2;
            this.columnHeaderCell29.Value = "終日";
            // 
            // StaffName
            // 
            this.StaffName.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StaffName.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StaffName.ExitOnArrowKey = true;
            this.StaffName.Location = new System.Drawing.Point(105, 0);
            this.StaffName.MaxLength = 256;
            this.StaffName.Name = "StaffName";
            this.StaffName.ReadOnly = true;
            this.StaffName.Selectable = false;
            this.StaffName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.StaffName.Size = new System.Drawing.Size(150, 20);
            cellStyle2.BackColor = System.Drawing.SystemColors.Control;
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle2.Border = border1;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle2.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.StaffName.Style = cellStyle2;
            this.StaffName.TabIndex = 0;
            this.StaffName.TabStop = false;
            this.StaffName.Value = "12345678901234567890";
            // 
            // StaffCode
            // 
            this.StaffCode.AllowDeleteToNull = true;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.StaffCode.DisplayFields.Add(numberSignDisplayField1);
            this.StaffCode.DisplayFields.Add(numberIntegerPartDisplayField1);
            this.StaffCode.ExitOnArrowKey = true;
            this.StaffCode.Fields.DecimalPart.MaxDigits = 0;
            this.StaffCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.StaffCode.Fields.IntegerPart.MaxDigits = 9;
            this.StaffCode.Location = new System.Drawing.Point(31, 0);
            this.StaffCode.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.StaffCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.StaffCode.Name = "StaffCode";
            this.StaffCode.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "SetZero"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.Subtract, "SwitchSign"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.OemMinus, "SwitchSign"),
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return))), "ApplyRecommendedValue")});
            sideButton1.ButtonWidth = 22;
            sideButton1.Text = "...";
            this.StaffCode.SideButtons.Add(sideButton1);
            this.StaffCode.Size = new System.Drawing.Size(74, 20);
            this.StaffCode.Spin.SpinOnKeys = false;
            cellStyle3.BackColor = System.Drawing.Color.LightYellow;
            cellStyle3.Format = "#";
            cellStyle3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle3.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
            this.StaffCode.Style = cellStyle3;
            this.StaffCode.TabIndex = 1;
            this.StaffCode.Value = new decimal(new int[] {
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
            // ShuJitsuFlg
            // 
            this.ShuJitsuFlg.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ShuJitsuFlg.Location = new System.Drawing.Point(335, 0);
            this.ShuJitsuFlg.Name = "ShuJitsuFlg";
            this.ShuJitsuFlg.Size = new System.Drawing.Size(40, 20);
            cellStyle5.ForeColor = System.Drawing.Color.Red;
            cellStyle5.ImeMode = System.Windows.Forms.ImeMode.Off;
            cellStyle5.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            this.ShuJitsuFlg.Style = cellStyle5;
            this.ShuJitsuFlg.TabIndex = 3;
            this.ShuJitsuFlg.TabStop = false;
            this.ShuJitsuFlg.Text = "";
            // 
            // StaffKbnName
            // 
            this.StaffKbnName.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StaffKbnName.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StaffKbnName.ExitOnArrowKey = true;
            this.StaffKbnName.Location = new System.Drawing.Point(255, 0);
            this.StaffKbnName.MaxLength = 256;
            this.StaffKbnName.Name = "StaffKbnName";
            this.StaffKbnName.ReadOnly = true;
            this.StaffKbnName.Selectable = false;
            this.StaffKbnName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.StaffKbnName.Size = new System.Drawing.Size(80, 20);
            cellStyle6.BackColor = System.Drawing.SystemColors.Control;
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle6.Border = border3;
            cellStyle6.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.StaffKbnName.Style = cellStyle6;
            this.StaffKbnName.TabIndex = 4;
            this.StaffKbnName.TabStop = false;
            this.StaffKbnName.Value = "1234567890";
            // 
            // IsEdited
            // 
            this.IsEdited.Location = new System.Drawing.Point(302, 0);
            this.IsEdited.Name = "IsEdited";
            this.IsEdited.Selectable = false;
            this.IsEdited.Size = new System.Drawing.Size(32, 21);
            cellStyle1.BackColor = System.Drawing.Color.White;
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.IsEdited.Style = cellStyle1;
            this.IsEdited.TabIndex = 5;
            this.IsEdited.TabStop = false;
            // 
            // SagyoinWariateMeisaiTpl
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 40;
            this.Width = 375;

        }

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell StaffName;
        private GrapeCity.Win.MultiRow.InputMan.GcNumberCell StaffCode;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell13;
        private GrapeCity.Win.MultiRow.RowHeaderCell RowNo;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell HeaderMei2;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell29;
        private GrapeCity.Win.MultiRow.CheckBoxCell ShuJitsuFlg;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell StaffKbnName;
        private GrapeCity.Win.MultiRow.CheckBoxCell IsEdited;
    }
}
