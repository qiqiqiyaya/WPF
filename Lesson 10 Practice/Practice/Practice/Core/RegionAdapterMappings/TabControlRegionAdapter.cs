﻿using System;
using Practice.Extensions;
using Practice.Models;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

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
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:

                        foreach (MenuBar item in e.NewItems!)
                        {
                            regionTarget.Items.Add(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:

                        foreach (MenuBar item in e.OldItems!)
                        {
                            regionTarget.Items.Remove(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
