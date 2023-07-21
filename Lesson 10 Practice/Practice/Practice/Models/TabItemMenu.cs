using Prism.Mvvm;
using System;
using System.Windows;

namespace Practice.Models
{
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
        public int Index { get; private set; } = -1;

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

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void Reset()
        {
            Index = -1;
            UserControl = null!;
        }
    }
}
