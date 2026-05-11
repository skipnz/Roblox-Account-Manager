using RBX_Alt_Manager.Classes;
using System;
using System.Windows.Forms;

namespace RBX_Alt_Manager.Forms
{
    public partial class AddEditServerDialog : Form
    {
        public PrivateServer Server { get; private set; }
        private bool IsEditing { get; set; }

        public AddEditServerDialog()
        {
            InitializeComponent();
            IsEditing = false;
            Text = "Add Server";
        }

        public AddEditServerDialog(PrivateServer server)
        {
            InitializeComponent();
            IsEditing = true;
            Text = "Edit Server";
            Server = server;
            ServerNameTextBox.Text = server.Name;
            JobIdTextBox.Text = server.JobId;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServerNameTextBox.Text))
            {
                MessageBox.Show("Server name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(JobIdTextBox.Text))
            {
                MessageBox.Show("Job ID cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrivateServerManager manager = PrivateServerManager.GetInstance();
            if (!manager.ValidateJobId(JobIdTextBox.Text))
            {
                MessageBox.Show("Invalid Job ID format. Please enter a valid GUID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsEditing)
            {
                Server.Name = ServerNameTextBox.Text;
                Server.JobId = JobIdTextBox.Text;
            }
            else
            {
                Server = new PrivateServer(ServerNameTextBox.Text, JobIdTextBox.Text);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
