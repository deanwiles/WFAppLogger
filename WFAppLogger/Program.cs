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
        // Global Service Collection for dependency injection
        //static IServiceCollection services;
        // Global Logger factory
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
            //services = new ServiceCollection();
            //services.AddLogging(builder =>
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConfiguration(config);

                builder.AddConsole();

                builder.AddFile(o => o.RootPath = AppContext.BaseDirectory);
            });

            //using (ServiceProvider sp = services.BuildServiceProvider())
            var logger = CreateLogger<Program>();
            {
                // create logger
                //ILogger<Program> logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

                logger.LogTrace("This is a trace message. Should be discarded.");
                logger.LogDebug("This is a debug message. Should be discarded.");
                logger.LogInformation("This is an info message. Should go into 'info.log' only.");
                logger.LogWarning("This is a warning message. Should go into 'warn+err.log' only.");
                logger.LogError("This is an error message. Should go into 'warn+err.log' only.");
                logger.LogCritical("This is a critical message. Should go into 'warn+err.log' only.");
            }

            // Create logger for Program class and log that we're starting up
            //var logger = CreateLogger<Program>();
            logger.LogInformation($"Starting {Application.ProductName}...");

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
#if (false)
            using (ServiceProvider sp = services.BuildServiceProvider())
            {
                return sp.GetRequiredService<ILoggerFactory>().CreateLogger<T>();
            }
#else
            //ServiceProvider sp = services.BuildServiceProvider();
            //return sp.GetRequiredService<ILoggerFactory>().CreateLogger<T>();
#endif
            // Create and return Logger instance for the given type using global logger factory
            return loggerFactory.CreateLogger<T>();
        }
    }
}
