using Practice.Extensions;
using Practice.Models;

namespace Practice.Core
{
    // 单例服务
    // 提供给其他服务使用，将当前菜单从 MenuManager 中玻璃出来
    public class CacheCurrentMenuBar : ICurrentMenuBar
    {
        //private readonly object _lock = new object();

        // 程序初始化后，该变量不可能为 null
        private MenuBar _currentMenuBar;

        public MenuBar CurrentMenuBar => _currentMenuBar;

        public void SetCurrentMenuBar(MenuBar bar)
        {
            Check.NotNull(bar, nameof(MenuBar));
            //lock (_lock)
            //{
            //    _currentMenuBar = bar;
            //}

            // 菜单值会由 MenuManager 操作，且都是有UI线程（单个线程）操作，故目前不存线程安全问题
            _currentMenuBar = bar;
        }
    }
}
