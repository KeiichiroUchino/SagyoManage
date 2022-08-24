namespace Jpsys.HaishaManageV10.Frame
{
    partial class CarKindGroupFrame
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
            GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField numberIntegerPartDisplayField1 = new GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField();
            GrapeCity.Win.Editors.Fields.NumberSignDisplayField numberSignDisplayField1 = new GrapeCity.Win.Editors.Fields.NumberSignDisplayField();
            GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField numberIntegerPartDisplayField2 = new GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField();
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ja-JP", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSearch = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtCarKindGroupName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numCarKindGroupCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.edtFukushaCarKindGroupName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numFukushaCarKindGroupCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.sbtnFukushaCarKindGroupCode = new GrapeCity.Win.Editors.SideButton();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.fpCarKindGroupListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpCarKindGroupListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripReference = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripNew = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRemove = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRemoveMeisaiGyo = new System.Windows.Forms.Button();
            this.btnAddMeisaiGyo = new System.Windows.Forms.Button();
            this.mrsCarKindGroupList = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.tokuisakiGroupMeisaiTpl1 = new Jpsys.HaishaManageV10.Frame.CarKindGroupMeisaiTpl();
            this.label17 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDisableFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtCarKindGroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCarKindGroupCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFukushaCarKindGroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFukushaCarKindGroupCode)).BeginInit();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpCarKindGroupListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpCarKindGroupListGrid_Sheet1)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mrsCarKindGroupList)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Location = new System.Drawing.Point(317, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 633);
            this.splitter1.TabIndex = 181;
            this.splitter1.TabStop = false;
            // 
            // edtSearch
            // 
            this.edtSearch.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearch.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.DisplayNull.Text = "車種グループの検索";
            this.edtSearch.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.Null.Text = "車種グループの検索";
            this.edtSearch.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSearch.DropDown.AllowDrop = false;
            this.edtSearch.HighlightText = true;
            this.edtSearch.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSearch.Location = new System.Drawing.Point(5, 5);
            this.edtSearch.MaxLength = 50;
            this.edtSearch.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtSearch.Name = "edtSearch";
            this.gcShortcut1.SetShortcuts(this.edtSearch, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtSearch.Size = new System.Drawing.Size(304, 28);
            this.edtSearch.TabIndex = 0;
            this.edtSearch.Text = "12345678901234567890123456789012345678901234567890";
            this.edtSearch.TextChanged += new System.EventHandler(this.edtSearch_TextChanged);
            // 
            // edtCarKindGroupName
            // 
            this.edtCarKindGroupName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtCarKindGroupName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtCarKindGroupName.DropDown.AllowDrop = false;
            this.edtCarKindGroupName.HighlightText = true;
            this.edtCarKindGroupName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtCarKindGroupName.Location = new System.Drawing.Point(173, 88);
            this.edtCarKindGroupName.MaxLength = 60;
            this.edtCarKindGroupName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtCarKindGroupName.Name = "edtCarKindGroupName";
            this.gcShortcut1.SetShortcuts(this.edtCarKindGroupName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtCarKindGroupName.Size = new System.Drawing.Size(481, 28);
            this.edtCarKindGroupName.TabIndex = 3;
            this.edtCarKindGroupName.Text = "123456789012345678901234567890123456789012345678901234567890";
            // 
            // numCarKindGroupCode
            // 
            this.numCarKindGroupCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numCarKindGroupCode.AllowDeleteToNull = true;
            this.numCarKindGroupCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numCarKindGroupCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numCarKindGroupCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.numCarKindGroupCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberIntegerPartDisplayField1});
            this.numCarKindGroupCode.DropDown.AllowDrop = false;
            this.numCarKindGroupCode.Fields.DecimalPart.MaxDigits = 0;
            this.numCarKindGroupCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numCarKindGroupCode.Fields.IntegerPart.MaxDigits = 3;
            this.numCarKindGroupCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numCarKindGroupCode.Fields.SignPrefix.NegativePattern = "";
            this.numCarKindGroupCode.HighlightText = true;
            this.numCarKindGroupCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numCarKindGroupCode.Location = new System.Drawing.Point(173, 14);
            this.numCarKindGroupCode.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numCarKindGroupCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCarKindGroupCode.Name = "numCarKindGroupCode";
            this.numCarKindGroupCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numCarKindGroupCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numCarKindGroupCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numCarKindGroupCode.Size = new System.Drawing.Size(66, 28);
            this.numCarKindGroupCode.Spin.AllowSpin = false;
            this.numCarKindGroupCode.TabIndex = 0;
            this.numCarKindGroupCode.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            this.numCarKindGroupCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // edtFukushaCarKindGroupName
            // 
            this.edtFukushaCarKindGroupName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtFukushaCarKindGroupName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtFukushaCarKindGroupName.DropDown.AllowDrop = false;
            this.edtFukushaCarKindGroupName.HighlightText = true;
            this.edtFukushaCarKindGroupName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtFukushaCarKindGroupName.Location = new System.Drawing.Point(262, 51);
            this.edtFukushaCarKindGroupName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtFukushaCarKindGroupName.Name = "edtFukushaCarKindGroupName";
            this.edtFukushaCarKindGroupName.ReadOnly = true;
            this.edtFukushaCarKindGroupName.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.edtFukushaCarKindGroupName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtFukushaCarKindGroupName.Size = new System.Drawing.Size(392, 28);
            this.edtFukushaCarKindGroupName.TabIndex = 2;
            this.edtFukushaCarKindGroupName.TabStop = false;
            this.edtFukushaCarKindGroupName.Text = "1234567890123456789012345678901234567890";
            // 
            // numFukushaCarKindGroupCode
            // 
            this.numFukushaCarKindGroupCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numFukushaCarKindGroupCode.AllowDeleteToNull = true;
            this.numFukushaCarKindGroupCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numFukushaCarKindGroupCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numFukushaCarKindGroupCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField2.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField2.MinDigits = 0;
            this.numFukushaCarKindGroupCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField1,
            numberIntegerPartDisplayField2});
            this.numFukushaCarKindGroupCode.DropDown.AllowDrop = false;
            this.numFukushaCarKindGroupCode.Fields.DecimalPart.MaxDigits = 0;
            this.numFukushaCarKindGroupCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numFukushaCarKindGroupCode.Fields.IntegerPart.MaxDigits = 3;
            this.numFukushaCarKindGroupCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numFukushaCarKindGroupCode.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numFukushaCarKindGroupCode.HighlightText = true;
            this.numFukushaCarKindGroupCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numFukushaCarKindGroupCode.Location = new System.Drawing.Point(173, 51);
            this.numFukushaCarKindGroupCode.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numFukushaCarKindGroupCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFukushaCarKindGroupCode.Name = "numFukushaCarKindGroupCode";
            this.numFukushaCarKindGroupCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numFukushaCarKindGroupCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numFukushaCarKindGroupCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numFukushaCarKindGroupCode.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sbtnFukushaCarKindGroupCode});
            this.numFukushaCarKindGroupCode.Size = new System.Drawing.Size(88, 28);
            this.numFukushaCarKindGroupCode.Spin.AllowSpin = false;
            this.numFukushaCarKindGroupCode.TabIndex = 1;
            this.numFukushaCarKindGroupCode.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            this.numFukushaCarKindGroupCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // sbtnFukushaCarKindGroupCode
            // 
            this.sbtnFukushaCarKindGroupCode.ButtonWidth = 22;
            this.sbtnFukushaCarKindGroupCode.Name = "sbtnFukushaCarKindGroupCode";
            this.sbtnFukushaCarKindGroupCode.Text = "...";
            this.sbtnFukushaCarKindGroupCode.Click += new System.EventHandler(this.sbtnFukushaCarKindGroupCode_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNew.Location = new System.Drawing.Point(202, 597);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(105, 30);
            this.btnNew.TabIndex = 1;
            this.btnNew.TabStop = false;
            this.btnNew.Text = "新規作成";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.fpCarKindGroupListGrid);
            this.pnlLeft.Controls.Add(this.edtSearch);
            this.pnlLeft.Controls.Add(this.btnNew);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 24);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(317, 633);
            this.pnlLeft.TabIndex = 10;
            // 
            // fpCarKindGroupListGrid
            // 
            this.fpCarKindGroupListGrid.AccessibleDescription = "fpCarKindGroupListGrid, Sheet1, Row 0, Column 0, 1";
            this.fpCarKindGroupListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpCarKindGroupListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpCarKindGroupListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpCarKindGroupListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpCarKindGroupListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpCarKindGroupListGrid.HorizontalScrollBar.Name = "";
            this.fpCarKindGroupListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpCarKindGroupListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpCarKindGroupListGrid.Location = new System.Drawing.Point(3, 39);
            this.fpCarKindGroupListGrid.Name = "fpCarKindGroupListGrid";
            this.fpCarKindGroupListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpCarKindGroupListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpCarKindGroupListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpCarKindGroupListGrid_Sheet1});
            this.fpCarKindGroupListGrid.Size = new System.Drawing.Size(311, 552);
            this.fpCarKindGroupListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpCarKindGroupListGrid.TabIndex = 2;
            this.fpCarKindGroupListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpCarKindGroupListGrid.VerticalScrollBar.Name = "";
            this.fpCarKindGroupListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpCarKindGroupListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpCarKindGroupListGrid.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpCarKindGroupListGrid_SelectionChanged);
            this.fpCarKindGroupListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpCarKindGroupListGrid_CellClick);
            this.fpCarKindGroupListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpCarKindGroupListGrid_CellDoubleClick);
            this.fpCarKindGroupListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpCarKindGroupListGrid_PreviewKeyDown);
            // 
            // fpCarKindGroupListGrid_Sheet1
            // 
            this.fpCarKindGroupListGrid_Sheet1.Reset();
            this.fpCarKindGroupListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpCarKindGroupListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpCarKindGroupListGrid_Sheet1.ColumnCount = 2;
            this.fpCarKindGroupListGrid_Sheet1.RowCount = 18;
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpCarKindGroupListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(0, 0).ParseFormatString = "E";
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(0, 0).Value = "1";
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(0, 1).Value = "基本グループ";
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(1, 0).Value = "123";
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpCarKindGroupListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(1, 1).ParseFormatString = "E";
            this.fpCarKindGroupListGrid_Sheet1.Cells.Get(1, 1).Value = "123456789123456789123456789123456789123456789";
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "コード";
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "車種グループ名";
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpCarKindGroupListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).Resizable = false;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(0).Width = 56F;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).Label = "車種グループ名";
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).Resizable = false;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpCarKindGroupListGrid_Sheet1.Columns.Get(1).Width = 234F;
            this.fpCarKindGroupListGrid_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.DefaultStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpCarKindGroupListGrid_Sheet1.FilterBar.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.FilterBar.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpCarKindGroupListGrid_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpCarKindGroupListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpCarKindGroupListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpCarKindGroupListGrid_Sheet1.Protect = true;
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpCarKindGroupListGrid_Sheet1.RowHeader.Visible = false;
            this.fpCarKindGroupListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpCarKindGroupListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpCarKindGroupListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpCarKindGroupListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpCarKindGroupListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpCarKindGroupListGrid_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpCarKindGroupListGrid_Sheet1.SheetCornerStyle.Locked = false;
            this.fpCarKindGroupListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpCarKindGroupListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // toolStripEnd
            // 
            this.toolStripEnd.ForeColor = System.Drawing.Color.White;
            this.toolStripEnd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripEnd.Name = "toolStripEnd";
            this.toolStripEnd.Size = new System.Drawing.Size(58, 20);
            this.toolStripEnd.Text = "F1：終了";
            // 
            // toolStripReference
            // 
            this.toolStripReference.ForeColor = System.Drawing.Color.White;
            this.toolStripReference.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripReference.Name = "toolStripReference";
            this.toolStripReference.Size = new System.Drawing.Size(58, 20);
            this.toolStripReference.Text = "F5：参照";
            // 
            // toolStripNew
            // 
            this.toolStripNew.ForeColor = System.Drawing.Color.White;
            this.toolStripNew.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripNew.Name = "toolStripNew";
            this.toolStripNew.Size = new System.Drawing.Size(82, 20);
            this.toolStripNew.Text = "F2：新規作成";
            // 
            // toolStripCancel
            // 
            this.toolStripCancel.ForeColor = System.Drawing.Color.White;
            this.toolStripCancel.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripCancel.Name = "toolStripCancel";
            this.toolStripCancel.Size = new System.Drawing.Size(119, 20);
            this.toolStripCancel.Text = "Shift+F6：編集取消";
            // 
            // toolStripSave
            // 
            this.toolStripSave.ForeColor = System.Drawing.Color.White;
            this.toolStripSave.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(58, 20);
            this.toolStripSave.Text = "F7：保存";
            // 
            // toolStripRemove
            // 
            this.toolStripRemove.ForeColor = System.Drawing.Color.White;
            this.toolStripRemove.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripRemove.Name = "toolStripRemove";
            this.toolStripRemove.Size = new System.Drawing.Size(95, 20);
            this.toolStripRemove.Text = "Shift+F8：削除";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(794, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDelete.Location = new System.Drawing.Point(907, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(105, 30);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(681, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 30);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "編集取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(12, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "終了";
            this.btnClose.UseVisualStyleBackColor = true;
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
            this.label5.Size = new System.Drawing.Size(1024, 1);
            this.label5.TabIndex = 82;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnDelete);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 657);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1024, 42);
            this.pnlBottom.TabIndex = 2;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.tabControl1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(321, 24);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(703, 633);
            this.pnlRight.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(703, 633);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edtFukushaCarKindGroupName);
            this.tabPage1.Controls.Add(this.numFukushaCarKindGroupCode);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.btnRemoveMeisaiGyo);
            this.tabPage1.Controls.Add(this.btnAddMeisaiGyo);
            this.tabPage1.Controls.Add(this.mrsCarKindGroupList);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.numCarKindGroupCode);
            this.tabPage1.Controls.Add(this.chkDisableFlag);
            this.tabPage1.Controls.Add(this.edtCarKindGroupName);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(695, 602);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本情報";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.CausesValidation = false;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(16, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 18);
            this.label12.TabIndex = 1069;
            this.label12.Text = "複写元";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRemoveMeisaiGyo
            // 
            this.btnRemoveMeisaiGyo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveMeisaiGyo.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRemoveMeisaiGyo.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveMeisaiGyo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemoveMeisaiGyo.Location = new System.Drawing.Point(173, 565);
            this.btnRemoveMeisaiGyo.Name = "btnRemoveMeisaiGyo";
            this.btnRemoveMeisaiGyo.Size = new System.Drawing.Size(80, 28);
            this.btnRemoveMeisaiGyo.TabIndex = 1067;
            this.btnRemoveMeisaiGyo.TabStop = false;
            this.btnRemoveMeisaiGyo.Text = "明細削除";
            this.btnRemoveMeisaiGyo.UseVisualStyleBackColor = true;
            this.btnRemoveMeisaiGyo.Click += new System.EventHandler(this.btnRemoveMeisaiGyo_Click);
            // 
            // btnAddMeisaiGyo
            // 
            this.btnAddMeisaiGyo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddMeisaiGyo.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAddMeisaiGyo.ForeColor = System.Drawing.Color.Black;
            this.btnAddMeisaiGyo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddMeisaiGyo.Location = new System.Drawing.Point(262, 565);
            this.btnAddMeisaiGyo.Name = "btnAddMeisaiGyo";
            this.btnAddMeisaiGyo.Size = new System.Drawing.Size(80, 28);
            this.btnAddMeisaiGyo.TabIndex = 1066;
            this.btnAddMeisaiGyo.TabStop = false;
            this.btnAddMeisaiGyo.Text = "明細挿入";
            this.btnAddMeisaiGyo.UseVisualStyleBackColor = true;
            this.btnAddMeisaiGyo.Click += new System.EventHandler(this.btnAddMeisaiGyo_Click);
            // 
            // mrsCarKindGroupList
            // 
            this.mrsCarKindGroupList.AllowUserToResize = false;
            this.mrsCarKindGroupList.AllowUserToZoom = false;
            this.mrsCarKindGroupList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mrsCarKindGroupList.Location = new System.Drawing.Point(173, 125);
            this.mrsCarKindGroupList.MultiSelect = false;
            this.mrsCarKindGroupList.Name = "mrsCarKindGroupList";
            this.mrsCarKindGroupList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mrsCarKindGroupList.ScrollBarStyle = System.Windows.Forms.FlatStyle.Standard;
            this.mrsCarKindGroupList.Size = new System.Drawing.Size(501, 431);
            this.mrsCarKindGroupList.SplitMode = GrapeCity.Win.MultiRow.SplitMode.None;
            this.mrsCarKindGroupList.TabIndex = 4;
            this.mrsCarKindGroupList.Template = this.tokuisakiGroupMeisaiTpl1;
            this.mrsCarKindGroupList.Text = "gcMultiRow1";
            this.mrsCarKindGroupList.RowsAdded += new System.EventHandler<GrapeCity.Win.MultiRow.RowsAddedEventArgs>(this.mrsCarKindGroupList_RowsAdded);
            this.mrsCarKindGroupList.CellValidating += new System.EventHandler<GrapeCity.Win.MultiRow.CellValidatingEventArgs>(this.mrsCarKindGroupList_CellValidating);
            this.mrsCarKindGroupList.EditingControlShowing += new System.EventHandler<GrapeCity.Win.MultiRow.EditingControlShowingEventArgs>(this.mrsCarKindGroupList_EditingControlShowing);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.CausesValidation = false;
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(15, 130);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 18);
            this.label17.TabIndex = 1063;
            this.label17.Text = "車種情報";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(16, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 18);
            this.label2.TabIndex = 1055;
            this.label2.Text = "名称 *";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDisableFlag
            // 
            this.chkDisableFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDisableFlag.AutoSize = true;
            this.chkDisableFlag.ForeColor = System.Drawing.Color.Red;
            this.chkDisableFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDisableFlag.Location = new System.Drawing.Point(575, 569);
            this.chkDisableFlag.Name = "chkDisableFlag";
            this.chkDisableFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkDisableFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkDisableFlag.Size = new System.Drawing.Size(87, 22);
            this.chkDisableFlag.TabIndex = 24;
            this.chkDisableFlag.TabStop = false;
            this.chkDisableFlag.Text = "無効にする";
            this.chkDisableFlag.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.CausesValidation = false;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(16, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 18);
            this.label6.TabIndex = 158;
            this.label6.Text = "コード *";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(1024, 24);
            this.menuStripTop.TabIndex = 9;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripNew,
            this.toolStripReference,
            this.toolStripCancel,
            this.toolStripSave,
            this.toolStripRemove});
            this.statusStrip1.Location = new System.Drawing.Point(0, 699);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1024, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 203;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CarKindGroupFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1024, 721);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "CarKindGroupFrame";
            this.Text = "CarKindGroupFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CarKindGroupFrame_FormClosing);
            this.Shown += new System.EventHandler(this.CarKindGroupFrame_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CarKindGroupFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtCarKindGroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCarKindGroupCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtFukushaCarKindGroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFukushaCarKindGroupCode)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpCarKindGroupListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpCarKindGroupListGrid_Sheet1)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mrsCarKindGroupList)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Button btnNew;
        private GrapeCity.Win.Editors.GcTextBox edtSearch;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNew;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCancel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSave;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRemove;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private GrapeCity.Win.Editors.GcNumber numCarKindGroupCode;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkDisableFlag;
        private GrapeCity.Win.Editors.GcTextBox edtCarKindGroupName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label2;
        private FarPoint.Win.Spread.FpSpread fpCarKindGroupListGrid;
        private FarPoint.Win.Spread.SheetView fpCarKindGroupListGrid_Sheet1;
        private GrapeCity.Win.MultiRow.GcMultiRow mrsCarKindGroupList;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripReference;
        private System.Windows.Forms.Button btnRemoveMeisaiGyo;
        private System.Windows.Forms.Button btnAddMeisaiGyo;
        private CarKindGroupMeisaiTpl tokuisakiGroupMeisaiTpl1;
        private GrapeCity.Win.Editors.GcTextBox edtFukushaCarKindGroupName;
        private GrapeCity.Win.Editors.GcNumber numFukushaCarKindGroupCode;
        private GrapeCity.Win.Editors.SideButton sbtnFukushaCarKindGroupCode;
        private System.Windows.Forms.Label label12;
    }
}