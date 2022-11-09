using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Preference;
using Google.Android.Material.AppBar;

namespace RowerStuff.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public override void OnCreatePreferences(Bundle? savedInstanceState, string? rootKey)
        {
            SetPreferencesFromResource(Resource.Xml.preferences, rootKey);
            ListPreference theme = (ListPreference)FindPreference("theme_preference")!;

            theme.PreferenceChange += Theme_PreferenceChange;

            Preference mail = FindPreference("mail_preference")!;
            mail.PreferenceClick += SendFeedback;

            Preference website = FindPreference("website_preference")!;
            website.PreferenceClick += OpenWebsite;

            Preference privacy = FindPreference("privacy_preference")!;
            privacy.PreferenceClick += OpenPrivacyPolicy;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            View view = base.OnCreateView(inflater, container, savedInstanceState)!;

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar)!;
            (Activity as MainActivity)!.SetupToolBar(toolbar);

            return view;
        }

        private void Theme_PreferenceChange(object? sender, Preference.PreferenceChangeEventArgs? e)
        {
            switch (e!.NewValue!.ToString())
            {
                case "Light":
                    ((AppCompatActivity)Activity!).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                    break;
                case "Dark":
                    ((AppCompatActivity)Activity!).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                    break;
                case "System Default":
                    ((AppCompatActivity)Activity!).Delegate.SetLocalNightMode((Build.VERSION.SdkInt >= BuildVersionCodes.Q) ? AppCompatDelegate.ModeNightFollowSystem : AppCompatDelegate.ModeNightAutoBattery);
                    break;
            }
        }

        private void SendFeedback(object? sender, Preference.PreferenceClickEventArgs e)
        {
            StartNewIntent("mailto: euandeasapps@gmail.com", "No email client found!");        
        }

        private void OpenWebsite(object? sender, Preference.PreferenceClickEventArgs e)
        {
            StartNewIntent("https://euandeas.com", "No web browser found!");
        }

        private void OpenPrivacyPolicy(object? sender, Preference.PreferenceClickEventArgs e)
        {
            StartNewIntent("https://euandeas.com/privacy", "No web browser found!");
        }

        private void StartNewIntent(string url, string error)
        {
            try
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse(url)));
            }
            catch
            {
                Toast.MakeText(Activity, error, ToastLength.Short)!.Show();
            }
        }
    }
}
