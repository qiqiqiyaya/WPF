using System.Threading.Tasks;

namespace Practice.Helpers
{
    /// <summary>
    /// 异步暂停
    /// </summary>
    /// <remarks>
    /// 源代码请参考 https://stackoverflow.com/questions/19613444/a-pattern-to-pause-resume-an-async-task
    /// </remarks>
    public struct PauseToken
    {
        private readonly PauseTokenSource _source;
        public PauseToken(PauseTokenSource source) { this._source = source; }

        /// <summary>
        /// 是否可暂停
        /// </summary>
        public bool IsPaused => _source != null && _source.IsPaused;

        /// <summary>
        /// 暂停等待
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync()
        {
            return IsPaused ?
                _source.WaitAsync() :
                PauseTokenSource.CompletedTask;
        }
    }
}
