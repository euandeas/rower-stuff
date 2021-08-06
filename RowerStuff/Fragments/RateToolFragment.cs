using System;
using System.Diagnostics;
using Android.Gms.Ads;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using Android.Views;
using Android.Widget;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace RowerStuff.Fragments
{
    public class RateToolFragment : Fragment
    {
        private ActionBar supportBar;
        private Button tapButton;
        private TextView rateLabel;
        private Stopwatch stopWatch = new Stopwatch();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_ratetool, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Rate";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            rateLabel = view.FindViewById<TextView>(Resource.Id.rateLabel);
            tapButton = view.FindViewById<Button>(Resource.Id.tapButton);

            tapButton.Click += TapButton_Click;

            var adView = view.FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            return view;
        }

        private void TapButton_Click(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning && stopWatch.ElapsedMilliseconds < 15000)
            {
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                var result = 60 / ts.TotalSeconds;
                var spm = result * 1;
                rateLabel.Text = $"{Math.Round(spm)}";
                stopWatch.Reset();
                stopWatch.Start();
            }
            else if (stopWatch.IsRunning && stopWatch.ElapsedMilliseconds >= 15000)
            {
                stopWatch.Stop();
                stopWatch.Reset();
                stopWatch.Start();
            }
            else if (!stopWatch.IsRunning)
            {
                stopWatch.Start();
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
                CommonFunctions.HelpDialog(Activity, "Rate", "Use this tool to keep track of a rower's/crew's rate while following along from the side.\n\nSimply tap the button in time with the rowers stroke and it will display an accurate estimate of their strokes per minute.");
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}