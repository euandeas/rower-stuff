using Android.Views;
using AndroidX.AppCompat.View.Menu;
using AndroidX.CardView.Widget;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class PaceFragment : Fragment
    {
        private EditText enteredDistance;
        private EditText enteredSplitMin;
        private EditText enteredSplitSec;
        private EditText enteredTimeMin;
        private EditText enteredTimeSec;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_pace, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);
            HasOptionsMenu = true;

            enteredDistance = view.FindViewById<EditText>(Resource.Id.enteredDistance);
            
            CardView splitCard = view.FindViewById<CardView>(Resource.Id.splitCard);
            enteredSplitMin = splitCard.FindViewById<EditText>(Resource.Id.enteredMin);
            enteredSplitSec = splitCard.FindViewById<EditText>(Resource.Id.enteredSec);
            
            CardView timeCard = view.FindViewById<CardView>(Resource.Id.timeCard);
            enteredTimeMin = timeCard.FindViewById<EditText>(Resource.Id.enteredMin);
            enteredTimeSec = timeCard.FindViewById<EditText>(Resource.Id.enteredSec);
            
            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredDistance.Text = "";
            enteredSplitMin.Text = "";
            enteredSplitSec.Text = "";
            enteredTimeMin.Text = "";
            enteredTimeSec.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            //Calculate Distance - distance = (time/split) * 500
            if ((enteredDistance.Text == "") && (enteredSplitMin.Text != "" || enteredSplitSec.Text != "") && (enteredTimeMin.Text != "" || enteredTimeSec.Text != ""))
            {
                if (Helpers.ParseMinSecMS(enteredSplitMin.Text, enteredSplitSec.Text) is TimeSpan parsedSplitTime && parsedSplitTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "Split has no value!", ToastLength.Short).Show();
                    return;
                }

                if (Helpers.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text) is TimeSpan parsedTotalTime && parsedTotalTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "Time has no value!", ToastLength.Short).Show();
                    return;
                }

                enteredDistance.Text = Formulas.DistanceFromTimeSplit(parsedTotalTime, parsedSplitTime).ToString();
              
            }
            //Calculate split - split = 500 * (time/distance)
            else if ((enteredDistance.Text != "") && (enteredSplitMin.Text == "" && enteredSplitSec.Text == "") && (enteredTimeMin.Text != "" || enteredTimeSec.Text != ""))
            {
                if (Helpers.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text) is TimeSpan parsedTotalTime && parsedTotalTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "Time has no value!", ToastLength.Short).Show();
                    return;
                }

                if(int.Parse(enteredDistance.Text) is int distance && distance == 0)
                {
                    Toast.MakeText(Activity, "Distance has no value!", ToastLength.Short).Show();
                    return;
                }

                TimeSpan splitReadable = Formulas.SplitFromTimeDistance(parsedTotalTime, distance);

                enteredSplitMin.Text = $"{(int)splitReadable.TotalMinutes}";
                enteredSplitSec.Text = $"{splitReadable.Seconds:D2}.{splitReadable.Milliseconds}";       
            }
            //Calculate total time - time = split * (distance/500)
            else if ((enteredDistance.Text != "") && (enteredSplitMin.Text != "" || enteredSplitSec.Text != "") && (enteredTimeMin.Text == "" && enteredTimeSec.Text == ""))
            {
                if (Helpers.ParseMinSecMS(enteredSplitMin.Text, enteredSplitSec.Text) is TimeSpan parsedSplitTime && parsedSplitTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "Split has no value!", ToastLength.Short).Show();
                    return;
                }

                if (int.Parse(enteredDistance.Text) is int distance && distance == 0)
                {
                    Toast.MakeText(Activity, "Distance has no value!", ToastLength.Short).Show();
                    return;
                }

                TimeSpan timeReadable = Formulas.TimeFromSplitDistance(parsedSplitTime, distance);
                enteredTimeMin.Text = $"{(int)timeReadable.TotalMinutes}";
                enteredTimeSec.Text = $"{timeReadable.Seconds:D2}.{timeReadable.Milliseconds}";
            }
            else
            {
                Toast.MakeText(Activity, "Must enter a pair of values!", ToastLength.Short).Show();
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.toolbar, menu);

            if (menu is MenuBuilder m)
            {
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.info)
            {
                Helpers.HelpDialog(Activity, "Pace", "Enter a pair of values and when you press the calculate button the third value will be returned.\n\nTo clear all data hold the calculate button.");
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
