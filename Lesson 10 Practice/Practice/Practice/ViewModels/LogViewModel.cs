using Newtonsoft.Json;
using Practice.Common;
using Practice.Core;
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

        public DelegateCommand PageChangedCommand { get; }

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
            PageChangedCommand = new DelegateCommand(PageChanged);

            Init();
        }

        private int _pageSize = SystemSettingKeys.RowNumber;
        public int PageSize
        {
            get => _pageSize;
            set => this.RaiseAndSetIfChanged(ref _pageSize, value);
        }

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => this.RaiseAndSetIfChanged(ref _pageNumber, value);
        }

        private int _total = 0;
        public int Total
        {
            get => _total;
            set => this.RaiseAndSetIfChanged(ref _total, value);
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
                PageList<List<LogDetail>>? pageList = null;

                await _containerProvider.NewScopeAsync(async provider =>
                {
                    var logProvider = provider.Resolve<ILogProvider>();
                    pageList = await logProvider.GetPageList(PageNumber, PageSize);
                });

                return () =>
                {
                    Logs.Clear();
                    Logs.AddRange(pageList!.Data);
                    Total = pageList.Count;
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

        protected void PageChanged()
        {
            Init();
        }
    }
}
