# Private Server Manager with Dropdown Selector

**Date:** 2026-05-11  
**Feature:** Global private server management with friendly names and automatic JOB ID selection  
**Scope:** Add dropdown selector for saved private servers, manage servers dialog, and enforce mandatory server selection before joining

---

## Overview

Users currently face two pain points:
1. Manual copy-pasting of JOB IDs is tedious and error-prone
2. High risk of accidentally joining public servers instead of private ones

This feature introduces a global list of saved private servers (identified by JOB ID) with friendly names. Users select a server from a dropdown before joining; if no server is selected, the join is blocked entirely. This eliminates manual JOB ID entry and prevents accidental public server joins.

---

## Requirements

### Functional Requirements

1. **Global Private Server List**
   - Maintain a global list of private servers across all accounts
   - Each entry contains: friendly name, JOB ID, date added
   - Persist to disk and load on app startup

2. **Server Selection UI**
   - Replace the current JobID text field with a dropdown (ComboBox)
   - Display friendly server names in the dropdown
   - Add a "Manage Servers" button to add/edit/delete servers

3. **Manage Servers Dialog**
   - List all saved servers (Name, JobID, Date Added columns)
   - Add button: open dialog to enter name + JobID with validation
   - Edit button: modify existing server name or JobID
   - Delete button: remove server from list
   - OK/Cancel buttons to save or discard changes

4. **Automatic JOB ID Usage**
   - When a server is selected, use its JobID automatically
   - No manual entry required
   - Hidden or read-only JobID field for reference

5. **Mandatory Server Selection**
   - Join button is **disabled** if no server is selected from dropdown
   - User cannot join without selecting a private server
   - This prevents accidental public server joins

---

## Data Model

### PrivateServer Class

```csharp
public class PrivateServer
{
    public string Name { get; set; }           // Friendly name (e.g., "Main VIP")
    public string JobId { get; set; }          // Server GUID
    public DateTime DateAdded { get; set; }    // When added
}
```

### Storage Format (JSON)

```json
{
  "servers": [
    {
      "name": "Main VIP Server",
      "jobId": "00000000-0000-0000-0000-000000000000",
      "dateAdded": "2026-05-11T00:00:00Z"
    }
  ]
}
```

---

## UI Changes

### AccountManager Form Changes

**Before:**
- JobID text field (manual entry)
- Join button always enabled

**After:**
- Private Server dropdown (ComboBox) showing friendly names
- "Manage Servers" button next to dropdown
- Hidden/read-only JobID reference field (optional)
- Join button **disabled** unless a server is selected

### Manage Servers Dialog

**Components:**
- List view: Name, JobID, Date Added columns
- Add button → opens "New Server" dialog
- Edit button → opens "Edit Server" dialog
- Delete button → removes selected server
- OK/Cancel buttons

**New/Edit Server Dialog:**
- Text field: "Server Name"
- Text field: "Job ID"
- Validate JobID format (GUID pattern)
- OK/Cancel buttons

---

## Logic Flow

### On App Startup
1. Load private server list from `%AppData%/RobloxAccountManager/private_servers.json`
2. Populate the dropdown with server names
3. Set dropdown to empty selection (no server selected)
4. Disable Join button

### When User Selects a Server from Dropdown
1. Populate hidden JobID field with the selected server's JobID
2. Enable Join button

### When User Adds/Edits/Deletes a Server
1. Update in-memory list
2. Save to disk
3. Refresh dropdown
4. If the previously selected server was deleted, clear selection and disable Join button

### When User Clicks Join Button
1. Get selected server from dropdown
2. Extract JobID from selected server
3. Proceed with existing join logic using that JobID
4. (No public server join possible because no server selection = disabled button)

---

## Configuration & Persistence

**Config File Location:** `%AppData%/RobloxAccountManager/private_servers.json`

**Default Behavior:**
- If file doesn't exist, start with empty list
- Create file on first server add
- Save immediately after any modification (add/edit/delete)

---

## Error Handling

1. **Invalid JOB ID Format**
   - Validate against GUID pattern when adding/editing
   - Show error dialog if format is invalid
   - Prevent save until corrected

2. **Duplicate Names**
   - Duplicate server names are allowed (users may have multiple servers with the same name, e.g., "Testing Server" on different regions)

3. **File Corrupted**
   - If JSON is unreadable on load, start with empty list and log error
   - User can manually re-add servers

4. **Selected Server Deleted Externally**
   - If config file is manually edited and selected server is missing, clear selection on next load

---

## Implementation Phases

### Phase 1: Core Data & Persistence
- Create `PrivateServer` class
- Implement load/save logic to JSON
- Create in-memory `List<PrivateServer>`

### Phase 2: UI Changes
- Replace JobID text field with dropdown
- Add "Manage Servers" button
- Bind dropdown to server list

### Phase 3: Manage Servers Dialog
- Create dialog form with list view
- Implement add/edit/delete logic
- Integrate with main form

### Phase 4: Join Logic & Protection
- Modify join button enable/disable logic
- Pass selected JobID to existing join method
- Disable public server joins

### Phase 5: Testing & Refinement
- Test add/edit/delete servers
- Test dropdown selection and join
- Test persistence across restarts
- Test protection against joining without server

---

## Testing Checklist

- [ ] Add a private server, verify it appears in dropdown
- [ ] Edit a server name, verify dropdown updates
- [ ] Delete a server, verify it's removed from dropdown
- [ ] Select a server, verify Join button is enabled
- [ ] Join with a selected server, verify correct JOB ID is used
- [ ] Attempt to click Join without selecting server (button should be disabled)
- [ ] Close and restart app, verify servers persist
- [ ] Test with invalid JOB ID format, verify error message
- [ ] Test with duplicate server names, verify both can coexist

---

## Files to Create/Modify

**New Files:**
- `RBX Alt Manager/Classes/PrivateServer.cs` — data model
- `RBX Alt Manager/Classes/PrivateServerManager.cs` — load/save logic
- `RBX Alt Manager/Forms/ManageServersDialog.cs` — dialog form
- `RBX Alt Manager/Forms/ManageServersDialog.Designer.cs` — UI designer

**Modified Files:**
- `RBX Alt Manager/AccountManager.cs` — replace JobID field with dropdown, add Manage button, disable join logic
- `RBX Alt Manager/AccountManager.Designer.cs` — remove JobID text field, add dropdown and button

---

## Notes

- The private server list is global (shared across all accounts) because users may have shared private servers
- The join logic remains mostly unchanged; we just provide the JOB ID from the selected server instead of user input
- The protection is enforced at the UI level (disabled button) and logic level (no join without selection)
