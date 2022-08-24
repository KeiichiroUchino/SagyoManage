namespace Jpsys.HaishaManageV10.ReportFrame
{
    partial class ReportPreviewARFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportPreviewARFrame));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonMoveFirst = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMovePrev = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMoveNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMoveLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonZoom = new System.Windows.Forms.ToolStripSplitButton();
            this.menuItemPageWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPageFull = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage400 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage300 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage200 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage150 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage100 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage75 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage50 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPage25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.cryViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.AR7Viewer = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
            this.pdfExport = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
            this.rtfExport = new GrapeCity.ActiveReports.Export.Word.Section.RtfExport();
            this.xlsExport = new GrapeCity.ActiveReports.Export.Excel.Section.XlsExport();
            this.dlgExportReport = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslCurrentPage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslDocumentPagesCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonPrint,
            this.toolStripSeparator1,
            this.toolStripButtonMoveFirst,
            this.toolStripButtonMovePrev,
            this.toolStripButtonMoveNext,
            this.toolStripButtonMoveLast,
            this.toolStripSeparator2,
            this.toolStripSplitButtonZoom,
            this.toolStripSeparator3,
            this.toolStripButtonExport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(717, 38);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrint.Image")));
            this.toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(65, 35);
            this.toolStripButtonPrint.Text = "   印　刷   ";
            this.toolStripButtonPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonPrint.Click += new System.EventHandler(this.toolStripButtonPrint_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButtonMoveFirst
            // 
            this.toolStripButtonMoveFirst.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveFirst.Image")));
            this.toolStripButtonMoveFirst.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonMoveFirst.Name = "toolStripButtonMoveFirst";
            this.toolStripButtonMoveFirst.Size = new System.Drawing.Size(73, 35);
            this.toolStripButtonMoveFirst.Text = "最初のページ";
            this.toolStripButtonMoveFirst.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMoveFirst.Click += new System.EventHandler(this.toolStripButtonMoveFirst_Click);
            // 
            // toolStripButtonMovePrev
            // 
            this.toolStripButtonMovePrev.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMovePrev.Image")));
            this.toolStripButtonMovePrev.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonMovePrev.Name = "toolStripButtonMovePrev";
            this.toolStripButtonMovePrev.Size = new System.Drawing.Size(61, 35);
            this.toolStripButtonMovePrev.Text = "前のページ";
            this.toolStripButtonMovePrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMovePrev.Click += new System.EventHandler(this.toolStripButtonMovePrev_Click);
            // 
            // toolStripButtonMoveNext
            // 
            this.toolStripButtonMoveNext.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveNext.Image")));
            this.toolStripButtonMoveNext.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonMoveNext.Name = "toolStripButtonMoveNext";
            this.toolStripButtonMoveNext.Size = new System.Drawing.Size(61, 35);
            this.toolStripButtonMoveNext.Text = "次のページ";
            this.toolStripButtonMoveNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMoveNext.Click += new System.EventHandler(this.toolStripButtonMoveNext_Click);
            // 
            // toolStripButtonMoveLast
            // 
            this.toolStripButtonMoveLast.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveLast.Image")));
            this.toolStripButtonMoveLast.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonMoveLast.Name = "toolStripButtonMoveLast";
            this.toolStripButtonMoveLast.Size = new System.Drawing.Size(73, 35);
            this.toolStripButtonMoveLast.Text = "最後のページ";
            this.toolStripButtonMoveLast.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonMoveLast.Click += new System.EventHandler(this.toolStripButtonMoveLast_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripSplitButtonZoom
            // 
            this.toolStripSplitButtonZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPageWidth,
            this.menuItemPageFull,
            this.menuItemPage400,
            this.menuItemPage300,
            this.menuItemPage200,
            this.menuItemPage150,
            this.menuItemPage100,
            this.menuItemPage75,
            this.menuItemPage50,
            this.menuItemPage25});
            this.toolStripSplitButtonZoom.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonZoom.Image")));
            this.toolStripSplitButtonZoom.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripSplitButtonZoom.Name = "toolStripSplitButtonZoom";
            this.toolStripSplitButtonZoom.Size = new System.Drawing.Size(76, 35);
            this.toolStripSplitButtonZoom.Text = "拡大/縮小";
            this.toolStripSplitButtonZoom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // menuItemPageWidth
            // 
            this.menuItemPageWidth.Name = "menuItemPageWidth";
            this.menuItemPageWidth.Size = new System.Drawing.Size(126, 22);
            this.menuItemPageWidth.Text = "ページ幅";
            this.menuItemPageWidth.Click += new System.EventHandler(this.menuItemPageWidth_Click);
            // 
            // menuItemPageFull
            // 
            this.menuItemPageFull.Name = "menuItemPageFull";
            this.menuItemPageFull.Size = new System.Drawing.Size(126, 22);
            this.menuItemPageFull.Text = "ページ全体";
            this.menuItemPageFull.Click += new System.EventHandler(this.menuItemPageFull_Click);
            // 
            // menuItemPage400
            // 
            this.menuItemPage400.Name = "menuItemPage400";
            this.menuItemPage400.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage400.Text = "400%";
            this.menuItemPage400.Click += new System.EventHandler(this.menuItemPage400_Click);
            // 
            // menuItemPage300
            // 
            this.menuItemPage300.Name = "menuItemPage300";
            this.menuItemPage300.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage300.Text = "300%";
            this.menuItemPage300.Click += new System.EventHandler(this.menuItemPage300_Click);
            // 
            // menuItemPage200
            // 
            this.menuItemPage200.Name = "menuItemPage200";
            this.menuItemPage200.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage200.Text = "200%";
            this.menuItemPage200.Click += new System.EventHandler(this.menuItemPage200_Click);
            // 
            // menuItemPage150
            // 
            this.menuItemPage150.Name = "menuItemPage150";
            this.menuItemPage150.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage150.Text = "150%";
            this.menuItemPage150.Click += new System.EventHandler(this.menuItemPage150_Click);
            // 
            // menuItemPage100
            // 
            this.menuItemPage100.Name = "menuItemPage100";
            this.menuItemPage100.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage100.Text = "100%";
            this.menuItemPage100.Click += new System.EventHandler(this.menuItemPage100_Click);
            // 
            // menuItemPage75
            // 
            this.menuItemPage75.Name = "menuItemPage75";
            this.menuItemPage75.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage75.Text = "75%";
            this.menuItemPage75.Click += new System.EventHandler(this.menuItemPage75_Click);
            // 
            // menuItemPage50
            // 
            this.menuItemPage50.Name = "menuItemPage50";
            this.menuItemPage50.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage50.Text = "50%";
            this.menuItemPage50.Click += new System.EventHandler(this.menuItemPage50_Click);
            // 
            // menuItemPage25
            // 
            this.menuItemPage25.Name = "menuItemPage25";
            this.menuItemPage25.Size = new System.Drawing.Size(126, 22);
            this.menuItemPage25.Text = "25%";
            this.menuItemPage25.Click += new System.EventHandler(this.menuItemPage25_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 38);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.Enabled = false;
            this.toolStripButtonExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExport.Image")));
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(65, 35);
            this.toolStripButtonExport.Text = " 外部出力 ";
            this.toolStripButtonExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButtonExport_Click);
            // 
            // cryViewer
            // 
            this.cryViewer.ActiveViewIndex = -1;
            this.cryViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cryViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.cryViewer.DisplayToolbar = false;
            this.cryViewer.Location = new System.Drawing.Point(276, 71);
            this.cryViewer.Name = "cryViewer";
            this.cryViewer.SelectionFormula = "";
            this.cryViewer.ShowCloseButton = false;
            this.cryViewer.ShowExportButton = false;
            this.cryViewer.ShowGotoPageButton = false;
            this.cryViewer.ShowGroupTreeButton = false;
            this.cryViewer.ShowPageNavigateButtons = false;
            this.cryViewer.ShowPrintButton = false;
            this.cryViewer.ShowRefreshButton = false;
            this.cryViewer.ShowTextSearchButton = false;
            this.cryViewer.Size = new System.Drawing.Size(411, 314);
            this.cryViewer.TabIndex = 5;
            this.cryViewer.ViewTimeSelectionFormula = "";
            // 
            // AR7Viewer
            // 
            this.AR7Viewer.BackColor = System.Drawing.SystemColors.Control;
            this.AR7Viewer.CurrentPage = 0;
            this.AR7Viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AR7Viewer.Location = new System.Drawing.Point(0, 38);
            this.AR7Viewer.Name = "AR7Viewer";
            this.AR7Viewer.PreviewPages = 0;
            // 
            // 
            // 
            // 
            // 
            // 
            this.AR7Viewer.Sidebar.ParametersPanel.ContextMenu = null;
            this.AR7Viewer.Sidebar.ParametersPanel.Width = 200;
            // 
            // 
            // 
            this.AR7Viewer.Sidebar.SearchPanel.ContextMenu = null;
            this.AR7Viewer.Sidebar.SearchPanel.Width = 200;
            // 
            // 
            // 
            this.AR7Viewer.Sidebar.ThumbnailsPanel.ContextMenu = null;
            this.AR7Viewer.Sidebar.ThumbnailsPanel.Width = 200;
            this.AR7Viewer.Sidebar.ThumbnailsPanel.Zoom = 0.1D;
            // 
            // 
            // 
            this.AR7Viewer.Sidebar.TocPanel.ContextMenu = null;
            this.AR7Viewer.Sidebar.TocPanel.Expanded = true;
            this.AR7Viewer.Sidebar.TocPanel.Width = 200;
            this.AR7Viewer.Sidebar.Width = 200;
            this.AR7Viewer.Size = new System.Drawing.Size(717, 500);
            this.AR7Viewer.TabIndex = 6;
            this.AR7Viewer.ZoomChanged += new GrapeCity.ActiveReports.Viewer.Win.ZoomChangedEventHandler(this.AR7Viewer_ZoomChanged);
            this.AR7Viewer.LoadCompleted += new GrapeCity.ActiveReports.Document.LoadCompletedEventHandler(this.AR7Viewer_LoadCompleted);
            this.AR7Viewer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.AR7Viewer_Scroll);
            // 
            // pdfExport
            // 
            this.pdfExport.Pagination = true;
            // 
            // rtfExport
            // 
            this.rtfExport.EnableShapes = false;
            this.rtfExport.Pagination = true;
            // 
            // xlsExport
            // 
            this.xlsExport.Pagination = true;
            this.xlsExport.Tweak = 0;
            // 
            // dlgExportReport
            // 
            this.dlgExportReport.Filter = "Adobe Acrobat(*.pdf)|*.pdf|Microsoft Excel(*.xls)|*.xls|リッチ テキスト形式(*.rtf)|*.rtf";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslCurrentPage,
            this.tslDocumentPagesCount,
            this.tslZoom});
            this.statusStrip1.Location = new System.Drawing.Point(0, 516);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(717, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslCurrentPage
            // 
            this.tslCurrentPage.AutoSize = false;
            this.tslCurrentPage.Name = "tslCurrentPage";
            this.tslCurrentPage.Size = new System.Drawing.Size(234, 17);
            this.tslCurrentPage.Spring = true;
            this.tslCurrentPage.Text = "現在のページ番号:";
            this.tslCurrentPage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tslDocumentPagesCount
            // 
            this.tslDocumentPagesCount.Name = "tslDocumentPagesCount";
            this.tslDocumentPagesCount.Size = new System.Drawing.Size(234, 17);
            this.tslDocumentPagesCount.Spring = true;
            this.tslDocumentPagesCount.Text = "合計ページ番号:";
            this.tslDocumentPagesCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tslZoom
            // 
            this.tslZoom.Name = "tslZoom";
            this.tslZoom.Size = new System.Drawing.Size(234, 17);
            this.tslZoom.Spring = true;
            this.tslZoom.Text = "ズーム率:";
            this.tslZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReportPreviewARFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(717, 538);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.AR7Viewer);
            this.Controls.Add(this.cryViewer);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ReportPreviewARFrame";
            this.Text = "ReportPreviewAR7Frame";
            this.Shown += new System.EventHandler(this.ReportPreviewARFrame_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonMoveFirst;
        private System.Windows.Forms.ToolStripButton toolStripButtonMovePrev;
        private System.Windows.Forms.ToolStripButton toolStripButtonMoveNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonMoveLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonZoom;
        private System.Windows.Forms.ToolStripMenuItem menuItemPageWidth;
        private System.Windows.Forms.ToolStripMenuItem menuItemPageFull;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage400;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage300;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage200;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage150;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage100;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage75;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage50;
        private System.Windows.Forms.ToolStripMenuItem menuItemPage25;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer cryViewer;




        private System.Windows.Forms.SaveFileDialog dlgExportReport;
        private GrapeCity.ActiveReports.Viewer.Win.Viewer AR7Viewer;
        private GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport pdfExport;
        private GrapeCity.ActiveReports.Export.Word.Section.RtfExport rtfExport;
        private GrapeCity.ActiveReports.Export.Excel.Section.XlsExport xlsExport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslCurrentPage;
        private System.Windows.Forms.ToolStripStatusLabel tslDocumentPagesCount;
        private System.Windows.Forms.ToolStripStatusLabel tslZoom;
    }
}