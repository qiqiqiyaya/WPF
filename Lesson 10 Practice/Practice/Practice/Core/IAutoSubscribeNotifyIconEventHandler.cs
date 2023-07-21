using Practice.Events;

namespace Practice.Core
{
    public interface IAutoSubscribeNotifyIconEventHandler
    {
        /// <summary>
        /// 自动订阅 <see cref="NotifyIconEvent"/> 事件
        /// </summary>
        /// <param name="model"></param>
        void Subscribe(IAutoSubscribeNotifyIconEvent model);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="model"></param>
        void Unsubscribe(IAutoSubscribeNotifyIconEvent model);
    }
}
