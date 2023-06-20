using Case1.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Case1
{
    // 通知更改
    public class _7CommadViewModel : INotifyPropertyChanged
    {
        public _7CommadViewModel()
        {
            Command = new Test7Command(Show);
            Name = "一库";
        }

        public Test7Command Command { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                //OnPropertyChanged(nameof(Name));
                OnPropertyChanged();
            }
        }

        public void Show()
        {
            MessageBox.Show("点击了按钮");
            Name = "点击了按钮";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class Test7Command : ICommand
    {
        private readonly Action _action;

        public Test7Command(Action action)
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
