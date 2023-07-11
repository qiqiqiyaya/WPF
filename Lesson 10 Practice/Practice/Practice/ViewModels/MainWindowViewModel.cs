using MaterialDesignThemes.Wpf;
using Practice.Models;
using Practice.Services;
using Prism.Commands;
using ReactiveUI;
using System;
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
        public DelegateCommand<MenuBar> TabItemMenuChangeCommand { get; }
        /// <summary>
        /// 关闭指定 tab 菜单命令
        /// </summary>
        public DelegateCommand<MenuBar> TabItemMenuCloseCommand { get; }
        /// <summary>
        /// 关闭所有菜单命令，除 Home 外
        /// </summary>
        public DelegateCommand TabItemMenuCloseAllCommand { get; }

        public MainWindowViewModel(
            MenuManager menuManager)
        {
            MenuManager = menuManager;
            MenuNavigateCommand = new DelegateCommand<MenuBar>(menuManager.MenuNavigate);
            LeftContentSwitchCommand = new DelegateCommand(LeftContentSwitch);
            TabItemMenuChangeCommand = new DelegateCommand<MenuBar>(menuManager.TabItemMenuChange);
            TabItemMenuCloseCommand = new DelegateCommand<MenuBar>(menuManager.TabItemMenuClose);
            TabItemMenuCloseAllCommand = new DelegateCommand(menuManager.TabItemMenuCloseAll);
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

        public MenuManager MenuManager { get; }
    }
}