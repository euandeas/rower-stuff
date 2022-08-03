using Android.Views;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using System.Globalization;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class PercentageWattsFragment : Fragment
    {
        private EditText enteredWatts;
        private TextView percentageLabel;
        private SeekBar seekBar;
        private TextView result;

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_percentage_watts, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "Percentage Watts",
                "Enter watts and the chosen percentage of the watts will be returned.\n\nTo clear all data hold the calculate button.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed);

            enteredWatts = view.FindViewById<EditText>(Resource.Id.enteredWatts);

            percentageLabel = view.FindViewById<TextView>(Resource.Id.percentageLabel);
            seekBar = view.FindViewById<SeekBar>(Resource.Id.seekBar);
            seekBar.ProgressChanged += (object? sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                percentageLabel.Text = ($"{e.Progress}%");
            };
            seekBar.Progress = 100;

            result = view.FindViewById<TextView>(Resource.Id.percentageWattsResult);

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredWatts.Text = "";
            seekBar.Progress = 100;
            result.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            if (enteredWatts.Text != "" && enteredWatts.Text != ".")
            {
                if (double.Parse(enteredWatts.Text, CultureInfo.InvariantCulture) is double watts && watts == 0)
                {
                    Toast.MakeText(Activity, "Watts has no value!", ToastLength.Short).Show();
                    return;
                }

                if (seekBar.Progress is int percent && percent == 0)
                {
                    Toast.MakeText(Activity, "Percentage cannot be 0%!", ToastLength.Short).Show();
                    return;
                }

                result.Text = Formulas.PercentageWatts(watts, percent).ToString();
            }
            else
            {
                Toast.MakeText(Activity, "Must enter watts!", ToastLength.Short).Show();
            }
        }
    }
}
