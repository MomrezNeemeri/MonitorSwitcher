using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace MonitorSwitcher
{
    public class ProcessMonitor
    {
        public ManagementEventWatcher _startWatcher;
        public ManagementEventWatcher _stopWatcher;

        public event EventHandler<ProcessEventArgs> ProcessStarted;
        public event EventHandler<ProcessEventArgs> ProcessStopped;

    public void StartMonitoring()
    {
        _startWatcher = new ManagementEventWatcher();
        _stopWatcher = new ManagementEventWatcher();
        var startQuery = new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace");


        }

    public void StopMonitoring()
        {
            if(_startWatcher != null)
            {
                _startWatcher.Stop();
                _startWatcher.Dispose();
                _startWatcher = null;
            }
            if(_stopWatcher != null)
            {
                _stopWatcher.Stop();
                _stopWatcher.Dispose();
                _stopWatcher = null;
            }

            Console.WriteLine("Monitoring stopped.");

        }
    public class ProcessEventArgs : EventArgs
{
    public string ProcessName { get; }
    public int ProcessId { get; }

    public ProcessEventArgs(string processName, int processId)
    {
        ProcessName = processName;
        ProcessId = processId;
    }
}
}
