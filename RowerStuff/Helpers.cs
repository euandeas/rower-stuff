using AndroidX.Fragment.App;
using Google.Android.Material.Dialog;
using System.Globalization;

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

        public static void HelpDialog(FragmentActivity activity, string toolName, string usage)
        {
            MaterialAlertDialogBuilder dialog = new(activity);
            dialog.SetTitle($"How to Use - {toolName}");
            dialog.SetMessage(usage);
            dialog.SetNegativeButton("Ok", delegate
            {
                dialog.Dispose();
            });
            dialog.Show();
        }

    }
}
