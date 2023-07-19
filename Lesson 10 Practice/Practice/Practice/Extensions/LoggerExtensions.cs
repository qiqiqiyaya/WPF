using Serilog;
using System;
using System.Runtime.CompilerServices;

namespace Practice.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// 记录错误详情信息
        /// </summary>
        /// <remarks>
        /// 调用者信息在代码编译期间生成
        /// </remarks>
        /// <param name="logger"></param>
        /// <param name="exception"></param>
        /// <param name="lineNumber">调用者代码行数</param>
        /// <param name="memberName">调用者成员</param>
        /// <param name="filePath">调用者绝对路径</param>
        public static void ErrorDetail(this ILogger logger,
            Exception exception,
            [CallerLineNumber] int lineNumber = default,
            [CallerMemberName] string memberName = default!,
            [CallerFilePath] string filePath = default!)
        {

            logger.Error(exception, $"CallerLineNumber: {lineNumber} , CallerMemberName: {memberName} , CallerFilePath: {filePath}");
        }
    }
}
