using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace Practice.Services.Interfaces
{
    public interface IRootDialogService
    {
        void Init(DialogHost rooDialogHost);

        void Show(object content);

        void Close();

        void DelayThenClose(int delay = 200);

        /// <summary>
        /// loading 加载状态展示，默认延时 500ms ，防止画面一闪而过。
        /// </summary>
        /// <param name="delay"></param>
        Task LoadingShowAsync(int delay = 500);

        /// <summary>
        /// 加载状态关闭
        /// </summary>
        void LoadingClose();
    }
}
