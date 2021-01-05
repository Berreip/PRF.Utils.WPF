namespace WpfModelApp.WPFCore.Views
{
    internal partial class MainWindowView
    {
        public MainWindowView(MainWindowViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
