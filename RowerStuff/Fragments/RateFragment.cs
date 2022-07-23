using Android.Views;
using AndroidX.AppCompat.View.Menu;
using Google.Android.Material.AppBar;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class RateFragment : Fragment
    {
        private TextView rateLabel;
        private Models.SPM spm = new();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_rate, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);
            HasOptionsMenu = true;

            rateLabel = view.FindViewById<TextView>(Resource.Id.rateLabel);
            Button tapButton = view.FindViewById<Button>(Resource.Id.tapButton);

            tapButton.Click += TapButton_Click;

            return view;
        }

        private void TapButton_Click(object? sender, EventArgs e)
        {
            rateLabel.Text = $"{spm.Beat()}";
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.toolbar, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.info)
            {
                Helpers.HelpDialog(Activity, "Rate", "Use this tool to measure the stroke rate of a rower/crew.", "Simply tap the button at a reference point within the rower's/crew's stroke and then their current stroke rate will be displayed.");
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
