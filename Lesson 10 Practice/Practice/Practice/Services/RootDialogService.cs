using MaterialDesignThemes.Wpf;
using Practice.CommonViews;
using Practice.Extensions;
using Practice.Services.Interfaces;
using System.Threading.Tasks;
#pragma warning disable CS8618

namespace Practice.Services
{
    /// <summary>
    /// 单例服务
    /// </summary>
    public class RootDialogService : IRootDialogService
    {
        private readonly SafetyUiActionService _safetyUiActionService;
        private DialogHost _rooDialogHost;

        public RootDialogService(SafetyUiActionService safetyUiActionService)
        {
            _safetyUiActionService = safetyUiActionService;
        }

        public void Init(DialogHost rooDialogHost)
        {
            _rooDialogHost = rooDialogHost;
        }

        public void Show(object content)
        {
            _safetyUiActionService.Invoke(() =>
            {
                Check.NotNull(content, nameof(content));
                _rooDialogHost.ShowDialog(content);
            });
        }

        public void Close()
        {
            _safetyUiActionService.Invoke(() =>
            {
                // 关闭 对话窗口
                _rooDialogHost.CurrentSession?.Close();
                // 切断对话窗口对象的引用
                _rooDialogHost.CurrentSession?.UpdateContent(null);
            });
        }

        public void DelayThenClose(int delay = 250)
        {
            _safetyUiActionService.DelayWhen(() =>
            {
                _rooDialogHost.CurrentSession?.Close();
                _rooDialogHost.CurrentSession?.UpdateContent(null);
            }, delay);
        }

        /// <summary>
        /// loading 加载状态展示，默认延时 500ms ，防止画面一闪而过。
        /// </summary>
        /// <param name="delay"></param>
        public async Task LoadingShowAsync(int delay = 250)
        {
            _safetyUiActionService.Invoke(() =>
            {
                _rooDialogHost.ShowDialog(new LoadingView());
            });
            await Task.Delay(delay);
        }

        /// <summary>
        /// 加载状态关闭
        /// </summary>
        public void LoadingClose()
        {
            Close();
        }
    }
}
