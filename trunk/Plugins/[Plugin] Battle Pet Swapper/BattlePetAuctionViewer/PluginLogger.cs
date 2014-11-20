using Styx.Common;
using System;

namespace BattlePetAuctionViewer
{
    public class PluginLogger : IPluginLogger
    {
        public void Write(string message)
        {
            if (Logging.LoggingLevel >= LogLevel.Normal)
            {
                Logging.Write(System.Windows.Media.Colors.Yellow, "[BPAV] " + message);
            }
        }

        public void WriteError(Exception e)
        {
            if (Logging.LoggingLevel >= LogLevel.None)
            {
                Logging.Write(System.Windows.Media.Colors.Red, "[BPAV] " + e.Message);
                WriteVerbose(e.ToString());
            }
        }
        public void WriteError(string message,Exception e)
        {
            if (Logging.LoggingLevel >= LogLevel.None)
            {
                Logging.Write(System.Windows.Media.Colors.Red, "[BPAV] " + message + ". " + e.Message);
                WriteVerbose(e.ToString());
            }
        }

        public void WriteVerbose(string message)
        {
            if (Logging.LoggingLevel >= LogLevel.Verbose)
            {
                Logging.WriteVerbose(System.Windows.Media.Colors.Yellow, "[BPAV] " + message);
            }
        }
    }
}