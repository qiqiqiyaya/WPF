using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Case2s
{
    public class MainWindowModelView : ViewModelBase
    {
        public MainWindowModelView()
        {
            Command = new RelayCommand<string>(Show);
            Name = "一库";
        }

        public RelayCommand<string> Command { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                //OnPropertyChanged(nameof(Name));
                RaisePropertyChanged();
            }
        }

        public void Show(string name)
        {
            //MessageBox.Show(name);
            // 发送消息，接收地址为token1
            Messenger.Default.Send(name,"token1");
            Name = name;
        }

    }
}
