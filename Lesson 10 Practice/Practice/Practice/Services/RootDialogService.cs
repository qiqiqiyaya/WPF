using MaterialDesignThemes.Wpf;
using Practice.CommonViews;
using Practice.Extensions;
using Practice.Services.Interfaces;
using System.Threading.Tasks;
#pragma warning disable CS8618

namespace Practice.Services
{
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
                _rooDialogHost.DialogContent = content;
                _rooDialogHost.IsOpen = true;
            });
        }

        public void Close()
        {
            _safetyUiActionService.Invoke(() =>
            {
                _rooDialogHost.DialogContent = null;
                _rooDialogHost.IsOpen = false;
            });
        }

        /// <summary>
        /// loading 加载状态展示
        /// </summary>
        public void LoadingShow()
        {
            _rooDialogHost.DialogContent = new LoadingView();
            _rooDialogHost.IsOpen = true;
        }

        /// <summary>
        /// loading 加载状态展示，默认延时 500ms ，防止画面一闪而过。
        /// </summary>
        /// <param name="delay"></param>
        public async Task LoadingShowAsync(int delay = 500)
        {
            _safetyUiActionService.Invoke(() =>
            {
                _rooDialogHost.DialogContent = new LoadingView();
                _rooDialogHost.IsOpen = true;
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
