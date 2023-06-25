using Practice.ViewModels;
using Practice.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows;

namespace Practice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<WorkingSoftwareView, WorkingSoftwareViewModel>("WorkingSoftware");
            //regionManager
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }
}
