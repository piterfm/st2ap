namespace GK.SportTracks.AttackPoint.UI
{
    partial class AboutBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBoxForm));
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelCaption = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkWebPage = new System.Windows.Forms.LinkLabel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.linkLogFile = new System.Windows.Forms.LinkLabel();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::GK.SportTracks.AttackPoint.Properties.Resources.OLogo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(-1, 2);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(52, 54);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.labelCaption.Location = new System.Drawing.Point(61, 7);
            this.labelCaption.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(133, 13);
            this.labelCaption.TabIndex = 1;
            this.labelCaption.Text = "AttackPoint Plugin v. 1.0.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.label2.Location = new System.Drawing.Point(61, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "(c) 2009 by Greg Khanlarov";
            // 
            // linkWebPage
            // 
            this.linkWebPage.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkWebPage.AutoSize = true;
            this.linkWebPage.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkWebPage.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkWebPage.Location = new System.Drawing.Point(64, 40);
            this.linkWebPage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkWebPage.Name = "linkWebPage";
            this.linkWebPage.Size = new System.Drawing.Size(133, 13);
            this.linkWebPage.TabIndex = 3;
            this.linkWebPage.TabStop = true;
            this.linkWebPage.Text = "http://st2ap.codeplex.com";
            this.linkWebPage.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkWebPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebPage_LinkClicked);
            // 
            // buttonOk
            // 
            this.buttonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(138, 69);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(56, 19);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = false;
            // 
            // linkLogFile
            // 
            this.linkLogFile.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkLogFile.AutoSize = true;
            this.linkLogFile.DisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkLogFile.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkLogFile.Location = new System.Drawing.Point(28, 78);
            this.linkLogFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLogFile.Name = "linkLogFile";
            this.linkLogFile.Size = new System.Drawing.Size(59, 13);
            this.linkLogFile.TabIndex = 5;
            this.linkLogFile.TabStop = true;
            this.linkLogFile.Text = "See log file";
            this.linkLogFile.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.linkLogFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLogFile_LinkClicked);
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(167)))), ((int)(((byte)(68)))));
            this.cbDebug.Location = new System.Drawing.Point(12, 61);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(111, 17);
            this.cbDebug.TabIndex = 6;
            this.cbDebug.Text = "Output debug info";
            this.cbDebug.UseVisualStyleBackColor = true;
            this.cbDebug.CheckedChanged += new System.EventHandler(this.cbDebug_CheckedChanged);
            // 
            // AboutBoxForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(208, 99);
            this.Controls.Add(this.cbDebug);
            this.Controls.Add(this.linkLogFile);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.linkWebPage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCaption);
            this.Controls.Add(this.pictureBoxLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBoxForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkWebPage;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.LinkLabel linkLogFile;
        private System.Windows.Forms.CheckBox cbDebug;
    }
}