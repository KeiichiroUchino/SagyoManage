namespace Jpsys.HaishaManageV10.VBReport
{
    partial class ReportPreviewVBFrame
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
            this.viewerControl1 = new AdvanceSoftware.VBReport8.ViewerControl();
            this.SuspendLayout();
            // 
            // viewerControl1
            // 
            this.viewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewerControl1.Enabled = false;
            this.viewerControl1.Location = new System.Drawing.Point(0, 0);
            this.viewerControl1.MinimumSize = new System.Drawing.Size(64, 64);
            this.viewerControl1.Name = "viewerControl1";
            this.viewerControl1.ShowReportFrame = AdvanceSoftware.VBReport8.ReportFrame.All;
            this.viewerControl1.Size = new System.Drawing.Size(884, 661);
            this.viewerControl1.TabIndex = 0;
            // 
            // ReportPreviewVBFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.viewerControl1);
            this.Name = "ReportPreviewVBFrame";
            this.Text = "ReportPreviewVBFrame";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private AdvanceSoftware.VBReport8.ViewerControl viewerControl1;
    }
}