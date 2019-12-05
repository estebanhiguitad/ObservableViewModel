using Android.App;

namespace ObservableViewModel
{
    public interface IActivityLifecycle
    {
        void RegisterLifecycleCallback(Application.IActivityLifecycleCallbacks activityLifecycleCallbacks);
    }
}