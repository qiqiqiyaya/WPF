using Practice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice.Provider.interfaces
{
    public interface IMenuProvider
    {
        Task<List<MenuBar>> GetAllAsync();
    }
}
