using Android.App;
using Android.OS;
using Android.Support.V7.App;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using RowerStuff.Fragments;
using Android.Content.PM;
using Android.Preferences;
using Android.Content;

namespace RowerStuff
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private HomeFragment homeFragment = new HomeFragment();
        private ISharedPreferences prefs = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (savedInstanceState == null)
            {
                prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                switch (prefs.GetString("theme", "Light"))
                {
                    case "Light":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                        break;
                    case "Dark":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                        break;
                    case "System Preference":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightFollowSystem);
                        break;
                }
            }

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.maintoolbar));
            SupportActionBar.Title = "Rower Stuff";

            if (savedInstanceState == null)
            { 
                FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, homeFragment);
                fragmentTx.Commit();
            } 
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }
    }
}
