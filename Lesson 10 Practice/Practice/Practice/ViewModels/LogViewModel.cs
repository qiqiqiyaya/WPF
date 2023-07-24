using Newtonsoft.Json;
using Practice.Common;
using Practice.Extensions;
using Practice.Models;
using Practice.Provider.interfaces;
using Practice.Services;
using Practice.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Practice.ViewModels
{
    public class LogViewModel : ReactiveObject
    {
        private readonly SafetyUiActionService _safetyUiActionService;
        private readonly IContainerExtension _containerProvider;
        private readonly IRootDialogService _rootDialogService;

        /// <summary>
        /// 查看详情
        /// </summary>
        public DelegateCommand<LogDetail> ViewDetailCommand { get; }

        /// <summary>
        /// 剪切板
        /// </summary>
        public DelegateCommand<LogDetail> CopyCommand { get; }

        public LogViewModel(
            SafetyUiActionService safetyUiActionService,
            IContainerExtension containerProvider,
            IRootDialogService rootDialogService)
        {
            _rootDialogService = rootDialogService;
            _safetyUiActionService = safetyUiActionService;
            _containerProvider = containerProvider;
            ViewDetailCommand = new DelegateCommand<LogDetail>(OpenLogDetail);
            CopyCommand = new DelegateCommand<LogDetail>(Copy);

            Init();
        }

        private ObservableCollection<LogDetail> _logs = new ObservableCollection<LogDetail>();
        public ObservableCollection<LogDetail> Logs
        {
            get => _logs;
            set => this.RaiseAndSetIfChanged(ref value, _logs);
        }

        protected void Init()
        {
            _safetyUiActionService.AsyncInvokeThenUiAction(async () =>
            {
                await _rootDialogService.LoadingShowAsync();
                PageList<List<LogDetail>>? list = null;

                await _containerProvider.NewScopeAsync(async provider =>
                {
                    var logProvider = provider.Resolve<ILogProvider>();
                    list = await logProvider.GetPageList(1);
                });

                return () =>
                {
                    Logs.AddRange(list!.Data);
                    _rootDialogService.LoadingClose();
                };
            });
        }

        protected void OpenLogDetail(LogDetail detail)
        {
            _rootDialogService.Show(new LogDetailView(detail));
        }

        protected void Copy(LogDetail detail)
        {
            Clipboard.SetText(JsonConvert.SerializeObject(detail));
        }
    }
}
