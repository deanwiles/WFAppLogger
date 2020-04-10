using System;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Karambolo.Extensions.Logging.File;

namespace WFAppLogger
{
    /// <summary>
    /// Custom File Logger Processor
    /// </summary>
    /// <remarks>
    /// <para>This class leverages the lightweight <see cref="FileLogger"/> from https://github.com/adams85/filelogger
    /// and provides custom log file Path settings:</para>
    /// <para>&lt;appname&gt;: Application Name (e.g. WFAppLogger)</para>
    /// <para>&lt;startdate&gt;: The <see cref="DateTime"/> that file logging was started. 
    /// It can include a standard .NET format string to be passed to <see cref="DateTime.ToString"/>.</para>
    /// </remarks>
    /// <example>
    /// <para>"&lt;appname&gt;-&lt;startdate:yyyyMMdd-HHmmss&gt;-&lt;counter:000&gt;.log" will create a log file such as "WFAppLogger-20200410-105108-001.log"
    /// </example>
    public class CustomFileLoggerProcessor : FileLoggerProcessor
    {
        private static readonly string s_appName = Assembly.GetEntryAssembly().GetName().Name;
        private static readonly DateTime s_startDate = DateTime.Now;

        public CustomFileLoggerProcessor(FileLoggerContext context) : base(context) { }

        // offsets the counter by 1 -> the counter will start at 1
        protected override string GetCounter(string inlineFormat, LogFileInfo logFile, FileLogEntry entry) =>
            (logFile.Counter + 1).ToString(inlineFormat ?? logFile.CounterFormat, CultureInfo.InvariantCulture);

        // returns formatted datetime of when logging started
        protected virtual string GetStartDate(string inlineFormat, LogFileInfo logFile, FileLogEntry entry) =>
            s_startDate.ToString(inlineFormat ?? logFile.DateFormat, CultureInfo.InvariantCulture);

        // adds support for the custom path variable '<appname>' and '<startdate>'
        protected override string FormatFilePath(LogFileInfo logFile, FileLogEntry entry)
        {
            return Regex.Replace(base.FormatFilePath(logFile, entry), @"<(appname|startdate)(?::([^<>]+))?>", match =>
            {
                var inlineFormat = match.Groups[2].Value;

                switch (match.Groups[1].Value)
                {
                    case "appname": return s_appName;
                    case "startdate": return GetStartDate(inlineFormat.Length > 0 ? inlineFormat : null, logFile, entry);
                    default: throw new InvalidOperationException();
                }
            });
        }
    }
}
