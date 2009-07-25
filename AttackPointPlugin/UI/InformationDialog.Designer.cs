namespace GK.SportTracks.AttackPoint.UI
{
    partial class InformationDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationDialog));
            this.actionBanner1 = new ZoneFiveSoftware.Common.Visuals.ActionBanner();
            this.progressBar1 = new ZoneFiveSoftware.Common.Visuals.ProgressBar();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.tbError = new ZoneFiveSoftware.Common.Visuals.TextBox();
            this.llFeedback = new System.Windows.Forms.LinkLabel();
            this.pSeparator = new System.Windows.Forms.Panel();
            this.bClose = new ZoneFiveSoftware.Common.Visuals.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionBanner1
            // 
            this.actionBanner1.BackColor = System.Drawing.Color.Transparent;
            this.actionBanner1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.actionBanner1.HasMenuButton = false;
            this.actionBanner1.Location = new System.Drawing.Point(2, 1);
            this.actionBanner1.Name = "actionBanner1";
            this.actionBanner1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.actionBanner1.Size = new System.Drawing.Size(333, 24);
            this.actionBanner1.Style = ZoneFiveSoftware.Common.Visuals.ActionBanner.BannerStyle.Header3;
            this.actionBanner1.TabIndex = 0;
            this.actionBanner1.Text = "Please wait";
            this.actionBanner1.UseStyleFont = true;
            // 
            // progressBar1
            // 
            this.progressBar1.AnimateSpeedMs = 200;
            this.progressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.progressBar1.Location = new System.Drawing.Point(13, 37);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Percent = 0F;
            this.progressBar1.Size = new System.Drawing.Size(310, 28);
            this.progressBar1.TabIndex = 1;
            // 
            // tbError
            // 
            this.tbError.AcceptsReturn = false;
            this.tbError.AcceptsTab = false;
            this.tbError.BackColor = System.Drawing.Color.White;
            this.tbError.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(114)))), ((int)(((byte)(108)))));
            this.tbError.ButtonImage = null;
            this.tbError.Location = new System.Drawing.Point(13, 32);
            this.tbError.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbError.MaxLength = 32767;
            this.tbError.Multiline = true;
            this.tbError.Name = "tbError";
            this.tbError.ReadOnly = true;
            this.tbError.ReadOnlyColor = System.Drawing.SystemColors.Control;
            this.tbError.ReadOnlyTextColor = System.Drawing.SystemColors.ControlLight;
            this.tbError.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbError.Size = new System.Drawing.Size(310, 113);
            this.tbError.TabIndex = 3;
            this.tbError.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // llFeedback
            // 
            this.llFeedback.ActiveLinkColor = System.Drawing.Color.Blue;
            this.llFeedback.AutoSize = true;
            this.llFeedback.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llFeedback.Location = new System.Drawing.Point(10, 10);
            this.llFeedback.Name = "llFeedback";
            this.llFeedback.Size = new System.Drawing.Size(103, 17);
            this.llFeedback.TabIndex = 5;
            this.llFeedback.TabStop = true;
            this.llFeedback.Text = "Send feedback";
            this.llFeedback.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llFeedback.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llFeedback_LinkClicked);
            // 
            // pSeparator
            // 
            this.pSeparator.BackColor = System.Drawing.Color.Gold;
            this.pSeparator.Location = new System.Drawing.Point(2, 1);
            this.pSeparator.Name = "pSeparator";
            this.pSeparator.Size = new System.Drawing.Size(333, 1);
            this.pSeparator.TabIndex = 3;
            // 
            // bClose
            // 
            this.bClose.BackColor = System.Drawing.Color.Transparent;
            this.bClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(120)))));
            this.bClose.CenterImage = null;
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bClose.HyperlinkStyle = false;
            this.bClose.ImageMargin = 2;
            this.bClose.LeftImage = null;
            this.bClose.Location = new System.Drawing.Point(256, 8);
            this.bClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bClose.Name = "bClose";
            this.bClose.PushStyle = true;
            this.bClose.RightImage = null;
            this.bClose.Size = new System.Drawing.Size(76, 24);
            this.bClose.TabIndex = 4;
            this.bClose.Text = "Close";
            this.bClose.TextAlign = System.Drawing.StringAlignment.Center;
            this.bClose.TextLeftMargin = 2;
            this.bClose.TextRightMargin = 2;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pSeparator);
            this.panel1.Controls.Add(this.bClose);
            this.panel1.Controls.Add(this.llFeedback);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 38);
            this.panel1.TabIndex = 6;
            // 
            // InformationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 190);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbError);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.actionBanner1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InformationDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Information";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZoneFiveSoftware.Common.Visuals.ActionBanner actionBanner1;
        private ZoneFiveSoftware.Common.Visuals.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private ZoneFiveSoftware.Common.Visuals.TextBox tbError;
        private System.Windows.Forms.LinkLabel llFeedback;
        private System.Windows.Forms.Panel pSeparator;
        private ZoneFiveSoftware.Common.Visuals.Button bClose;
        private System.Windows.Forms.Panel panel1;
    }
}