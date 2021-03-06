﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GK.Utils
{
    public interface ILogger
    {
        bool IsDebug { get; set; }
        string LogFileName { get; }

        void PrintMessage(string message);
        void PrintMessage(string message, Exception ex);

        void LogMessage(string message);
        void LogMessage(string message, bool writeToEventLog);
        void LogMessage(string message, Exception ex);

        void PrintWebResponse(string url, IHttpResponseWrapper response);
        void LogWebResponse(string url, IHttpResponseWrapper response);
    }
}
