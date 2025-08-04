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
        public Form1()
        {
            InitializeComponent();
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
        private void ProcessMonitor_ProcessStarted(object sender, ProcessEventArgs e)
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

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _processMonitor.StartMonitoring();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _processMonitor.StopMonitoring();
            AppendOutput("Application closing. Monitoring stopped.");
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
    }
}
