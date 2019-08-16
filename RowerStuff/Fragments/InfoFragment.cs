using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace RowerStuff.Fragments
{
    public class InfoFragment : Fragment
    {
        Android.Support.V7.App.ActionBar supportbar;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_info, container, false);

            supportbar = ((AppCompatActivity)Activity).SupportActionBar;
            supportbar.Title = "Info";
            supportbar.SetDisplayHomeAsUpEnabled(true);
            supportbar.SetDisplayShowHomeEnabled(true);

            return view;
        }
    }
}