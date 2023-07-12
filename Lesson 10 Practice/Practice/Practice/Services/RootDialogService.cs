using MaterialDesignThemes.Wpf;
using Practice.CommonViews;
using Practice.Services.interfaces;
using System.Threading.Tasks;
using Practice.Core.Contract;

namespace Practice.Services
{
    public class RootDialogService : IRootDialogService
    {
        private readonly SafetyUiAction _safetyUiAction;
        private DialogHost _rooDialogHost;

        public RootDialogService(SafetyUiAction safetyUiAction)
        {
            _safetyUiAction = safetyUiAction;
        }

        public void Init(DialogHost rooDialogHost)
        {
            _rooDialogHost = rooDialogHost;
        }

        public void LoadingShow()
        {
            _rooDialogHost.Content = new LoadingView();
            _rooDialogHost.IsOpen = true;
            //DialogHost.Show(new LoadingView(), SystemSettingKeys.RootDialogIdentity);
        }

        public void LoadingClose()
        {
            _rooDialogHost.Content = null;
            _rooDialogHost.IsOpen = false;
            //DialogHost.Close(SystemSettingKeys.RootDialogIdentity);
        }
    }
}
