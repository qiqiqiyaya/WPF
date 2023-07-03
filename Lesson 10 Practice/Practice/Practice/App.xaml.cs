﻿using DryIoc;
using MaterialDesignThemes.Wpf;
using Practice.Services;
using Practice.Services.interfaces;
using Practice.ViewModels;
using Practice.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Practice.Core.Contract;
using Prism.Regions;
using Practice.Core.RegionAdapterMappings;

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
            Exit += App_Exit;
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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File($"Logs/logs.txt"))
                .CreateLogger();

            Log.Logger.Information("应用程序启动");
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Log.Logger.Information("应用程序退出");
            Log.CloseAndFlush();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //var regionManager = containerRegistry.Resolve<IRegionManager>();

            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<WorkingSoftwareView, WorkingSoftwareViewModel>("WorkingSoftware");
            containerRegistry.RegisterForNavigation<GameView, GameViewModel>("Test");
            containerRegistry.RegisterForNavigation<ThemeChangeView, ThemeChangeViewModel>("ThemeChange");
            containerRegistry.RegisterForNavigation<ReactiveView, ReactiveViewModel>("Reactive");
            //regionManager

            containerRegistry.RegisterSingleton<PaletteHelper>();
            containerRegistry.RegisterSingleton<SettingsManager>();
            containerRegistry.RegisterInstance(new SafetyUiAction(Dispatcher));
            containerRegistry.RegisterSingleton<IRootDialogService, RootDialogService>();
        }

        protected override Window CreateShell()
        {
            var settingsManager = Container.Resolve<SettingsManager>();

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
