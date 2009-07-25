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

namespace GK.SportTracks.AttackPoint.Export
{
    public abstract class ExportAction : IAction
    {
        private IActivity activity;
        private bool _batchMode = false;

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
                        var error = Populate(note, activity, ApPlugin.GetApData(activity), ApPlugin.GetApplication().Logbook, proxy.Metadata, ApPlugin.ApConfig);
                        if (error == null || error == ExportError.EquipmentNotMapped) {
                            bool upload = true;
                            if (ApPlugin.ApConfig.WarnOnNotMappedEquipment && error == ExportError.EquipmentNotMapped) {
                                upload = ShowWarning(dialog, GetMessage(error.Value), "Warning");
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
        public abstract ExportError? Populate(ApNote note, IActivity activity, ApActivityData data, ILogbook logbook, ApMetadata metadata, ApConfig config);
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


        protected string ConvertToEmptyIfNull(string s) {
            return s == null ? string.Empty : s;
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

        protected bool ShowWarning(Form parent, string message, string caption) {
            return (bool)parent.Invoke(new ShowWarningHandler(ShowWarningDialog), parent, message, caption);
        }

        private bool ShowWarningDialog(Form parent, string message, string caption) {
            return DialogResult.Yes == MessageBox.Show(parent, message, caption, MessageBoxButtons.YesNo);
        }

        private string GetMessage(ExportError error) {
            return Resources.ResourceManager.GetString("ExportError_" + error.ToString()).Replace("\\n", "\n");
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
        EquipmentNotFound,
        EquipmentNotMapped
    }
}
