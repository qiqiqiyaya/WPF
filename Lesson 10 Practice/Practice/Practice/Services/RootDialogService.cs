using MaterialDesignThemes.Wpf;
using Practice.CommonViews;
using Practice.Extensions;
using Practice.Services.Interfaces;

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
            Check.NotNull(content, nameof(content));
            _rooDialogHost.DialogContent = content;
            _rooDialogHost.IsOpen = true;
        }

        public void Close()
        {
            _rooDialogHost.DialogContent = null;
            _rooDialogHost.IsOpen = false;
        }

        public void LoadingStateShow()
        {
            _rooDialogHost.DialogContent = new LoadingView();
            _rooDialogHost.IsOpen = true;
        }

        public void LoadingStateClose()
        {
            Close();
        }
    }
}
