using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Content;

namespace RowerStuff
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            //Ensures that the SDK has been initialized with our publisher app ID
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-6671601320564750~3044693219");

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}