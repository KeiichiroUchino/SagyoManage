namespace Jpsys.HaishaManageV10.Frame
{
    partial class PointGroupFrame
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
            this.edtPointGroupName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtPointGroupNameKana = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numPointGroupCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.fpPointGroupListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.fpPointGroupListGrid = new FarPoint.Win.Spread.FpSpread();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRemove = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripNew = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRefarense = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.chkDisableFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPointGroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPointGroupNameKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPointGroupCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPointGroupListGrid_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPointGroupListGrid)).BeginInit();
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
            this.edtSearch.AlternateText.DisplayNull.Text = "発着地グループの検索";
            this.edtSearch.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.Null.Text = "発着地グループの検索";
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
            // edtPointGroupName
            // 
            this.edtPointGroupName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtPointGroupName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtPointGroupName.DropDown.AllowDrop = false;
            this.edtPointGroupName.HighlightText = true;
            this.edtPointGroupName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtPointGroupName.Location = new System.Drawing.Point(173, 51);
            this.edtPointGroupName.MaxLength = 20;
            this.edtPointGroupName.Name = "edtPointGroupName";
            this.gcShortcut1.SetShortcuts(this.edtPointGroupName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtPointGroupName.Size = new System.Drawing.Size(247, 28);
            this.edtPointGroupName.TabIndex = 1;
            this.edtPointGroupName.Text = "12345678901234567890";
            // 
            // edtPointGroupNameKana
            // 
            this.edtPointGroupNameKana.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtPointGroupNameKana.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtPointGroupNameKana.DropDown.AllowDrop = false;
            this.edtPointGroupNameKana.HighlightText = true;
            this.edtPointGroupNameKana.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtPointGroupNameKana.Location = new System.Drawing.Point(173, 88);
            this.edtPointGroupNameKana.MaxLength = 20;
            this.edtPointGroupNameKana.Name = "edtPointGroupNameKana";
            this.gcShortcut1.SetShortcuts(this.edtPointGroupNameKana, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtPointGroupNameKana.Size = new System.Drawing.Size(247, 28);
            this.edtPointGroupNameKana.TabIndex = 2;
            this.edtPointGroupNameKana.Text = "12345678901234567890";
            // 
            // numPointGroupCode
            // 
            this.numPointGroupCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numPointGroupCode.AllowDeleteToNull = true;
            this.numPointGroupCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numPointGroupCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numPointGroupCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.numPointGroupCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField1,
            numberIntegerPartDisplayField1});
            this.numPointGroupCode.DropDown.AllowDrop = false;
            this.numPointGroupCode.Fields.DecimalPart.MaxDigits = 0;
            this.numPointGroupCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numPointGroupCode.Fields.IntegerPart.MaxDigits = 3;
            this.numPointGroupCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numPointGroupCode.HighlightText = true;
            this.numPointGroupCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numPointGroupCode.Location = new System.Drawing.Point(173, 14);
            this.numPointGroupCode.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numPointGroupCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPointGroupCode.Name = "numPointGroupCode";
            this.numPointGroupCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numPointGroupCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numPointGroupCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numPointGroupCode.Size = new System.Drawing.Size(40, 28);
            this.numPointGroupCode.Spin.AllowSpin = false;
            this.numPointGroupCode.TabIndex = 0;
            this.numPointGroupCode.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            this.numPointGroupCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // fpPointGroupListGrid_Sheet1
            // 
            this.fpPointGroupListGrid_Sheet1.Reset();
            this.fpPointGroupListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpPointGroupListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpPointGroupListGrid_Sheet1.ColumnCount = 3;
            this.fpPointGroupListGrid_Sheet1.RowCount = 18;
            this.fpPointGroupListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 0).ParseFormatString = "E";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 0).Value = "1";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 1).ParseFormatString = "E";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 1).Value = "飼料";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 2).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 2).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 2).ParseFormatString = "E";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(0, 2).Value = "ｼﾘｮｳ";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(1, 0).Value = "12345";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpPointGroupListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpPointGroupListGrid_Sheet1.Cells.Get(1, 1).ParseFormatString = "E";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(1, 1).Value = "123456789123456789123456789123456789123456789";
            this.fpPointGroupListGrid_Sheet1.Cells.Get(1, 2).Value = "12345678901234567890";
            this.fpPointGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpPointGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "コード";
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "発着地グループ名";
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "フリガナ";
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpPointGroupListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpPointGroupListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(0).Width = 56F;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(1).Label = "発着地グループ名";
            this.fpPointGroupListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(1).Width = 166F;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(2).Label = "フリガナ";
            this.fpPointGroupListGrid_Sheet1.Columns.Get(2).ShowSortIndicator = false;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpPointGroupListGrid_Sheet1.Columns.Get(2).Width = 68F;
            this.fpPointGroupListGrid_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.DefaultStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpPointGroupListGrid_Sheet1.FilterBar.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.FilterBar.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpPointGroupListGrid_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpPointGroupListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpPointGroupListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpPointGroupListGrid_Sheet1.Protect = true;
            this.fpPointGroupListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpPointGroupListGrid_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpPointGroupListGrid_Sheet1.RowHeader.Visible = false;
            this.fpPointGroupListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpPointGroupListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.fpPointGroupListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpPointGroupListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpPointGroupListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpPointGroupListGrid_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpPointGroupListGrid_Sheet1.SheetCornerStyle.Locked = false;
            this.fpPointGroupListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpPointGroupListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpPointGroupListGrid
            // 
            this.fpPointGroupListGrid.AccessibleDescription = "fpPointGroupListGrid, Sheet1, Row 0, Column 0, 1";
            this.fpPointGroupListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpPointGroupListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpPointGroupListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpPointGroupListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpPointGroupListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpPointGroupListGrid.HorizontalScrollBar.Name = "";
            this.fpPointGroupListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpPointGroupListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpPointGroupListGrid.Location = new System.Drawing.Point(3, 39);
            this.fpPointGroupListGrid.Name = "fpPointGroupListGrid";
            this.fpPointGroupListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpPointGroupListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpPointGroupListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpPointGroupListGrid_Sheet1});
            this.fpPointGroupListGrid.Size = new System.Drawing.Size(311, 412);
            this.fpPointGroupListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpPointGroupListGrid.TabIndex = 1;
            this.fpPointGroupListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpPointGroupListGrid.VerticalScrollBar.Name = "";
            this.fpPointGroupListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpPointGroupListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpPointGroupListGrid.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpPointGroupListGrid_SelectionChanged);
            this.fpPointGroupListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpPointGroupListGrid_CellClick);
            this.fpPointGroupListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpPointGroupListGrid_CellDoubleClick);
            this.fpPointGroupListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpPointGroupListGrid_PreviewKeyDown);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.btnNew);
            this.pnlLeft.Controls.Add(this.edtSearch);
            this.pnlLeft.Controls.Add(this.fpPointGroupListGrid);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 24);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(317, 493);
            this.pnlLeft.TabIndex = 10;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNew.Location = new System.Drawing.Point(202, 457);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(105, 30);
            this.btnNew.TabIndex = 2;
            this.btnNew.TabStop = false;
            this.btnNew.Text = "新規作成";
            this.btnNew.UseVisualStyleBackColor = true;
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
            this.toolStripNew,
            this.toolStripRefarense,
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
            // toolStripNew
            // 
            this.toolStripNew.ForeColor = System.Drawing.Color.White;
            this.toolStripNew.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripNew.Name = "toolStripNew";
            this.toolStripNew.Size = new System.Drawing.Size(82, 20);
            this.toolStripNew.Text = "F2：新規作成";
            // 
            // toolStripRefarense
            // 
            this.toolStripRefarense.ForeColor = System.Drawing.Color.White;
            this.toolStripRefarense.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripRefarense.Name = "toolStripRefarense";
            this.toolStripRefarense.Size = new System.Drawing.Size(58, 20);
            this.toolStripRefarense.Text = "F5：参照";
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
            this.tabPage1.Controls.Add(this.numPointGroupCode);
            this.tabPage1.Controls.Add(this.edtPointGroupNameKana);
            this.tabPage1.Controls.Add(this.label55);
            this.tabPage1.Controls.Add(this.chkDisableFlag);
            this.tabPage1.Controls.Add(this.edtPointGroupName);
            this.tabPage1.Controls.Add(this.label12);
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
            // chkDisableFlag
            // 
            this.chkDisableFlag.AutoSize = true;
            this.chkDisableFlag.ForeColor = System.Drawing.Color.Red;
            this.chkDisableFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDisableFlag.Location = new System.Drawing.Point(442, 419);
            this.chkDisableFlag.Name = "chkDisableFlag";
            this.chkDisableFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkDisableFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkDisableFlag.Size = new System.Drawing.Size(87, 22);
            this.chkDisableFlag.TabIndex = 4;
            this.chkDisableFlag.TabStop = false;
            this.chkDisableFlag.Text = "無効にする";
            this.chkDisableFlag.UseVisualStyleBackColor = true;
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
            // PointGroupFrame
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
            this.Name = "PointGroupFrame";
            this.Text = "PointGroupFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointGroupFrame_FormClosing);
            this.Shown += new System.EventHandler(this.PointGroupFrame_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PointGroupFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPointGroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtPointGroupNameKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPointGroupCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPointGroupListGrid_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPointGroupListGrid)).EndInit();
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
        private FarPoint.Win.Spread.SheetView fpPointGroupListGrid_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpPointGroupListGrid;
        private GrapeCity.Win.Editors.GcTextBox edtSearch;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNew;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRefarense;
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
        private GrapeCity.Win.Editors.GcNumber numPointGroupCode;
        private GrapeCity.Win.Editors.GcTextBox edtPointGroupNameKana;
        private System.Windows.Forms.Label label55;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkDisableFlag;
        private GrapeCity.Win.Editors.GcTextBox edtPointGroupName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.Button btnNew;
    }
}