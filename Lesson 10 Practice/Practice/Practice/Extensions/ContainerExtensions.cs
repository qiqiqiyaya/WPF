using Prism.Ioc;
using System;
using System.Threading.Tasks;

namespace Practice.Extensions
{
    public static class ContainerExtensions
    {
        /// <summary>
        /// 新建一个scope
        /// </summary>
        /// <param name="containerExtension"></param>
        /// <param name="action"></param>
        public static async Task NewScopeAsync(this IContainerExtension containerExtension, Func<IScopedProvider, Task> action)
        {
            using (var serviceProvider = containerExtension.CreateScope())
            {
                await action(serviceProvider);
            }
        }
    }
}
