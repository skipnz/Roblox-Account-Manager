using RBX_Alt_Manager.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RBX_Alt_Manager.Forms
{
    public partial class ManageServersDialog : Form
    {
        private PrivateServerManager manager;
        private List<PrivateServer> servers;

        public ManageServersDialog()
        {
            InitializeComponent();
            manager = PrivateServerManager.GetInstance();
            servers = new List<PrivateServer>(manager.GetServers());
            LoadServersIntoListView();
        }

        private void LoadServersIntoListView()
        {
            ServersListView.Items.Clear();
            foreach (var server in servers)
            {
                ListViewItem item = new ListViewItem(server.Name);
                item.SubItems.Add(server.JobId);
                item.SubItems.Add(server.DateAdded.ToString("yyyy-MM-dd HH:mm:ss"));
                item.Tag = server;
                ServersListView.Items.Add(item);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddEditServerDialog dialog = new AddEditServerDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                servers.Add(dialog.Server);
                manager.AddServer(dialog.Server);
                LoadServersIntoListView();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (ServersListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a server to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = ServersListView.SelectedIndices[0];
            PrivateServer server = servers[selectedIndex];
            AddEditServerDialog dialog = new AddEditServerDialog(server);

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                servers[selectedIndex] = dialog.Server;
                manager.UpdateServer(selectedIndex, dialog.Server);
                LoadServersIntoListView();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ServersListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a server to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = ServersListView.SelectedIndices[0];
            if (MessageBox.Show("Are you sure you want to delete this server?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                servers.RemoveAt(selectedIndex);
                manager.DeleteServer(selectedIndex);
                LoadServersIntoListView();
            }
        }
    }
}
