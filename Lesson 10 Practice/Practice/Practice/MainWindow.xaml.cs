using Practice.Core;
using Practice.Services;
using Practice.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;
using Practice.Events;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly INotifyIconService _notifyIconService;

        public MainWindow(IRegionManager regionManager,
            IMenuManager menuManager,
            IRootDialogService rootDialogService,
            INotifyIconService notifyIconService,
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

            Closed += (sender, args) => { notifyIconService.MainWindowsClose(); };

            _eventAggregator.GetEvent<NotifyIconEvent>().Subscribe()

            //NotifyIcon.DoubleClickCommand = new DelegateCommand(() =>
            //{
            //    this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
            //    // 程序任务栏 展示
            //    this.ShowInTaskbar = true;
            //    // 程序窗口置顶
            //    this.Activate();
            //    _eventAggregator.GetEvent<NotifyIconEvent>().Publish((PracticeWindowState)this.WindowState);
            //});
        }

        private void Minimized_OnClick(object sender, RoutedEventArgs e)
        {
            // 最小化
            this.WindowState = WindowState.Minimized;
            // 程序任务栏 隐藏
            this.ShowInTaskbar = false;
        }

        private void Maximized_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            _notifyIconService.MainWindowsClose();
            //this.Close();
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
