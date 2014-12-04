using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MasterBaiter
{
    class MyWaitTimer
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private TimeSpan _WaitTime;
        //private static bool _Finished = false;

        public bool IsFinished
        {
            get
            {
                if (TimeLeft.TotalSeconds <= 0)
                {
                    _stopwatch.Stop();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public MyWaitTimer()
        {
        }

        public MyWaitTimer(TimeSpan WaitTime)
        {
            _WaitTime = new TimeSpan(WaitTime.Ticks);
            _stopwatch.Start();
        }

        public TimeSpan TimeLeft
        {
            get
            {
                return _WaitTime - _stopwatch.Elapsed;
            }
        }
    }
}
