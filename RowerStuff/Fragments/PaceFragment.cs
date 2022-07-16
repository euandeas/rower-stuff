using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class PaceFragment : Fragment
    { 

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_pace, container, false);


            return view;
        }
    }
}
