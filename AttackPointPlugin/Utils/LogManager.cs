using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace GK.Utils
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
            protected const string DefaultLogFileName = "attackpoint-plugin.log";
            private const string EventLogSource = "AttackPoint Plugin";
            protected string _logFile;
            protected bool _isDebug;
            protected bool _writeToWindowsEventLog;
            protected long _maxFileSize = DefaultMaxFileSize;

            public DefaultLogger() {
                _logFile = DefaultLogFileName;
                _isDebug = true;
                _writeToWindowsEventLog = false;
            }

            public bool IsDebug {
                get { return _isDebug; }
                set { _isDebug = value; }
            }

            public string LogFileName { get { return _logFile; } }

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

                    PrintMessage(message);
                    WriteMessageToEventLog(message, EventLogEntryType.Error);

                    using (StreamWriter writer = new StreamWriter(_logFile, IsAppend())) {
                        writer.WriteLine(message);
                    }
                }
                catch (Exception e) {
                    LogFatal(message, e, ex);
                }

            }

            public void PrintWebResponse(string url, IHttpResponseWrapper response)
            {
                if (!_isDebug) return;
                OutputResponseStream(url, response, null);
            }

            public void LogWebResponse(string url, IHttpResponseWrapper response)
            {
                try {
                    using (StreamWriter writer = new StreamWriter(_logFile, IsAppend())) {
                        OutputResponseStream(url, response, writer);
                    }
                }
                catch (Exception ex) {
                    LogFatal("Unable to log response.", ex, null);
                }
            }

            private void OutputResponseStream(string url, IHttpResponseWrapper response, TextWriter writer)
            {
                StreamReader readStream = null;
                try {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{1}*********************{1}{0}: Response received{1}", DateTime.Now, Environment.NewLine);
                    sb.AppendFormat("URL: {0}", url).AppendLine();
                    sb.AppendFormat("Status: {0} - {1}", response.StatusCode, response.StatusDescription).AppendLine();
                    sb.AppendFormat("Protocol/Method: {0} - {1}", response.ProtocolVersion, response.Method).AppendLine();
                    sb.AppendFormat("Response URI: {0}", response.ResponseUri).AppendLine();
                    sb.AppendFormat("Character set: {0}", response.CharacterSet).AppendLine();
                    sb.AppendFormat("Character encoding: {0}", response.ContentEncoding).AppendLine();
                    if (response.Headers != null) {
                        foreach (var header in response.Headers.AllKeys) {
                            sb.AppendFormat("{0}={1}", header, response.Headers[header]).AppendLine();
                        }
                    }
                    WriteMessage(writer, sb.ToString());

                    var stream = response.GetResponseStream();
                    var encoding = Encoding.GetEncoding("utf-8");
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    readStream = new StreamReader(stream, encoding);
                    
                    var read = new char[256];
                    // Reads 256 characters at a time.    
                    var count = readStream.Read(read, 0, 256);
                    WriteMessage(writer, "--- START HTML ---" + Environment.NewLine);
                    while (count > 0) {
                        var str = new string(read, 0, count);
                        WriteMessage(writer, str);
                        count = readStream.Read(read, 0, 256);
                    }
                    WriteMessage(writer, "--- END HTML ---" + Environment.NewLine);
                }
                catch (Exception ex) {
                    LogFatal("Unable to print response.", ex, null);
                }
                finally {
                    if (readStream != null) readStream.Close();
                }
            }

            private void WriteMessage(TextWriter writer, string str) {
                PrintMessage(str);
                if (writer != null)
                    writer.WriteLine(str);
            }

            protected void LogFatal(string message, Exception ex, Exception oex) {
                try {
                    message = string.Format("Unable to log message.{0}----- ORIGINAL MESSAGE/EXCEPTION ----{0}{1}{0}{2}{0}--------- END -------{0}{3}",
                            Environment.NewLine,
                            message,
                            (oex == null ? "<No orignal exception>" : oex.ToString()),
                            ex);
                    Debug.WriteLine(message);
                    WriteMessageToEventLog(message, EventLogEntryType.Error);
                }
                catch (Exception shouldNeverHappenEx) {
                    Debug.WriteLine("Totally screwed up while logging: " + shouldNeverHappenEx);
                }
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
