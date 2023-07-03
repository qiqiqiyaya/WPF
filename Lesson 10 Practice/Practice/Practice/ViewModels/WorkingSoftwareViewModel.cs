﻿using MaterialDesignThemes.Wpf;
using Practice.CommonViews;
using Practice.Helpers;
using Practice.Services.interfaces;
using Prism.Services.Dialogs;
using System.Windows.Input;
using Practice.Core.Contract;

namespace Practice.ViewModels
{
    public class WorkingSoftwareViewModel
    {
        private readonly IRootDialogService _rootDialogService;
        private readonly IDialogService _dialogService;


        public WorkingSoftwareViewModel(IRootDialogService rootDialogService,
            IDialogService dialogService)
        {
            _rootDialogService = rootDialogService;
            _dialogService = dialogService;
            OpenCommand = new RelayCommand(OnShowDialog);
        }

        private async void OnShowDialog()
        {
            var aa = await DialogHost.Show(new LoadingView(), SystemSettingKeys.RootDialogIdentity);
        }

        public ICommand OpenCommand { get; }
    }
}
