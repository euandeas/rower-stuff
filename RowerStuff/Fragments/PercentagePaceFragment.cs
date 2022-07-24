using Android.Views;
using AndroidX.AppCompat.View.Menu;
using Google.Android.Material.AppBar;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class PercentagePaceFragment : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_percentage_pace, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);
            HasOptionsMenu = true;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.toolbar, menu);

            if (menu is MenuBuilder m)
            {
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.info)
            {
                Helpers.HelpDialog(Activity, "Rate", "");
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
