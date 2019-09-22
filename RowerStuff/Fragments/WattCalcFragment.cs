using System;
using System.Globalization;
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
    public class WattCalcFragment : Fragment
    {
        private EditText enteredWatts;
        private EditText enteredSplitMin;
        private EditText enteredSplitSec;
        private Button calculateButton;
        private ActionBar supportBar;
        private CardView splitCard;
        private CardView wattCard;
        private string result;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_wattcalc, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Watts";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            enteredWatts = view.FindViewById<EditText>(Resource.Id.enteredWatts);
            enteredSplitMin = view.FindViewById<EditText>(Resource.Id.enteredSplitMin);
            enteredSplitSec = view.FindViewById<EditText>(Resource.Id.enteredSplitSec);
            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            splitCard = view.FindViewById<CardView>(Resource.Id.splitCard);
            wattCard = view.FindViewById<CardView>(Resource.Id.wattCard);

            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            splitCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard($"{enteredSplitMin.Text}:{enteredSplitSec.Text}", Activity);
            wattCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard(enteredWatts.Text, Activity);

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
                CommonFunctions.HelpDialog(Activity, "Watts", "Working Out Average Watts:\nEnter your average 500M split during a piece and press calculate.\n\nWorking Out Average Pace:\nEntert your average watts during a piece and press calculate.\n\nTo copy data hold down on any card.\n\nTo clear all data hold the calculate button.");
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            //Calculate Watts - watts = 2.8/(split/500)³
            if ((enteredWatts.Text == "") && (enteredSplitMin.Text != "" || enteredSplitSec.Text != ""))
            {
                if ((enteredSplitSec.Text == "."))
                {
                    Toast.MakeText(Activity, "Make sure seconds value doesn't only have a '.' in it!", ToastLength.Long).Show();
                }
                else
                {
                    TimeSpan parsedSplitTime = CommonFunctions.ParseMinSecMS(enteredSplitMin.Text, enteredSplitSec.Text);
                    double watts = 2.8 / Math.Pow((parsedSplitTime.TotalSeconds / 500), 3);
                    watts = Math.Round(watts, 2);
                    result = watts.ToString();
                    enteredWatts.Text = result;
                }

            }
            //Calculate Split - split = (cube-root(w*2.8))*500
            else if ((enteredWatts.Text != "") && (enteredSplitMin.Text == "" || enteredSplitSec.Text == ""))
            {
                double wattsAsInt = double.Parse(enteredWatts.Text, CultureInfo.InvariantCulture);
                double splitSecs = Math.Pow((2.8/wattsAsInt), 1.0 / 3.0);
                splitSecs = splitSecs * 500;
                TimeSpan splitReadable = TimeSpan.FromSeconds(splitSecs);
                result = string.Format("{0}:{1}.{2}", (int)splitReadable.TotalMinutes, splitReadable.Seconds, splitReadable.Milliseconds);
                var SplitAsStringParts = result.Split(':');
                enteredSplitMin.Text = SplitAsStringParts[0];
                enteredSplitSec.Text = SplitAsStringParts[1];
            }
            else
            {
                Toast.MakeText(Activity, "Must enter either Split or Watts.", ToastLength.Short).Show();
            }


        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            enteredWatts.Text = "";
            enteredSplitMin.Text = "";
            enteredSplitSec.Text = "";
        }
    }
}