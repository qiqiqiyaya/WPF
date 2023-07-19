using Practice.Models;
using System;
using System.Collections.Generic;

namespace Practice.Provider.Interfaces
{
    public interface IAppInfoProvider
    {
        /// <summary>
        /// 获取所有应用程序信息
        /// </summary>
        /// <returns></returns>
        List<AppInfo> GetAll();

        /// <summary>
        /// 获取所有应用程序信息
        /// </summary>
        /// <returns></returns>
        List<AppInfo> GetList(Func<AppInfo, bool> filter);
    }
}
