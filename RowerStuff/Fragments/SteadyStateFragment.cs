using Android.Views;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class SteadyStateFragment : Fragment
    {
        private EditText enteredMin;
        private EditText enteredSec;
        private TextView result;

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_steady_state, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "Steady State",
                "Enter your 2KM personal best and this will give you the pace range you should hold for steady state work. This pace range is 50-60% of your avg 2KM wattage.\n\nTo clear all data hold the calculate button.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed);

            enteredMin = view.FindViewById<EditText>(Resource.Id.enteredMin);
            enteredSec = view.FindViewById<EditText>(Resource.Id.enteredSec);

            result = view.FindViewById<TextView>(Resource.Id.paceRange);

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredMin.Text = "";
            enteredSec.Text = "";
            result.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            if ((enteredMin.Text != "" || enteredSec.Text != ""))
            {
                if (Helpers.ParseMinSecMS(enteredMin.Text, enteredSec.Text) is TimeSpan parsedTotalTime && parsedTotalTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "2KM PB time has no value!", ToastLength.Short).Show();
                    return;
                }

                TimeSpan splitReadable50 = Formulas.PercentagePace(parsedTotalTime, 50);
                TimeSpan splitReadable60 = Formulas.PercentagePace(parsedTotalTime, 60);

                var Split50AsString = string.Format("{0}:{1}.{2}", (int)splitReadable50.TotalMinutes, splitReadable50.Seconds, splitReadable50.Milliseconds);
                var Split60AsString = string.Format("{0}:{1}.{2}", (int)splitReadable60.TotalMinutes, splitReadable60.Seconds, splitReadable60.Milliseconds);

                result.Text = $"{Split60AsString}  -  {Split50AsString}";
            }
            else
            {
                Toast.MakeText(Activity, "Must enter a time!", ToastLength.Short).Show();
            }
        }
    }
}
