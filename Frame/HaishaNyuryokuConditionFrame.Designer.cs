namespace Jpsys.HaishaManageV10.Frame
{
    partial class HaishaNyuryokuConditionFrame
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
            GrapeCity.Win.Editors.ListItem listItem1 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.ListItem listItem2 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem1 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ListItem listItem3 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem2 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ItemTemplate itemTemplate1 = new GrapeCity.Win.Editors.ItemTemplate();
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ja-JP", false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripReference = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSelect = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.numGroupCode = new GrapeCity.Win.Editors.GcNumber(this.components);
            this.sbtnHomenGroupCode = new GrapeCity.Win.Editors.SideButton();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.cmbBranchOffice = new GrapeCity.Win.Editors.GcComboBox(this.components);
            this.dropDownButton16 = new GrapeCity.Win.Editors.DropDownButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnAllRemove = new System.Windows.Forms.Button();
            this.btnAllSelect = new System.Windows.Forms.Button();
            this.lblTokuisakiShitei = new System.Windows.Forms.Label();
            this.edtGroupName = new GrapeCity.Win.Editors.GcTextBox(this.components);
            this.lblBranchOffice = new System.Windows.Forms.Label();
            this.lblTokuisakiGroup = new System.Windows.Forms.Label();
            this.grpTokuisakiShitei = new System.Windows.Forms.GroupBox();
            this.radGroup = new System.Windows.Forms.RadioButton();
            this.radKobetsu = new System.Windows.Forms.RadioButton();
            this.fpListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGroupCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchOffice)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtGroupName)).BeginInit();
            this.grpTokuisakiShitei.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStripTop.Size = new System.Drawing.Size(934, 24);
            this.menuStripTop.TabIndex = 202;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripReference,
            this.toolStripSelect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(934, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 204;
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
            // toolStripReference
            // 
            this.toolStripReference.ForeColor = System.Drawing.Color.White;
            this.toolStripReference.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripReference.Name = "toolStripReference";
            this.toolStripReference.Size = new System.Drawing.Size(58, 20);
            this.toolStripReference.Text = "F5：参照";
            // 
            // toolStripSelect
            // 
            this.toolStripSelect.ForeColor = System.Drawing.Color.White;
            this.toolStripSelect.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripSelect.Name = "toolStripSelect";
            this.toolStripSelect.Size = new System.Drawing.Size(58, 20);
            this.toolStripSelect.Text = "F7：選択";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label12);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnSelect);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 595);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(934, 44);
            this.pnlBottom.TabIndex = 205;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.CausesValidation = false;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(934, 1);
            this.label12.TabIndex = 1031;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelect.Location = new System.Drawing.Point(817, 6);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(105, 30);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "選択";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // numGroupCode
            // 
            this.numGroupCode.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numGroupCode.AllowDeleteToNull = true;
            this.numGroupCode.AlternateText.DisplayZero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numGroupCode.AlternateText.Zero.ForeColor = System.Drawing.SystemColors.WindowText;
            this.numGroupCode.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
            numberIntegerPartDisplayField1.GroupSizes = new int[] {
        0};
            numberIntegerPartDisplayField1.MinDigits = 0;
            this.numGroupCode.DisplayFields.AddRange(new GrapeCity.Win.Editors.Fields.NumberDisplayField[] {
            numberSignDisplayField1,
            numberIntegerPartDisplayField1});
            this.numGroupCode.DropDown.AllowDrop = false;
            this.numGroupCode.Fields.DecimalPart.MaxDigits = 0;
            this.numGroupCode.Fields.IntegerPart.GroupSizes = new int[] {
        0};
            this.numGroupCode.Fields.IntegerPart.MaxDigits = 3;
            this.numGroupCode.Fields.IntegerPart.SpinIncrement = 1;
            this.numGroupCode.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numGroupCode.HighlightText = true;
            this.numGroupCode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.numGroupCode.Location = new System.Drawing.Point(140, 90);
            this.numGroupCode.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numGroupCode.MinValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numGroupCode.Name = "numGroupCode";
            this.numGroupCode.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numGroupCode.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.gcShortcut1.SetShortcuts(this.numGroupCode, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl"}));
            this.numGroupCode.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.sbtnHomenGroupCode});
            this.numGroupCode.Size = new System.Drawing.Size(110, 28);
            this.numGroupCode.Spin.AllowSpin = false;
            this.numGroupCode.TabIndex = 5;
            this.numGroupCode.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            this.numGroupCode.ValueSign = GrapeCity.Win.Editors.ValueSignControl.Positive;
            // 
            // sbtnHomenGroupCode
            // 
            this.sbtnHomenGroupCode.ButtonWidth = 22;
            this.sbtnHomenGroupCode.Name = "sbtnHomenGroupCode";
            this.sbtnHomenGroupCode.Text = "...";
            this.sbtnHomenGroupCode.Click += new System.EventHandler(this.sbtnGroupCode_Click);
            // 
            // cmbBranchOffice
            // 
            this.cmbBranchOffice.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmbBranchOffice.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbBranchOffice.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBranchOffice.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbBranchOffice.DropDown.AllowResize = false;
            this.cmbBranchOffice.DropDownMaxHeight = 233;
            this.cmbBranchOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBranchOffice.FlatStyle = GrapeCity.Win.Editors.FlatStyleEx.Office2007Black;
            this.cmbBranchOffice.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            subItem1.Value = "出力する";
            listItem2.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem1});
            subItem2.Value = "出力しない";
            listItem3.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem2});
            this.cmbBranchOffice.Items.AddRange(new GrapeCity.Win.Editors.ListItem[] {
            listItem1,
            listItem2,
            listItem3});
            this.cmbBranchOffice.ListGridLines.VerticalLines = new GrapeCity.Win.Editors.Line(GrapeCity.Win.Editors.LineStyle.None, System.Drawing.Color.Empty);
            this.cmbBranchOffice.ListHeaderPane.Height = 25;
            this.cmbBranchOffice.ListHeaderPane.Visible = false;
            itemTemplate1.Height = 21;
            this.cmbBranchOffice.ListItemTemplates.Add(itemTemplate1);
            this.cmbBranchOffice.Location = new System.Drawing.Point(140, 6);
            this.cmbBranchOffice.MaxDropDownItems = 10;
            this.cmbBranchOffice.Name = "cmbBranchOffice";
            this.gcShortcut1.SetShortcuts(this.cmbBranchOffice, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Left,
                System.Windows.Forms.Keys.Right}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.cmbBranchOffice)),
                ((object)(this.cmbBranchOffice))}, new string[] {
                "NextControl",
                "ShortcutSpinUp",
                "ShortcutSpinDown"}));
            this.cmbBranchOffice.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton16});
            this.cmbBranchOffice.Size = new System.Drawing.Size(217, 28);
            this.cmbBranchOffice.Spin.SpinOnWheel = false;
            this.cmbBranchOffice.Spin.Wrap = false;
            this.cmbBranchOffice.TabIndex = 1093;
            // 
            // dropDownButton16
            // 
            this.dropDownButton16.Name = "dropDownButton16";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.cmbBranchOffice);
            this.pnlMain.Controls.Add(this.btnAllRemove);
            this.pnlMain.Controls.Add(this.btnAllSelect);
            this.pnlMain.Controls.Add(this.lblTokuisakiShitei);
            this.pnlMain.Controls.Add(this.edtGroupName);
            this.pnlMain.Controls.Add(this.lblBranchOffice);
            this.pnlMain.Controls.Add(this.lblTokuisakiGroup);
            this.pnlMain.Controls.Add(this.numGroupCode);
            this.pnlMain.Controls.Add(this.grpTokuisakiShitei);
            this.pnlMain.Controls.Add(this.fpListGrid);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(934, 571);
            this.pnlMain.TabIndex = 0;
            // 
            // btnAllRemove
            // 
            this.btnAllRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllRemove.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAllRemove.ForeColor = System.Drawing.Color.Black;
            this.btnAllRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAllRemove.Location = new System.Drawing.Point(577, 540);
            this.btnAllRemove.Name = "btnAllRemove";
            this.btnAllRemove.Size = new System.Drawing.Size(64, 28);
            this.btnAllRemove.TabIndex = 1091;
            this.btnAllRemove.TabStop = false;
            this.btnAllRemove.Text = "全解除";
            this.btnAllRemove.UseVisualStyleBackColor = true;
            this.btnAllRemove.Click += new System.EventHandler(this.btnAllRemove_Click);
            // 
            // btnAllSelect
            // 
            this.btnAllSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllSelect.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAllSelect.ForeColor = System.Drawing.Color.Black;
            this.btnAllSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAllSelect.Location = new System.Drawing.Point(505, 540);
            this.btnAllSelect.Name = "btnAllSelect";
            this.btnAllSelect.Size = new System.Drawing.Size(64, 28);
            this.btnAllSelect.TabIndex = 1090;
            this.btnAllSelect.TabStop = false;
            this.btnAllSelect.Text = "全選択";
            this.btnAllSelect.UseVisualStyleBackColor = true;
            this.btnAllSelect.Click += new System.EventHandler(this.btnAllSelect_Click);
            // 
            // lblTokuisakiShitei
            // 
            this.lblTokuisakiShitei.AutoSize = true;
            this.lblTokuisakiShitei.CausesValidation = false;
            this.lblTokuisakiShitei.ForeColor = System.Drawing.Color.Black;
            this.lblTokuisakiShitei.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTokuisakiShitei.Location = new System.Drawing.Point(12, 53);
            this.lblTokuisakiShitei.Name = "lblTokuisakiShitei";
            this.lblTokuisakiShitei.Size = new System.Drawing.Size(79, 18);
            this.lblTokuisakiShitei.TabIndex = 1089;
            this.lblTokuisakiShitei.Text = "得意先指定 *";
            this.lblTokuisakiShitei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // edtGroupName
            // 
            this.edtGroupName.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.edtGroupName.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.edtGroupName.DropDown.AllowDrop = false;
            this.edtGroupName.HighlightText = true;
            this.edtGroupName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.edtGroupName.Location = new System.Drawing.Point(251, 90);
            this.edtGroupName.MaxLengthUnit = GrapeCity.Win.Editors.LengthUnit.Byte;
            this.edtGroupName.Name = "edtGroupName";
            this.edtGroupName.ReadOnly = true;
            this.edtGroupName.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this.edtGroupName.Size = new System.Drawing.Size(580, 28);
            this.edtGroupName.TabIndex = 7;
            this.edtGroupName.TabStop = false;
            this.edtGroupName.Text = "1234567890123456789012345678901234567890";
            // 
            // lblBranchOffice
            // 
            this.lblBranchOffice.AutoSize = true;
            this.lblBranchOffice.CausesValidation = false;
            this.lblBranchOffice.ForeColor = System.Drawing.Color.Black;
            this.lblBranchOffice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblBranchOffice.Location = new System.Drawing.Point(12, 11);
            this.lblBranchOffice.Name = "lblBranchOffice";
            this.lblBranchOffice.Size = new System.Drawing.Size(48, 18);
            this.lblBranchOffice.TabIndex = 1041;
            this.lblBranchOffice.Text = "営業所 ";
            this.lblBranchOffice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTokuisakiGroup
            // 
            this.lblTokuisakiGroup.AutoSize = true;
            this.lblTokuisakiGroup.CausesValidation = false;
            this.lblTokuisakiGroup.ForeColor = System.Drawing.Color.Black;
            this.lblTokuisakiGroup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTokuisakiGroup.Location = new System.Drawing.Point(12, 95);
            this.lblTokuisakiGroup.Name = "lblTokuisakiGroup";
            this.lblTokuisakiGroup.Size = new System.Drawing.Size(92, 18);
            this.lblTokuisakiGroup.TabIndex = 1037;
            this.lblTokuisakiGroup.Text = "得意先グループ";
            this.lblTokuisakiGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpTokuisakiShitei
            // 
            this.grpTokuisakiShitei.Controls.Add(this.radGroup);
            this.grpTokuisakiShitei.Controls.Add(this.radKobetsu);
            this.grpTokuisakiShitei.Location = new System.Drawing.Point(140, 32);
            this.grpTokuisakiShitei.Name = "grpTokuisakiShitei";
            this.grpTokuisakiShitei.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.grpTokuisakiShitei.Size = new System.Drawing.Size(240, 48);
            this.grpTokuisakiShitei.TabIndex = 3;
            this.grpTokuisakiShitei.TabStop = false;
            // 
            // radGroup
            // 
            this.radGroup.AutoSize = true;
            this.radGroup.Checked = true;
            this.radGroup.Location = new System.Drawing.Point(15, 17);
            this.radGroup.Name = "radGroup";
            this.radGroup.Size = new System.Drawing.Size(98, 22);
            this.radGroup.TabIndex = 0;
            this.radGroup.TabStop = true;
            this.radGroup.Text = "グループ指定";
            this.radGroup.UseVisualStyleBackColor = true;
            this.radGroup.CheckedChanged += new System.EventHandler(this.radHomen_CheckedChanged);
            this.radGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctl_KeyDown);
            // 
            // radKobetsu
            // 
            this.radKobetsu.AutoSize = true;
            this.radKobetsu.Location = new System.Drawing.Point(126, 17);
            this.radKobetsu.Name = "radKobetsu";
            this.radKobetsu.Size = new System.Drawing.Size(74, 22);
            this.radKobetsu.TabIndex = 1;
            this.radKobetsu.Text = "個別指定";
            this.radKobetsu.UseVisualStyleBackColor = true;
            this.radKobetsu.CheckedChanged += new System.EventHandler(this.radHomen_CheckedChanged);
            this.radKobetsu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctl_KeyDown);
            // 
            // fpListGrid
            // 
            this.fpListGrid.AccessibleDescription = "fpListGrid, Sheet1, Row 0, Column 0, 1";
            this.fpListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.HorizontalScrollBar.Name = "";
            this.fpListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpListGrid.Location = new System.Drawing.Point(15, 124);
            this.fpListGrid.Name = "fpListGrid";
            this.fpListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpListGrid_Sheet1});
            this.fpListGrid.Size = new System.Drawing.Size(639, 413);
            this.fpListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpListGrid.TabIndex = 6;
            this.fpListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpListGrid.VerticalScrollBar.Name = "";
            this.fpListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpHomenListGrid_CellClick);
            this.fpListGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fpHomenListGrid_KeyDown);
            // 
            // fpListGrid_Sheet1
            // 
            this.fpListGrid_Sheet1.Reset();
            this.fpListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpListGrid_Sheet1.ColumnCount = 4;
            this.fpListGrid_Sheet1.RowCount = 18;
            this.fpListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 0).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 0).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 0).Value = "1";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.fpListGrid_Sheet1.Cells.Get(0, 1).ParseFormatInfo)).NumberDecimalDigits = 0;
            this.fpListGrid_Sheet1.Cells.Get(0, 1).ParseFormatString = "E";
            this.fpListGrid_Sheet1.Cells.Get(0, 1).Value = "秋川";
            this.fpListGrid_Sheet1.Cells.Get(1, 0).Value = "123";
            this.fpListGrid_Sheet1.Cells.Get(1, 1).Value = "12345678901234567890123456789012345678901234567890";
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "コード";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "得意先名";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "選択";
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpListGrid_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "Id";
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.fpListGrid_Sheet1.Columns.Get(0).Label = "コード";
            this.fpListGrid_Sheet1.Columns.Get(0).Resizable = false;
            this.fpListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(0).Width = 70F;
            this.fpListGrid_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.fpListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpListGrid_Sheet1.Columns.Get(1).Label = "得意先名";
            this.fpListGrid_Sheet1.Columns.Get(1).Resizable = false;
            this.fpListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(1).Width = 500F;
            this.fpListGrid_Sheet1.Columns.Get(2).CellType = checkBoxCellType1;
            this.fpListGrid_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Label = "選択";
            this.fpListGrid_Sheet1.Columns.Get(2).Resizable = false;
            this.fpListGrid_Sheet1.Columns.Get(2).ShowSortIndicator = false;
            this.fpListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(2).Width = 48F;
            this.fpListGrid_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.fpListGrid_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Label = "Id";
            this.fpListGrid_Sheet1.Columns.Get(3).Resizable = false;
            this.fpListGrid_Sheet1.Columns.Get(3).ShowSortIndicator = false;
            this.fpListGrid_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpListGrid_Sheet1.Columns.Get(3).Width = 48F;
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
            // HaishaNyuryokuConditionFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(934, 661);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripTop);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "HaishaNyuryokuConditionFrame";
            this.Text = "HaishaNyuryokuConditionFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HaishaNyuryokuConditionFrame_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HaishaNyuryokuConditionFrame_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numGroupCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBranchOffice)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtGroupName)).EndInit();
            this.grpTokuisakiShitei.ResumeLayout(false);
            this.grpTokuisakiShitei.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpListGrid_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripReference;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSelect;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelect;
        private GrapeCity.Win.Editors.GcNumber numGroupCode;
        private GrapeCity.Win.Editors.SideButton sbtnHomenGroupCode;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTokuisakiGroup;
        private System.Windows.Forms.Label lblBranchOffice;
        private GrapeCity.Win.Editors.GcTextBox edtGroupName;
        private System.Windows.Forms.Label lblTokuisakiShitei;
        private System.Windows.Forms.GroupBox grpTokuisakiShitei;
        private System.Windows.Forms.RadioButton radGroup;
        private System.Windows.Forms.RadioButton radKobetsu;
        private FarPoint.Win.Spread.FpSpread fpListGrid;
        private FarPoint.Win.Spread.SheetView fpListGrid_Sheet1;
        private System.Windows.Forms.Button btnAllRemove;
        private System.Windows.Forms.Button btnAllSelect;
        private GrapeCity.Win.Editors.GcComboBox cmbBranchOffice;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton16;
    }
}