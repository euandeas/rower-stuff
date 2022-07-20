using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerStuff
{
    static class DataProcessing
    {

        public static TimeSpan ParseMinSecMS(string inputMins, string inputSecs)
        {
            string minutes = inputMins;
            string seconds = inputSecs;

            if (string.IsNullOrWhiteSpace(inputMins))
                minutes = "00";

            if (string.IsNullOrWhiteSpace(inputSecs) || inputSecs == ".")
                seconds = "00";

            long minutesParsed = long.Parse(minutes);
            double secondsParsed = double.Parse(seconds, CultureInfo.InvariantCulture);

            return TimeSpan.FromMinutes(minutesParsed).Add(TimeSpan.FromSeconds(secondsParsed));
        }

    }
}
