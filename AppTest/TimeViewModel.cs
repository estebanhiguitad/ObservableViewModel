using System;
using System.Diagnostics;
using Android.Util;
using ObservableViewModel;

namespace AppTest
{
    internal class TimeViewModel : BaseViewModel<string>
    {
        protected override string LoadInBackground()
        {
            for (int i = 0; i < 1000; i++)
            {
                Debug.WriteLine($"Loop at: {i}");
                Log.Debug(GetType().Name, $"Loop at: {i}");
            }

            return "hecho";
        }

        protected override void ProcessResponse(string response, IObserver<string> observer)
        {
            observer.OnNext(response);
        }
    }
}