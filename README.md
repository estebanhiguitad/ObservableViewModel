# ObservableViewModel
It allows the creation of background tasks with ViewModel in Xamarin Android.

In this section we provide you an example to implement this library in your xamarin android project. Before you do it, you can review an example from [xamarin ViewModel repository](https://developer.xamarin.com/samples/monodroid/android-support/ViewModel/). For better understanding how ViewModels work, go to [android documentation](https://developer.android.com/topic/libraries/architecture/viewmodel). You need to know that this library doesn't use LiveData, in other hand, we use observables to handle threads and ViewModel to attach it to lifecycle activity or fragment.

 ## Get Started

Import the nuget package in your project.

https://www.nuget.org/packages/cloud.ludus/


### Config your ViewModel

After it, you need to create a ViewModel that extends from [BaseViewModel](ObservableViewModel/BaseViewModel.cs) as shown below. Here you need to override two methods. **LoadInBackground** to process the task in this thread and **ProcesssResponse** for incoming messages. 

```

public class ExampleViewModel : BaseViewModel<MyResponseType>
{
    protected override MyResponseType LoadInBackground()
    {
        // Consume your rest service or doing here anything that you need

        return new MyResponseType();
    }

    protected override void ProcessResponse(MyResponseType response, IObserver<MyResponseType> observer)
    {
        /*
         * Here you have an observer object, then you can define when call to
         * OnError or OnNext callbacks to finalize successfully. We recommend to allways call
         * OnComplete callback to process common actions.
         */
    
        if (response == null)
        {
            observer.OnError(new Exception());
        }
        else
        {
            observer.OnNext(response);
        }

        observer.OnCompleted();
    }
}

```

### Config on your activity

Now, initialize the viewModelManager to add the viewModel and actions to OnNext and OnError callbacks or optionally to OnComplete callback as shown below

```

var viewModelManager = new ViewModelManager();
var exampleViewModel = ViewModelProviders.Of(this).Get(Java.Lang.Class.FromType(typeof(ExampleViewModel))) as ExampleViewModel;

// Register ViewModel and actions
viewModelManager.AddViewModel(exampleViewModel, ExampleSuccess, ExampleError);

// Init the task
exampleViewModel.InitObserver();

```

These are examples methods to OnNext and OnError callbacks

```

private void ExampleSuccess(MyResponseType response)
{
    // Handle response here
}

private void ExampleError(Exception exception)
{
    // Handle exception here 
}

```

