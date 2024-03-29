﻿using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Practice.Services;
using Prism.Commands;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Practice.Core;
#pragma warning disable CS8618

namespace Practice.ViewModels
{
    public class ThemeChangeViewModel : ReactiveObject
    {
        private readonly PaletteHelper _paletteHelper;
        private readonly SystemSettingsManager _systemSettingsManager;
        public DelegateCommand<object> ChangeHueCommand { get; }
        public DelegateCommand<ISwatch> MainColorButtonCommand { get; }

        public ThemeChangeViewModel(PaletteHelper paletteHelper, SystemSettingsManager systemSettingsManager, SafetyUiActionService safetyUiActionService)
        {
            _paletteHelper = paletteHelper;
            _systemSettingsManager = systemSettingsManager;
            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
            MainColorButtonCommand = new DelegateCommand<ISwatch>(swatch =>
            {
                Colors.Clear();
                Colors.AddRange(swatch.Hues);
            });

            _isDarkTheme = _paletteHelper.GetTheme().GetBaseTheme() == BaseTheme.Dark;
            _colors = new ObservableCollection<Color>();

            safetyUiActionService.DelayWhen(() => Swatches = new ObservableCollection<ISwatch>(SwatchHelper.Swatches), 150);
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

        private ObservableCollection<ISwatch> _swatches;
        public ObservableCollection<ISwatch> Swatches
        {
            get => _swatches;
            protected set => this.RaiseAndSetIfChanged(ref _swatches, value);
        }

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
            _systemSettingsManager.SetSetting(SystemSettingKeys.Theme, theme);
        }

        private void ChangeHue(object obj)
        {
            var hue = (Color)obj;
            ITheme theme = _paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(hue.Lighten());
            theme.PrimaryMid = new ColorPair(hue);
            theme.PrimaryDark = new ColorPair(hue.Darken());
            _paletteHelper.SetTheme(theme);
            _systemSettingsManager.SetSetting(SystemSettingKeys.Theme, theme);
        }
    }
}
