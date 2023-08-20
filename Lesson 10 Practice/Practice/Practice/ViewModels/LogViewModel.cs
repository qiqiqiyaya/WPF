﻿using Newtonsoft.Json;
using Practice.Common;
using Practice.Core.BaseModels;
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

        public LogViewModel(
            SafetyUiActionService safetyUiActionService,
            IContainerExtension containerProvider,
            IRootDialogService rootDialogService,
            IPaginationService pagination) : base(pagination)
        {
            _rootDialogService = rootDialogService;
            _safetyUiActionService = safetyUiActionService;

            _scopedProvider = containerProvider.CreateScope();
            _logProvider = _scopedProvider.Resolve<ILogProvider>();

            ViewDetailCommand = new DelegateCommand<LogDetail>(OpenLogDetail);
            CopyCommand = new DelegateCommand<LogDetail>(Copy);
            PageChangedCommand = new DelegateCommand(PageChanged);

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

        protected void LoadingData()
        {
            _safetyUiActionService.TaskRun(async () =>
            {
                await _rootDialogService.LoadingShowAsync();
                PageList<List<LogDetail>> pageList = await _logProvider.GetPageList(PageNumber, PageSize);

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
