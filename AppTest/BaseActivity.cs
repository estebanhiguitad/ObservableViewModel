using Android.App;
using Android.Arch.Lifecycle;
using Android.OS;
using Android.Support.V7.App;
using ObservableViewModel;

namespace AppTest
{
    public abstract class BaseActivity : AppCompatActivity, IActivityLifecycle
    {
        private Application.IActivityLifecycleCallbacks activityLifecycleCallbacks;
        protected ViewModelManager ViewModelManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ViewModelManager = new ViewModelManager(this);
            activityLifecycleCallbacks?.OnActivityCreated(this, savedInstanceState);

            base.OnCreate(savedInstanceState);


        }

        protected override void OnStart()
        {
            activityLifecycleCallbacks?.OnActivityStarted(this);
            base.OnStart();
        }

        protected override void OnResume()
        {
            activityLifecycleCallbacks?.OnActivityResumed(this);
            base.OnResume();
        }

        protected override void OnPause()
        {
            activityLifecycleCallbacks?.OnActivityPaused(this);
            base.OnPause();
        }

        protected override void OnStop()
        {
            activityLifecycleCallbacks?.OnActivityStopped(this);
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            activityLifecycleCallbacks?.OnActivityDestroyed(this);
            base.OnDestroy();
        }

        public void RegisterLifecycleCallback(Application.IActivityLifecycleCallbacks activityLifecycleCallbacks)
        {
            this.activityLifecycleCallbacks = activityLifecycleCallbacks;
        }

        public TimeViewModelTwo GetViewModel()
        {
            return ViewModelProviders.Of(this).Get(Java.Lang.Class.FromType(typeof(TimeViewModelTwo))) as TimeViewModelTwo;
        }
    }
}
