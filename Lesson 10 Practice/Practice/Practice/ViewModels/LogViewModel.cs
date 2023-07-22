using Practice.Models;
using Practice.Provider.Interfaces;
using Practice.Services;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Practice.ViewModels
{
    public class LogViewModel : ReactiveObject
    {
        private readonly IMenuProvider _menuProvider;
        private readonly SafetyUiActionService _safetyUiActionService;

        public LogViewModel(IMenuProvider menuProvider, SafetyUiActionService safetyUiActionService)
        {
            _menuProvider = menuProvider;
            _safetyUiActionService = safetyUiActionService;

            _safetyUiActionService.AsyncInvokeThenUiAction(async () =>
            {
                var all = await _menuProvider.GetAllAsync();
                return () =>
                {
                    List.AddRange(all);
                };
            });
        }

        private ObservableCollection<MenuBar> _list = new ObservableCollection<MenuBar>();
        public ObservableCollection<MenuBar> List
        {
            get => _list;
            set => this.RaiseAndSetIfChanged(ref value, _list);

        }
    }
}
