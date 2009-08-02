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
        private List<StHeartZoneCategory> _stHRCategories;
        private TabPage _heartZonesTab;
        private bool _refreshing;
        private ApActivity _noneActivity = new ApActivity() { Id = "-1", Title = "None" };
        private ApShoes _noneShoes = new ApShoes() { Id = "-1", Title = "None" };
        private ApEntity _noneIntensity = new ApEntity() { Id = "-1", Title = "None" };
        private List<ApEntity> _intensities;
        private StHeartZoneCategory _currentHRCategory;

        public ApConfig ApConfig { get { return ApPlugin.ApConfig; } }
        public bool IsProfileEmpty { get { return ApConfig.Profile.IsEmpty; } }
        public bool IsMappingEmpty { get { return ApConfig.IsMappingEmpty; } }
        public bool AdvancedFeaturesEnabled { get { return ApConfig.Profile.AdvancedFeaturesEnabled; } }

        public SettingsControl() {
            InitializeComponent();
            linkWebPage.Text = ApPlugin.WebPage;
            _heartZonesTab = tabControlMapping.TabPages[3];

            comboHRCategory.DisplayMember = "Title";
            comboHRCategory.ValueMember = "Id";

            //RefreshPage();
        }

        public void RefreshPage() {
            _refreshing = true;
            try {
                tbUsername.Text = ApConfig.Profile.Username;
                tbPassword.Text = ApConfig.Profile.Password;
                UpdateRetrieveButton();

                dgActivities.Rows.Clear();
                dgShoes.Rows.Clear();
                dgIntensities.Rows.Clear();
                dgHRZones.Rows.Clear();

                if (!IsProfileEmpty) {
                    bool guess = IsMappingEmpty;

                    lAdvancedFeatures.Text = AdvancedFeaturesEnabled ? Resources.AdvancedFeaturesEnabled : Resources.AdvancedFeaturesDisabled;

                    comboNotes.Enabled = true;
                    tbInclusionFormat.Enabled = true;
                    tabControlMapping.Enabled = true;

                    cbWarnAboutShoes.Checked = ApConfig.WarnOnNotMappedEquipment;
                    cbWarnAboutIntensity.Checked = ApConfig.WarnOnUnspecifiedIntensity;
                    cbAutoIntensity.Checked = ApConfig.AutoCalculateMixedIntensity;

                    var oldSelectedIndex = comboNotes.SelectedIndex;
                    comboNotes.Items.Clear();
                    comboNotes.Items.Add("Notes");
                    if (ApConfig.Profile.AdvancedFeaturesEnabled) {
                        comboNotes.Items.Add("Private Notes");
                    }
                    comboNotes.SelectedIndex = oldSelectedIndex == -1 || comboNotes.Items.Count == 1 ? 0 : oldSelectedIndex;

                    PopulateInclusionFormatTextBox();

                    // Populate activities grid
                    var activities = new List<ApActivity>(ApConfig.Profile.Activities);
                    activities.Insert(0, _noneActivity);
                    SetComboColumn(dgActivities, activities, "ApActivity");
                    SetComboColumn(dgActivities, ApConfig.Profile.ConstantData.Workouts, "Workout");
                    _stCategories = ApPlugin.UpdateStCategories();
                    foreach (var category in _stCategories) {
                        var activity = Mapper.MapCategory(ApConfig.Profile, category, guess);
                        var subActivity = activity == null ? string.Empty : category.SubType;
                        dgActivities.Rows.Add(category, (activity ?? _noneActivity).Id, subActivity, category.WorkoutId ?? "1");
                    }

                    // Populate intensities grid
                    _intensities = new List<ApEntity>(ApConfig.Profile.Intensities);
                    _intensities.Insert(0, _noneIntensity);

                    SetComboColumn(dgIntensities, _intensities, "ApIntensity");
                    _stIntensities = ApPlugin.UpdateStIntensities();
                    foreach (var intensity in _stIntensities) {
                        var apIntensity = Mapper.MapIntensity(ApConfig.Profile, intensity, guess);
                        dgIntensities.Rows.Add(intensity, (apIntensity ?? _noneIntensity).Id);
                    }

                    // Populate shoes grid
                    var shoes = new List<ApShoes>(ApConfig.Profile.Shoes);
                    shoes.Insert(0, _noneShoes);

                    var comboColumn = SetComboColumn(dgShoes, shoes, "ApShoes");
                    comboColumn.DisplayMember = "FullTitle";
                    _stEquipment = ApPlugin.UpdateStEquipmentItems();
                    foreach (var item in _stEquipment) {
                        var apShoes = Mapper.MapEquipment(ApConfig.Profile, item, guess);
                        dgShoes.Rows.Add(item, (apShoes ?? _noneShoes).Id);
                    }

                    // Populate HR zones grid
                    if (AdvancedFeaturesEnabled) {
                        if (tabControlMapping.TabPages.Count < 4) {
                            tabControlMapping.TabPages.Add(_heartZonesTab);
                        }
                        cbAutoIntensity.Visible = true;

                        _stHRCategories = ApPlugin.UpdateStHeartRateZones();

                        oldSelectedIndex = comboHRCategory.SelectedIndex;
                        comboHRCategory.Items.Clear();
                        foreach (var c in _stHRCategories) {
                            comboHRCategory.Items.Add(c);
                        }
                        comboHRCategory.SelectedIndex =
                            oldSelectedIndex > -1 && oldSelectedIndex < _stHRCategories.Count ? oldSelectedIndex : 0;
                        _currentHRCategory = _stHRCategories[comboHRCategory.SelectedIndex];

                        // Populate HR zones grid
                        PopuluteHRZoneGrid(guess);
                    }
                    else {
                        if (tabControlMapping.TabPages.Count == 4) {
                            tabControlMapping.TabPages.RemoveAt(3);
                        }
                        cbAutoIntensity.Visible = false;
                    }

                }
                else {
                    lAdvancedFeatures.Text = Resources.AdvancedFeaturesUnknown;
                    comboNotes.Enabled = false;
                    tbInclusionFormat.Text = null;
                    tbInclusionFormat.Enabled = false;
                    tabControlMapping.Enabled = false;
                }

                lblFetch.Text = IsProfileEmpty ?
                    "Click 'Retrieve' button to download your AttackPoint profile." :
                    "Tip: Click 'Retrieve' button to download your latest AP profile settings.";
            }
            catch (Exception ex) {
                ApPlugin.HandleUnhandledException(ex);
            }
            finally {
                _refreshing = false;
            }
        }

        private void PopuluteHRZoneGrid(bool guess) {
            dgHRZones.Rows.Clear();
            SetComboColumn(dgHRZones, _intensities, "ApIntensity2");
            var hrCategory = _stHRCategories.Find(c => c.Id == _stHRCategories[comboHRCategory.SelectedIndex].Id);
            foreach (var zone in hrCategory.HeartZones) {
                var apIntensity = Mapper.MapHeartRateZone(ApConfig.Profile, zone, guess);
                dgHRZones.Rows.Add(zone, (apIntensity ?? _noneIntensity).Id);
            }
        }

        private DataGridViewComboBoxColumn SetComboColumn(DataGridView dgView, object dataSource, string columnName) {
            var comboColumn = (DataGridViewComboBoxColumn)dgView.Columns[columnName];
            comboColumn.DisplayMember = "Title";
            comboColumn.ValueMember = "Id";
            comboColumn.DataSource = dataSource;
            return comboColumn;
        }

        public void UpdateConfiguration(ApConfig config) {
            try {
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
                config.AutoCalculateMixedIntensity = cbAutoIntensity.Checked;

                dgActivities.CommitEdit(DataGridViewDataErrorContexts.Commit);
                for (int i = 0; i < dgActivities.Rows.Count; ++i) {
                    var row = dgActivities.Rows[i];
                    _stCategories[i].ApId = (string)row.Cells[1].Value;
                    _stCategories[i].SubType = (string)row.Cells[2].Value;
                    _stCategories[i].WorkoutId = (string)row.Cells[3].Value;
                }

                dgIntensities.CommitEdit(DataGridViewDataErrorContexts.Commit);
                for (int i = 0; i < dgIntensities.Rows.Count; ++i) {
                    var row = dgIntensities.Rows[i];
                    _stIntensities[i].ApId = (string)row.Cells[1].Value;
                }

                dgShoes.CommitEdit(DataGridViewDataErrorContexts.Commit);
                for (int i = 0; i < dgShoes.Rows.Count; ++i) {
                    var row = dgShoes.Rows[i];
                    _stEquipment[i].ApId = (string)row.Cells[1].Value;
                }

                UpdateHRZones();
            }
            catch (Exception ex) {
                ApPlugin.HandleUnhandledException(ex);
            }
        }

        private void UpdateHRZones() {
            if (_currentHRCategory == null) return;

            dgHRZones.CommitEdit(DataGridViewDataErrorContexts.Commit);
            var hrCategory = _stHRCategories.Find(x => x.Id == _currentHRCategory.Id);
            for (int i = 0; i < dgHRZones.Rows.Count; ++i) {
                var row = dgHRZones.Rows[i];
                hrCategory.HeartZones[i].ApId = (string)row.Cells[1].Value;
            }

        }

        private void bFetch_Click(object sender, EventArgs e) {
            UpdateConfiguration(ApConfig);

            if (ApConfig.Profile.UsernameChanged) {
                ApConfig.Clear();
            }

            var dialog = new InformationDialog(_theme);
            dialog.Execute(ParentForm, "AttackPoint Plugin", "Retrieving AttackPoint profile...",
                (s, ee) =>
                {
                    try {
                        ApPlugin.GetProxy().ScrapeApData(ApConfig.Profile);
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

        internal void ThemeChanged(ITheme theme) {
            _theme = theme;
        }

        private void comboHRCategory_SelectedIndexChanged(object sender, EventArgs e) {
            if (_refreshing) return;
            try {
                UpdateHRZones();
                _currentHRCategory = _stHRCategories[comboHRCategory.SelectedIndex];
                PopuluteHRZoneGrid(false);
            }
            catch (Exception ex) {
                ApPlugin.HandleUnhandledException(ex);
            }
        }

        private void comboNotes_SelectedIndexChanged(object sender, EventArgs e) {
            if (_refreshing) return;

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

        private void tabControlMapping_SelectedIndexChanged(object sender, EventArgs e) {
            lOverrideTip.Visible = tabControlMapping.SelectedIndex == 0;
        }

        private void tabControlMapping_DrawItem(object sender, DrawItemEventArgs e) {
            DrawTabControlTabs(tabControlMapping, e, null);
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

        private void dg_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            // This method is required in order to use DataGridView.Commit method
        }

    }
}
