using Practice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice.Services.interfaces
{
    public interface IMenuService
    {
        Task<List<MenuBar>> GetAllAsync();
    }
}
