using Serilog;
using System;
using System.Threading.Tasks;

namespace Practice.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// 触发后，遗忘此操作
        /// </summary>
        /// <remarks>
        /// <see cref="OperationCanceledException"/> 属于任务被取消，不会粗放此处异常
        /// </remarks>
        /// <param name="task"></param>
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception != null)
                {
                    var ex = t.Exception.GetBaseException();
                    Log.Information(ex, "The exception occurrences in task.");
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
