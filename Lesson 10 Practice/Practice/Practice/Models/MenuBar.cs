using System.Windows.Controls;
using Prism.Mvvm;
#pragma warning disable CS8618

namespace Practice.Models
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class MenuBar : BindableBase
    {
        public string Id { get; set; }

        /// <summary>
        /// 默认值 -1
        /// </summary>
        public int Index { get; protected set; } = -1;

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

        /// <summary>
        /// 设置索引
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index)
        {
            Index = index;
        }

        /// <summary>
        /// 获取 视图模型
        /// </summary>
        /// <returns></returns>
        public object? GetViewModel()
        {
            var userControl = (UserControl)TabItemMenu.UserControl;
            var viewModel = userControl?.DataContext;
            return viewModel;
        }
    }
}
