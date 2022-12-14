namespace Jpsys.SagyoManage.Frame
{
    partial class CmnSearchMeishoFrame
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
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSearchText = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSelect = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAllFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.fpListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchText)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // edtSearchText
            // 
            this.edtSearchText.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearchText.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearchText.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearchText.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSearchText.DropDown.AllowDrop = false;
            this.edtSearchText.HighlightText = true;
            this.edtSearchText.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSearchText.Location = new System.Drawing.Point(3, 7);
            this.edtSearchText.Name = "edtSearchText";
            this.gcShortcut1.SetShortcuts(this.edtSearchText, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl",
                "PreviousControl"}));
            this.edtSearchText.Size = new System.Drawing.Size(280, 28);
            this.edtSearchText.TabIndex = 0;
            this.edtSearchText.TextChanged += new System.EventHandler(this.edtSearchText_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripSelect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 499);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(544, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 203;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripEnd
            // 
            this.toolStripEnd.ForeColor = System.Drawing.Color.White;
            this.toolStripEnd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripEnd.Name = "toolStripEnd";
            this.toolStripEnd.Size = new System.Drawing.Size(58, 20);
            this.toolStripEnd.Text = "F1：終了";
            // 
            // toolStripSelect
            // 
            this.toolStripSelect.ForeColor = System.Drawing.Color.White;
            this.toolStripSelect.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSelect.Name = "toolStripSelect";
            this.toolStripSelect.Size = new System.Drawing.Size(58, 20);
            this.toolStripSelect.Text = "F7：選択";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 457);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(544, 42);
            this.panel2.TabIndex = 204;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(432, 6);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 0;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "選択";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStripTop.Size = new System.Drawing.Size(544, 24);
            this.menuStripTop.TabIndex = 206;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAllFlag);
            this.panel1.Controls.Add(this.edtSearchText);
            this.panel1.Controls.Add(this.fpListGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 433);
            this.panel1.TabIndex = 207;
            // 
            // chkAllFlag
            // 
            this.chkAllFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllFlag.AutoSize = true;
            this.chkAllFlag.Checked = true;
            this.chkAllFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAllFlag.Location = new System.Drawing.Point(431, 11);
            this.chkAllFlag.Name = "chkAllFlag";
            this.chkAllFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkAllFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkAllFlag.Size = new System.Drawing.Size(99, 22);
            this.chkAllFlag.TabIndex = 100;
            this.chkAllFlag.TabStop = false;
            this.chkAllFlag.Text = "使用停止表示";
            this.chkAllFlag.UseVisualStyleBackColor = true;
            this.chkAllFlag.CheckedChanged += new System.EventHandler(this.chkFlag_CheckedChanged);
            // 
            // fpListGrid
            // 
            this.fpListGrid.AccessibleDescription = "fpListGrid, Sheet1, Row 0, Column 0";
            this.fpListGrid.AllowColumnMove = true;
            this.fpListGrid.AllowUserZoom = false;
            this.fpListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.HorizontalScrollBar.Name = "";
            this.fpListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpListGrid.Location = new System.Drawing.Point(3, 39);
            this.fpListGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fpListGrid.Name = "fpListGrid";
            this.fpListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpListGrid_Sheet1});
            this.fpListGrid.Size = new System.Drawing.Size(538, 392);
            this.fpListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpListGrid.SpreadScaleMode = FarPoint.Win.Spread.ScaleMode.ZoomDpiSupport;
            this.fpListGrid.TabIndex = 1;
            this.fpListGrid.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.fpListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.VerticalScrollBar.Name = "";
            this.fpListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpListGrid_CellClick);
            this.fpListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpListGrid_CellDoubleClick);
            this.fpListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpListGrid_PreviewKeyDown);
            this.fpListGrid.SetViewportLeftColumn(0, 0, 2);
            this.fpListGrid.SetActiveViewport(0, 0, -1);
            // 
            // fpListGrid_Sheet1
            // 
            this.fpListGrid_Sheet1.Reset();
            this.fpListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpListGrid_Sheet1.ColumnCount = 4;
            this.fpListGrid_Sheet1.RowCount = 30;
            this.fpListGrid_Sheet1.Cells.Get(0, 0).Value = "123456";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 2).Value = "1234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 3).Value = "使用停止";
            this.fpListGrid_Sheet1.Cells.Get(1, 0).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(2, 0).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(3, 0).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(4, 0).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(5, 0).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(6, 0).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(7, 0).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(8, 0).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(9, 0).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(10, 0).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(11, 0).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(12, 0).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(13, 0).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(14, 0).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(15, 0).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(16, 0).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(17, 0).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(18, 0).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(19, 0).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(20, 0).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(21, 0).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(22, 0).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(23, 0).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(24, 0).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(25, 0).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(26, 0).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(27, 0).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(28, 0).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(29, 0).Value = 30D;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "名称";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "フリガナ";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "使用停止";
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpListGrid_Sheet1.Columns.Get(0).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(0).Width = 72F;
            this.fpListGrid_Sheet1.Columns.Get(1).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpListGrid_Sheet1.Columns.Get(1).Label = "名称";
            this.fpListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(1).Width = 269F;
            this.fpListGrid_Sheet1.Columns.Get(2).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpListGrid_Sheet1.Columns.Get(2).Label = "フリガナ";
            this.fpListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Width = 96F;
            this.fpListGrid_Sheet1.Columns.Get(3).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.fpListGrid_Sheet1.Columns.Get(3).Label = "使用停止";
            this.fpListGrid_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Width = 80F;
            this.fpListGrid_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpListGrid_Sheet1.FilterBar.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.FilterBar.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpListGrid_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
            this.fpListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpListGrid_Sheet1.FrozenColumnCount = 2;
            this.fpListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpListGrid_Sheet1.Protect = true;
            this.fpListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpListGrid_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpListGrid_Sheet1.RowHeader.Visible = false;
            this.fpListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpListGrid_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.SheetCornerStyle.Locked = false;
            this.fpListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // CmnSearchMeishoFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(544, 521);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CmnSearchMeishoFrame";
            this.Text = "名称検索";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmnSearchMeishoFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchText)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSelect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.Panel panel1;
        private GrapeCity.Win.Editors.GcTextBox edtSearchText;
        private FarPoint.Win.Spread.FpSpread fpListGrid;
        private FarPoint.Win.Spread.SheetView fpListGrid_Sheet1;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkAllFlag;
    }
}