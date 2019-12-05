using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;

namespace ObservableViewModel
{
    public class ViewModelManager : Java.Lang.Object, IViewModelManager, Application.IActivityLifecycleCallbacks
    {
        private readonly List<object> viewModels;
        private int finishedViewModels;
        private IActivityLifecycle lifecycle;
        private ActivityState activityState;

        private IReceptor Receptor { get; }

        public ViewModelManager(IActivityLifecycle lifecycle)
        {
            this.lifecycle = lifecycle;
            lifecycle.RegisterLifecycleCallback(this);
            viewModels = new List<object>();
        }

        public ViewModelManager(IActivityLifecycle lifecycle, IReceptor receptor) : this(lifecycle)
        {
            Receptor = receptor;
        }

        public void AddViewModel<T>(BaseViewModel<T> viewModel, Action<T> OnNextAction, Action<Exception> OnErrorAction, Action OnCompleteAction)
        {
            if (!viewModels.Contains(viewModel as BaseViewModel<object>))
            {
                viewModel.OnActivityResumed(activityState);
                ValidateEvents(viewModel, OnNextAction, OnErrorAction, OnCompleteAction);

                viewModels.Add(viewModel);
            }
        }

        public void AddViewModel<T>(BaseViewModel<T> viewModel, Action<T> OnNextAction, Action<Exception> OnErrorAction)
        {
            AddViewModel(viewModel, OnNextAction, OnErrorAction, null);
        }

        public void RemoveViewModel<T>(BaseViewModel<T> viewModel)
        {
            if (viewModels.Contains(viewModel as BaseViewModel<object>))
            {
                viewModels.Remove(viewModel as BaseViewModel<object>);
            }
        }

        public void ValidateStatus<T>()
        {
            for (int i = 0; i < viewModels.Count; i++)
            {
                var model = viewModels[i] as BaseViewModel<T>;
                if (model.Status == StatusObserver.Ready)
                {
                    finishedViewModels++;
                }

                if (finishedViewModels == viewModels.Count)
                {
                    Receptor.Complete();
                }
            }
        }

        private void ValidateEvents<T>(BaseViewModel<T> viewModel, Action<T> OnNextAction, Action<Exception> OnErrorAction, Action OnCompleteAction)
        {
            if (viewModel.OnNextAction == null)
            {
                viewModel.OnNextAction += OnNextAction;
            }
            else
            {
                viewModel.OnNextAction = null;
                viewModel.OnNextAction += OnNextAction;
            }

            if (viewModel.OnErrorAction == null)
            {
                viewModel.OnErrorAction += OnErrorAction;
            }
            else
            {
                viewModel.OnErrorAction = null;
                viewModel.OnErrorAction += OnErrorAction;
            }

            if (viewModel.OnCompleteAction == null)
            {
                viewModel.OnCompleteAction += OnCompleteAction;
            }
            else
            {
                viewModel.OnCompleteAction = null;
                viewModel.OnCompleteAction += OnCompleteAction;
            }
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            activityState = ActivityState.OnCreate;
            NotifyLifecycleChange();
        }

        public void OnActivityDestroyed(Activity activity)
        {
            // Not implemented
        }

        public void OnActivityPaused(Activity activity)
        {
            activityState = ActivityState.OnPause;
            NotifyLifecycleChange();
        }


        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            // Not Implemented
        }

        public void OnActivityStarted(Activity activity)
        {
            activityState = ActivityState.OnStart;
            NotifyLifecycleChange();
        }

        public void OnActivityStopped(Activity activity)
        {
            activityState = ActivityState.OnStop;
            NotifyLifecycleChange();
        }

        public void OnActivityResumed(Activity activity)
        {
            activityState = ActivityState.OnResume;
            NotifyLifecycleChange();
        }

        private void NotifyLifecycleChange()
        {
            foreach (var viewModel in viewModels)
            {
                (viewModel as IOnResumeObserver).OnActivityResumed(activityState);
            }
        }
    }
}
