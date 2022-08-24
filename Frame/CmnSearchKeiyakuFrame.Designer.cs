namespace Jpsys.SagyoManage.Frame
{
    partial class CmnSearchKeiyakuFrame
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
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
            this.edtSearchText.AlternateText.DisplayNull.Text = "契約の検索";
            this.edtSearchText.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearchText.AlternateText.Null.Text = "契約の検索";
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
            this.statusStrip1.Size = new System.Drawing.Size(910, 22);
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
            this.panel2.Size = new System.Drawing.Size(910, 42);
            this.panel2.TabIndex = 204;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(798, 6);
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
            this.menuStripTop.Size = new System.Drawing.Size(910, 24);
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
            this.panel1.Size = new System.Drawing.Size(910, 433);
            this.panel1.TabIndex = 207;
            // 
            // chkAllFlag
            // 
            this.chkAllFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllFlag.AutoSize = true;
            this.chkAllFlag.Checked = true;
            this.chkAllFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAllFlag.Location = new System.Drawing.Point(797, 11);
            this.chkAllFlag.Name = "chkAllFlag";
            this.chkAllFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkAllFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkAllFlag.Size = new System.Drawing.Size(99, 22);
            this.chkAllFlag.TabIndex = 100;
            this.chkAllFlag.TabStop = false;
            this.chkAllFlag.Text = "使用停止表示";
            this.chkAllFlag.UseVisualStyleBackColor = true;
            this.chkAllFlag.CheckedChanged += new System.EventHandler(this.chkAllFlag_CheckedChanged);
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
            this.fpListGrid.Size = new System.Drawing.Size(904, 391);
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
            this.fpListGrid_Sheet1.ColumnCount = 12;
            this.fpListGrid_Sheet1.RowCount = 30;
            this.fpListGrid_Sheet1.Cells.Get(0, 0).Value = "12345";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 2).Value = "2021/01/01";
            this.fpListGrid_Sheet1.Cells.Get(0, 3).Value = "2021/01/01";
            this.fpListGrid_Sheet1.Cells.Get(0, 4).Value = "12345";
            this.fpListGrid_Sheet1.Cells.Get(0, 5).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 6).Value = "12345";
            this.fpListGrid_Sheet1.Cells.Get(0, 7).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 8).Value = "123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 9).Value = "123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 10).Value = "123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 11).Value = "使用停止";
            this.fpListGrid_Sheet1.Cells.Get(1, 0).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 4).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 6).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(2, 0).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 4).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 6).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(3, 0).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 4).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 6).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(4, 0).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 4).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 6).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(5, 0).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 4).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 6).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(6, 0).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 4).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 6).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(7, 0).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 4).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 6).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(8, 0).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 4).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 6).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(9, 0).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 4).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 6).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(10, 0).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 4).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 6).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(11, 0).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 4).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 6).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(12, 0).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 4).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 6).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(13, 0).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 4).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 6).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(14, 0).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 4).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 6).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(15, 0).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 4).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 6).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(16, 0).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 4).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 6).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(17, 0).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 4).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 6).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(18, 0).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 4).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 6).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(19, 0).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 4).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 6).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(20, 0).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 4).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 6).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(21, 0).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 4).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 6).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(22, 0).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 4).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 6).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(23, 0).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 4).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 6).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(24, 0).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 4).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 6).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(25, 0).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 4).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 6).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(26, 0).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 4).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 6).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(27, 0).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 4).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 6).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(28, 0).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 4).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 6).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(29, 0).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 4).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 6).Value = 30D;
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
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "契約コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "契約名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "契約開始日";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "契約終了日";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "得意先コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "得意先名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "作業場所コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "作業場所";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "作業大分類";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "作業中分類";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "作業小分類";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "使用停止";
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpListGrid_Sheet1.Columns.Get(0).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(0).Label = "契約コード";
            this.fpListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(0).Width = 84F;
            this.fpListGrid_Sheet1.Columns.Get(1).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpListGrid_Sheet1.Columns.Get(1).Label = "契約名";
            this.fpListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(1).Width = 317F;
            this.fpListGrid_Sheet1.Columns.Get(2).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpListGrid_Sheet1.Columns.Get(2).Label = "契約開始日";
            this.fpListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Width = 88F;
            this.fpListGrid_Sheet1.Columns.Get(3).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.fpListGrid_Sheet1.Columns.Get(3).Label = "契約終了日";
            this.fpListGrid_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Width = 88F;
            this.fpListGrid_Sheet1.Columns.Get(4).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.fpListGrid_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(4).Label = "得意先コード";
            this.fpListGrid_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(4).Width = 97F;
            this.fpListGrid_Sheet1.Columns.Get(5).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.fpListGrid_Sheet1.Columns.Get(5).Label = "得意先名";
            this.fpListGrid_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(5).Width = 317F;
            this.fpListGrid_Sheet1.Columns.Get(6).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.fpListGrid_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(6).Label = "作業場所コード";
            this.fpListGrid_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(6).Width = 109F;
            this.fpListGrid_Sheet1.Columns.Get(7).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.fpListGrid_Sheet1.Columns.Get(7).Label = "作業場所";
            this.fpListGrid_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(7).Width = 317F;
            this.fpListGrid_Sheet1.Columns.Get(8).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.fpListGrid_Sheet1.Columns.Get(8).Label = "作業大分類";
            this.fpListGrid_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(8).Width = 180F;
            this.fpListGrid_Sheet1.Columns.Get(9).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.fpListGrid_Sheet1.Columns.Get(9).Label = "作業中分類";
            this.fpListGrid_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(9).Width = 180F;
            this.fpListGrid_Sheet1.Columns.Get(10).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.fpListGrid_Sheet1.Columns.Get(10).Label = "作業小分類";
            this.fpListGrid_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(10).Width = 180F;
            this.fpListGrid_Sheet1.Columns.Get(11).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.fpListGrid_Sheet1.Columns.Get(11).Label = "使用停止";
            this.fpListGrid_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(11).Width = 80F;
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
            // CmnSearchKeiyakuFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(910, 521);
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
            this.Name = "CmnSearchKeiyakuFrame";
            this.Text = "契約検索";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmnSearchKeiyakuFrame_KeyDown);
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