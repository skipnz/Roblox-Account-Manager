# Private Server Manager Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement a global private server manager that eliminates manual JOB ID entry and prevents accidental public server joins through mandatory server selection.

**Architecture:** Three-layer approach: (1) Data layer with PrivateServer model and PrivateServerManager for persistence, (2) UI layer with ManageServersDialog for CRUD operations and dropdown selector in main form, (3) Logic layer modifications to enforce mandatory server selection during join operations.

**Tech Stack:** C# WinForms, Newtonsoft.Json for JSON persistence, ComboBox for dropdown selection, ListViewItem for server list display.

---

## File Structure

### New Files to Create
- `RBX Alt Manager/Classes/PrivateServer.cs` — Data model for individual private servers
- `RBX Alt Manager/Classes/PrivateServerManager.cs` — Load/save logic with JSON persistence
- `RBX Alt Manager/Forms/ManageServersDialog.cs` — Dialog form for server management (Code-Behind)
- `RBX Alt Manager/Forms/ManageServersDialog.Designer.cs` — Auto-generated UI designer file
- `RBX Alt Manager/Forms/AddEditServerDialog.cs` — Dialog for adding/editing individual servers (Code-Behind)
- `RBX Alt Manager/Forms/AddEditServerDialog.Designer.cs` — Auto-generated UI designer file

### Modified Files
- `RBX Alt Manager/AccountManager.cs` — Replace JobID field access with dropdown, modify join button logic
- `RBX Alt Manager/AccountManager.Designer.cs` — Remove JobID text field, add dropdown and button controls

---

## Task 1: Create PrivateServer Data Model

**Files:**
- Create: `RBX Alt Manager/Classes/PrivateServer.cs`

**Steps:**

- [ ] **Step 1: Create PrivateServer.cs with the data model**

Create file `/home/skip/Roblox-Account-Manager/RBX Alt Manager/Classes/PrivateServer.cs` with:

```csharp
using System;

namespace RBX_Alt_Manager.Classes
{
    public class PrivateServer
    {
        public string Name { get; set; }
        public string JobId { get; set; }
        public DateTime DateAdded { get; set; }

        public PrivateServer()
        {
            DateAdded = DateTime.UtcNow;
        }

        public PrivateServer(string name, string jobId)
        {
            Name = name;
            JobId = jobId;
            DateAdded = DateTime.UtcNow;
        }
    }
}
```

- [ ] **Step 2: Commit**

```bash
git add "RBX Alt Manager/Classes/PrivateServer.cs"
git commit -m "feat: add PrivateServer data model class"
```

---

## Task 2: Create PrivateServerManager for Persistence

**Files:**
- Create: `RBX Alt Manager/Classes/PrivateServerManager.cs`

**Steps:**

- [ ] **Step 1: Create PrivateServerManager.cs with load/save logic**

Create file `/home/skip/Roblox-Account-Manager/RBX Alt Manager/Classes/PrivateServerManager.cs` with:

```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RBX_Alt_Manager.Classes
{
    public class PrivateServerManager
    {
        private static PrivateServerManager Instance;
        private List<PrivateServer> servers;
        private readonly string configPath;
        private readonly string configFileName = "private_servers.json";

        private PrivateServerManager()
        {
            configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RobloxAccountManager");
            servers = new List<PrivateServer>();
            LoadServers();
        }

        public static PrivateServerManager GetInstance()
        {
            if (Instance == null)
                Instance = new PrivateServerManager();
            return Instance;
        }

        public List<PrivateServer> GetServers()
        {
            return new List<PrivateServer>(servers);
        }

        public void AddServer(PrivateServer server)
        {
            servers.Add(server);
            SaveServers();
        }

        public void UpdateServer(int index, PrivateServer server)
        {
            if (index >= 0 && index < servers.Count)
            {
                servers[index] = server;
                SaveServers();
            }
        }

        public void DeleteServer(int index)
        {
            if (index >= 0 && index < servers.Count)
            {
                servers.RemoveAt(index);
                SaveServers();
            }
        }

        public bool ValidateJobId(string jobId)
        {
            // GUID format validation
            return Guid.TryParse(jobId, out _);
        }

        private void LoadServers()
        {
            try
            {
                if (!Directory.Exists(configPath))
                    Directory.CreateDirectory(configPath);

                string filePath = Path.Combine(configPath, configFileName);
                if (!File.Exists(filePath))
                {
                    servers = new List<PrivateServer>();
                    return;
                }

                string json = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(json))
                {
                    servers = new List<PrivateServer>();
                    return;
                }

                JObject obj = JObject.Parse(json);
                servers = obj["servers"]?.ToObject<List<PrivateServer>>() ?? new List<PrivateServer>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading private servers: {ex.Message}");
                servers = new List<PrivateServer>();
            }
        }

        private void SaveServers()
        {
            try
            {
                if (!Directory.Exists(configPath))
                    Directory.CreateDirectory(configPath);

                string filePath = Path.Combine(configPath, configFileName);
                JObject obj = new JObject();
                obj["servers"] = JArray.FromObject(servers);
                string json = obj.ToString(Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving private servers: {ex.Message}");
            }
        }
    }
}
```

- [ ] **Step 2: Commit**

```bash
git add "RBX Alt Manager/Classes/PrivateServerManager.cs"
git commit -m "feat: add PrivateServerManager for persistence"
```

---

## Task 3: Create AddEditServerDialog Form

**Files:**
- Create: `RBX Alt Manager/Forms/AddEditServerDialog.cs`
- Create: `RBX Alt Manager/Forms/AddEditServerDialog.Designer.cs`

**Steps:**

- [ ] **Step 1: Create AddEditServerDialog.Designer.cs with form layout**

Create file `/home/skip/Roblox-Account-Manager/RBX Alt Manager/Forms/AddEditServerDialog.Designer.cs`:

```csharp
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
```

- [ ] **Step 2: Create AddEditServerDialog.cs with logic**

Create file `/home/skip/Roblox-Account-Manager/RBX Alt Manager/Forms/AddEditServerDialog.cs`:

```csharp
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
```

- [ ] **Step 3: Commit**

```bash
git add "RBX Alt Manager/Forms/AddEditServerDialog.cs" "RBX Alt Manager/Forms/AddEditServerDialog.Designer.cs"
git commit -m "feat: add AddEditServerDialog for server management"
```

---

## Task 4: Create ManageServersDialog Form

**Files:**
- Create: `RBX Alt Manager/Forms/ManageServersDialog.cs`
- Create: `RBX Alt Manager/Forms/ManageServersDialog.Designer.cs`

**Steps:**

- [ ] **Step 1: Create ManageServersDialog.Designer.cs**

Create file `/home/skip/Roblox-Account-Manager/RBX Alt Manager/Forms/ManageServersDialog.Designer.cs`:

```csharp
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
```

- [ ] **Step 2: Create ManageServersDialog.cs with logic**

Create file `/home/skip/Roblox-Account-Manager/RBX Alt Manager/Forms/ManageServersDialog.cs`:

```csharp
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
```

- [ ] **Step 3: Commit**

```bash
git add "RBX Alt Manager/Forms/ManageServersDialog.cs" "RBX Alt Manager/Forms/ManageServersDialog.Designer.cs"
git commit -m "feat: add ManageServersDialog for CRUD operations"
```

---

## Task 5: Modify AccountManager.Designer.cs - Replace JobID Field with Dropdown

**Files:**
- Modify: `RBX Alt Manager/AccountManager.Designer.cs`

**Steps:**

- [ ] **Step 1: Find and understand the JobID field location in AccountManager.Designer.cs**

Run:
```bash
grep -n "this.JobID" "/home/skip/Roblox-Account-Manager/RBX Alt Manager/AccountManager.Designer.cs" | head -5
```

This will show the lines where JobID is defined.

- [ ] **Step 2: Find the exact section in the designer file that defines JobID**

Run:
```bash
grep -n "JobID" "/home/skip/Roblox-Account-Manager/RBX Alt Manager/AccountManager.Designer.cs" | grep -E "Location|Size|Name"
```

- [ ] **Step 3: Read the AccountManager.Designer.cs file to see the JobID field definition**

Read the file to find the jobID control and its parent container (you'll look for something like `this.JobID = new System.Windows.Forms.TextBox()`)

- [ ] **Step 4: Replace the JobID TextBox with a ComboBox and add a Manage Servers button**

This step will:
- Remove the `private System.Windows.Forms.TextBox JobID;` declaration
- Add `private System.Windows.Forms.ComboBox PrivateServerSelector;` and `private System.Windows.Forms.Button ManageServersButton;`
- Replace the JobID initialization code with ComboBox initialization
- Add the button initialization
- Update the layout to position the dropdown and button

The exact changes depend on the current layout. You will:
1. Find the line declaring `this.JobID = new System.Windows.Forms.TextBox();`
2. Change it to `this.PrivateServerSelector = new System.Windows.Forms.ComboBox();`
3. Update the JobID.Location/Size properties to become PrivateServerSelector.Location/Size
4. Add ManageServersButton initialization below it
5. Remove the old field declaration at the top and add the new ones

Expected pattern (find this exact section):
```csharp
// JobID
this.JobID.Location = new System.Drawing.Point(X, Y);
this.JobID.Name = "JobID";
this.JobID.Size = new System.Drawing.Size(W, H);
this.JobID.TabIndex = N;
```

Replace with:
```csharp
// PrivateServerSelector
this.PrivateServerSelector.Location = new System.Drawing.Point(X, Y);
this.PrivateServerSelector.Name = "PrivateServerSelector";
this.PrivateServerSelector.Size = new System.Drawing.Size(W-85, H);
this.PrivateServerSelector.TabIndex = N;
this.PrivateServerSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
this.PrivateServerSelector.SelectedIndexChanged += new System.EventHandler(this.PrivateServerSelector_SelectedIndexChanged);

// ManageServersButton
this.ManageServersButton.Location = new System.Drawing.Point(X + W - 80, Y);
this.ManageServersButton.Name = "ManageServersButton";
this.ManageServersButton.Size = new System.Drawing.Size(75, H);
this.ManageServersButton.TabIndex = N + 1;
this.ManageServersButton.Text = "Manage";
this.ManageServersButton.UseVisualStyleBackColor = true;
this.ManageServersButton.Click += new System.EventHandler(this.ManageServersButton_Click);
```

And add these field declarations at the top of the file with other field declarations:
```csharp
private System.Windows.Forms.ComboBox PrivateServerSelector;
private System.Windows.Forms.Button ManageServersButton;
```

Also remove:
```csharp
private System.Windows.Forms.TextBox JobID;
```

And in the controls collection, change:
```csharp
this.Controls.Add(this.JobID);
```

To:
```csharp
this.Controls.Add(this.PrivateServerSelector);
this.Controls.Add(this.ManageServersButton);
```

- [ ] **Step 5: Commit**

```bash
git add "RBX Alt Manager/AccountManager.Designer.cs"
git commit -m "feat: replace JobID field with PrivateServerSelector dropdown and Manage button"
```

---

## Task 6: Modify AccountManager.cs - Update Properties and Join Logic

**Files:**
- Modify: `RBX Alt Manager/AccountManager.cs`

**Steps:**

- [ ] **Step 1: Update CurrentJobId property**

Find the line:
```csharp
public static string CurrentJobId { get => Instance.JobID.Text; }
```

Replace with:
```csharp
public static string CurrentJobId 
{ 
    get 
    { 
        if (Instance.PrivateServerSelector.SelectedItem is ComboBoxItem item)
            return item.Value;
        return "";
    } 
}
```

And add a helper class right after the AccountManager class definition (before the closing brace):

```csharp
public class ComboBoxItem
{
    public string Label { get; set; }
    public string Value { get; set; }

    public ComboBoxItem(string label, string value)
    {
        Label = label;
        Value = value;
    }

    public override string ToString()
    {
        return Label;
    }
}
```

- [ ] **Step 2: Add the PrivateServerSelector_SelectedIndexChanged event handler**

Find where other event handlers are defined in AccountManager.cs. Add this method:

```csharp
private void PrivateServerSelector_SelectedIndexChanged(object sender, EventArgs e)
{
    if (PrivateServerSelector.SelectedItem is ComboBoxItem item && !string.IsNullOrEmpty(item.Value))
    {
        JoinServer.Enabled = true;
    }
    else
    {
        JoinServer.Enabled = false;
    }
}
```

- [ ] **Step 3: Add the ManageServersButton_Click event handler**

Add this method near the PrivateServerSelector_SelectedIndexChanged handler:

```csharp
private void ManageServersButton_Click(object sender, EventArgs e)
{
    ManageServersDialog dialog = new ManageServersDialog();
    if (dialog.ShowDialog(this) == DialogResult.OK)
    {
        RefreshPrivateServerSelector();
    }
}
```

- [ ] **Step 4: Add RefreshPrivateServerSelector helper method**

Add this method:

```csharp
private void RefreshPrivateServerSelector()
{
    PrivateServerSelector.Items.Clear();
    PrivateServerManager manager = PrivateServerManager.GetInstance();
    foreach (var server in manager.GetServers())
    {
        PrivateServerSelector.Items.Add(new ComboBoxItem(server.Name, server.JobId));
    }
    PrivateServerSelector.SelectedIndex = -1;
    JoinServer.Enabled = false;
}
```

- [ ] **Step 5: Initialize the dropdown on form load**

Find the form initialization code (in the constructor or Form_Load). Add this call to populate the dropdown:

```csharp
RefreshPrivateServerSelector();
```

Look for where `InitializeComponent()` is called, and add the RefreshPrivateServerSelector() call after it.

- [ ] **Step 6: Update any existing code that sets JobID.Text**

Find these lines and remove them or comment them out (they were setting JobID from saved data):

```csharp
if (!string.IsNullOrEmpty(SelectedAccount.GetField("SavedJobId"))) JobID.Text = SelectedAccount.GetField("SavedJobId");
```

And:

```csharp
account.SetField("SavedJobId", JobID.Text);
```

Also remove or comment out:

```csharp
JobID.Text = PlaceID.Text;
```

And anywhere else JobID.Text is being directly set.

- [ ] **Step 7: Add using statement for ManageServersDialog and PrivateServerManager**

Add these at the top of AccountManager.cs if not already present:

```csharp
using RBX_Alt_Manager.Forms;
```

(The Forms namespace should already be available since other dialogs are used)

- [ ] **Step 8: Commit**

```bash
git add "RBX Alt Manager/AccountManager.cs"
git commit -m "feat: update AccountManager to use private server selector dropdown"
```

---

## Task 7: Build and Test the Application

**Files:**
- No files created/modified (build step)

**Steps:**

- [ ] **Step 1: Build the solution**

Run:
```bash
cd "/home/skip/Roblox-Account-Manager"
dotnet build "RBX Alt Manager.sln" -c Release
```

Expected output: Build succeeds with no errors (warnings are okay).

- [ ] **Step 2: Run the application**

Navigate to the build output directory and run the app:

```bash
cd "/home/skip/Roblox-Account-Manager/RBX Alt Manager/bin/Release"
./AccountManager.exe
```

Or from Visual Studio, press F5 to launch with debugger.

- [ ] **Step 3: Test adding a private server**

1. Click "Manage" button next to the private server dropdown
2. Click "Add" button
3. Enter:
   - Server Name: "Test Server"
   - Job ID: "00000000-0000-0000-0000-000000000000"
4. Click OK, verify it appears in the list
5. Click OK to close the dialog
6. Verify the dropdown now shows "Test Server"

- [ ] **Step 4: Test server selection and join button**

1. Click the dropdown, select "Test Server"
2. Verify the Join Server button becomes enabled
3. Click the dropdown again, select empty/no selection
4. Verify the Join Server button becomes disabled

- [ ] **Step 5: Test persistence**

1. Close the application completely
2. Reopen it
3. Verify "Test Server" still appears in the dropdown
4. Verify the private_servers.json file exists in `%AppData%/RobloxAccountManager/`

- [ ] **Step 6: Test editing and deleting servers**

1. Click "Manage" button
2. Select "Test Server"
3. Click "Edit", change the name to "Test Server Updated"
4. Click OK, verify the list shows the updated name
5. Close and verify the dropdown reflects the change
6. Click "Manage" again, select the server, click "Delete"
7. Click Yes to confirm, verify it's removed
8. Click OK to close the dialog

- [ ] **Step 7: Test invalid Job ID validation**

1. Click "Manage" button
2. Click "Add"
3. Enter Server Name: "Invalid Server"
4. Enter Job ID: "not-a-guid"
5. Click OK
6. Verify error message appears: "Invalid Job ID format. Please enter a valid GUID."
7. Close the error dialog

- [ ] **Step 8: Commit test results**

```bash
git add -A
git commit -m "feat: complete private server manager implementation

- Added data model and persistence layer
- Created ManageServersDialog with CRUD operations
- Added dropdown selector to main form
- Enforced mandatory server selection
- Tested all features working as expected"
```

---

## Testing Checklist

All of these tests should pass before marking the implementation complete:

- [ ] Add a private server, verify it appears in dropdown
- [ ] Edit a server name, verify dropdown updates
- [ ] Delete a server, verify it's removed from dropdown
- [ ] Select a server, verify Join button is enabled
- [ ] Deselect server (empty selection), verify Join button is disabled
- [ ] Attempt to click Join without selecting server (button should be disabled)
- [ ] Close and restart app, verify servers persist
- [ ] Test with invalid JOB ID format, verify error message
- [ ] Test with duplicate server names, verify both can coexist
- [ ] Test with multiple servers in list, verify all are selectable
- [ ] Test Server Name field cannot be empty - shows error
- [ ] Test Job ID field cannot be empty - shows error
- [ ] Verify the selected server's Job ID is used when joining a game
