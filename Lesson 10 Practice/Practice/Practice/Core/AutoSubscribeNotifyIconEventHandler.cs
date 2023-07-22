using System;
using Practice.Events;
using Prism.Events;
using System.Windows.Controls;
#pragma warning disable CS8618

namespace Practice.Core
{
    /// <summary>
    /// 单例处理器
    /// </summary>
    public class AutoSubscribeNotifyIconEventHandler : IAutoSubscribeNotifyIconEventHandler, IDisposable
    {
        private readonly ICurrentMenuBar _currentMenuBar;
        private readonly IEventAggregator _eventAggregator;
        private SubscriptionToken _token;

        public AutoSubscribeNotifyIconEventHandler(IEventAggregator eventAggregator, ICurrentMenuBar currentMenuBar)
        {
            _eventAggregator = eventAggregator;
            _currentMenuBar = currentMenuBar;
        }

        /// <summary>
        /// 自动订阅 <see cref="NotifyIconEvent"/> 事件
        /// </summary>
        public void Init()
        {
            // 订阅事件
            _token = _eventAggregator.GetEvent<NotifyIconEvent>().Subscribe(state =>
            {
                //程序初始化完成后，ICurrentMenuBar 必定存在值

                // 获取当前ViewModel
                var viewModel = GetSubscribeViewModel();
                if (viewModel == null) return;

                // 触发当前主界面选中的菜单，视图模型
                viewModel.Subscribe(state);
            });
        }

        private IAutoSubscribeNotifyIconEvent? GetSubscribeViewModel()
        {
            var menuBar = _currentMenuBar.CurrentMenuBar;
            var userControl = (UserControl)menuBar.TabItemMenu.UserControl;
            var viewModel = userControl.DataContext;

            if (viewModel is IAutoSubscribeNotifyIconEvent model)
            {
                return model;
            }

            return null;
        }

        public void Dispose()
        {
            _token.Dispose();
        }
    }
}
