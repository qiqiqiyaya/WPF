namespace Practice.Core
{
    /// <summary>
    /// 分页控件视图展示处理器
    /// </summary>
    public interface IPaginationControlViewPresentHandler
    {
        /// <summary>
        /// 自动订阅 <see cref="Events.MenuChangedEvent"/> 事件
        /// </summary>
        void Init();
    }
}
