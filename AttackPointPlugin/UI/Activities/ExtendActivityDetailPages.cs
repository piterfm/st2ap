using System;
using System.Collections.Generic;
using System.Text;
using ZoneFiveSoftware.Common.Visuals.Fitness;

namespace GK.SportTracks.AttackPoint.UI.Activities
{
    class ExtendActivityDetailPages : IExtendActivityDetailPages
    {

        public IList<IActivityDetailPage> ActivityDetailPages {
            get {
                return new IActivityDetailPage[] {
                    new ApActivityPage()
                };
            }
        }

    }
}
