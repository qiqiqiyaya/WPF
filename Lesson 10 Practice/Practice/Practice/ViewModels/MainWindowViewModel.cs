using MaterialDesignThemes.Wpf;
using Practice.Models;
using Practice.Services;
using Practice.Views;
using Prism.Commands;
using Prism.Ioc;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        //private readonly IRegionManager _regionManager;
        private readonly IContainerProvider _containerProvider;
        private readonly SafetyUiDispatcher _safetyUiDispatcher;

        public MainWindowViewModel(IContainerExtension containerProvider, SafetyUiDispatcher safetyUiDispatcher)
        {
            MenuNavigateCommand = new DelegateCommand<MenuBar>(MenuNavigate);
            LeftContentSwitchCommand = new DelegateCommand(LeftContentSwitch);
            TabItemChangeCommand = new DelegateCommand<MenuBar>(TabItemChange);
            TabItemCloseCommand = new DelegateCommand<MenuBar>(TabItemClose);
            CloseAllTabItemCommand = new DelegateCommand(CloseAllTabItem);
            //_regionManager = regionManager;
            _containerProvider = containerProvider;
            _safetyUiDispatcher = safetyUiDispatcher;
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


        private ObservableCollection<MenuBar> _tabItems = new ObservableCollection<MenuBar>();
        /// <summary>
        /// tab项
        /// </summary>
        public ObservableCollection<MenuBar> TabItems
        {
            get => _tabItems;
            set => this.RaiseAndSetIfChanged(ref _tabItems, value);
        }

        protected void LoadMenu()
        {
            MenuItems = new ObservableCollection<MenuBar>();
            var list = new List<MenuBar>()
            {
                new MenuBar()
                {
                    Icon = "Home", NameSpace = "", Title = "Home",
                    TabItemInfo = new TabItemInfo() { CloseBtn = Visibility.Collapsed, ViewType = typeof(HomeView) }
                },
                new MenuBar()
                {
                    Icon = "Apps", NameSpace = "", Title = "工作软件",
                    TabItemInfo = new TabItemInfo()
                        { CloseBtn = Visibility.Visible, ViewType = typeof(WorkingSoftwareView) }
                },
                new MenuBar()
                {
                    Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏",
                    TabItemInfo = new TabItemInfo() { CloseBtn = Visibility.Visible, ViewType = typeof(GameView) }
                },
                new MenuBar()
                {
                    Icon = "Palette", NameSpace = "", Title = "主题切换",
                    TabItemInfo = new TabItemInfo()
                        { CloseBtn = Visibility.Visible, ViewType = typeof(ThemeChangeView) }
                },
                new MenuBar()
                {
                    Icon = "React", NameSpace = "", Title = "ReactiveUI",TabItemInfo =
                        new TabItemInfo(){CloseBtn = Visibility.Visible,ViewType = typeof(ReactiveView)}
                },
                new MenuBar()
                {
                    Icon = "MicrosoftWindows", NameSpace = "", Title = "系统信息",
                    TabItemInfo = new TabItemInfo(){CloseBtn = Visibility.Visible ,ViewType = typeof(SystemInformationView)}
                },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
            };

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index = i;
            }
            MenuItems.AddRange(list);

            MenuNavigate(MenuItems[0]);
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
                TabItems.Add(menu);
                menu.TabItemInfo.Index = _tabItems.Count == 0 ? 0 : _tabItems.Count - 1;
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
            _safetyUiDispatcher.DelayWhen(() => TabItems.RemoveAll(x => x.TabItemInfo.CloseBtn == Visibility.Visible), 150);
        }

        /// <summary>
        /// 关闭指定 tab 菜单
        /// </summary>
        /// <param name="menu"></param>
        protected virtual void TabItemClose(MenuBar menu)
        {
            menu.TabItemInfo.Index = -1;
            menu.TabItemInfo.Content = null;
            TabItems.Remove(menu);
            TabItemIndexReset();
        }

        /// <summary>
        /// tab内容区域动态解析
        /// </summary>
        /// <param name="menu"></param>
        private void TabContentResolve(MenuBar menu)
        {
            if (menu.TabItemInfo.Content != null || menu.TabItemInfo.ViewType == null) return;
            var userControl = _containerProvider.Resolve(menu.TabItemInfo.ViewType);
            if (userControl is UserControl control)
            {
                menu.TabItemInfo.Content = control;
            }
        }

        /// <summary>
        /// TabItem索引重置
        /// </summary>
        protected virtual void TabItemIndexReset()
        {
            for (int i = 0; i < TabItems.Count; i++)
            {
                if (TabItems[i].TabItemInfo.Index != i)
                {
                    TabItems[i].TabItemInfo.Index = i;
                }
            }
        }
    }
}