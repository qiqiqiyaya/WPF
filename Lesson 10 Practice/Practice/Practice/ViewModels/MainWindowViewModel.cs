using ImTools;
using MaterialDesignThemes.Wpf;
using Practice.Core.Contract;
using Practice.Models;
using Practice.Services;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

// ReSharper disable ConvertToAutoProperty

namespace Practice.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        /// <summary>
        /// 左侧菜单内容区域样式切换
        /// </summary>
        public DelegateCommand LeftContentSwitchCommand { get; }
        /// <summary>
        /// 菜单导航
        /// </summary>
        public DelegateCommand<MenuBar> MenuNavigateCommand { get; }
        /// <summary>
        /// tabItem 发生切换
        /// </summary>
        public DelegateCommand<MenuBar> TabItemChangeCommand { get; }
        /// <summary>
        /// 关闭指定 tab 菜单命令
        /// </summary>
        public DelegateCommand<MenuBar> TabItemCloseCommand { get; }
        /// <summary>
        /// 关闭所有菜单命令，除 Home 外
        /// </summary>
        public DelegateCommand CloseAllTabItemCommand { get; }

        private readonly SafetyUiAction _safetyUiAction;
        private readonly IRegionViewRegistry _regionViewRegistry;
        private readonly MenuService _menuService;
        private readonly IRegionManager _regionManager;
        private IRegion _region;

        public MainWindowViewModel(
            SafetyUiAction safetyUiAction,
            IRegionViewRegistry regionViewRegistry,
            IRegionManager regionManager,
            MenuService menuService)
        {
            _regionManager = regionManager;
            MenuNavigateCommand = new DelegateCommand<MenuBar>(MenuNavigate);
            LeftContentSwitchCommand = new DelegateCommand(LeftContentSwitch);
            TabItemChangeCommand = new DelegateCommand<MenuBar>(TabItemChange);
            TabItemCloseCommand = new DelegateCommand<MenuBar>(TabItemClose);
            CloseAllTabItemCommand = new DelegateCommand(CloseAllTabItem);
            _safetyUiAction = safetyUiAction;
            _regionViewRegistry = regionViewRegistry;
            _menuService = menuService;
            LoadMenu();
        }

        private double _leftMenuMenuContentWidth = 200;

        /// <summary>
        /// 左侧内容区域宽度，菜单折叠宽 80 ，菜单展开宽 200
        /// </summary>
        public double LeftMenuContentWidth
        {
            get => _leftMenuMenuContentWidth;
            set => this.RaiseAndSetIfChanged(ref _leftMenuMenuContentWidth, value);
        }

        protected static PackIcon IconMenuClose = new PackIcon() { Kind = PackIconKind.MenuClose, Width = 24, Height = 24 };
        protected static PackIcon IconMenuOpen = new PackIcon() { Kind = PackIconKind.MenuOpen, Width = 24, Height = 24 };
        private PackIcon _leftContentButtonIcon = IconMenuOpen;

        /// <summary>
        /// 左侧内容区域操作按钮Icon
        /// </summary>
        public PackIcon LeftContentButtonIcon
        {
            get => _leftContentButtonIcon;
            set => this.RaiseAndSetIfChanged(ref _leftContentButtonIcon, value);
        }

        /// <summary>
        /// 本机名称
        /// </summary>
        public string HostName => Environment.UserName;

        private Visibility _bigAvatarVisibility = Visibility.Visible;

        /// <summary>
        /// 大头像是否显示
        /// </summary>
        public Visibility BigAvatarVisibility
        {
            get => _bigAvatarVisibility;
            set => this.RaiseAndSetIfChanged(ref _bigAvatarVisibility, value);
        }

        private double _bigAvatarHeight = 200;

        /// <summary>
        /// 大头像高度
        /// </summary>
        public double BigAvatarHeight
        {
            get => _bigAvatarHeight;
            set => this.RaiseAndSetIfChanged(ref _bigAvatarHeight, value);
        }

        private Visibility _minAvatarVisibility = Visibility.Hidden;

        /// <summary>
        /// 小头像是否显示
        /// </summary>
        public Visibility MinAvatarVisibility
        {
            get => _minAvatarVisibility;
            set => this.RaiseAndSetIfChanged(ref _minAvatarVisibility, value);
        }

        private double _minAvatarHeight;

        /// <summary>
        /// 小头像高度
        /// </summary>
        public double MinAvatarHeight
        {
            get => _minAvatarHeight;
            set => this.RaiseAndSetIfChanged(ref _minAvatarHeight, value);
        }

        /// <summary>
        /// 左侧菜单内容区域样式切换
        /// </summary>
        protected virtual void LeftContentSwitch()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (LeftMenuContentWidth == 80)
            {
                LeftMenuContentWidth = 200;
                LeftContentButtonIcon = IconMenuOpen;
                BigAvatarVisibility = Visibility.Visible;
                MinAvatarVisibility = Visibility.Hidden;
                MinAvatarHeight = 0;
                BigAvatarHeight = 200;
            }
            else
            {
                LeftMenuContentWidth = 80;
                LeftContentButtonIcon = IconMenuClose;
                BigAvatarVisibility = Visibility.Hidden;
                MinAvatarVisibility = Visibility.Visible;
                MinAvatarHeight = 80;
                BigAvatarHeight = 0;
            }
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

        private int _menuSelectIndex = -1;
        /// <summary>
        /// 菜单选中索引
        /// </summary>
        public int MenuSelectIndex
        {
            get => _menuSelectIndex;
            set => this.RaiseAndSetIfChanged(ref _menuSelectIndex, value);
        }

        private int _tabItemSelectedIndex = -1;
        /// <summary>
        /// tabItem选中项
        /// </summary>
        public int TabItemSelectedIndex
        {
            get => _tabItemSelectedIndex;
            set => this.RaiseAndSetIfChanged(ref _tabItemSelectedIndex, value);
        }

        /// <summary>
        /// tab项
        /// </summary>
        public IViewsCollection TabItems
        {
            get
            {
                // see
                // MainWindow.xaml.cs
                // RegionManager.SetRegionName(TabMenus, SystemSettingKeys.TabMenuRegion);
                // RegionManager.SetRegionManager(TabMenus, regionManager);

                // ReSharper disable once ConstantNullCoalescingCondition
                _region ??= _regionManager.Regions[SystemSettingKeys.TabMenuRegion];
                return _region.Views;
            }
        }

        protected void LoadMenu()
        {
            _safetyUiAction.InvokeAsync(async () =>
            {
                var menus = await _menuService.GetAll();
                MenuItems = new ObservableCollection<MenuBar>();

                for (int i = 0; i < menus.Count; i++)
                {
                    menus[i].Index = i;
                }

                MenuItems.AddRange(menus);
                // select home
                MenuSelectIndex = 0;
            });
        }

        /// <summary>
        /// 菜单导航
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void MenuNavigate(MenuBar menu)
        {
            // 初次，tabItem需要被添加
            if (menu.TabItemInfo.Index == -1)
            {
                MenuSelectIndex = menu.Index;
                menu.TabItemInfo.Index = !_region.Views.Any() ? 0 : _region.Views.Count();
                TabItemSelectedIndex = menu.TabItemInfo.Index;
                TabContentResolve(menu);
                return;
            }

            // 后续，菜单 或 tabItem 切换时
            if (_tabItemSelectedIndex != menu.TabItemInfo.Index)
            {
                TabItemSelectedIndex = menu.TabItemInfo.Index;
            }
        }

        /// <summary>
        /// tabItem 发生切换
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void TabItemChange(MenuBar? menu)
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
        public virtual void CloseAllTabItem()
        {
            _safetyUiAction.DelayWhen(() =>
            {
                var list = _region.Views.Cast<MenuBar>().Where(x => x.TabItemInfo.CloseBtn == Visibility.Visible)
                    .Reverse()
                    .ToList();
                foreach (var menuBar in list)
                {
                    TabItemCloseLogic(menuBar);
                }
            }, 150);
        }

        /// <summary>
        /// 关闭指定 tab 菜单
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void TabItemClose(MenuBar menu)
        {
            int index = menu.TabItemInfo.Index;
            TabItemCloseLogic(menu);
            RestTabItemIndexReset();

            TabItemSelectedIndex = index;
        }

        /// <summary>
        /// 菜单关闭具体逻辑
        /// </summary>
        /// <param name="menu"></param>
        private void TabItemCloseLogic(MenuBar menu)
        {
            _region.Remove(menu);
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
        /// 剩余TabItem索引重置
        /// </summary>
        protected virtual void RestTabItemIndexReset()
        {
            int index = 0;
            foreach (var view in _region.Views)
            {
                var menuBar = (MenuBar)view;
                if (menuBar.TabItemInfo.Index != index)
                {
                    menuBar.TabItemInfo.Index = index;
                }

                index++;
            }
        }
    }
}