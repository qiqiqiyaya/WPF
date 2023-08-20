using Practice.Helpers;
using Practice.ViewModels;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Practice.Views
{
    /// <summary>
    /// AutoStartupView.xaml 的交互逻辑
    /// </summary>
    public partial class AutoStartupView : UserControl
    {
        public AutoStartupView()
        {
            InitializeComponent();
            Loaded += AutoStartupView_Loaded;
        }

        private void AutoStartupView_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as AutoStartupViewModel)!;

            // 是否是管理员角色
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                CheckForAllUsers.IsEnabled = true;
                viewModel.ResetIsCheckForAllUsers();
                IsAdmin.Visibility = Visibility.Collapsed;
            }
            else
            {
                IsAdmin.Text = "当前用户不是管理员，功能禁用！！！";
            }
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            FieldMethodHelper.OpenInBrowser(e.Uri?.AbsoluteUri);
        }
    }
}
