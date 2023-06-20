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
    /// _5Binding.xaml 的交互逻辑
    /// </summary>
    public partial class _5Binding : Window
    {
        public _5Binding()
        {
            InitializeComponent();

            this.DataContext = new _5BindingClass()
            {
                Name = "stewae"
            };
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //TextBox1.Text = Slider.Value.ToString("F1");
            //TextBox2.Text = Slider.Value.ToString("F1");
            //TextBox3.Text = Slider.Value.ToString("F1");
        }

        private void TextBox1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //if (double.TryParse(TextBox1.Text, out var result))
            //{
            //    Slider.Value = result;
            //}
        }
    }
}
