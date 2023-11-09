using DryIoc;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MaterialDesignThemes.Wpf;
using Practice.Core;
using Practice.Core.Configuration;
using Practice.Core.RegionAdapterMappings;
using Practice.Events.Handlers;
using Practice.Provider;
using Practice.Provider.DBContext;
using Practice.Provider.interfaces;
using Practice.Provider.Interfaces;
using Practice.Services;
using Practice.Services.Interfaces;
using Practice.ViewModels;
using Practice.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Practice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {

            // UI线程未捕获异常处理事件
            DispatcherUnhandledException += (o, args) =>
            {
                //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出
                args.Handled = true;
                Log.Logger.Error(args.Exception, "UI线程出现异常");
            };

            // task 任务调度器中 task 执行发生异常
            TaskScheduler.UnobservedTaskException += (o, args) =>
            {
                Log.Logger.Error(args.Exception, $"{nameof(Task)}执行出现异常");
            };

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += (o, args) =>
            {
                if (args.IsTerminating)
                {
                    if (args.ExceptionObject is Exception ex)
                    {
                        Log.Logger.Error(ex, $"Host terminated unexpectedly!");
                    }
                    else
                    {
                        Log.Logger.Error(args.ExceptionObject.ToString() ?? string.Empty, $"Host terminated unexpectedly!");
                    }
                }
            };

            LiveCharts.Configure(config =>
                    config
                        // registers SkiaSharp as the library backend
                        // REQUIRED unless you build your own
                        .AddSkiaSharp()

                        // adds the default supported types
                        // OPTIONAL but highly recommend
                        .AddDefaultMappers()

                        // select a theme, default is Light
                        // OPTIONAL
                        //.AddDarkTheme()
                        .AddLightTheme()
            // .HasMap<Foo>( .... ) 
            // .HasMap<Bar>( .... ) 
            );
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<WorkingSoftwareView, WorkingSoftwareViewModel>("WorkingSoftware");
            containerRegistry.RegisterForNavigation<GameView, GameViewModel>("Test");
            containerRegistry.RegisterForNavigation<ThemeChangeView, ThemeChangeViewModel>("ThemeChange");
            containerRegistry.RegisterForNavigation<ReactiveView, ReactiveViewModel>("Reactive");
            containerRegistry.RegisterForNavigation<ChartZoomView, ChartZoomViewModel>(nameof(ChartZoomView));
            //regionManager

            // Singleton
            containerRegistry.RegisterSingleton<IMenuManager, MenuManager>();
            containerRegistry.RegisterSingleton<PaletteHelper>();
            containerRegistry.RegisterSingleton<SystemSettingsManager>();
            containerRegistry.RegisterInstance(new SafetyUiActionService(Dispatcher));
            containerRegistry.RegisterSingleton<IRootDialogService, RootDialogService>();
            containerRegistry.RegisterSingleton<ILogger>(() => Log.Logger);
            containerRegistry.RegisterSingleton<RootConfiguration>(provider =>
            {
                var systemSettingsManager = provider.Resolve<SystemSettingsManager>();
                return systemSettingsManager.GetSetting<RootConfiguration>(SystemSettingKeys.RootConfiguration) ?? new RootConfiguration();
            });
            containerRegistry.RegisterSingleton<INotifyIconService, NotifyIconService>();
            containerRegistry.RegisterSingleton<IAutoSubscribeNotifyIconEventHandler, AutoSubscribeNotifyIconEventHandler>();
            containerRegistry.RegisterSingleton<ICaptureMenuBar, CacheCurrentMenuBar>();
            containerRegistry.RegisterSingleton<IPaginationService, PaginationService>();
            containerRegistry.RegisterSingleton<IPaginationControlViewPresentHandler, PaginationControlViewPresentHandler>();
            containerRegistry.RegisterSingleton<ISnackbarService, SnackbarService>();
            containerRegistry.RegisterSingleton<MainWindowsContentService>();

            // Transient
            containerRegistry.Register<IMenuProvider, MenuProvider>();
            containerRegistry.Register<IAppInfoProvider, AppInfoProvider>();
            containerRegistry.Register<IAppInfoManager, AppInfoManager>();
            containerRegistry.Register<IAutoStartupService, AutoStartupService>();
            containerRegistry.Register<ILogProvider, LogProvider>();

            // Scope
            containerRegistry.RegisterScoped<IPracticeDataDbContext, PracticeDataDbContext>();
        }

        protected override Window CreateShell()
        {
            var settingsManager = Container.Resolve<SystemSettingsManager>();

            var theme = settingsManager.GetSetting<Theme>(SystemSettingKeys.Theme);
            if (theme != null)
            {
                var paletteHelper = Container.Resolve<PaletteHelper>();
                paletteHelper.SetTheme(theme);
            }

            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping<TabControl, TabControlRegionAdapter>();
        }
    }
}
