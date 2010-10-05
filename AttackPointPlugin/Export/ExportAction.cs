using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;

using GK.AttackPoint;
using GK.SportTracks.AttackPoint;
using System.Text;
using System.Diagnostics;
using GK.SportTracks.AttackPoint.Properties;
using GK.SportTracks.AttackPoint.UI;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using ZoneFiveSoftware.Common.Visuals.Util;

namespace GK.SportTracks.AttackPoint.Export
{

    delegate void UploadDelegate(ExportDialog dialog, DoWorkEventArgs ee, List<ApNote> notes, List<IActivity> activities);

    public abstract class ExportAction : IAction
    {
        private IDailyActivityView _dailyActivityView;
        private IActivityReportsView _activityReportsView;
        private IActivity _activity;
        private IList<IActivity> _activities;
        private static List<ExportWarning> Warnings = new List<ExportWarning>();
        protected static CultureInfo _formatProvider = new CultureInfo("en-US"); // Use US culture formatting numbers

        static ExportAction() {
            var warnings = Enum.GetNames(typeof(ExportWarning));
            for (int i = 1; i < warnings.Length; ++i) { // Skip None
                Warnings.Add((ExportWarning)Enum.Parse(typeof(ExportWarning), warnings[i]));
            }
        }

        public ExportAction(IDailyActivityView view) {
            _dailyActivityView = view;
        }

        public ExportAction(IActivityReportsView view) {
            _activityReportsView = view;
        }

        private IList<IActivity> Activities {
            get {
                if (_dailyActivityView != null) {
                    return CollectionUtils.GetAllContainedItemsOfType<IActivity>(_dailyActivityView.SelectionProvider.SelectedItems);
                }
                else {
                    return CollectionUtils.GetAllContainedItemsOfType<IActivity>(_activityReportsView.SelectionProvider.SelectedItems);
                }
            }
        }

        protected bool BatchMode { get { return _activities != null && _activities.Count > 1; } }

        public void Refresh() { }

        public void Run(Rectangle rectButton) {
            _activities = Activities;

            if (BatchMode) {
                ExportMany();
            }
            else {
                ExportSingle();
            }

        }

        private void ExportMany() {
            var dialog = new ExportDialog(ApPlugin.GetApplication().VisualTheme);

            var results = new ExportDialog.PreExportResults();
            dialog.Execute(Form.ActiveForm, "AttackPoint Plugin", "Processing activities...",
                (s, ee) => {
                    try {
                        Thread.Sleep(500); // Delay this thread to make sure the dialog handle is created before the following code executes.
                        ee.Result = results;
                        results.Notes = new List<ApNote>();
                        results.Activities = _activities;

                        dialog.UpdateProgress("Analyzing activities...", null);
                        int i = 1;
                        foreach (var activity in _activities) {
                            dialog.UpdateProgress(null, "Analyzing activity: " + i);
                            var note = CreateNote();
                            var edata = new ExportConfig() {
                                ActivityData = ApPlugin.GetApData(activity),
                                Logbook = ApPlugin.GetApplication().Logbook,
                                SystemPreferences = ApPlugin.GetApplication().SystemPreferences,
                                Metadata = ApPlugin.Metadata,
                                Config = ApPlugin.ApConfig
                            };

                            var error = Populate(note, activity, edata);
                            if (error == null) {
                                results.Notes.Add(note);

                                StringBuilder sb;
                                if (edata.Warnings != ExportWarning.None &&
                                    ((sb = ProcessWarnings(edata)).Length != 0)) {
                                    results.IsWarning = true;
                                    dialog.UpdateProgress(string.Format("{2}Warning(s) in {0}: {1}", ApPlugin.GetCaption(activity), sb.ToString().Trim(), Environment.NewLine), null);
                                }
                            }
                            else {
                                results.IsError = true;
                                dialog.UpdateProgress(string.Format("{2}Error in {0}: {1}", ApPlugin.GetCaption(activity), GetMessage(error.Value), Environment.NewLine), null);
                            }
                            ++i;
                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception ex) {
                        results.Error = ex;
                    }
                });

            dialog.Dispose();
        }

        private StringBuilder ProcessWarnings(ExportConfig edata) {
            var sb = new StringBuilder();
            foreach (var warning in Warnings) {
                if (warning == ExportWarning.EquipmentNotMapped && !ApPlugin.ApConfig.WarnOnNotMappedEquipment) {
                    continue;
                }
                else if (warning == ExportWarning.IntensityNotSpecified && !ApPlugin.ApConfig.WarnOnUnspecifiedIntensity) {
                    continue;
                }

                if ((warning & edata.Warnings) != 0) {
                    sb.AppendLine(GetMessage(warning));
                }
            }
            return sb;
        }

        private void ExportSingle() {
            _activity = _activities[0];

            var dialog = new InformationDialog(ApPlugin.GetApplication().VisualTheme);

            dialog.Execute(Form.ActiveForm, "AttackPoint Plugin", "Exporting to AttackPoint...",
                (s, ee) =>
                {
                    try {
                        Thread.Sleep(500); // Delay this thread to make sure the dialog handle is created before the following code executes.
                        var note = CreateNote();
                        var edata = new ExportConfig()
                        {
                            ActivityData = ApPlugin.GetApData(_activity),
                            Logbook = ApPlugin.GetApplication().Logbook,
                            SystemPreferences = ApPlugin.GetApplication().SystemPreferences,
                            Metadata = ApPlugin.Metadata,
                            Config = ApPlugin.ApConfig
                        };

                        var error = Populate(note, _activity, edata);

                        if (error == null) {
                            bool upload = true;
                            if (edata.Warnings != ExportWarning.None) {
                                var sb = ProcessWarnings(edata);
                                if (sb.Length > 0) {
                                    sb.AppendLine("Do you want to proceed with export?");
                                    upload = ShowWarning(dialog, sb.ToString());
                                }
                            }

                            if (upload) {
                                var proxy = ApPlugin.GetProxy();
                                proxy.Upload(note);
                                ee.Result = "Export completed.";
                            }
                            else {
                                throw new IgnoreException();
                            }
                        }
                        else {
                            ShowError(dialog, GetMessage(error.Value));
                            throw new IgnoreException();
                        }
                    }
                    catch (Exception ex) {
                        ee.Result = ex;
                    }
                });

            dialog.Dispose();
        }

        protected abstract ApNote CreateNote();
        public abstract ExportError? Populate(ApNote note, IActivity activity, ExportConfig edata);
        public abstract string Title { get; }
        public virtual Image Image { get { return Properties.Resources.FavIcon.ToBitmap(); } }
        public virtual bool HasMenuArrow { get { return false; } }
        public virtual bool Enabled { get { return !ApPlugin.ApConfig.IsMappingEmpty && Activities != null && Activities.Count > 0; } }
        public bool Visible { get { return true; } }
        public IList<string> MenuPath { get { return new List<string>(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected string ConvertToString(float p, int decimals) {
            if (HasValue(p))
                return Math.Round(p, decimals).ToString(_formatProvider);
            return null;
        }

        protected string ConvertToString(double p, int decimals) {
            if (HasValue(p))
                return Math.Round(p, decimals).ToString(_formatProvider);
            return null;
        }

        protected bool HasValue(float p) {
            return !float.IsNaN(p) && p != 0;
        }

        protected bool HasValue(double p) {
            return !double.IsNaN(p) && p != 0;
        }

        protected string ConvertToString(float f) {
            return float.IsNaN(f) ? null : f.ToString(_formatProvider);
        }

        protected string ConvertToString(double f) {
            return double.IsNaN(f) ? null : f.ToString(_formatProvider);
        }

        protected string ConvertToString(int f) {
            return f.ToString(_formatProvider);
        }

        protected string ConvertToString(bool b) {
            return b ? "1" : null;
        }

        delegate void ShowErrorHandler(Form form, string message, string caption);
        delegate bool ShowWarningHandler(Form form, string message, string caption);

        protected void ShowError(Form parent, string message) {
            if (BatchMode)
                throw new ApplicationException(message);

            parent.Invoke(new ShowErrorHandler(ShowError), parent, message, "Export error");
        }

        private void ShowError(Form parent, string message, string caption) {
            MessageBox.Show(parent, message, caption);
        }

        protected bool ShowWarning(Form parent, string message) {
            return (bool)parent.Invoke(new ShowWarningHandler(ShowWarningDialog), parent, message, "Warning");
        }

        private bool ShowWarningDialog(Form parent, string message, string caption) {
            return DialogResult.Yes == MessageBox.Show(parent, message, caption, MessageBoxButtons.YesNo);
        }

        private string GetMessage(ExportError error) {
            return Resources.ResourceManager.GetString("ExportError_" + error.ToString()).Replace("\\n", "\n");
        }

        private string GetMessage(ExportWarning warning) {
            return Resources.ResourceManager.GetString("ExportWarning_" + warning.ToString()).Replace("\\n", "\n");
        }

        protected DateTime AdjustDateTime(DateTime dateTime) {
            return ApPlugin.AdjustDateTime(dateTime);
        }

    }

    public enum ExportError
    {
        DateNotSpecified,
        CategoryNotFound,
        CategoryNotMapped,
        TimeNotSpecified,
        IntensityNotFound,
        IntensityNotMapped,
        EquipmentNotFound
    }

    [Flags]
    public enum ExportWarning
    {
        None = 0,
        EquipmentNotMapped = 1,
        IntensityNotSpecified = 2,
        FormatNotesFailed = 4,
        FormatPrivateNoteFailed = 8
    }

    public class ExportConfig
    {
        public ExportConfig() {
            Warnings = ExportWarning.None;
        }

        public ApActivityData ActivityData { get; set; }
        public ILogbook Logbook { get; set; }
        public ISystemPreferences SystemPreferences { get; set; }
        public ApMetadata Metadata { get; set; }
        public ApConfig Config { get; set; }
        public ExportWarning Warnings { get; set; }
    }

}
