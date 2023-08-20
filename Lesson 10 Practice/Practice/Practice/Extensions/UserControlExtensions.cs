using System;
using System.Windows.Controls;

namespace Practice.Extensions
{
    public static class UserControlExtensions
    {
        /// <summary>
        /// 如果继承 <see cref="IDisposable"/> 接口，则释放
        /// </summary>
        /// <param name="userControl"></param>
        public static void TryDispose(this UserControl userControl)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (userControl is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
