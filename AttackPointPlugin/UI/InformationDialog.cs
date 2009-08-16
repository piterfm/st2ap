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
    public delegate void OperationCompletedHandler(RunWorkerCompletedEventArgs e);

    public partial class InformationDialog : Form
    {
        private bool _closeAfterComplete;
        private int _smallSize;
        private int _bigSize;

        public InformationDialog() {
            InitializeComponent();
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

        // Disable close button
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public void Execute(Form owner, string caption, string waitMessage, DoWorkEventHandler action) {
            Execute(owner, caption, waitMessage, action, false);
        }

        public void Execute(Form owner, string caption, string waitMessage, DoWorkEventHandler action, bool closeAfterComplete) {
            bgWorker.DoWork += action;
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            _closeAfterComplete = closeAfterComplete;

            Height = _smallSize;
            actionBanner1.Text = waitMessage;
            Text = caption;
            bClose.Enabled = false;
            tbError.Visible = llFeedback.Visible = false;

            progressBar1.StartAnimation();
            bgWorker.RunWorkerAsync();
            ShowDialog(owner);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Invoke(new OperationCompletedHandler(OperationCompleted), e);
        }

        private void OperationCompleted(RunWorkerCompletedEventArgs e) {
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

                var aex = ex as ApplicationException;
                var exceptionToLog = aex == null ? ex : aex.InnerException;
                if (exceptionToLog != null) {
                    ApPlugin.Logger.LogMessage("Operation failed.", exceptionToLog);
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

        private void bClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void llFeedback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ApPlugin.OpenEmailClient("AttackPoint Plugin Error");
        }

        public class IgnoreException : Exception { }
    }
}
