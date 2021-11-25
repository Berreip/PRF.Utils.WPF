using WpfModelApp.WPFCore.Views.MainView.View1;

namespace WpfModelApp.WPFCore.Views.Assertion
{
    internal partial class AssertionFailedView
    {
        public AssertionFailedView(AssertionFailedViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
