namespace Practice.Core
{
    /// <summary>
    /// tab菜单切换时，触发的相关操作
    /// </summary>
    public interface ITabItemMenuChangeAction
    {
        /// <summary>
        /// 菜单Tab上已显示，且从其他View进入到当前View时的操作
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 菜单Tab上已显示，且从当前View进入到其他View时的操作
        /// </summary>
        void OnLeave();
    }
}
