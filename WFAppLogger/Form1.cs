using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace WFAppLogger
{
    public partial class Form1 : Form
    {
        private readonly ILogger logger;
        public Form1(ILogger Logger)
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
            // Log user message
            string message = txtMessage.Text;
            logger.LogInformation("Message='{Message}'", message);
            logger.LogTrace("Exiting {Method}()", "BtnLogMessage_Click");
        }
    }
}
