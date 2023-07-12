namespace Practice.Core
{
    /// <summary>
    /// tab切换item时，触发的相关操作
    /// </summary>
    public interface ITabItemMenuChangeAction
    {
        /// <summary>
        /// 进入到当前View时，假的初始操作
        /// </summary>
        void OnInit();

        /// <summary>
        /// 退出当前View时
        /// </summary>
        void OnDestroy();
    }
}
