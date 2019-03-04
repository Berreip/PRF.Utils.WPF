using System;
using System.Windows;
using PRF.Utils.Injection.Containers;
using PRF.Utils.Injection.Utils;
using PRF.Utils.WPF.BootStrappers;
using PRF.Utils.WPF.PopupManager;
using WpfModelApp.Config;
using WpfModelApp.Navigation;
using WpfModelApp.Views;
using WpfModelApp.Views.MainView.View1;
using WpfModelApp.Views.MainView.View2;
using WpfModelApp.Views.Popups.Popup1;
using WpfModelApp.Views.SecondaryView.SecondaryView1;

namespace WpfModelApp
{
    internal class ModelAppBoot : BootStrapperPresentation<MainWindowView, MainWindowViewModel>
    {
        protected override void Register(IInjectionContainerRegister container)
        {
            //Gestionnaire de fenêtres:
            var popupManager = new WindowsPopupManager<WpfModelAppEnumWindow>(container.GetRegistrableContainer());
            container.RegisterInstance<IWindowsPopupManager<WpfModelAppEnumWindow>>(popupManager);

            //Gestionnaires:
            container.Register<IMainPanelNavigation, MainPanelNavigation>(LifeTime.Singleton);
            container.Register<ISecondaryPanelNavigation, SecondaryPanelNavigation>(LifeTime.Singleton);
            
            //Views
            container.RegisterType<View1View>(LifeTime.Singleton);
            container.RegisterType<View2View>(LifeTime.Singleton);
            container.RegisterType<Secondary1View>(LifeTime.Singleton);

            //ViewModels
            container.RegisterType<View1ViewModel>(LifeTime.Singleton);
            container.RegisterType<View2ViewModel>(LifeTime.Singleton);
            container.RegisterType<Secondary1ViewModel>(LifeTime.Singleton);

            // Windows
            popupManager.RegisterWindow<Popup1ViewModel, Popup1View>(WpfModelAppEnumWindow.Popup1);

        }

        protected override void ResolveUnregisteredType(object sender, Type type)
        {
            MessageBox.Show($"ResolveUnregisteredType: {type.FullName}");
        }

        protected override void Initialize(IInjectionContainer container)
        {
            base.Initialize(container);

            // navigue vers la première vue de l'écran principal
            container.Resolve<IMainPanelNavigation>().NavigateToFirstView();

            // navigue vers la première vue de l'écran secondaire
            container.Resolve<ISecondaryPanelNavigation>().NavigateToFirstView();

        }
    }
}
