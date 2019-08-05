using System;

namespace ObservableViewModel
{
    public interface IViewModelManager
    {
        void AddViewModel<T>(BaseViewModel<T> viewModel, Action<T> OnNextAction, Action<Exception> OnErrorAction);
        void AddViewModel<T>(BaseViewModel<T> viewModel, Action<T> OnNextAction, Action<Exception> OnErrorAction, Action OnCompleteAction);
        void RemoveViewModel<T>(BaseViewModel<T> observable);
        void ValidateStatus<T>();
    }
}
