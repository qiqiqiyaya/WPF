namespace Practice.Services.Interfaces
{
    public interface IAutoStartupService
    {
        /// <summary>
        /// 是否开机自启动
        /// </summary>
        /// <returns></returns>
        bool IsAutoStartup(bool forAllUsers = false);

        /// <summary>
        /// 启动
        /// </summary>
        bool Enable(bool forAllUsers = false);

        /// <summary>
        /// 禁用
        /// </summary>
        bool Disable(bool forAllUsers = false);
    }
}
