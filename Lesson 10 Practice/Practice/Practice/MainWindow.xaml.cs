﻿using Practice.Core.Contract;
using Prism.Regions;
using System.Windows;
using System.Windows.Input;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IRegionManager regionManager)
        {
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

            // code-behind set region
            RegionManager.SetRegionName(TabMenus, SystemSettingKeys.TabMenuRegion);
            RegionManager.SetRegionManager(TabMenus, regionManager);

            // root mdDialog Identifier set
            RootDialog.Identifier = SystemSettingKeys.RootDialogIdentity;
        }

        private void Minimized_OnClick(object sender, RoutedEventArgs e)
        {
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
