namespace ObservableViewModel
{
    internal interface IOnResumeObserver
    {
        void OnActivityResumed(ActivityState state);
    }
}