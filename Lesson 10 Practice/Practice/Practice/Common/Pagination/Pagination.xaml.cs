using System.Windows.Controls;

namespace Practice.Common.Pagination
{
    /// <summary>
    /// Pagination.xaml 的交互逻辑
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
            DataContext = new PaginationViewModel();
        }
    }
}
