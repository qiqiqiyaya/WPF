using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Models
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class MenuBar : BindableBase
    {
        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _nameSpace;

        public string NameSpace
        {
            get { return _nameSpace; }
            set { _nameSpace = value; }
        }

        /// <summary>
        /// tab内容
        /// </summary>
        public MenuTabItem MenuTabItem { get; set; }

    }

    public class MenuTabItem
    {
        public Visibility CloseBtn { get; set; } 

        public object Content { get; set; }
    }
}
