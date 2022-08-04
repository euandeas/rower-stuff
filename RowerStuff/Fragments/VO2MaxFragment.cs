using Android.Views;
using AndroidX.CardView.Widget;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using System.Globalization;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class VO2MaxFragment : Fragment
    {
        private EditText enteredBodyWeight;
        Spinner weightSpinner;
        private EditText enteredMin;
        private EditText enteredSec;
        Spinner genderSpinner;
        Spinner trainingSpinner;
        private TextView result;


        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_vo2_max, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "VO2 Max",
                "This will estimate you VO2 Max. VO2 max is a measure of the maximum amount of oxygen your body can utilize during exercise. Real world testing will always give more accurate results.\n\nTo clear all data hold the calculate button.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed);

            CardView bodyWeightCard = view.FindViewById<CardView>(Resource.Id.bodyWeightCard);
            enteredBodyWeight = view.FindViewById<EditText>(Resource.Id.enteredBodyWeight);
            bodyWeightCard.LongClick += (s, e) => enteredBodyWeight.Text = "";

            weightSpinner = view.FindViewById<Spinner>(Resource.Id.weightUnitSpinner);
            ArrayAdapter wAdapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.weight_units_array, Android.Resource.Layout.SimpleSpinnerItem);
            wAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            weightSpinner.Adapter = wAdapter;

            CardView splitCard = view.FindViewById<CardView>(Resource.Id.twokTimeCard);
            enteredMin = view.FindViewById<EditText>(Resource.Id.enteredMin);
            enteredSec = view.FindViewById<EditText>(Resource.Id.enteredSec);
            splitCard.LongClick += (s, e) => { enteredMin.Text = ""; enteredSec.Text = ""; };

            genderSpinner = view.FindViewById<Spinner>(Resource.Id.genderSpinner);
            ArrayAdapter gAdapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.gender_array, Android.Resource.Layout.SimpleSpinnerItem);
            gAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            genderSpinner.Adapter = gAdapter;

            trainingSpinner = view.FindViewById<Spinner>(Resource.Id.traingLevelSpinner);
            ArrayAdapter tAdapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.training_level_array, Android.Resource.Layout.SimpleSpinnerItem);
            tAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            trainingSpinner.Adapter = tAdapter;

            result = view.FindViewById<TextView>(Resource.Id.vo2Result);

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredBodyWeight.Text = "";
            enteredMin.Text = "";
            enteredSec.Text = "";
            result.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            if ((enteredBodyWeight.Text != "") && (enteredMin.Text != "" || enteredSec.Text != ""))
            {
                double weight;

                if (weightSpinner.SelectedItem.ToString() == "lb")
                {
                    if (double.Parse(enteredBodyWeight.Text, CultureInfo.InvariantCulture) is double weightlb && weightlb == 0)
                    {
                        Toast.MakeText(Activity, "Weight has no value!", ToastLength.Short).Show();
                        return;
                    }

                    weight = Formulas.LBToKG(weightlb);
                }
                else // if kg selected
                {
                    if (double.Parse(enteredBodyWeight.Text, CultureInfo.InvariantCulture) is double weightkg && weightkg == 0)
                    {
                        Toast.MakeText(Activity, "Weight has no value!", ToastLength.Short).Show();
                        return;
                    }

                    weight = weightkg;
                }

                if (Helpers.ParseMinSecMS(enteredMin.Text, enteredSec.Text) is TimeSpan parsedTotalTime && parsedTotalTime.TotalSeconds == 0)
                {
                    Toast.MakeText(Activity, "2KM PB time has no value!", ToastLength.Short).Show();
                    return;
                }

                result.Text = $"{Math.Round(Formulas.VO2Max(weight, genderSpinner.SelectedItem.ToString(), trainingSpinner.SelectedItem.ToString(), parsedTotalTime), 2)} ml/(kg*min)";
            }
            else
            {
                Toast.MakeText(Activity, "Make sure all values have been entered.", ToastLength.Short).Show();
            }
        }
    }
}
