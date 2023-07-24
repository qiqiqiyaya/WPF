using Practice.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Practice.Services
{
    public class SafetyUiActionService
    {
        public Dispatcher UiDispatcher { get; protected set; }

        public SafetyUiActionService(Dispatcher uiDispatcher)
        {
            UiDispatcher = uiDispatcher;
        }

        /// <summary>
        /// 延迟指定时间(秒)后，执行操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public void DelayWhen(Action action, int delay)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                UiDispatcher.Invoke(action);
            }).FireAndForget();
        }

        /// <summary>
        /// 执行操作后，在执行等待指定时间
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public void ActionThenDelay(Action action, int delay)
        {
            Task.Run(async () =>
            {
                UiDispatcher.Invoke(action);
                await Task.Delay(delay);
            }).FireAndForget();
        }

        /// <summary>
        /// 异步执行操作，然后再执行UI操作
        /// </summary>
        /// <param name="function"></param>
        public void AsyncInvokeThenUiAction(Func<Task<Action>> function)
        {
            Task.Run(async () =>
            {
                var uiAction = await function();
                UiDispatcher.Invoke(uiAction);
            }).FireAndForget();
        }

        /// <summary>
        /// 执行UI操作
        /// </summary>
        /// <param name="action"></param>
        public void Invoke(Action action)
        {
            UiDispatcher.Invoke(action);
        }
    }
}
