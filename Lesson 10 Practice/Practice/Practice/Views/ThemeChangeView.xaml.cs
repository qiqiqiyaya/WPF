using Practice.ViewModels;
using System.Windows.Controls;

namespace Practice.Views
{
    /// <summary>
    /// ThemeChangeView.xaml 的交互逻辑
    /// </summary>
    public partial class ThemeChangeView : UserControl
    {
        public ThemeChangeView(ThemeChangeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
