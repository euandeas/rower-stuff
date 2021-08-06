using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using RowerStuff.Fragments;
using Android.Content.PM;
using AndroidX.Preference;
using Android.Content;
using Android.Runtime;

namespace RowerStuff
{
    [Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
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
                    case "System Default":
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                        {
                            base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightFollowSystem);
                        }
                        else
                        {
                            base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightAutoBattery);
                        }
                        break;
                }
            }

            SetTheme(Resource.Style.AppTheme);

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Ensures that the SDK has been initialized with our publisher app ID
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.maintoolbar));
            SupportActionBar.Title = "Rower Stuff";

            if (savedInstanceState == null)
            {
                FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, new HomeFragment());
                fragmentTx.Commit();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }
    }
}
