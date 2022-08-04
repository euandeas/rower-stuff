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
    public class WeightAdjustmentFragment : Fragment
    {
        private EditText enteredBodyWeight;
        private Spinner weightSpinner;
        private EditText enteredMin;
        private EditText enteredSec;
        private EditText enteredDistance;
        private TextView resultLabel;
        private TextView result;

        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_weight_adjustment, container, false)!;

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar)!;
            (Activity as MainActivity)!.SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "Weight Adjustment",
                "Enter a body weight and then enter either the total time or distance of a piece. The adjusted result corresponding to what you entered will be returned.\n\nTo clear specific data hold on the specific card.\nTo clear all data hold the calculate button.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed!);

            CardView bodyWeightCard = view.FindViewById<CardView>(Resource.Id.bodyWeightCard)!;
            enteredBodyWeight = view.FindViewById<EditText>(Resource.Id.enteredBodyWeight)!;
            bodyWeightCard.LongClick += (s, e) => enteredBodyWeight.Text = "";

            weightSpinner = view.FindViewById<Spinner>(Resource.Id.weightUnitSpinner)!;
            ArrayAdapter wAdapter = ArrayAdapter.CreateFromResource(view.Context!, Resource.Array.weight_units_array, Android.Resource.Layout.SimpleSpinnerItem);
            wAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            weightSpinner.Adapter = wAdapter;

            CardView timeDistanceCard = view.FindViewById<CardView>(Resource.Id.timeDistanceCard)!;
            enteredMin = view.FindViewById<EditText>(Resource.Id.enteredMin)!;
            enteredSec = view.FindViewById<EditText>(Resource.Id.enteredSec)!; 
            enteredDistance = view.FindViewById<EditText>(Resource.Id.enteredDistance)!;
            timeDistanceCard.LongClick += (s, e) => { enteredMin.Text = ""; enteredSec.Text = ""; enteredDistance.Text = ""; };

            resultLabel = view.FindViewById<TextView>(Resource.Id.adjustedResultLabel)!;
            result = view.FindViewById<TextView>(Resource.Id.adjustedResult)!;

            Button calculateButton = view.FindViewById<Button>(Resource.Id.calculateButton)!;
            calculateButton.Click += CalculateButton_Click;
            calculateButton.LongClick += CalculateButton_LongClick;

            return view;
        }

        private void CalculateButton_LongClick(object? sender, View.LongClickEventArgs e)
        {
            enteredBodyWeight.Text = "";
            enteredMin.Text = "";
            enteredSec.Text = "";
            enteredDistance.Text = "";
            result.Text = "";
        }

        private void CalculateButton_Click(object? sender, EventArgs e)
        {
            if (enteredBodyWeight.Text != "" && enteredBodyWeight.Text != "." && enteredBodyWeight.Text != ".")
            {
                double weight;

                if (weightSpinner.SelectedItem!.ToString() == "kg")
                {
                    if (double.Parse(enteredBodyWeight.Text!, CultureInfo.InvariantCulture) is double weightkg && weightkg == 0)
                    {
                        Toast.MakeText(Activity, "Weight has no value!", ToastLength.Short)!.Show();
                        return;
                    }

                    weight = Formulas.KGToLB(weightkg);
                }
                else // if lb selected
                {
                    if (double.Parse(enteredBodyWeight.Text!, CultureInfo.InvariantCulture) is double weightlb && weightlb == 0)
                    {
                        Toast.MakeText(Activity, "Weight has no value!", ToastLength.Short)!.Show();
                        return;
                    }

                    weight = weightlb;
                }

                //Corrected time = Wf x actual time(seconds)
                if ((enteredMin.Text != "" || enteredSec.Text != "") && (enteredDistance.Text == ""))
                {
                    if (Helpers.ParseMinSecMS(enteredMin.Text!, enteredSec.Text!) is TimeSpan parsedSplitTime && parsedSplitTime.TotalSeconds == 0)
                    {
                        Toast.MakeText(Activity, "Split has no value!", ToastLength.Short)!.Show();
                        return;
                    }

                    TimeSpan timeReadable = Formulas.TimeWeightAdjusted(weight, parsedSplitTime);
                    resultLabel.Text = "Adjusted Time";
                    result.Text = string.Format("{0}:{1}.{2}", (int)timeReadable.TotalMinutes, timeReadable.Seconds, timeReadable.Milliseconds);
                }
                //Corrected distance = actual distance / Wf
                else if ((enteredDistance.Text != "") && (enteredMin.Text == "" && enteredSec.Text == ""))
                {
                    if (int.Parse(enteredDistance.Text!) is int distance && distance == 0)
                    {
                        Toast.MakeText(Activity, "Distance has no value!", ToastLength.Short)!.Show();
                        return;
                    }

                    resultLabel.Text = "Adjusted Distance";
                    result.Text = Formulas.DistanceWeightAdjusted(weight, distance).ToString();
                }
                else
                {
                    Toast.MakeText(Activity, "Must enter either Time Or Distance.", ToastLength.Short)!.Show();
                }
            }
            else
            {
                Toast.MakeText(Activity, "Must enter body weight!", ToastLength.Short)!.Show();
            }
        }
    }
}
