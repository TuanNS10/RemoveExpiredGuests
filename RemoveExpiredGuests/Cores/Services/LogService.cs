/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: LogService
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using RemoveExpiredGuests.Bases.Definitions;

/// <summary>
/// Application services
/// </summary>
namespace RemoveExpiredGuests.Cores.Services
{
    /// Instant using methods
    using static RemoveExpiredGuests.Bases.Definitions.Constants;

    /// <summary>
    /// Application log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Error log level
        /// </summary>
        Error,

        /// <summary>
        /// Warning log level
        /// </summary>
        Warning,

        /// <summary>
        /// Success log level
        /// </summary>
        Success
    }

    /// <summary>
    /// Application log service
    /// </summary>
    public class LogService : IValidation
    {
        /// <summary>
        /// Get or set the event source name
        /// </summary>
        private string EventSource { get; set; }

        /// <summary>
        /// Get or set the text log file path
        /// </summary>
        private string LogFile { get; set; }

        /// <summary>
        /// Get the application setting
        /// </summary>
        private AppSetting AppSetting
        {
            get
            {
                return AppSetting.GetInstance();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogService()
        {
            EventSource = AppSetting.EventSource;
            LogFile = "{0}\\RemoveExpiredGuests_{1}.log";
        }

        /// <summary>
        /// Validate log service object
        /// </summary>
        public void Validate()
        {
            LogFile = Format(LogFile, AppSetting.LogPath, DateTime.Now.ToString("yyyyMMdd"));
            if (Directory.Exists(AppSetting.LogPath) == false)
            {
                Directory.CreateDirectory(AppSetting.LogPath);
            }
        }

        /// <summary>
        /// Write error to log file and window event
        /// </summary>
        /// <param name="error">The error exception</param>
        public void WriteError(Exception error)
        {
            var logMessage = CreateMessage(error);

            WriteText(LogLevel.Error, logMessage);
            WriteEvent(LogLevel.Error, logMessage);
        }

        /// <summary>
        /// Write warning to log file and window event
        /// </summary>
        /// <param name="error">The warning exception</param>
        public void WriteWarning(Exception error)
        {
            var logMessage = CreateMessage(error);

            WriteText(LogLevel.Warning, logMessage);
            WriteEvent(LogLevel.Warning, logMessage);
        }

        /// <summary>
        /// Write information to log file and window event
        /// </summary>
        /// <param name="message"></param>
        public void WriteSuccess(string message)
        {
            WriteText(LogLevel.Success, message);
            WriteEvent(LogLevel.Success, message);
        }

        /// <summary>
        /// Write a message to log file
        /// </summary>
        /// <param name="level">The log level (Error/Warning/Success)</param>
        /// <param name="message">The message will be written</param>
        private void WriteText(LogLevel level, string message)
        {
            var log = new StringBuilder();
            log.Append($"{ DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss fff") } ");
            log.Append($"{ level   } ");
            log.Append($"{ message } ");

            if (File.Exists(LogFile))
            {
                File.AppendAllLines(LogFile, new string[] { log.ToString() });
            }
            else
            {
                File.WriteAllLines(LogFile, new string[] { log.ToString() });
            }
        }

        /// <summary>
        /// Write a message to window event
        /// </summary>
        /// <param name="level">The log level (Error/Warning/Success)</param>
        /// <param name="message">The message will be written</param>
        private void WriteEvent(LogLevel level, string message)
        {
            var entryType = EventLogEntryType.Information;
            switch (level)
            {
                case LogLevel.Error:
                    {
                        entryType = EventLogEntryType.Error;
                    }
                    break;
                case LogLevel.Success:
                    {
                        entryType = EventLogEntryType.Information;
                    }
                    break;
                case LogLevel.Warning:
                    {
                        entryType = EventLogEntryType.Warning;
                    }
                    break;
            }

            EventSource = string.IsNullOrEmpty(EventSource) == true
                ? AppSetting.EventSource : EventSource;
            EventLog.WriteEntry(EventSource, message, entryType);
        }

        /// <summary>
        /// Create message string from error/warning exception
        /// </summary>
        /// <param name="error">The error/warning exception</param>
        /// <returns>The log message string</returns>
        private string CreateMessage(Exception error)
        {
            var message = new StringBuilder();
            message.Append(error.Message);

            var inner = error.InnerException;
            while (inner != null)
            {
                message.Append($"{ Environment.NewLine }");
                message.Append(inner.Message);
                inner = inner.InnerException;
            }
            return message.ToString();
        }
    }
}