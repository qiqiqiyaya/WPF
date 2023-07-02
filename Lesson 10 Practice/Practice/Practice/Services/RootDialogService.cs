using MaterialDesignThemes.Wpf;
using Practice.CommonViews;
using Practice.Services.Contract;
using Practice.Services.interfaces;
using System.Threading.Tasks;

namespace Practice.Services
{
    public class RootDialogService : IRootDialogService
    {
        private readonly SafetyUiAction _safetyUiAction;

        public RootDialogService(SafetyUiAction safetyUiAction)
        {
            _safetyUiAction = safetyUiAction;
        }

        public async Task LoadingShow()
        {
            await DialogHost.Show(new LoadingView(), SystemSettingKeys.RootDialogIdentity);
        }

        public void LoadingClose()
        {
            DialogHost.Close(SystemSettingKeys.RootDialogIdentity);
        }
    }
}
