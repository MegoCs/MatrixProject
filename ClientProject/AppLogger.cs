using System;

namespace ClientProject
{
    internal static class AppLogger
    {
        public static void Info(string step, string message) => Write("INFO", step, message);

        public static void Warning(string step, string message) => Write("WARN", step, message);

        public static void Error(string step, string message) => Write("ERROR", step, message);

        private static void Write(string level, string step, string message)
        {
            Console.WriteLine($"{DateTime.UtcNow:O} [{level}] [{step}] {message}");
        }
    }
}
