using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using ObservableViewModel;
using System;
using Android.Arch.Lifecycle;
using Android.Util;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace AppTest
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button myButton;
        private ViewModelManager viewModelManager;
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

            viewModelManager = new ViewModelManager();

            timeViewModel = ViewModelProviders.Of(this).Get(Java.Lang.Class.FromType(typeof(TimeViewModel))) as TimeViewModel;
            viewModelManager.AddViewModel(timeViewModel, OnSuccessTime, OnErrorTime);
        }

        private void MyButton_Click(object sender, EventArgs e)
        {
            timeViewModel.InitObserver();
        }

        private void OnSuccessTime(string obj)
        {
            Intent intent = new Intent(this, typeof(SecondActivity));
            StartActivity(intent);
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