using Microsoft.EntityFrameworkCore;
using Practice.Common;
using Practice.Core;
using Practice.Models;
using Practice.Provider.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice.Provider
{
    public class LogProvider : ILogProvider
    {
        private readonly IPracticeDataDbContext _dbContext;

        public LogProvider(IPracticeDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PageList<List<LogDetail>>> GetPageList(int page, int rowNumber = SystemSettingKeys.RowNumber)
        {
            var list = await _dbContext.Log
                .OrderByDescending(s => s.Timestamp)
                .Skip((page - 1) * rowNumber)
                .Take(rowNumber)
                .ToListAsync();

            var count = await _dbContext.Log.CountAsync();

            return new PageList<List<LogDetail>>(list, count);
        }
    }
}
