using Practice.Provider.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Models;
using Practice.Services.interfaces;
using System.Drawing;
using System.IO;
using Practice.Helpers.IconExtractor;
using System.Reflection;

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
            var list = new List<string>() { "visual studio code", "visual studio", "starUML", "microsoft edge",
                "google chrome", "photoshop","oracle vm virtualbox","docker desktop","postman","sql server management",
                "navicat"
            };

            var aaa = _appInfoProvider.GetList(x =>
            {
                var name = x.DisplayName.ToLower();

                if (list.Any(se => name.Contains(se)))
                {
                    return true;
                }

                return false;
            });

            //FillIcon
            return aaa;
        }

        //public Icon ConvertToIcon(string fileName)
        //{
        //    if (Path.GetExtension(fileName).ToLower() == ".ico")
        //    {
        //        return new Icon(fileName);
        //    }

        //    var extractor = new IconExtractor(fileName);
        //    //icon = extractor.GetIcon(index);
        //}
    }
}
