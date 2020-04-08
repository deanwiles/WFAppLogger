using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;
using WFAppLogger.Properties;

namespace WFAppLogger
{
    internal class Program
    {
        // Global Service Collection for dependency injection
        static IServiceCollection services;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize application logging with dependency injection using settings in App.config
            services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder
                    .AddFilter("Microsoft", Settings.Default.LogLevelMicrosoft)
                    .AddFilter("System", Settings.Default.LogLevelSystem)
                    .AddFilter("WFAppLogger", Settings.Default.LogLevelWFAppLogger)
                    .AddConsole();
            });

            // Create logger for Program class and log that we're starting up
            var logger = CreateLogger<Program>();
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
            using (ServiceProvider sp = services.BuildServiceProvider())
            { 
                return sp.GetRequiredService<ILoggerFactory>().CreateLogger<T>();
            }
        }
    }
}
