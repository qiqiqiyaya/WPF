using System;
using Practice.Core;
using Practice.Events;
using Practice.Services.Interfaces;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using XamlAnimatedGif;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INotifyIconService _notifyIconService;
        private readonly IEventAggregator _eventAggregator;

        public MainWindow(IRegionManager regionManager,
            IMenuManager menuManager,
            IRootDialogService rootDialogService,
            INotifyIconService notifyIconService,
            IAutoSubscribeNotifyIconEventHandler notifyIconEventHandler,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _notifyIconService = notifyIconService;
            InitializeComponent();

            // root mdDialog Identifier set
            RootDialog.Identifier = SystemSettingKeys.RootDialogIdentity;
            rootDialogService.Init(RootDialog);

            // code-behind set region
            RegionManager.SetRegionName(TabMenus, SystemSettingKeys.TabMenuRegion);
            RegionManager.SetRegionManager(TabMenus, regionManager);

            menuManager.SetContentRegion(regionManager.Regions[SystemSettingKeys.TabMenuRegion]);
            menuManager.LoadMenus();

            // 通知图标初始化
            notifyIconService.Init(this, NotifyIcon);
            notifyIconEventHandler.Init();

            this.Header.MouseDown += (sender, args) =>
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            this.Header.MouseDoubleClick += (sender, args) =>
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };

            _eventAggregator.GetEvent<NotifyIconEvent>().Subscribe(res =>
            {
                var animator = AnimationBehavior.GetAnimator(Avatar);
                if (animator == null) return;
                switch (res)
                {
                    case PracticeWindowState.Normal:
                    case PracticeWindowState.Maximized:
                        if (animator.IsPaused) animator.Play();
                        break;
                    case PracticeWindowState.Minimized:
                    case PracticeWindowState.Tray:
                        animator.Pause();
                        break;
                }
            });

            Closed += (sender, args) => { notifyIconService.MainWindowsClose(); };
        }

        private void Minimized_OnClick(object sender, RoutedEventArgs e)
        {
            _notifyIconService.Minimized();
        }

        private void Maximized_OnClick(object sender, RoutedEventArgs e)
        {
            _notifyIconService.Maximized();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            _notifyIconService.MainWindowsClose();
        }

        /// <summary>
        /// 阻止事件上浮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventEventRaise_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

    }
}
