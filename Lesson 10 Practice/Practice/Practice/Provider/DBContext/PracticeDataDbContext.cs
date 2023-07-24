using Microsoft.EntityFrameworkCore;
using Practice.Common;
using Practice.Core;
using Practice.Models;
using Practice.Provider.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice.Provider.DBContext
{
    /// <summary>
    /// scope 服务
    /// </summary>
    public class PracticeDataDbContext : DbContext, IPracticeDataDbContext
    {
        public DbSet<LogDetail> Log { get; set; }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={SystemSettingKeys.SqlitePath}");
        }

        public async Task<PageList<List<LogDetail>>> GetPageList(int page, int rowNumber)
        {
            var list = await Log.Skip((page - 1) * rowNumber).Take(rowNumber).ToListAsync();
            var count = await Log.CountAsync();

            return new PageList<List<LogDetail>>(list, count);
        }
    }
}
