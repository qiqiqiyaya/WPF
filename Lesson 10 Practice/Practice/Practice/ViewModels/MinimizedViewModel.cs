using MaterialDesignThemes.Wpf;
using Practice.Core;
using Practice.Services;
using Practice.Services.Interfaces;
using Prism.Commands;
using ReactiveUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using Practice.Events;
using Practice.Extensions;

namespace Practice.ViewModels
{
    public class MinimizedViewModel : ReactiveObject, ITabItemMenuChangeAction, IDisposable, IAutoSubscribeNotifyIconEvent
    {
        /// <summary>
        /// 重置最小化配置命令
        /// </summary>
        public DelegateCommand ResetCommand { get; }
        private readonly SafetyUiActionService _safetyUiActionService;
        private readonly INotifyIconService _notifyIconService;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;

        public MinimizedViewModel(SafetyUiActionService safetyUiActionService,
            INotifyIconService notifyIconService
            )
        {
            _safetyUiActionService = safetyUiActionService;
            _notifyIconService = notifyIconService;
            ResetCommand = new DelegateCommand(Reset);
            _resetIcon = IconRestart;

            Init();
        }

        private void Init()
        {
            _cancellationToken = _cancellationTokenSource.Token;
            // 注册个回调委托，当切换tab菜单，或者关闭该菜单时执行
            _cancellationToken.Register(() =>
            {
                _isRunning = false;
            });

            if (ButtonProgressValue != 0)
            {
                this.Reset();
            }
        }

        protected static PackIcon IconRestart = new PackIcon() { Kind = PackIconKind.Restart };
        protected static PackIcon IconCheck = new PackIcon() { Kind = PackIconKind.Check };

        private PackIcon _resetIcon;
        public PackIcon ResetIcon
        {
            get => _resetIcon;
            set => this.RaiseAndSetIfChanged(ref _resetIcon, value);
        }

        /// <summary>
        /// 缓存下进度
        /// </summary>
        private static double _buttonProgressValue;
        public double ButtonProgressValue
        {
            get => _buttonProgressValue;
            set => this.RaiseAndSetIfChanged(ref _buttonProgressValue, value);
        }

        /// <summary>
        /// 是否正在重置中
        /// </summary>
        private bool _isRunning;

        private void Reset()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;
            Task.Run(async () =>
            {
                double count = ButtonProgressValue == 0 ? 100 : 100 - ButtonProgressValue;
                for (int i = 0; i < count; i++)
                {
                    await Task.Delay(30, _cancellationToken);
                    _safetyUiActionService.Invoke(() => ButtonProgressValue++);
                }

                _safetyUiActionService.Invoke(() => ResetIcon = IconCheck);
                await Task.Delay(500, _cancellationToken);

                _notifyIconService.ResetConfiguration();
                _safetyUiActionService.Invoke(() =>
                {
                    ResetIcon = IconRestart;
                    ButtonProgressValue = 0;
                });
                _isRunning = false;
            }, _cancellationToken).FireAndForget();
        }

        public void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Init();
        }

        public void OnLeave()
        {
            _cancellationTokenSource.CancelAndDispose();
        }

        public void Dispose()
        {
            _cancellationTokenSource.CancelAndDispose();
        }

        public void NotifyIconEventSubscribe(PracticeWindowState state)
        {
            switch (state)
            {
                case PracticeWindowState.Normal:
                case PracticeWindowState.Maximized:
                    this.OnEnter();
                    break;

                case PracticeWindowState.Minimized:
                case PracticeWindowState.Tray:
                    this.OnLeave();
                    break;
            }
        }
    }
}
