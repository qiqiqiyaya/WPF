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
