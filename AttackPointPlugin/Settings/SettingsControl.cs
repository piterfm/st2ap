using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using GK.AttackPoint;
using GK.SportTracks.AttackPoint;
using ZoneFiveSoftware.Common.Data.Fitness;
using GK.SportTracks.AttackPoint.Properties;
using GK.SportTracks.AttackPoint.UI;
using ZoneFiveSoftware.Common.Visuals;
using System.Diagnostics;

namespace GK.SportTracks.AttackPoint.Settings
{
    public partial class SettingsControl : UserControl
    {
        private ITheme _theme;
        private List<StCategory> _stCategories;
        private List<StIntensity> _stIntensities;
        private List<StEquipment> _stEquipment;

        public ApConfig ApConfig { get { return ApPlugin.ApConfig; } }

        public SettingsControl() {
            InitializeComponent();
            linkWebPage.Text = ApPlugin.WebPage;
            //RefreshPage();

        }

        public void RefreshPage() {
            tbUsername.Text = ApConfig.Profile.Username;
            tbPassword.Text = ApConfig.Profile.Password;
            if (ApConfig.Profile.AdvancedFeaturesEnabledSpecified) {
                lAdvancedFeatures.Text = ApConfig.Profile.AdvancedFeaturesEnabled ? Resources.AdvancedFeaturesEnabled : Resources.AdvancedFeaturesDisabled;

                comboNotes.Enabled = true;
                tbInclusionFormat.Enabled = true;
                cbWarnAboutShoes.Enabled = true;
                cbWarnAboutIntensity.Enabled = true;

                if (comboNotes.Items.Count == 0) {
                    comboNotes.Items.Clear();
                    comboNotes.Items.Add("Notes");
                    if (ApConfig.Profile.AdvancedFeaturesEnabled) {
                        comboNotes.Items.Add("Private Notes");
                    }
                    comboNotes.SelectedIndex = 0;
                    comboNotes.SelectedIndexChanged += new System.EventHandler(this.comboNotes_SelectedIndexChanged);
                }

                PopulateInclusionFormatTextBox();
                cbWarnAboutShoes.Checked = ApConfig.WarnOnNotMappedEquipment;
                cbWarnAboutIntensity.Checked = ApConfig.WarnOnUnspecifiedIntensity;
            }
            else {
                lAdvancedFeatures.Text = Resources.AdvancedFeaturesUnknown;
                comboNotes.Enabled = false;
                tbInclusionFormat.Enabled = false;
                cbWarnAboutShoes.Enabled = false;
                cbWarnAboutIntensity.Enabled = false;
            }

            UpdateRetrieveButton();

            bool guess = ApConfig.IsMappingEmpty;
            var nodata = false;

            dgActivities.Rows.Clear();
            // Populate activities grid
            if (ApConfig.Profile.Activities != null && ApConfig.Profile.Activities.Count != 0) {
                var activities = new List<ApActivity>(ApConfig.Profile.Activities);
                var noneActivity = new ApActivity() { Id = "-1", Title = "None" };
                activities.Insert(0, noneActivity);
                SetComboColumn(dgActivities, activities, "ApActivity");
                SetComboColumn(dgActivities, ApConfig.Profile.ConstantData.Workouts, "Workout");
                _stCategories = ApPlugin.GetStCategories();
                foreach (var category in _stCategories) {
                    var activity = Mapper.MapCategory(ApConfig.Profile, category, guess);
                    var subActivity = activity == null ? string.Empty : category.SubType;
                    dgActivities.Rows.Add(category, (activity ?? noneActivity).Id, subActivity, category.WorkoutId ?? "1");
                }
            }
            else {
                nodata = true;
            }

            dgIntensities.Rows.Clear();
            // Populate intensities grid
            var intensities = new List<ApEntity>(ApConfig.Profile.Intensities);
            var nonIntensity = new ApEntity() { Id = "-1", Title = "Mixed" };
            intensities.Insert(0, nonIntensity);
            SetComboColumn(dgIntensities, intensities, "ApIntensity");
            _stIntensities = ApPlugin.GetStIntensities();
            foreach (var intensity in _stIntensities) {
                var apIntensity = Mapper.MapIntensity(ApConfig.Profile, intensity, guess);
                dgIntensities.Rows.Add(intensity, (apIntensity ?? nonIntensity).Id);
            }

            dgShoes.Rows.Clear();
            // Populate shoes grid
            if (ApConfig.Profile.Shoes != null && ApConfig.Profile.Shoes.Count != 0) {
                var shoes = new List<ApShoes>(ApConfig.Profile.Shoes);
                var noneShoes = new ApShoes() { Id = "-1", Title = "None" };
                shoes.Insert(0, noneShoes);

                var comboColumn = SetComboColumn(dgShoes, shoes, "ApShoes");
                comboColumn.DisplayMember = "FullTitle";
                _stEquipment = ApPlugin.GetStEquipmentItems();
                foreach (var item in _stEquipment) {
                    var apShoes = Mapper.MapEquipment(ApConfig.Profile, item, guess);
                    dgShoes.Rows.Add(item, (apShoes ?? noneShoes).Id);
                }
            }
            else {
                nodata = true;
            }

            if (nodata) {
                lblFetch.Text = "Click 'Retrieve' button to download your AttackPoint profile.";
            }
            else {
                lblFetch.Text = "Tip: Click 'Retrieve' button to download your latest AP categories and shoes.";
            }
        }

        private void UpdateUnitsCombo(ComboBox combo, Units units) {
            combo.SelectedIndex = units == Units.None ? -1 : (units == Units.Metric ? 0 : 1);
        }

        private DataGridViewComboBoxColumn SetComboColumn(DataGridView dgView, object dataSource, string columnName) {
            var comboColumn = (DataGridViewComboBoxColumn)dgView.Columns[columnName];
            comboColumn.DisplayMember = "Title";
            comboColumn.ValueMember = "Id";
            comboColumn.DataSource = dataSource;
            return comboColumn;
        }

        public void UpdateConfiguration(ApConfig config) {
            config.Profile.Username = tbUsername.Text.Trim();
            config.Profile.Password = tbPassword.Text.Trim();

            if (comboNotes.SelectedIndex == 0) {
                ApConfig.NotesFormat = tbInclusionFormat.Text.Trim();
            }
            else {
                ApConfig.PrivateNotesFormat = tbInclusionFormat.Text.Trim();
            }

            //config.NotesFormat = tbInclusionFormat.Text;
            config.WarnOnNotMappedEquipment = cbWarnAboutShoes.Checked;
            config.WarnOnUnspecifiedIntensity = cbWarnAboutIntensity.Checked;

            for (int i = 0; i < dgActivities.Rows.Count; ++i) {
                var row = dgActivities.Rows[i];
                _stCategories[i].ApId = (string)row.Cells[1].Value;
                _stCategories[i].SubType = (string)row.Cells[2].Value;
                _stCategories[i].WorkoutId = (string)row.Cells[3].Value;
            }
            config.Mapping.Activities = _stCategories;

            for (int i = 0; i < dgIntensities.Rows.Count; ++i) {
                var row = dgIntensities.Rows[i];
                _stIntensities[i].ApId = (string)row.Cells[1].Value;
            }
            config.Mapping.Intensities = _stIntensities;

            for (int i = 0; i < dgShoes.Rows.Count; ++i) {
                var row = dgShoes.Rows[i];
                _stEquipment[i].ApId = (string)row.Cells[1].Value;
            }
            config.Mapping.Shoes = _stEquipment;
        }

        private void bFetch_Click(object sender, EventArgs e) {
            UpdateConfiguration(ApConfig);
            ApConfig.Profile.Username = tbUsername.Text.Trim();
            ApConfig.Profile.Password = tbPassword.Text.Trim();

            var dialog = new InformationDialog(_theme);
            dialog.Execute(ParentForm, "AttackPoint Plugin", "Retrieving AttackPoint profile...",
                (s, ee) =>
                {
                    try {
                        ApPlugin.GetProxy().ScrapeApData(ApConfig.Profile);
                        //System.Threading.Thread.Sleep(7000);
                        //throw new Exception("EEEEEROOORR!!!");
                        ee.Result = "Completed";
                    }
                    catch (Exception ex) {
                        ee.Result = ex;
                    }
                });

            dialog.Dispose();

            RefreshPage();
        }

        private void Credentials_TextChanged(object sender, EventArgs e) {
            UpdateRetrieveButton();
        }

        private void UpdateRetrieveButton() {
            bFetch.Enabled =
                !string.IsNullOrEmpty(tbUsername.Text.Trim()) &&
                !string.IsNullOrEmpty(tbPassword.Text.Trim());
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) {
            /*
            var backBrush = new SolidBrush(Color.FromArgb(255, 167, 68));
            var foreBrush = new SolidBrush(Color.Black);
            var tabName = this.tabControl1.TabPages[e.Index].Text;
            var sf = new StringFormat();

            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            var r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, e.Font, foreBrush, r, sf);

            sf.Dispose();
            backBrush.Dispose();
            foreBrush.Dispose();
             */

            DrawTabControlTabs(tabControl1, e, null);
        }


        private void DrawTabControlTabs(TabControl tabControl, DrawItemEventArgs e, ImageList images) {
            // Get the bounding end of tab strip rectangles.
            Rectangle tabstripEndRect = tabControl.GetTabRect(tabControl.TabPages.Count - 1);
            RectangleF tabstripEndRectF = new RectangleF(tabstripEndRect.X + tabstripEndRect.Width, tabstripEndRect.Y - 5,
         tabControl.Width - (tabstripEndRect.X + tabstripEndRect.Width), tabstripEndRect.Height + 5);

            // First, do the end of the tab strip.
            // If we have an image use it.
            if (tabControl.Parent.BackgroundImage != null) {
                RectangleF src = new RectangleF(tabstripEndRectF.X + tabControl.Left, tabstripEndRectF.Y + tabControl.Top, tabstripEndRectF.Width, tabstripEndRectF.Height);
                e.Graphics.DrawImage(tabControl.Parent.BackgroundImage, tabstripEndRectF, src, GraphicsUnit.Pixel);
            }
            // If we have no image, use the background color.
            else {
                using (Brush backBrush = new SolidBrush(tabControl.Parent.BackColor)) {
                    e.Graphics.FillRectangle(backBrush, tabstripEndRectF);
                }
            }

            // Set up the page and the various pieces.
            TabPage page = tabControl.TabPages[e.Index];
            Brush BackBrush = new SolidBrush(Color.FromArgb(255, 167, 68)); //new SolidBrush(page.BackColor);
            Brush ForeBrush = new SolidBrush(Color.Black); //new SolidBrush(page.ForeColor);
            string TabName = page.Text;

            // Set up the offset for an icon, the bounding rectangle and image size and then fill the background.
            int iconOffset = 0;
            Rectangle tabBackgroundRect = e.Bounds;
            e.Graphics.FillRectangle(BackBrush, tabBackgroundRect);

            // If we have images, process them.
            if (images != null) {
                // Get sice and image.
                Size size = images.ImageSize;
                Image icon = null;
                if (page.ImageIndex > -1)
                    icon = images.Images[page.ImageIndex];
                else if (page.ImageKey != "")
                    icon = images.Images[page.ImageKey];

                // If there is an image, use it.
                if (icon != null) {
                    Point startPoint = new Point(tabBackgroundRect.X + 2 + ((tabBackgroundRect.Height - size.Height) / 2),
                 tabBackgroundRect.Y + 2 + ((tabBackgroundRect.Height - size.Height) / 2));
                    e.Graphics.DrawImage(icon, new Rectangle(startPoint, size));
                    iconOffset = size.Width + 4;
                }
            }

            // Draw out the label.
            Rectangle labelRect = new Rectangle(tabBackgroundRect.X + iconOffset, tabBackgroundRect.Y + 3,
         tabBackgroundRect.Width - iconOffset, tabBackgroundRect.Height/* - 3*/);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(TabName, e.Font, ForeBrush, labelRect, sf);

            //Dispose objects
            sf.Dispose();
            BackBrush.Dispose();
            ForeBrush.Dispose();
        }

        internal void ThemeChanged(ITheme theme) {
            _theme = theme;
        }

        private void comboNotes_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboNotes.SelectedIndex == 1) {
                ApConfig.NotesFormat = tbInclusionFormat.Text.Trim();
            }
            else {
                ApConfig.PrivateNotesFormat = tbInclusionFormat.Text.Trim();
            }

            PopulateInclusionFormatTextBox();
        }

        private void PopulateInclusionFormatTextBox() {
            tbInclusionFormat.Text = comboNotes.SelectedIndex == 0 ? ApConfig.NotesFormat : ApConfig.PrivateNotesFormat;
        }

        private void linkWebPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            ApPlugin.ShowWebPage();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            AboutBoxForm f = new AboutBoxForm();
            f.ShowDialog(this);
            f.Dispose();
        }

    }
}
