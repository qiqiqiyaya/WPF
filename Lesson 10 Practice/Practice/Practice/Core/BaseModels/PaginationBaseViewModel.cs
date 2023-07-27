using Practice.Events;
using Practice.Services.Interfaces;
using ReactiveUI;
using System.Windows;
using Practice.Common;

namespace Practice.Core.BaseModels
{
    public abstract class PaginationBaseViewModel : ReactiveObject, IPagination
    {
        protected PaginationBaseViewModel(IPaginationService pagination)
        {
            Pagination = pagination;
            LocalPaginationInfo = new LocalPaginationInfo();
        }

        /// <summary>
        /// 分页服务
        /// </summary>
        /// <remarks>
        /// 通过属性注入
        /// </remarks>
        protected IPaginationService Pagination { get; set; }

        /// <summary>
        /// 本地分页信息
        /// </summary>
        public LocalPaginationInfo LocalPaginationInfo { get; }

        public int Total
        {
            get => Pagination.Total;
            set
            {
                Pagination.Total = value;
                LocalPaginationInfo.Total = value;
            }
        }

        public int PageSize
        {
            get
            {
                LocalPaginationInfo.PageSize = Pagination.PageSize;
                return Pagination.PageSize;
            }
            set
            {
                Pagination.PageSize = value;
                LocalPaginationInfo.PageSize = value;
            }
        }

        public int PageNumber
        {
            get
            {
                LocalPaginationInfo.PageNumber = Pagination.PageNumber;
                return Pagination.PageNumber;
            }
            set
            {
                Pagination.PageNumber = value;
                LocalPaginationInfo.PageNumber = value;
            }
        }

        /// <summary>
        /// 分页控件是否展示
        /// </summary>
        public Visibility PaginationShow
        {
            get => Pagination.PaginationShow;
            set
            {
                Pagination.PaginationShow = value;
                LocalPaginationInfo.PaginationShow = value;
            }
        }

        public virtual void PageChanged()
        {

        }
    }
}
