namespace Jpsys.SagyoManage.Frame
{
    partial class MeishoFrame
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
            GrapeCity.Win.Editors.ListItem listItem1 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem1 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.SubItem subItem2 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ItemTemplate itemTemplate1 = new GrapeCity.Win.Editors.ItemTemplate();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ja-JP", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.numMeishoCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.cmbNameKbn = new GrapeCity.Win.Editors.GcComboBox(this.components);
            this.dropDownButton5 = new GrapeCity.Win.Editors.DropDownButton();
            this.edtMeishoName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtMeishoNameKana = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.fpMeishoListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.fpMeishoListGrid = new FarPoint.Win.Spread.FpSpread();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblKensu = new System.Windows.Forms.Label();
            this.chkAllFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripNew = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRemove = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label55 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkDisableFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.numMeishoCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNameKbn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMeishoName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMeishoNameKana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMeishoListGrid_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMeishoListGrid)).BeginInit();
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
            this.splitter1.Location = new System.Drawing.Point(326, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 493);
            this.splitter1.TabIndex = 181;
            this.splitter1.TabStop = false;
            // 
            // numMeishoCode
            // 
            this.numMeishoCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numMeishoCode.AllowDeleteToNull = true;
            this.numMeishoCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numMeishoCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numMeishoCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.numMeishoCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberIntegerPartDisplayField1});
            this.numMeishoCode.DropDown.AllowDrop = false;
            this.numMeishoCode.Fields.DecimalPart.MaxDigits = 0;
            this.numMeishoCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numMeishoCode.Fields.IntegerPart.MaxDigits = 6;
            this.numMeishoCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numMeishoCode.Fields.SignPrefix.NegativePattern = "";
            this.numMeishoCode.HighlightText = true;
            this.numMeishoCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numMeishoCode.Location = new System.Drawing.Point(173, 14);
            this.numMeishoCode.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numMeishoCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numMeishoCode.Name = "numMeishoCode";
            this.numMeishoCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numMeishoCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numMeishoCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numMeishoCode.Size = new System.Drawing.Size(57, 28);
            this.numMeishoCode.Spin.AllowSpin = false;
            this.numMeishoCode.TabIndex = 0;
            this.numMeishoCode.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            this.numMeishoCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // cmbNameKbn
            // 
            this.cmbNameKbn.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbNameKbn.DropDown.AllowResize = false;
            this.cmbNameKbn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNameKbn.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbNameKbn.ImeMode = System.Windows.Forms.ImeMode.Disable;
            subItem1.Value = "平成";
            subItem2.Value = "伊藤";
            listItem1.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem1,
            subItem2});
            this.cmbNameKbn.Items.AddRange(new GrapeCity.Win.Editors.ListItem[] {
            listItem1});
            this.cmbNameKbn.ListGridLines.VerticalLines = new GrapeCity.Win.Editors.Line(GrapeCity.Win.Editors.LineStyle.None, System.Drawing.Color.Empty);
            this.cmbNameKbn.ListHeaderPane.Height = 25;
            this.cmbNameKbn.ListHeaderPane.Visible = false;
            itemTemplate1.Height = 21;
            this.cmbNameKbn.ListItemTemplates.Add(itemTemplate1);
            this.cmbNameKbn.Location = new System.Drawing.Point(3, 9);
            this.cmbNameKbn.Name = "cmbNameKbn";
            this.gcShortcut1.SetShortcuts(this.cmbNameKbn, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.F12}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl"}));
            this.cmbNameKbn.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton5});
            this.cmbNameKbn.Size = new System.Drawing.Size(307, 24);
            this.cmbNameKbn.Spin.AllowSpin = false;
            this.cmbNameKbn.TabIndex = 2;
            this.cmbNameKbn.SelectedIndexChanged += new System.EventHandler(this.cmbNameKbn_SelectedIndexChanged);
            // 
            // dropDownButton5
            // 
            this.dropDownButton5.Name = "dropDownButton5";
            // 
            // edtMeishoName
            // 
            this.edtMeishoName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtMeishoName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtMeishoName.DropDown.AllowDrop = false;
            this.edtMeishoName.HighlightText = true;
            this.edtMeishoName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtMeishoName.Location = new System.Drawing.Point(173, 48);
            this.edtMeishoName.MaxLength = 30;
            this.edtMeishoName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtMeishoName.Name = "edtMeishoName";
            this.gcShortcut1.SetShortcuts(this.edtMeishoName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtMeishoName.Size = new System.Drawing.Size(247, 28);
            this.edtMeishoName.TabIndex = 1;
            this.edtMeishoName.Text = "123456789012345678901234567890";
            // 
            // edtMeishoNameKana
            // 
            this.edtMeishoNameKana.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtMeishoNameKana.AutoConvert = false;
            this.edtMeishoNameKana.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtMeishoNameKana.DropDown.AllowDrop = false;
            this.edtMeishoNameKana.HighlightText = true;
            this.edtMeishoNameKana.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtMeishoNameKana.Location = new System.Drawing.Point(173, 82);
            this.edtMeishoNameKana.MaxLength = 10;
            this.edtMeishoNameKana.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtMeishoNameKana.Name = "edtMeishoNameKana";
            this.gcShortcut1.SetShortcuts(this.edtMeishoNameKana, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtMeishoNameKana.Size = new System.Drawing.Size(247, 28);
            this.edtMeishoNameKana.TabIndex = 2;
            this.edtMeishoNameKana.Text = "1234567890";
            // 
            // fpMeishoListGrid_Sheet1
            // 
            this.fpMeishoListGrid_Sheet1.Reset();
            this.fpMeishoListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpMeishoListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpMeishoListGrid_Sheet1.ColumnCount = 2;
            this.fpMeishoListGrid_Sheet1.RowCount = 18;
            this.fpMeishoListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpMeishoListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpMeishoListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpMeishoListGrid_Sheet1.Cells.Get(0, 0).ParseFormatString = "E";
            this.fpMeishoListGrid_Sheet1.Cells.Get(0, 0).Value = "1";
            this.fpMeishoListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpMeishoListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpMeishoListGrid_Sheet1.Cells.Get(0, 1).ParseFormatString = "E";
            this.fpMeishoListGrid_Sheet1.Cells.Get(0, 1).Value = "作業員名称１";
            this.fpMeishoListGrid_Sheet1.Cells.Get(1, 0).Value = "999";
            this.fpMeishoListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpMeishoListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpMeishoListGrid_Sheet1.Cells.Get(1, 1).ParseFormatString = "E";
            this.fpMeishoListGrid_Sheet1.Cells.Get(1, 1).Value = "作業員名称２";
            this.fpMeishoListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpMeishoListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "コード";
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "名称";
            this.fpMeishoListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpMeishoListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpMeishoListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpMeishoListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpMeishoListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpMeishoListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpMeishoListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpMeishoListGrid_Sheet1.Columns.Get(0).Width = 56F;
            this.fpMeishoListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpMeishoListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpMeishoListGrid_Sheet1.Columns.Get(1).Label = "名称";
            this.fpMeishoListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpMeishoListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpMeishoListGrid_Sheet1.Columns.Get(1).Width = 243F;
            this.fpMeishoListGrid_Sheet1.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.DefaultStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fpMeishoListGrid_Sheet1.FilterBar.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.FilterBar.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpMeishoListGrid_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpMeishoListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpMeishoListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpMeishoListGrid_Sheet1.Protect = true;
            this.fpMeishoListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpMeishoListGrid_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpMeishoListGrid_Sheet1.RowHeader.Visible = false;
            this.fpMeishoListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpMeishoListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpMeishoListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpMeishoListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpMeishoListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpMeishoListGrid_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpMeishoListGrid_Sheet1.SheetCornerStyle.Locked = false;
            this.fpMeishoListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpMeishoListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // fpMeishoListGrid
            // 
            this.fpMeishoListGrid.AccessibleDescription = "fpMeishoListGrid, Sheet1, Row 0, Column 0";
            this.fpMeishoListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpMeishoListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpMeishoListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpMeishoListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpMeishoListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpMeishoListGrid.HorizontalScrollBar.Name = "";
            this.fpMeishoListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpMeishoListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpMeishoListGrid.Location = new System.Drawing.Point(3, 39);
            this.fpMeishoListGrid.Name = "fpMeishoListGrid";
            this.fpMeishoListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpMeishoListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpMeishoListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpMeishoListGrid_Sheet1});
            this.fpMeishoListGrid.Size = new System.Drawing.Size(320, 412);
            this.fpMeishoListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpMeishoListGrid.TabIndex = 1;
            this.fpMeishoListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpMeishoListGrid.VerticalScrollBar.Name = "";
            this.fpMeishoListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpMeishoListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpMeishoListGrid.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpMeishoListGrid_SelectionChanged);
            this.fpMeishoListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpMeishoListGrid_CellClick);
            this.fpMeishoListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpMeishoListGrid_CellDoubleClick);
            this.fpMeishoListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpMeishoListGrid_PreviewKeyDown);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNew.Location = new System.Drawing.Point(211, 457);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(105, 30);
            this.btnNew.TabIndex = 1;
            this.btnNew.TabStop = false;
            this.btnNew.Text = "新規作成";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lblKensu);
            this.pnlLeft.Controls.Add(this.chkAllFlag);
            this.pnlLeft.Controls.Add(this.cmbNameKbn);
            this.pnlLeft.Controls.Add(this.btnNew);
            this.pnlLeft.Controls.Add(this.fpMeishoListGrid);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 24);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(326, 493);
            this.pnlLeft.TabIndex = 10;
            // 
            // lblKensu
            // 
            this.lblKensu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKensu.BackColor = System.Drawing.Color.Transparent;
            this.lblKensu.CausesValidation = false;
            this.lblKensu.ForeColor = System.Drawing.Color.Black;
            this.lblKensu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblKensu.Location = new System.Drawing.Point(150, 463);
            this.lblKensu.Name = "lblKensu";
            this.lblKensu.Size = new System.Drawing.Size(55, 18);
            this.lblKensu.TabIndex = 1070;
            this.lblKensu.Text = "9999件";
            this.lblKensu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAllFlag
            // 
            this.chkAllFlag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAllFlag.AutoSize = true;
            this.chkAllFlag.Checked = true;
            this.chkAllFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAllFlag.Location = new System.Drawing.Point(12, 462);
            this.chkAllFlag.Name = "chkAllFlag";
            this.chkAllFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkAllFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkAllFlag.Size = new System.Drawing.Size(99, 22);
            this.chkAllFlag.TabIndex = 1069;
            this.chkAllFlag.TabStop = false;
            this.chkAllFlag.Text = "使用停止表示";
            this.chkAllFlag.UseVisualStyleBackColor = true;
            this.chkAllFlag.CheckedChanged += new System.EventHandler(this.chkAllFlag_CheckedChanged);
            // 
            // toolStripEnd
            // 
            this.toolStripEnd.ForeColor = System.Drawing.Color.White;
            this.toolStripEnd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripEnd.Name = "toolStripEnd";
            this.toolStripEnd.Size = new System.Drawing.Size(58, 20);
            this.toolStripEnd.Text = "F1：終了";
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
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripNew,
            this.toolStripCancel,
            this.toolStripSave,
            this.toolStripRemove});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(785, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 203;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(668, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDelete.Location = new System.Drawing.Point(12, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(105, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(555, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "編集取消";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.label5.Size = new System.Drawing.Size(785, 1);
            this.label5.TabIndex = 82;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnDelete);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 517);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(785, 42);
            this.pnlBottom.TabIndex = 2;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.tabControl1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(330, 24);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(455, 493);
            this.pnlRight.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 487);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edtMeishoName);
            this.tabPage1.Controls.Add(this.label55);
            this.tabPage1.Controls.Add(this.edtMeishoNameKana);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.numMeishoCode);
            this.tabPage1.Controls.Add(this.chkDisableFlag);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 456);
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
            this.label55.Location = new System.Drawing.Point(16, 87);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(56, 18);
            this.label55.TabIndex = 1045;
            this.label55.Text = "フリガナ";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.CausesValidation = false;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(16, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 18);
            this.label12.TabIndex = 1044;
            this.label12.Text = "名称 *";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDisableFlag
            // 
            this.chkDisableFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDisableFlag.AutoSize = true;
            this.chkDisableFlag.ForeColor = System.Drawing.Color.Red;
            this.chkDisableFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDisableFlag.Location = new System.Drawing.Point(361, 427);
            this.chkDisableFlag.Name = "chkDisableFlag";
            this.chkDisableFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkDisableFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkDisableFlag.Size = new System.Drawing.Size(75, 22);
            this.chkDisableFlag.TabIndex = 3;
            this.chkDisableFlag.TabStop = false;
            this.chkDisableFlag.Text = "使用停止";
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
            this.menuStripTop.Size = new System.Drawing.Size(785, 24);
            this.menuStripTop.TabIndex = 9;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // MeishoFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(785, 581);
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
            this.Name = "MeishoFrame";
            this.Text = "MeishoFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MeishoFrame_FormClosing);
            this.Shown += new System.EventHandler(this.MeishoFrame_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numMeishoCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNameKbn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMeishoName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtMeishoNameKana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMeishoListGrid_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpMeishoListGrid)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
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
        private FarPoint.Win.Spread.SheetView fpMeishoListGrid_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpMeishoListGrid;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.ToolStripStatusLabel toolStripNew;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCancel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSave;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRemove;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private GrapeCity.Win.Editors.GcNumber numMeishoCode;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkDisableFlag;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private GrapeCity.Win.Editors.GcComboBox cmbNameKbn;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton5;
        private GrapeCity.Win.Editors.GcTextBox edtMeishoName;
        private System.Windows.Forms.Label label55;
        private GrapeCity.Win.Editors.GcTextBox edtMeishoNameKana;
        private System.Windows.Forms.Label label12;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkAllFlag;
        private System.Windows.Forms.Label lblKensu;
    }
}