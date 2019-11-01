using System;
using Android.Gms.Ads;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

namespace RowerStuff.Fragments
{
    public class PredictionFragment : Fragment
    {
        private ActionBar supportBar;
        private Button calculateButton;
        private EditText entAcDistance;
        private EditText entSplitMin;
        private EditText entSplitSec;
        private EditText entPreDistance;
        private TextView preAnswer;
        private CardView predictedCard;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_prediction, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Prediction";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            entAcDistance = view.FindViewById<EditText>(Resource.Id.enteredAcDistance);
            entSplitMin = view.FindViewById<EditText>(Resource.Id.enteredSplitMin);
            entSplitSec = view.FindViewById<EditText>(Resource.Id.enteredSplitSec);
            entPreDistance = view.FindViewById<EditText>(Resource.Id.enteredPreDistance);
            preAnswer = view.FindViewById<TextView>(Resource.Id.predictedAnswer);
            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            predictedCard = view.FindViewById<CardView>(Resource.Id.predictedCard);

            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            predictedCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard(preAnswer.Text, Activity);

            //sends request for ad
            var adView = view.FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.toolbar_menu, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_info)
            {
                CommonFunctions.HelpDialog(Activity, "Prediction", "Use prediction to work out what split you should hold during a pieced based on other pieces you have carried out. Always follow your coaches advice if you have one as this is an estimate and everyone is different.\n\nEnter the distance and average pace of a piece that you have carried out (2KM time is good reference) and the distance that you want to predict for, then press calculate. It will give you back an estimate of the split you should hold during the distance you wanted predicted.\n\nTo copy results hold down on the result card.\n\nTo clear all data hold the calculate button.");
            }

            return base.OnOptionsItemSelected(item);
        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            entAcDistance.Text = "";
            entSplitMin.Text = "";
            entSplitSec.Text = "";
            entPreDistance.Text = "";
            preAnswer.Text = "";
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if ((entAcDistance.Text != "" && int.Parse(entAcDistance.Text) > 0) && (entSplitMin.Text != "" || entSplitSec.Text != "") && (entPreDistance.Text != ""  && int.Parse(entPreDistance.Text) > 0))
            {
                if ((entSplitSec.Text == "."))
                {
                    Toast.MakeText(Activity, "Make sure seconds value doesn't only have a '.' in it!", ToastLength.Long).Show();
                }
                else
                {
                    double acDistanceInt = long.Parse(entAcDistance.Text);
                    double preDistanceInt = long.Parse(entPreDistance.Text);
                    double splitsToAdd = 5 * ((Math.Log(preDistanceInt / acDistanceInt)) / (Math.Log(2)));

                    TimeSpan parsedSplitTime = CommonFunctions.ParseMinSecMS(entSplitMin.Text, entSplitSec.Text);
                    double ammendedSplit = parsedSplitTime.TotalSeconds + splitsToAdd;
                    TimeSpan splitReadable = TimeSpan.FromSeconds(ammendedSplit);
                    preAnswer.Text = string.Format("{0}:{1}.{2}", (int)splitReadable.TotalMinutes, splitReadable.Seconds, splitReadable.Milliseconds);
                }
            }
            else
            {
                Toast.MakeText(Activity, "Make sure to enter all values before calculating! (Distances must be greater than 0)", ToastLength.Short).Show();
            }
        }
    }
}