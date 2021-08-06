using System.Collections.Generic;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace RowerStuff.Fragments
{
    public class HomeFragment : Fragment
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private ActionBar supportBar;

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
            
            mLayoutManager = new LinearLayoutManager(Activity);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.SetAdapter(new RecyclerAdapter(new List<string>() { "Pace", "Percentage", "Watts", "Steady State", "Weight Adjustment", "Prediction", "VO2 Max", "Rate" }));

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
                fragmentTx.Replace(Resource.Id.container, new InfoFragment());
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            }

            return base.OnOptionsItemSelected(item);
        }
    }

    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<string> mCalcTypes;

        public RecyclerAdapter (List<string> calcTypes)
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
                        fragment = new SplitCalcFragment();
                        break;
                    case "Watts":
                        fragment = new WattCalcFragment();
                        break;
                    case "Weight Adjustment":
                        fragment = new WeightAdjustmentFragment();
                        break;
                    case "Prediction":
                        fragment = new PredictionFragment();
                        break;
                    case "VO2 Max":
                        fragment = new VO2Fragment();
                        break;
                    case "Rate":
                        fragment = new RateToolFragment();
                        break;
                    case "Percentage":
                        fragment = new PercentageFragment();
                        break;
                    case "Steady State":
                        fragment = new SteadyStateFragment();
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
            myHolder.mCalcName.Text = mCalcTypes[position];
        }

        public override int ItemCount
        {
            get { return mCalcTypes.Count; }
        }
    }
}