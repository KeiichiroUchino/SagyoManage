namespace Jpsys.SagyoManage.Frame
{
    partial class CsvExportFrame
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
            GrapeCity.Win.Editors.ListItem listItem1 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem1 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.SubItem subItem2 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ListItem listItem2 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem3 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.SubItem subItem4 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ListItem listItem3 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem5 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.SubItem subItem6 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ListItem listItem4 = new GrapeCity.Win.Editors.ListItem();
            GrapeCity.Win.Editors.SubItem subItem7 = new GrapeCity.Win.Editors.SubItem();
            GrapeCity.Win.Editors.ItemTemplate itemTemplate1 = new GrapeCity.Win.Editors.ItemTemplate();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbExportTask = new GrapeCity.Win.Editors.GcComboBox(this.components);
            this.dropDownButton9 = new GrapeCity.Win.Editors.DropDownButton();
            this.btnCancelSelection = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lbxExportTargetItem = new System.Windows.Forms.ListBox();
            this.btnCancelSelectionAll = new System.Windows.Forms.Button();
            this.lbxSelectionTargetItem = new System.Windows.Forms.ListBox();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkDisableFlag = new jp.co.jpsys.util.ui.CheckBoxExNSK(this.components);
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripExportCsv = new System.Windows.Forms.ToolStripStatusLabel();
            this.gcShortcut1 = new GrapeCity.Win.Editors.GcShortcut(this.components);
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExportTask)).BeginInit();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 24);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(528, 493);
            this.panel3.TabIndex = 186;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbExportTask);
            this.groupBox1.Controls.Add(this.btnCancelSelection);
            this.groupBox1.Controls.Add(this.btnSelectAll);
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.lbxExportTargetItem);
            this.groupBox1.Controls.Add(this.btnCancelSelectionAll);
            this.groupBox1.Controls.Add(this.lbxSelectionTargetItem);
            this.groupBox1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(521, 485);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力する項目を選択してください *";
            // 
            // cmbExportTask
            // 
            this.cmbExportTask.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmbExportTask.DropDown.AllowResize = false;
            this.cmbExportTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExportTask.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbExportTask.ImeMode = System.Windows.Forms.ImeMode.Disable;
            subItem1.Value = "米部門";
            subItem2.Value = "伊藤";
            listItem1.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem1,
            subItem2});
            subItem3.Value = "芋部門";
            subItem4.Value = "山迫";
            listItem2.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem3,
            subItem4});
            subItem5.Value = "肥料部門";
            subItem6.Value = "斉藤";
            listItem3.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem5,
            subItem6});
            subItem7.Value = "一般部門";
            listItem4.SubItems.AddRange(new GrapeCity.Win.Editors.SubItem[] {
            subItem7});
            this.cmbExportTask.Items.AddRange(new GrapeCity.Win.Editors.ListItem[] {
            listItem1,
            listItem2,
            listItem3,
            listItem4});
            this.cmbExportTask.ListGridLines.VerticalLines = new GrapeCity.Win.Editors.Line(GrapeCity.Win.Editors.LineStyle.None, System.Drawing.Color.Empty);
            this.cmbExportTask.ListHeaderPane.Height = 25;
            this.cmbExportTask.ListHeaderPane.Visible = false;
            itemTemplate1.Height = 21;
            this.cmbExportTask.ListItemTemplates.Add(itemTemplate1);
            this.cmbExportTask.Location = new System.Drawing.Point(6, 27);
            this.cmbExportTask.MaxDropDownItems = 25;
            this.cmbExportTask.Name = "cmbExportTask";
            this.gcShortcut1.SetShortcuts(this.cmbExportTask, new GrapeCity.Win.Editors.ShortcutCollection(new System.Windows.Forms.Keys[] {
                System.Windows.Forms.Keys.Return,
                System.Windows.Forms.Keys.Up,
                System.Windows.Forms.Keys.Down,
                System.Windows.Forms.Keys.Left,
                System.Windows.Forms.Keys.Right}, new object[] {
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.gcShortcut1)),
                ((object)(this.cmbExportTask)),
                ((object)(this.cmbExportTask))}, new string[] {
                "NextControl",
                "PreviousControl",
                "NextControl",
                "ShortcutSpinUp",
                "ShortcutSpinDown"}));
            this.cmbExportTask.SideButtons.AddRange(new GrapeCity.Win.Editors.SideButtonBase[] {
            this.dropDownButton9});
            this.cmbExportTask.Size = new System.Drawing.Size(195, 24);
            this.cmbExportTask.Spin.SpinOnWheel = false;
            this.cmbExportTask.Spin.Wrap = false;
            this.cmbExportTask.TabIndex = 0;
            this.cmbExportTask.SelectedValueChanged += new System.EventHandler(this.cmbExportTask_SelectedValueChanged);
            // 
            // dropDownButton9
            // 
            this.dropDownButton9.Name = "dropDownButton9";
            // 
            // btnCancelSelection
            // 
            this.btnCancelSelection.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancelSelection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancelSelection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelSelection.Location = new System.Drawing.Point(208, 275);
            this.btnCancelSelection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancelSelection.Name = "btnCancelSelection";
            this.btnCancelSelection.Size = new System.Drawing.Size(108, 24);
            this.btnCancelSelection.TabIndex = 5;
            this.btnCancelSelection.TabStop = false;
            this.btnCancelSelection.Text = "解除";
            this.btnCancelSelection.UseVisualStyleBackColor = true;
            this.btnCancelSelection.Click += new System.EventHandler(this.btnCancelSelection_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSelectAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelectAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectAll.Location = new System.Drawing.Point(208, 164);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(108, 24);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.TabStop = false;
            this.btnSelectAll.Text = "すべて選択";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSelect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelect.Location = new System.Drawing.Point(208, 196);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(108, 24);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.TabStop = false;
            this.btnSelect.Text = "選択";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lbxExportTargetItem
            // 
            this.lbxExportTargetItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxExportTargetItem.FormattingEnabled = true;
            this.lbxExportTargetItem.ItemHeight = 18;
            this.lbxExportTargetItem.Items.AddRange(new object[] {
            "車種コード",
            "車種名"});
            this.lbxExportTargetItem.Location = new System.Drawing.Point(322, 58);
            this.lbxExportTargetItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbxExportTargetItem.Name = "lbxExportTargetItem";
            this.lbxExportTargetItem.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxExportTargetItem.Size = new System.Drawing.Size(196, 364);
            this.lbxExportTargetItem.TabIndex = 2;
            this.lbxExportTargetItem.DoubleClick += new System.EventHandler(this.lbxExportTargetItem_DoubleClick);
            this.lbxExportTargetItem.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.lbxExportTargetItem_PreviewKeyDown);
            // 
            // btnCancelSelectionAll
            // 
            this.btnCancelSelectionAll.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancelSelectionAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancelSelectionAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelSelectionAll.Location = new System.Drawing.Point(208, 307);
            this.btnCancelSelectionAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancelSelectionAll.Name = "btnCancelSelectionAll";
            this.btnCancelSelectionAll.Size = new System.Drawing.Size(108, 24);
            this.btnCancelSelectionAll.TabIndex = 6;
            this.btnCancelSelectionAll.TabStop = false;
            this.btnCancelSelectionAll.Text = "すべて解除";
            this.btnCancelSelectionAll.UseVisualStyleBackColor = true;
            this.btnCancelSelectionAll.Click += new System.EventHandler(this.btnCancelSelectionAll_Click);
            // 
            // lbxSelectionTargetItem
            // 
            this.lbxSelectionTargetItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxSelectionTargetItem.FormattingEnabled = true;
            this.lbxSelectionTargetItem.ItemHeight = 18;
            this.lbxSelectionTargetItem.Items.AddRange(new object[] {
            "略称",
            "車種区分",
            "輸送トン数"});
            this.lbxSelectionTargetItem.Location = new System.Drawing.Point(7, 58);
            this.lbxSelectionTargetItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbxSelectionTargetItem.Name = "lbxSelectionTargetItem";
            this.lbxSelectionTargetItem.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxSelectionTargetItem.Size = new System.Drawing.Size(195, 364);
            this.lbxSelectionTargetItem.TabIndex = 1;
            this.lbxSelectionTargetItem.DoubleClick += new System.EventHandler(this.lbxSelectionTargetItem_DoubleClick);
            this.lbxSelectionTargetItem.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.lbxSelectionTargetItem_PreviewKeyDown);
            // 
            // menuStripTop
            // 
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStripTop.Size = new System.Drawing.Size(528, 24);
            this.menuStripTop.TabIndex = 187;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCsv.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExportCsv.Location = new System.Drawing.Point(416, 4);
            this.btnExportCsv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(100, 28);
            this.btnExportCsv.TabIndex = 22;
            this.btnExportCsv.TabStop = false;
            this.btnExportCsv.Text = "CSV出力";
            this.btnExportCsv.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkDisableFlag);
            this.panel2.Controls.Add(this.btnExportCsv);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 521);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(528, 38);
            this.panel2.TabIndex = 188;
            // 
            // chkDisableFlag
            // 
            this.chkDisableFlag.AutoSize = true;
            this.chkDisableFlag.Checked = true;
            this.chkDisableFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisableFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDisableFlag.Location = new System.Drawing.Point(9, 6);
            this.chkDisableFlag.Name = "chkDisableFlag";
            this.chkDisableFlag.NextControlShortcutKeys = System.Windows.Forms.Keys.Return;
            this.chkDisableFlag.PreviousControlShortcutKeys = System.Windows.Forms.Keys.F12;
            this.chkDisableFlag.Size = new System.Drawing.Size(183, 22);
            this.chkDisableFlag.TabIndex = 1141;
            this.chkDisableFlag.TabStop = false;
            this.chkDisableFlag.Text = "「無効にする」は出力しない";
            this.chkDisableFlag.UseVisualStyleBackColor = true;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 517);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(528, 4);
            this.splitter2.TabIndex = 189;
            this.splitter2.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.SteelBlue;
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripEnd,
            this.toolStripExportCsv});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(528, 22);
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
            // toolStripExportCsv
            // 
            this.toolStripExportCsv.ForeColor = System.Drawing.Color.White;
            this.toolStripExportCsv.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.toolStripExportCsv.Name = "toolStripExportCsv";
            this.toolStripExportCsv.Size = new System.Drawing.Size(119, 20);
            this.toolStripExportCsv.Text = "Shift+F8：CSV出力";
            // 
            // CsvExportFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(528, 581);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStripTop);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "CsvExportFrame";
            this.Text = "CsvExportFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CsvExportFrame_FormClosing);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbExportTask)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private GrapeCity.Win.Editors.GcComboBox cmbExportTask;
        private GrapeCity.Win.Editors.DropDownButton dropDownButton9;
        private System.Windows.Forms.Button btnCancelSelection;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ListBox lbxExportTargetItem;
        private System.Windows.Forms.Button btnCancelSelectionAll;
        private System.Windows.Forms.ListBox lbxSelectionTargetItem;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripEnd;
        private jp.co.jpsys.util.ui.CheckBoxExNSK chkDisableFlag;
        private GrapeCity.Win.Editors.GcShortcut gcShortcut1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripExportCsv;
    }
}