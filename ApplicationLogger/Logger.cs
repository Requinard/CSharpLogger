using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static List<LogItem> SortLogItemsByLogLevel(LogLevel level)
        {
            IOrderedEnumerable<LogItem> s = from logItem in log
                where logItem.Level == level
                orderby logItem.Time descending
                select logItem;

            return s.ToList();
        }

        public static List<LogItem> SortLogItemsFromDate(DateTime startDate)
        {
            IOrderedEnumerable<LogItem> s = from logItem in log
                where logItem.Time > startDate
                orderby logItem.Time descending
                select logItem;

            return s.ToList();
        }

        public static List<LogItem> SortLogItemsToDate(DateTime toDate)
        {
            IOrderedEnumerable<LogItem> s = from logItem in log
                where logItem.Time < toDate
                orderby logItem.Time descending
                select logItem;

            return s.ToList();
        }

        public static List<LogItem> SortLogItemsBetweenDate(DateTime startDate, DateTime endDate)
        {
            IOrderedEnumerable<LogItem> s = from logItem in log
                where logItem.Time > startDate
                where logItem.Time < endDate
                orderby logItem.Time descending
                select logItem;

            return s.ToList();
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
        public static void LogMessage(string logMessage, LogLevel level)
        {
            var item = new LogItem(logMessage, level);

            log.Add(item);

            Console.WriteLine(item.ToString());

            OnNewLogItem(item);
        }

        /// <summary>
        ///     Logs an info message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Info(string message)
        {
            LogMessage(message, LogLevel.Info);
        }

        /// <summary>
        ///     Logs a debug message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Debug(string message)
        {
            LogMessage(message, LogLevel.Debug);
        }

        /// <summary>
        ///     Logs a success message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Success(string message)
        {
            LogMessage(message, LogLevel.Success);
        }

        /// <summary>
        ///     Logs a warning message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Warning(string message)
        {
            LogMessage(message, LogLevel.Warning);
        }

        /// <summary>
        ///     Logs an error message
        /// </summary>
        /// <param name="message">Message to be logged</param>
        public static void Error(string message)
        {
            LogMessage(message, LogLevel.Error);
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
        private readonly DateTime time;

        /// <summary>
        ///     Initializes a log item
        /// </summary>
        /// <param name="message">Message to be saved</param>
        /// <param name="level">Severity of the message</param>
        public LogItem(string message, LogLevel level)
        {
            time = DateTime.Now;
            this.message = message;
            this.level = level;
        }

        public DateTime Time
        {
            get { return time; }
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

            return String.Format(LogString, level, Time, Message);
        }
    }
}