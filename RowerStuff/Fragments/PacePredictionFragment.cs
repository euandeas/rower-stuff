using Android.Views;
using AndroidX.AppCompat.View.Menu;
using AndroidX.CardView.Widget;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class PacePredictionFragment : Fragment
    {
        private EditText actualDistance;
        private EditText predictDistance;
        private EditText enteredSplitMin;
        private EditText enteredSplitSec;
        private TextView result;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_pace_prediction, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);
            HasOptionsMenu = true;

            CardView actualDistanceCard = view.FindViewById<CardView>(Resource.Id.actualDistanceCard);
            actualDistance = actualDistanceCard.FindViewById<EditText>(Resource.Id.enteredDistance);

            enteredSplitMin = view.FindViewById<EditText>(Resource.Id.enteredMin);
            enteredSplitSec = view.FindViewById<EditText>(Resource.Id.enteredSec);

            CardView predictDistanceCard = view.FindViewById<CardView>(Resource.Id.predictDistanceCard);
            predictDistance = predictDistanceCard.FindViewById<EditText>(Resource.Id.enteredDistance);

            result = view.FindViewById<TextView>(Resource.Id.predictionResult);

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            actualDistance.Text = "";
            predictDistance.Text = "";
            enteredSplitMin.Text = "";
            enteredSplitSec.Text = "";
            result.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
        if ((actualDistance.Text != "") && (enteredSplitMin.Text != "" || enteredSplitSec.Text != "") && (predictDistance.Text != ""))
        {
            if (int.Parse(actualDistance.Text) is int acDistance && acDistance == 0)
            {
                Toast.MakeText(Activity, "Actual distance must be greater than 0!", ToastLength.Short).Show();
                return;
            }

            if (Helpers.ParseMinSecMS(enteredSplitMin.Text, enteredSplitSec.Text) is TimeSpan parsedSplitTime && parsedSplitTime.TotalSeconds == 0)
            {
                Toast.MakeText(Activity, "Split has no value!", ToastLength.Short).Show();
                return;
            }

            if (int.Parse(predictDistance.Text) is int preDistance && preDistance == 0)
            {
                Toast.MakeText(Activity, "Distance to predict must be greater than 0!", ToastLength.Short).Show();
                return;
            }

            TimeSpan splitReadable = Formulas.PaulsLaw(acDistance, preDistance, parsedSplitTime);
            result.Text = string.Format("{0}:{1}.{2}", (int)splitReadable.TotalMinutes, splitReadable.Seconds, splitReadable.Milliseconds);
        }
        else
        {
            Toast.MakeText(Activity, "Make sure to enter all values before calculating!", ToastLength.Short).Show();
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
                Helpers.HelpDialog(Activity, "Pace Prediction", "This calculator uses Paul's Law to predict pace based on a previous results.\n\n Always follow your coaches advice first and foremost. This is only an estimate.\n\nEnter the distance and average pace of a previous result (e.g. your 2KM pace), along with the distance you want to predict for. The estimated pace for that distance will be returned.");
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
