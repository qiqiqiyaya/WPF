using Practice.Models;
using System.Collections.Generic;

namespace Practice.Services.interfaces
{
    public interface IAppInfoManager
    {
        List<AppInfo> GetAll();

        List<AppInfo> GetWorkingSoftware();
    }
}
