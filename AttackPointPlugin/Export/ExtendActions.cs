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
    class ExtendActions : IExtendDailyActivityViewActions, IExtendActivityReportsViewActions
    {
        public IList<IAction> GetActions(IDailyActivityView view, ExtendViewActions.Location location) {
            if (location == ExtendViewActions.Location.ExportMenu) {
                return new IAction[] { new ExportTrainingAction(view), new ExportNoteAction(view) };
            }
            else return new IAction[0];
        }
        public IList<IAction> GetActions(IActivityReportsView view, ExtendViewActions.Location location) {
            if (location == ExtendViewActions.Location.ExportMenu) {
                return new IAction[] { new ExportTrainingAction(view), new ExportNoteAction(view) };
            }
            else return new IAction[0];
        }

    }
}
