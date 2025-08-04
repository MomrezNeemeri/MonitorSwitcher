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
            if (_startWatcher != null || _stopWatcher != null)
            {

                StopMonitoring();
            }

            /*WQL Query for process Creation
            Within 1 second interval*/
            string creationQuery = "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";

            _startWatcher = new ManagementEventWatcher(creationQuery);
            _startWatcher.EventArrived += StartWatcher_EventArrived;
            _startWatcher.Start();

            //WQL Query for process Termination
            string terminationQuery = "SELECT * FROM __InstanceDeletionEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";

            _stopWatcher = new ManagementEventWatcher(terminationQuery);
            _stopWatcher.EventArrived += StopWatcher_EventArrived;
            _stopWatcher.Start();

            Console.WriteLine("Monitoring started.");

        }

        public void StopMonitoring()
        {
            if (_startWatcher != null)
            {
                _startWatcher.Stop();
                _startWatcher.Dispose();
                _startWatcher = null;
            }
            if (_stopWatcher != null)
            {
                _stopWatcher.Stop();
                _stopWatcher.Dispose();
                _stopWatcher = null;
            }

            Console.WriteLine("Monitoring stopped.");

        }

        public void StartWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject newProcess = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string processName = newProcess["Name"].ToString();
            int processId = Convert.ToInt32(newProcess["ProcessId"]);
            /*raise cusstiom event ;; notify form1*/
            ProcessStarted?.Invoke(this, new ProcessEventArgs(processName, processId));
        }
        // Fix: In StopWatcher_EventArrived, should invoke ProcessStopped, not ProcessStarted
        public void StopWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject oldProcess = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string processName = oldProcess["Name"].ToString();
            int processId = Convert.ToInt32(oldProcess["ProcessId"]);
            /*raise custom event ;; notify form1*/
            ProcessStopped?.Invoke(this, new ProcessEventArgs(processName, processId));
        }
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
