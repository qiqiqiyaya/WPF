using System;
using System.Collections.Generic;
using System.Linq;
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

namespace RoutedEvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.AddHandler(Button.ClickEvent, new RoutedEventHandler(this.Button_Click));
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            string message = "Sender: " + sender.ToString() + "\r\n" +
                             "Source: " + e.Source + "\r\n" +
                             "Original Source: " + e.OriginalSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string strOriginSource = string.Format("Visual tree start point:{0},type is {1}",
                (e.OriginalSource as FrameworkElement).Name, e.OriginalSource.GetType().Name);
            string strSource = string.Format("Logic tree start point:{0},type is {1}",
                (e.Source as FrameworkElement).Name, e.Source.GetType().Name);
            MessageBox.Show(strOriginSource + "\r\n" + strSource);
        }
    }
}
