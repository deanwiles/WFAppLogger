using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Windows.Forms;
using WFAppLogger.Properties;

namespace WFAppLogger
{
    internal class Program
    {
        // Global Service Provider for dependency injection
        static ServiceProvider serviceProvider;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // FYI: the roaming configuration that applies to the current user is at
            // %LOCALAPPDATA%\\<Company Name>\<appdomainname>_<eid>_<hash>\<version>\user.config
            //string userConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(
            //    System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            // If no user.config file exists, then the application.exe.config file is used
            //string appConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(
            //    System.Configuration.ConfigurationUserLevel.None).FilePath;

            // Get logging configuration from settings in app.config (or user.config)
            var loggingConfig = Settings.Default.LoggingConfig;
            var stream = new MemoryStream();
            loggingConfig.Save(stream);
            stream.Position = 0;
            var configuration = new ConfigurationBuilder()
                .AddXmlStream(stream)
                .Build();
            var config = configuration.GetSection("Logging");

            // Initialize application logging via dependency injection
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(config);

                builder.AddConsole();

                // Utilize Karambolo.Extensions.Logging.File from https://github.com/adams85/filelogger
                builder.AddFile<CustomFileLoggerProvider>(configure: o => o.RootPath = Path.GetTempPath());
            });
            serviceProvider = services.BuildServiceProvider();

            // Create logger for Program class and log that we're starting up
            var logger = CreateLogger<Program>();
            logger.LogInformation($"Starting {Application.ProductName}...");
            logger.LogTrace("This is a trace message.");
            logger.LogDebug("This is a debug message.");
            logger.LogInformation("This is an info message.");
            logger.LogWarning("This is a warning message.");
            logger.LogError("This is an error message.");
            logger.LogCritical("This is a critical message.");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the application
            Application.Run(new Form1(CreateLogger<Form1>()));

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
            // Create and return Logger instance for the given type using global dependency injection for logger factory
            return serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<T>();
        }
    }
}
