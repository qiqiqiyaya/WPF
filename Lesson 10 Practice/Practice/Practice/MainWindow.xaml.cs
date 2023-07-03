using Practice.Services.Contract;
using Practice.Services.interfaces;
using System.Windows;
using System.Windows.Input;
using Prism.Regions;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRootDialogService _rootDialogService;
        private readonly IRegionManager _regionManager;
        private readonly IRegionViewRegistry _regionViewRegistry;

        public MainWindow(IRootDialogService rootDialogService, IRegionManager regionManager, IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
            _regionManager = regionManager;

            //regionManager.Regions[""].
            _rootDialogService = rootDialogService;
            InitializeComponent();

            this.Header.MouseDown += (sender, args) =>
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            this.Header.MouseDoubleClick += (sender, args) =>
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };
            var aa = regionManager.RegisterViewWithRegion("Region", "WorkingSoftware");
            var vvv = aa;

            var aacc = _regionViewRegistry.GetContents("Region");
            var aaaaaa = regionManager.Regions["Region"];

            RootDialog.Identifier = SystemSettingKeys.RootDialogIdentity;
        }

        private void Minimized_OnClick(object sender, RoutedEventArgs e)
        {

            //RegionManager.SetRegionName(this.Region,"test");

            this.WindowState = WindowState.Minimized;
        }

        private void Maximized_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 阻止事件上浮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreventEventRaise_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
