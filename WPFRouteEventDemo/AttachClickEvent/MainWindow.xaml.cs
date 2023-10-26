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

namespace AttachClickEvent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // StackPanel面板命名为ButtonsPanel
            ButtonsPanel.AddHandler(Button.ClickEvent, new RoutedEventHandler(DoSomething));
        }

        private void DoSomething(object sender, RoutedEventArgs e)
        {
            if (sender == btn1)
            { 

            }
            else if (sender == btn2)
            {

            }
            else
            {
 
            }
        }
    }
}
