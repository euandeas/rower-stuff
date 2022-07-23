using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RowerStuff
{
    static class Models
    {
        static class Formulas
        {
            //Calculate Distance - distance = (time/split) * 500
            static double DistanceFromTimeSplit(TimeSpan time, TimeSpan split)
            {
                return (time.TotalMilliseconds / split.TotalMilliseconds) * 500;
            }

            //Calculate split - split = 500 * (time/distance)
            static TimeSpan SplitFromTimeDistance(TimeSpan time, int distance)
            {
                return TimeSpan.FromMilliseconds((time.TotalMilliseconds / distance) * 500);
            }

            //Calculate total time - time = split * (distance/500)
            static TimeSpan TimeFromSplitDistance(TimeSpan split, int distance)
            {
                return TimeSpan.FromMilliseconds(split.TotalMilliseconds * (distance / 500));
            }


        }

        public class SPM : Stopwatch
        {
            private System.Timers.Timer timer = new();
            private TimeSpan ts;

            public SPM()
            {
                timer.Interval = 15000;
                timer.AutoReset = false;
                timer.Elapsed += TimeElapsed;
            }

            private void TimeElapsed(object? sender, ElapsedEventArgs e)
            {
                Reset();
            }

            public int Beat()
            {
                if(IsRunning)
                {
                    ts = Elapsed;
                    timer.Stop();
                    timer.Start();
                    Restart();

                    double result = 60 / ts.TotalSeconds;
                    return (int)Math.Round(result);
                }
                else
                {
                    timer.Start();
                    Start();
                    return 0;
                }
            }

        }
    }
}
