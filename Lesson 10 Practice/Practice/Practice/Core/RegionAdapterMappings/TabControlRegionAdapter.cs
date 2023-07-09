using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Practice.Extensions;
using Practice.Models;
using Prism.Ioc;
using Prism.Regions;

namespace Practice.Core.RegionAdapterMappings
{
    public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
    {
        private readonly IContainerExtension _containerProvider;

        public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory,
            IContainerExtension containerProvider) : base(regionBehaviorFactory)
        {
            _containerProvider = containerProvider;
        }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (MenuBar item in e.NewItems!)
                    {
                        var userControl = _containerProvider.Resolve(item.TabItemInfo.ViewType) as UserControl;
                        Check.NotNull(userControl, nameof(userControl));

                        item.TabItemInfo.UserControl = userControl;
                        regionTarget.Items.Add(item);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (MenuBar item in e.OldItems!)
                    {
                        item.TabItemInfo.UserControl = null;
                        regionTarget.Items.Remove(item);
                        //var tabTodelete = regionTarget.Items.OfType<TabItem>().FirstOrDefault(n => n.Content == item);
                        //regionTarget.Items.Remove(tabTodelete);
                    }
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
