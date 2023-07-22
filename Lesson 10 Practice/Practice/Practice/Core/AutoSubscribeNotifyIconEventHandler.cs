using Practice.Events;
using Prism.Events;
using System.Collections.Generic;

namespace Practice.Core
{
    /// <summary>
    /// 单例处理器
    /// </summary>
    public class AutoSubscribeNotifyIconEventHandler : IAutoSubscribeNotifyIconEventHandler
    {
        private readonly Dictionary<string, SubscriptionToken> _container;
        private readonly object _lock = new object();
        private readonly NotifyIconEvent _notifyIconEvent;

        public AutoSubscribeNotifyIconEventHandler(IEventAggregator eventAggregator)
        {
            _notifyIconEvent = eventAggregator.GetEvent<NotifyIconEvent>();
            _container = new Dictionary<string, SubscriptionToken>();
        }

        /// <summary>
        /// 自动订阅 <see cref="NotifyIconEvent"/> 事件
        /// </summary>
        /// <param name="model"></param>
        public void Subscribe(IAutoSubscribeNotifyIconEvent model)
        {
            lock (_lock)
            {
                var token = _notifyIconEvent.Subscribe(model.Subscribe);
                var key = model.GetType().FullName;
                _container.Add(key!, token);
            }
        }

        public void Unsubscribe(IAutoSubscribeNotifyIconEvent model)
        {
            lock (_lock)
            {
                var key = model.GetType().FullName;
                if (_container.TryGetValue(key!, out SubscriptionToken? token))
                {
                    token?.Dispose();
                    _container.Remove(key!);
                }
            }
        }
    }
}
