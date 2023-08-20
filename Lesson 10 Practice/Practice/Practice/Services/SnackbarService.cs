using System;
using MaterialDesignThemes.Wpf;
using Practice.Services.Interfaces;

namespace Practice.Services
{
    /// <summary>
    /// 单例服务
    /// </summary>
    public class SnackbarService : ISnackbarService
    {
        private Snackbar _snackbar;

        public void Init(Snackbar snackbar)
        {
            _snackbar = snackbar;
            // 扔掉重复的值
            _snackbar.MessageQueue!.DiscardDuplicates = true;
        }

        /// <summary>
        /// 展示消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="duration">持续时间，默认2000毫秒</param>
        public void Show(string message, int duration = 2000)
        {
            _snackbar.MessageQueue?.Enqueue(
                message,
                null,
                null,
                null,
                false,
                false,
                TimeSpan.FromMilliseconds(duration));
        }

        /// <summary>
        /// 展示消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="duration">持续时间，默认2000毫秒</param>
        public void ShowImmediately(string message, int duration = 2000)
        {
            _snackbar.MessageQueue?.Clear();
            _snackbar.MessageQueue?.Enqueue(
                message,
                null,
                null,
                null,
                false,
                false,
                TimeSpan.FromMilliseconds(duration));
        }
    }
}
