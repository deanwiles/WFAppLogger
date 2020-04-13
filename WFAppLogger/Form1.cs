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
            logger.LogTrace("Entering {Method}()", "ProjectForm");
            // Run designer code
            InitializeComponent();
            logger.LogTrace("Exiting {Method}()", "ProjectForm");
        }

        private void BtnLogMessage_Click(object sender, EventArgs e)
        {
            logger.LogTrace("Entering {Method}()", "BtnLogMessage_Click");
            // Conditionally log user message (if Debug set)
            string message = txtMessage.Text;
            logger.LogDebug("Message='{Message}'", message);
            // Save last user message (bound to Settings.Default.DefaultLogMessage) in user.config
            Settings.Default.Save();
            logger.LogTrace("Exiting {Method}()", "BtnLogMessage_Click");
        }
    }
}
