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
        private List<string> _configuredPrograms;
        public Form1()
        {
            InitializeComponent();
            //init the ConfigurationManager
            _configManager = new ConfigurationManager();
            _configuredPrograms = new List<string>();

            //init the ProcessMonitor
            _processMonitor = new ProcessMonitor();

            _processMonitor.ProcessStarted += ProcessMonitor_ProcessStarted;
            _processMonitor.ProcessStopped += ProcessMonitor_ProcessStopped;

            loadConfigurationbutton.Click += LoadConfigButton_Click;
            saveConfigurationButton.Click += SaveConfigButton_Click;
            removeGameButton.Click += RemoveGameButton_Click;
            addGameButton.Click += AddGameButton_Click;

            // Start monitoring when the form loads
            this.Load += Form1_Load;
            // Stop monitoring when the form is closing
            this.FormClosing += Form1_FormClosing;
            AppendOutput("Application closing. Monitoring stopped.");


        }
/*        private void ProcessMonitor_ProcessStarted(object sender, ProcessEventArgs e)
        {
            // Checking if we are currently on a different thread than the UI thread.
            if (this.InvokeRequired)
            {
                // If so, invoke this method on the UI thread.
                this.Invoke(new Action(() => ProcessMonitor_ProcessStarted(sender, e)));
                return;
            }

            // This code runs on the UI thread
            string message = $"{DateTime.Now.ToLongTimeString()} - Process STARTED: {e.ProcessName} (PID: {e.ProcessId})";
            AppendOutput(message);

        }

        private void ProcessMonitor_ProcessStopped(object sender, ProcessEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ProcessMonitor_ProcessStopped(sender, e)));
                return;
            }
            //ui thread confirmed
            string message = $"{DateTime.Now.ToLongTimeString()} - Process ENDED: {e.ProcessName} (PID: {e.ProcessId})";
            AppendOutput(message);

        }*/
        private async void Form1_Load(object sender, EventArgs e)
        {
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
            //validate the process name
            if (string.IsNullOrEmpty(processName))
            {
                MessageBox.Show("Please enter a valid process name.");
                return;
            }
            if ((_configuredPrograms.Contains(processName, StringComparer.OrdinalIgnoreCase)))
            {
                MessageBox.Show("already in the list", "duplicate entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //add to list and ui
            _configuredPrograms.Add(processName);
            gameListBox.Items.Add(processName);
            gameNameTextBox.Clear();
            //save list to file
            await _configManager.SaveConfigAsync(_configuredPrograms);
            AppendOutput($"Added game: {processName}");
        }
        private async void RemoveGameButton_Click(object sender, EventArgs e)
        {
            if (gameListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a game to remove.");
                return;
            }
            string selectedProcess = gameListBox.SelectedItem.ToString();
            _configuredPrograms.Remove(selectedProcess);
            gameListBox.Items.Remove(selectedProcess);

            await _configManager.SaveConfigAsync(_configuredPrograms);
            AppendOutput($"Removed game: {selectedProcess}");
        }

        private void RefreshProcessListUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshProcessListUI));
                return;
            }
            gameListBox.Items.Clear();
            foreach (var game in _configuredPrograms)
            {
                gameListBox.Items.Add(game);
            }
        }
        private async void SaveConfigButton_Click(object sender, EventArgs e)
        {
            await _configManager.SaveConfigAsync(_configuredPrograms);
            AppendOutput("Configuration manually saved.");
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

            string message = $"{DateTime.Now.ToLongTimeString()} - Process STARTED: {e.ProcessName} (PID: {e.ProcessId})";
            AppendOutput(message);

            if (_configuredPrograms.Contains(e.ProcessName, StringComparer.OrdinalIgnoreCase))
            {
                AppendOutput($"--- Configured PROGRAM STARTED: {e.ProcessName} ---");
                // TODO: Implement monitor switching logic .
            }
        }

        private void ProcessMonitor_ProcessStopped(object sender, ProcessEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ProcessMonitor_ProcessStopped(sender, e)));
                return;
            }

            string message = $"{DateTime.Now.ToLongTimeString()} - Process ENDED: {e.ProcessName} (PID: {e.ProcessId})";
            AppendOutput(message);

            if (_configuredPrograms.Contains(e.ProcessName, StringComparer.OrdinalIgnoreCase))
            {
                AppendOutput($"--- Configured PROGRAM ENDED: {e.ProcessName} ---");
                // TODO: Implement  monitor reverting logic .
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
