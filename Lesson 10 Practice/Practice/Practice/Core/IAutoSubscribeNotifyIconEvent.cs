using Practice.Events;

namespace Practice.Core
{
    /// <summary>
    /// 订阅 <see cref="NotifyIconEvent"/> 事件 
    /// </summary>
    /// <remarks>
    /// 当tab菜单处于主界面选择的View时，才会触发
    /// </remarks>
    public interface IAutoSubscribeNotifyIconEvent
    {
        /// <summary>
        /// 订阅 <see cref="NotifyIconEvent"/> 事件
        /// </summary>
        /// <param name="state"></param>
        void NotifyIconEventSubscribe(PracticeWindowState state);
    }
}
