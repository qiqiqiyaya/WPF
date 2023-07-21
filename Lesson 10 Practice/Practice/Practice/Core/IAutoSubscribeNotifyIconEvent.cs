using Practice.Events;

namespace Practice.Core
{
    /// <summary>
    /// 订阅 <see cref="NotifyIconEvent"/> 事件 
    /// </summary>
    public interface IAutoSubscribeNotifyIconEvent
    {
        /// <summary>
        /// 订阅 <see cref="NotifyIconEvent"/> 事件
        /// </summary>
        /// <param name="state"></param>
        void Subscribe(PracticeWindowState state);
    }
}
