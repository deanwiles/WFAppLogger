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
        // Global Logger Factory
        static ILoggerFactory loggerFactory;

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
                // Utilize Microsoft ConsoleLogger
                builder.AddConsole();
                // Utilize Karambolo.Extensions.Logging.File from https://github.com/adams85/filelogger
                builder.AddFile<CustomFileLoggerProvider>(configure: o => o.RootPath = Path.GetTempPath());
            });
            // Add WinForm(s) that will be created through service provider
            services.AddTransient<Form1>();

            // Build application objects in the context of global service provider
            using (serviceProvider = services.BuildServiceProvider())
            {
                // Create global logger factory
                loggerFactory = GetRequiredService<ILoggerFactory>();

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
                var form = GetRequiredService<Form1>();
                Application.Run(form);

                // Log that we're exiting
                logger.LogInformation($"Exiting {Application.ProductName}.");
            }
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

        /// <summary>
        /// Get service of type T from the System.IServiceProvider
        /// </summary>
        /// <typeparam name="T">The type of service object to get</typeparam>
        /// <returns>A service object of type T</returns>
        /// <exception cref="System.InvalidOperationException">There is no service of type T</exception>
        public static T GetRequiredService<T>()
        {
            // Create and return class instance for the given type using global dependency injection
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
