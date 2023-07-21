using System;

namespace Practice.Common
{
    public class MainWindowsCloseDialogAction
    {
        public Action<MainWindowsCloseDialogAction, bool>? Ok { get; set; }

        public Action<MainWindowsCloseDialogAction, bool>? Cancel { get; set; }

        /// <summary>
        /// 是否改变过值
        /// </summary>
        public bool IsCheckChange { get; set; }
    }
}
