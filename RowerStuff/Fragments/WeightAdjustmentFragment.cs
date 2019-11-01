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
    public class WeightAdjustmentFragment : Fragment
    {
        private Button calculateButton;
        private EditText enteredWeight;
        private EditText enteredTimeMin;
        private EditText enteredTimeSec;
        private EditText enteredDistance;
        private TextView adjustedLabel;
        private TextView adjustedAnswer;
        private CardView adjustedCard;
        private string whatUnit;
        private ActionBar supportBar;
        private string result;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_weightadjustment, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Weight Adjustment";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.weightUnit);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.weightUnits_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            enteredWeight = view.FindViewById<EditText>(Resource.Id.enteredWeight);
            enteredDistance = view.FindViewById<EditText>(Resource.Id.enteredDistance);
            enteredTimeMin = view.FindViewById<EditText>(Resource.Id.enteredTimeMin);
            enteredTimeSec = view.FindViewById<EditText>(Resource.Id.enteredTimeSec);
            adjustedLabel = view.FindViewById<TextView>(Resource.Id.adjustedLabel);
            adjustedAnswer = view.FindViewById<TextView>(Resource.Id.adjustedAnswer);
            adjustedCard = view.FindViewById<CardView>(Resource.Id.adjustedCard);

            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;
            adjustedCard.LongClick += (s, e) => CommonFunctions.CopyToClipBoard(adjustedLabel.Text, Activity);

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
                CommonFunctions.HelpDialog(Activity, "Weight Adjustment", "Enter your body weight (either lbs or kgs) and then enter either the total time or distance of a piece. Click calculate and it will give you the adjusted result corresponding to what you entered.\n\nTo copy results hold down on the results card.\n\nTo clear all data hold the calculate button.");
            }
            return base.OnOptionsItemSelected(item);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string selectedSpinnerItem = spinner.GetItemAtPosition(e.Position).ToString();
            if (selectedSpinnerItem == "kg")
            {
                whatUnit = "kg";
            }
            else if (selectedSpinnerItem == "lb")
            {
                whatUnit = "lb";
            }
        }

        private void CalculateButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            enteredWeight.Text = "";
            enteredTimeMin.Text = "";
            enteredTimeSec.Text = "";
            enteredDistance.Text = "";
            adjustedLabel.Text = "Adjusted ...";
            adjustedAnswer.Text = "";
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            double weightlb = 0;

            if (enteredWeight.Text != "" && enteredWeight.Text != ".")
            {
                if (whatUnit == "kg")
                {
                    weightlb = kgTolb(float.Parse(enteredWeight.Text));
                }
                else if (whatUnit == "lb")
                {
                    weightlb = float.Parse(enteredWeight.Text);
                }

                //Corrected time = Wf x actual time(seconds)
                if ((enteredWeight.Text != "") && (enteredTimeMin.Text != "" || enteredTimeSec.Text != "") && (enteredDistance.Text == ""))
                {
                    TimeSpan parsedTotalTime = CommonFunctions.ParseMinSecMS(enteredTimeMin.Text, enteredTimeSec.Text);
                    var Wf = WeightFactor(weightlb);
                    double correctedTime = Wf * parsedTotalTime.TotalSeconds;
                    TimeSpan timeReadable = TimeSpan.FromSeconds(correctedTime);
                    var TimeAsString = string.Format("{0}:{1}.{2}", (int)timeReadable.TotalMinutes, timeReadable.Seconds, timeReadable.Milliseconds);
                    adjustedLabel.Text = "Adjusted Time";
                    adjustedAnswer.Text = TimeAsString;
                    result = TimeAsString;
                }
                //Corrected distance = actual distance / Wf
                else if ((enteredWeight.Text != "") && (enteredDistance.Text != "") && (enteredTimeMin.Text == "" && enteredTimeSec.Text == ""))
                {
                    var Wf = WeightFactor(weightlb);
                    double distanceAsInt = long.Parse(enteredDistance.Text);
                    double correctedDistance = distanceAsInt / Wf;
                    adjustedLabel.Text = "Adjusted Distance";
                    result = correctedDistance.ToString();
                    adjustedAnswer.Text = result;
                }
                else
                {
                    Toast.MakeText(Activity, "Must enter either Time Or Distance.", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(Activity, "Must enter Body Weight.", ToastLength.Short).Show();
            }
        }

        private static double WeightFactor(double bodyWeight)
        {
            return Math.Pow(bodyWeight / 270, 0.222);
        }

        private static double kgTolb(double bodyWeightKG)
        {
            return bodyWeightKG / 0.45359237;
        }
    }
}