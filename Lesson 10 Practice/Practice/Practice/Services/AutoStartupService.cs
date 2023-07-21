using Microsoft.Win32;
using Practice.Services.Interfaces;
using System;
using System.IO;

namespace Practice.Services
{
    public class AutoStartupService : IAutoStartupService
    {
        // 程序自启动
        // 代码参考源 https://stackoverflow.com/questions/11065139/launch-window-on-windows-startup
        // 管理员权限启动程序
        // https://www.cnblogs.com/flamegreat/p/14620625.html

        // start it for all users
        private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private readonly string _applicationName;

        public AutoStartupService()
        {
            _applicationName = AppDomain.CurrentDomain.FriendlyName;
            //_applicationName = AppDomain.CurrentDomain.FriendlyName;
        }

        /// <summary>
        /// 得到可执行路径应用程序
        /// </summary>
        /// <returns></returns>
        private string GetCurrentExecutableFile()
        {
            return Path.Combine(Environment.CurrentDirectory, AppDomain.CurrentDomain.FriendlyName + ".exe");
        }

        /// <summary>
        /// 是否开机自启动
        /// </summary>
        /// <returns></returns>
        public bool IsAutoStartup(bool forAllUsers = false)
        {
            bool result = false;
            AutoStartupRegistryAction(key =>
            {
                var executableFile = key.GetValue(_applicationName)?.ToString();
                result = GetCurrentExecutableFile() == executableFile;
            }, forAllUsers);

            return result;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Enable(bool forAllUsers = false)
        {
            AutoStartupRegistryAction(key =>
            {
                var executableFile = GetCurrentExecutableFile();
                key.SetValue(_applicationName, executableFile);
            }, forAllUsers);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Disable(bool forAllUsers = false)
        {
            AutoStartupRegistryAction(key =>
            {
                key.DeleteValue(_applicationName, false);
            }, forAllUsers);
        }

        /// <summary>
        /// 更新自启动设定注册表
        /// </summary>
        /// <param name="action">自启动注册表操作</param>
        /// <param name="forAllUsers">此程序是否为所有用户电脑自启动，如果是，则需要 “管理员权限”</param>
        protected virtual void AutoStartupRegistryAction(Action<RegistryKey> action, bool forAllUsers = false)
        {
            var key = forAllUsers
                ? Registry.LocalMachine.OpenSubKey(RegistryKey, true)
                : Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (key == null) return;

            using (key)
            {
                action(key);
            }
        }
    }
}
