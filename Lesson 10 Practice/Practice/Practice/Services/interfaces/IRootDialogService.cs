using System.Threading.Tasks;

namespace Practice.Services.interfaces
{
    public interface IRootDialogService
    {
        Task LoadingShow();

        void LoadingClose();
    }
}
