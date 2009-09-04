using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace GK.SportTracks.AttackPoint.UI
{

    public delegate void OperationCompletedHandler(RunWorkerCompletedEventArgs e);

    public class BaseDialog : Form
    {
        protected bool _closeAfterComplete;

        // Disable close button
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public BaseDialog() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // bgWorker
            // 
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // BaseDialog
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "BaseDialog";
            this.ResumeLayout(false);

        }

        protected void Close_Click(object sender, EventArgs e) {
            Close();
        }

        protected void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Invoke(new OperationCompletedHandler(OperationCompleted), e);
        }


        protected virtual void OperationCompleted(RunWorkerCompletedEventArgs e) {
        }

        protected void Feedback_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ApPlugin.OpenEmailClient("AttackPoint Plugin Error");
        }

        private System.ComponentModel.BackgroundWorker bgWorker;

        public void Execute(Form owner, string caption, string waitMessage, DoWorkEventHandler action) {
            Execute(owner, caption, waitMessage, action, false);
        }

        public void Execute(Form owner, string caption, string waitMessage, DoWorkEventHandler action, bool closeAfterComplete) {
            bgWorker.DoWork += action;
            //bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            _closeAfterComplete = closeAfterComplete;
            Text = caption;

            UpdateUI(waitMessage);

            bgWorker.RunWorkerAsync(this);
            ShowDialog(owner);
        }

        protected virtual void UpdateUI(string waitMessage) {
        }
    }

    public class IgnoreException : Exception { }

}
