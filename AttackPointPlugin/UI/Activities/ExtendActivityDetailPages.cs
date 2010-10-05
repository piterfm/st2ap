using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using ZoneFiveSoftware.Common.Visuals;

namespace GK.SportTracks.AttackPoint.UI.Activities
{
    class ExtendActivityDetailPages : IExtendActivityDetailPages
    {
        public IList<IDetailPage> GetDetailPages(IDailyActivityView view, ExtendViewDetailPages.Location location) {
            return new IDetailPage[] { new ApActivityPage(view) };
        }


    }
}
