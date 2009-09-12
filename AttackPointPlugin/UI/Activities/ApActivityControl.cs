using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ZoneFiveSoftware.Common.Visuals;
using GK.AttackPoint;
using ZoneFiveSoftware.Common.Data.Fitness;

namespace GK.SportTracks.AttackPoint.UI.Activities
{
    public partial class ApActivityControl : UserControl
    {
        private ApActivityData _data;
        private ITheme _theme;
        private ZoneFiveSoftware.Common.Visuals.TextBox[] _iTextBoxes = new ZoneFiveSoftware.Common.Visuals.TextBox[6];

        public ApActivityControl() {
            InitializeComponent();
            _iTextBoxes[0] = tbI0;
            _iTextBoxes[1] = tbI1;
            _iTextBoxes[2] = tbI2;
            _iTextBoxes[3] = tbI3;
            _iTextBoxes[4] = tbI4;
            _iTextBoxes[5] = tbI5;

            comboWorkout.DisplayMember = "Title";
            comboWorkout.ValueMember = "Id";

            comboTechnicalIntensity.DisplayMember = "Title";
            comboTechnicalIntensity.ValueMember = "Id";
        }

        public ApActivityData Data { set { _data = value; } }
        public IActivity Activity { get; set; }
        public ActivityInfo ActivityInfo { get { return Activity != null ? ActivityInfoCache.Instance.GetInfo(Activity) : null; } }

        public void RefreshPage() {
            try {
                var profile = ApPlugin.ApConfig.Profile;

                if (ActivityInfo != null && !ApPlugin.ApConfig.IsMappingEmpty) {
                    comboWorkout.DataSource = profile.Workouts;
                    comboTechnicalIntensity.DataSource = profile.TechnicalIntensities;

                    // Intensities
                    if (!_data.IsMixedIntensitySpecified() && !_data.IntensitiesCleared) {
                        if (ApPlugin.ApConfig.AutoCalculateMixedIntensity) {
                            CalculateMixedIntensity(false);
                        }
                    }

                    RefreshIntensities();

                    // Workout
                    var workoutId = _data.WorkoutId;
                    if (workoutId == null) {
                        var stCategory = ApPlugin.ApConfig.Mapping.Activities.Find(a => a.StId == Activity.Category.ReferenceId);
                        if (stCategory != null) {
                            workoutId = stCategory.WorkoutId;
                        }
                    }

                    if (!string.IsNullOrEmpty(workoutId)) {
                        comboWorkout.SelectedValue = workoutId;
                    }
                    else {
                        comboWorkout.SelectedIndex = -1;
                    }
                    tbSubtype.Text = _data.ActivitySubtype;

                    // Technical intensity
                    if (profile.ContainsTechnicalIntensityId(_data.TechnicalIntensityId)) {
                        comboTechnicalIntensity.SelectedValue = _data.TechnicalIntensityId;
                    }
                    else {
                        comboTechnicalIntensity.SelectedIndex = 0;
                    }

                    // Orienteering stuff
                    tbSpiked.Text = _data.SpikedControls;
                    tbTotal.Text = _data.TotalControls;
                    comboBoxCourseName.Text = _data.CourseName;
                    tbDistance.Text = _data.CourseLength;
                    tbClimb.Text = _data.CourseClimb;

                    tbPrivateNote.Text = _data.PrivateNote;

                    Enabled = true;
                    pIntensity.Enabled =
                    tbPrivateNote.Enabled =
                    bClear.Enabled =
                    bCalculateIntensity.Enabled = profile.AdvancedFeaturesEnabled;
                }
                else {
                    lblTotalTime.Text = "N/A";
                    comboWorkout.SelectedIndex = -1;
                    comboTechnicalIntensity.SelectedIndex = -1;
                    tbSubtype.Text = null;
                    tbI0.Text = null;
                    tbI1.Text = null;
                    tbI2.Text = null;
                    tbI3.Text = null;
                    tbI4.Text = null;
                    tbI5.Text = null;
                    tbSpiked.Text = null;
                    tbTotal.Text = null;
                    tbDistance.Text = null;
                    tbClimb.Text = null;
                    tbPrivateNote.Text = null;
                    Enabled = false;
                }
            }
            catch (Exception ex) {
                ApPlugin.HandleUnhandledException(ex);
            }
        }

        private void RefreshIntensities() {
            for (int i = 0; i <= 5; ++i) {
                _iTextBoxes[i].Text = _data.Intensities[i];
            }
            lblTotalTime.Text = FormatTime(_data.GetMixedIntensityTime());
        }

        public void UpdateData() {
            if (_data == null || ApPlugin.ApConfig.IsMappingEmpty) return;

            try {
                UpdateIntensity();
                UpdateAllExceptIntensity();
            }
            catch (Exception ex) {
                ApPlugin.HandleUnhandledException(ex);
            }
        }

        private void UpdateAllExceptIntensity() {
            _data.WorkoutId = GetValue(comboWorkout.SelectedValue);
            _data.ActivitySubtype = GetValue(tbSubtype.Text);
            _data.SpikedControls = GetValue(tbSpiked.Text);
            _data.TotalControls = GetValue(tbTotal.Text);
            _data.TechnicalIntensityId = GetValue(comboTechnicalIntensity.SelectedValue);
            _data.CourseName = GetValue(comboBoxCourseName.Text);
            _data.CourseLength = GetValue(tbDistance.Text);
            _data.CourseClimb = GetValue(tbClimb.Text);
            _data.PrivateNote = GetValue(tbPrivateNote.Text);
        }

        private void UpdateIntensity() {
            var stIntensity = ApPlugin.ApConfig.Mapping.Intensities.Find(i => i.StId == Activity.Intensity.ToString());
            var intensity = stIntensity != null ? int.Parse(stIntensity.ApId) : -1;

            // Check if only one intensity time is specified
            // If it corresponds with Activity Time and Intensity then, clear IntensityX property
            if (ActivityInfo != null &&
                _data.IsMixedIntensitySpecified() &&
                _data.IsSingleIntensitySpecified(ActivityInfo.Time, intensity)) {
                _data.ResetIntensity();
            }
            else {
                for (int i = 0; i <= 5; ++i) {
                    _data.Intensities[i] = GetValue(_iTextBoxes[i].Text);
                }
            }
        }

        private string GetValue(object o) {
            if (o == null) return null;
            var s = o.ToString().Trim();
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public void ThemeChanged(ITheme visualTheme) {
            _theme = visualTheme;
            lblClimb.ForeColor =
                lblWorkout.ForeColor =
                lblWorkoutType.ForeColor =
                lblCourseName.ForeColor =
                lblKm.ForeColor =
                lblMeters.ForeColor =
                lblOrienteering.ForeColor =
                lblSpikedControls.ForeColor =
                lblTechnicalIntensity.ForeColor =
                lblTotalControls.ForeColor =
                lblClimb.ForeColor =
                lblActivitySubtype.ForeColor =
                lblTipSubtype.ForeColor =
                lTimePerIntensity.ForeColor =
                lblTotalCaption.ForeColor =
                lblTotalTime.ForeColor =
                lI0.ForeColor =
                lI1.ForeColor =
                lI2.ForeColor =
                lI3.ForeColor =
                lI4.ForeColor =
                lI5.ForeColor =
                visualTheme.ControlText;

            tbI0.ThemeChanged(visualTheme);
            tbI1.ThemeChanged(visualTheme);
            tbI2.ThemeChanged(visualTheme);
            tbI3.ThemeChanged(visualTheme);
            tbI4.ThemeChanged(visualTheme);
            tbI5.ThemeChanged(visualTheme);
            tbPrivateNote.ThemeChanged(visualTheme);

            tbClimb.ThemeChanged(visualTheme);
            tbDistance.ThemeChanged(visualTheme);
            tbSpiked.ThemeChanged(visualTheme);
            tbTotal.ThemeChanged(visualTheme);
            tbSubtype.ThemeChanged(visualTheme);

            ChangeTheme(comboWorkout, visualTheme);
            ChangeTheme(comboTechnicalIntensity, visualTheme);
            ChangeTheme(comboBoxCourseName, visualTheme);
        }

        private void ChangeTheme(ComboBox combo, ITheme visualTheme) {
            combo.BackColor = visualTheme.Window;
            combo.ForeColor = visualTheme.ControlText;
        }

        private string FormatTime(TimeSpan t) {
            var r = string.Format("{0:D2}{1:D2}{2:D2}", t.Hours, t.Minutes, t.Seconds);
            while (r.StartsWith("0")) {
                r = r.Substring(1);
            }

            return r;
        }

        private void ControlEdited(object sender, EventArgs e) {
            UpdateAllExceptIntensity();
        }

        private void ValidateTimePerIntensity(object sender, EventArgs e) {
            var textBox = (ZoneFiveSoftware.Common.Visuals.TextBox)sender;
            var text = textBox.Text.Trim();
            var ts = ApActivityData.GetIntensityTime(text);

            int time;
            if (!string.IsNullOrEmpty(text) && (!int.TryParse(text, out time) || (time != 0 && ts == TimeSpan.Zero))) {
                LightUpTextBox(textBox);
                DisplayError("Invalid intensity time specified.\nUse 'hhmmss' format. The time must be less than 24 hours.");
                textBox.Focus();
            }
            else {
                textBox.ThemeChanged(_theme);
                textBox.Text = FormatTime(ts);
                var index = int.Parse(textBox.Name[textBox.Name.Length - 1].ToString());
                _data.Intensities[index] = textBox.Text;
                lblTotalTime.Text = FormatTime(_data.GetMixedIntensityTime());
            }
        }

        private static void LightUpTextBox(ZoneFiveSoftware.Common.Visuals.TextBox textBox) {
            textBox.BackColor = Color.Pink;
            textBox.ForeColor = Color.Black;
        }

        private void bCalculateIntensity_Click(object sender, EventArgs e) {
            try {
                CalculateMixedIntensity(true);
                RefreshIntensities();
            }
            catch (Exception ex) {
                ApPlugin.HandleUnhandledException(ex);
            }
        }

        private bool CalculateMixedIntensity(bool recalculate) {
            if (ApPlugin.ApConfig.Mapping.HeartZoneCatogories == null ||
                ApPlugin.ApConfig.Mapping.HeartZoneCatogories.Count == 0) {
                DisplayHeartZoneMappingError(recalculate);
                return false;
            }

            var zoneCategory = Activity.Category.HeartRateZone;
            var stZoneCategory = ApPlugin.ApConfig.Mapping.HeartZoneCatogories.Find(x => x.Id == zoneCategory.ReferenceId);
            if (stZoneCategory == null || stZoneCategory.HeartZones == null || stZoneCategory.HeartZones.Count == 0) {
                DisplayHeartZoneMappingError(recalculate);
                return false;
            }

            var intensities = new string[6];
            var categoryInfo = ActivityInfo.HeartRateZoneInfo(zoneCategory);
            foreach (var info in categoryInfo.Zones) {
                if (info.Name == "Total") continue;
                var stHRZone = stZoneCategory.HeartZones.Find(x => x.StId == info.Zone.Name);
                int index;
                if (stHRZone == null ||
                    string.IsNullOrEmpty(stHRZone.ApId) ||
                    ((index = int.Parse(stHRZone.ApId)) < 0)) {
                    DisplayHeartZoneMappingError(recalculate);
                    return false;
                }

                intensities[index] = FormatTime(
                    info.TotalTime +
                    ApActivityData.GetIntensityTime(intensities[index]));
            }

            _data.Intensities = intensities;
            _data.IntensitiesCleared = false;
            return true;
        }

        private void DisplayHeartZoneMappingError(bool recalculate) {
            if (!recalculate) return;
            DisplayError("Unable to recalculate mixed intensity.\nMapping for active heart rate zone category is not specified.\nCheck plugin settings.", "Mapping Error");
        }

        private void bClear_Click(object sender, EventArgs e) {
            Array.ForEach(_iTextBoxes, t => t.Text = null);
            _data.ResetIntensity();
            _data.IntensitiesCleared = true;
        }

        private void ValidatingNumberDouble(object sender, CancelEventArgs e) {
            ValidateNumber(sender, false, e);
        }

        private void ValidatingNumberInteger(object sender, CancelEventArgs e) {
            ValidateNumber(sender, true, e);
        }

        private void ValidateNumber(object sender, bool interger, CancelEventArgs e) {
            var textBox = (ZoneFiveSoftware.Common.Visuals.TextBox)sender;
            var text = textBox.Text.Trim();
            if (text != string.Empty) {
                try {
                    // With lambda expressions it would've been much more elegant
                    if (interger)
                        int.Parse(text);
                    else
                        double.Parse(text);
                }
                catch (FormatException) {
                    e.Cancel = true;
                    DisplayError("Invalid number specified.");
                    LightUpTextBox(textBox);
                    textBox.Focus();
                    return;
                }
            }
            // Reset texbox colors
            textBox.ThemeChanged(_theme);
        }

        private void DisplayError(string message) {
            DisplayError(message, "Input Error");
        }

        private void DisplayError(string message, string caption) {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
