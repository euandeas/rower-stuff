using System.Collections.Generic;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace RowerStuff.Fragments
{
    public class HomeFragment : Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private List<CalcTypes> mCalcTypes;
        private ActionBar supportBar;
        private InfoFragment infoFragment = new InfoFragment();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_home, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Rower Stuff";
            supportBar.SetDisplayHomeAsUpEnabled(false);
            supportBar.SetDisplayShowHomeEnabled(false);
            HasOptionsMenu = true;

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mCalcTypes = new List<CalcTypes>();
            mCalcTypes.Add(new CalcTypes() { CalcName = "Pace" });
            mCalcTypes.Add(new CalcTypes() { CalcName = "Watts" });
            mCalcTypes.Add(new CalcTypes() { CalcName = "Weight Adjustment" });
            mCalcTypes.Add(new CalcTypes() { CalcName = "Prediction" });
            mCalcTypes.Add(new CalcTypes() { CalcName = "VO2 Max" });
            mCalcTypes.Add(new CalcTypes() { CalcName = "Rate" });
            mCalcTypes.Add(new CalcTypes() { CalcName = "Percentage" });

            mLayoutManager = new LinearLayoutManager(Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(new RecyclerAdapter(mCalcTypes));

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
                FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, infoFragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            }

            return base.OnOptionsItemSelected(item);
        }
    }

    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<CalcTypes> mCalcTypes;
        private SplitCalcFragment splitFragment = new SplitCalcFragment();
        private WattCalcFragment wattFragment = new WattCalcFragment();
        private WeightAdjustmentFragment weightFragment = new WeightAdjustmentFragment();
        private PredictionFragment predictionFragment = new PredictionFragment();
        private VO2Fragment vO2Fragment = new VO2Fragment();
        private RateToolFragment rateToolFragment = new RateToolFragment();
        private PercentageFragment percentageFragment = new PercentageFragment();

        public RecyclerAdapter (List<CalcTypes> calcTypes)
        {
            mCalcTypes = calcTypes;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mCalcName { get; set; }

            public MyView (View view) : base(view)
            {
                mMainView = view;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View menucardview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.menucardview, parent, false);

            TextView cardLabel = menucardview.FindViewById<TextView>(Resource.Id.cardLabel);

            MyView view = new MyView(menucardview) { mCalcName = cardLabel };

            menucardview.Click += delegate
            {
                Fragment fragment = null;

                switch (cardLabel.Text)
                {
                    case "Pace":
                        fragment = splitFragment;
                        break;
                    case "Watts":
                        fragment = wattFragment;
                        break;
                    case "Weight Adjustment":
                        fragment = weightFragment;
                        break;
                    case "Prediction":
                        fragment = predictionFragment;
                        break;
                    case "VO2 Max":
                        fragment = vO2Fragment;
                        break;
                    case "Rate":
                        fragment = rateToolFragment;
                        break;
                    case "Percentage":
                        fragment = percentageFragment;
                        break;
                }

                FragmentTransaction fragmentTx = ((AppCompatActivity)menucardview.Context).SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, fragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            };

            return view;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mCalcName.Text = mCalcTypes[position].CalcName;
        }

        public override int ItemCount
        {
            get { return mCalcTypes.Count; }
        }
    }

    public class CalcTypes
    {
        public string CalcName { get; set; }
    }
}