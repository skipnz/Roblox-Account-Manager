namespace RBX_Alt_Manager.Forms
{
    partial class AddEditServerDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.ServerNameTextBox = new System.Windows.Forms.TextBox();
            this.JobIdLabel = new System.Windows.Forms.Label();
            this.JobIdTextBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ServerNameLabel
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Location = new System.Drawing.Point(12, 15);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(82, 13);
            this.ServerNameLabel.TabIndex = 0;
            this.ServerNameLabel.Text = "Server Name:";

            // ServerNameTextBox
            this.ServerNameTextBox.Location = new System.Drawing.Point(12, 31);
            this.ServerNameTextBox.Name = "ServerNameTextBox";
            this.ServerNameTextBox.Size = new System.Drawing.Size(360, 20);
            this.ServerNameTextBox.TabIndex = 1;

            // JobIdLabel
            this.JobIdLabel.AutoSize = true;
            this.JobIdLabel.Location = new System.Drawing.Point(12, 60);
            this.JobIdLabel.Name = "JobIdLabel";
            this.JobIdLabel.Size = new System.Drawing.Size(49, 13);
            this.JobIdLabel.TabIndex = 2;
            this.JobIdLabel.Text = "Job ID:";

            // JobIdTextBox
            this.JobIdTextBox.Location = new System.Drawing.Point(12, 76);
            this.JobIdTextBox.Name = "JobIdTextBox";
            this.JobIdTextBox.Size = new System.Drawing.Size(360, 20);
            this.JobIdTextBox.TabIndex = 3;
            this.JobIdTextBox.Font = new System.Drawing.Font("Courier New", 9F);

            // OkButton
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(216, 110);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);

            // CancelButton
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(297, 110);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;

            // AddEditServerDialog
            this.AcceptButton = this.OkButton;
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(384, 145);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.JobIdTextBox);
            this.Controls.Add(this.JobIdLabel);
            this.Controls.Add(this.ServerNameTextBox);
            this.Controls.Add(this.ServerNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditServerDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Server";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label ServerNameLabel;
        private System.Windows.Forms.TextBox ServerNameTextBox;
        private System.Windows.Forms.Label JobIdLabel;
        private System.Windows.Forms.TextBox JobIdTextBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
