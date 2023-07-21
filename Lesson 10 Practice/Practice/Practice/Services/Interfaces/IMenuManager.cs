using Practice.Models;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace Practice.Services.Interfaces
{
    public interface IMenuManager
    {
        /// <summary>
        /// 菜单
        /// </summary>
        ObservableCollection<MenuBar> MenuItems { get; set; }

        /// <summary>
        /// 菜单选中索引
        /// </summary>
        int MenuSelectIndex { get; set; }

        /// <summary>
        /// tabItem-newMenu 选中项
        /// </summary>
        int TabItemMenuSelectedIndex { get; set; }

        /// <summary>
        /// tab项
        /// </summary>
        public IViewsCollection TabItemMenus { get; }

        /// <summary>
        /// 设置内容区域 
        /// </summary>
        /// <param name="region"></param>
        void SetContentRegion(IRegion region);

        /// <summary>
        /// 菜单异步加载，然后ui线程更新元素
        /// </summary>
        /// <returns></returns>
        void LoadMenus();

        /// <summary>
        /// 菜单导航
        /// </summary>
        /// <param name="menu"></param>
        void MenuNavigate(MenuBar? menu);

        /// <summary>
        /// tabItem 菜单切换
        /// </summary>
        /// <param name="newMenu"></param>
        void TabItemMenuChange(MenuBar? newMenu);

        /// <summary>
        /// 关闭所有菜单，除 Home 外
        /// </summary>
        void TabItemMenuCloseAll();

        /// <summary>
        /// 关闭指定 tab 菜单
        /// </summary>
        /// <param name="menu"></param>
        void TabItemMenuClose(MenuBar menu);
    }
}
