﻿using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;

namespace Practice.Services.Interfaces
{
    public interface INotifyIconService
    {
        void Init(Window mainWindow, TaskbarIcon taskBarIcon);

        /// <summary>
        /// 主窗口关闭
        /// </summary>
        void MainWindowsClose();

        /// <summary>
        /// 重置相关配置
        /// </summary>
        void ResetConfiguration();
    }
}