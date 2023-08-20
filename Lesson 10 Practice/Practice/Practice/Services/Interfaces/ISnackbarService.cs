using MaterialDesignThemes.Wpf;

namespace Practice.Services.Interfaces
{
    public interface ISnackbarService
    {
        void Init(Snackbar snackbar);

        void Show(string message, int duration = 3000);

        /// <summary>
        /// 展示消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="duration">持续时间，默认2000毫秒</param>
        void ShowImmediately(string message, int duration = 2000);
    }
}
