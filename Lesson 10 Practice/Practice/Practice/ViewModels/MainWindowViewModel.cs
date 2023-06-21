using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Practice.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand LeftContentButtonCommand { get; }

        public MainWindowViewModel()
        {
            LeftContentButtonCommand = new DelegateCommand(LeftContentButtonAction);
            this.Init();
        }

        private void Init()
        {
            IconMenuClose = new PackIcon() { Kind = PackIconKind.MenuClose, Width = 24, Height = 24 };
            IconMenuOpen = new PackIcon() { Kind = PackIconKind.MenuOpen, Width = 24, Height = 24 };
            _leftContentButtonIcon = IconMenuOpen;
        }

        #region LeftMenu

        /// <summary>
        /// 指示左侧菜单是否收缩
        /// </summary>
        private bool _leftMenuClose = false;

        private double _leftContentWidth = 200;
        /// <summary>
        /// 左侧内容区域宽度，菜单折叠宽 80 ，菜单展开宽 200
        /// </summary>
        public double LeftContentWidth
        {
            get => _leftContentWidth;
            set => SetProperty(ref _leftContentWidth, value);
        }

        protected PackIcon IconMenuClose;
        protected PackIcon IconMenuOpen;
        private PackIcon _leftContentButtonIcon;
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
        public string HostName => System.Environment.MachineName;

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

        private double _minAvatarHeight = 0;
        /// <summary>
        /// 小头像高度
        /// </summary>
        public double MinAvatarHeight
        {
            get => _minAvatarHeight;
            set => SetProperty(ref _minAvatarHeight, value);
        }

        private void LeftContentButtonAction()
        {
            if (_leftMenuClose)
            {
                LeftContentWidth = 200;
                LeftContentButtonIcon = IconMenuOpen;
                BigAvatarVisibility = Visibility.Visible;
                MinAvatarVisibility = Visibility.Hidden;
                MinAvatarHeight = 0;
                BigAvatarHeight = 200;
            }
            else
            {
                LeftContentWidth = 80;
                LeftContentButtonIcon = IconMenuClose;
                BigAvatarVisibility = Visibility.Hidden;
                MinAvatarVisibility = Visibility.Visible;
                MinAvatarHeight = 80;
                BigAvatarHeight = 0;
            }

            _leftMenuClose = !_leftMenuClose;
        }
        #endregion
    }
}
