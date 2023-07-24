using Practice.Models;
using System.Windows.Controls;

namespace Practice.Common
{
    /// <summary>
    /// LogDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class LogDetailView : UserControl
    {
        public LogDetailView(LogDetail log)
        {
            InitializeComponent();
            DataContext = log;
        }
    }
}
