using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;
using System.Diagnostics;
using GK.SportTracks.AttackPoint.Export;
using GK.AttackPoint;
using ZoneFiveSoftware.Common.Data.Fitness;
using System.Threading;

namespace GK.SportTracks.AttackPoint.UI
{
    public partial class ExportDialog : BaseDialog
    {
        private PreExportResults _results;

        public ExportDialog() {
            InitializeComponent();
            bClose.Click += new EventHandler(Close_Click);
            llFeedback.LinkClicked += new LinkLabelLinkClickedEventHandler(Feedback_LinkClicked);
        }

        public ExportDialog(ITheme theme)
            : this() {
            actionBanner1.ThemeChanged(theme);
            bClose.BackColor = theme.Control;
            bClose.ForeColor = theme.ControlText;
            bIgnore.BackColor = theme.Control;
            bIgnore.ForeColor = theme.ControlText;
            pSeparator.BackColor = theme.Border;
        }

        protected override void UpdateUI(string waitMessage) {
            actionBanner1.Text = waitMessage;
            bClose.Enabled = false;
            llFeedback.Visible = false;
            bIgnore.Visible = false;
        }

        protected override void OperationCompleted(RunWorkerCompletedEventArgs e) {
            if (!Visible) return;

            bClose.Enabled = true;

            _results = e.Result as PreExportResults;
            var ex = _results.Error as Exception;
            if (ex != null) {
                if (ProcessException(ex)) return;
                llFeedback.Visible = true;
            }
            else {
                if (_results.IsError || _results.IsWarning) {
                    actionBanner1.Text = "Check the log below for errors/warnings.";
                    bIgnore.Visible = !_results.IsError;
                }
                else {
                    Upload();
                }
            }
        }

        private bool ProcessException(Exception ex) {
            if (ex is IgnoreException) {
                Close();
                return true;
            }

            actionBanner1.Text = "Operation failed.";
            if (!(ex is ApplicationException)) {
                ApPlugin.Logger.LogMessage("Operation failed.", ex);
            }

            tbLog.AppendText(string.Format("{1}{0}", ex.Message, Environment.NewLine));
            return false;
        }

        private void Upload() {
            bClose.Enabled = false;
            bIgnore.Visible = false;
            llFeedback.Visible = false;
            bgwUploader.DoWork += new DoWorkEventHandler(UploadAction);
            bgwUploader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwUploader_RunWorkerCompleted);
            actionBanner1.Text = "Exporting to AttackPoint...";
            bgwUploader.RunWorkerAsync(this);
        }

        private void UploadAction(object sender, DoWorkEventArgs e) {
            try {
                UpdateProgress(Environment.NewLine + Environment.NewLine + "Initiating export to AttackPoint...", "Exporting to AttackPoint");
                var proxy = ApPlugin.GetProxy();
                int i = 1;
                _results.Notes.Sort((n1, n2) =>
                {
                    if (n1.Date < n2.Date) return -1;
                    else if (n1.Date > n2.Date) return 1;
                    return 0;
                });

                UpdateProgress(string.Format("{1}Exporting {0} activities...", _results.Notes.Count, Environment.NewLine), null);
                foreach (var note in _results.Notes) {
                    proxy.Upload(note);
                    UpdateProgress(null, "Exporting activity: " + i);
                    ++i;
                    Thread.Sleep(50); // I don't want to stress the server.
                }
                UpdateProgress(string.Format("{0}DONE.", Environment.NewLine), "Export completed.");
                e.Result = "Export completed.";
            }
            catch (Exception ex) {
                e.Result = ex;
            }
        }

        void bgwUploader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Invoke(new OperationCompletedHandler(ExportCompleted), e);
        }
        
        private void bIgnore_Click(object sender, EventArgs e) {
            Upload();
        }

        private void ExportCompleted(RunWorkerCompletedEventArgs e) {
            if (!Visible) return;

            bClose.Enabled = true;
            
            var ex = e.Result as Exception;
            if (ex != null) {
                if (ProcessException(ex)) return;
                llFeedback.Visible = true;
            }
            else {
                actionBanner1.Text = e.Result.ToString();
            }
        }

        internal void UpdateProgress(string message, string actionBannerMessage) {
            Invoke(new UpdateLogDelegate(UpdateLog), message, actionBannerMessage);
        }

        private delegate void UpdateLogDelegate(string message, string actionBannerMessage);
        private void UpdateLog(string message, string actionBannerMessage) {
            if (actionBannerMessage != null) actionBanner1.Text = actionBannerMessage;
            if (message != null) tbLog.AppendText(message);
        }

        public class PreExportResults
        {
            public bool IsError { get; set; }
            public bool IsWarning { get; set; }
            public Exception Error { get; set; }
            public List<ApNote> Notes { get; set; }
            public IList<IActivity> Activities { get; set; }
        }

    }
}
