﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace AppTest
{
    [Activity]
    public class SecondActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_second);
        }
    }
}
