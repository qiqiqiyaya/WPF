using Prism.Events;
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
using ModuleA.Event;

namespace ModuleA.Views
{
    /// <summary>
    /// ViewC.xaml 的交互逻辑
    /// </summary>
    public partial class ViewC : UserControl
    {
        public ViewC(IEventAggregator aggregator)
        {
            InitializeComponent();

            var even = aggregator.GetEvent<MessageEvent>();
            SubscriptionToken? token = null;
            token = even.Subscribe(str =>
            {
                token?.Dispose();
                MessageBox.Show(str);

            });
        }
    }
}
