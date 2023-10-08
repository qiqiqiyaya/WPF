using System;
using Practice.Common;
using Practice.Core;
using Practice.Models;
using Practice.Provider.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Practice.Dtos.Inputs;
using Practice.Extensions;

namespace Practice.Provider
{
    public class LogProvider : ILogProvider
    {
        private readonly IPracticeDataDbContext _dbContext;

        public LogProvider(IPracticeDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PageList<List<LogDetail>>> GetPageListAsync(int page, int rowNumber = SystemSettingKeys.PageSize)
        {
            var list = await _dbContext.Log
                .OrderByDescending(s => s.Id)
                .Skip((page - 1) * rowNumber)
                .Take(rowNumber)
                .ToListAsync();

            var count = await _dbContext.Log.CountAsync();

            return new PageList<List<LogDetail>>(list, count);
        }

        public async Task<PageList<List<LogDetail>>> GetPageListAsync(LogSearchInput input, int page, int rowNumber = SystemSettingKeys.PageSize)
        {
            var beginTime = input.BeginTime?.GetTimestamp();
            var endTime = input.EndTime?.GetTimestamp();

            var query = _dbContext.Log
                .WhereIf(!input.Key.IsNullOrWhiteSpace(),
                    x => x.Exception.Contains(input.Key) || x.RenderedMessage.Contains(input.Key))
                .WhereIf(beginTime.HasValue, x => x.Timestamp >= beginTime!.Value)
                .WhereIf(endTime.HasValue, x => x.Timestamp <= endTime!.Value)
                .WhereIf(!input.Level!.IsNullOrWhiteSpace(), x => x.Level == input.Level);

            var count = await query.CountAsync();

            var data = await query
                .PageBy(x => x.Timestamp, page, rowNumber)
                .ToListAsync();

            return new PageList<List<LogDetail>>(data, count);
        }
    }
}
