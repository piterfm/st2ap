using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace GK.AttackPoint
{
    public class LogManager
    {
        private static ILogger _logger;

        public static ILogger Logger {
            get {
                if (_logger == null) {
                    _logger = new DefaultLogger();
                }
                return _logger;
            }
            set {
                _logger = value;
            }
        }

        public class DefaultLogger : ILogger
        {
            public const long DefaultMaxFileSize = 1024 * 512; // 0.5 MB
            protected const string LogFileName = "attackpoint-plugin.log";
            private const string EventLogSource = "AttackPoint Plugin";
            protected string _logFile;
            protected bool _isDebug;
            protected bool _writeToWindowsEventLog;
            protected long _maxFileSize = DefaultMaxFileSize;

            public DefaultLogger() {
                _logFile = LogFileName;
                _isDebug = true;
                _writeToWindowsEventLog = false;
            }

            public bool IsDebug { get { return _isDebug; } }

            public void PrintMessage(string message) {
                PrintMessage(message, null);
            }

            public void PrintMessage(string message, Exception ex) {
                if (!_isDebug) return;
                if (ex != null) {
                    Debug.WriteLine(message + Environment.NewLine + ex);
                }
                else {
                    Debug.WriteLine(message);
                }
            }

            public void LogMessage(string message) {
                LogMessage(message, null);
            }

            public void LogMessage(string message, Exception ex) {
                try {
                    message = string.Format("{0}: {1}", DateTime.Now, message);
                    if (ex != null) {
                        message += Environment.NewLine + ex;
                    }

                    using (StreamWriter writer = new StreamWriter(_logFile, IsAppend())) {
                        PrintMessage(message);
                        writer.WriteLine(message);
                        WriteMessageToEventLog(message, EventLogEntryType.Error);
                    }
                }
                catch (Exception e) {
                    LogFatal(message, e, ex);
                }

            }

            public void PrintWebResponse(HttpWebResponse response) {
                if (!_isDebug) return;
                OutputResponseStream(response, null);
            }

            public void LogWebResponse(HttpWebResponse response) {
                try {
                    WriteMessageToEventLog(string.Format("Response stream received. Status: {0} - {1}. See log files for details.",
                        response.StatusCode, response.StatusDescription), EventLogEntryType.Error);

                    using (StreamWriter writer = new StreamWriter(_logFile, IsAppend())) {
                        OutputResponseStream(response, writer);
                    }
                }
                catch (Exception ex) {
                    LogFatal("Unable to log response.", ex, null);
                }
            }

            private void OutputResponseStream(HttpWebResponse response, TextWriter writer) {
                StreamReader readStream = null;
                try {
                    var stream = response.GetResponseStream();
                    var encoding = Encoding.GetEncoding("utf-8");
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    readStream = new StreamReader(stream, encoding);
                    var str = string.Format("{3}{0}: Response stream received. Status: {1} - {2} ****************",
                            DateTime.Now, response.StatusCode, response.StatusDescription, Environment.NewLine);
                    PrintMessage(str);
                    if (writer != null)
                        writer.WriteLine(str);
                    var read = new char[256];
                    // Reads 256 characters at a time.    
                    var count = readStream.Read(read, 0, 256);
                    str = "HTML..." + Environment.NewLine;
                    PrintMessage(str);
                    if (writer != null)
                        writer.WriteLine(str);
                    while (count > 0) {
                        str = new string(read, 0, count);
                        PrintMessage(str);
                        if (writer != null)
                            writer.Write(str);
                        count = readStream.Read(read, 0, 256);
                    }
                    PrintMessage(string.Empty);
                    if (writer != null)
                        writer.WriteLine(string.Empty);

                }
                catch (Exception ex) {
                    LogFatal("Unable to print response.", ex, null);
                }
                finally {
                    if (readStream != null) readStream.Close();
                }
            }

            protected void LogFatal(string message, Exception ex, Exception oex) {
                message = string.Format("Unable to log message.{0}----- ORIGINAL MESSAGE/EXCEPTION ----{0}{1}{0}{2}{0}--------- END -------{0}{3}",
                        Environment.NewLine,
                        message,
                        (oex == null ? "<No orignal exception>" : oex.ToString()),
                        ex);
                Debug.WriteLine(message);
                WriteMessageToEventLog(message, EventLogEntryType.Error);
            }

            private void WriteMessageToEventLog(string message, EventLogEntryType type) {
                try {
                    if (!EventLog.SourceExists(EventLogSource)) {
                        EventLog.CreateEventSource(EventLogSource, "Application");
                    }

                    EventLog log = new EventLog();
                    log.Source = EventLogSource;
                    log.WriteEntry(message, type);
                }
                catch (Exception ex) {
                    Debug.WriteLine("Failed to write to Windows Event Log: " + ex);
                }
            }

            private bool IsAppend() {
                bool append = true;
                if (File.Exists(_logFile)) {
                    var file = new FileInfo(_logFile);
                    append = file.Length < _maxFileSize;
                }
                return append;
            }

        }
    }
}
