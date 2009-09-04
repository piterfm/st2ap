namespace GK.SportTracks.AttackPoint.UI
{
    partial class ExportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportDialog));
            this.actionBanner1 = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.pSeparator = new System.Windows.Forms.Panel();
            this.bClose = new ZoneFiveSoftware.Common.Visuals.Button();
            this.llFeedback = new System.Windows.Forms.LinkLabel();
            this.bgwUploader = new System.ComponentModel.BackgroundWorker();
            this.bIgnore = new ZoneFiveSoftware.Common.Visuals.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // actionBanner1
            // 
            this.actionBanner1.BackColor = System.Drawing.Color.Transparent;
            this.actionBanner1.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionBanner1.HasMenuButton = false;
            this.actionBanner1.Location = new System.Drawing.Point(0, 0);
            this.actionBanner1.Margin = new System.Windows.Forms.Padding(4);
            this.actionBanner1.Name = "actionBanner1";
            this.actionBanner1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.actionBanner1.Size = new System.Drawing.Size(550, 38);
            this.actionBanner1.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header3;
            this.actionBanner1.TabIndex = 0;
            this.actionBanner1.Text = "actionBanner1";
            this.actionBanner1.UseStyleFont = true;
            // 
            // pSeparator
            // 
            this.pSeparator.BackColor = System.Drawing.Color.Gold;
            this.pSeparator.Location = new System.Drawing.Point(0, 245);
            this.pSeparator.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.pSeparator.Name = "pSeparator";
            this.pSeparator.Size = new System.Drawing.Size(556, 1);
            this.pSeparator.TabIndex = 2;
            // 
            // bClose
            // 
            this.bClose.BackColor = System.Drawing.Color.Transparent;
            this.bClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.bClose.CenterImage = null;
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bClose.HyperlinkStyle = false;
            this.bClose.ImageMargin = 2;
            this.bClose.LeftImage = null;
            this.bClose.Location = new System.Drawing.Point(465, 255);
            this.bClose.Margin = new System.Windows.Forms.Padding(5);
            this.bClose.Name = "bClose";
            this.bClose.PushStyle = true;
            this.bClose.RightImage = null;
            this.bClose.Size = new System.Drawing.Size(71, 28);
            this.bClose.TabIndex = 5;
            this.bClose.Text = "Close";
            this.bClose.TextAlign = System.Drawing.StringAlignment.Center;
            this.bClose.TextLeftMargin = 2;
            this.bClose.TextRightMargin = 2;
            // 
            // llFeedback
            // 
            this.llFeedback.ActiveLinkColor = System.Drawing.Color.Blue;
            this.llFeedback.AutoSize = true;
            this.llFeedback.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llFeedback.Location = new System.Drawing.Point(13, 258);
            this.llFeedback.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llFeedback.Name = "llFeedback";
            this.llFeedback.Size = new System.Drawing.Size(103, 17);
            this.llFeedback.TabIndex = 4;
            this.llFeedback.TabStop = true;
            this.llFeedback.Text = "Send feedback";
            this.llFeedback.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // bIgnore
            // 
            this.bIgnore.BackColor = System.Drawing.Color.Transparent;
            this.bIgnore.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.bIgnore.CenterImage = null;
            this.bIgnore.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bIgnore.HyperlinkStyle = false;
            this.bIgnore.ImageMargin = 2;
            this.bIgnore.LeftImage = null;
            this.bIgnore.Location = new System.Drawing.Point(388, 255);
            this.bIgnore.Margin = new System.Windows.Forms.Padding(5);
            this.bIgnore.Name = "bIgnore";
            this.bIgnore.PushStyle = true;
            this.bIgnore.RightImage = null;
            this.bIgnore.Size = new System.Drawing.Size(69, 28);
            this.bIgnore.TabIndex = 6;
            this.bIgnore.Text = "Ignore";
            this.bIgnore.TextAlign = System.Drawing.StringAlignment.Center;
            this.bIgnore.TextLeftMargin = 2;
            this.bIgnore.TextRightMargin = 2;
            this.bIgnore.Click += new System.EventHandler(this.bIgnore_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(10, 45);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(528, 193);
            this.tbLog.TabIndex = 7;
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 291);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.bIgnore);
            this.Controls.Add(this.llFeedback);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.pSeparator);
            this.Controls.Add(this.actionBanner1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.ActionBanner actionBanner1;
        private System.Windows.Forms.Panel pSeparator;
        private ZoneFiveSoftware.Common.Visuals.Button bClose;
        private System.Windows.Forms.LinkLabel llFeedback;
        private System.ComponentModel.BackgroundWorker bgwUploader;
        private ZoneFiveSoftware.Common.Visuals.Button bIgnore;
        private System.Windows.Forms.TextBox tbLog;
    }
}