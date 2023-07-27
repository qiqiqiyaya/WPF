using System.Windows;
using Practice.Common;
using Practice.Core;
using Practice.Events;
using Practice.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using ReactiveUI;

namespace Practice.Services
{
    /// <summary>
    /// 单例服务
    /// </summary>
    public class PaginationService : ReactiveObject, IPaginationService
    {
        public DelegateCommand PageChangedCommand { get; }
        private readonly InternalPageChangedEvent _internalPageChangedEvent;

        public PaginationService(IEventAggregator eventAggregator)
        {
            _internalPageChangedEvent = eventAggregator.GetEvent<InternalPageChangedEvent>();
            PageChangedCommand = new DelegateCommand(PageChanged);
        }

        private int _total = 0;
        /// <summary>
        /// 总数
        /// </summary>
        public int Total
        {
            get => _total;
            set => this.RaiseAndSetIfChanged(ref _total, value);
        }

        private int _pageSize = SystemSettingKeys.PageSize;
        /// <summary>
        /// 行数
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => this.RaiseAndSetIfChanged(ref _pageSize, value);
        }

        private int _pageNumber = 0;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => this.RaiseAndSetIfChanged(ref _pageNumber, value);
        }

        private Visibility _paginationShow = Visibility.Collapsed;
        /// <summary>
        /// 分页控件是否展示
        /// </summary>
        public Visibility PaginationShow
        {
            get => _paginationShow;
            set => this.RaiseAndSetIfChanged(ref _paginationShow, value);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            PageNumber = 0;
            Total = 0;
        }

        public void Show(LocalPaginationInfo localPaginationInfo)
        {
            PageNumber = localPaginationInfo.PageNumber;
            Total = localPaginationInfo.Total;
            PaginationShow = localPaginationInfo.PaginationShow;
        }

        public void Close()
        {
            PageNumber = 0;
            Total = 0;
            PaginationShow = Visibility.Collapsed;
        }

        private void PageChanged()
        {
            _internalPageChangedEvent.Publish();
        }
    }
}
