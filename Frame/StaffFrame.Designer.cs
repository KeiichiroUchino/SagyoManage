namespace Jpsys.SagyoManage.Frame
{
    partial class StaffFrame
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
            GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField numberIntegerPartDisplayField3 = new GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField();
            GrapeCity.Win.Editors.Fields.NumberSignDisplayField numberSignDisplayField2 = new GrapeCity.Win.Editors.Fields.NumberSignDisplayField();
            GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField numberIntegerPartDisplayField4 = new GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ja-JP", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer2 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSearch = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtStaffName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numStaffCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.edtEmail = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtTel = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtStaffKbnNm = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numStaffKbnCd = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.sideButton6 = new GrapeCity.Win.Editors.SideButton();
            this.fpStaffListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.fpStaffListGrid = new FarPoint.Win.Spread.FpSpread();
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
            this.label24 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.chkDisableFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.toolStripSearch = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStaffCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffKbnNm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStaffKbnCd)).BeginInit();
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
            this.splitter1.Location = new System.Drawing.Point(326, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 493);
            this.splitter1.TabIndex = 181;
            this.splitter1.TabStop = false;
            // 
            // edtSearch
            // 
            this.edtSearch.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearch.AlternateText.DisplayNull.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.DisplayNull.Text = "社員の検索";
            this.edtSearch.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearch.AlternateText.Null.Text = "社員の検索";
            this.edtSearch.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSearch.DropDown.AllowDrop = false;
            this.edtSearch.HighlightText = true;
            this.edtSearch.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSearch.Location = new System.Drawing.Point(5, 5);
            this.edtSearch.MaxLength = 20;
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
            this.edtSearch.Text = "12345678901234567890";
            this.edtSearch.TextChanged += new System.EventHandler(this.edtSearch_TextChanged);
            // 
            // edtStaffName
            // 
            this.edtStaffName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtStaffName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtStaffName.DropDown.AllowDrop = false;
            this.edtStaffName.HighlightText = true;
            this.edtStaffName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtStaffName.Location = new System.Drawing.Point(173, 48);
            this.edtStaffName.MaxLength = 20;
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
            this.edtStaffName.Text = "1234567890";
            // 
            // numStaffCode
            // 
            this.numStaffCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numStaffCode.AllowDeleteToNull = true;
            this.numStaffCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numStaffCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numStaffCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField3.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField3.MinDigits = 0;
            this.numStaffCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberIntegerPartDisplayField3});
            this.numStaffCode.DropDown.AllowDrop = false;
            this.numStaffCode.Fields.DecimalPart.MaxDigits = 0;
            this.numStaffCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numStaffCode.Fields.IntegerPart.MaxDigits = 6;
            this.numStaffCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numStaffCode.Fields.SignPrefix.NegativePattern = "";
            this.numStaffCode.HighlightText = true;
            this.numStaffCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numStaffCode.Location = new System.Drawing.Point(173, 14);
            this.numStaffCode.MaxValue = new decimal(new int[] {
            99999,
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
            this.numStaffCode.Size = new System.Drawing.Size(57, 28);
            this.numStaffCode.Spin.AllowSpin = false;
            this.numStaffCode.TabIndex = 0;
            this.numStaffCode.Value = new decimal(new int[] {
            12345,
            0,
            0,
            0});
            this.numStaffCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // edtEmail
            // 
            this.edtEmail.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtEmail.DropDown.AllowDrop = false;
            this.edtEmail.HighlightText = true;
            this.edtEmail.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtEmail.Location = new System.Drawing.Point(172, 150);
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
            this.edtEmail.Size = new System.Drawing.Size(376, 28);
            this.edtEmail.TabIndex = 5;
            this.edtEmail.Text = "12345678901234567890";
            // 
            // edtTel
            // 
            this.edtTel.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtTel.DropDown.AllowDrop = false;
            this.edtTel.HighlightText = true;
            this.edtTel.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.edtTel.Location = new System.Drawing.Point(172, 116);
            this.edtTel.MaxLength = 14;
            this.edtTel.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtTel.Name = "edtTel";
            this.gcShortcut1.SetShortcuts(this.edtTel, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtTel.Size = new System.Drawing.Size(224, 28);
            this.edtTel.TabIndex = 4;
            this.edtTel.Text = "12345678901234";
            // 
            // edtStaffKbnNm
            // 
            this.edtStaffKbnNm.ActiveBackColor = System.Drawing.SystemColors.Control;
            this.edtStaffKbnNm.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtStaffKbnNm.DropDown.AllowDrop = false;
            this.edtStaffKbnNm.HighlightText = true;
            this.edtStaffKbnNm.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtStaffKbnNm.Location = new System.Drawing.Point(248, 82);
            this.edtStaffKbnNm.MaxLength = 30;
            this.edtStaffKbnNm.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtStaffKbnNm.Name = "edtStaffKbnNm";
            this.edtStaffKbnNm.ReadOnly = true;
            this.edtStaffKbnNm.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.edtStaffKbnNm, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtStaffKbnNm.Size = new System.Drawing.Size(252, 28);
            this.edtStaffKbnNm.TabIndex = 3;
            this.edtStaffKbnNm.TabStop = false;
            this.edtStaffKbnNm.Text = "123456789012345678901234567890";
            // 
            // numStaffKbnCd
            // 
            this.numStaffKbnCd.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numStaffKbnCd.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numStaffKbnCd.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numStaffKbnCd.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField4.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField4.MinDigits = 0;
            this.numStaffKbnCd.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField2,
            numberIntegerPartDisplayField4});
            this.numStaffKbnCd.DropDown.AllowDrop = false;
            this.numStaffKbnCd.Fields.DecimalPart.MaxDigits = 0;
            this.numStaffKbnCd.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numStaffKbnCd.Fields.IntegerPart.MaxDigits = 5;
            this.numStaffKbnCd.Fields.IntegerPart.SpinIncrement = 1;
            this.numStaffKbnCd.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numStaffKbnCd.HighlightText = true;
            this.numStaffKbnCd.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numStaffKbnCd.Location = new System.Drawing.Point(172, 82);
            this.numStaffKbnCd.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numStaffKbnCd.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStaffKbnCd.Name = "numStaffKbnCd";
            this.numStaffKbnCd.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numStaffKbnCd.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numStaffKbnCd, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numStaffKbnCd.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sideButton6});
            this.numStaffKbnCd.Size = new System.Drawing.Size(75, 28);
            this.numStaffKbnCd.Spin.AllowSpin = false;
            this.numStaffKbnCd.TabIndex = 2;
            this.numStaffKbnCd.Value = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numStaffKbnCd.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // sideButton6
            // 
            this.sideButton6.ButtonWidth = 22;
            this.sideButton6.Name = "sideButton6";
            this.sideButton6.Text = "...";
            this.sideButton6.Click += new System.EventHandler(this.sbtn_Click);
            // 
            // fpStaffListGrid_Sheet1
            // 
            this.fpStaffListGrid_Sheet1.Reset();
            this.fpStaffListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpStaffListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpStaffListGrid_Sheet1.ColumnCount = 2;
            this.fpStaffListGrid_Sheet1.RowCount = 18;
            this.fpStaffListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).ParseFormatString = "E";
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 0).Value = "1";
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).ParseFormatString = "E";
            this.fpStaffListGrid_Sheet1.Cells.Get(0, 1).Value = "作業員名称１";
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 0).Value = "999";
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).ParseFormatString = "E";
            this.fpStaffListGrid_Sheet1.Cells.Get(1, 1).Value = "作業員名称２";
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
            this.fpStaffListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "社員名";
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpStaffListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpStaffListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpStaffListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpStaffListGrid_Sheet1.Columns.Get(0).Width = 56F;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).CellType = textCellType4;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).Label = "社員名";
            this.fpStaffListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpStaffListGrid_Sheet1.Columns.Get(1).Width = 243F;
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
            this.fpStaffListGrid.AccessibleDescription = "fpHomenListGrid, Sheet1, Row 0, Column 0";
            this.fpStaffListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpStaffListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpStaffListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpStaffListGrid.FocusRenderer = defaultFocusIndicatorRenderer2;
            this.fpStaffListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpStaffListGrid.HorizontalScrollBar.Name = "";
            this.fpStaffListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
            this.fpStaffListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpStaffListGrid.Location = new System.Drawing.Point(3, 39);
            this.fpStaffListGrid.Name = "fpStaffListGrid";
            this.fpStaffListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpStaffListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpStaffListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpStaffListGrid_Sheet1});
            this.fpStaffListGrid.Size = new System.Drawing.Size(320, 412);
            this.fpStaffListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpStaffListGrid.TabIndex = 1;
            this.fpStaffListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpStaffListGrid.VerticalScrollBar.Name = "";
            this.fpStaffListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
            this.fpStaffListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpStaffListGrid.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(this.fpStaffListGrid_SelectionChanged);
            this.fpStaffListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpStaffListGrid_CellClick);
            this.fpStaffListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpStaffListGrid_CellDoubleClick);
            this.fpStaffListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpStaffListGrid_PreviewKeyDown);
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
            this.pnlLeft.Controls.Add(this.edtSearch);
            this.pnlLeft.Controls.Add(this.btnNew);
            this.pnlLeft.Controls.Add(this.fpStaffListGrid);
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
            this.lblKensu.Location = new System.Drawing.Point(157, 463);
            this.lblKensu.Name = "lblKensu";
            this.lblKensu.Size = new System.Drawing.Size(48, 18);
            this.lblKensu.TabIndex = 1071;
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
            this.chkAllFlag.TabIndex = 1068;
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
            this.toolStripSearch,
            this.toolStripCancel,
            this.toolStripSave,
            this.toolStripRemove});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(906, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 203;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(789, 6);
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
            this.btnDelete.TabIndex = 1;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(676, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 30);
            this.btnCancel.TabIndex = 0;
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
            this.label5.Size = new System.Drawing.Size(906, 1);
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
            this.pnlBottom.Size = new System.Drawing.Size(906, 42);
            this.pnlBottom.TabIndex = 2;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.tabControl1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(330, 24);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(576, 493);
            this.pnlRight.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(576, 493);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edtStaffKbnNm);
            this.tabPage1.Controls.Add(this.numStaffKbnCd);
            this.tabPage1.Controls.Add(this.label24);
            this.tabPage1.Controls.Add(this.edtEmail);
            this.tabPage1.Controls.Add(this.edtTel);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numStaffCode);
            this.tabPage1.Controls.Add(this.edtStaffName);
            this.tabPage1.Controls.Add(this.label55);
            this.tabPage1.Controls.Add(this.chkDisableFlag);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(568, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本情報";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.CausesValidation = false;
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label24.Location = new System.Drawing.Point(16, 87);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(67, 18);
            this.label24.TabIndex = 1046;
            this.label24.Text = "社員区分 *";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.CausesValidation = false;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(16, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 18);
            this.label11.TabIndex = 1044;
            this.label11.Text = "メールアドレス";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(16, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 1045;
            this.label1.Text = "電話番号";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.CausesValidation = false;
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label55.Location = new System.Drawing.Point(16, 53);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(55, 18);
            this.label55.TabIndex = 1041;
            this.label55.Text = "社員名 *";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDisableFlag
            // 
            this.chkDisableFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDisableFlag.AutoSize = true;
            this.chkDisableFlag.ForeColor = System.Drawing.Color.Red;
            this.chkDisableFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDisableFlag.Location = new System.Drawing.Point(485, 430);
            this.chkDisableFlag.Name = "chkDisableFlag";
            this.chkDisableFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkDisableFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkDisableFlag.Size = new System.Drawing.Size(75, 22);
            this.chkDisableFlag.TabIndex = 6;
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
            this.menuStripTop.Size = new System.Drawing.Size(906, 24);
            this.menuStripTop.TabIndex = 9;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // toolStripSearch
            // 
            this.toolStripSearch.ForeColor = System.Drawing.Color.White;
            this.toolStripSearch.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSearch.Name = "toolStripSearch";
            this.toolStripSearch.Size = new System.Drawing.Size(58, 20);
            this.toolStripSearch.Text = "F5：検索";
            // 
            // StaffFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(906, 581);
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
            this.Name = "StaffFrame";
            this.Text = "StaffFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StaffFrame_FormClosing);
            this.Shown += new System.EventHandler(this.StaffFrame_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StaffFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStaffCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtTel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtStaffKbnNm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStaffKbnCd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpStaffListGrid_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpStaffListGrid)).EndInit();
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
        private FarPoint.Win.Spread.SheetView fpStaffListGrid_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpStaffListGrid;
        private System.Windows.Forms.Button btnNew;
        private GrapeCity.Win.Editors.GcTextBox edtSearch;
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
        private GrapeCity.Win.Editors.GcNumber numStaffCode;
        private GrapeCity.Win.Editors.GcTextBox edtStaffName;
        private System.Windows.Forms.Label label55;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkDisableFlag;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private GrapeCity.Win.Editors.GcTextBox edtEmail;
        private GrapeCity.Win.Editors.GcTextBox edtTel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label24;
        private GrapeCity.Win.Editors.GcTextBox edtStaffKbnNm;
        private GrapeCity.Win.Editors.GcNumber numStaffKbnCd;
        private GrapeCity.Win.Editors.SideButton sideButton6;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkAllFlag;
        private System.Windows.Forms.Label lblKensu;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSearch;
    }
}