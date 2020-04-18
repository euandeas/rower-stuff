using Android.OS;
using AndroidX.AppCompat.App;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using Android.Content;
using AndroidX.Preference;
using Android.Widget;
using System;

namespace RowerStuff.Fragments
{
    public class InfoFragment : Fragment
    {
        ActionBar supportBar;
        private ISharedPreferences prefs = null;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_info, container, false);

            supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = "Info";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.themeSelect);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.theme_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            switch (prefs.GetString("theme", "Light"))
            {
                case "Light":
                    spinner.SetSelection(0);
                    break;
                case "Dark":
                    spinner.SetSelection(1);
                    break;
                case "System Default":
                    spinner.SetSelection(2);
                    break;
            }

            return view;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string selectedSpinnerItem = spinner.GetItemAtPosition(e.Position).ToString();
            string chosenTheme = prefs.GetString("theme", "Light");

            if (selectedSpinnerItem != chosenTheme)
            {
                switch (selectedSpinnerItem)
                {
                    case "Light":
                        ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                        prefs.Edit().PutString("theme", "Light").Commit();
                        break;
                    case "Dark":
                        ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                        prefs.Edit().PutString("theme", "Dark").Commit();
                        break;
                    case "System Default":
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                        {
                            ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightFollowSystem);
                        }
                        else
                        {
                            ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightAutoBattery);
                        }
                        prefs.Edit().PutString("theme", "System Default").Commit();
                        break;
                }
            }
        }
    }
}