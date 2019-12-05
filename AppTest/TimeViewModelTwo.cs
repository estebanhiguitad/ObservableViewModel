using System;
using System.Diagnostics;
using Android.Util;
using ObservableViewModel;

namespace AppTest
{
    public class TimeViewModelTwo : BaseViewModel<string>
    {
        protected override string LoadInBackground()
        {
            for (int i = 0; i < 1000; i++)
            {
                Debug.WriteLine($"Loop at two: {i}");
                Log.Debug(GetType().Name, $"Loop at: {i}");
            }

            return "hecho";
        }

        protected override void ProcessResponse(string response, IObserver<string> observer)
        {
            observer.OnNext(response);
            observer.OnCompleted();
        }
    }
}