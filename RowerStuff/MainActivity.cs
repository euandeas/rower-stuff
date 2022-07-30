using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using AndroidX.Preference;
using Google.Android.Material.AppBar;
using Google.Android.Material.Navigation;
using SplashScreenX = AndroidX.Core.SplashScreen.SplashScreen;

namespace RowerStuff
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.App.Starting", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private NavController navController;
        private AppBarConfiguration appBarConfiguration;
        private NavigationView navView;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            if (savedInstanceState == null)
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                switch (prefs.GetString("theme_preference", "sysdefault"))
                {
                    case "light":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                        break;
                    case "dark":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                        break;
                    case "sysdefault":
                        base.Delegate.SetLocalNightMode((Build.VERSION.SdkInt >= BuildVersionCodes.Q) ? AppCompatDelegate.ModeNightFollowSystem : AppCompatDelegate.ModeNightAutoBattery);
                        break;
                }
            }

            SplashScreenX.InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.SetStatusBarBackgroundColor(Color.Transparent);

            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navController = ((NavHostFragment)SupportFragmentManager.FindFragmentById(Resource.Id.nav_host_fragment)).NavController;
            appBarConfiguration = new AppBarConfiguration.Builder(
                Resource.Id.paceFragment, 
                Resource.Id.wattsFragment,
                Resource.Id.weightAdjustmentFragment,
                Resource.Id.percentagePaceFragment,
                Resource.Id.percentageWattsFragment,
                Resource.Id.steadyStateFragment,
                Resource.Id.pacePredictionFragment,
                Resource.Id.vo2MaxFragment,
                Resource.Id.rateFragment,
                Resource.Id.settingsFragment
                ).SetOpenableLayout(drawer).Build();

            NavigationUI.SetupWithNavController(navView, navController);
        }

        public void SetupToolBar(MaterialToolbar toolbar)
        {
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Pace";

            NavigationUI.SetupWithNavController(toolbar, navController, appBarConfiguration);
        }

    }
}