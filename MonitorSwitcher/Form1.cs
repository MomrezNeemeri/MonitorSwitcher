using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace MonitorSwitcher
{
    public partial class Form1 : Form
    {
        private ProcessMonitor _processMonitor;

        //configuration manager instance
        private ConfigurationManager _configManager;
            
        private List<ProgramConfig> _configuredPrograms;
        private MonitorManager _monitorManager;
        public Form1()
        {
            InitializeComponent();
            //init the ConfigurationManager
            _configManager = new ConfigurationManager();
            _configuredPrograms = new List<ProgramConfig>();

            //init the ProcessMonitor
            _processMonitor = new ProcessMonitor();

            _processMonitor.ProcessStarted += ProcessMonitor_ProcessStarted;
            _processMonitor.ProcessStopped += ProcessMonitor_ProcessStopped;

            // Start monitoring when the form loads
            this.Load += Form1_Load;
            // Stop monitoring when the form is closing
            this.FormClosing += Form1_FormClosing;
            AppendOutput("Application closing. Monitoring stopped.");


        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            PopulateMonitorComboBox();
            // Load programs automatically when the form loads.
            _configuredPrograms = await _configManager.LoadConfigAsync();
            RefreshProcessListUI();

            // Start monitoring processes.
            _processMonitor.StartMonitoring();
            AppendOutput("Application started. Monitoring processes...");
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Logic for hiding the app to the system tray.
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancel the close operation.
                this.Hide();     // Hide the form.
                notifyIconApp.Visible = true; // Make the tray icon visible.
                AppendOutput("Application minimized to tray.");
            }
            // For a final close, we'll let it happen.
            else
            {
                _processMonitor.StopMonitoring();
                notifyIconApp.Visible = false;
            }
        }

        // Append the message to the output TextBox
        private void AppendOutput(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendOutput), text);
            }
            outputListBox.Items.Add(text);
            // Scrolling to the bottom
            outputListBox.SelectedIndex = outputListBox.Items.Count - 1;

        }

        private async void AddGameButton_Click(object sender, EventArgs e)
        {
            string processName = gameNameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(processName)||monitorComboBox.SelectedItem ==null)
            {
                MessageBox.Show("Please enter a valid process name and monitor.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_configuredPrograms.Any(p => p.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("This process name is already in the list.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //new config
            var newConfig = new ProgramConfig
            {
                ProcessName = processName,
                // Cast the ComboBox item back to the enum
                Mode = (MonitorMode)monitorComboBox.SelectedItem
            };

            _configuredPrograms.Add(newConfig);
            RefreshProcessListUI();
            gameNameTextBox.Clear();

            await _configManager.SaveConfigAsync(_configuredPrograms);
            AppendOutput($"Added and saved: {processName}");
        }
        private async void RemoveGameButton_Click(object sender, EventArgs e)
        {
            if (gameListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select an item to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // The item in the list box is now a string representation of the ProgramConfig
            string selectedItemText = gameListBox.SelectedItem.ToString();

            // Find the corresponding object in our list
            var configToRemove = _configuredPrograms.FirstOrDefault(p => $"{p.ProcessName} -> {p.Mode}" == selectedItemText);

            if (configToRemove != null)
            {
                _configuredPrograms.Remove(configToRemove);
                RefreshProcessListUI();
                await _configManager.SaveConfigAsync(_configuredPrograms);
                AppendOutput($"Removed and saved: {selectedItemText}");
            }
        }

        private void RefreshProcessListUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshProcessListUI));
                return;
            }
            gameListBox.Items.Clear();
            foreach (var program in _configuredPrograms)
            {
                gameListBox.Items.Add($"{program.ProcessName} -> {program.Mode}");
            }
        }
        private async void SaveConfigButton_Click(object sender, EventArgs e)
        {
            await _configManager.SaveConfigAsync(_configuredPrograms);
            AppendOutput("Configuration manually saved.");
        }
        private void PopulateMonitorComboBox()
        {
            monitorComboBox.Items.Clear();
            monitorComboBox.Items.AddRange(Enum.GetValues(typeof(MonitorMode)).Cast<object>().ToArray());
            if (monitorComboBox.Items.Count > 0)
            {
                monitorComboBox.SelectedIndex = 0;
            }
        }

        private async void LoadConfigButton_Click(object sender, EventArgs e)
        {
            _configuredPrograms = await _configManager.LoadConfigAsync();
            RefreshProcessListUI();
            AppendOutput("Configuration manually loaded.");
        }
        // process moinitoring event handlers

        private void ProcessMonitor_ProcessStarted(object sender, ProcessEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ProcessMonitor_ProcessStarted(sender, e)));
                return;
            }

            // Find the configured monitor for this process
            var config = _configuredPrograms.FirstOrDefault(p => p.ProcessName.Equals(e.ProcessName, StringComparison.OrdinalIgnoreCase));

            string message = $"{DateTime.Now.ToLongTimeString()} - Process STARTED: {e.ProcessName} (PID: {e.ProcessId})";
            AppendOutput(message);

            if (config != null)
            {
                AppendOutput($"--- Configured PROGRAM STARTED: {e.ProcessName} on {config.Mode} ---");
                notifyIconApp.ShowBalloonTip(3000, "Program Started", $"{e.ProcessName} has started on {config.Mode}.", ToolTipIcon.Info);

                // Call the monitor switching logic
                if (config.Mode == MonitorMode.Monitor1)
                {
                    _monitorManager.SetSingleMonitor(1);
                }
                else if (config.Mode == MonitorMode.Monitor2)
                {
                    _monitorManager.SetSingleMonitor(2);
                }
                else // Dual monitor mode
                {
                    _monitorManager.SetDualMonitors();
                }
            }
        }

        private void ProcessMonitor_ProcessStopped(object sender, ProcessEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ProcessMonitor_ProcessStopped(sender, e)));
                return;
            }

            var config = _configuredPrograms.FirstOrDefault(p => p.ProcessName.Equals(e.ProcessName, StringComparison.OrdinalIgnoreCase));

            string message = $"{DateTime.Now.ToLongTimeString()} - Process ENDED: {e.ProcessName} (PID: {e.ProcessId})";
            AppendOutput(message);

            if (config != null)
            {
                AppendOutput($"--- Configured PROGRAM ENDED: {e.ProcessName} ---");
                notifyIconApp.ShowBalloonTip(3000, "Program Ended", $"{e.ProcessName} has closed.", ToolTipIcon.Info);

                // Revert to the initial dual-monitor setup when a configured program closes
                _monitorManager.SetDualMonitors();
            }
        }
        // --- System Tray Icon Event Handlers ---

        private void notifyIconApp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIconApp.Visible = false;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIconApp_MouseDoubleClick(sender, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
