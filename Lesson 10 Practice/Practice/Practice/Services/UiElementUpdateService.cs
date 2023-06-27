using Practice.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Practice.Services
{
    public class UiElementUpdateService : IUiElementUpdateService
    {
        private SynchronizationContext _synchronizationContext;
        public UiElementUpdateService()
        {
            var aa = new SynchronizationContext();
        }

        public void SetUiSynchronizationContext(SynchronizationContext synchronizationContext)
        {
            var uiThread = Thread.CurrentThread;
            if (uiThread.IsBackground)
            {
                throw new Exception("The current thread is not a  UI thread.");
            }
            _synchronizationContext = synchronizationContext;

            //_synchronizationContext.Post();
        }


    }
}
