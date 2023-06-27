using DryIoc;
using MaterialDesignThemes.Wpf;
using Practice.Properties;
using Practice.Services;
using Practice.Services.Contract;
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
        public App()
        {

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<WorkingSoftwareView, WorkingSoftwareViewModel>("WorkingSoftware");
            containerRegistry.RegisterForNavigation<GameView, GameViewModel>("Test");
            containerRegistry.RegisterForNavigation<ThemeChangeView, ThemeChangeViewModel>("ThemeChange");
            containerRegistry.RegisterForNavigation<ReactiveView, ReactiveViewModel>("Reactive");
            //regionManager

            containerRegistry.RegisterSingleton<PaletteHelper>();
            containerRegistry.RegisterSingleton<SettingsManager>();
        }

        protected override Window CreateShell()
        {
            var settingsManager = Container.Resolve<SettingsManager>();

            var theme = settingsManager.GetSetting<Theme>(SettingKeys.Theme);
            if (theme != null)
            {
                var paletteHelper = Container.Resolve<PaletteHelper>();
                paletteHelper.SetTheme(theme);
            }

            return Container.Resolve<MainWindow>();
        }
    }
}
