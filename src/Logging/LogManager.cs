using System;

namespace LEE.Logging
{
    public enum LogLevel
    {
        Error = 0,
        Warning,
        Info,
        Debug,
    }

    public static class LogManager
    {
        public static bool CanLog = true;
        public static LogLevel GlobalLevel = LogLevel.Warning;

        // TODO: Write to adapters instead of 
        internal static void LogError(string formattedMessage)
        {
            if(CanLog)
                Console.WriteLine (string.Format("ERROR {0}", formattedMessage));
        }

        internal static void LogWarning(string formattedMessage)
        {
            if(CanLog)
                Console.WriteLine (string.Format("WARN {0}", formattedMessage));
        }

        internal static void LogInfo(string formattedMessage)
        {
            if(CanLog)
                Console.WriteLine (string.Format("INFO {0}", formattedMessage));
        }

        internal static void LogDebug(string formattedMessage)
        {
            if(CanLog)
                Console.WriteLine (string.Format("DEBUG {0}", formattedMessage));
        }

        public static Logger CreateLogger(string name)
        {
            return new Logger () 
            {
                Name = name,
                Level = GlobalLevel
            };
        }
    }

    public class Logger
    {
        public string Name { get; set; }
        public LogLevel Level { get; set; }

        public bool IsErrorEnabled { get { return CanLogAt(LogLevel.Error); } }
        public bool IsWarningEnabled { get { return CanLogAt(LogLevel.Warning); } }
        public bool IsInfoEnabled { get { return CanLogAt(LogLevel.Info); } }
        public bool IsDebugEnabled { get { return CanLogAt(LogLevel.Debug); } }

        public void LogErrorFormat(string message, params object[] args)
        {
            if(IsErrorEnabled)
            {
                LogError(string.Format(message, args));
            }
        }

        public void LogError(string message)
        {
            if (IsErrorEnabled)
            {
                LogManager.LogError (string.Format("{0} -- {1}", Name, message));
            }
        }

        public void LogWarningFormat(string message, params object[] args)
        {
            if(IsWarningEnabled)
            {
                LogWarning(string.Format(message, args));
            }
        }

        public void LogWarning(string message)
        {
            if (IsWarningEnabled)
            {
                LogManager.LogWarning (string.Format("{0} -- {1}", Name, message));
            }
        }

        public void LogInfoFormat(string message, params object[] args)
        {
            if(IsInfoEnabled)
            {
                LogInfo(string.Format(message, args));
            }
        }

        public void LogInfo(string message)
        {
            if (IsInfoEnabled)
            {
                LogManager.LogInfo (string.Format("{0} -- {1}", Name, message));
            }
        }

        public void LogDebugFormat(string message, params object[] args)
        {
            if(IsDebugEnabled)
            {
                LogDebug(string.Format(message, args));
            }
        }

        public void LogDebug(string message)
        {
            if (IsDebugEnabled)
            {
                LogManager.LogDebug (string.Format("{0} -- {1}", Name, message));
            }
        }

        private bool CanLogAt(LogLevel l)
        {
            return l <= Level;
        }
    }
}

