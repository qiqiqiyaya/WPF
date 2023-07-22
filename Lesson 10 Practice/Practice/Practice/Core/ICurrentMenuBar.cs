using Practice.Models;

namespace Practice.Core
{
    /// <summary>
    /// 当前菜单
    /// </summary>
    /// <remarks>
    /// 线程安全
    /// </remarks>
    public interface ICurrentMenuBar
    {
        /// <summary>
        /// 当前选中的菜单
        /// </summary>
        MenuBar CurrentMenuBar { get; }

        /// <summary>
        /// 设置当前选中菜单
        /// </summary>
        /// <param name="bar"></param>
        void SetCurrentMenuBar(MenuBar bar);
    }
}
