namespace MK.Tools.ForceDel
{
    using System;
    using System.Text;

    /// <summary>
    /// Log-Level of the message to be logged.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Normal output. Will suppressed only if the logger is switched to quiet.
        /// </summary>
        Normal,

        /// <summary>
        /// Verbose output (messages for detailed processing information).
        /// </summary>
        Verbose,

        /// <summary>
        /// Debug messages.
        /// </summary>
        Debug,

        /// <summary>
        /// An exception is always logged.
        /// </summary>
        Exception
    }

    /// <summary>
    /// A simple logging facility. It is not really a logger, it is just a simple
    /// wrapper around the output to the console.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// If Quiet is set to true no output will generated at all.
        /// </summary>
        public static bool Quiet { get; set; }

        /// <summary>
        /// If Verbose is set to true all messages tagged as "verbose" will be logged.
        /// </summary>
        public static bool Verbose { get; set; }

        /// <summary>
        /// If Debug is set to true the output of debug messages is enabled (and all verbose messages).
        /// </summary>
        public static bool Debug { get; set; }

        /// <summary>
        /// Writes a message.
        /// </summary>
        /// <param name="level">The log level of the message (Normal, Verbose, Debug).</param>
        /// <param name="msg">The message to be logged.</param>
        public static void Log(LogLevel level, string msg)
        {
            if (level == LogLevel.Exception)
            {
                Console.Error.WriteLine(msg);
                return;
            }

            if (Quiet)
                return;

            if (level == LogLevel.Normal)
                Console.Out.WriteLine(msg);
            else if (level == LogLevel.Verbose && (Verbose == true || Debug == true))
                Console.Out.WriteLine(msg);
            else if (level == LogLevel.Debug && Debug == true)
                Console.Out.WriteLine(msg);
        }

        /// <summary>
        /// Flushes all buffered log messages.
        /// </summary>
        public static void Flush()
        {
            Console.Out.Flush();
        }
    }
}
