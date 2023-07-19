using Practice.Extensions;
using System.Drawing;

namespace Practice.Models
{
    /// <summary>
    /// 应用程序Icon
    /// </summary>
    public class AppIcon
    {
        public AppIcon(AppInfo appInfo, Icon? icon)
        {
            AppInfo = appInfo;
            Icon = icon;
        }

        public AppInfo AppInfo { get; }

        public Icon? Icon { get; set; }
    }


    /// <summary>
    /// 应用程序Icon
    /// </summary>
    public static class AppIconExtensions
    {
        public static AppIcon ToIcon(this AppInfo appInfo, Icon? icon)
        {
            Check.NotNull(appInfo, nameof(AppInfo));
            return new AppIcon(appInfo, icon);
        }
    }
}
