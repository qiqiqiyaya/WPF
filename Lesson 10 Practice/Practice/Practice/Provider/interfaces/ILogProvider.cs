using Practice.Common;
using Practice.Core;
using Practice.Dtos.Inputs;
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
        Task<PageList<List<LogDetail>>> GetPageListAsync(int page, int rowNumber = SystemSettingKeys.PageSize);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        Task<PageList<List<LogDetail>>> GetPageListAsync(LogSearchInput input, int page,
            int rowNumber = SystemSettingKeys.PageSize);
    }
}
