namespace Jpsys.HaishaManageV10.Frame
{
    partial class CmnSearchContractFrame
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
            GrapeCity.Win.Editors.Fields.NumberSignDisplayField numberSignDisplayField2 = new GrapeCity.Win.Editors.Fields.NumberSignDisplayField();
            GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField numberIntegerPartDisplayField2 = new GrapeCity.Win.Editors.Fields.NumberIntegerPartDisplayField();
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSearchTokuisakiName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numSearchTokuisakiCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.sbtnSearchTokuisakiCode = new GrapeCity.Win.Editors.SideButton();
            this.numSearchClmClassCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.sbtnSearchClmClassCode = new GrapeCity.Win.Editors.SideButton();
            this.edtHyojiKensu = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSearch = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSelect = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.edtSearchClmClassName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.fpListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchTokuisakiName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchTokuisakiCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchClmClassCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtHyojiKensu)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchClmClassName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // edtSearchTokuisakiName
            // 
            this.edtSearchTokuisakiName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearchTokuisakiName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSearchTokuisakiName.DropDown.AllowDrop = false;
            this.edtSearchTokuisakiName.HighlightText = true;
            this.edtSearchTokuisakiName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSearchTokuisakiName.Location = new System.Drawing.Point(147, 7);
            this.edtSearchTokuisakiName.MaxLength = 999999999;
            this.edtSearchTokuisakiName.Name = "edtSearchTokuisakiName";
            this.edtSearchTokuisakiName.ReadOnly = true;
            this.edtSearchTokuisakiName.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.edtSearchTokuisakiName, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.edtSearchTokuisakiName.Size = new System.Drawing.Size(239, 28);
            this.edtSearchTokuisakiName.TabIndex = 1;
            this.edtSearchTokuisakiName.TabStop = false;
            this.edtSearchTokuisakiName.Text = "12345678901234567890";
            // 
            // numSearchTokuisakiCode
            // 
            this.numSearchTokuisakiCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numSearchTokuisakiCode.AllowDeleteToNull = true;
            this.numSearchTokuisakiCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numSearchTokuisakiCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numSearchTokuisakiCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.numSearchTokuisakiCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField1,
            numberIntegerPartDisplayField1});
            this.numSearchTokuisakiCode.DropDown.AllowDrop = false;
            this.numSearchTokuisakiCode.Fields.DecimalPart.MaxDigits = 0;
            this.numSearchTokuisakiCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numSearchTokuisakiCode.Fields.IntegerPart.MaxDigits = 8;
            this.numSearchTokuisakiCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numSearchTokuisakiCode.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numSearchTokuisakiCode.HighlightText = true;
            this.numSearchTokuisakiCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numSearchTokuisakiCode.Location = new System.Drawing.Point(58, 7);
            this.numSearchTokuisakiCode.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numSearchTokuisakiCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSearchTokuisakiCode.Name = "numSearchTokuisakiCode";
            this.numSearchTokuisakiCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numSearchTokuisakiCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numSearchTokuisakiCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numSearchTokuisakiCode.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sbtnSearchTokuisakiCode});
            this.numSearchTokuisakiCode.Size = new System.Drawing.Size(88, 28);
            this.numSearchTokuisakiCode.Spin.AllowSpin = false;
            this.numSearchTokuisakiCode.TabIndex = 0;
            this.numSearchTokuisakiCode.Value = new decimal(new int[] {
            12345678,
            0,
            0,
            0});
            this.numSearchTokuisakiCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            this.numSearchTokuisakiCode.DoubleClick += new System.EventHandler(this.sbtn_Click);
            // 
            // sbtnSearchTokuisakiCode
            // 
            this.sbtnSearchTokuisakiCode.ButtonWidth = 22;
            this.sbtnSearchTokuisakiCode.Name = "sbtnSearchTokuisakiCode";
            this.sbtnSearchTokuisakiCode.Text = "...";
            this.sbtnSearchTokuisakiCode.Click += new System.EventHandler(this.sbtn_Click);
            // 
            // numSearchClmClassCode
            // 
            this.numSearchClmClassCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numSearchClmClassCode.AllowDeleteToNull = true;
            this.numSearchClmClassCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numSearchClmClassCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numSearchClmClassCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField2.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField2.MinDigits = 0;
            this.numSearchClmClassCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField2,
            numberIntegerPartDisplayField2});
            this.numSearchClmClassCode.DropDown.AllowDrop = false;
            this.numSearchClmClassCode.Fields.DecimalPart.MaxDigits = 0;
            this.numSearchClmClassCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numSearchClmClassCode.Fields.IntegerPart.MaxDigits = 5;
            this.numSearchClmClassCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numSearchClmClassCode.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numSearchClmClassCode.HighlightText = true;
            this.numSearchClmClassCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numSearchClmClassCode.Location = new System.Drawing.Point(451, 7);
            this.numSearchClmClassCode.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numSearchClmClassCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSearchClmClassCode.Name = "numSearchClmClassCode";
            this.numSearchClmClassCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numSearchClmClassCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numSearchClmClassCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.numSearchClmClassCode.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sbtnSearchClmClassCode});
            this.numSearchClmClassCode.Size = new System.Drawing.Size(76, 28);
            this.numSearchClmClassCode.Spin.AllowSpin = false;
            this.numSearchClmClassCode.TabIndex = 2;
            this.numSearchClmClassCode.Value = new decimal(new int[] {
            12345,
            0,
            0,
            0});
            this.numSearchClmClassCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            this.numSearchClmClassCode.DoubleClick += new System.EventHandler(this.sbtn_Click);
            // 
            // sbtnSearchClmClassCode
            // 
            this.sbtnSearchClmClassCode.ButtonWidth = 22;
            this.sbtnSearchClmClassCode.Name = "sbtnSearchClmClassCode";
            this.sbtnSearchClmClassCode.Text = "...";
            this.sbtnSearchClmClassCode.Click += new System.EventHandler(this.sbtn_Click);
            // 
            // edtHyojiKensu
            // 
            this.edtHyojiKensu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.edtHyojiKensu.BackColor = System.Drawing.SystemColors.Control;
            this.edtHyojiKensu.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.edtHyojiKensu.DropDown.AllowDrop = false;
            this.edtHyojiKensu.HighlightText = true;
            this.edtHyojiKensu.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtHyojiKensu.Location = new System.Drawing.Point(86, 7);
            this.edtHyojiKensu.MaxLength = 20;
            this.edtHyojiKensu.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtHyojiKensu.Name = "edtHyojiKensu";
            this.edtHyojiKensu.ReadOnly = true;
            this.edtHyojiKensu.Size = new System.Drawing.Size(99, 28);
            this.edtHyojiKensu.TabIndex = 1005;
            this.edtHyojiKensu.TabStop = false;
            this.edtHyojiKensu.Text = "123,456,789件";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripSearch,
            this.toolStripSelect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 499);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(749, 22);
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
            // toolStripSearch
            // 
            this.toolStripSearch.ForeColor = System.Drawing.Color.White;
            this.toolStripSearch.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSearch.Name = "toolStripSearch";
            this.toolStripSearch.Size = new System.Drawing.Size(58, 20);
            this.toolStripSearch.Text = "F5：参照";
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
            this.panel2.Controls.Add(this.edtHyojiKensu);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 457);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(749, 42);
            this.panel2.TabIndex = 204;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.CausesValidation = false;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(12, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 18);
            this.label5.TabIndex = 1004;
            this.label5.Text = "表示件数：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(531, 6);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 0;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "選択";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(637, 6);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "終了";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStripTop.Size = new System.Drawing.Size(749, 24);
            this.menuStripTop.TabIndex = 206;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.edtSearchClmClassName);
            this.panel1.Controls.Add(this.edtSearchTokuisakiName);
            this.panel1.Controls.Add(this.numSearchClmClassCode);
            this.panel1.Controls.Add(this.numSearchTokuisakiCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.fpListGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(749, 433);
            this.panel1.TabIndex = 0;
            // 
            // edtSearchClmClassName
            // 
            this.edtSearchClmClassName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearchClmClassName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSearchClmClassName.DropDown.AllowDrop = false;
            this.edtSearchClmClassName.HighlightText = true;
            this.edtSearchClmClassName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSearchClmClassName.Location = new System.Drawing.Point(528, 7);
            this.edtSearchClmClassName.MaxLength = 999999999;
            this.edtSearchClmClassName.Name = "edtSearchClmClassName";
            this.edtSearchClmClassName.ReadOnly = true;
            this.edtSearchClmClassName.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.edtSearchClmClassName.Size = new System.Drawing.Size(200, 28);
            this.edtSearchClmClassName.TabIndex = 3;
            this.edtSearchClmClassName.TabStop = false;
            this.edtSearchClmClassName.Text = "12345678901234567890";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.CausesValidation = false;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(389, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 18);
            this.label1.TabIndex = 1059;
            this.label1.Text = "請求部門";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.CausesValidation = false;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(8, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 18);
            this.label11.TabIndex = 1059;
            this.label11.Text = "得意先";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fpListGrid
            // 
            this.fpListGrid.AccessibleDescription = "fpListGrid, Sheet1, Row 0, Column 0, 12345678";
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
            this.fpListGrid.Size = new System.Drawing.Size(743, 391);
            this.fpListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpListGrid.SpreadScaleMode = FarPoint.Win.Spread.ScaleMode.ZoomDpiSupport;
            this.fpListGrid.TabIndex = 4;
            this.fpListGrid.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.fpListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.VerticalScrollBar.Name = "";
            this.fpListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpListGrid_CellClick);
            this.fpListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpListGrid_CellDoubleClick);
            this.fpListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpListGrid_PreviewKeyDown);
            // 
            // fpListGrid_Sheet1
            // 
            this.fpListGrid_Sheet1.Reset();
            this.fpListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpListGrid_Sheet1.ColumnCount = 6;
            this.fpListGrid_Sheet1.RowCount = 30;
            this.fpListGrid_Sheet1.Cells.Get(0, 0).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 2).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 3).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(0, 4).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 5).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(1, 0).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 3).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 5).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(2, 0).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 3).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 5).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(3, 0).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 3).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 5).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(4, 0).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 3).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 5).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(5, 0).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 3).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 5).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(6, 0).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 3).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 5).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(7, 0).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 3).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 5).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(8, 0).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 3).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 5).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(9, 0).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 3).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 5).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(10, 0).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 3).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 5).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(11, 0).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 3).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 5).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(12, 0).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 3).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 5).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(13, 0).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 3).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 5).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(14, 0).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 3).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 5).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(15, 0).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 3).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 5).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(16, 0).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 3).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 5).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(17, 0).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 3).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 5).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(18, 0).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 3).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 5).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(19, 0).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 3).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 5).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(20, 0).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 3).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 5).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(21, 0).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 3).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 5).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(22, 0).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 3).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 5).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(23, 0).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 3).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 5).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(24, 0).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 3).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 5).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(25, 0).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 3).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 5).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(26, 0).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 3).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 5).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(27, 0).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 3).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 5).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(28, 0).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 3).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 5).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(29, 0).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 3).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 5).Value = 30D;
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
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "請負No";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "請負名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "請負期間";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "請負金額";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "税";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "通行料";
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpListGrid_Sheet1.Columns.Get(0).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(0).Label = "請負No";
            this.fpListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(0).Width = 66F;
            this.fpListGrid_Sheet1.Columns.Get(1).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpListGrid_Sheet1.Columns.Get(1).Label = "請負名";
            this.fpListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(1).Width = 266F;
            this.fpListGrid_Sheet1.Columns.Get(2).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpListGrid_Sheet1.Columns.Get(2).Label = "請負期間";
            this.fpListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Width = 178F;
            this.fpListGrid_Sheet1.Columns.Get(3).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.fpListGrid_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(3).Label = "請負金額";
            this.fpListGrid_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Width = 88F;
            this.fpListGrid_Sheet1.Columns.Get(4).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.fpListGrid_Sheet1.Columns.Get(4).Label = "税";
            this.fpListGrid_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(4).Width = 36F;
            this.fpListGrid_Sheet1.Columns.Get(5).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.fpListGrid_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(5).Label = "通行料";
            this.fpListGrid_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(5).Width = 88F;
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
            // CmnSearchContractFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(749, 521);
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
            this.Name = "CmnSearchContractFrame";
            this.Text = "請負検索";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmnSearchContractFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchTokuisakiName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchTokuisakiCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchClmClassCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtHyojiKensu)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchClmClassName)).EndInit();
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
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread fpListGrid;
        private FarPoint.Win.Spread.SheetView fpListGrid_Sheet1;
        private System.Windows.Forms.Label label5;
        private GrapeCity.Win.Editors.GcTextBox edtHyojiKensu;
        private GrapeCity.Win.Editors.GcTextBox edtSearchTokuisakiName;
        private GrapeCity.Win.Editors.GcNumber numSearchTokuisakiCode;
        private GrapeCity.Win.Editors.SideButton sbtnSearchTokuisakiCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSearch;
        private GrapeCity.Win.Editors.GcTextBox edtSearchClmClassName;
        private GrapeCity.Win.Editors.GcNumber numSearchClmClassCode;
        private GrapeCity.Win.Editors.SideButton sbtnSearchClmClassCode;
        private System.Windows.Forms.Label label1;
    }
}