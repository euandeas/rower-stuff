using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Preference;
using Google.Android.Material.AppBar;

namespace RowerStuff.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            SetPreferencesFromResource(Resource.Xml.preferences, rootKey);
            ListPreference theme = (ListPreference)FindPreference("theme_preference");

            theme.PreferenceChange += Theme_PreferenceChange;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            View view = base.OnCreateView(inflater, container, savedInstanceState);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);


            return view;
        }

        private void Theme_PreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            switch (e.NewValue.ToString())
            {
                case "Light":
                    ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                    break;
                case "Dark":
                    ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                    break;
                case "System Default":
                    ((AppCompatActivity)Activity).Delegate.SetLocalNightMode((Build.VERSION.SdkInt >= BuildVersionCodes.Q) ? AppCompatDelegate.ModeNightFollowSystem : AppCompatDelegate.ModeNightAutoBattery);
                    break;
            }
        }
    }
}
