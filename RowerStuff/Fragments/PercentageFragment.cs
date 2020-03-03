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
    public class PercentageFragment : Fragment
    {
        private ActionBar supportBar;
        private EditText enteredWatts;
        private EditText enteredSplitMin;
        private EditText enteredSplitSec;
        private TextView percentLabel;
        private SeekBar seekBar;
        private TextView percentageAnswer;
        private TextView percentageLabel;
        private Button calculateButton;
        private CardView percentageCard;
        private CardView splitCard;
        private CardView wattCard;
        private string percentageType = "Pace";
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_percentage, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Percentage";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.percentageType);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.percentageType_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            enteredSplitMin = view.FindViewById<EditText>(Resource.Id.enteredSplitMin);
            enteredSplitSec = view.FindViewById<EditText>(Resource.Id.enteredSplitSec);
            enteredWatts = view.FindViewById<EditText>(Resource.Id.enteredWatts);
            percentLabel = view.FindViewById<TextView>(Resource.Id.percentLabel);
            seekBar = view.FindViewById<SeekBar>(Resource.Id.seekBar);
            percentageAnswer = view.FindViewById<TextView>(Resource.Id.percentageAnswer);
            percentageLabel = view.FindViewById<TextView>(Resource.Id.percentageLabel);
            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            percentageCard = view.FindViewById<CardView>(Resource.Id.percentageCard);
            splitCard = view.FindViewById<CardView>(Resource.Id.splitCard);
            wattCard = view.FindViewById<CardView>(Resource.Id.wattCard);

            wattCard.Visibility = ViewStates.Gone;
            seekBar.SetProgress(100, false);
            
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            percentageCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard($"{percentageAnswer.Text}", Activity);
            seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {                       
                    percentLabel.Text = ($"{e.Progress}%");               
            };
                
            //sends request for ad
            var adView = view.FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            return view;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string selectedSpinnerItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (selectedSpinnerItem == "Pace")
            {
                percentageType = "Pace";
                splitCard.Visibility = ViewStates.Visible;
                wattCard.Visibility = ViewStates.Gone;
                percentageLabel.Text = "Percentage Pace";
            }
            else if (selectedSpinnerItem == "Watts")
            {
                percentageType = "Watts";
                wattCard.Visibility = ViewStates.Visible;
                splitCard.Visibility = ViewStates.Gone;
                percentageLabel.Text = "Percentage Watts";
            }
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
                CommonFunctions.HelpDialog(Activity, "Percentage Pace", "Use this tool to work out the percentage of a certain pace or wattage, to help decide what split or watts to hold during a piece.\nSimply enter a split or watts, choose a percentage and then press calculate.\n\nTo copy data hold down on the percentage card.\n\nTo clear all data hold the calculate button.");
            }

            return base.OnOptionsItemSelected(item);
        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {

            enteredSplitMin.Text = "";
            enteredSplitSec.Text = "";
            enteredWatts.Text = "";
            seekBar.SetProgress(100, false);
            percentageAnswer.Text = "";

        }        
        
        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (percentageType == "Pace" && enteredSplitMin.Text != "" || enteredSplitSec.Text != "" && enteredSplitSec.Text != ".")
            {
                if (seekBar.Progress != 0)
                {
                    TimeSpan parsedSplitTime = CommonFunctions.ParseMinSecMS(enteredSplitMin.Text, enteredSplitSec.Text);
                    double percent = (Convert.ToDouble(seekBar.Progress) / 100);
                    double percentageSplitAsSec = parsedSplitTime.TotalSeconds / percent;
                    TimeSpan splitReadable = TimeSpan.FromSeconds(percentageSplitAsSec);
                    percentageAnswer.Text = string.Format("{0}:{1}.{2}", (int)splitReadable.TotalMinutes, splitReadable.Seconds, splitReadable.Milliseconds);
                }
                else
                {
                    Toast.MakeText(Activity, "Percentage cannot be 0%.", ToastLength.Short).Show();
                }
            }
            else if (percentageType == "Pace")
            {
                Toast.MakeText(Activity, "Must enter a Split.", ToastLength.Short).Show();
            }

            if (percentageType == "Watts" && enteredWatts.Text != "" && enteredWatts.Text != ".")
            {
                if (seekBar.Progress != 0)
                {
                    double percent = (Convert.ToDouble(seekBar.Progress) / 100);
                    double wattsAsInt = double.Parse(enteredWatts.Text, CultureInfo.InvariantCulture);
                    double percentageWatt = wattsAsInt * percent;
                    
                    percentageAnswer.Text = percentageWatt.ToString();
                }
                else
                {
                    Toast.MakeText(Activity, "Percentage cannot be 0%.", ToastLength.Short).Show();
                }
            }
            else if (percentageType == "Watts")
            {
                Toast.MakeText(Activity, "Must enter watts.", ToastLength.Short).Show();
            }
        }
    }
}