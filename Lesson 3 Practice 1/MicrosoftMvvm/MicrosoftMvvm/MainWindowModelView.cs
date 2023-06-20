using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MicrosoftMvvm
{
    public class MainWindowModelView:ObservableObject
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
                OnPropertyChanged();
            }
        }

        public void Show(string name)
        {
            //MessageBox.Show(name);
            // 发送消息，接收地址为token1
            WeakReferenceMessenger.Default.Send(name, "token1");
            Name = name;
        }
    }
}
