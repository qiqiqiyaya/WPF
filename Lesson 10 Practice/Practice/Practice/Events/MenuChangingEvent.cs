using Practice.Models;
using Prism.Events;

namespace Practice.Events
{
    /// <summary>
    /// 菜单变更之前
    /// </summary>
    public class MenuChangingEvent : PubSubEvent<MenuBar?>
    {

    }
}
