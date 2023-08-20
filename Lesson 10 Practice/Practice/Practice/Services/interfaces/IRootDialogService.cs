using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace Practice.Services.Interfaces
{
    /// <summary>
    /// 根级对话窗口，单例服务
    /// </summary>
    public interface IRootDialogService
    {
        void Init(DialogHost rooDialogHost);

        void Show(object content);

        void Close();

        void DelayThenClose(int delay = 250);

        /// <summary>
        /// loading 加载状态展示，默认延时 500ms ，防止画面一闪而过。
        /// </summary>
        /// <param name="delay"></param>
        Task LoadingShowAsync(int delay = 250);

        /// <summary>
        /// 加载状态关闭
        /// </summary>
        void LoadingClose();
    }
}
