﻿using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace Practice.Services.interfaces
{
    public interface IRootDialogService
    {
        void Init(DialogHost rooDialogHost);

        void LoadingShow();

        void LoadingClose();
    }
}
