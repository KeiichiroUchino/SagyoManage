namespace Jpsys.HaishaManageV10.Frame
{
    partial class MergeToraDonTableFrame
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
            FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnCancelSelected = new System.Windows.Forms.Button();
            this.fpTargetTableListGrid = new FarPoint.Win.Spread.FpSpread();
            this.fpTargetTableListGrid_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radAll = new System.Windows.Forms.RadioButton();
            this.radKobetsuShitei = new System.Windows.Forms.RadioButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpTargetTableListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpTargetTableListGrid_Sheet1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripTop
            // 
            this.menuStripTop.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(440, 24);
            this.menuStripTop.TabIndex = 202;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(100)))), ((int)(((byte)(75)))));
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd});
            this.statusStrip1.Location = new System.Drawing.Point(0, 583);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(440, 22);
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
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.label12);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnMerge);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 541);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(440, 42);
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
            this.label12.Size = new System.Drawing.Size(440, 1);
            this.label12.TabIndex = 1031;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(12, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "終了";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnMerge
            // 
            this.btnMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMerge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMerge.Location = new System.Drawing.Point(323, 6);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(105, 30);
            this.btnMerge.TabIndex = 0;
            this.btnMerge.TabStop = false;
            this.btnMerge.Text = "同期";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnSelectAll);
            this.pnlMain.Controls.Add(this.btnCancelSelected);
            this.pnlMain.Controls.Add(this.fpTargetTableListGrid);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.groupBox2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(440, 517);
            this.pnlMain.TabIndex = 0;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSelectAll.ForeColor = System.Drawing.Color.Black;
            this.btnSelectAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectAll.Location = new System.Drawing.Point(285, 13);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(64, 28);
            this.btnSelectAll.TabIndex = 1096;
            this.btnSelectAll.TabStop = false;
            this.btnSelectAll.Text = "全選択";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnCancelSelected
            // 
            this.btnCancelSelected.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancelSelected.ForeColor = System.Drawing.Color.Black;
            this.btnCancelSelected.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelSelected.Location = new System.Drawing.Point(357, 13);
            this.btnCancelSelected.Name = "btnCancelSelected";
            this.btnCancelSelected.Size = new System.Drawing.Size(64, 28);
            this.btnCancelSelected.TabIndex = 1097;
            this.btnCancelSelected.TabStop = false;
            this.btnCancelSelected.Text = "全解除";
            this.btnCancelSelected.UseVisualStyleBackColor = true;
            this.btnCancelSelected.Click += new System.EventHandler(this.btnCancelSelected_Click);
            // 
            // fpTargetTableListGrid
            // 
            this.fpTargetTableListGrid.AccessibleDescription = "fpTargetTableListGrid, Sheet1, Row 0, Column 0";
            this.fpTargetTableListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fpTargetTableListGrid.BackColor = System.Drawing.SystemColors.Control;
            this.fpTargetTableListGrid.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpTargetTableListGrid.FocusRenderer = defaultFocusIndicatorRenderer1;
            this.fpTargetTableListGrid.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpTargetTableListGrid.HorizontalScrollBar.Name = "";
            this.fpTargetTableListGrid.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
            this.fpTargetTableListGrid.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpTargetTableListGrid.Location = new System.Drawing.Point(12, 46);
            this.fpTargetTableListGrid.Name = "fpTargetTableListGrid";
            this.fpTargetTableListGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fpTargetTableListGrid.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.fpTargetTableListGrid.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpTargetTableListGrid_Sheet1});
            this.fpTargetTableListGrid.Size = new System.Drawing.Size(418, 465);
            this.fpTargetTableListGrid.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
            this.fpTargetTableListGrid.TabIndex = 1098;
            this.fpTargetTableListGrid.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.fpTargetTableListGrid.VerticalScrollBar.Name = "";
            this.fpTargetTableListGrid.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
            this.fpTargetTableListGrid.VisualStyles = FarPoint.Win.VisualStyles.Off;
            this.fpTargetTableListGrid.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpTargetTableListGrid_CellClick);
            this.fpTargetTableListGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fpTargetTableListGrid_KeyDown);
            // 
            // fpTargetTableListGrid_Sheet1
            // 
            this.fpTargetTableListGrid_Sheet1.Reset();
            this.fpTargetTableListGrid_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpTargetTableListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpTargetTableListGrid_Sheet1.ColumnCount = 3;
            this.fpTargetTableListGrid_Sheet1.RowCount = 18;
            this.fpTargetTableListGrid_Sheet1.RowHeader.ColumnCount = 0;
            this.fpTargetTableListGrid_Sheet1.Cells.Get(0, 0).Value = "日本システム";
            this.fpTargetTableListGrid_Sheet1.Cells.Get(0, 2).Value = "1";
            this.fpTargetTableListGrid_Sheet1.Cells.Get(1, 0).Value = "12345678901234567890123456789012345678901234567890";
            this.fpTargetTableListGrid_Sheet1.Cells.Get(1, 2).Value = "2";
            this.fpTargetTableListGrid_Sheet1.ColumnFooter.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.ColumnFooter.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.ColumnFooter.DefaultStyle.Parent = "HeaderDefault";
            this.fpTargetTableListGrid_Sheet1.ColumnFooterSheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.ColumnFooterSheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.ColumnFooterSheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "対象テーブル名";
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "選択";
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(226)))), ((int)(((byte)(188)))));
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).ForeColor = System.Drawing.SystemColors.ControlText;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "区分";
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.DefaultStyle.Parent = "HeaderDefault";
            this.fpTargetTableListGrid_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).Label = "対象テーブル名";
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).Resizable = false;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).ShowSortIndicator = false;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(0).Width = 349F;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).CellType = checkBoxCellType1;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).Label = "選択";
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).Resizable = false;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).ShowSortIndicator = false;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(1).Width = 48F;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).Label = "区分";
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).Resizable = false;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).ShowSortIndicator = false;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.fpTargetTableListGrid_Sheet1.Columns.Get(2).Width = 48F;
            this.fpTargetTableListGrid_Sheet1.FilterBar.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.FilterBar.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefault";
            this.fpTargetTableListGrid_Sheet1.GrayAreaBackColor = System.Drawing.SystemColors.ControlDark;
            this.fpTargetTableListGrid_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
            this.fpTargetTableListGrid_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpTargetTableListGrid_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpTargetTableListGrid_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefault";
            this.fpTargetTableListGrid_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpTargetTableListGrid_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.fpTargetTableListGrid_Sheet1.RowHeader.Visible = false;
            this.fpTargetTableListGrid_Sheet1.Rows.Default.Height = 24F;
            this.fpTargetTableListGrid_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fpTargetTableListGrid_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.Single;
            this.fpTargetTableListGrid_Sheet1.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.SelectionColors;
            this.fpTargetTableListGrid_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.fpTargetTableListGrid_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.Empty;
            this.fpTargetTableListGrid_Sheet1.SheetCornerStyle.Parent = "RowHeaderDefault";
            this.fpTargetTableListGrid_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.CausesValidation = false;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(15, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 18);
            this.label3.TabIndex = 1091;
            this.label3.Text = "対象 *";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radAll);
            this.groupBox2.Controls.Add(this.radKobetsuShitei);
            this.groupBox2.Location = new System.Drawing.Point(77, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.groupBox2.Size = new System.Drawing.Size(193, 40);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // radAll
            // 
            this.radAll.AutoSize = true;
            this.radAll.Checked = true;
            this.radAll.Location = new System.Drawing.Point(16, 13);
            this.radAll.Name = "radAll";
            this.radAll.Size = new System.Drawing.Size(62, 22);
            this.radAll.TabIndex = 0;
            this.radAll.TabStop = true;
            this.radAll.Text = "すべて";
            this.radAll.UseVisualStyleBackColor = true;
            this.radAll.CheckedChanged += new System.EventHandler(this.radTargetTable_CheckedChanged);
            this.radAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctl_KeyDown);
            // 
            // radKobetsuShitei
            // 
            this.radKobetsuShitei.AutoSize = true;
            this.radKobetsuShitei.Location = new System.Drawing.Point(95, 13);
            this.radKobetsuShitei.Name = "radKobetsuShitei";
            this.radKobetsuShitei.Size = new System.Drawing.Size(74, 22);
            this.radKobetsuShitei.TabIndex = 1;
            this.radKobetsuShitei.Text = "個別指定";
            this.radKobetsuShitei.UseVisualStyleBackColor = true;
            this.radKobetsuShitei.CheckedChanged += new System.EventHandler(this.radTargetTable_CheckedChanged);
            this.radKobetsuShitei.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctl_KeyDown);
            // 
            // MergeToraDonTableFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(440, 605);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripTop);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MergeToraDonTableFrame";
            this.Text = "MergeToraDonTableFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MergeToraDonTableFrame_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MergeToraDonTableFrame_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpTargetTableListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpTargetTableListGrid_Sheet1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMerge;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radAll;
        private System.Windows.Forms.RadioButton radKobetsuShitei;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnCancelSelected;
        private FarPoint.Win.Spread.FpSpread fpTargetTableListGrid;
        private FarPoint.Win.Spread.SheetView fpTargetTableListGrid_Sheet1;
    }
}