using Newtonsoft.Json;
using Practice.Common;
using Practice.Core.BaseModels;
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
    public class LogViewModel : PaginationBaseViewModel
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

        public DelegateCommand PageChangedCommand { get; }

        public LogViewModel(
            SafetyUiActionService safetyUiActionService,
            IContainerExtension containerProvider,
            IRootDialogService rootDialogService,
            IPaginationService pagination) : base(pagination)
        {
            _rootDialogService = rootDialogService;
            _safetyUiActionService = safetyUiActionService;
            _containerProvider = containerProvider;
            ViewDetailCommand = new DelegateCommand<LogDetail>(OpenLogDetail);
            CopyCommand = new DelegateCommand<LogDetail>(Copy);
            PageChangedCommand = new DelegateCommand(PageChanged);

            PaginationShow = Visibility.Visible;
            PageNumber = 1;

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
            _safetyUiActionService.TaskRun(async () =>
            {
                await _rootDialogService.LoadingShowAsync();
                PageList<List<LogDetail>>? pageList = null;

                await _containerProvider.NewScopeAsync(async provider =>
                {
                    var logProvider = provider.Resolve<ILogProvider>();
                    pageList = await logProvider.GetPageList(PageNumber, PageSize);
                });

                _safetyUiActionService.Invoke(() =>
                {
                    Logs.Clear();
                    //Logs.AddRange(pageList!.Data);
                    Total = pageList!.Count;
                });

                _safetyUiActionService.NonBlockingAdd(pageList!.Data, Logs, () =>
                {
                    _rootDialogService.LoadingClose();
                });
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

        public override void PageChanged()
        {
            Init();
        }
    }
}
