using Practice.Common;
using Practice.Core;
using Practice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice.Provider.interfaces
{
    public interface ILogProvider
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        Task<PageList<List<LogDetail>>> GetPageList(int page, int rowNumber = SystemSettingKeys.RowNumber);
    }
}
