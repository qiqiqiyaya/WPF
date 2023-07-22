using Practice.Events;

namespace Practice.Core
{
    public interface IAutoSubscribeNotifyIconEventHandler
    {
        /// <summary>
        /// 自动订阅 <see cref="NotifyIconEvent"/> 事件
        /// </summary>
        void Init();
    }
}
