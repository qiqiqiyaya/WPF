namespace Practice.Core
{
    /// <summary>
    /// 系统静态设置键
    /// </summary>
    public class SystemSettingKeys
    {
        /// <summary>
        /// 主题
        /// </summary>
        public const string Theme = nameof(Theme);

        /// <summary>
        /// 根节点对话框
        /// </summary>

        public static string RootDialogIdentity = "RootDialog";

        /// <summary>
        /// tab菜单区域名称
        /// </summary>
        public static string TabMenuRegion = nameof(TabMenuRegion);

        /// <summary>
        /// 开机是否自启动
        /// </summary>
        public static string AutoStartup = nameof(AutoStartup);


        public static string RootConfiguration = nameof(RootConfiguration);

        /// <summary>
        /// Sqlite 数据路径
        /// </summary>
        public static string SqlitePath = "Db/PracticeData.db";

        /// <summary>
        /// 分页，默认20行
        /// </summary>
        public const int RowNumber = 20;
    }
}
