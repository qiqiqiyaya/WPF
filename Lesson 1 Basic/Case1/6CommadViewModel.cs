using System;
using System.Windows;
using System.Windows.Input;

namespace Case1
{
    public class _6CommadViewModel
    {
        public _6CommadViewModel()
        {
            Command = new TestCommand(Show);
            Name = "一库";
        }

        public TestCommand Command { get; set; }

        public string Name { get; set; }

        public void Show()
        {
            MessageBox.Show("点击了按钮");
            Name = "点击了按钮";
        }
    }


    public class TestCommand : ICommand
    {
        private readonly Action _action;

        public TestCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}
