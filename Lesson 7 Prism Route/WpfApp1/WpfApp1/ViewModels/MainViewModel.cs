using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> OpenCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        private IRegionNavigationJournal _journal;

        public MainViewModel(IRegionManager regionManager)
        {
            OpenCommand = new DelegateCommand<string>(Open);
            BackCommand = new DelegateCommand(Back);
            _regionManager = regionManager;
        }

        private void Back()
        {
            if (_journal.CanGoBack)
            {
                _journal.GoBack();
            }
        }

        private void Open(string obj)
        {
            // 通过 IRegionManager 接口获取全局定义得可用区域
            // 往这个区域动态得去设置内容
            // 设置内容得方式是通过依赖注入注册的
            NavigationParameters keys = new NavigationParameters();
            keys.Add("Title", "Hello!");

            _regionManager.Regions["ContentRegion"].RequestNavigate(obj, navigationResult =>
            {
                if (navigationResult.Result.HasValue)
                {
                    _journal = navigationResult.Context.NavigationService.Journal;
                }
            });

        }
    }
}
