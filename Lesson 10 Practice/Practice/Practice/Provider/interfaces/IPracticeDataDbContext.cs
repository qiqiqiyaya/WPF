using Microsoft.EntityFrameworkCore;
using Practice.Models;

namespace Practice.Provider.interfaces
{
    public interface IPracticeDataDbContext
    {
        DbSet<LogDetail> Log { get; }
    }
}
