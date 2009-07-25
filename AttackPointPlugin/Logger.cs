using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using GK.AttackPoint;

namespace GK.SportTracks.AttackPoint
{
    public class Logger : LogManager.DefaultLogger
    {
        public Logger() {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ZoneFiveSoftware\SportTracks");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            _logFile = Path.Combine(path, LogFileName);
            _isDebug = Environment.GetEnvironmentVariable("DEBUG_ATTACKPOINT_PLUGIN", EnvironmentVariableTarget.User) == "true";
            _writeToWindowsEventLog = true;
        }

    }
}
