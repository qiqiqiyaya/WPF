using System.Windows.Threading;

namespace Practice.Services
{
    public class SafetyUiDispatcher
    {
        public Dispatcher UiDispatcher { get; protected set; }

        public SafetyUiDispatcher(Dispatcher uiDispatcher)
        {
            UiDispatcher = uiDispatcher;
        }
    }
}
