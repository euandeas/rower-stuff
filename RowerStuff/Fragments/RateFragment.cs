using Android.Views;
using AndroidX.Core.View;
using AndroidX.Lifecycle;
using Google.Android.Material.AppBar;
using RowerStuff.Models;
using Fragment = AndroidX.Fragment.App.Fragment;


namespace RowerStuff.Fragments
{
    public class RateFragment : Fragment
    {
        private TextView rateLabel;
        private readonly SPM spm = new();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_rate, container, false);

            MaterialToolbar toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar);
            (Activity as MainActivity).SetupToolBar(toolbar);

            IMenuHost menuHost = RequireActivity();
            menuHost.AddMenuProvider(new Helpers.StandardInfoMenu(
                Activity,
                "Rate",
                "Simply tap the button at a reference point within the rower's/crew's stroke and then their current stroke rate will be displayed.")
                , ViewLifecycleOwner, Lifecycle.State.Resumed);

            rateLabel = view.FindViewById<TextView>(Resource.Id.rateLabel);
            Button tapButton = view.FindViewById<Button>(Resource.Id.tapButton);

            tapButton.Click += TapButton_Click;

            return view;
        }

        private void TapButton_Click(object? sender, EventArgs e)
        {
            rateLabel.Text = $"{spm.Beat()}";
        }
    }
}
