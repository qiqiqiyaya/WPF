using Practice.Common;
using System.Windows;

namespace Practice.Core
{
    /// <summary>
    /// 分页
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// 动态总数
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// 动态行数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 动态页码
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// 动态是否展示分页组件
        /// </summary>
        Visibility PaginationShow { get; set; }

        /// <summary>
        /// 本地分页信息
        /// </summary>
        LocalPaginationInfo LocalPaginationInfo { get; }

        void PageChanged();
    }
}
