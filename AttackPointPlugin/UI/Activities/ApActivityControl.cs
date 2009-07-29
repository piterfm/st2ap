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
        private Units _defaultUnits;
        private ZoneFiveSoftware.Common.Visuals.TextBox[] _iTextBoxes = new ZoneFiveSoftware.Common.Visuals.TextBox[6];

        public ApActivityControl() {
            InitializeComponent();
            _iTextBoxes[0] = tbI0;
            _iTextBoxes[1] = tbI1;
            _iTextBoxes[2] = tbI2;
            _iTextBoxes[3] = tbI3;
            _iTextBoxes[4] = tbI4;
            _iTextBoxes[5] = tbI5;
        }

        public ApActivityData Data { set { _data = value; } }
        public IActivity Activity { get; set; }
        public ActivityInfo ActivityInfo { get { return Activity != null ? ActivityInfoCache.Instance.GetInfo(Activity) : null; } }

        public void UpdateData() {
            if (_data == null) return;

            _data.WorkoutId = GetValue(comboWorkout.SelectedValue);

            var stIntensity = ApPlugin.ApConfig.Mapping.Intensities.Find(i => i.StId == Activity.Intensity.ToString());
            var intensity = stIntensity != null ? int.Parse(stIntensity.ApId) : -1;

            // Check if only one intensity time is specified
            // If it corresponds with Activity Time and Intensity then, clear IntensityX property
            if (ActivityInfo != null &&
                _data.IsMixedIntensitySpecified() &&
                _data.IsSingleIntensitySpecified(ActivityInfo.Time, intensity)) {
                for (int i = 0; i <= 5; ++i) {
                    _data.Intensities[i] = null;
                }
            }
            else {
                for (int i = 0; i <= 5; ++i) {
                    _data.Intensities[i] = GetValue(_iTextBoxes[i].Text);
                }
            }

            _data.SpikedControls = GetValue(tbSpiked.Text);
            _data.TotalControls = GetValue(tbTotal.Text);
            _data.TechnicalIntensityId = GetValue(comboTechnicalIntensity.SelectedValue);
            _data.CourseName = GetValue(comboBoxCourseName.Text);
            _data.CourseLength = GetValue(tbDistance.Text);
            _data.CourseClimb = GetValue(tbClimb.Text);
            _data.PrivateNote = GetValue(tbPrivateNote.Text);
        }

        private string GetValue(object o) {
            if (o == null) return null;
            var s = o.ToString().Trim();
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public void RefreshPage() {
            var apData = ApPlugin.ApConfig.Profile;

            // Display total time and intensities' time
            if (ActivityInfo != null && !ApPlugin.ApConfig.IsMappingEmpty) {
                if (_data.IsMixedIntensitySpecified()) {
                    lblTotalTime.Text = FormatTime(_data.GetMixedIntensityTime());                        
                }
                else {
                    Array.ForEach(_iTextBoxes, t => t.Text = null);

                    var defaultTextBox = tbI3; // the default intensity is 3

                    // If intensities's times are not specified,
                    // choose a default intensity and set its time to be activity time
                    var stIntensity = ApPlugin.ApConfig.Mapping.Intensities.Find(i => i.StId == Activity.Intensity.ToString());
                    if (stIntensity != null && stIntensity.ApId != "-1") {
                        var index = int.Parse(stIntensity.ApId);
                        _data.Intensities[index] = FormatTime(ActivityInfo.Time);
                        defaultTextBox = _iTextBoxes[index];
                    }

                    defaultTextBox.Text = lblTotalTime.Text =
                        FormatTime(ActivityInfo.Time);
                }
            }
            else {
                lblTotalTime.Text = "N/A";
            }

            comboWorkout.DisplayMember = "Title";
            comboWorkout.ValueMember = "Id";
            comboWorkout.DataSource = apData.Workouts;

            comboTechnicalIntensity.DisplayMember = "Title";
            comboTechnicalIntensity.ValueMember = "Id";
            comboTechnicalIntensity.DataSource = apData.TechnicalIntensities;

            var workoutId = _data != null ? _data.WorkoutId : null;
            if (workoutId == null && Activity != null && !ApPlugin.ApConfig.IsMappingEmpty) {
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

            if (Activity != null && !ApPlugin.ApConfig.IsMappingEmpty) {
                if (apData.ContainsTechnicalIntensityId(_data.TechnicalIntensityId)) {
                    comboTechnicalIntensity.SelectedValue = _data.TechnicalIntensityId;
                }
                else {
                    comboTechnicalIntensity.SelectedIndex = 0;
                }

                
                _defaultUnits = ApPlugin.GetDistanceUnits();

                if (_data.IsMixedIntensitySpecified()) {
                    for (int i = 0; i <= 5; ++i) {
                        _iTextBoxes[i].Text = _data.Intensities[i];
                    }
                }

                tbSpiked.Text = _data.SpikedControls;
                tbTotal.Text = _data.TotalControls;
                comboBoxCourseName.Text = _data.CourseName;
                tbDistance.Text = _data.CourseLength;
                tbClimb.Text = _data.CourseClimb;

                tbPrivateNote.Text = _data.PrivateNote;

                panel1.Enabled = true;
                pIntensity.Enabled = apData.AdvancedFeaturesEnabled;
                tbPrivateNote.Enabled = apData.AdvancedFeaturesEnabled;
            }
            else {
                comboWorkout.SelectedIndex = -1;
                comboTechnicalIntensity.SelectedIndex = -1;
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
                panel1.Enabled = false;
            }

        }

        public void ThemeChanged(ITheme visualTheme) {
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
            //UpdateData();
        }

        private void ValidateTimePerIntensity(object sender, EventArgs e) {
            var textBox = (ZoneFiveSoftware.Common.Visuals.TextBox)sender;
            var text = textBox.Text.Trim();
            var ts = ApActivityData.GetIntensityTime(text);

            int time;
            if (!string.IsNullOrEmpty(text) && (!int.TryParse(text, out time) || (time != 0 && ts == TimeSpan.Zero))) {
                MessageBox.Show("Invalid time per intensity specified.\nUse 'hhmmss' format. The time must be less than 24 hours.");
                textBox.Focus();
            }
            else {
                textBox.Text = FormatTime(ts);
                var index = int.Parse(textBox.Name[textBox.Name.Length - 1].ToString());
                _data.Intensities[index] = textBox.Text;
                lblTotalTime.Text = FormatTime(_data.GetMixedIntensityTime());
            }
        }

     }
}
