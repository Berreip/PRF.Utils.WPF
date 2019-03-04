using PRF.Utils.WPF.Navigation;

namespace WpfModelApp.Views.MainView.View1
{
    internal partial class View1View : INavigableView
    {
        public View1View(View1ViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        public void NavigateToCurrentRequested()
        {
            
        }

        public void NavigateFromCurrentRequested()
        {
        }

        public void NavigateToItSelfRequested()
        {
        }
    }
}
