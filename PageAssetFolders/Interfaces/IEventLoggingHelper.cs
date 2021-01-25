using System;

namespace KenticoCommunty.PageAssetFolders.Interfaces
{
    public interface IEventLoggingHelper
    {
        /// <summary>
        /// Log a warning to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="message"></param>
        void LogWarning(string source, string eventCode, string message);

        /// <summary>
        /// Log an exception to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="ex"></param>
        void LogException(string source, string eventCode, Exception ex);

        /// <summary>
        /// /// Log an error to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="message"></param>
        void LogError(string source, string eventCode, string message);

        /// <summary>
        /// Log an information message to the Kentico event log
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventCode"></param>
        /// <param name="message"></param>
        void LogInformation(string source, string eventCode, string message);
    }
}
