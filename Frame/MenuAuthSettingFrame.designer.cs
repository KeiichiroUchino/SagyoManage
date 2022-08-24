namespace Jpsys.HaishaManageV10.Frame
{
    partial class MenuAuthSettingFrame
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
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer5 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer6 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.mnuStripTop = new System.Windows.Forms.MenuStrip();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fpOperatorListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpOperatorListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.mrsMenuAuthorizeList = new GrapeCity.Win.MultiRow.GcMultiRow();
            this.menuAuthSettingTpl2 = new Jpsys.HaishaManageV10.Frame.MenuAuthSettingTpl();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fpAuthListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpAuthListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripCancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlCenter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpOperatorListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpOperatorListGrid_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mrsMenuAuthorizeList)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpAuthListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpAuthListGrid_Sheet1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUpdate.Location = new System.Drawing.Point(747, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(105, 30);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.Text = "保存";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(634, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 30);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "編集取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // mnuStripTop
            // 
            this.mnuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.mnuStripTop.Location = new System.Drawing.Point(0, 0);
            this.mnuStripTop.Name = "mnuStripTop";
            this.mnuStripTop.Size = new System.Drawing.Size(864, 24);
            this.mnuStripTop.TabIndex = 30;
            this.mnuStripTop.Text = "menuStrip1";
            // 
            // pnlCenter
            // 
            this.pnlCenter.Controls.Add(this.groupBox2);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(170, 24);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(694, 519);
            this.pnlCenter.TabIndex = 31;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.fpOperatorListGrid);
            this.groupBox2.Controls.Add(this.mrsMenuAuthorizeList);
            this.groupBox2.Location = new System.Drawing.Point(4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(687, 513);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "使用を許可するメニューにチェックを付けてください";
            // 
            // fpOperatorListGrid
            // 
            this.fpOperatorListGrid.AccessibleDescription = "fpOperatorListGrid, Sheet1, Row 0, Column 0, 権限1";
            this.fpOperatorListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fpOperatorListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpOperatorListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpOperatorListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpOperatorListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpOperatorListGrid.HorizontalScrollBar.Name = "";
            this.fpOperatorListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
            this.fpOperatorListGrid.HorizontalScrollBar.TabIndex = 40;
            this.fpOperatorListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpOperatorListGrid.Location = new System.Drawing.Point(6, 24);
            this.fpOperatorListGrid.Name = "fpOperatorListGrid";
            this.fpOperatorListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpOperatorListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpOperatorListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpOperatorListGrid_Sheet1});
            this.fpOperatorListGrid.Size = new System.Drawing.Size(172, 485);
            this.fpOperatorListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpOperatorListGrid.SpreadScaleMode = FarPoint.Win.Spread.ScaleMode.ZoomDpiSupport;
            this.fpOperatorListGrid.TabIndex = 0;
            this.fpOperatorListGrid.TabStop = false;
            this.fpOperatorListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpOperatorListGrid.VerticalScrollBar.Name = "";
            this.fpOperatorListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
            this.fpOperatorListGrid.VerticalScrollBar.TabIndex = 41;
            this.fpOperatorListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            // 
            // fpOperatorListGrid_Sheet1
            // 
            this.fpOperatorListGrid_Sheet1.Reset();
            this.fpOperatorListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpOperatorListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpOperatorListGrid_Sheet1.ColumnCount = 1;
            this.fpOperatorListGrid_Sheet1.RowCount = 20;
            this.fpOperatorListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpOperatorListGrid_Sheet1.Cells.Get(0, 0).Value = "権限1";
            this.fpOperatorListGrid_Sheet1.Cells.Get(1, 0).Value = "権限2";
            this.fpOperatorListGrid_Sheet1.Cells.Get(2, 0).Value = "権限3";
            this.fpOperatorListGrid_Sheet1.Cells.Get(3, 0).Value = "権限4";
            this.fpOperatorListGrid_Sheet1.Cells.Get(4, 0).Value = "権限5";
            this.fpOperatorListGrid_Sheet1.Cells.Get(5, 0).Value = "権限6";
            this.fpOperatorListGrid_Sheet1.Cells.Get(6, 0).Value = "権限7";
            this.fpOperatorListGrid_Sheet1.Cells.Get(7, 0).Value = "権限8";
            this.fpOperatorListGrid_Sheet1.Cells.Get(8, 0).Value = "権限9";
            this.fpOperatorListGrid_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpOperatorListGrid_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpOperatorListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpOperatorListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpOperatorListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "対象の利用者";
            this.fpOperatorListGrid_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpOperatorListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpOperatorListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpOperatorListGrid_Sheet1.Columns.Get(0).Label = "対象の利用者";
            this.fpOperatorListGrid_Sheet1.Columns.Get(0).Resizable = false;
            this.fpOperatorListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpOperatorListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpOperatorListGrid_Sheet1.Columns.Get(0).Width = 150F;
            this.fpOperatorListGrid_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpOperatorListGrid_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpOperatorListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpOperatorListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpOperatorListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpOperatorListGrid_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpOperatorListGrid_Sheet1.RowHeader.Visible = false;
            this.fpOperatorListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpOperatorListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.fpOperatorListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpOperatorListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpOperatorListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpOperatorListGrid_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpOperatorListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpOperatorListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // mrsMenuAuthorizeList
            // 
            this.mrsMenuAuthorizeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mrsMenuAuthorizeList.Location = new System.Drawing.Point(185, 24);
            this.mrsMenuAuthorizeList.Name = "mrsMenuAuthorizeList";
            this.mrsMenuAuthorizeList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mrsMenuAuthorizeList.Size = new System.Drawing.Size(496, 483);
            this.mrsMenuAuthorizeList.TabIndex = 1;
            this.mrsMenuAuthorizeList.Template = this.menuAuthSettingTpl2;
            this.mrsMenuAuthorizeList.Text = "gcMultiRow1";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label12);
            this.pnlBottom.Controls.Add(this.btnUpdate);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 543);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(864, 42);
            this.pnlBottom.TabIndex = 179;
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
            this.label12.Size = new System.Drawing.Size(864, 1);
            this.label12.TabIndex = 1032;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Location = new System.Drawing.Point(166, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 519);
            this.splitter1.TabIndex = 182;
            this.splitter1.TabStop = false;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.groupBox3);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Enabled = false;
            this.pnlLeft.Location = new System.Drawing.Point(0, 24);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(166, 519);
            this.pnlLeft.TabIndex = 181;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.fpAuthListGrid);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(160, 513);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "権限を選択してください";
            // 
            // fpAuthListGrid
            // 
            this.fpAuthListGrid.AccessibleDescription = "fpAuthListGrid, Sheet1, Row 0, Column 0, 権限1";
            this.fpAuthListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpAuthListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpAuthListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpAuthListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpAuthListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpAuthListGrid.HorizontalScrollBar.Name = "";
            this.fpAuthListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer5;
            this.fpAuthListGrid.HorizontalScrollBar.TabIndex = 32;
            this.fpAuthListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.fpAuthListGrid.Location = new System.Drawing.Point(6, 24);
            this.fpAuthListGrid.Name = "fpAuthListGrid";
            this.fpAuthListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpAuthListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpAuthListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpAuthListGrid_Sheet1});
            this.fpAuthListGrid.Size = new System.Drawing.Size(148, 485);
            this.fpAuthListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpAuthListGrid.SpreadScaleMode = FarPoint.Win.Spread.ScaleMode.ZoomDpiSupport;
            this.fpAuthListGrid.TabIndex = 0;
            this.fpAuthListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpAuthListGrid.VerticalScrollBar.Name = "";
            this.fpAuthListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer6;
            this.fpAuthListGrid.VerticalScrollBar.TabIndex = 33;
            this.fpAuthListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpAuthListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpAuthListGrid_CellClick);
            this.fpAuthListGrid.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpAuthListGrid_CellDoubleClick);
            this.fpAuthListGrid.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.fpAuthListGrid_PreviewKeyDown);
            // 
            // fpAuthListGrid_Sheet1
            // 
            this.fpAuthListGrid_Sheet1.Reset();
            this.fpAuthListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpAuthListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpAuthListGrid_Sheet1.ColumnCount = 1;
            this.fpAuthListGrid_Sheet1.RowCount = 10;
            this.fpAuthListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpAuthListGrid_Sheet1.Cells.Get(0, 0).Value = "権限1";
            this.fpAuthListGrid_Sheet1.Cells.Get(1, 0).Value = "権限2";
            this.fpAuthListGrid_Sheet1.Cells.Get(2, 0).Value = "権限3";
            this.fpAuthListGrid_Sheet1.Cells.Get(3, 0).Value = "権限4";
            this.fpAuthListGrid_Sheet1.Cells.Get(4, 0).Value = "権限5";
            this.fpAuthListGrid_Sheet1.Cells.Get(5, 0).Value = "権限6";
            this.fpAuthListGrid_Sheet1.Cells.Get(6, 0).Value = "権限7";
            this.fpAuthListGrid_Sheet1.Cells.Get(7, 0).Value = "権限8";
            this.fpAuthListGrid_Sheet1.Cells.Get(8, 0).Value = "権限9";
            this.fpAuthListGrid_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpAuthListGrid_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpAuthListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(168)))), ((int)(((byte)(168)))));
            this.fpAuthListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpAuthListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "権限";
            this.fpAuthListGrid_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpAuthListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpAuthListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpAuthListGrid_Sheet1.Columns.Get(0).Label = "権限";
            this.fpAuthListGrid_Sheet1.Columns.Get(0).Resizable = false;
            this.fpAuthListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpAuthListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpAuthListGrid_Sheet1.Columns.Get(0).Width = 127F;
            this.fpAuthListGrid_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpAuthListGrid_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefault";
            this.fpAuthListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpAuthListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpAuthListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpAuthListGrid_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpAuthListGrid_Sheet1.RowHeader.Visible = false;
            this.fpAuthListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpAuthListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(219)))), ((int)(((byte)(219)))));
            this.fpAuthListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpAuthListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpAuthListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpAuthListGrid_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fpAuthListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpAuthListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.SteelBlue;
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripCancel,
            this.toolStripSave});
            this.statusStrip1.Location = new System.Drawing.Point(0, 585);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(864, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 191;
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
            // MenuAuthSettingFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(864, 607);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.mnuStripTop);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MenuAuthSettingFrame";
            this.Text = "MenuAuthSettingFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuAuthSettingFrame_FormClosing);
            this.pnlCenter.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpOperatorListGrid)).EndInit();
            this.fpOperatorListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.SheetCornerStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.DefaultStyle.Locked = false;
            this.fpOperatorListGrid_Sheet1.Protect = true;
            ((System.ComponentModel.ISupportInitialize)(this.fpOperatorListGrid_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mrsMenuAuthorizeList)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpAuthListGrid)).EndInit();
                        this.fpAuthListGrid_Sheet1.FilterBarHeaderStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.FilterBar.DefaultStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.SheetCornerStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.RowHeader.DefaultStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.DefaultStyle.Locked = false;
                        this.fpAuthListGrid_Sheet1.Protect = true;
            ((System.ComponentModel.ISupportInitialize)(this.fpAuthListGrid_Sheet1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MenuStrip mnuStripTop;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.GroupBox groupBox2;
        private GrapeCity.Win.MultiRow.GcMultiRow mrsMenuAuthorizeList;
        private MenuAuthSettingTpl menuAuthSettingTpl2;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.GroupBox groupBox3;
        private FarPoint.Win.Spread.FpSpread fpAuthListGrid;
        private FarPoint.Win.Spread.SheetView fpAuthListGrid_Sheet1;
        private FarPoint.Win.Spread.FpSpread fpOperatorListGrid;
        private FarPoint.Win.Spread.SheetView fpOperatorListGrid_Sheet1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCancel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSave;
    }
}