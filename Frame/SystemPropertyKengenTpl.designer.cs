namespace Jpsys.SagyoManage.Frame
{          
    [System.ComponentModel.ToolboxItem(true)]
    partial class SystemPropertyKengenTpl
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
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.KengenName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.Kengen = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            // 
            // Row
            // 
            this.Row.Cells.Add(this.Kengen);
            this.Row.Cells.Add(this.KengenName);
            cellStyle3.EditingBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle3.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            cellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Row.DefaultCellStyle = cellStyle3;
            cellStyle4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Row.DefaultHeaderCellStyle = cellStyle4;
            this.Row.Height = 28;
            this.Row.Width = 260;
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
            this.columnHeaderSection1.Width = 260;
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
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle5.Border = border3;
            cellStyle5.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell2.Style = cellStyle5;
            this.columnHeaderCell2.TabIndex = 0;
            this.columnHeaderCell2.Value = "権限";
            // 
            // columnHeaderCell1
            // 
            this.columnHeaderCell1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell1.Location = new System.Drawing.Point(100, 0);
            this.columnHeaderCell1.Name = "columnHeaderCell1";
            this.columnHeaderCell1.ResizeMode = GrapeCity.Win.MultiRow.ResizeMode.None;
            this.columnHeaderCell1.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.None;
            this.columnHeaderCell1.Size = new System.Drawing.Size(160, 28);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle6.Border = border4;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell1.Style = cellStyle6;
            this.columnHeaderCell1.TabIndex = 1;
            this.columnHeaderCell1.Value = "名称 *";
            // 
            // KengenName
            // 
            this.KengenName.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KengenName.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KengenName.Location = new System.Drawing.Point(100, 0);
            this.KengenName.MaxLength = 20;
            this.KengenName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.KengenName.Name = "KengenName";
            this.KengenName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.KengenName.Size = new System.Drawing.Size(160, 28);
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle2.Border = border2;
            cellStyle2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            cellStyle2.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.KengenName.Style = cellStyle2;
            this.KengenName.TabIndex = 1;
            this.KengenName.Value = "12345678901234567890";
            // 
            // Kengen
            // 
            this.Kengen.AutoComplete.CandidateListItemFont = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Kengen.AutoComplete.HighlightStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Kengen.Location = new System.Drawing.Point(0, 0);
            this.Kengen.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.Kengen.Name = "Kengen";
            this.Kengen.Selectable = false;
            this.Kengen.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.Kengen.Size = new System.Drawing.Size(100, 28);
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle1.Border = border1;
            cellStyle1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            cellStyle1.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.Kengen.Style = cellStyle1;
            this.Kengen.TabIndex = 2;
            this.Kengen.Value = "012345678901234567890123456789";
            // 
            // SystemPropertyKengenTpl
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 56;
            this.Width = 260;

        }
        

        #endregion

        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell2;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell1;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell KengenName;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell Kengen;
        
    }
}
