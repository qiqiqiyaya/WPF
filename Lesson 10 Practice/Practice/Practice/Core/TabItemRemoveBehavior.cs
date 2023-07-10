using Microsoft.Xaml.Behaviors;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Practice.Core
{
    public class TabItemRemoveBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(TabItem_Closing));
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(TabItem_Closing));
        }

        void TabItem_Closing(object sender, RoutedEventArgs e)
        {
            IRegion region = RegionManager.GetObservableRegion(AssociatedObject).Value;
            if (region == null)
                return;

            var args = (TabClosingEventArgs)e;

            args.Cancel = !CanRemoveItem(GetItemFromTabItem(args.OriginalSource), region);
        }

        void TabItem_Closed(object sender, RoutedEventArgs e)
        {
            IRegion region = RegionManager.GetObservableRegion(AssociatedObject).Value;
            if (region == null)
                return;

            RemoveItemFromRegion(GetItemFromTabItem(e.OriginalSource), region);
        }

        object GetItemFromTabItem(object source)
        {
            var tabItem = source as TabItemEx;
            if (tabItem == null)
                return null;

            return tabItem.Content;
        }

        bool CanRemoveItem(object item, IRegion region)
        {
            bool canRemove = true;

            var context = new NavigationContext(region.NavigationService, null);

            var confirmRequestItem = item as IConfirmNavigationRequest;
            if (confirmRequestItem != null)
            {
                confirmRequestItem.ConfirmNavigationRequest(context, result =>
                {
                    canRemove = result;
                });
            }

            FrameworkElement frameworkElement = item as FrameworkElement;
            if (frameworkElement != null && canRemove)
            {
                IConfirmNavigationRequest confirmRequestDataContext = frameworkElement.DataContext as IConfirmNavigationRequest;
                if (confirmRequestDataContext != null)
                {
                    confirmRequestDataContext.ConfirmNavigationRequest(context, result =>
                    {
                        canRemove = result;
                    });
                }
            }

            return canRemove;
        }

        void RemoveItemFromRegion(object item, IRegion region)
        {
            var context = new NavigationContext(region.NavigationService, null);

            InvokeOnNavigatedFrom(item, context);

            region.Remove(item);
        }

        void InvokeOnNavigatedFrom(object item, NavigationContext navigationContext)
        {
            var navigationAwareItem = item as INavigationAware;
            if (navigationAwareItem != null)
            {
                navigationAwareItem.OnNavigatedFrom(navigationContext);
            }

            FrameworkElement frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                INavigationAware navigationAwareDataContext = frameworkElement.DataContext as INavigationAware;
                if (navigationAwareDataContext != null)
                {
                    navigationAwareDataContext.OnNavigatedFrom(navigationContext);
                }
            }
        }
    }
}
