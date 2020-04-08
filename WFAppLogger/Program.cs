using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;
using WFAppLogger.Properties;

namespace WFAppLogger
{
    class Program
    {
        // Global Logger factory
        static ILoggerFactory loggerFactory;
        // Logger instance for logging from global exception handlers
        static ILogger logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize application logging using settins in App.config
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", Settings.Default.LogLevelMicrosoft)
                    .AddFilter("System", Settings.Default.LogLevelSystem)
                    .AddFilter("WFAppLogger", Settings.Default.LogLevelWFAppLogger)
                    .AddConsole();
            });
            logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation($"Starting {Application.ProductName}...");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the application
            Application.Run(new Form1());

            // Log that we're exiting
            logger.LogInformation($"Exiting {Application.ProductName}.");
        }

        /// <summary>
        /// Creates a new Microsoft.Extensions.Logging.ILogger instance using the full name of the given type
        /// </summary>
        /// <typeparam name="T">The class type to create a logger for</typeparam>
        /// <returns>The Microsoft.Extensions.Logging.ILogger that was created</returns>
        public static ILogger<T> CreateLogger<T>()
        {
            // Create and return Logger instance for the given type using global logger factory
            return loggerFactory.CreateLogger<T>();
        }
    }
}
