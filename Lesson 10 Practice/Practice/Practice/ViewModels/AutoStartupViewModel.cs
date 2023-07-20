using Practice.Services.Interfaces;
using Prism.Commands;
using ReactiveUI;

namespace Practice.ViewModels
{
    /// <summary>
    /// 开机自启动
    /// </summary>
    public class AutoStartupViewModel : ReactiveObject
    {
        private readonly IAutoStartupService _autoStartupService;

        public DelegateCommand CheckChangeCommand { get; }
        public DelegateCommand CheckForAllUsersChangeCommand { get; }

        public AutoStartupViewModel(IAutoStartupService autoStartupService)
        {
            _autoStartupService = autoStartupService;
            CheckChangeCommand = new DelegateCommand(() =>
            {
                if (IsCheck)
                {
                    _autoStartupService.Enable();
                }
                else
                {
                    _autoStartupService.Disable();
                }
            });
            CheckForAllUsersChangeCommand = new DelegateCommand(() =>
            {
                if (IsCheckForAllUsers)
                {
                    _autoStartupService.Enable(true);
                }
                else
                {
                    _autoStartupService.Disable(true);
                }
            });

            this.IsCheck = _autoStartupService.IsAutoStartup();
            this.IsCheckForAllUsers = _autoStartupService.IsAutoStartup(true);
        }

        private bool _isCheck;

        public bool IsCheck
        {
            get => _isCheck;
            set => this.RaiseAndSetIfChanged(ref _isCheck, value);
        }


        private bool _isCheckForAllUsers;

        public bool IsCheckForAllUsers
        {
            get => _isCheck;
            set => this.RaiseAndSetIfChanged(ref _isCheckForAllUsers, value);
        }
    }
}
