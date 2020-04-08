namespace WFAppLogger
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnLogMessage = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(53, 13);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "&Message:";
            // 
            // btnLogMessage
            // 
            this.btnLogMessage.Location = new System.Drawing.Point(228, 65);
            this.btnLogMessage.Name = "btnLogMessage";
            this.btnLogMessage.Size = new System.Drawing.Size(87, 23);
            this.btnLogMessage.TabIndex = 2;
            this.btnLogMessage.Text = "&Log Message";
            this.btnLogMessage.UseVisualStyleBackColor = true;
            this.btnLogMessage.Click += new System.EventHandler(this.BtnLogMessage_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::WFAppLogger.Properties.Settings.Default, "DefaultLogMessage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtMessage.Location = new System.Drawing.Point(16, 30);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(515, 20);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.Text = global::WFAppLogger.Properties.Settings.Default.DefaultLogMessage;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnLogMessage;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 100);
            this.Controls.Add(this.btnLogMessage);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblMessage);
            this.Name = "Form1";
            this.Text = "WinForm Application Logger";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnLogMessage;
    }
}

