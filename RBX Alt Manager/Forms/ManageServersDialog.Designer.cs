namespace RBX_Alt_Manager.Forms
{
    partial class ManageServersDialog
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
            this.ServersListView = new System.Windows.Forms.ListView();
            this.NameColumn = new System.Windows.Forms.ColumnHeader();
            this.JobIdColumn = new System.Windows.Forms.ColumnHeader();
            this.DateAddedColumn = new System.Windows.Forms.ColumnHeader();
            this.AddButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ServersListView
            this.ServersListView.Columns.AddRange(new[] { this.NameColumn, this.JobIdColumn, this.DateAddedColumn });
            this.ServersListView.FullRowSelect = true;
            this.ServersListView.Location = new System.Drawing.Point(12, 12);
            this.ServersListView.Name = "ServersListView";
            this.ServersListView.Size = new System.Drawing.Size(560, 300);
            this.ServersListView.TabIndex = 0;
            this.ServersListView.UseCompatibleStateImageBehavior = false;
            this.ServersListView.View = System.Windows.Forms.View.Details;

            // NameColumn
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 150;

            // JobIdColumn
            this.JobIdColumn.Text = "Job ID";
            this.JobIdColumn.Width = 250;

            // DateAddedColumn
            this.DateAddedColumn.Text = "Date Added";
            this.DateAddedColumn.Width = 150;

            // AddButton
            this.AddButton.Location = new System.Drawing.Point(12, 318);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);

            // EditButton
            this.EditButton.Location = new System.Drawing.Point(93, 318);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(75, 23);
            this.EditButton.TabIndex = 2;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);

            // DeleteButton
            this.DeleteButton.Location = new System.Drawing.Point(174, 318);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);

            // OkButton
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(416, 318);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;

            // CancelButton
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(497, 318);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;

            // ManageServersDialog
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(584, 353);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.ServersListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageServersDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Private Servers";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListView ServersListView;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader JobIdColumn;
        private System.Windows.Forms.ColumnHeader DateAddedColumn;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
