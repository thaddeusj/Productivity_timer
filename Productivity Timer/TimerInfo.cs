using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Productivity_Timer
{
    class TimerInfo
    {

        public DateTime Time { get; set; }
        public string CancelReason { get; set; }
        public string StoppedReason { get; set; }
        public bool Completed { get; set; }
        public bool Canceled { get; set; }

    }
}
