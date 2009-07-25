using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals;
using System.Windows.Forms;

namespace GK.SportTracks.AttackPoint.Settings
{
    public class SettingsPage : ISettingsPage
    {
        private SettingsControl _settingsControl;

        public Guid Id {
            get { throw new NotImplementedException(); }
        }

        public IList<ISettingsPage> SubPages {
            get { return null; }
        }


        public Control CreatePageControl() {
            _settingsControl = new SettingsControl();
            return _settingsControl;
        }

        public bool HidePage() {
            _settingsControl.UpdateConfiguration(ApPlugin.ApConfig);
            return true;
        }

        public string PageName {
            get { return "AttackPoint"; }
        }

        public void ShowPage(string bookmark) {
            if (_settingsControl != null)
                _settingsControl.RefreshPage();
        }

        public IPageStatus Status {
            get { return null; }
        }

        public void ThemeChanged(ITheme visualTheme) {
            if (_settingsControl != null)
                _settingsControl.ThemeChanged(visualTheme);
        }

        public string Title {
            get { return "AttackPoint settings"; }
        }

        public void UICultureChanged(System.Globalization.CultureInfo culture) {
            //NOOP
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

    }

}
