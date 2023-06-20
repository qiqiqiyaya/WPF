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
using System.Windows.Shapes;

namespace Case1
{
    /// <summary>
    /// _4DataTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class _4DataTemplate : Window
    {
        public _4DataTemplate()
        {
            InitializeComponent();

            List<Color> test = new List<Color>();

            test.Add(new Color(){Code = "#FFA07A",Name = "Lightsalmon" });
            test.Add(new Color(){Code = "#E9967A", Name = "332432" });
            test.Add(new Color(){Code = "#DC143C", Name = "423432423" });

            List.ItemsSource = test;
            Grid.ItemsSource = test;
        }
    }

    public class Color
    {
        public string Code { get; set; }

        public string Name { get; set; }

        
    }
}

