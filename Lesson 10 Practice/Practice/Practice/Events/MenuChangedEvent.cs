using Practice.Models;
using Prism.Events;

namespace Practice.Events
{
    /// <summary>
    /// 菜单变更之后
    /// </summary>
    public class MenuChangedEvent : PubSubEvent<MenuBar>
    {

    }
}
