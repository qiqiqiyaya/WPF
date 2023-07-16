using Practice.Provider.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Models;
using Practice.Services.interfaces;

namespace Practice.Services
{
    public class AppInfoManager : IAppInfoManager
    {
        private readonly IAppInfoProvider _appInfoProvider;

        public AppInfoManager(IAppInfoProvider appInfoProvider)
        {
            _appInfoProvider = appInfoProvider;
        }


        public List<AppInfo> GetAll()
        {
            return _appInfoProvider.GetAll();
        }

        public List<AppInfo> GetWorkingSoftware()
        {
            var list = new List<string>() { "Visual Studio Code", "Visual Studio 2022", "StarUML", "Microsoft Edge",
                "Google Chrome", "Adobe Photoshop 2022","Oracle VM VirtualBox","Docker Desktop","Postman","Microsoft SQL Server Management Studio 18",
                "Navicat Premium 12"
            };

            var aaa = _appInfoProvider.GetList(x => list.Any(se => se.Equals(x.DisplayName)));
            return aaa;
        }
    }
}
