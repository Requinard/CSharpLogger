using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ApplicationLogger
{
    public delegate void ItemAdded(LogItem item);

    public static class Logger
    {
        private static List<LogItem> log;

        public static List<LogItem> Log
        {
            get { return log; }
        }

        public static event ItemAdded OnNewLogItem;
        public static event ItemAdded OnDebugLog;
        public static event ItemAdded OnInfoLog;
        public static event ItemAdded OnSuccessLog;
        public static event ItemAdded OnWarningLog;
        public static event ItemAdded OnErrorLog;

        /// <summary>
        ///     Initializes a logger
        /// </summary>
        public static void Initialize(string LogFileName)
        {
            if (File.Exists(LogFileName))
            {
                Stream s = new FileStream(LogFileName, FileMode.Open);
                var bf = new BinaryFormatter();

                log = (List<LogItem>) bf.Deserialize(s);
                s.Close();
            }
            else
            {
                throw new SerializationException("The file does not exist!");
            }
        }

        public static void Initialize()
        {
            log = new List<LogItem>();
        }

        /// <summary>
        ///     Sorts the log by a specific log Level
        /// </summary>
        /// <param name="level">Level you want sorted</param>
        /// <returns>A list containing all items with the specified log level</returns>
        public static List<LogItem> SortLogItemsByLogLevel(DateTime startDate, DateTime? endDate = null, LogLevel level= (LogLevel)1)
        {
            if (endDate == null)
                endDate = DateTime.MaxValue;

            if ((int) level == 1)
            {
                var s = from logItem in log
                    where logItem.DateTime > startDate && logItem.DateTime < endDate
                    select logItem;

                return s.ToList();
            }
            else
            {
               var s = from logItem in log
                    where logItem.DateTime > startDate && logItem.DateTime < endDate && logItem.Level == level
                    select logItem;

                return s.ToList();
            }
        }
        
        /// <summary>
        ///     Writes log to hard drive
        /// </summary>
        public static void Destruct(string LogFileName)
        {
            Stream s;
            if (File.Exists(LogFileName))
            {
                s = new FileStream(LogFileName, FileMode.Truncate);
            }
            else
            {
                s = new FileStream(LogFileName, FileMode.CreateNew);
            }
            var bf = new BinaryFormatter();

            bf.Serialize(s, log);

            s.Close();
        }

        /// <summary>
        ///     Adds a message to the message log
        /// </summary>
        /// <param name="logMessage">Message that needs to be logged</param>
        /// <param name="level">Severity of the message</param>
        public static LogItem LogMessage(string logMessage, LogLevel level)
        {
            var item = new LogItem(logMessage, level);

            log.Add(item);

            Console.WriteLine(item.ToString());
            if(OnNewLogItem != null)
                OnNewLogItem(item);

            return item;
        }

        /// <summary>
        ///     Logs an info message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Info(string message)
        {
            LogItem item = LogMessage(message, LogLevel.Info);

            if (OnInfoLog != null)
                OnInfoLog(item);
        }

        /// <summary>
        ///     Logs a debug message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Debug(string message)
        {
            LogItem item = LogMessage(message, LogLevel.Debug);

            if (OnDebugLog != null)
                OnDebugLog(item);
        }

        /// <summary>
        ///     Logs a success message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Success(string message)
        {
            LogItem item = LogMessage(message, LogLevel.Success);

            if (OnSuccessLog != null)
                OnSuccessLog(item);
        }

        /// <summary>
        ///     Logs a warning message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Warning(string message)
        {
            LogItem item = LogMessage(message, LogLevel.Warning);

            if (OnWarningLog != null)
                OnWarningLog(item);
        }

        /// <summary>
        ///     Logs an error message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Error(string message)
        {
            LogItem item = LogMessage(message, LogLevel.Error);

            if (OnErrorLog != null)
                OnErrorLog(item);
        }
    }

    [Serializable]
    public enum LogLevel
    {
        Debug = 10,
        Info = 20,
        Success = 30,
        Warning = 40,
        Error = 50,
    }

    [Serializable]
    public class LogItem
    {
        private readonly LogLevel level;
        private readonly string message;
        private readonly DateTime dateTime;

        /// <summary>
        ///     Initializes a log item
        /// </summary>
        /// <param name="message">Message to be saved</param>
        /// <param name="level">Severity of the message</param>
        public LogItem(string message, LogLevel level)
        {
            dateTime = DateTime.Now;
            this.message = message;
            this.level = level;
        }

        public DateTime DateTime
        {
            get { return dateTime; }
        }

        public string Message
        {
            get { return message; }
        }

        public LogLevel Level
        {
            get { return level; }
        }

        /// <summary>
        ///     Returns a string of the logmessage
        /// </summary>
        /// <returns>String that contains a formatted log</returns>
        public override string ToString()
        {
            const string LogString = "[{0}] {1}: {2}";

            return String.Format(LogString, level, DateTime, Message);
        }
    }
}