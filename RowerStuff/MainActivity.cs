using Android.Graphics;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using AndroidX.Navigation.Fragment;
using AndroidX.Navigation.UI;
using AndroidX.Navigation;
using Google.Android.Material.Navigation;
using Google.Android.Material.AppBar;

namespace RowerStuff
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private NavController navController;
        private AppBarConfiguration appBarConfiguration;
        private NavigationView navView;
        private DrawerLayout drawer;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.SetStatusBarBackgroundColor(Color.Transparent);

            navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navController = ((NavHostFragment)SupportFragmentManager.FindFragmentById(Resource.Id.nav_host_fragment)).NavController;
            appBarConfiguration = new AppBarConfiguration.Builder(
                Resource.Id.paceFragment, 
                Resource.Id.wattsFragment
                ).SetOpenableLayout(drawer).Build();

            NavigationUI.SetupWithNavController(navView, navController);

        }

        public void SetupToolBar(MaterialToolbar toolbar, string title)
        {
            SetSupportActionBar(toolbar);
            SupportActionBar.Title=title;
        
            NavigationUI.SetupWithNavController(toolbar, navController, appBarConfiguration);
            NavigationUI.SetupWithNavController(navView, navController);
        }
    }
}