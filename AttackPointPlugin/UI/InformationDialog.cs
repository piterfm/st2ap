using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;
using System.Diagnostics;
using System.Web;

namespace GK.SportTracks.AttackPoint.UI
{

    public partial class InformationDialog : BaseDialog
    {
        private int _smallSize;
        private int _bigSize;

        public InformationDialog() {
            InitializeComponent();
            bClose.Click += new EventHandler(Close_Click);
            llFeedback.LinkClicked += new LinkLabelLinkClickedEventHandler(Feedback_LinkClicked);
            _bigSize = Height;
            _smallSize = _bigSize - (tbError.Height - progressBar1.Height);
        }

        public InformationDialog(ITheme theme) : this() {
            progressBar1.ThemeChanged(theme);
            actionBanner1.ThemeChanged(theme);
            bClose.BackColor = theme.Control;
            bClose.ForeColor = theme.ControlText;
            pSeparator.BackColor = theme.Border;
        }

        protected override void UpdateUI(string waitMessage)
        {
            Height = _smallSize;
            actionBanner1.Text = waitMessage;
            bClose.Enabled = false;
            tbError.Visible = llFeedback.Visible = false;
            progressBar1.StartAnimation();
        }

        protected override void OperationCompleted(RunWorkerCompletedEventArgs e) {
            if (!Visible) return;

            progressBar1.StopAnimation();
            progressBar1.Visible = false;
            bClose.Enabled = true;
            var ex = e.Result as Exception;
            if (ex != null) {
                if (ex is IgnoreException) {
                    Close();
                    return;
                }

                actionBanner1.Text = "Operation failed.";
                tbError.Visible = llFeedback.Visible = true;

                if (!(ex is ApplicationException)) {
                    ApPlugin.Logger.LogMessage("Operation failed.", ex);
                }

                tbError.Text = string.Format("{0}", ex.Message);
                Height = _bigSize;
            }
            else if (_closeAfterComplete) {
                Close();
            }
            else {
                actionBanner1.Text = e.Result.ToString();
            }
        }

    }
}
