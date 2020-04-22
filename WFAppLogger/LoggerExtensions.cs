using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WFAppLogger
{
    //
    // Summary:
    //     ILogger extension methods for common scenarios
    public static class LoggerExtensions
    {
        /// <summary>
        /// Combine method name and argulment list into a single object array
        /// </summary>
        /// <param name="methodName">Calling method</param>
        /// <param name="args">An object array that contains zero or more objects</param>
        /// <returns>An object array that contains the calling method and any argument objects</returns>
        public static object[] CombineParams(string methodName, params object[] args)
        {
            var list = new List<object>() { methodName };
            foreach (var arg in args)
                list.Add(arg);
            return list.ToArray();
        }

        /// <summary>
        /// Formats and writes a trace log method entry message
        /// </summary>
        /// <param name="logger">The Microsoft.Extensions.Logging.ILogger to write to</param>
        /// <param name="message">Format string of the optional entry parameter message in message template format. Example: "User={User}, Address={Address}"</param>
        /// <param name="methodName">Calling method (will be derived at compile time)</param>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        public static void TraceEntry(this ILogger logger, string message = null, [CallerMemberName]string methodName = null, params object[] args)
        {
            logger.LogTrace($"Entering {{Method}}({message})", CombineParams(methodName, args));
        }

        /// <summary>
        /// Formats and writes a trace log method exit message
        /// </summary>
        /// <param name="logger">The Microsoft.Extensions.Logging.ILogger to write to</param>
        /// <param name="message">Format string of the optional return parameter message in message template format. Example: "User={User}, Address={Address}"</param>
        /// <param name="methodName">Calling method (will be derived at compile time)</param>
        /// <param name="args">An object array that contains zero or more objects to format</param>
        public static void TraceExit(this ILogger logger, string message = null, [CallerMemberName]string methodName = null, params object[] args)
        {
            logger.LogTrace($"Exiting {{Method}}({message})", CombineParams(methodName, args));
        }
    }
}
