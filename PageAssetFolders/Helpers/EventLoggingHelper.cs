using System;
using CMS;
using CMS.EventLog;
using KenticoCommunty.PageAssetFolders.Helpers;
using KenticoCommunty.PageAssetFolders.Interfaces;

namespace KenticoCommunty.PageAssetFolders.Helpers
{
    public class EventLoggingHelper : IEventLoggingHelper
    {
        /// <summary>
        /// Log an exception to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="ex"></param>
        public void LogException(string source, string eventCode, Exception ex)
        {
            EventLogProvider.LogException(source, eventCode, ex);
        }

        /// <summary>
        /// /// Log a warning to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="message"></param>
        public void LogWarning(string source, string eventCode, string message)
        {
            EventLogProvider.LogEvent(EventType.WARNING, source, eventCode, message);
        }

        /// <summary>
        /// /// Log an error to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="message"></param>
        public void LogError(string source, string eventCode, string message)
        {
            EventLogProvider.LogEvent(EventType.ERROR, source, eventCode, message);
        }

        /// <summary>
        /// Log an information message to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="message"></param>
        public void LogInformation(string source, string eventCode, string message)
        {
            EventLogProvider.LogInformation(source, eventCode, message);
        }
    }
}
