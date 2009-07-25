using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;

namespace GK.SportTracks.AttackPoint.UI.Activities
{
    class ApActivityPage : IActivityDetailPage
    {
        private IActivity _activity;
        private ApActivityData _data;
        private ApActivityControl _control;

        public IActivity Activity {
            set {
                // Save AP data if change current activity to null or different activity
                if ((_activity != null && value == null) ||
                    (_activity != null && value != null && _activity.ReferenceId != value.ReferenceId)) {

                    if (_control != null) {
                        _control.UpdateData();
                        ApPlugin.SaveApData(_activity, _data);
                    }

                }

                // Retrieve AP data from logbook only if a new activity is assigned
                if ((_activity == null && value != null) ||
                    (_activity != null && value != null && _activity.ReferenceId != value.ReferenceId)) {
                    _activity = value;
                    _data = ApPlugin.GetApData(_activity);
                    if (_control != null) {
                        _control.Data = _data;
                        _control.Activity = _activity;
                    }
                }
                else {
                    _activity = value;
                }

                if (_activity == null) {
                    _data = null;
                    if (_control != null) {
                        _control.Data = _data;
                        _control.Activity = _activity;
                    }
                }

                if (_control != null) {
                    _control.RefreshPage();
                }

            }
        }

        public void RefreshPage() {
            if (_control != null) {
                _control.RefreshPage();
            }
        }


        public Control CreatePageControl() {
            if (_control == null) {
                _control = new ApActivityControl();
                _control.Data = _data;
                _control.Activity = _activity;
                _control.RefreshPage();
            }
            return _control;
        }

        public bool HidePage() {
            if (_control != null) {
                _control.UpdateData();
            }

            if (_activity != null) {
                ApPlugin.SaveApData(_activity, _data);
            }
            return true;
        }

        public string PageName {
            get { return Title; }
        }

        public void ShowPage(string bookmark) {
        }

        public IPageStatus Status {
            get { return null; }
        }

        public void ThemeChanged(ITheme visualTheme) {
            if (_control != null) {
                _control.ThemeChanged(visualTheme);
            }
        }

        public string Title {
            get { return "AttackPoint"; }
        }

        public void UICultureChanged(CultureInfo culture) {
            if (_control != null) {
                //_control.UICultureChanged(culture);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
