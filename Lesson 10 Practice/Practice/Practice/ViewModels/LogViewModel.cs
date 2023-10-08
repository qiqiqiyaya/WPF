using Newtonsoft.Json;
using Practice.Common;
using Practice.Core.BaseModels;
using Practice.Dtos.Inputs;
using Practice.Models;
using Practice.Provider.interfaces;
using Practice.Services;
using Practice.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Practice.ViewModels
{
    public class LogViewModel : PaginationBaseViewModel, IDisposable
    {
        private readonly SafetyUiActionService _safetyUiActionService;
        private readonly IRootDialogService _rootDialogService;
        private readonly ISnackbarService _snackbarService;

        /* 范围服务 */
        private readonly IScopedProvider _scopedProvider;
        private readonly ILogProvider _logProvider;
        /* 范围服务 */

        /// <summary>
        /// 查看详情
        /// </summary>
        public DelegateCommand<LogDetail> ViewDetailCommand { get; }

        /// <summary>
        /// 剪切板
        /// </summary>
        public DelegateCommand<LogDetail> CopyCommand { get; }

        public DelegateCommand PageChangedCommand { get; }

        /// <summary>
        /// 查询操作
        /// </summary>
        public DelegateCommand SearchCommand { get; }

        public LogViewModel(
            SafetyUiActionService safetyUiActionService,
            IContainerExtension containerProvider,
            IRootDialogService rootDialogService,
            ISnackbarService snackbarService,
            IPaginationService pagination) : base(pagination)
        {
            _rootDialogService = rootDialogService;
            _safetyUiActionService = safetyUiActionService;
            _snackbarService = snackbarService;

            _scopedProvider = containerProvider.CreateScope();
            _logProvider = _scopedProvider.Resolve<ILogProvider>();

            ViewDetailCommand = new DelegateCommand<LogDetail>(OpenLogDetail);
            CopyCommand = new DelegateCommand<LogDetail>(Copy);
            PageChangedCommand = new DelegateCommand(PageChanged);
            SearchCommand = new DelegateCommand(() => LoadingData(true));

            PaginationShow = Visibility.Visible;
            PageNumber = 1;

            LoadingData();
        }

        private readonly ObservableCollection<LogDetail> _logs = new ObservableCollection<LogDetail>();
        public ObservableCollection<LogDetail> Logs
        {
            get => _logs;
            set => this.RaiseAndSetIfChanged(ref value, _logs);
        }

        private LogSearchInput _input = new LogSearchInput();
        /// <summary>
        /// 日志查询条件输入值
        /// </summary>
        public LogSearchInput Input
        {
            get => _input;
            set => this.RaiseAndSetIfChanged(ref value, _input);
        }

        protected void LoadingData(bool toSearch = false)
        {
            LogSearchInput? input = null;
            if (toSearch) input = _input;

            _safetyUiActionService.SafetyTaskRun(async () =>
            {
                await _rootDialogService.LoadingShowAsync();
                PageList<List<LogDetail>> pageList;
                if (input != null)
                {
                    pageList = await _logProvider.GetPageListAsync(input, PageNumber, PageSize);
                }
                else
                {
                    pageList = await _logProvider.GetPageListAsync(PageNumber, PageSize);
                }

                // 界面数据清空，分页按钮执行
                _safetyUiActionService.Invoke(() =>
                {
                    Logs.Clear();
                    Total = pageList.Count;
                });

                // 防止ui阻塞，分批添加数据
                await _safetyUiActionService.NonBlockingAdd(pageList.Data, Logs);
                // 加载框关闭友好型
                _rootDialogService.LoadingClose();
            });
        }

        protected void OpenLogDetail(LogDetail detail)
        {
            _rootDialogService.Show(new LogDetailView(detail));
        }

        protected void Copy(LogDetail detail)
        {
            Clipboard.SetText(JsonConvert.SerializeObject(detail));
            _snackbarService.Show("已复制到粘贴板！", 800);
        }

        public override void PageChanged()
        {
            LoadingData();
        }

        public void Dispose()
        {
            _scopedProvider.Dispose();
        }
    }
}
