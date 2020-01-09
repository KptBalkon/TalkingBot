namespace ExpertSystem
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
            this.TopicLabel = new System.Windows.Forms.Label();
            this.CurrentTopic = new System.Windows.Forms.Label();
            this.DialogBox = new System.Windows.Forms.TextBox();
            this.DialogWindow = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // TopicLabel
            // 
            this.TopicLabel.AutoSize = true;
            this.TopicLabel.Location = new System.Drawing.Point(12, 9);
            this.TopicLabel.Name = "TopicLabel";
            this.TopicLabel.Size = new System.Drawing.Size(37, 13);
            this.TopicLabel.TabIndex = 0;
            this.TopicLabel.Text = "Topic:";
            // 
            // CurrentTopic
            // 
            this.CurrentTopic.AutoSize = true;
            this.CurrentTopic.Location = new System.Drawing.Point(55, 9);
            this.CurrentTopic.Name = "CurrentTopic";
            this.CurrentTopic.Size = new System.Drawing.Size(31, 13);
            this.CurrentTopic.TabIndex = 1;
            this.CurrentTopic.Text = "none";
            // 
            // DialogBox
            // 
            this.DialogBox.Location = new System.Drawing.Point(15, 437);
            this.DialogBox.Name = "DialogBox";
            this.DialogBox.Size = new System.Drawing.Size(367, 20);
            this.DialogBox.TabIndex = 2;
            this.DialogBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DialogBox_KeyPress);
            // 
            // DialogWindow
            // 
            this.DialogWindow.FormattingEnabled = true;
            this.DialogWindow.Location = new System.Drawing.Point(15, 36);
            this.DialogWindow.Name = "DialogWindow";
            this.DialogWindow.Size = new System.Drawing.Size(367, 381);
            this.DialogWindow.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 471);
            this.Controls.Add(this.DialogWindow);
            this.Controls.Add(this.DialogBox);
            this.Controls.Add(this.CurrentTopic);
            this.Controls.Add(this.TopicLabel);
            this.Name = "Form1";
            this.Text = "Rozmowa";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TopicLabel;
        private System.Windows.Forms.Label CurrentTopic;
        private System.Windows.Forms.TextBox DialogBox;
        private System.Windows.Forms.ListBox DialogWindow;
    }
}

