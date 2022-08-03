using Android.Views;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using System.Globalization;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class WattsFragment : Fragment
    {
        private EditText enteredMin;
        private EditText enteredSec;
        private EditText enteredWatts;

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_watts, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "Watts",
                "Enter either splits or watts and it will be converted to the other.\n\nTo clear all data hold the calculate button.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed);

            enteredMin = view.FindViewById<EditText>(Resource.Id.enteredMin);
            enteredSec = view.FindViewById<EditText>(Resource.Id.enteredSec);

            enteredWatts = view.FindViewById<EditText>(Resource.Id.enteredWatts);

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredMin.Text = "";
            enteredSec.Text = "";
            enteredWatts.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            //Calculate Watts - watts = 2.8/(split/500)³
            if ((enteredWatts.Text == "") && (enteredMin.Text != "" || enteredSec.Text != ""))
            {
                if (Helpers.ParseMinSecMS(enteredMin.Text, enteredSec.Text) is TimeSpan parsedSplitTime && parsedSplitTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "Split has no value!", ToastLength.Short).Show();
                    return;
                }

                enteredWatts.Text = $"{Formulas.PaceToWatts(parsedSplitTime)}";
            }
            //Calculate Split - split = (cube-root(w*2.8))*500
            else if ((enteredWatts.Text != "" && (enteredWatts.Text != "." || enteredWatts.Text != ",")) && (enteredMin.Text == "" && enteredSec.Text == ""))
            {
                if (double.Parse(enteredWatts.Text, CultureInfo.InvariantCulture) is double watts && watts == 0)
                {
                    Toast.MakeText(Activity, "Watts has no value!", ToastLength.Short).Show();
                    return;
                }

                TimeSpan splitReadable = Formulas.WattsToPace(watts);

                enteredMin.Text = $"{(int)splitReadable.TotalMinutes}";
                enteredSec.Text = $"{splitReadable.Seconds:D2}.{splitReadable.Milliseconds}";
            }
            else
            {
                Toast.MakeText(Activity, "Must enter either split or watts!", ToastLength.Short).Show();
            }
        }
    }
}
