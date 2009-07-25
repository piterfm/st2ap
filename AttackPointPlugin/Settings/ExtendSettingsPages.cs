using System;
using System.Collections.Generic;
using System.Text;

using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;

namespace GK.SportTracks.AttackPoint.Settings
{
    class ExtendSettingsPages : IExtendSettingsPages
    {
        public IList<ISettingsPage> SettingsPages
        {
            get
            {
                List<ISettingsPage> pageList = new List<ISettingsPage>();
                pageList.Add(new SettingsPage());
                return pageList;
            }
        }

    }
}
