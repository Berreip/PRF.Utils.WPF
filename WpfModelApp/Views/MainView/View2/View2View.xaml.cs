﻿using PRF.Utils.WPF.Navigation;

namespace WpfModelApp.Views.MainView.View2
{
    internal partial class View2View : INavigableView
    {
        public View2View(View2ViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        public void NavigateToCurrentRequested() { }
        public void NavigateFromCurrentRequested() { }
        public void NavigateToItSelfRequested() { }
    }
}
