using Practice.Events;
using Prism.Events;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

#pragma warning disable CS8618

namespace Practice.Services
{
    /// <summary>
    /// 主窗口tab中内容区域服务，单例服务。 <para></para>该服务只能在UI线程操作
    /// <para/>
    /// 例如：实时获取内容区域高度
    /// </summary>
    public class MainWindowsContentService : ReactiveObject, IDisposable
    {
        /// <summary>
        /// 内容区域控件名称
        /// </summary>
        public static string Name => "MainWindowsContent";
        /// <summary>
        /// tab header 控件
        /// </summary>
        private ScrollViewer _tabHeadControl;
        /// <summary>
        /// tab 控件
        /// </summary>
        private TabControl _tabControl;

        private readonly HeightChangeInfo _heightChangeInfo = new HeightChangeInfo();
        private readonly Dictionary<string, Action<double>> _sizeChangedContainer = new Dictionary<string, Action<double>>();

        public MainWindowsContentService(IEventAggregator eventAggregator)
        {
            var menuChangedEvent = eventAggregator.GetEvent<MenuChangedEvent>();
            var menuChangingEvent = eventAggregator.GetEvent<MenuChangingEvent>();
            var menuClosedEvent = eventAggregator.GetEvent<MenuClosedEvent>();

            // 变更前
            menuChangingEvent.Subscribe(_ =>
            {
                _heightChangeInfo.CurrentAction = null;
                _heightChangeInfo.CurrentMenuId = null;
            });

            // 菜单变更之后
            menuChangedEvent.Subscribe(menuBar =>
            {
                _heightChangeInfo.CurrentMenuId = menuBar.Id;
                // 查找是否当前菜单页面是否有订阅
                if (_sizeChangedContainer.TryGetValue(_heightChangeInfo.CurrentMenuId!, out var action))
                {
                    _heightChangeInfo.CurrentAction = action;
                }
            });

            // 当该菜单关闭之后
            menuClosedEvent.Subscribe(me =>
            {
                // 存在 _sizeChangedContainer 订阅也销毁
                _sizeChangedContainer.Remove(me.Id);
            });
        }

        public void Init(TabControl tabControl, ScrollViewer tabHeadControl)
        {
            _tabHeadControl = tabHeadControl;
            _tabControl = tabControl;
            _tabHeadControl.SizeChanged += (_, args) =>
            {
                if (args.HeightChanged)
                {
                    HeightChangeActionInvoke();
                }
            };

            tabControl.SizeChanged += (_, args) =>
            {
                if (args.HeightChanged)
                {
                    HeightChangeActionInvoke();
                }
            };
        }

        public void Dispose()
        {
            _sizeChangedContainer.Clear();
        }

        /// <summary>
        /// 当前菜单页面订阅，方法在UI线程被执行。
        /// </summary>
        /// <param name="sizeChangedAction"></param>
        public void Subscribe(Action<double> sizeChangedAction)
        {
            if (_heightChangeInfo.CurrentMenuId == null)
            {
                // 如果当前页面订阅时，_currentMenuBar 为空则抛出错误。
                throw new NullReferenceException("Current menuBar is null.");
            }

            if (_sizeChangedContainer.TryGetValue(_heightChangeInfo.CurrentMenuId, out var value))
            {
                _heightChangeInfo.CurrentAction = value;
            }
            else
            {
                _heightChangeInfo.CurrentAction = sizeChangedAction;
                _sizeChangedContainer.TryAdd(_heightChangeInfo.CurrentMenuId, sizeChangedAction);
            }

            var currentHeight = GetContentFullHeight();
            _heightChangeInfo.CurrentAction?.Invoke(GetContentFullHeight());
            _heightChangeInfo.LastHeight = currentHeight;
        }

        /// <summary>
        /// 内容区域高度变更动作执行
        /// </summary>
        private void HeightChangeActionInvoke()
        {
            if (_heightChangeInfo.CurrentAction == null) return;

            var currentHeight = GetContentFullHeight();
            if (!_heightChangeInfo.LastHeight.HasValue || Math.Abs(currentHeight - _heightChangeInfo.LastHeight.Value) > 0)
            {
                _heightChangeInfo.CurrentAction?.Invoke(GetContentFullHeight());
                _heightChangeInfo.LastHeight = currentHeight;
            }
        }

        private double GetContentFullHeight()
        {
            // tab控件高度 - tab头部高度 - 10 内边距 - 5 外边底部距
            return _tabControl.ActualHeight - _tabHeadControl.ActualHeight - 10 - 5;
        }

        public class HeightChangeInfo
        {
            /// <summary>
            /// 上一次变更时的高度
            /// </summary>
            public double? LastHeight { get; set; }

            /// <summary>
            /// 当前菜单Id
            /// </summary>
            public string? CurrentMenuId { get; set; }

            public Action<double>? CurrentAction { get; set; }
        }
    }
}
