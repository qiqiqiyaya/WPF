using Practice.Events;
using Practice.Models;
using Prism.Events;
#pragma warning disable CS8618

namespace Practice.Core
{
    // 单例服务
    // 提供给其他服务使用，将当前菜单从 MenuManager 中玻璃出来
    public class CacheCurrentMenuBar : ICaptureMenuBar
    {
        public CacheCurrentMenuBar(IEventAggregator eventAggregator)
        {
            // 订阅 菜单变更事件
            eventAggregator.GetEvent<MenuChangedEvent>().Subscribe(menu =>
            {
                CurrentMenuBar = menu;
            });
        }

        // 程序初始化后，该变量不可能为 null
        public MenuBar CurrentMenuBar
        {
            get;
            protected set;
        }
    }
}
