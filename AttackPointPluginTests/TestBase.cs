using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;
using GK.AttackPoint;
using GK.SportTracks.AttackPoint;
using GK.Utils;

namespace AttackPointPluginTests
{
    /// <summary>
    /// Base class that intitializes console only logging.
    /// </summary>
    public class TestBase
    {
        public TestBase() {
            LogManager.Logger = new ConsoleLogger();
        }

        class ConsoleLogger : ILogger
        {
            public bool IsDebug {
                get { return false; }
            }

            public void PrintMessage(string message) {
            }

            public void PrintMessage(string message, Exception ex) {
            }

            public void LogMessage(string message) {
                LogMessage(message, null);
            }

            public void LogMessage(string message, Exception ex) {
                var sb = new StringBuilder(message);
                if (ex != null) {
                    sb.AppendLine().Append(ex);
                }
                Debug.WriteLine(sb);
            }

            public void PrintWebResponse(string url, IHttpResponseWrapper response) {
            }

            public void LogWebResponse(string url, IHttpResponseWrapper response) {
            }

        }

    }

}
