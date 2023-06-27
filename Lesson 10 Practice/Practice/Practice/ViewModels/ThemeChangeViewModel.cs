using DynamicData;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Practice.Services;
using Practice.Services.Contract;
using Prism.Commands;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Practice.ViewModels
{
    public class ThemeChangeViewModel : ReactiveObject
    {
        private readonly PaletteHelper _paletteHelper;
        private readonly SettingsManager _settingsManager;
        public DelegateCommand<object> ChangeHueCommand { get; private set; }
        public DelegateCommand<ISwatch> MainColorButtonCommand { get; private set; }

        public ThemeChangeViewModel(PaletteHelper paletteHelper, SettingsManager settingsManager)
        {
            _paletteHelper = paletteHelper;
            _settingsManager = settingsManager;
            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
            MainColorButtonCommand = new DelegateCommand<ISwatch>(swatch =>
            {
                Colors.Clear();
                Colors.AddRange(swatch.Hues);
            });

            _isDarkTheme = _paletteHelper.GetTheme().GetBaseTheme() == BaseTheme.Dark;
            _colors = new ObservableCollection<Color>();
        }

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                this.RaiseAndSetIfChanged(ref _isDarkTheme, value);
                ModifyTheme(value ? Theme.Dark : Theme.Light);
            }
        }

        public IEnumerable<ISwatch> Swatches => SwatchHelper.Swatches;

        private ObservableCollection<Color> _colors;

        public ObservableCollection<Color> Colors
        {
            get => _colors;
            set => this.RaiseAndSetIfChanged(ref _colors, value);
        }


        private void ModifyTheme(IBaseTheme baseTheme)
        {
            ITheme theme = _paletteHelper.GetTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
            _settingsManager.SetSetting(SettingKeys.Theme, theme);
        }

        private void ChangeHue(object obj)
        {
            var hue = (Color)obj;
            ITheme theme = _paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(hue.Lighten());
            theme.PrimaryMid = new ColorPair(hue);
            theme.PrimaryDark = new ColorPair(hue.Darken());
            _paletteHelper.SetTheme(theme);
            _settingsManager.SetSetting(SettingKeys.Theme, theme);
        }
    }
}
