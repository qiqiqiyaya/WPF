using Practice.Core;

namespace Practice.Common
{
    public class PaginationInfo
    {
        public int Total { get; set; }

        public int PageSize { get; set; } = SystemSettingKeys.PageSize;

        public int PageNumber { get; set; }
    }
}
