using Android.Views;
using AndroidX.CardView.Widget;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class PercentagePaceFragment : Fragment
    {
        private EditText enteredSplitMin;
        private EditText enteredSplitSec;
        private TextView percentageLabel;
        private SeekBar seekBar;
        private TextView result;

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_percentage_pace, container, false)!;

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar)!;
            (Activity as MainActivity)!.SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "Percentage Pace",
                "Enter a split and the chosen percentage of that split, worked out using the wattage, will be returned.\n\nTo clear specific data hold on the specific card.\nTo clear all data hold the calculate button.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed!);

            CardView splitCard = view.FindViewById<CardView>(Resource.Id.splitCard)!;
            enteredSplitMin = view.FindViewById<EditText>(Resource.Id.enteredMin)!;
            enteredSplitSec = view.FindViewById<EditText>(Resource.Id.enteredSec)!;
            splitCard.LongClick += (s, e) => { enteredSplitMin.Text = ""; enteredSplitSec.Text = ""; };

            percentageLabel = view.FindViewById<TextView>(Resource.Id.percentageLabel)!;
            seekBar = view.FindViewById<SeekBar>(Resource.Id.seekBar)!;
            seekBar.ProgressChanged += (object? sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                percentageLabel.Text = ($"{e.Progress}%");
            };
            seekBar.Progress = 100;

            result = view.FindViewById<TextView>(Resource.Id.percentagePaceResult)!;

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton)!;
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredSplitMin.Text = "";
            enteredSplitSec.Text = "";
            seekBar.Progress = 100;
            result.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            if (enteredSplitMin.Text != "" || enteredSplitSec.Text != "")
            {
                if (Helpers.ParseMinSecMS(enteredSplitMin.Text!, enteredSplitSec.Text!) is TimeSpan parsedSplitTime && parsedSplitTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "Split has no value!", ToastLength.Short)!.Show();
                    return;
                }

                if (seekBar.Progress is int percent && percent == 0)
                {
                    Toast.MakeText(Activity, "Percentage cannot be 0%!", ToastLength.Short)!.Show();
                    return;
                }
          
                TimeSpan splitReadable = Formulas.PercentagePace(parsedSplitTime, percent);
                result.Text = string.Format("{0}:{1}.{2}", (int)splitReadable.TotalMinutes, splitReadable.Seconds, splitReadable.Milliseconds);
                
            }
            else
            {
                Toast.MakeText(Activity, "Must enter a split!", ToastLength.Short)!.Show();
            }
        }
    }
}
