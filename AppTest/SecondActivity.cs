using Android.App;
using Android.OS;
using ObservableViewModel;

namespace AppTest
{
    [Activity]
    public class SecondActivity : BaseActivity
    {
        private ButtonFragment fragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_second);

            fragment = new ButtonFragment();

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.fragment_container_view, fragment, fragment.GetType().Name)
                .Commit();
        }
    }
}
