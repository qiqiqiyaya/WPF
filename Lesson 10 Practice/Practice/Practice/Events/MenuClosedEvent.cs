using Practice.Models;
using Prism.Events;

namespace Practice.Events
{
    /// <summary>
    /// tab栏菜单关闭后
    /// </summary>
    public class MenuClosedEvent : PubSubEvent<MenuBar>
    {

    }
}
