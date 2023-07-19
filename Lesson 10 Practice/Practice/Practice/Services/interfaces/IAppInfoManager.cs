using Practice.Common;
using Practice.Models;
using System.Collections.Generic;

namespace Practice.Services.Interfaces
{
    public interface IAppInfoManager
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        Result<List<AppInfo>> GetAll();

        /// <summary>
        /// 获取所有、及其Icon
        /// </summary>
        /// <returns></returns>
        IAsyncEnumerable<AppIcon> GetAllIcons();

        /// <summary>
        /// 获取工作软件信息
        /// </summary>
        /// <returns></returns>
        Result<List<AppInfo>> GetWorkingSoftwareInfo();

        /// <summary>
        /// 获取工作软件信息、及其Icon
        /// </summary>
        /// <returns></returns>
        Result<List<AppIcon>> GetWorkingSoftwareIcon();
    }
}
