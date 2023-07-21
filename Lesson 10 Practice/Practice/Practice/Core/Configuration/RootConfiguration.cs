namespace Practice.Core.Configuration
{
    public class RootConfiguration
    {
        /// <summary>
        /// 是否可最小化到托盘
        /// </summary>
        public bool CanMinimizeToTray { get; set; }

        /// <summary>
        /// 显示最小化到托盘提示吗？
        /// </summary>
        /// <remarks>
        /// 默认值 true
        /// </remarks>
        public bool ShowMinimizeToTrayTip { get; set; } = true;
    }
}
