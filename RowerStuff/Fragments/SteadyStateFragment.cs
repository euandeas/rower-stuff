using Android.Gms.Ads;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using Fragment = Android.Support.V4.App.Fragment;

namespace RowerStuff.Fragments
{
    public class SteadyStateFragment : Fragment
    {
        private ActionBar supportBar;
        private Button calculateButton;
        private EditText enteredTimeMin;
        private EditText enteredTimeSec;
        private TextView paceRangeAnswer;
        private CardView paceRangeCard;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_steadystate, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Steady State";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            enteredTimeMin = view.FindViewById<EditText>(Resource.Id.enteredTimeMin);
            enteredTimeSec = view.FindViewById<EditText>(Resource.Id.enteredTimeSec);
            paceRangeAnswer = view.FindViewById<TextView>(Resource.Id.paceRangeAnswer);
            paceRangeCard = view.FindViewById<CardView>(Resource.Id.paceRangeCard);

            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            paceRangeCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard(paceRangeAnswer.Text, Activity);

            //sends request for ad
            var adView = view.FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            return view;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if ((enteredTimeMin.Text != "" || enteredTimeSec.Text != ""))
            {
                TimeSpan parsedTotalTime = CommonFunctions.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text);
                double avgSplitSeconds = parsedTotalTime.TotalSeconds / 4;
                double avgWatts = 2.8 / Math.Pow((avgSplitSeconds / 500), 3);

                double avgWatts50 = avgWatts * 0.5;
                double avgWatts60 = avgWatts * 0.6;

                double splitSecs = Math.Pow((2.8 / avgWatts50), 1.0 / 3.0);
                splitSecs = splitSecs * 500;
                TimeSpan splitReadable50 = TimeSpan.FromSeconds(splitSecs);

                splitSecs = Math.Pow((2.8 / avgWatts60), 1.0 / 3.0);
                splitSecs = splitSecs * 500;
                TimeSpan splitReadable60 = TimeSpan.FromSeconds(splitSecs);

                var Split50AsString = string.Format("{0}:{1}.{2}", (int)splitReadable50.TotalMinutes, splitReadable50.Seconds, splitReadable50.Milliseconds);
                var Split60AsString = string.Format("{0}:{1}.{2}", (int)splitReadable60.TotalMinutes, splitReadable60.Seconds, splitReadable60.Milliseconds);

                paceRangeAnswer.Text = $"{Split60AsString}  -  {Split50AsString}";
            }
            else
            {
                Toast.MakeText(Activity, "Make sure to enter a time!", ToastLength.Short).Show();
            }
        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            enteredTimeMin.Text = "";
            enteredTimeSec.Text = "";
            paceRangeAnswer.Text = "";
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
                CommonFunctions.HelpDialog(Activity, "Steady State", "Enter your 2KM personal best and this will give you the pace range you should hold for steady state work. This pace range is 50-60% of your avg 2KM wattage. Only use this as guidance and remember to listen to your coaches advice firsts. \n\nTo copy results hold down on the result card.\n\nTo clear all data hold the calculate button.");
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}