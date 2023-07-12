using Prism.Mvvm;

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
}
