using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GK.AttackPoint
{
    public interface ILogger
    {
        void PrintMessage(string message);
        void PrintMessage(string message, Exception ex);

        void LogMessage(string message);
        void LogMessage(string message, Exception ex);

        void PrintWebResponse(HttpWebResponse response);
        void LogWebResponse(HttpWebResponse response);
    }
}
