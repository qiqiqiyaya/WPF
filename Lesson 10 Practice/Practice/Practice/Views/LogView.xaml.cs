using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Practice.Views
{
    /// <summary>
    /// LogView.xaml 的交互逻辑
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        private void LogDataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = MouseWheelEvent;
            eventArg.Source = sender;
            // 事件上浮，让外界的那个滚动条滚动
            RaiseEvent(eventArg);
        }

        private void Pagination_OnPageChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
