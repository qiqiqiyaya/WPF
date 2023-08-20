using Practice.Core;
using Practice.Services.Interfaces;
using Prism.Events;
using System;
using System.Windows;

#pragma warning disable CS8618

namespace Practice.Events.Handlers
{
    /// <summary>
    /// 分页控件视图展示处理器
    /// </summary>
    /// <remarks>
    /// 单例
    /// </remarks>
    public class PaginationControlViewPresentHandler : IPaginationControlViewPresentHandler, IDisposable
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPaginationService _paginationService;
        private readonly ICaptureMenuBar _currentMenuBar;

        private SubscriptionToken _menuChangedToken;
        private SubscriptionToken _pageChangedToken;
        /// <summary>
        /// 上一次 ViewModel 分页控件是否有展示
        /// </summary>
        private bool _lastMenuBarPaginationShow;

        public PaginationControlViewPresentHandler(IEventAggregator eventAggregator,
            IPaginationService paginationService,
            ICaptureMenuBar currentMenuBar)
        {
            _eventAggregator = eventAggregator;
            _paginationService = paginationService;
            _currentMenuBar = currentMenuBar;
        }

        /// <summary>
        /// 自动订阅 <see cref="MenuChangedEvent"/> 事件
        /// </summary>
        public void Init()
        {
            // 订阅事件
            // 控制分页是否展示
            _menuChangedToken = _eventAggregator.GetEvent<MenuChangedEvent>().Subscribe(menuBar =>
            {
                var viewModel = menuBar.GetViewModel();
                bool currentMenuBarPaginationShow = false;
                IPagination? pagination = null;
                if (viewModel is IPagination model)
                {
                    pagination = model;
                    currentMenuBarPaginationShow = model.LocalPaginationInfo.PaginationShow == Visibility.Visible;
                }

                // 上一次，有分页
                if (_lastMenuBarPaginationShow)
                {
                    // 本次无分页
                    // 分页隐藏
                    if (!currentMenuBarPaginationShow)
                    {
                        _paginationService.Close();
                    }
                    else
                    {
                        // 当前页有分页
                        // 菜单重置一下
                        _paginationService.Reset();
                    }
                }
                else
                {
                    // 上一次，无分页
                    if (!currentMenuBarPaginationShow)
                    {
                        _paginationService.Close();
                    }
                    else
                    {
                        if (pagination != null)
                        {
                            _paginationService.Show(pagination.LocalPaginationInfo);
                        }
                    }
                }

                _lastMenuBarPaginationShow = currentMenuBarPaginationShow;
            });

            // 订阅事件
            // 分页控件触发事件
            _pageChangedToken = _eventAggregator.GetEvent<PageChangedEvent>().Subscribe(() =>
            {
                var menu = _currentMenuBar.CurrentMenuBar;
                var viewModel = menu.GetViewModel();
                if (viewModel is IPagination model)
                {
                    model.PageChanged();
                }
            });
        }

        public void Dispose()
        {
            _menuChangedToken?.Dispose();
            _pageChangedToken?.Dispose();
        }
    }
}
