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
    public class VO2Fragment : Fragment
    {
        private ActionBar supportBar;
        private string weightUnit;
        private string trainingUnit;
        private string genderUnit;
        private Button calculateButton;
        private EditText enteredWeight;
        private EditText enteredTimeMin;
        private EditText enteredTimeSec;
        private TextView vo2Answer;
        private CardView vo2Card;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_vo2, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "VO2 Max";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            //weight spinner
            Spinner weightSpinner = view.FindViewById<Spinner>(Resource.Id.weightUnit);

            weightSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(weightSpinner_ItemSelected);
            var wAdapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.weightUnits_array, Android.Resource.Layout.SimpleSpinnerItem);

            wAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            weightSpinner.Adapter = wAdapter;

            //gender spinner
            Spinner genderSpinner = view.FindViewById<Spinner>(Resource.Id.genderType);

            genderSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(genderSpinner_ItemSelected);
            var gAdapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.gender_array, Android.Resource.Layout.SimpleSpinnerItem);

            gAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            genderSpinner.Adapter = gAdapter;

            //training spinner
            Spinner trainingSpinner = view.FindViewById<Spinner>(Resource.Id.trainingLevel);

            trainingSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(trainingSpinner_ItemSelected);
            var tAdapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.trainingLvl_array, Android.Resource.Layout.SimpleSpinnerItem);

            tAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            trainingSpinner.Adapter = tAdapter;

            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            enteredWeight = view.FindViewById<EditText>(Resource.Id.enteredWeight);
            enteredTimeMin = view.FindViewById<EditText>(Resource.Id.enteredTimeMin);
            enteredTimeSec = view.FindViewById<EditText>(Resource.Id.enteredTimeSec);
            vo2Answer = view.FindViewById<TextView>(Resource.Id.vo2Answer);
            vo2Card = view.FindViewById<CardView>(Resource.Id.vo2Card);

            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            vo2Card.LongClick += (s, e) => CommonFunctions.CopyToClipBoard(vo2Answer.Text, Activity);

            //sends request for ad
            var adView = view.FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            return view;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            double weightkg = 0;
            double Y = 0;

            if (((enteredWeight.Text != "") && (enteredWeight.Text != ".")) && (enteredTimeMin.Text != "" || enteredTimeSec.Text != ""))
            {
                if (enteredTimeSec.Text != ".")
                {
                    double timeMinutes = CommonFunctions.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text).TotalMinutes;

                    if (weightUnit == "lb")
                    {
                        weightkg = float.Parse(enteredWeight.Text) * 0.45359237;
                    }
                    else if (weightUnit == "kg")
                    {
                        weightkg = float.Parse(enteredWeight.Text);
                    }

                    if (genderUnit == "Male")
                    {
                        if (trainingUnit == "Low")
                        {
                            Y = 10.7 - (0.9 * timeMinutes);
                        }
                        else if (trainingUnit == "High")
                        {
                            if (weightkg > 75)
                            {
                                Y = 15.7 - (1.5 * timeMinutes);
                            }
                            else if (weightkg <= 75)
                            {
                                Y = 15.1 - (1.5 * timeMinutes);
                            }
                        }
                    }
                    else if (genderUnit == "Female")
                    {
                        if (trainingUnit == "Low")
                        {
                            Y = 10.26 - (0.93 * timeMinutes);
                        }
                        else if (trainingUnit == "High")
                        {
                            if (weightkg > 61.36)
                            {
                                Y = 14.9 - (1.5 * timeMinutes);
                            }
                            else if (weightkg <= 61.36)
                            {
                                Y = 14.6 - (1.5 * timeMinutes);
                            }
                        }
                    }

                    vo2Answer.Text = $"{Math.Round(((Y * 1000) / weightkg),2)} ml/(kg*min)";
                }
                else
                {
                    Toast.MakeText(Activity, "Make sure seconds value doesn't only have a '.' in it!", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(Activity, "Make sure all values have been entered.", ToastLength.Short).Show();
            }
        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            enteredWeight.Text = "";
            enteredTimeMin.Text = "";
            enteredTimeSec.Text = "";
            vo2Answer.Text = "";
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
                CommonFunctions.HelpDialog(Activity, "VO2 Max", "This is just an estimate, proper testing will always give more accurate results.\n\nEnter your body weight (either lbs or kgs) and then enter your 2KM personal best. Then select your gender and your training level (High is recommended for elite, national team and top college rowers. Low is recommended for novices). Press calculate to get your result.\n\nTo copy results hold down on the result card.\n\nTo clear all data hold the calculate button.");
            }
            return base.OnOptionsItemSelected(item);
        }

        private void weightSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string selectedSpinnerItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (selectedSpinnerItem == "kg")
            {
                weightUnit = "kg";
            }
            else if (selectedSpinnerItem == "lb")
            {
                weightUnit = "lb";
            }
        }

        private void genderSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string selectedSpinnerItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (selectedSpinnerItem == "Male")
            {
                genderUnit = "Male";
            }
            else if (selectedSpinnerItem == "Female")
            {
                genderUnit = "Female";
            }
        }

        private void trainingSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string selectedSpinnerItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (selectedSpinnerItem == "High")
            {
                trainingUnit = "High";
            }
            else if (selectedSpinnerItem == "Low")
            {
                trainingUnit = "Low";
            }
        }
    }
}