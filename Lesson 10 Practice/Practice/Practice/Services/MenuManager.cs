﻿using Practice.Core;
using Practice.Extensions;
using Practice.Models;
using Practice.Services.interfaces;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Services
{
    public class MenuManager : ReactiveObject
    {
        private readonly IMenuService _menuService;
        private readonly IRegionViewRegistry _regionViewRegistry;
        private readonly SafetyUiAction _safetyUiAction;
        private readonly IContainerExtension _containerProvider;
        /// <summary>
        /// 内容显示区域
        /// </summary>
        protected IRegion Region;

        public MenuManager(IMenuService menuService,
            IRegionViewRegistry regionViewRegistry,
            SafetyUiAction safetyUiAction,
            IContainerExtension containerProvider)
        {
            _menuService = menuService;
            _regionViewRegistry = regionViewRegistry;
            _safetyUiAction = safetyUiAction;
            _containerProvider = containerProvider;

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
                await Task.Delay(1500);
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
        /// tabItem-newMenu 选中项
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
        /// 上一次被选中的菜单对象
        /// </summary>
        private MenuBar? _lastSelectedMenuBar;

        /// <summary>
        /// 是否正在执行关闭指定TabItem项操作
        /// </summary>
        private bool _doingRemoveAction;

        /// <summary>
        /// 是否正在执行关闭所有TabItem项操作
        /// </summary>
        private bool _doingRemoveAllAction;

        /// <summary>
        /// 菜单导航
        /// </summary>
        /// <param name="menu"></param>
        public void MenuNavigate(MenuBar? menu)
        {
            if (menu == null || _doingRemoveAction || _doingRemoveAllAction) return;

            // 初次，tabItem需要被添加
            if (menu.TabItemMenu.Index == -1)
            {
                MenuSelectIndex = menu.Index;
                var count = Region.Views.Count();
                menu.TabItemMenu.Index = Region.Views.Any() ? count : 0;

                TabItemMenuSelectedIndex = menu.TabItemMenu.Index;
                TabContentResolve(menu);
                if (count >= 1 && _lastSelectedMenuBar != null)
                {
                    TabItemMenuChangeAction(_lastSelectedMenuBar, null);
                }

                _lastSelectedMenuBar = menu;
                return;
            }

            // 后续，菜单 或 tabItem 切换时
            if (_tabItemMenuSelectedIndex != menu.TabItemMenu.Index)
            {
                TabItemMenuChangeAction(_lastSelectedMenuBar, menu);
                TabItemMenuSelectedIndex = menu.TabItemMenu.Index;
                _lastSelectedMenuBar = menu;
            }
        }

        /// <summary>
        /// tab内容区域动态解析
        /// </summary>
        /// <param name="menu"></param>
        private void TabContentResolve(MenuBar menu)
        {
            var userControl = _containerProvider.Resolve(menu.TabItemMenu.ViewType) as UserControl;

            Check.NotNull(userControl, nameof(userControl));
            if (userControl is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                // view与viewModel连接
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }

            menu.TabItemMenu.UserControl = userControl;
            _regionViewRegistry.RegisterViewWithRegion(SystemSettingKeys.TabMenuRegion, () => menu);
        }

        /// <summary>
        /// tabItem 发生切换
        /// </summary>
        /// <param name="newMenu"></param>
        public virtual void TabItemMenuChange(MenuBar? newMenu)
        {
            if (newMenu == null || _doingRemoveAction || _doingRemoveAllAction) return;
            if (_menuSelectIndex != newMenu.Index)
            {
                TabItemMenuChangeAction(_lastSelectedMenuBar, newMenu);
                MenuSelectIndex = newMenu.Index;
                _lastSelectedMenuBar = newMenu;
            }
        }

        /// <summary>
        /// 关闭所有菜单，除 Home 外
        /// </summary>
        public virtual void TabItemMenuCloseAll()
        {
            _doingRemoveAllAction = true;
            _safetyUiAction.DelayWhen(() =>
            {
                MenuSelectIndex = 0;
                TabItemMenuSelectedIndex = 0;
                _lastSelectedMenuBar = MenuItems[0];
                TabItemMenuChangeAction(null, _lastSelectedMenuBar);

                var list = Region.Views.Cast<MenuBar>().Where(x => x.TabItemMenu.CloseBtn == Visibility.Visible)
                    .Reverse()
                    .ToList();
                foreach (var menuBar in list)
                {
                    ToCloseTabItemMenu(menuBar);
                }

                _doingRemoveAllAction = false;
            }, 150);
        }

        /// <summary>
        /// 关闭指定 tab 菜单
        /// </summary>
        /// <param name="menu"></param>
        public virtual void TabItemMenuClose(MenuBar menu)
        {
            _doingRemoveAction = true;

            // 当前项是否是被选中
            if (TabItemMenuSelectedIndex == menu.TabItemMenu.Index)
            {
                var count = TabItemMenus.Count();
                // 当前项是否是最后一项
                if (count - 1 == TabItemMenuSelectedIndex)
                {
                    // tabItem项向前移设置，选中项
                    TabItemMenuSelectedIndex -= 1;
                    // 设置菜单选中项
                    var prev = (MenuBar)TabItemMenus.First(x => ((MenuBar)x).TabItemMenu.Index == TabItemMenuSelectedIndex);
                    MenuSelectIndex = prev.Index;
                    TabItemMenuChangeAction(null, prev);
                    _lastSelectedMenuBar = prev;
                }
                else
                {
                    // tabItem项向后移设置，选中项
                    var nextIndex = TabItemMenuSelectedIndex + 1;
                    var next = (MenuBar)TabItemMenus.First(x => ((MenuBar)x).TabItemMenu.Index == nextIndex);
                    MenuSelectIndex = next.Index;
                    TabItemMenuChangeAction(null, next);
                    _lastSelectedMenuBar = next;
                }
            }

            ToCloseTabItemMenu(menu);
            RestTabItemMenuIndexReset();
            _doingRemoveAction = false;
        }

        /// <summary>
        /// 菜单关闭具体逻辑
        /// </summary>
        /// <param name="menu"></param>
        private void ToCloseTabItemMenu(MenuBar menu)
        {
            Region.Remove(menu);
            ViewDispose(menu);
            menu.TabItemMenu.Reset();
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

        /// <summary>
        /// tab切换item时，触发的相关操作
        /// </summary>
        /// <param name="oldMenu">上一个菜单</param>
        /// <param name="newMenu">当前菜单</param>
        protected virtual void TabItemMenuChangeAction(MenuBar? oldMenu, MenuBar? newMenu)
        {
            if (oldMenu != null && oldMenu.TabItemMenu.UserControl is UserControl oldControl)
            {
                if (oldControl.DataContext is ITabItemMenuChangeAction oldAction)
                {
                    oldAction.OnLeave();
                }
            }

            if (newMenu != null && newMenu.TabItemMenu.UserControl is UserControl newControl)
            {
                if (newControl.DataContext is ITabItemMenuChangeAction newAction)
                {
                    newAction.OnEnter();
                }
            }
        }

        /// <summary>
        /// View、ViewModel相关对象释放
        /// </summary>
        /// <param name="oldMenu"></param>
        protected virtual void ViewDispose(MenuBar oldMenu)
        {
            if (oldMenu.TabItemMenu.UserControl is UserControl oldControl)
            {
                if (oldControl.DataContext is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
