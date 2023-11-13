using System;

namespace Practice.Services.Models
{
    public class MainWindowsContentHeightChanged
    {
        /// <summary>
        /// 内容区域变更，回调方法只执行一次
        /// <para></para>
        /// 默认为 true
        /// </summary>
        public bool InvokeJustInOnceChanged { get; set; }

        /// <summary>
        /// 是否已经执行过
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// 内容区域变更，回调方法
        /// </summary>
        public Action<double> HeightChangedAction { get; set; }

        public MainWindowsContentHeightChanged(Action<double> heightChangedAction) : this(true, heightChangedAction)
        {

        }

        public MainWindowsContentHeightChanged(bool invokeJustInOnceChanged, Action<double> heightChangedAction)
        {
            InvokeJustInOnceChanged = invokeJustInOnceChanged;
            IsExecuted = false;
            HeightChangedAction = heightChangedAction;
        }
    }
}
