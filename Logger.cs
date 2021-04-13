using System;

namespace SafeTest
{
    class Logger
    {
        private string name;

        public Logger(string name)
        {
            this.name = name;
        }

        private void Log(string message, LogLevel level)
        {
            Console.ForegroundColor = level.GetTextColor();
            Console.WriteLine($"[{name}/{level}] " + message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Debug(string message)
        {
            Log(message, LogLevel.DEBUG);
        }

        public void Info(string message)
        {
            Log(message, LogLevel.INFO);
        }

        public void Warning(string message)
        {
            Log(message, LogLevel.WARNING);
        }

        public void Error(string message)
        {
            Log(message, LogLevel.ERROR);
        }

    }

    enum LogLevel
    {
        DEBUG, INFO, WARNING, ERROR
    }

    static class LogLevelExtensions
    {
        public static ConsoleColor GetTextColor(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.WARNING:
                    return ConsoleColor.Yellow;
                case LogLevel.ERROR:
                    return ConsoleColor.Red;
                case LogLevel.DEBUG:
                case LogLevel.INFO: // FALLTHROUGH
                default:
                    return ConsoleColor.Gray;
            }
        }
    }
}
