using Android.OS;
using AndroidX.AppCompat.App;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace RowerStuff.Fragments
{
    public class InfoFragment : Fragment
    {
        ActionBar supportBar;

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

            return view;
        }
    }
}