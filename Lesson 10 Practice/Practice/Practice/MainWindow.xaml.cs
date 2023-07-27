using System;
using Practice.Core;
using Practice.Events;
using Practice.Services.Interfaces;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using XamlAnimatedGif;
using System.Windows.Media;

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
            IEventAggregator eventAggregator,
            IPaginationControlViewPresentHandler paginationControlViewPresentHandler)
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
            paginationControlViewPresentHandler.Init();

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

        /// <summary>
        /// 查找子控件
        /// </summary>
        /// <typeparam name="T">子控件的类型</typeparam>
        /// <param name="obj">要找的是obj的子控件</param>
        /// <param name="name">想找的子控件的Name属性</param>
        /// <returns>目标子控件</returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    // 在下一级中没有找到指定名字的子控件，就再往下一级找
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }

            return null;

        }

    }
}
