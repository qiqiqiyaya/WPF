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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = Btn.Width;// 设置动画初始值
            animation.To = Btn.Width - 39;// 设置动画的结束值
            animation.Duration = TimeSpan.FromSeconds(2); // 设置动画的持续时间
            animation.AutoReverse = true;// 反转，是否反向执行
            animation.RepeatBehavior =new RepeatBehavior(1);
            animation.Completed += Animation_Completed;

            // 在当前按钮上执行该动画
            Btn.BeginAnimation(Button.WidthProperty, animation);
        }

        private void Animation_Completed(object? sender, EventArgs e)
        {
            Btn.Content = "内容完成";
        }
    }
}
