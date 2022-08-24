namespace Jpsys.HaishaManageV10.Frame
{
    partial class CmnSearchHanroFrame
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.edtSearchText = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.edtSearchTokuisakiName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.numSearchTokuisakiCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.sbtnSearchTokuisakiCode = new GrapeCity.Win.Editors.SideButton();
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
            this.label11 = new System.Windows.Forms.Label();
            this.chkAllFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.fpListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchTokuisakiName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchTokuisakiCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtHyojiKensu)).BeginInit();
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
            this.edtSearchText.AlternateText.DisplayNull.Text = "販路の検索";
            this.edtSearchText.AlternateText.Null.ForeColor = System.Drawing.Color.DarkGray;
            this.edtSearchText.AlternateText.Null.Text = "販路の検索";
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
            // edtSearchTokuisakiName
            // 
            this.edtSearchTokuisakiName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtSearchTokuisakiName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtSearchTokuisakiName.DropDown.AllowDrop = false;
            this.edtSearchTokuisakiName.HighlightText = true;
            this.edtSearchTokuisakiName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtSearchTokuisakiName.Location = new System.Drawing.Point(428, 7);
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
            this.edtSearchTokuisakiName.Size = new System.Drawing.Size(367, 28);
            this.edtSearchTokuisakiName.TabIndex = 2;
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
            this.numSearchTokuisakiCode.Location = new System.Drawing.Point(339, 7);
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
            this.numSearchTokuisakiCode.TabIndex = 1;
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
            this.gcShortcut1.SetShortcuts(this.edtHyojiKensu, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 579);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(975, 22);
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
            this.panel2.Location = new System.Drawing.Point(0, 537);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(975, 42);
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
            this.btnOk.Location = new System.Drawing.Point(757, 6);
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
            this.btnCancel.Location = new System.Drawing.Point(863, 6);
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
            this.menuStripTop.Size = new System.Drawing.Size(975, 24);
            this.menuStripTop.TabIndex = 206;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.edtSearchTokuisakiName);
            this.panel1.Controls.Add(this.numSearchTokuisakiCode);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.chkAllFlag);
            this.panel1.Controls.Add(this.edtSearchText);
            this.panel1.Controls.Add(this.fpListGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 513);
            this.panel1.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.CausesValidation = false;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(289, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 18);
            this.label11.TabIndex = 1059;
            this.label11.Text = "得意先";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkAllFlag
            // 
            this.chkAllFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllFlag.AutoSize = true;
            this.chkAllFlag.Checked = true;
            this.chkAllFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAllFlag.Location = new System.Drawing.Point(862, 11);
            this.chkAllFlag.Name = "chkAllFlag";
            this.chkAllFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkAllFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkAllFlag.Size = new System.Drawing.Size(99, 22);
            this.chkAllFlag.TabIndex = 3;
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
            this.fpListGrid.Size = new System.Drawing.Size(969, 471);
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
            this.fpListGrid_Sheet1.ColumnCount = 13;
            this.fpListGrid_Sheet1.RowCount = 30;
            this.fpListGrid_Sheet1.Cells.Get(0, 0).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 2).Value = "1234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 3).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(0, 4).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 5).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(0, 6).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 7).Value = "12345678";
            this.fpListGrid_Sheet1.Cells.Get(0, 8).Value = "12345678901234567890";
            this.fpListGrid_Sheet1.Cells.Get(0, 9).Value = "1,234,567";
            this.fpListGrid_Sheet1.Cells.Get(0, 10).Value = "123,456,789";
            this.fpListGrid_Sheet1.Cells.Get(0, 11).Value = "123,456";
            this.fpListGrid_Sheet1.Cells.Get(0, 12).Value = "使用停止";
            this.fpListGrid_Sheet1.Cells.Get(1, 0).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 1).Value = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ";
            this.fpListGrid_Sheet1.Cells.Get(1, 3).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 5).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 6).Value = "ＷＷＷＷＷＷＷＷＷＷ";
            this.fpListGrid_Sheet1.Cells.Get(1, 7).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 8).Value = "ＷＷＷＷＷＷＷＷＷＷ";
            this.fpListGrid_Sheet1.Cells.Get(1, 9).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 10).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(1, 11).Value = 2D;
            this.fpListGrid_Sheet1.Cells.Get(2, 0).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 3).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 5).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 7).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 9).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 10).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(2, 11).Value = 3D;
            this.fpListGrid_Sheet1.Cells.Get(3, 0).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 3).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 5).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 7).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 9).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 10).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(3, 11).Value = 4D;
            this.fpListGrid_Sheet1.Cells.Get(4, 0).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 3).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 5).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 7).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 9).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 10).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(4, 11).Value = 5D;
            this.fpListGrid_Sheet1.Cells.Get(5, 0).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 3).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 5).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 7).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 9).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 10).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(5, 11).Value = 6D;
            this.fpListGrid_Sheet1.Cells.Get(6, 0).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 3).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 5).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 7).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 9).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 10).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(6, 11).Value = 7D;
            this.fpListGrid_Sheet1.Cells.Get(7, 0).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 3).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 5).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 7).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 9).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 10).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(7, 11).Value = 8D;
            this.fpListGrid_Sheet1.Cells.Get(8, 0).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 3).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 5).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 7).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 9).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 10).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(8, 11).Value = 9D;
            this.fpListGrid_Sheet1.Cells.Get(9, 0).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 3).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 5).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 7).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 9).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 10).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(9, 11).Value = 10D;
            this.fpListGrid_Sheet1.Cells.Get(10, 0).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 3).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 5).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 7).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 9).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 10).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(10, 11).Value = 11D;
            this.fpListGrid_Sheet1.Cells.Get(11, 0).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 3).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 5).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 7).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 9).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 10).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(11, 11).Value = 12D;
            this.fpListGrid_Sheet1.Cells.Get(12, 0).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 3).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 5).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 7).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 9).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 10).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(12, 11).Value = 13D;
            this.fpListGrid_Sheet1.Cells.Get(13, 0).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 3).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 5).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 7).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 9).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 10).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(13, 11).Value = 14D;
            this.fpListGrid_Sheet1.Cells.Get(14, 0).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 3).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 5).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 7).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 9).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 10).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(14, 11).Value = 15D;
            this.fpListGrid_Sheet1.Cells.Get(15, 0).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 3).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 5).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 7).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 9).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 10).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(15, 11).Value = 16D;
            this.fpListGrid_Sheet1.Cells.Get(16, 0).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 3).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 5).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 7).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 9).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 10).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(16, 11).Value = 17D;
            this.fpListGrid_Sheet1.Cells.Get(17, 0).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 3).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 5).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 7).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 9).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 10).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(17, 11).Value = 18D;
            this.fpListGrid_Sheet1.Cells.Get(18, 0).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 3).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 5).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 7).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 9).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 10).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(18, 11).Value = 19D;
            this.fpListGrid_Sheet1.Cells.Get(19, 0).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 3).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 5).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 7).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 9).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 10).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(19, 11).Value = 20D;
            this.fpListGrid_Sheet1.Cells.Get(20, 0).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 3).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 5).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 7).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 9).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 10).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(20, 11).Value = 21D;
            this.fpListGrid_Sheet1.Cells.Get(21, 0).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 3).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 5).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 7).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 9).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 10).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(21, 11).Value = 22D;
            this.fpListGrid_Sheet1.Cells.Get(22, 0).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 3).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 5).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 7).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 9).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 10).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(22, 11).Value = 23D;
            this.fpListGrid_Sheet1.Cells.Get(23, 0).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 3).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 5).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 7).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 9).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 10).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(23, 11).Value = 24D;
            this.fpListGrid_Sheet1.Cells.Get(24, 0).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 3).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 5).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 7).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 9).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 10).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(24, 11).Value = 25D;
            this.fpListGrid_Sheet1.Cells.Get(25, 0).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 3).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 5).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 7).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 9).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 10).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(25, 11).Value = 26D;
            this.fpListGrid_Sheet1.Cells.Get(26, 0).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 3).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 5).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 7).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 9).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 10).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(26, 11).Value = 27D;
            this.fpListGrid_Sheet1.Cells.Get(27, 0).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 3).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 5).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 7).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 9).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 10).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(27, 11).Value = 28D;
            this.fpListGrid_Sheet1.Cells.Get(28, 0).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 3).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 5).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 7).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 9).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 10).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(28, 11).Value = 29D;
            this.fpListGrid_Sheet1.Cells.Get(29, 0).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 3).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 5).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 7).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 9).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 10).Value = 30D;
            this.fpListGrid_Sheet1.Cells.Get(29, 11).Value = 30D;
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
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "販路名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "フリガナ";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "得意先名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "積地名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "着地名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "請求単価";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "傭車金額";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "業務料";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 12).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 12).ForeColor = System.Drawing.Color.Black;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "使用停止";
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
            this.fpListGrid_Sheet1.Columns.Get(0).Width = 66F;
            this.fpListGrid_Sheet1.Columns.Get(1).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpListGrid_Sheet1.Columns.Get(1).Label = "販路名";
            this.fpListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(1).Width = 244F;
            this.fpListGrid_Sheet1.Columns.Get(2).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.fpListGrid_Sheet1.Columns.Get(2).Label = "フリガナ";
            this.fpListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Width = 72F;
            this.fpListGrid_Sheet1.Columns.Get(3).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.fpListGrid_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(3).Label = "コード";
            this.fpListGrid_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Width = 66F;
            this.fpListGrid_Sheet1.Columns.Get(4).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.fpListGrid_Sheet1.Columns.Get(4).Label = "得意先名";
            this.fpListGrid_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(4).Width = 120F;
            this.fpListGrid_Sheet1.Columns.Get(5).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.fpListGrid_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(5).Label = "コード";
            this.fpListGrid_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(6).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.fpListGrid_Sheet1.Columns.Get(6).Label = "積地名";
            this.fpListGrid_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(6).Width = 124F;
            this.fpListGrid_Sheet1.Columns.Get(7).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.fpListGrid_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(7).Label = "コード";
            this.fpListGrid_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(8).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.fpListGrid_Sheet1.Columns.Get(8).Label = "着地名";
            this.fpListGrid_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(8).Width = 124F;
            this.fpListGrid_Sheet1.Columns.Get(9).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.fpListGrid_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(9).Label = "請求単価";
            this.fpListGrid_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(9).Width = 80F;
            this.fpListGrid_Sheet1.Columns.Get(10).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.fpListGrid_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(10).Label = "傭車金額";
            this.fpListGrid_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(10).Width = 80F;
            this.fpListGrid_Sheet1.Columns.Get(11).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.fpListGrid_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(11).Label = "業務料";
            this.fpListGrid_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(11).Width = 80F;
            this.fpListGrid_Sheet1.Columns.Get(12).AllowAutoSort = true;
            this.fpListGrid_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.fpListGrid_Sheet1.Columns.Get(12).Label = "使用停止";
            this.fpListGrid_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(12).Width = 80F;
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
            // CmnSearchHanroFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(975, 601);
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
            this.Name = "CmnSearchHanroFrame";
            this.Text = "販路検索";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CmnSearchHanroFrame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtSearchTokuisakiName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchTokuisakiCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edtHyojiKensu)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.Panel panel1;
        private GrapeCity.Win.Editors.GcTextBox edtSearchText;
        private FarPoint.Win.Spread.FpSpread fpListGrid;
        private FarPoint.Win.Spread.SheetView fpListGrid_Sheet1;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkAllFlag;
        private System.Windows.Forms.Label label5;
        private GrapeCity.Win.Editors.GcTextBox edtHyojiKensu;
        private GrapeCity.Win.Editors.GcTextBox edtSearchTokuisakiName;
        private GrapeCity.Win.Editors.GcNumber numSearchTokuisakiCode;
        private GrapeCity.Win.Editors.SideButton sbtnSearchTokuisakiCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSearch;
    }
}