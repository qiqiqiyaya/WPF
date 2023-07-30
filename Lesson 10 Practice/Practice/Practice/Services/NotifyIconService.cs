using Hardcodet.Wpf.TaskbarNotification;
using Practice.Common;
using Practice.Core;
using Practice.Core.Configuration;
using Practice.Events;
using Practice.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using System.Windows;

#pragma warning disable CS8618

namespace Practice.Services
{
    /// <summary>
    /// 通知图标服务
    /// </summary>
    public class NotifyIconService : INotifyIconService
    {
        private readonly RootConfiguration _rootConfiguration;
        private Window _mainWindow;
        private TaskbarIcon _taskBarIcon;
        private readonly IRootDialogService _rootDialogService;
        private readonly SystemSettingsManager _systemSettingsManager;
        private readonly NotifyIconEvent _notifyIconEvent;

        public NotifyIconService(RootConfiguration rootConfiguration,
            IRootDialogService rootDialogService,
            SystemSettingsManager systemSettingsManager,
            IEventAggregator eventAggregator)
        {
            _systemSettingsManager = systemSettingsManager;
            _rootConfiguration = rootConfiguration;
            _rootDialogService = rootDialogService;

            _notifyIconEvent = eventAggregator.GetEvent<NotifyIconEvent>();
        }

        public void Init(Window mainWindow,
            TaskbarIcon taskBarIcon)
        {
            _mainWindow = mainWindow;
            _taskBarIcon = taskBarIcon;

            // 双击托盘图标
            taskBarIcon.DoubleClickCommand = new DelegateCommand(MainWindowsShow);
        }

        /// <summary>
        /// 主窗口关闭
        /// </summary>
        public void MainWindowsClose()
        {
            var action = new MainWindowsCloseDialogAction
            {
                Cancel = CancelMinimizeToTray,
                Ok = MinimizeToTray
            };
            if (_rootConfiguration.ShowMinimizeToTrayTip)
            {
                _rootDialogService.Show(new MainWindowsCloseDialog(action));
            }
            else
            {
                if (_rootConfiguration.CanMinimizeToTray)
                {
                    this.MinimizeToTray(action, false);
                }
                else
                {
                    _mainWindow.Close();
                }
            }
        }

        /// <summary>
        /// 直接关闭应用程序
        /// </summary>
        public void DirectClose()
        {
            _mainWindow.Close();
        }

        /// <summary>
        /// 主窗口展示
        /// </summary>
        public void MainWindowsShow()
        {
            _mainWindow.WindowState = _mainWindow.WindowState == WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
            // 程序任务栏 展示
            _mainWindow.ShowInTaskbar = true;
            // 程序窗口置顶
            _mainWindow.Activate();

            _notifyIconEvent.Publish((PracticeWindowState)_mainWindow.WindowState);
        }

        /// <summary>
        /// 取消最小化到托盘
        /// </summary>
        /// <param name="action"></param>
        /// <param name="showMinimizeToTrayTip">显示最小化到托盘提示吗？</param>
        public void CancelMinimizeToTray(MainWindowsCloseDialogAction action, bool showMinimizeToTrayTip)
        {
            if (showMinimizeToTrayTip)
            {
                // 不再显示
                _rootConfiguration.ShowMinimizeToTrayTip = !showMinimizeToTrayTip;
            }

            _rootConfiguration.CanMinimizeToTray = false;
            // 存储配置
            if (action.IsCheckChange)
            {
                _systemSettingsManager.SetSetting(SystemSettingKeys.RootConfiguration, _rootConfiguration);
            }
            _mainWindow.Close();
        }

        /// <summary>
        /// 最小化到托盘
        /// </summary>
        /// <param name="action"></param>
        /// <param name="showMinimizeToTrayTip">显示最小化到托盘提示吗？</param>
        public void MinimizeToTray(MainWindowsCloseDialogAction action, bool showMinimizeToTrayTip)
        {
            if (showMinimizeToTrayTip)
            {
                // 不再显示
                _rootConfiguration.ShowMinimizeToTrayTip = !showMinimizeToTrayTip;
            }

            _rootConfiguration.CanMinimizeToTray = true;
            // 最小化
            _mainWindow.WindowState = WindowState.Minimized;
            // 程序任务栏 隐藏
            _mainWindow.ShowInTaskbar = false;
            // 存储配置
            if (action.IsCheckChange)
            {
                _systemSettingsManager.SetSetting(SystemSettingKeys.RootConfiguration, _rootConfiguration);
            }

            _notifyIconEvent.Publish(PracticeWindowState.Tray);
        }

        /// <summary>
        /// 最小化
        /// </summary>
        public void Minimized()
        {
            _mainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化
        /// </summary>
        public void Maximized()
        {
            _mainWindow.WindowState = _mainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// 重置相关配置
        /// </summary>
        public virtual void ResetConfiguration()
        {
            _rootConfiguration.ShowMinimizeToTrayTip = true;
            _systemSettingsManager.SetSetting(SystemSettingKeys.RootConfiguration, _rootConfiguration);
        }
    }
}
