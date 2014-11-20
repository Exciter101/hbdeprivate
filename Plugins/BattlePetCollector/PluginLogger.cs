using Styx.Common;
using System;

namespace BattlePetCollector
{
    public class PluginLogger : IPluginLogger
    {
        public void Write(string message)
        {
            if (Logging.LoggingLevel >= LogLevel.Normal)
            {
                Logging.Write(System.Windows.Media.Colors.Cyan, "[BPC] " + message);
            }
        }

        public void WriteError(Exception e)
        {
            if (Logging.LoggingLevel >= LogLevel.None)
            {
                Logging.Write(System.Windows.Media.Colors.Red, "[BPC] " + e.Message);
                WriteVerbose(e.ToString());
            }
        }
        public void WriteError(string message,Exception e)
        {
            if (Logging.LoggingLevel >= LogLevel.None)
            {
                Logging.Write(System.Windows.Media.Colors.Red, "[BPC] " + message + ". " + e.Message);
                WriteVerbose(e.ToString());
            }
        }

        public void WriteVerbose(string message)
        {
            if (Logging.LoggingLevel >= LogLevel.Verbose)
            {
                Logging.WriteVerbose(System.Windows.Media.Colors.Cyan, "[BPC] " + message);
            }
        }

        private int _lastStatus = int.MinValue;
        public void StatusUpdate(string message)
        {
            if (_lastStatus + 3000 < Environment.TickCount)
            {
                Write(message+" " +DateTime.Now.ToLongTimeString());
                _lastStatus = Environment.TickCount;
            }
        }
    }
}