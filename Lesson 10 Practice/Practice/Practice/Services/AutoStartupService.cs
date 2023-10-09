using Microsoft.Win32;
using Practice.Core;
using Practice.Services.Interfaces;
using Serilog;
using System;
using Practice.Extensions;

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
        private readonly ILogger _logger;

        public AutoStartupService(ILogger logger)
        {
            _applicationName = AppDomain.CurrentDomain.FriendlyName;
            _logger = logger;
        }

        /// <summary>
        /// 得到可执行路径应用程序
        /// </summary>
        /// <returns></returns>
        private string GetCurrentExecutableFile()
        {
            return SystemSettingKeys.GetExecutableFile();
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
        public bool Enable(bool forAllUsers = false)
        {
            return AutoStartupRegistryAction(key =>
            {
                var executableFile = GetCurrentExecutableFile();
                key.SetValue(_applicationName, executableFile);

                _logger.Information(forAllUsers
                    ? $"给【所有用户】设置开启启动！可执行程序路径：{executableFile}"
                    : $"给【当前用户】设置开启启动！可执行程序路径：{executableFile}");
            }, forAllUsers);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disable(bool forAllUsers = false)
        {
            return AutoStartupRegistryAction(key =>
            {
                key.DeleteValue(_applicationName, false);
            }, forAllUsers);
        }

        /// <summary>
        /// 更新自启动设定注册表,操作成功返回 true
        /// </summary>
        /// <param name="action">自启动注册表操作</param>
        /// <param name="forAllUsers">此程序是否为所有用户电脑自启动，如果是，则需要 “管理员权限”</param>
        protected virtual bool AutoStartupRegistryAction(Action<RegistryKey> action, bool forAllUsers = false)
        {
            // 如果杀毒软件阻止操作，将会产生异常
            try
            {
                var key = forAllUsers
                    ? Registry.LocalMachine.OpenSubKey(RegistryKey, true)
                    : Registry.CurrentUser.OpenSubKey(RegistryKey, true);
                if (key == null) return false;

                using (key)
                {
                    action(key);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorDetail(ex);
                return false;
            }
        }
    }
}
