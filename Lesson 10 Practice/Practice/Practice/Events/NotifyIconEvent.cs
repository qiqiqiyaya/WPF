using Prism.Events;

namespace Practice.Events
{
    public class NotifyIconEvent : PubSubEvent<PracticeWindowState>
    {

    }

    public enum PracticeWindowState
    {
        Normal,
        Minimized,
        Maximized,
        /// <summary>
        /// 托盘化
        /// </summary>
        Tray
    }
}
