using DynamicData;
using Practice.Core.Configuration;
using Practice.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Practice.Services
{
    /// <summary>
    /// 安全的ui操作，单例服务
    /// </summary>
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

        /// <summary>
        /// 非阻塞添加。每次在UI线程中向 <see cref="target"/> 添加最大 <see cref="PaginationConfiguration.NonBlockingAddSize"/> 行数据,
        /// 然后休眠指定时间 <see cref="delay"/> 再次在UI线程中添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="target">目标容器</param>
        /// <param name="continueWith">后续操作</param>
        /// <param name="size">每次添加多少条数据</param>
        /// <param name="delay">等待时间</param>
        public async Task NonBlockingAdd<T>(IList<T> source,
            IList<T> target,
            Action? continueWith,
            int size = PaginationConfiguration.NonBlockingAddSize,
            int delay = 5)
        {
            int loopTimes;
            if (source.Count <= size)
            {
                loopTimes = 1;
            }
            else
            {
                loopTimes = source.Count % size == 0 ? source.Count / size : source.Count / size + 1;
            }

            for (int i = 0; i < loopTimes; i++)
            {
                // ReSharper disable once AccessToModifiedClosure
                UiDispatcher.Invoke(() => target.AddRange(source.Skip(i * size).Take(size)));
                await Task.Delay(delay);
            }

            //Task.Run(async () =>
            //    {

            //    }).ContinueWith(task =>
            //    {
            //        continueWith?.Invoke();
            //    })
            //    .FireAndForget();
        }

        /// <summary>
        /// 非阻塞添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">待添加的数据</param>
        /// <param name="container">容器</param>
        /// <param name="size">每次添加多少条数据</param>
        /// <param name="delay">等待时间</param>
        public Task NonBlockingAdd<T>(IList<T> data, IList<T> container, int size = PaginationConfiguration.NonBlockingAddSize, int delay = 5)
        {
            return NonBlockingAdd(data, container, null, size, delay);
        }

        /// <summary>
        /// 非阻塞添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">待添加的数据</param>
        /// <param name="container">容器</param>
        /// <param name="size">每次添加多少条数据</param>
        /// <param name="delay">等待时间</param>
        public void NonBlockingAddWithNewTask<T>(IList<T> data, IList<T> container, int size = PaginationConfiguration.NonBlockingAddSize, int delay = 5)
        {
#pragma warning disable CS4014
            NonBlockingAdd(data, container, null, size, delay);
#pragma warning restore CS4014
        }

        /// <summary>
        /// 执行异步操作，action 内部，使用 Invoke 去执行
        /// </summary>
        /// <param name="action"></param>
        public void TaskRun(Func<Task> action)
        {
            Task.Run(async () =>
            {
                await action();
            }).FireAndForget();
        }
    }
}
