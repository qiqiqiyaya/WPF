using Practice.Common;
using Prism.Commands;
using System.Windows;

namespace Practice.Services.Interfaces
{
    /// <summary>
    /// 单例服务
    /// </summary>
    public interface IPaginationService
    {
        DelegateCommand PageChangedCommand { get; }

        int Total { get; set; }

        int PageSize { get; set; }

        int PageNumber { get; set; }

        /// <summary>
        /// 分页控件是否展示
        /// </summary>
        Visibility PaginationShow { get; set; }

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        void Show(LocalPaginationInfo localPaginationInfo);

        void Close();
    }
}
