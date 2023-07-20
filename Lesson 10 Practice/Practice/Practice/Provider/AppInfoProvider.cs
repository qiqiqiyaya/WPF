using Microsoft.Win32;
using Practice.Extensions;
using Practice.Models;
using Practice.Provider.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Practice.Provider
{
    public class AppInfoProvider : IAppInfoProvider
    {
        private readonly Regex _regex;
        private readonly char[] _invalidFileNameChars;

        public AppInfoProvider()
        {
            _invalidFileNameChars = Path.GetInvalidFileNameChars();
            string regexSearch = new string(_invalidFileNameChars) + new string(Path.GetInvalidPathChars());
            _regex = new Regex($"[{Regex.Escape(regexSearch)}]");
        }

        /// <summary>
        /// 获取所有应用程序信息
        /// </summary>
        /// <returns></returns>
        public List<AppInfo> GetAll()
        {
            var list = LocalMachine64();
            list.AddRange(LocalMachine32());

            return list;
        }

        /// <summary>
        /// 获取所有应用程序信息
        /// </summary>
        /// <returns></returns>
        public List<AppInfo> GetList(Func<AppInfo, bool> filter)
        {
            var list = LocalMachine64(filter);
            list.AddRange(LocalMachine32(filter));
            return list;
        }

        private List<AppInfo> LocalMachine64(Func<AppInfo, bool>? filter = null)
        {
            return GetListFromRegistry(RegistryView.Registry64, filter);
        }

        private List<AppInfo> LocalMachine32(Func<AppInfo, bool>? filter = null)
        {
            return GetListFromRegistry(RegistryView.Registry32, filter);
        }

        /// <summary>
        /// 从注册表中获取 “应用程序” 信息
        /// </summary>
        /// <param name="registryView"></param>
        /// <param name="filter">筛选器</param>
        /// <returns></returns>
        private List<AppInfo> GetListFromRegistry(RegistryView registryView, Func<AppInfo, bool>? filter = null)
        {
            var data = new List<AppInfo>();
            const string path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);

            using (RegistryKey? rk = localMachine.OpenSubKey(path))
            {
                if (rk == null) return data;

                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey? sk = rk.OpenSubKey(skName))
                    {
                        if (sk == null) continue;
                        var name = sk.GetString("DisplayName");
                        // 过滤掉没有名称的应用
                        if (name.IsNullOrWhiteSpace()) continue;

                        var appInfo = new AppInfo();
                        appInfo.DisplayName = name;
                        appInfo.Publisher = sk.GetString("Publisher");
                        appInfo.InstallLocation = sk.GetString("InstallLocation");
                        appInfo.HelpLink = sk.GetString("HelpLink");
                        appInfo.DisplayIcon = sk.GetString("DisplayIcon");

                        // 路径清理
                        appInfo.DisplayIcon = PathCleaning(appInfo.DisplayIcon);

                        if (filter != null)
                        {
                            if (filter(appInfo))
                            {
                                data.Add(appInfo);
                            }

                            continue;
                        }

                        data.Add(appInfo);
                    }
                }
            }

            return data;
        }

        public string PathCleaning(string path)
        {
            if (path.IsNullOrWhiteSpace()) return path;

            if (path.StartsWith('\"'))
            {
                // 只需去除，路径开头、结尾不合法字符
                path = path.Trim(_invalidFileNameChars);
            }

            var extension = Path.GetExtension(path);
            // 清理路径扩展名、路径结尾的不合法字符
            extension = _regex.Replace(extension, "");

            // 去掉文件名扩展名中 “,” 逗号，及其后面所有字符
            var index = extension.IndexOf(',');
            if (index != -1)
            {
                extension = extension.Substring(0, index);
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string? root = Path.GetPathRoot(path);

            if (!root!.IsNullOrWhiteSpace())
            {
                var directory = Path.GetDirectoryName(path);

                return directory + "\\" + fileNameWithoutExtension + extension;
            }
            else
            {
                return fileNameWithoutExtension + extension;
            }
        }
    }
}
