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

namespace GK.SportTracks.AttackPoint.Export
{
    public abstract class ExportAction : IAction
    {
        private IActivity activity;
        private bool _batchMode = false;
        private static List<ExportWarning> Warnings = new List<ExportWarning>();

        static ExportAction() {
            var warnings = Enum.GetNames(typeof(ExportWarning));
            for (int i = 1; i < warnings.Length; ++i) { // Skip None
                Warnings.Add((ExportWarning)Enum.Parse(typeof(ExportWarning), warnings[i]));
            }
        }

        public ExportAction(IActivity activity) {
            this.activity = activity;
        }

        public void Refresh() {
        }

        public void Run(Rectangle rectButton) {
            var dialog = new InformationDialog(ApPlugin.GetApplication().VisualTheme);

            dialog.Execute(Form.ActiveForm, "AttackPoint Plugin", "Exporting to AttackPoint...",
                (s, ee) =>
                {
                    try {
                        var proxy = ApPlugin.GetProxy();
                        var note = CreateNote();
                        var edata = new ExportConfig() {
                            ActivityData = ApPlugin.GetApData(activity),
                            Logbook = ApPlugin.GetApplication().Logbook,
                            Metadata = proxy.Metadata,
                            Config = ApPlugin.ApConfig
                        };

                        var error = Populate(note, activity, edata);

                        if (error == null) {
                            bool upload = true;
                            if (edata.Warnings != ExportWarning.None) {
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

                                if (sb.Length > 0) {
                                    sb.AppendLine("Do you want to proceed with export?");
                                    upload = ShowWarning(dialog, sb.ToString());
                                }
                            }

                            if (upload) {
                                var sb = new StringBuilder();
                                using (var w = new StringWriter(sb)) {
                                    var ser = new XmlSerializer(typeof(ApNote));
                                    ser.Serialize(w, note);
                                }

                                ApPlugin.Logger.PrintMessage(sb.ToString());
                                proxy.Upload(note);
                                ee.Result = "Export completed.";
                            }
                            else {
                                throw new InformationDialog.IgnoreException();
                            }
                        }
                        else {
                            ShowError(dialog, GetMessage(error.Value));
                            throw new InformationDialog.IgnoreException();
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
        public virtual bool Enabled { get { return !ApPlugin.ApConfig.IsMappingEmpty; } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected string ConvertToString(float p, int decimals) {
            if (HasValue(p))
                return Math.Round(p, decimals).ToString();
            return null;
        }

        protected string ConvertToString(double p, int decimals) {
            if (HasValue(p))
                return Math.Round(p, decimals).ToString();
            return null;
        }

        protected bool HasValue(float p) {
            return !float.IsNaN(p) && p != 0;
        }

        protected bool HasValue(double p) {
            return !double.IsNaN(p) && p != 0;
        }

        protected string ConvertToString(float f) {
            return float.IsNaN(f) ? null : f.ToString();
        }

        protected string ConvertToString(bool b) {
            return b ? "on" : null;
        }

        delegate void ShowErrorHandler(Form form, string message, string caption);
        delegate bool ShowWarningHandler(Form form, string message, string caption);

        protected void ShowError(Form parent, string message) {
            if (_batchMode)
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

    }

    public enum ExportError
    {
        DateNotSpecified,
        CategoryNotFound,
        CategoryNotMapped,
        TimeNotSpecified,
        IntensityNotFound,
        IntensityNotMapped,
        DistanceNotSpecified,
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
        public ApMetadata Metadata { get; set; }
        public ApConfig Config { get; set; }
        public ExportWarning Warnings { get; set; }
    }

}
