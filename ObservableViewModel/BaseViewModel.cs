using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Android.App;

namespace ObservableViewModel
{
    public abstract class BaseViewModel<T> : Android.Arch.Lifecycle.ViewModel,
        IObserver<T>, IOnResumeObserver
    {
        public Action<T> OnNextAction;
        public Action<Exception> OnErrorAction;
        public Action OnCompleteAction;

        private ActivityState activityState;

        public StatusObserver Status { get; private set; }
        private IObservable<T> observable;

        private Thread thread;
        private T response;
        private IObserver<T> observer;

        public virtual void InitObserver()
        {
            if (thread == null || (thread.ThreadState != ThreadState.Running && thread.ThreadState != ThreadState.Background))
            {
                observable = Observable.Create(FunctionToExecute());
                observable.SubscribeOn(new NewThreadScheduler()).ObserveOn(Application.SynchronizationContext).Subscribe(this);
            }
        }

        public void OnNext(T value)
        {
            Status = StatusObserver.Ready;
            OnNextAction?.Invoke(value);
        }

        public void OnError(Exception error)
        {
            Status = StatusObserver.Failed;
            OnErrorAction?.Invoke(error);
        }

        public void OnCompleted()
        {
            OnCompleteAction?.Invoke();
        }

        protected abstract T LoadInBackground();

        protected abstract void ProcessResponse(T response, IObserver<T> observer);

        private Func<IObserver<T>, IDisposable> FunctionToExecute()
        {
            return observer =>
            {
                var cancel = new CancellationDisposable();
                this.observer = observer;

                try
                {
                    thread = Thread.CurrentThread;

                    response = LoadInBackground();

                    Status = StatusObserver.Completed;
                    ValidateResponse(activityState);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }

                return cancel;

            };
        }

        public void OnActivityResumed(ActivityState state)
        {
            activityState = state;
            ValidateResponse(state);
        }

        private void ValidateResponse(ActivityState state)
        {
            if (state == ActivityState.OnResume && Status == StatusObserver.Completed)
            {
                ProcessResponse(response, observer);
            }
        }
    }
}
