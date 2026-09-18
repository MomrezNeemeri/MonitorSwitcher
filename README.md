# MonitorSwitcher

A Windows tray utility that switches your display configuration automatically based on which
programs are running. Tell it "when `game.exe` starts, use `\.\DISPLAY2` only" — it watches
process creation via WMI, flips to that single monitor, and restores your multi-monitor layout
when the program exits.

Useful when a game or full-screen app misbehaves on a multi-monitor setup and you'd otherwise
be visiting Windows display settings before and after every session.

## Requirements

- Windows (tested on Windows 11)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) to build; .NET 8 Desktop
  Runtime to run
- Two or more monitors attached to the desktop (single-monitor machines have nothing to switch)

## Build & run

```powershell
git clone <repo-url>
cd MonitorSwitcher
dotnet build MonitorSwitcher.sln
dotnet run --project MonitorSwitcher/MonitorSwitcher.csproj
```

Or open `MonitorSwitcher.sln` in Visual Studio 2022 and press F5.

## Usage

1. Launch the app. On startup it records the current layout of every attached display — this
   snapshot is what "restore" means later, so start it while your displays are arranged the way
   you want them normally.
2. Type a process name in **Process Name** — the executable name as Windows reports it,
   including the extension (`witcher3.exe`, not `Witcher 3`). Matching is case-insensitive.
3. Pick a target from the monitor dropdown. Entries are Windows device names
   (`\.\DISPLAY1`, `\.\DISPLAY2`, …), plus a **Dual Monitors** entry meaning "keep the saved
   multi-monitor layout".
4. Click **Add Process**. The entry is saved immediately.
5. Leave the app running. Closing the window hides it to the system tray rather than exiting —
   double-click the tray icon to bring it back, or use **Exit** on its right-click menu to quit
   for real.

When a configured process starts, every other display is disabled and the chosen one becomes
primary at its saved resolution and refresh rate. When that process exits, the saved layout is
restored. The list box at the bottom logs every process start/stop and the result code of each
display change.

**Save Processes** / **Load Process** re-write and re-read the config file on demand; adding and
removing entries already saves automatically, so these are mostly for recovering a file you
edited by hand.

## Configuration file

Entries are stored as JSON at:

```
%LOCALAPPDATA%\MonitorSwitcher\config.json
```

```json
[
  {
    "ProcessName": "witcher3.exe",
    "MonitorDeviceName": "\\.\DISPLAY2"
  }
]
```

## How it works

| File | Role |
| --- | --- |
| [ProcessMonitor.cs](MonitorSwitcher/ProcessMonitor.cs) | Subscribes to the WMI events `__InstanceCreationEvent` and `__InstanceDeletionEvent` over `Win32_Process` (1-second polling interval) and raises `ProcessStarted` / `ProcessStopped`. |
| [MonitorManager.cs](MonitorSwitcher/MonitorManager.cs) | P/Invokes `EnumDisplayDevices`, `EnumDisplaySettings` and `ChangeDisplaySettingsEx` from `user32.dll` to enumerate displays, snapshot their `DEVMODE`, and apply single- or dual-monitor layouts. |
| [ConfigurationManager.cs](MonitorSwitcher/ConfigurationManager.cs) | Async JSON load/save of the `ProgramConfig` list under `%LOCALAPPDATA%`. |
| [Form1.cs](MonitorSwitcher/Form1.cs) | UI, tray behavior, and the glue that maps a started process to a monitor switch. |

Display changes are staged with `CDS_UPDATEREGISTRY | CDS_NORESET` per device and then committed
with a single `ChangeDisplaySettingsEx(null, ...)` call, so all monitors change in one step
instead of flickering through intermediate states.

## Known limitations

- The WMI watcher fires for *every* process on the system at a 1-second cadence, and the output
  log records all of them. On a busy machine that list grows quickly.
- The initial display snapshot is taken only at startup. If you rearrange your monitors while the
  app is running, restart it so "Dual Monitors" restores the new arrangement.
- Restoring after a process exits always applies the full saved layout, regardless of which
  monitor that process was mapped to.
- If two configured programs run at once, the last one to start wins; the first to exit restores
  the full layout.
- Process names are matched by executable name only, so two unrelated programs sharing an exe
  name cannot be told apart.
