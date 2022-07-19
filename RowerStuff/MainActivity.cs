using Android.Graphics;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;

namespace RowerStuff
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.SetStatusBarBackgroundColor(Color.Transparent);
        }
    }
}