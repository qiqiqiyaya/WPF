namespace Practice.Core.Configuration
{
    public class PaginationConfiguration
    {
        public const int PageSize = 20;
        /// <summary>
        /// 非阻塞情况下每次添加的数据条数
        /// </summary>
        public const int NonBlockingAddSize = 10;

        protected PaginationConfiguration()
        {

        }
    }
}
