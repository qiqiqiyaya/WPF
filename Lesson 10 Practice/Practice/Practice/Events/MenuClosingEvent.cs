using Practice.Models;
using Prism.Events;

namespace Practice.Events
{
    /// <summary>
    /// 菜单关闭前
    /// </summary>
    public class MenuClosingEvent : PubSubEvent<MenuBar>
    {

    }
}
