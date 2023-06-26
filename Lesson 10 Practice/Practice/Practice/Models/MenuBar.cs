﻿using Prism.Mvvm;
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

        private TabItemInfo _tabItemInfo;
        /// <summary>
        /// tab项目信息内容
        /// </summary>
        public TabItemInfo TabItemInfo
        {
            get => _tabItemInfo;
            set => SetProperty(ref _tabItemInfo, value);
        }
    }

    public class TabItemInfo : BindableBase
    {
        /// <summary>
        /// tab 上的索引，仅用于存储信息
        /// </summary>
        /// <remarks>
        /// 默认值 -1
        /// </remarks>
        public int Index { get; set; } = -1;

        /// <summary>
        /// 前台视图类型
        /// </summary>
        public Type ViewType { get; set; }

        /// <summary>
        /// 关闭按钮显示与否
        /// </summary>
        public Visibility CloseBtn { get; set; }

        private UserControl? _content;
        /// <summary>
        /// TabItem 对应的用户控件
        /// </summary>
        public UserControl? Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
    }
}
