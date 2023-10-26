using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BubbleLabelClick.Controls
{
    public class TestEventControl : Control
    {
        private const string TestButtonName = "TestButton";

        public static readonly RoutedEvent CustomEvent;
        public static readonly RoutedEvent ClickEvent;

        static TestEventControl()
        {
            CustomEvent = EventManager.RegisterRoutedEvent("TestEventClickEvent",
                RoutingStrategy.Bubble,
                typeof(RoutedEventArgs),
                typeof(TestEventControl));

            ClickEvent = EventManager.RegisterRoutedEvent("Click",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(TestEventControl));
        }

        public event RoutedEventHandler Custom
        {
            add => AddHandler(CustomEvent, value);
            remove => RemoveHandler(CustomEvent, value);
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = GetTemplateChild(TestButtonName) as Button;

            if (button != null)
            {
                button.Click += (sender, args) =>
                {
                    //RaiseEvent(new RoutedEventArgs(CustomEvent));
                    RaiseEvent(new RoutedEventArgs(ClickEvent));
                };

                // MouseUp 继承自UIElement
                // TestEventControl 继承自 Control ，继承UIElement ，存在一个 MouseUp 事件
                button.MouseUp += (sender, args) =>
                {
                    args.Handled = true;
                };
            }


        }
    }
}
