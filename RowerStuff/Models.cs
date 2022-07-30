using System.Diagnostics;
using System.Timers;

namespace RowerStuff.Models
{
    public static class Formulas
    {
        //Calculate Distance - distance = (time/split) * 500
        public static double DistanceFromTimeSplit(TimeSpan time, TimeSpan split)
        {
            return (time.TotalMilliseconds / split.TotalMilliseconds) * 500;
        }

        //Calculate split - split = 500 * (time/distance)
        public static TimeSpan SplitFromTimeDistance(TimeSpan time, int distance)
        {
            return TimeSpan.FromMilliseconds((time.TotalMilliseconds / distance) * 500);
        }

        //Calculate total time - time = split * (distance/500)
        public static TimeSpan TimeFromSplitDistance(TimeSpan split, int distance)
        {
            return TimeSpan.FromMilliseconds(split.TotalMilliseconds * (distance / 500));
        }

        public static TimeSpan PaulsLaw(int distance, int distanceToPredict, TimeSpan split)
        {
            return TimeSpan.FromSeconds(split.TotalSeconds + (5 * Math.Log2(distanceToPredict / distance)));
        }

        public static TimeSpan PercentagePace(TimeSpan split, int percent)
        {
            return WattsToPace(PaceToWatts(split) * (percent / 100));
        }

        public static double PercentageWatts(double watts, int percent)
        {
            return watts * (percent / 100);
        }

        public static double PaceToWatts(TimeSpan split)
        {
            return 2.8 / Math.Pow((split.TotalSeconds / 500), 3);
        }

        public static TimeSpan WattsToPace(double watts)
        {
            return TimeSpan.FromSeconds(Math.Pow(2.8 / watts, 1.0 / 3.0) * 500);
        }

        public static double VO2Max(double weightkg, string gender, string traininglvl, TimeSpan twoKTime)
        {
            double timeMinutes = twoKTime.TotalMinutes;
            double Y = 0;

            if (gender == "Male")
            {
                if (traininglvl == "Low")
                {
                    Y = 10.7 - (0.9 * timeMinutes);
                }
                else if (traininglvl == "High")
                {
                    if (weightkg > 75)
                    {
                        Y = 15.7 - (1.5 * timeMinutes);
                    }
                    else if (weightkg <= 75)
                    {
                        Y = 15.1 - (1.5 * timeMinutes);
                    }
                }
            }
            else if (gender == "Female")
            {
                if (traininglvl == "Low")
                {
                    Y = 10.26 - (0.93 * timeMinutes);
                }
                else if (traininglvl == "High")
                {
                    if (weightkg > 61.36)
                    {
                        Y = 14.9 - (1.5 * timeMinutes);
                    }
                    else if (weightkg <= 61.36)
                    {
                        Y = 14.6 - (1.5 * timeMinutes);
                    }
                }
            }

                return (Y * 1000) / weightkg;
        }

        public static double LBToKG(double weight)
        {
            return weight * 0.45359237;
        }

        public static double KGToLB(double weight)
        {
            return weight / 0.45359237;
        }

        public static TimeSpan TimeWeightAdjusted(double weightlb, TimeSpan time)
        {
            return TimeSpan.FromSeconds(WeightFactor(weightlb) * time.TotalSeconds);
        }

        public static double DistanceWeightAdjusted(double weightlb,  double distance)
        {
            return distance / WeightFactor(weightlb);
        }

        public static double WeightFactor(double weightlb)
        {
            return Math.Pow(weightlb / 270, 0.222);
        }
    }

    public class SPM : Stopwatch
    {
        private readonly System.Timers.Timer timer = new();
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
