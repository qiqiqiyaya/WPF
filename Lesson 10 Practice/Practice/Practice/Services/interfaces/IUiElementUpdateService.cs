using System;

namespace Practice.Services.interfaces
{
    public interface IUiElementUpdateService
    {
        void AsyncInvoke<T>(Action asyncAction);

        void AsyncInvoke<T>(Action<T> asyncAction, T state);

        void SyncInvoke<T>(Action asyncAction);

        void SyncInvoke<T>(Action<T> asyncAction, T state);
    }
}
