using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace GK.SportTracks.AttackPoint.UI
{
    public partial class AboutBoxForm : Form
    {
        public AboutBoxForm() {
            InitializeComponent();
            labelCaption.Text = string.Format("AttackPoint Plugin v. {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
            linkWebPage.Text = ApPlugin.WebPage;
            buttonOk.Focus();
        }

        private void linkWebPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ApPlugin.ShowWebPage();
            Close();
        }

    }
}
