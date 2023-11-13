using System;
using Practice.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Events;

namespace Practice.Views
{
    /// <summary>
    /// LogView.xaml 的交互逻辑
    /// </summary>
    public partial class LogView : UserControl, IDisposable
    {
        private readonly MainWindowsContentService _mainWindowsContentService;
        private readonly SubscriptionToken _subscriptionToken;
        private bool _isChanged;

        public LogView(MainWindowsContentService mainWindowsContentService)
        {
            _mainWindowsContentService = mainWindowsContentService;
            InitializeComponent();

            //_subscriptionToken = _mainWindowsContentService.SizeChangeEvent.Subscribe(size =>
            //{
            //    var aa = size.Height - ConditionPanel.ActualHeight - 10 - 5;


            //});

            ConditionPanel.SizeChanged += (sender, args) =>
            {

            };

            Loaded += (sender, args) =>
            {
                mainWindowsContentService.Subscribe(height =>
                {
                    // 内容区域高度 - 搜索条件输入框
                    LogDataGrid.Height = height - ConditionPanel.ActualHeight;
                });
            };
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

        public void Dispose()
        {
            //_subscriptionToken.Dispose();
        }
    }
}
