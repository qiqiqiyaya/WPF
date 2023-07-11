using Practice.Core.Contract;
using Practice.Models;
using Practice.Services.interfaces;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Practice.Services
{
    public class MenuManager : ReactiveObject
    {
        private readonly IMenuService _menuService;
        private readonly IRegionViewRegistry _regionViewRegistry;
        private readonly SafetyUiAction _safetyUiAction;
        /// <summary>
        /// 内容显示区域
        /// </summary>
        protected IRegion Region;

        public MenuManager(IMenuService menuService,
            IRegionViewRegistry regionViewRegistry,
            SafetyUiAction safetyUiAction)
        {
            _menuService = menuService;
            _regionViewRegistry = regionViewRegistry;
            _safetyUiAction = safetyUiAction;

            _menuItems = new ObservableCollection<MenuBar>();
        }


        private ObservableCollection<MenuBar> _menuItems;

        /// <summary>
        /// 菜单
        /// </summary>
        public ObservableCollection<MenuBar> MenuItems
        {
            get => _menuItems;
            set => this.RaiseAndSetIfChanged(ref _menuItems, value);
        }

        private int _menuSelectIndex;
        /// <summary>
        /// 菜单选中索引
        /// </summary>
        public int MenuSelectIndex
        {
            get => _menuSelectIndex;
            set => this.RaiseAndSetIfChanged(ref _menuSelectIndex, value);
        }

        /// <summary>
        /// 设置内容区域 
        /// </summary>
        /// <param name="region"></param>
        public void SetContentRegion(IRegion region)
        {
            Region = region;
        }

        /// <summary>
        /// 菜单异步加载，然后ui线程更新元素
        /// </summary>
        /// <returns></returns>
        public void MenuLoad()
        {
            _safetyUiAction.AsyncInvokeThenUiAction(async () =>
            {
                var menus = await _menuService.GetAllAsync();
                await Task.Delay(3000);
                return () =>
                {
                    MenuInitialSettings(menus);
                    MenuItems.AddRange(menus);
                    // 默认选中第一个
                    MenuSelectIndex = 0;
                };
            });
        }

        /// <summary>
        /// 菜单初始设置
        /// </summary>
        protected virtual void MenuInitialSettings(List<MenuBar> menus)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].Index = i;
                //Check.NotNull(menus[i].TabItemMenu, nameof(TabItemMenu));
                if (menus[i].TabItemMenu == null) continue;
                menus[i].TabItemMenu.Reset();
            }
        }

        private int _tabItemMenuSelectedIndex;
        /// <summary>
        /// tabItem-menu 选中项
        /// </summary>
        public int TabItemMenuSelectedIndex
        {
            get => _tabItemMenuSelectedIndex;
            set => this.RaiseAndSetIfChanged(ref _tabItemMenuSelectedIndex, value);
        }

        /// <summary>
        /// tab项
        /// </summary>
        public IViewsCollection TabItemMenus => Region.Views;

        /// <summary>
        /// 菜单导航
        /// </summary>
        /// <param name="menu"></param>
        public void MenuNavigate(MenuBar? menu)
        {
            if (menu == null) return;

            // 初次，tabItem需要被添加
            if (menu.TabItemMenu.Index == -1)
            {
                MenuSelectIndex = menu.Index;
                menu.TabItemMenu.Index = !Region.Views.Any() ? 0 : Region.Views.Count();
                TabItemMenuSelectedIndex = menu.TabItemMenu.Index;
                TabContentResolve(menu);
                return;
            }

            // 后续，菜单 或 tabItem 切换时
            if (_tabItemMenuSelectedIndex != menu.TabItemMenu.Index)
            {
                TabItemMenuSelectedIndex = menu.TabItemMenu.Index;
            }
        }

        /// <summary>
        /// tab内容区域动态解析
        /// </summary>
        /// <param name="menu"></param>
        private void TabContentResolve(MenuBar menu)
        {
            _regionViewRegistry.RegisterViewWithRegion(SystemSettingKeys.TabMenuRegion, () => menu);
        }

        /// <summary>
        /// tabItem 发生切换
        /// </summary>
        /// <param name="menu"></param>
        public virtual void TabItemMenuChange(MenuBar? menu)
        {
            if (menu == null) return;
            if (_menuSelectIndex != menu.Index)
            {
                MenuSelectIndex = menu.Index;
            }
        }

        /// <summary>
        /// 关闭所有菜单，除 Home 外
        /// </summary>
        public virtual void TabItemMenuCloseAll()
        {
            _safetyUiAction.DelayWhen(() =>
            {
                var list = Region.Views.Cast<MenuBar>().Where(x => x.TabItemMenu.CloseBtn == Visibility.Visible)
                    .Reverse()
                    .ToList();
                foreach (var menuBar in list)
                {
                    ToCloseTabItemMenu(menuBar);
                }
            }, 150);
        }

        /// <summary>
        /// 菜单关闭具体逻辑
        /// </summary>
        /// <param name="menu"></param>
        private void ToCloseTabItemMenu(MenuBar menu)
        {
            Region.Remove(menu);
        }

        /// <summary>
        /// 关闭指定 tab 菜单
        /// </summary>
        /// <param name="menu"></param>
        public virtual void TabItemMenuClose(MenuBar menu)
        {
            int index = menu.TabItemMenu.Index;
            ToCloseTabItemMenu(menu);
            RestTabItemMenuIndexReset();

            TabItemMenuSelectedIndex = index;
        }

        /// <summary>
        /// 剩余TabItemMenu索引重置
        /// </summary>
        protected virtual void RestTabItemMenuIndexReset()
        {
            int index = 0;
            foreach (var view in Region.Views)
            {
                var menuBar = (MenuBar)view;
                if (menuBar.TabItemMenu.Index != index)
                {
                    menuBar.TabItemMenu.Index = index;
                }

                index++;
            }
        }
    }
}
