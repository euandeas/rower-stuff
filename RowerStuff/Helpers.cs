using AndroidX.Fragment.App;
using Google.Android.Material.Dialog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerStuff
{
    static class Helpers
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

        public static void HelpDialog(FragmentActivity activity, string toolName, string about, string usage)
        {
            MaterialAlertDialogBuilder dialog = new MaterialAlertDialogBuilder(activity);
            dialog.SetTitle($"How to Use - {toolName}");
            dialog.SetMessage($"{about}\n\n{usage}");
            dialog.SetNegativeButton("Ok", delegate
            {
                dialog.Dispose();
            });
            dialog.Show();
        }

    }
}
