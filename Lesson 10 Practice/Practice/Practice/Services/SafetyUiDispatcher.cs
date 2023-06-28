using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Practice.Services
{
    public class SafetyUiDispatcher
    {
        public Dispatcher UiDispatcher { get; protected set; }

        public SafetyUiDispatcher(Dispatcher uiDispatcher)
        {
            UiDispatcher = uiDispatcher;
        }

        /// <summary>
        /// 延迟指定时间后，执行操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public void DelayWhen(Action action, int delay)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                UiDispatcher.Invoke(action);
            });
        }
    }
}
