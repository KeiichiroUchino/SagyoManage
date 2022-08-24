namespace Jpsys.HaishaManageV10.Frame
{          
    [System.ComponentModel.ToolboxItem(true)]
    partial class MenuAuthSettingTpl
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
            GrapeCity.Win.MultiRow.Border border1 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle2 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle3 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle7 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.CellStyle cellStyle8 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border5 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle4 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border2 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle5 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border3 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle6 = new GrapeCity.Win.MultiRow.CellStyle();
            GrapeCity.Win.MultiRow.Border border4 = new GrapeCity.Win.MultiRow.Border();
            GrapeCity.Win.MultiRow.CellStyle cellStyle1 = new GrapeCity.Win.MultiRow.CellStyle();
            this.columnHeaderSection1 = new GrapeCity.Win.MultiRow.ColumnHeaderSection();
            this.columnHeaderCell1 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell2 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.columnHeaderCell3 = new GrapeCity.Win.MultiRow.ColumnHeaderCell();
            this.TBunruiName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.MBunruiName = new GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell(false);
            this.AllowUseFlag = new GrapeCity.Win.MultiRow.CheckBoxCell();
            // 
            // Row
            // 
            border1.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.None, System.Drawing.Color.Black);
            this.Row.Border = border1;
            this.Row.Cells.Add(this.TBunruiName);
            this.Row.Cells.Add(this.MBunruiName);
            this.Row.Cells.Add(this.AllowUseFlag);
            cellStyle2.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle2.PatternColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Row.DefaultCellStyle = cellStyle2;
            cellStyle3.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Row.DefaultHeaderCellStyle = cellStyle3;
            this.Row.Height = 28;
            this.Row.Width = 474;
            // 
            // columnHeaderSection1
            // 
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell1);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell2);
            this.columnHeaderSection1.Cells.Add(this.columnHeaderCell3);
            cellStyle7.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.columnHeaderSection1.DefaultCellStyle = cellStyle7;
            border5.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle8.Border = border5;
            cellStyle8.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.columnHeaderSection1.DefaultHeaderCellStyle = cellStyle8;
            this.columnHeaderSection1.Height = 28;
            this.columnHeaderSection1.Name = "columnHeaderSection1";
            this.columnHeaderSection1.Width = 474;
            // 
            // columnHeaderCell1
            // 
            this.columnHeaderCell1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell1.Location = new System.Drawing.Point(0, 0);
            this.columnHeaderCell1.Name = "columnHeaderCell1";
            this.columnHeaderCell1.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.IntersectedCells;
            this.columnHeaderCell1.Size = new System.Drawing.Size(165, 28);
            cellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border2.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle4.Border = border2;
            cellStyle4.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle4.ImageAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            cellStyle4.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            cellStyle4.TextAngle = 0F;
            cellStyle4.TextEffect = GrapeCity.Win.MultiRow.TextEffect.Flat;
            cellStyle4.TextImageRelation = GrapeCity.Win.MultiRow.MultiRowTextImageRelation.ImageBeforeText;
            cellStyle4.TextIndent = 0;
            cellStyle4.WordWrap = GrapeCity.Win.MultiRow.MultiRowTriState.False;
            this.columnHeaderCell1.Style = cellStyle4;
            this.columnHeaderCell1.TabIndex = 0;
            this.columnHeaderCell1.Value = "分類";
            // 
            // columnHeaderCell2
            // 
            this.columnHeaderCell2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell2.Location = new System.Drawing.Point(165, 0);
            this.columnHeaderCell2.Name = "columnHeaderCell2";
            this.columnHeaderCell2.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.IntersectedCells;
            this.columnHeaderCell2.Size = new System.Drawing.Size(225, 28);
            cellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border3.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle5.Border = border3;
            cellStyle5.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle5.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            this.columnHeaderCell2.Style = cellStyle5;
            this.columnHeaderCell2.TabIndex = 0;
            this.columnHeaderCell2.Value = "メニュー";
            // 
            // columnHeaderCell3
            // 
            this.columnHeaderCell3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.columnHeaderCell3.Location = new System.Drawing.Point(390, 0);
            this.columnHeaderCell3.Name = "columnHeaderCell3";
            this.columnHeaderCell3.SelectionMode = GrapeCity.Win.MultiRow.MultiRowSelectionMode.IntersectedCells;
            this.columnHeaderCell3.Size = new System.Drawing.Size(84, 28);
            cellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            border4.Outline = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
            cellStyle6.Border = border4;
            cellStyle6.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            cellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            cellStyle6.ImageAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
            cellStyle6.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleLeft;
            cellStyle6.TextAngle = 0F;
            cellStyle6.TextEffect = GrapeCity.Win.MultiRow.TextEffect.Flat;
            cellStyle6.TextImageRelation = GrapeCity.Win.MultiRow.MultiRowTextImageRelation.ImageBeforeText;
            cellStyle6.TextIndent = 0;
            cellStyle6.WordWrap = GrapeCity.Win.MultiRow.MultiRowTriState.False;
            this.columnHeaderCell3.Style = cellStyle6;
            this.columnHeaderCell3.TabIndex = 0;
            this.columnHeaderCell3.Value = "使用許可";
            // 
            // TBunruiName
            // 
            this.TBunruiName.Location = new System.Drawing.Point(0, 0);
            this.TBunruiName.Name = "TBunruiName";
            this.TBunruiName.ReadOnly = true;
            this.TBunruiName.Selectable = false;
            this.TBunruiName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.TBunruiName.Size = new System.Drawing.Size(165, 28);
            this.TBunruiName.TabIndex = 0;
            this.TBunruiName.TabStop = false;
            this.TBunruiName.Value = "売上業務";
            // 
            // MBunruiName
            // 
            this.MBunruiName.Location = new System.Drawing.Point(165, 0);
            this.MBunruiName.Name = "MBunruiName";
            this.MBunruiName.ReadOnly = true;
            this.MBunruiName.Selectable = false;
            this.MBunruiName.ShortcutKeys.AddRange(new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry[] {
            new GrapeCity.Win.MultiRow.InputMan.ShortcutDictionaryEntry(System.Windows.Forms.Keys.F2, "ShortcutClear")});
            this.MBunruiName.Size = new System.Drawing.Size(225, 28);
            this.MBunruiName.TabIndex = 1;
            this.MBunruiName.TabStop = false;
            this.MBunruiName.Value = "売上業務";
            // 
            // AllowUseFlag
            // 
            this.AllowUseFlag.Location = new System.Drawing.Point(390, 0);
            this.AllowUseFlag.Name = "AllowUseFlag";
            this.AllowUseFlag.Size = new System.Drawing.Size(84, 28);
            this.AllowUseFlag.Style = cellStyle1;
            this.AllowUseFlag.TabIndex = 2;
            this.AllowUseFlag.Text = "許可する";
            this.AllowUseFlag.Value = 0;
            // 
            // MenuAuthSettingTpl
            // 
            this.ColumnHeaders.AddRange(new GrapeCity.Win.MultiRow.ColumnHeaderSection[] {
            this.columnHeaderSection1});
            this.Height = 56;
            this.Width = 474;

        }
        

        #endregion

        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell TBunruiName;
        private GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell MBunruiName;
        private GrapeCity.Win.MultiRow.CheckBoxCell AllowUseFlag;
        private GrapeCity.Win.MultiRow.ColumnHeaderSection columnHeaderSection1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell1;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell2;
        private GrapeCity.Win.MultiRow.ColumnHeaderCell columnHeaderCell3;


    }
}
