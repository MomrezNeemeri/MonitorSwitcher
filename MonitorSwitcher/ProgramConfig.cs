using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSwitcher
{
    public enum MonitorMode
    {
        Dual,
        Monitor1,
        Monitor2
    }

    public class ProgramConfig
    {
        public string ProcessName { get; set; }
        public MonitorMode Mode { get; set; }
    }
}
