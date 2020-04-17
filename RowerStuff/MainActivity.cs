﻿using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using RowerStuff.Fragments;
using Android.Content.PM;

namespace RowerStuff
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private HomeFragment homeFragment = new HomeFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.maintoolbar));
            SupportActionBar.Title = "Rower Stuff";

            FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, homeFragment);
            fragmentTx.Commit();
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }
    }
}
