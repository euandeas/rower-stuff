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

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.SetStatusBarBackgroundColor(Color.Transparent);

            navController = ((NavHostFragment)SupportFragmentManager.FindFragmentById(Resource.Id.nav_host_fragment)).NavController;
            appBarConfiguration = new AppBarConfiguration.Builder(navController.Graph).SetDrawerLayout(drawer).Build();
        }

        public void SetupToolBar(MaterialToolbar toolbar, string title)
        {
            SetSupportActionBar(toolbar);
            SupportActionBar.Title=title;

            NavigationUI.SetupWithNavController(toolbar, navController, appBarConfiguration);
        }
    }
}