using Practice.Services.interfaces;
using System;
using System.Threading;
using System.Windows.Threading;

namespace Practice.Services
{
    public class UiElementUpdateService : IUiElementUpdateService
    {
        private readonly Dispatcher _dispatcher;

        public UiElementUpdateService(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        ///// <summary>
        ///// 设置UI线程上下文
        ///// </summary>
        ///// <param name="synchronizationContext"></param>
        ///// <exception cref="Exception"></exception>
        //public void SetUiSynchronizationContext(SynchronizationContext synchronizationContext)
        //{
        //    _synchronizationContext = synchronizationContext;
        //}

        public virtual void AsyncInvoke<T>(Action asyncAction)
        {

        }

        public virtual void AsyncInvoke<T>(Action<T> asyncAction, T state)
        {
            //_synchronizationContext.Post(o =>
            //{
            //    asyncAction(state);
            //}, null);
        }

        public virtual void SyncInvoke<T>(Action asyncAction)
        {
            //_synchronizationContext.Send(o =>
            //{
            //    asyncAction();
            //}, null);
        }

        public virtual void SyncInvoke<T>(Action<T> asyncAction, T state)
        {
            //_synchronizationContext.Send(o =>
            //{
            //    asyncAction(state);
            //}, null);
        }
    }
}
