﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
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

            SetSupportActionBar(FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.maintoolbar));
            SupportActionBar.Title = "Rower Stuff";

            if (savedInstanceState == null)
            { 
                FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, homeFragment);
                fragmentTx.Commit();
            } 
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }
    }
}
