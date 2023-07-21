using MaterialDesignThemes.Wpf;

namespace Practice.Services.Interfaces
{
    public interface IRootDialogService
    {
        void Init(DialogHost rooDialogHost);

        void Show(object content);

        void Close();

        void LoadingStateShow();

        void LoadingStateClose();
    }
}
