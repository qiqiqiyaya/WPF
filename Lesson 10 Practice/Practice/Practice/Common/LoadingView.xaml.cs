using System.Windows.Controls;

namespace Practice.CommonViews
{
    /// <summary>
    /// LoadingView.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingView : UserControl
    {
        public LoadingView(string text = "LoadingData")
        {
            InitializeComponent();
            this.Content.Text = text;
        }
    }
}
