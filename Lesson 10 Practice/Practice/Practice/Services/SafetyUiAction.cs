using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Practice.Services
{
    public class SafetyUiAction
    {
        public Dispatcher UiDispatcher { get; protected set; }

        public SafetyUiAction(Dispatcher uiDispatcher)
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

        /// <summary>
        /// 延迟指定时间后，执行操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public void DelayWhen(Func<Task> action, int delay)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                await UiDispatcher.InvokeAsync(action);
            });
        }

        /// <summary>
        /// 延迟指定时间后，执行操作
        /// </summary>
        /// <param name="action"></param>
        public void InvokeAsync(Func<Task> action)
        {
            Task.Run(async () =>
            {
                await UiDispatcher.InvokeAsync(action);
            });
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
