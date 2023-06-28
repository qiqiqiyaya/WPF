﻿using MaterialDesignThemes.Wpf;
using Practice.Models;
using Practice.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable ConvertToAutoProperty

namespace Practice.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand LeftContentButtonCommand { get; }
        public DelegateCommand<MenuBar> MenuNavigateCommand { get; }
        public DelegateCommand<MenuBar> TabItemChangeCommand { get; }
        public DelegateCommand<MenuBar> TabItemCloseCommand { get; }

        //private readonly IRegionManager _regionManager;
        private readonly IContainerProvider _containerProvider;

        public MainWindowViewModel(IContainerExtension containerProvider)
        {
            MenuNavigateCommand = new DelegateCommand<MenuBar>(MenuNavigate);
            LeftContentButtonCommand = new DelegateCommand(LeftContentButtonAction);
            TabItemChangeCommand = new DelegateCommand<MenuBar>(TabItemChange);
            TabItemCloseCommand = new DelegateCommand<MenuBar>(TabItemClose);
            //_regionManager = regionManager;
            _containerProvider = containerProvider;
            LoadMenu();
        }

        private double _leftMenuMenuContentWidth = 200;

        /// <summary>
        /// 左侧内容区域宽度，菜单折叠宽 80 ，菜单展开宽 200
        /// </summary>
        public double LeftMenuContentWidth
        {
            get => _leftMenuMenuContentWidth;
            set => SetProperty(ref _leftMenuMenuContentWidth, value);
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
            set => SetProperty(ref _leftContentButtonIcon, value);
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
            set => SetProperty(ref _bigAvatarVisibility, value);
        }

        private double _bigAvatarHeight = 200;

        /// <summary>
        /// 大头像高度
        /// </summary>
        public double BigAvatarHeight
        {
            get => _bigAvatarHeight;
            set => SetProperty(ref _bigAvatarHeight, value);
        }

        private Visibility _minAvatarVisibility = Visibility.Hidden;

        /// <summary>
        /// 小头像是否显示
        /// </summary>
        public Visibility MinAvatarVisibility
        {
            get => _minAvatarVisibility;
            set => SetProperty(ref _minAvatarVisibility, value);
        }

        private double _minAvatarHeight;

        /// <summary>
        /// 小头像高度
        /// </summary>
        public double MinAvatarHeight
        {
            get => _minAvatarHeight;
            set => SetProperty(ref _minAvatarHeight, value);
        }

        protected virtual void LeftContentButtonAction()
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
            set => SetProperty(ref _menuItems, value);
        }

        private int _menuSelectIndex = -1;
        /// <summary>
        /// 菜单选中索引
        /// </summary>
        public int MenuSelectIndex
        {
            get => _menuSelectIndex;
            set => SetProperty(ref _menuSelectIndex, value);
        }

        private int _tabItemSelectedIndex = -1;
        /// <summary>
        /// tabItem选中项
        /// </summary>
        public int TabItemSelectedIndex
        {
            get => _tabItemSelectedIndex;
            set => SetProperty(ref _tabItemSelectedIndex, value);
        }


        private ObservableCollection<MenuBar> _tabItems = new ObservableCollection<MenuBar>();
        /// <summary>
        /// tab项
        /// </summary>
        public ObservableCollection<MenuBar> TabItems
        {
            get => _tabItems;
            set => SetProperty(ref _tabItems, value);
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

        protected virtual void MenuNavigate(MenuBar menu)
        {
            //var aa = _regionManager.;

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

        protected virtual void TabItemChange(MenuBar? menu)
        {
            if (menu == null) return;
            if (_menuSelectIndex != menu.Index)
            {
                MenuSelectIndex = menu.Index;
            }
        }

        protected virtual void TabItemClose(MenuBar menu)
        {
            menu.TabItemInfo.Index = -1;
            menu.TabItemInfo.Content = null;
            TabItems.Remove(menu);
            TabItemIndexReset();
        }

        private void TabContentResolve(MenuBar menu)
        {
            if (menu.TabItemInfo.Content != null || menu.TabItemInfo.ViewType == null) return;
            var userControl = _containerProvider.Resolve(menu.TabItemInfo.ViewType);
            if (userControl is UserControl control)
            {
                menu.TabItemInfo.Content = control;
            }
        }

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