using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace GK.SportTracks.AttackPoint.UI
{
    public partial class AboutBoxForm : Form
    {
        public AboutBoxForm() {
            InitializeComponent();
            labelCaption.Text = string.Format("AttackPoint Plugin v. {0}", ApPlugin.GetVersion());
            linkWebPage.Text = ApPlugin.HomePage;
            cbDebug.Checked = ApPlugin.Logger.IsDebug;
            buttonOk.Focus();
        }

        private void linkWebPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ApPlugin.ShowWebPage(ApPlugin.HomePage);
            Close();
        }

        private void linkLogFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (File.Exists(ApPlugin.Logger.LogFileName))
            {
                ApPlugin.StartProcess(ApPlugin.Logger.LogFileName, "Unable to open log file.");
            }
            else {
                MessageBox.Show("Log file does not exist.");
            }
            Close();
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e) {
            ApPlugin.Logger.IsDebug = cbDebug.Checked;
        }

    }
}
