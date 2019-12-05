using System;
using Android.Arch.Lifecycle;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using ObservableViewModel;

namespace AppTest
{
    public class ButtonFragment : Fragment
    {
        private Button buttonFragment;
        private ViewModelManager viewModelManager;
        private TimeViewModelTwo timeViewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            viewModelManager = new ViewModelManager((BaseActivity)Activity);

            timeViewModel = ((BaseActivity)Activity).GetViewModel();
            viewModelManager.AddViewModel(timeViewModel, OnSuccessTime, OnErrorTime);
        }

        private void OnErrorTime(Exception obj)
        {
            Log.Debug(GetType().Name, $"{obj}");
        }

        private void OnSuccessTime(object obj)
        {
            if (Activity != null)
            {
                Intent intent = new Intent(Activity, typeof(MainActivity));
                Activity.StartActivity(intent);
                Activity.Finish();
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_button, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            buttonFragment = view.FindViewById<Button>(Resource.Id.fragment_button);
            buttonFragment.Text = "Click me now";
            buttonFragment.Click += ButtonFragment_Click;
        }

        private void ButtonFragment_Click(object sender, EventArgs e)
        {
            timeViewModel.InitObserver();
        }
    }
}
