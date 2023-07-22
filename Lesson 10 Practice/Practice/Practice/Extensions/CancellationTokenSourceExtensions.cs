using System.Threading;

namespace Practice.Extensions
{
    public static class CancellationTokenSourceExtensions
    {
        /// <summary>
        /// 取消并且释放
        /// </summary>
        /// <param name="cts"></param>
        /// <remarks>
        /// 如果已取消，则释放
        /// </remarks>
        public static void CancelAndDispose(this CancellationTokenSource? cts)
        {
            if (cts == null) return;
            if (cts.IsCancellationRequested)
            {
                cts.Dispose();
            }
            else
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
}
