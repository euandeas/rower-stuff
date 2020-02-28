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
    public class SplitCalcFragment : Fragment
    {
        private EditText enteredDistance;
        private EditText enteredSplitMin;
        private EditText enteredSplitSec;
        private EditText enteredTimeMin;
        private EditText enteredTimeSec;
        private Button calculateButton;
        private ActionBar supportBar;
        private CardView distanceCard;
        private CardView splitCard;
        private CardView timeCard;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_splitcalc, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Pace";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            // add functionality here
            enteredDistance = view.FindViewById<EditText>(Resource.Id.enteredDistance);
            enteredSplitMin = view.FindViewById<EditText>(Resource.Id.enteredSplitMin);
            enteredSplitSec = view.FindViewById<EditText>(Resource.Id.enteredSplitSec);
            enteredTimeMin = view.FindViewById<EditText>(Resource.Id.enteredTimeMin);
            enteredTimeSec = view.FindViewById<EditText>(Resource.Id.enteredTimeSec);
            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            distanceCard = view.FindViewById<CardView>(Resource.Id.distanceCard);
            splitCard = view.FindViewById<CardView>(Resource.Id.splitCard);
            timeCard = view.FindViewById<CardView>(Resource.Id.timeCard);

            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            distanceCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard(enteredDistance.Text, Activity);
            splitCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard($"{enteredSplitMin.Text}:{enteredSplitSec.Text}", Activity);
            timeCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard($"{enteredTimeMin.Text}:{enteredTimeSec.Text}", Activity);

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
                CommonFunctions.HelpDialog(Activity, "Pace", "Working Out Distance:\nEnter a 500M split time and the total time taken and then press calculate.\n\nWorking Out Split:\nEnter the distance and the time taken to cover that distance and then press calculate.\n\nWorking Out Total Time:\nEnter the distance and the average split time held during that distance and then press calculate.\n\nTo copy data hold down on any card.\n\nTo clear all data hold the calculate button.");
            }

            return base.OnOptionsItemSelected(item);
        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            enteredDistance.Text = "";
            enteredSplitMin.Text = "";
            enteredSplitSec.Text = "";
            enteredTimeMin.Text = "";
            enteredTimeSec.Text = "";
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            //Calculate Distance - distance = (time/split) * 500
            if ((enteredDistance.Text == "") && (enteredSplitMin.Text != "" || enteredSplitSec.Text != "") && (enteredTimeMin.Text != "" || enteredTimeSec.Text != ""))
            {
                if ((enteredSplitSec.Text == ".") || enteredTimeSec.Text == ".")
                {
                    Toast.MakeText(Activity, "Make sure seconds values don't have only a '.' in them!", ToastLength.Short).Show();
                }
                else
                {
                    TimeSpan parsedSplitTime = CommonFunctions.ParseMinSecMS(enteredSplitMin.Text,enteredSplitSec.Text);
                    TimeSpan parsedTotalTime = CommonFunctions.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text);

                    double timeForCalc = parsedTotalTime.TotalMilliseconds / parsedSplitTime.TotalMilliseconds;
                    double distance = timeForCalc * 500;
                    if (double.IsNaN(distance))
                    {
                        enteredDistance.Text = "0";
                    }
                    else
                    {
                        enteredDistance.Text = distance.ToString();
                    }
                }
            }
            //Calculate split - split = 500 * (time/distance)
            else if ((enteredDistance.Text != "") && (enteredSplitMin.Text == "" && enteredSplitSec.Text == "" ) && (enteredTimeMin.Text != "" || enteredTimeSec.Text != ""))
            {
                if (enteredTimeSec.Text == ".")
                {
                    Toast.MakeText(Activity, "Make sure seconds value doesn't only have a '.' in it!", ToastLength.Short).Show();
                }
                else
                {
                    TimeSpan parsedTotalTime = CommonFunctions.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text);
                    double distanceAsInt = long.Parse(enteredDistance.Text);

                    if (distanceAsInt != 0)
                    {
                        double timeForCalc = parsedTotalTime.TotalMilliseconds / distanceAsInt;
                        double splitMilli = timeForCalc * 500;
                        TimeSpan splitReadable = TimeSpan.FromMilliseconds(splitMilli);

                        var splitAsStringParts = string.Format("{0}:{1}.{2}", (int)splitReadable.TotalMinutes, splitReadable.Seconds, splitReadable.Milliseconds).Split(':');
                        enteredSplitMin.Text = splitAsStringParts[0];
                        enteredSplitSec.Text = splitAsStringParts[1];
                    }
                    else
                    {
                        Toast.MakeText(Activity, "Make sure distance value is greater than 0!", ToastLength.Short).Show();
                    }

                }                
            }
            //Calculate total time - time = split * (distance/500)
            else if ((enteredDistance.Text != "") && (enteredSplitMin.Text != "" || enteredSplitSec.Text != "") && (enteredTimeMin.Text == "" && enteredTimeSec.Text == ""))
            {
                if ((enteredSplitSec.Text == "."))
                {
                    Toast.MakeText(Activity, "Make sure seconds value doesn't only have a '.' in it!", ToastLength.Long).Show();
                }
                else
                {
                    TimeSpan parsedSplitTime = CommonFunctions.ParseMinSecMS(enteredSplitMin.Text, enteredSplitSec.Text);

                    double distanceAsInt = long.Parse(enteredDistance.Text);
                    distanceAsInt = distanceAsInt / 500;

                    double totalTimeMilli = parsedSplitTime.TotalMilliseconds * distanceAsInt;

                    TimeSpan timeReadable = TimeSpan.FromMilliseconds(totalTimeMilli);
                    var timeAsStringParts = string.Format("{0}:{1}.{2}", (int)timeReadable.TotalMinutes, timeReadable.Seconds, timeReadable.Milliseconds).Split(':');
                    enteredTimeMin.Text = timeAsStringParts[0];
                    enteredTimeSec.Text = timeAsStringParts[1];
                }
            }
            else
            {
                Toast.MakeText(Activity, "Must enter two values.", ToastLength.Short).Show();
            }
        }
    }
}