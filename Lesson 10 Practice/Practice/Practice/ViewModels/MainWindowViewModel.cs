using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;

namespace Practice.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand<object> MenuAction { get; private set; }

        public MainWindowViewModel()
        {
            MenuAction = new DelegateCommand<object>(fdasfdsa);
            _iconDynamic = new PackIconExtension(PackIconKind.MenuClose, 24);
        }

        private bool _menuClose = true;
        public bool MenuClose
        {
            get => _menuClose;
            set => SetProperty(ref _menuClose, value);
        }


        private double _navRailWidth = 80;
        public double NavRailWidth
        {
            get => _navRailWidth;
            set => SetProperty(ref _navRailWidth, value);
        }

        private readonly PackIconExtension _iconClose = new PackIconExtension(PackIconKind.MenuClose, 24);
        private readonly PackIconExtension _iconOpen = new PackIconExtension(PackIconKind.MenuOpen, 24);
        private PackIconExtension _iconDynamic;
        public PackIconExtension Icon
        {
            get => _iconDynamic;
            set
            {
                _iconDynamic = _menuClose ? _iconClose : _iconOpen;
                RaisePropertyChanged();
            }
        }

        private void fdasfdsa(object packIcon)
        {
            if (_menuClose)
            {
                NavRailWidth = 200;
            }
            else
            {
                NavRailWidth = 80;
            }

            _menuClose = !_menuClose;
        }
    }
}
