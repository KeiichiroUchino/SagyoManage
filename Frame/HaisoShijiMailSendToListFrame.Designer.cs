namespace Jpsys.HaishaManageV10.Frame
{
    partial class HaisoShijiMailSendToListFrame
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
            GrapeCity.Win.Editors.Fields.NumberSignDisplayField numberSignDisplayField1 = new GrapeCity.Win.Editors.Fields.NumberSignDisplayField();
            GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField numberIntegerPartDisplayField1 = new GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HaisoShijiMailSendToListFrame));
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ja-JP", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSearch = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtStaffName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtStaffNameKana = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numStaffCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.edtEmail = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.fpStaffListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.fpStaffListGrid = new FarPoint.Win.Spread.FpSpread();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRemove = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripReference = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label55 = new System.Windows.Forms.Label();
            this.chkSendFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffNameKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpStaffListGrid_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpStaffListGrid)).BeginInit();
            this.pnlLeft.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Location = new System.Drawing.Point(317, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 493);
            this.splitter1.TabIndex = 181;
            this.splitter1.TabStop = false;
            // 
            // edtSearch
            // 
            this.edtSearch.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearch.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.DisplayNull.Text = "乗務員の検索";
            this.edtSearch.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.Null.Text = "乗務員の検索";
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
            // edtStaffName
            // 
            this.edtStaffName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtStaffName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtStaffName.DropDown.AllowDrop = false;
            this.edtStaffName.HighlightText = true;
            this.edtStaffName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtStaffName.Location = new System.Drawing.Point(173, 51);
            this.edtStaffName.MaxLength = 30;
            this.edtStaffName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtStaffName.Name = "edtStaffName";
            this.gcShortcut1.SetShortcuts(this.edtStaffName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtStaffName.Size = new System.Drawing.Size(247, 28);
            this.edtStaffName.TabIndex = 1;
            this.edtStaffName.Text = "123456789012345678901234567890";
            // 
            // edtStaffNameKana
            // 
            this.edtStaffNameKana.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtStaffNameKana.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtStaffNameKana.DropDown.AllowDrop = false;
            this.edtStaffNameKana.HighlightText = true;
            this.edtStaffNameKana.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtStaffNameKana.Location = new System.Drawing.Point(173, 88);
            this.edtStaffNameKana.MaxLength = 10;
            this.edtStaffNameKana.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtStaffNameKana.Name = "edtStaffNameKana";
            this.gcShortcut1.SetShortcuts(this.edtStaffNameKana, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtStaffNameKana.Size = new System.Drawing.Size(247, 28);
            this.edtStaffNameKana.TabIndex = 2;
            this.edtStaffNameKana.Text = "1234567890";
            // 
            // numStaffCode
            // 
            this.numStaffCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numStaffCode.AllowDeleteToNull = true;
            this.numStaffCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numStaffCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numStaffCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.numStaffCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField1,
            numberIntegerPartDisplayField1});
            this.numStaffCode.DropDown.AllowDrop = false;
            this.numStaffCode.Fields.DecimalPart.MaxDigits = 0;
            this.numStaffCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numStaffCode.Fields.IntegerPart.MaxDigits = 9;
            this.numStaffCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numStaffCode.HighlightText = true;
            this.numStaffCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numStaffCode.Location = new System.Drawing.Point(173, 14);
            this.numStaffCode.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numStaffCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStaffCode.Name = "numStaffCode";
            this.numStaffCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numStaffCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numStaffCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numStaffCode.Size = new System.Drawing.Size(66, 28);
            this.numStaffCode.Spin.AllowSpin = false;
            this.numStaffCode.TabIndex = 0;
            this.numStaffCode.Value = new decimal(new int[] {
            123456,
            0,
            0,
            0});
            this.numStaffCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // edtEmail
            // 
            this.edtEmail.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtEmail.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtEmail.DropDown.AllowDrop = false;
            this.edtEmail.HighlightText = true;
            this.edtEmail.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtEmail.Location = new System.Drawing.Point(173, 125);
            this.edtEmail.MaxLength = 255;
            this.edtEmail.Name = "edtEmail";
            this.gcShortcut1.SetShortcuts(this.edtEmail, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtEmail.Size = new System.Drawing.Size(377, 28);
            this.edtEmail.TabIndex = 2;
            this.edtEmail.Text = resources.GetString("edtEmail.Text");
            // 
            // fpStaffListGrid_Sheet1
            // 
            this.fpStaffListGrid_Sheet1.Reset();
            this.fpStaffListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpStaffListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpStaffListGrid_Sheet1.ColumnCount = 3;
            this.fpStaffListGrid_Sheet1.RowCount = 18;
            this.fpStaffListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).ParseFormatString = "E";
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).Value = "20";
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).ParseFormatString = "E";
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).Value = "N-GMO";
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 0).Value = "12345";
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).ParseFormatString = "E";
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).Value = "123456789123456789123456789123456789123456789";
            this.fpStaffListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpStaffListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "コード";
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "乗務員名";
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "送信";
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpStaffListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpStaffListGrid_Sheet1.Columns.Get(0).Resizable = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).Width = 56F;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).Label = "乗務員名";
            this.fpStaffListGrid_Sheet1.Columns.Get(1).Resizable = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).Width = 186F;
            this.fpStaffListGrid_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpStaffListGrid_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpStaffListGrid_Sheet1.Columns.Get(2).Label = "送信";
            this.fpStaffListGrid_Sheet1.Columns.Get(2).Resizable = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(2).ShowSortIndicator = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpStaffListGrid_Sheet1.Columns.Get(2).Width = 48F;
            this.fpStaffListGrid_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.DefaultStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpStaffListGrid_Sheet1.FilterBar.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.FilterBar.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpStaffListGrid_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpStaffListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpStaffListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpStaffListGrid_Sheet1.Protect = true;
            this.fpStaffListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpStaffListGrid_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpStaffListGrid_Sheet1.RowHeader.Visible = false;
            this.fpStaffListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpStaffListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpStaffListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpStaffListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpStaffListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpStaffListGrid_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.SheetCornerStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpStaffListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpStaffListGrid
            // 
            this.fpStaffListGrid.AccessibleDescription = "fpStaffListGrid, Sheet1, Row 0, Column 0";
            this.fpStaffListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpStaffListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpStaffListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpStaffListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpStaffListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpStaffListGrid.HorizontalScrollBar.Name = "";
            this.fpStaffListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpStaffListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpStaffListGrid.Location = new System.Drawing.Point(3, 39);
            this.fpStaffListGrid.Name = "fpStaffListGrid";
            this.fpStaffListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpStaffListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpStaffListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpStaffListGrid_Sheet1});
            this.fpStaffListGrid.Size = new System.Drawing.Size(311, 448);
            this.fpStaffListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpStaffListGrid.TabIndex = 1;
            this.fpStaffListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpStaffListGrid.VerticalScrollBar.Name = "";
            this.fpStaffListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpStaffListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpStaffListGrid.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpStaffListGrid_SelectionChanged);
            this.fpStaffListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpStaffListGrid_CellClick);
            this.fpStaffListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpStaffListGrid_CellDoubleClick);
            this.fpStaffListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpStaffListGrid_PreviewKeyDown);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.edtSearch);
            this.pnlLeft.Controls.Add(this.fpStaffListGrid);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 24);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(317, 493);
            this.pnlLeft.TabIndex = 10;
            // 
            // toolStripEnd
            // 
            this.toolStripEnd.ForeColor = System.Drawing.Color.White;
            this.toolStripEnd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripEnd.Name = "toolStripEnd";
            this.toolStripEnd.Size = new System.Drawing.Size(58, 20);
            this.toolStripEnd.Text = "F1：終了";
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
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripReference,
            this.toolStripCancel,
            this.toolStripSave,
            this.toolStripRemove});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(887, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 203;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripReference
            // 
            this.toolStripReference.ForeColor = System.Drawing.Color.White;
            this.toolStripReference.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripReference.Name = "toolStripReference";
            this.toolStripReference.Size = new System.Drawing.Size(58, 20);
            this.toolStripReference.Text = "F5：参照";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(657, 6);
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
            this.btnDelete.Location = new System.Drawing.Point(770, 6);
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
            this.btnCancel.Location = new System.Drawing.Point(544, 6);
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
            this.label5.Size = new System.Drawing.Size(887, 1);
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
            this.pnlBottom.Location = new System.Drawing.Point(0, 517);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(887, 42);
            this.pnlBottom.TabIndex = 2;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.tabControl1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(321, 24);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(566, 493);
            this.pnlRight.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(566, 493);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.numStaffCode);
            this.tabPage1.Controls.Add(this.edtEmail);
            this.tabPage1.Controls.Add(this.edtStaffNameKana);
            this.tabPage1.Controls.Add(this.label55);
            this.tabPage1.Controls.Add(this.chkSendFlag);
            this.tabPage1.Controls.Add(this.edtStaffName);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(558, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本情報";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.CausesValidation = false;
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label55.Location = new System.Drawing.Point(16, 93);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(56, 18);
            this.label55.TabIndex = 1041;
            this.label55.Text = "フリガナ";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkSendFlag
            // 
            this.chkSendFlag.AutoSize = true;
            this.chkSendFlag.ForeColor = System.Drawing.Color.Black;
            this.chkSendFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkSendFlag.Location = new System.Drawing.Point(173, 166);
            this.chkSendFlag.Name = "chkSendFlag";
            this.chkSendFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkSendFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkSendFlag.Size = new System.Drawing.Size(75, 22);
            this.chkSendFlag.TabIndex = 17;
            this.chkSendFlag.TabStop = false;
            this.chkSendFlag.Text = "送信する";
            this.chkSendFlag.UseVisualStyleBackColor = true;
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
            this.label12.Size = new System.Drawing.Size(32, 18);
            this.label12.TabIndex = 157;
            this.label12.Text = "名称";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(16, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 18);
            this.label1.TabIndex = 164;
            this.label1.Text = "送信可否フラグ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.CausesValidation = false;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(16, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 18);
            this.label2.TabIndex = 164;
            this.label2.Text = "メールアドレス *";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.label6.Size = new System.Drawing.Size(44, 18);
            this.label6.TabIndex = 158;
            this.label6.Text = "コード";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(887, 24);
            this.menuStripTop.TabIndex = 9;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // HaisoShijiMailSendToListFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(887, 581);
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
            this.Name = "HaisoShijiMailSendToListFrame";
            this.Text = "HaisoShijiMailSendToListFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HaisoShijiMailSendToListFrame_FormClosing);
            this.Shown += new System.EventHandler(this.HaisoShijiMailSendToListFrame_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HaisoShijiMailSendToListFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffNameKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpStaffListGrid_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpStaffListGrid)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private FarPoint.Win.Spread.SheetView fpStaffListGrid_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpStaffListGrid;
        private GrapeCity.Win.Editors.GcTextBox edtSearch;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCancel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSave;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRemove;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private GrapeCity.Win.Editors.GcNumber numStaffCode;
        private GrapeCity.Win.Editors.GcTextBox edtStaffNameKana;
        private System.Windows.Forms.Label label55;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkSendFlag;
        private GrapeCity.Win.Editors.GcTextBox edtStaffName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.ToolStripStatusLabel toolStripReference;
        private GrapeCity.Win.Editors.GcTextBox edtEmail;
        private System.Windows.Forms.Label label1;
    }
}