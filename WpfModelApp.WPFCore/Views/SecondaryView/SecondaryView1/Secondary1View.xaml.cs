﻿
using PRF.WPFCore.Navigation;

namespace WpfModelApp.WPFCore.Views.SecondaryView.SecondaryView1
{
    internal partial class Secondary1View : INavigableView
    {
        public Secondary1View(Secondary1ViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        public void NavigateToCurrentRequested() { }
        public void NavigateFromCurrentRequested() { }
        public void NavigateToItSelfRequested() { }
    }
}
