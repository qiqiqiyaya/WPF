using System.Windows;
using System.Windows.Controls;

namespace Practice.Common
{
    /// <summary>
    /// MainWindowsCloseDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindowsCloseDialog : UserControl
    {
        private readonly MainWindowsCloseDialogAction _action;

        public MainWindowsCloseDialog(MainWindowsCloseDialogAction action)
        {
            _action = action;
            InitializeComponent();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            _action.Ok?.Invoke(_action, CheckBox.IsChecked ?? false);
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            _action.Cancel?.Invoke(_action, CheckBox.IsChecked ?? false);
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            this._action.IsCheckChange = true;
        }
    }
}
