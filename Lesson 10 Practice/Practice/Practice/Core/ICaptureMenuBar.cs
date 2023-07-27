using Practice.Models;

namespace Practice.Core
{
    /// <summary>
    /// 当前菜单
    /// </summary>
    /// <remarks>
    /// 线程安全
    /// </remarks>
    public interface ICaptureMenuBar
    {
        /// <summary>
        /// 当前选中的菜单
        /// </summary>
        MenuBar CurrentMenuBar { get; }
    }
}
