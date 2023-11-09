using DryIoc;
using Practice.Events;
using Practice.Models;
using Prism.Events;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Windows;
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
        private ScrollViewer _tabHeadControl;
        private TabControl _tabControl;

        private MenuBar? _currentMenuBar;
        private Action<double>? _currentAction;
        private readonly Dictionary<string, Action<double>> _sizeChangedContainer = new Dictionary<string, Action<double>>();

        public MainWindowsContentService(IEventAggregator eventAggregator)
        {
            var menuChangedEvent = eventAggregator.GetEvent<MenuChangedEvent>();
            var menuChangingEvent = eventAggregator.GetEvent<MenuChangingEvent>();

            // 变更前
            menuChangingEvent.Subscribe(menuBar =>
            {
                _currentAction = null;
                _currentMenuBar = null;
            });

            // 菜单变更之后
            menuChangedEvent.Subscribe(menuBar =>
            {
                _currentMenuBar = menuBar;
                // 查找是否当前菜单页面是否有订阅
                if (_sizeChangedContainer.TryGetValue(_currentMenuBar.Id, out var action))
                {
                    _currentAction = action;
                }
            });
        }

        public void Init(double totalHeight, TabControl tabControl, ScrollViewer tabHeadControl)
        {
            _tabHeadControl = tabHeadControl;
            _tabControl = tabControl;
            _tabHeadControl.SizeChanged += (sender, args) =>
            {
                if (args.HeightChanged)
                {
                    _currentAction?.Invoke(_tabControl.ActualHeight - _tabHeadControl.ActualHeight);
                }
            };

            tabControl.SizeChanged += (sender, args) =>
            {
                if (args.HeightChanged)
                {
                    _currentAction?.Invoke(_tabControl.ActualHeight - _tabHeadControl.ActualHeight);
                }
            };
        }

        private double _height;

        /// <summary>
        /// 内容区域宽度
        /// </summary>
        public double Height
        {
            get => _height;
            private set => this.RaiseAndSetIfChanged(ref _height, value);
        }

        public void Dispose()
        {
            //var aa = _sizeChangeEvent.Subscribe(a => { });
            //aa.Dispose();

            _sizeChangedContainer.Clear();
        }

        /// <summary>
        /// 当前菜单页面订阅，方法在UI线程被执行。
        /// </summary>
        /// <param name="sizeChangedAction"></param>
        public void Subscribe(Action<double> sizeChangedAction)
        {
            if (_currentMenuBar == null)
            {
                // 如果当前页面订阅时，_currentMenuBar 为空则抛出错误。
                throw new NullReferenceException("Current menuBar is null.");
            }

            _currentAction = sizeChangedAction;
            _currentAction(_tabControl.ActualHeight - _tabHeadControl.ActualHeight);
            _sizeChangedContainer.TryAdd(_currentMenuBar.Id, sizeChangedAction);
        }
    }
}
