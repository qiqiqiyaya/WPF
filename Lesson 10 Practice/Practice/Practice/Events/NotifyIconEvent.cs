using Prism.Events;

namespace Practice.Events
{
    /// <summary>
    /// 最大最小化，最小化到托盘事件
    /// </summary>
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
