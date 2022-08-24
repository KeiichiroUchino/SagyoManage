namespace Jpsys.SagyoManage.Frame
{
    partial class SagyoBashoViewFrame
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
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer2 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ja-JP", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSagyoBashoName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtTokuisakiCd = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.sideButton1 = new GrapeCity.Win.Editors.SideButton();
            this.edtTokuisakiNm = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtSagyoBashoCode = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.btnView = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAllFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.grpDetail = new System.Windows.Forms.GroupBox();
            this.fpListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label10 = new System.Windows.Forms.Label();
            this.toolStripNew = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripClose = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSearch = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnNew = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.dropDownButton1 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton2 = new GrapeCity.Win.Editors.DropDownButton();
            this.dropDownButton3 = new GrapeCity.Win.Editors.DropDownButton();
            this.pnlMid = new System.Windows.Forms.Panel();
            this.sbtnSeikyusakiCode = new GrapeCity.Win.Editors.SideButton();
            ((System.ComponentModel.ISupportInitialize)(this.edtSagyoBashoName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTokuisakiCd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTokuisakiNm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtSagyoBashoCode)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlMid.SuspendLayout();
            this.SuspendLayout();
            // 
            // edtSagyoBashoName
            // 
            this.edtSagyoBashoName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSagyoBashoName.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSagyoBashoName.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSagyoBashoName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSagyoBashoName.DropDown.AllowDrop = false;
            this.edtSagyoBashoName.HighlightText = true;
            this.edtSagyoBashoName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSagyoBashoName.Location = new System.Drawing.Point(251, 56);
            this.edtSagyoBashoName.MaxLength = 70;
            this.edtSagyoBashoName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtSagyoBashoName.Name = "edtSagyoBashoName";
            this.gcShortcut1.SetShortcuts(this.edtSagyoBashoName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtSagyoBashoName.Size = new System.Drawing.Size(510, 28);
            this.edtSagyoBashoName.TabIndex = 2;
            this.edtSagyoBashoName.Text = "1234567890123456789012345678901234567890123456789012345678901234567890";
            // 
            // edtTokuisakiCd
            // 
            this.edtTokuisakiCd.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtTokuisakiCd.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtTokuisakiCd.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtTokuisakiCd.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtTokuisakiCd.DropDown.AllowDrop = false;
            this.edtTokuisakiCd.HighlightText = true;
            this.edtTokuisakiCd.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.edtTokuisakiCd.Location = new System.Drawing.Point(111, 16);
            this.edtTokuisakiCd.MaxLength = 8;
            this.edtTokuisakiCd.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtTokuisakiCd.Name = "edtTokuisakiCd";
            this.gcShortcut1.SetShortcuts(this.edtTokuisakiCd, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtTokuisakiCd.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sideButton1});
            this.edtTokuisakiCd.Size = new System.Drawing.Size(87, 28);
            this.edtTokuisakiCd.TabIndex = 0;
            this.edtTokuisakiCd.Text = "12345678";
            // 
            // sideButton1
            // 
            this.sideButton1.ButtonWidth = 22;
            this.sideButton1.Name = "sideButton1";
            this.sideButton1.Text = "...";
            this.sideButton1.Click += new System.EventHandler(this.sbtn_Click);
            // 
            // edtTokuisakiNm
            // 
            this.edtTokuisakiNm.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtTokuisakiNm.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtTokuisakiNm.DropDown.AllowDrop = false;
            this.edtTokuisakiNm.HighlightText = true;
            this.edtTokuisakiNm.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtTokuisakiNm.Location = new System.Drawing.Point(199, 16);
            this.edtTokuisakiNm.MaxLength = 70;
            this.edtTokuisakiNm.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtTokuisakiNm.Name = "edtTokuisakiNm";
            this.edtTokuisakiNm.ReadOnly = true;
            this.edtTokuisakiNm.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.edtTokuisakiNm, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtTokuisakiNm.Size = new System.Drawing.Size(517, 28);
            this.edtTokuisakiNm.TabIndex = 1;
            this.edtTokuisakiNm.TabStop = false;
            this.edtTokuisakiNm.Text = "1234567890123456789012345678901234567890123456789012345678901234567890";
            // 
            // edtSagyoBashoCode
            // 
            this.edtSagyoBashoCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSagyoBashoCode.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSagyoBashoCode.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSagyoBashoCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSagyoBashoCode.DropDown.AllowDrop = false;
            this.edtSagyoBashoCode.HighlightText = true;
            this.edtSagyoBashoCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.edtSagyoBashoCode.Location = new System.Drawing.Point(111, 55);
            this.edtSagyoBashoCode.MaxLength = 9;
            this.edtSagyoBashoCode.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtSagyoBashoCode.Name = "edtSagyoBashoCode";
            this.gcShortcut1.SetShortcuts(this.edtSagyoBashoCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtSagyoBashoCode.Size = new System.Drawing.Size(72, 28);
            this.edtSagyoBashoCode.TabIndex = 1;
            this.edtSagyoBashoCode.Text = "123456789";
            // 
            // btnView
            // 
            this.btnView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnView.Location = new System.Drawing.Point(985, 54);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(86, 30);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "検索";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            this.btnView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctl_KeyDown);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.groupBox1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 24);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1097, 101);
            this.pnlTop.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkAllFlag);
            this.groupBox1.Controls.Add(this.edtSagyoBashoCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.edtTokuisakiNm);
            this.groupBox1.Controls.Add(this.edtTokuisakiCd);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.edtSagyoBashoName);
            this.groupBox1.Controls.Add(this.btnView);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1077, 90);
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
            this.chkAllFlag.Location = new System.Drawing.Point(767, 59);
            this.chkAllFlag.Name = "chkAllFlag";
            this.chkAllFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkAllFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkAllFlag.Size = new System.Drawing.Size(99, 22);
            this.chkAllFlag.TabIndex = 1053;
            this.chkAllFlag.TabStop = false;
            this.chkAllFlag.Text = "使用停止表示";
            this.chkAllFlag.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 18);
            this.label2.TabIndex = 1052;
            this.label2.Text = "作業場所コード";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 1051;
            this.label1.Text = "得意先";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.CausesValidation = false;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(189, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 18);
            this.label7.TabIndex = 49;
            this.label7.Text = "作業場所";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpDetail
            // 
            this.grpDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDetail.Controls.Add(this.fpListGrid);
            this.grpDetail.Controls.Add(this.label10);
            this.grpDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.grpDetail.Location = new System.Drawing.Point(8, 1);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(1077, 573);
            this.grpDetail.TabIndex = 0;
            this.grpDetail.TabStop = false;
            this.grpDetail.Text = "検索結果";
            // 
            // fpListGrid
            // 
            this.fpListGrid.AccessibleDescription = "fpListGrid, Sheet1, Row 0, Column 0";
            this.fpListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpListGrid.FocusRenderer = defaultFocusIndicatorRenderer2;
            this.fpListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.HorizontalScrollBar.Name = "";
            this.fpListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
            this.fpListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpListGrid.Location = new System.Drawing.Point(9, 25);
            this.fpListGrid.Name = "fpListGrid";
            this.fpListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpListGrid_Sheet1});
            this.fpListGrid.Size = new System.Drawing.Size(1062, 523);
            this.fpListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpListGrid.TabIndex = 0;
            this.fpListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.VerticalScrollBar.Name = "";
            this.fpListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
            this.fpListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpListGrid_CellClick);
            this.fpListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpListGrid_CellDoubleClick);
            this.fpListGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fpListGrid_MouseUp);
            this.fpListGrid.SetViewportLeftColumn(0, 0, 3);
            this.fpListGrid.SetActiveViewport(0, 0, -1);
            // 
            // fpListGrid_Sheet1
            // 
            this.fpListGrid_Sheet1.Reset();
            this.fpListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpListGrid_Sheet1.ColumnCount = 8;
            this.fpListGrid_Sheet1.RowCount = 18;
            this.fpListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpListGrid_Sheet1.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;
            this.fpListGrid_Sheet1.AutoSortEnhancedContextMenu = true;
            this.fpListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 1).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).Value = "A1";
            this.fpListGrid_Sheet1.Cells.Get(0, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 2).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 2).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 3).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 3).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 3).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 3).Value = "A1";
            this.fpListGrid_Sheet1.Cells.Get(0, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 4).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 4).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 5).Value = "12345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 6).Value = "12345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 7).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 7).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 7).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 7).Value = "〇";
            this.fpListGrid_Sheet1.Cells.Get(1, 1).Value = "A1234567";
            this.fpListGrid_Sheet1.Cells.Get(1, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(1, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(1, 2).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(1, 2).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(1, 3).Value = "A1234567";
            this.fpListGrid_Sheet1.Cells.Get(1, 4).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(1, 4).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(1, 4).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(1, 4).Value = "1234567890123456789012345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(1, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(1, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(1, 5).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(1, 5).Value = "12345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(1, 6).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(1, 6).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(1, 6).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(1, 6).Value = "12345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(1, 7).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(1, 7).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(1, 7).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(1, 7).Value = "〇";
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "ID";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "作業場所コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "作業場所";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "得意先コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "得意先名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "住所1";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "住所2";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "Shelter連携";
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpListGrid_Sheet1.Columns.Get(0).Label = "ID";
            this.fpListGrid_Sheet1.Columns.Get(0).Visible = false;
            this.fpListGrid_Sheet1.Columns.Get(1).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.fpListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(1).Label = "作業場所コード";
            this.fpListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(1).Width = 109F;
            this.fpListGrid_Sheet1.Columns.Get(2).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(2).CellType = textCellType9;
            this.fpListGrid_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(2).Label = "作業場所";
            this.fpListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Width = 284F;
            this.fpListGrid_Sheet1.Columns.Get(3).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(3).CellType = textCellType10;
            this.fpListGrid_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(3).Label = "得意先コード";
            this.fpListGrid_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Width = 97F;
            this.fpListGrid_Sheet1.Columns.Get(4).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(4).CellType = textCellType11;
            this.fpListGrid_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(4).Label = "得意先名";
            this.fpListGrid_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(4).Width = 291F;
            this.fpListGrid_Sheet1.Columns.Get(5).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(5).CellType = textCellType12;
            this.fpListGrid_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(5).Label = "住所1";
            this.fpListGrid_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(5).Width = 389F;
            this.fpListGrid_Sheet1.Columns.Get(6).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(6).CellType = textCellType13;
            this.fpListGrid_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(6).Label = "住所2";
            this.fpListGrid_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(6).Width = 389F;
            this.fpListGrid_Sheet1.Columns.Get(7).AllowAutoFilter = true;
            this.fpListGrid_Sheet1.Columns.Get(7).CellType = textCellType14;
            this.fpListGrid_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(7).Label = "Shelter連携";
            this.fpListGrid_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(7).Width = 106F;
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
            this.fpListGrid_Sheet1.FrozenColumnCount = 3;
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
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 550);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(414, 18);
            this.label10.TabIndex = 84;
            this.label10.Text = "※修正する場合は明細をダブルクリック または F5キーを押してください。";
            // 
            // toolStripNew
            // 
            this.toolStripNew.ForeColor = System.Drawing.Color.White;
            this.toolStripNew.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripNew.Name = "toolStripNew";
            this.toolStripNew.Size = new System.Drawing.Size(82, 20);
            this.toolStripNew.Text = "F2：新規作成";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripClose,
            this.toolStripNew,
            this.toolStripSearch});
            this.statusStrip1.Location = new System.Drawing.Point(0, 746);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1097, 22);
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
            // toolStripSearch
            // 
            this.toolStripSearch.ForeColor = System.Drawing.Color.White;
            this.toolStripSearch.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSearch.Name = "toolStripSearch";
            this.toolStripSearch.Size = new System.Drawing.Size(58, 20);
            this.toolStripSearch.Text = "F5：検索";
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNew.Location = new System.Drawing.Point(980, 6);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(105, 30);
            this.btnNew.TabIndex = 1;
            this.btnNew.TabStop = false;
            this.btnNew.Text = "新規作成";
            this.btnNew.UseVisualStyleBackColor = true;
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
            this.label5.Size = new System.Drawing.Size(1097, 1);
            this.label5.TabIndex = 82;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.btnNew);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 704);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1097, 42);
            this.pnlBottom.TabIndex = 2;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(1097, 24);
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
            this.pnlMid.Location = new System.Drawing.Point(0, 125);
            this.pnlMid.Name = "pnlMid";
            this.pnlMid.Size = new System.Drawing.Size(1097, 579);
            this.pnlMid.TabIndex = 204;
            // 
            // sbtnSeikyusakiCode
            // 
            this.sbtnSeikyusakiCode.ButtonWidth = 22;
            this.sbtnSeikyusakiCode.Name = "sbtnSeikyusakiCode";
            this.sbtnSeikyusakiCode.Text = "...";
            this.sbtnSeikyusakiCode.Click += new System.EventHandler(this.sbtn_Click);
            // 
            // SagyoBashoViewFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1097, 768);
            this.Controls.Add(this.pnlMid);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SagyoBashoViewFrame";
            this.Text = "SagyoBashoViewFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JuchuIchiranFrame_FormClosing);
            this.Shown += new System.EventHandler(this.JuchuIchiranFrame_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.JuchuIchiranFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSagyoBashoName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTokuisakiCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTokuisakiNm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtSagyoBashoCode)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlMid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNew;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton1;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton2;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpDetail;
        private System.Windows.Forms.Panel pnlMid;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStripStatusLabel toolStripClose;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSearch;
        private FarPoint.Win.Spread.FpSpread fpListGrid;
        private FarPoint.Win.Spread.SheetView fpListGrid_Sheet1;
        private GrapeCity.Win.Editors.SideButton sbtnSeikyusakiCode;
        private System.Windows.Forms.Label label7;
        private GrapeCity.Win.Editors.GcTextBox edtSagyoBashoName;
        private GrapeCity.Win.Editors.GcTextBox edtTokuisakiCd;
        private System.Windows.Forms.Label label1;
        private GrapeCity.Win.Editors.GcTextBox edtTokuisakiNm;
        private GrapeCity.Win.Editors.SideButton sideButton1;
        private System.Windows.Forms.Label label2;
        private GrapeCity.Win.Editors.GcTextBox edtSagyoBashoCode;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkAllFlag;
    }
}