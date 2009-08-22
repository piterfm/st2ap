using System;
using System.Collections.Generic;

using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data.GPS;
using ZoneFiveSoftware.Common.Visuals;
using ZoneFiveSoftware.Common.Visuals.Fitness;
using System.IO;
using System.Xml.Serialization;

namespace GK.SportTracks.AttackPoint.Export
{
    class ExtendActions : IExtendActivityExportActions
    {

        public IList<IAction> GetActions(IList<IActivity> activities) {
            return new IAction[] { new ExportTrainingAction(activities), new ExportNoteAction(activities) };
        }

        public IList<IAction> GetActions(IActivity activity) {
            return new IAction[] { new ExportTrainingAction(activity), new ExportNoteAction(activity) };
        }

    }
}
