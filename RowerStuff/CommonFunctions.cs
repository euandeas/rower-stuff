using Android.Content;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Widget;
using System;

namespace RowerStuff
{
    static class CommonFunctions
    {
        public static TimeSpan ParseMinSecMS(string inputMins, string inputSecs)
        {
            string minutes = inputMins;
            string seconds = inputSecs;

            if (string.IsNullOrWhiteSpace(inputMins))
                minutes = "00";

            if (string.IsNullOrWhiteSpace(inputSecs))
                seconds = "00";

            long minutesParsed = long.Parse(minutes);
            double secondsParsed = double.Parse(seconds);

            return TimeSpan.FromMinutes(minutesParsed).Add(TimeSpan.FromSeconds(secondsParsed));
        }

        public static void HelpDialog(FragmentActivity activity, string toolName, string message)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(activity);
            dialog.SetTitle($"How To Use - {toolName}");
            dialog.SetMessage(message);
            dialog.SetNegativeButton("Ok", delegate
            {
                dialog.Dispose();
            });
            dialog.Show();
        }

        public static void CopyToClipBoard(string result, Context context)
        {
            ClipboardManager clipboard = (ClipboardManager)context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("Rower Stuff Data", result);
            clipboard.PrimaryClip = clip;
            Toast.MakeText(context, "Copied to clipboard", ToastLength.Short).Show();
        }
    }
}