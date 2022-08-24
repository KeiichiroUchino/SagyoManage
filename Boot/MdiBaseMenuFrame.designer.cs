namespace Jpsys.SagyoManage.Boot
{
    partial class MdiBaseMenuFrame
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelPrintSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemShowMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemCascading = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemShowMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.NSKMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BranchDeployToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExplodingCurrentFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.内容CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.インデックスIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.検索SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.バージョン情報AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripBottom = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelLoginUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBootDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButtonAlartInfo = new System.Windows.Forms.ToolStripSplitButton();
            this.ShowInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bgwInformationCheck = new System.ComponentModel.BackgroundWorker();
            this.tmrInformationCheckDriven = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStripBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ToolToolStripMenuItem,
            this.WindowToolStripMenuItem,
            this.ToolStripMenuItemShowMenuButton,
            this.NSKMenuToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.WindowToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(638, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "-";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了XToolStripMenuItem});
            this.FileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(66, 19);
            this.FileToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 終了XToolStripMenuItem
            // 
            this.終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
            this.終了XToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.終了XToolStripMenuItem.Text = "終了(&X)";
            this.終了XToolStripMenuItem.Click += new System.EventHandler(this.終了XToolStripMenuItem_Click);
            // 
            // ToolToolStripMenuItem
            // 
            this.ToolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DelPrintSettingToolStripMenuItem});
            this.ToolToolStripMenuItem.Name = "ToolToolStripMenuItem";
            this.ToolToolStripMenuItem.Size = new System.Drawing.Size(60, 19);
            this.ToolToolStripMenuItem.Text = "ツール(&T)";
            // 
            // DelPrintSettingToolStripMenuItem
            // 
            this.DelPrintSettingToolStripMenuItem.Name = "DelPrintSettingToolStripMenuItem";
            this.DelPrintSettingToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.DelPrintSettingToolStripMenuItem.Text = "全ての印刷設定の削除(&D)";
            this.DelPrintSettingToolStripMenuItem.Click += new System.EventHandler(this.DelPrintSettingToolStripMenuItem_Click);
            // 
            // WindowToolStripMenuItem
            // 
            this.WindowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemShowMainMenu,
            this.toolStripSeparator1,
            this.ToolStripMenuItemCascading,
            this.toolStripSeparator3});
            this.WindowToolStripMenuItem.Name = "WindowToolStripMenuItem";
            this.WindowToolStripMenuItem.Size = new System.Drawing.Size(82, 19);
            this.WindowToolStripMenuItem.Text = "ウインドウ(&W)";
            // 
            // ToolStripMenuItemShowMainMenu
            // 
            this.ToolStripMenuItemShowMainMenu.Name = "ToolStripMenuItemShowMainMenu";
            this.ToolStripMenuItemShowMainMenu.Size = new System.Drawing.Size(175, 22);
            this.ToolStripMenuItemShowMainMenu.Text = "メインメニュー表示(&M)";
            this.ToolStripMenuItemShowMainMenu.Click += new System.EventHandler(this.ToolStripMenuItemShowMainMenu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // ToolStripMenuItemCascading
            // 
            this.ToolStripMenuItemCascading.Name = "ToolStripMenuItemCascading";
            this.ToolStripMenuItemCascading.Size = new System.Drawing.Size(175, 22);
            this.ToolStripMenuItemCascading.Text = "重ねて表示(&C)";
            this.ToolStripMenuItemCascading.Click += new System.EventHandler(this.ToolStripMenuItemCascading_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(172, 6);
            // 
            // ToolStripMenuItemShowMenuButton
            // 
            this.ToolStripMenuItemShowMenuButton.Name = "ToolStripMenuItemShowMenuButton";
            this.ToolStripMenuItemShowMenuButton.Size = new System.Drawing.Size(111, 19);
            this.ToolStripMenuItemShowMenuButton.Text = "メインメニューの表示";
            this.ToolStripMenuItemShowMenuButton.Click += new System.EventHandler(this.ToolStripMenuItemShowMenuButton_Click);
            // 
            // NSKMenuToolStripMenuItem
            // 
            this.NSKMenuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BranchDeployToolStripMenuItem,
            this.ExplodingCurrentFolderToolStripMenuItem});
            this.NSKMenuToolStripMenuItem.Name = "NSKMenuToolStripMenuItem";
            this.NSKMenuToolStripMenuItem.Size = new System.Drawing.Size(114, 19);
            this.NSKMenuToolStripMenuItem.Text = "NSK管理メニュー(&A)";
            // 
            // BranchDeployToolStripMenuItem
            // 
            this.BranchDeployToolStripMenuItem.Name = "BranchDeployToolStripMenuItem";
            this.BranchDeployToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.BranchDeployToolStripMenuItem.Text = "拠点用配布フォルダへ配置";
            this.BranchDeployToolStripMenuItem.Click += new System.EventHandler(this.BranchDeployToolStripMenuItem_Click);
            // 
            // ExplodingCurrentFolderToolStripMenuItem
            // 
            this.ExplodingCurrentFolderToolStripMenuItem.Name = "ExplodingCurrentFolderToolStripMenuItem";
            this.ExplodingCurrentFolderToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.ExplodingCurrentFolderToolStripMenuItem.Text = "現在実行中のフォルダを表示";
            this.ExplodingCurrentFolderToolStripMenuItem.Click += new System.EventHandler(this.ExplodingCurrentFolderToolStripMenuItem_Click);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.内容CToolStripMenuItem,
            this.インデックスIToolStripMenuItem,
            this.検索SToolStripMenuItem,
            this.toolStripSeparator5,
            this.バージョン情報AToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(65, 19);
            this.HelpToolStripMenuItem.Text = "ヘルプ(&H)";
            this.HelpToolStripMenuItem.Visible = false;
            // 
            // 内容CToolStripMenuItem
            // 
            this.内容CToolStripMenuItem.Name = "内容CToolStripMenuItem";
            this.内容CToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.内容CToolStripMenuItem.Text = "内容(&C)";
            // 
            // インデックスIToolStripMenuItem
            // 
            this.インデックスIToolStripMenuItem.Name = "インデックスIToolStripMenuItem";
            this.インデックスIToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.インデックスIToolStripMenuItem.Text = "インデックス(&I)";
            // 
            // 検索SToolStripMenuItem
            // 
            this.検索SToolStripMenuItem.Name = "検索SToolStripMenuItem";
            this.検索SToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.検索SToolStripMenuItem.Text = "検索(&S)";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(164, 6);
            // 
            // バージョン情報AToolStripMenuItem
            // 
            this.バージョン情報AToolStripMenuItem.Name = "バージョン情報AToolStripMenuItem";
            this.バージョン情報AToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.バージョン情報AToolStripMenuItem.Text = "バージョン情報(&A)...";
            // 
            // statusStripBottom
            // 
            this.statusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelLoginUserName,
            this.toolStripStatusLabelBootDate,
            this.toolStripSplitButtonAlartInfo,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelVersion});
            this.statusStripBottom.Location = new System.Drawing.Point(0, 648);
            this.statusStripBottom.Name = "statusStripBottom";
            this.statusStripBottom.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStripBottom.Size = new System.Drawing.Size(638, 24);
            this.statusStripBottom.TabIndex = 5;
            this.statusStripBottom.Text = "statusStrip1";
            // 
            // toolStripStatusLabelLoginUserName
            // 
            this.toolStripStatusLabelLoginUserName.Name = "toolStripStatusLabelLoginUserName";
            this.toolStripStatusLabelLoginUserName.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabelLoginUserName.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabelBootDate
            // 
            this.toolStripStatusLabelBootDate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabelBootDate.Name = "toolStripStatusLabelBootDate";
            this.toolStripStatusLabelBootDate.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabelBootDate.Text = "toolStripStatusLabel1";
            // 
            // toolStripSplitButtonAlartInfo
            // 
            this.toolStripSplitButtonAlartInfo.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSplitButtonAlartInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripSplitButtonAlartInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowInfoToolStripMenuItem});
            this.toolStripSplitButtonAlartInfo.ForeColor = System.Drawing.Color.Red;
            this.toolStripSplitButtonAlartInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAlartInfo.Name = "toolStripSplitButtonAlartInfo";
            this.toolStripSplitButtonAlartInfo.Size = new System.Drawing.Size(135, 22);
            this.toolStripSplitButtonAlartInfo.Text = "お知らせがあります！！";
            this.toolStripSplitButtonAlartInfo.Visible = false;
            this.toolStripSplitButtonAlartInfo.ButtonClick += new System.EventHandler(this.toolStripSplitButtonAlartInfo_ButtonClick);
            // 
            // ShowInfoToolStripMenuItem
            // 
            this.ShowInfoToolStripMenuItem.Name = "ShowInfoToolStripMenuItem";
            this.ShowInfoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.ShowInfoToolStripMenuItem.Text = "お知らせを表示する";
            this.ShowInfoToolStripMenuItem.Click += new System.EventHandler(this.ShowInfoToolStripMenuItem_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(263, 19);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabelVersion
            // 
            this.toolStripStatusLabelVersion.Name = "toolStripStatusLabelVersion";
            this.toolStripStatusLabelVersion.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabelVersion.Text = "toolStripStatusLabel2";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bgwInformationCheck
            // 
            this.bgwInformationCheck.WorkerSupportsCancellation = true;
            this.bgwInformationCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwInformationCheck_DoWork);
            this.bgwInformationCheck.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwInformationCheck_RunWorkerCompleted);
            // 
            // tmrInformationCheckDriven
            // 
            this.tmrInformationCheckDriven.Tick += new System.EventHandler(this.tmrInformationCheckDriven_Tick);
            // 
            // MdiBaseMenuFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(638, 672);
            this.Controls.Add(this.statusStripBottom);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MdiBaseMenuFrame";
            this.Text = "MdiBaseMenuFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MdiBaseMenuFrame_FormClosing);
            this.Shown += new System.EventHandler(this.MdiBaseMenuFrame_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStripBottom.ResumeLayout(false);
            this.statusStripBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DelPrintSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemShowMainMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCascading;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemShowMenuButton;
        private System.Windows.Forms.ToolStripMenuItem NSKMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BranchDeployToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 内容CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem インデックスIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 検索SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem バージョン情報AToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLoginUserName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBootDate;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonAlartInfo;
        private System.Windows.Forms.ToolStripMenuItem ShowInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelVersion;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker bgwInformationCheck;
        private System.Windows.Forms.Timer tmrInformationCheckDriven;
        private System.Windows.Forms.ToolStripMenuItem ExplodingCurrentFolderToolStripMenuItem;
    }
}