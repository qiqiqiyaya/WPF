using MaterialDesignThemes.Wpf;
using Practice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MenuClose.Content = new DynamicResourceExtension()
            //{
            //    ResourceKey = new PackIconExtension(PackIconKind.MenuClose, 24)
            //}; 

            
            this.Header.MouseDown += (sender, args) =>
            {
                if (args.LeftButton==MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            this.Header.MouseDoubleClick += (sender, args) =>
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };

            //BindingOperations.SetBinding()


            //var packIcon = new PackIcon();
            //packIcon.Kind = PackIconKind.MenuClose;
            //packIcon.Height = 24;
            //packIcon.Width = 24;

            //var bind = new Binding("");
            //bind.Source = packIcon;

            //MenuClose.SetBinding(ContentControl.ContentProperty, bind);

        }


        private void Minimized_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximized_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        // Register a custom routed event using the Bubble routing strategy.
        public static readonly RoutedEvent ConditionalClickEvent = EventManager.RegisterRoutedEvent(
            name: "ConditionalClick",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(MainWindow));

        public event RoutedEventHandler ConditionalClick
        {
            add { AddHandler(ConditionalClickEvent, value); }
            remove { RemoveHandler(ConditionalClickEvent, value); }
        }

        private void MenuClose_OnClick(object sender, RoutedEventArgs e)
        {
            var aa = MenuClose.Content as PackIcon;
            aa.Kind = PackIconKind.MenuOpen;
            //e.Handled = true;
            //MenuOpen.Width = 0;
            //NavRail.Width = 240;
        }

        /// <summary>
        /// 阻止冒泡事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventBubbling(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
