using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;
using WFAppLogger.Properties;

namespace WFAppLogger
{
    public partial class Form1 : Form
    {
        private readonly ILogger logger;
        public Form1(ILogger<Form1> Logger)
        {
            // Save Logger for our class
            logger = Logger;
            logger.TraceEntry("Logger={Logger}", nameof(Form1), Logger);    // Specify class constructor name instead of defaulting to ".ctor"
            // Run designer code
            InitializeComponent();
            logger.TraceExit(methodName: nameof(Form1));
        }

        private void LogMessage(string Message)
        {
            logger.TraceEntry("Message='{Message}'", args: Message);    // Method name automatically derived & logged; just supply argument
            // Conditionally log Message (if Debug set)
            logger.LogDebug(Message);
            logger.TraceExit();
        }

        private void BtnLogMessage_Click(object sender, EventArgs e)
        {
            logger.TraceEntry("sender={sender}, e={e}", args: new object[] { sender, e });  // Encapsulate arguments when more than one
            // Conditionally log user message (if Debug set)
            string message = txtMessage.Text;
            LogMessage(message);
            // Save last user message (bound to Settings.Default.DefaultLogMessage) in user.config
            Settings.Default.Save();
            logger.TraceExit();
        }
    }
}
