using Practice.Helpers.IconExtractor;
using Practice.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Practice.ViewModels.Models
{
    public class AppBitmapImage
    {
        public AppBitmapImage(AppInfo appInfo)
        {
            AppInfo = appInfo;

            //var aa=new IconExtractor()
        }

        public AppInfo AppInfo { get; }

        public Icon Icon { get; set; }
    }
}
