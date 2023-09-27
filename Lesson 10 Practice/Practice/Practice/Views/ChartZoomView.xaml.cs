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

namespace Practice.Views
{
    /// <summary>
    /// ChartZoomView.xaml 的交互逻辑
    /// </summary>
    public partial class ChartZoomView : UserControl
    {
        public ChartZoomView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // 阻止事件冒泡 ，阻止图表上的操作转移到其他UI上去
            e.Handled = true;
        }


        private void ResetChart_OnClick(object sender, RoutedEventArgs e)
        {
            // x,y轴，最大、最小值设为 null ，图表将自动计算
            var xAxis = Chart.XAxes.FirstOrDefault();
            var yAxis = Chart.YAxes.FirstOrDefault();
            if (xAxis == null) return;
            xAxis.MaxLimit = null;
            xAxis.MinLimit = null;

            if (yAxis == null) return;
            yAxis.MaxLimit = null;
            yAxis.MinLimit = null;
        }
    }
}
