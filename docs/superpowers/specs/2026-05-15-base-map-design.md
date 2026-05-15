# Base Map Feature Design

**Date:** 2026-05-15  
**Feature:** Visual base map window for Steal a Brainrot account coordination  
**Status:** Design Approved

---

## Overview

A dedicated dockable window displaying a 2×4 grid of 8 base boxes, each showing the base number and assigned account name. Clicking a base switches focus to that account's Roblox client window. The system auto-captures base assignments from Account Control with manual override capability for flexibility.

**Problem Solved:** Enables quick visual identification and window switching between multiple Roblox accounts running simultaneously, reducing confusion during fast-paced gameplay.

---

## Core Features

### 1. Base Grid Display
- **Layout:** 2×4 grid showing all 8 bases (standard Steal a Brainrot spawn points)
- **Each box displays:**
  - Base number (large, prominent)
  - Account name assigned to that base (below number)
- **Visual states:**
  - **Assigned:** Normal styling (green/default background)
  - **Unassigned:** Gray/dimmed with "No Account" label
  - **Current/Active:** Highlighted with border or glow
  - **Hover:** Visual feedback (cursor change, slight highlight)

### 2. Account Switching
- **Click behavior:** Left-click any base box to bring that account's Roblox window to foreground
- **If window not running:** Show tooltip "Account not running - launch to switch"
- **Window search:** Match by:
  - Window title containing account username
  - GameInstance mapping from existing Account Manager tracking
  - Roblox client process list if necessary

### 3. Base Assignment (Hybrid Approach)

#### Auto-Capture (Primary)
- Query Account Control: `Account:GetField("BaseId")`
- In-game Lua script stores base assignment when player claims/places a base
- Automatic sync on app startup, window focus, and every 30 seconds
- Also sync on WebSocket message from Account Control

#### Manual Override (Fallback)
- Right-click context menu on any base box:
  - **"Assign Account..."** → Dropdown showing unassigned accounts
  - **"Unassign"** → Clear the assignment
- Drag-and-drop account onto base to assign (optional implementation if UI allows)
- Manual assignments stored in local JSON file (`BaseAssignments.json`)
- Priority: Account Control data > Manual assignments

### 4. Data Persistence
- **File:** `BaseAssignments.json` (alongside `AccountControlData.json`)
- **Format:**
  ```json
  {
    "Alt1": "3",
    "Alt2": "5",
    "MainAccount": "1"
  }
  ```
- Persists manual overrides across app restarts
- Auto-captured data from Account Control takes precedence

### 5. Real-Time Updates
- **Sync frequency:**
  - On window load
  - Every 30 seconds (timer-based polling)
  - Immediately on Account Control WebSocket message
  - On manual assignment change
- **UI refresh:** Update grid without disrupting user interaction
- **Status indicator:** Show "Syncing..." or "Ready" state

---

## Architecture

### Components

#### BaseMapWindow (WinForms Form/UserControl)
- Renders the 2×4 grid layout
- Handles click/right-click events
- Updates box styling based on data state
- Manages context menus
- Implements drag-and-drop (optional)
- Displays sync status indicator

#### BaseMapManager (Data Manager Class)
- Owns data sync logic
- Queries Account Control periodically
- Reads/writes `BaseAssignments.json`
- Resolves conflicts (Account Control vs. manual)
- Raises events on data changes (for UI refresh)
- Implements polling timer and WebSocket event subscriptions

#### WindowSwitcher (Window Management Class)
- Finds Roblox window for a given account
- Brings window to foreground using Windows API
- Handles cases where window doesn't exist
- Caches window handles (with timeout) for performance

#### BaseAssignments.json (Data File)
- Persists manual base-to-account mappings
- Loaded on app startup
- Updated when user makes manual assignments
- Validated on load (skip corrupted entries)

### Data Flow

```
Account Control              BaseMapManager              UI (BaseMapWindow)
    ↓                            ↓                             ↓
Query GetField("BaseId")    Sync Timer (30s)            Display Grid
     ↓                            ↓                             ↓
Returns base ID            BaseAssignments.json         On Click: WindowSwitcher
     ↓                            ↓                             ↓
Merge with local          Build account → base map     Bring Roblox to foreground
     ↓                            ↓
Conflict resolution        Raise change event
(Account Control wins)            ↓
                            Update UI
```

---

## Interactions

### User Workflows

**Workflow 1: Switch to an Account**
1. User sees base grid window
2. Clicks a base box (e.g., "Base 3 / Alt2")
3. WindowSwitcher finds Alt2's Roblox window
4. Window brought to foreground, gains focus
5. User can now play on that account

**Workflow 2: Manually Assign a Base**
1. User right-clicks an unassigned base box
2. Selects "Assign Account..."
3. Dropdown shows available accounts
4. User selects account
5. Assignment saved to `BaseAssignments.json`
6. Grid updates immediately

**Workflow 3: Auto-Capture from In-Game**
1. Player places a base in Steal a Brainrot
2. In-game Lua script detects placement
3. Script calls `Account:SetField("BaseId", "3")`
4. Account Control stores the data
5. BaseMapManager syncs (next poll or WebSocket event)
6. Grid updates automatically

---

## Error Handling

| Scenario | Behavior |
|----------|----------|
| Account Control unreachable | Retry with 3-second backoff; use cached data |
| No Roblox window for account | Show tooltip "Not running"; don't crash |
| Multiple Roblox instances (same account) | Use GameInstance tracking; switch to most-recently-focused window |
| Corrupted BaseAssignments.json | Skip corrupted entries; log warning |
| Base assigned to account, but account deleted | Mark as unassigned; log warning |
| WebSocket disconnects | Fallback to polling timer |
| Data conflict (Account Control vs. manual) | Account Control wins (source of truth) |

---

## Window Management

- **Window Type:** Dockable WinForms Form or detachable from main window
- **Initial State:** Appears unassigned until Account Control syncs
- **Position Memory:** Save/restore window position and size on close/open
- **Always on Top:** Optional toggle (for gameplay visibility)
- **Minimize/Restore:** Preserve state across sessions
- **Keyboard Shortcut:** Optional quick-switch hotkey (e.g., Alt+B for "Base")

---

## Testing & Validation

**Unit Tests:**
- BaseMapManager: Sync logic, conflict resolution, file I/O
- WindowSwitcher: Window finding, error cases
- Data parsing: BaseAssignments.json validation

**Integration Tests:**
- Account Control data sync (mock WebSocket)
- Manual assignment → file save → reload
- Window switching (real Roblox windows)

**Manual Testing:**
- Launch 2-4 accounts, assign bases, click to switch
- Test unassigned/missing account scenarios
- Verify sync works with Account Control
- Test manual override when Account Control data missing

---

## Out of Scope (Phase 1)

- Custom base names or nicknames (future enhancement)
- Base status info (lock timer, cash/sec, Brainrot count) — can add in Phase 2
- Keyboard shortcuts for base switching (can add later)
- Multi-monitor window detection (handled by OS window manager)
- Visual map overlay (stick with grid layout for Phase 1)

---

## Implementation Notes

- Use existing Account Manager patterns (GameInstance, Account, ControlledAccount classes)
- Reuse Account Control query logic where possible
- Leverage WebSocketServer events for real-time updates
- Store config alongside AccountControlData.json for consistency
- Consider thread safety if syncing on background timer

---

## Success Criteria

✓ Grid displays all 8 bases with assigned account names  
✓ Clicking a base brings that Roblox window to focus  
✓ Auto-capture from Account Control works when script runs in-game  
✓ Manual assignment available as fallback  
✓ Unassigned bases clearly marked  
✓ Real-time updates (30-second max latency)  
✓ No crashes on missing windows or Account Control errors  
✓ Assignments persist across app restarts
