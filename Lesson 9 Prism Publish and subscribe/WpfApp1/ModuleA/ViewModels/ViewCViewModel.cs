using Prism.Services.Dialogs;
using System;
using ModuleA.Event;
using Prism.Commands;
using Prism.Events;

namespace ModuleA.ViewModels
{
    public class ViewCViewModel : IDialogAware
    {
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand OkCommand { get; set; }
        private readonly IEventAggregator _aggregator;

        public ViewCViewModel(IEventAggregator aggregator)
        {
            CancelCommand = new DelegateCommand(() =>
            {
                var @event = aggregator.GetEvent<MessageEvent>();
                @event.Publish("holle");

                //DialogParameters keys = new DialogParameters();
                //keys.Add("value", "CancelCommand");
                //RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, keys));
            });
            OkCommand = new DelegateCommand(() =>
            {
                DialogParameters keys = new DialogParameters();
                keys.Add("value", "OkCommand");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
            });

            _aggregator = aggregator;

        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("value", "Hello");
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("Title");
        }

        public string Title { get; private set; }
        public event Action<IDialogResult>? RequestClose;
    }
}
