using Android.App;
using Android.OS;
using Android.Runtime;
using ObservableViewModel;
using System;
using Android.Arch.Lifecycle;
using Android.Util;
using Android.Content;
using Android.Widget;

namespace AppTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : BaseActivity
    {
        private Button myButton;
        private TimeViewModel timeViewModel;
        private readonly string tag;

        public MainActivity()
        {
            tag = GetType().Name;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            myButton = FindViewById<Button>(Resource.Id.myButton);
            myButton.Click += MyButton_Click;

            timeViewModel = ViewModelProviders.Of(this).Get(Java.Lang.Class.FromType(typeof(TimeViewModel))) as TimeViewModel;
            ViewModelManager.AddViewModel(timeViewModel, OnSuccessTime, OnErrorTime);
        }

        private void MyButton_Click(object sender, EventArgs e)
        {
            timeViewModel.InitObserver();
        }

        private void OnSuccessTime(string obj)
        {
            Intent intent = new Intent(this, typeof(SecondActivity));
            StartActivity(intent);
            Finish();
        }

        private void OnErrorTime(Exception obj)
        {
            Log.Debug(tag, $"{obj}");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}