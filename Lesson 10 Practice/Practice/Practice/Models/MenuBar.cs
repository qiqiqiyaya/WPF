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
        public int Index { get; protected set; } = -1;

        public string Icon { get; set; }

        public string Title { get; set; }

        public string NameSpace { get; set; }

        /// <summary>
        /// 当前菜单是否被选中
        /// </summary>
        public bool IsSelectedMenu { get; protected set; }

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
        /// 设置选中状态
        /// </summary>
        /// <param name="isSelected"></param>
        public void SetSelectedState(bool isSelected)
        {
            IsSelectedMenu = isSelected;
        }
    }
}
