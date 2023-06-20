using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public DelegateCommand<string> OpenCommand { get; private set; }
        private readonly IDialogService _dialogService;

        public MainViewModel(IDialogService dialogService)
        {
            OpenCommand = new DelegateCommand<string>(Open);
            _dialogService = dialogService;
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }

        private void Open(string obj)
        {
            DialogParameters dialog = new DialogParameters();
            dialog.Add("Title", "测试弹出");
            _dialogService.ShowDialog(obj, dialog, result =>
            {
                var str = result.Parameters.GetValue<string>("value");
                Text = str;
            });
        }
    }
}
