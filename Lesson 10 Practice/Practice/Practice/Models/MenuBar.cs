using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Models
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class MenuBar : BindableBase
    {
        /// <summary>
        /// 默认值 -1
        /// </summary>
        public int Index { get; set; } = -1;

        public string Icon { get; set; }

        public string Title { get; set; }

        public string NameSpace { get; set; }

        private TabItemMenu _tabItemMenu;
        /// <summary>
        /// tab项目信息内容
        /// </summary>
        public TabItemMenu TabItemMenu
        {
            get => _tabItemMenu;
            set => SetProperty(ref _tabItemMenu, value);
        }
    }

    public class TabItemMenu : BindableBase
    {
        public TabItemMenu(Type viewType, Visibility closeBtn = Visibility.Visible)
        {
            ViewType = viewType;
            CloseBtn = closeBtn;
        }

        /// <summary>
        /// tab 上的索引
        /// </summary>
        /// <remarks>
        /// 默认值 -1
        /// </remarks>
        public int Index { get; set; } = -1;

        /// <summary>
        /// 前台视图类型
        /// </summary>
        public Type ViewType { get; }

        /// <summary>
        /// 关闭按钮显示与否
        /// </summary>
        public Visibility CloseBtn { get; }

        private object _userControl;

        public object UserControl
        {
            get => _userControl;
            set => SetProperty(ref _userControl, value);
        }

        public void Reset()
        {
            Index = -1;
            UserControl = null;
        }
    }
}
