using Practice.Models;
using Practice.Services;
using Practice.Services.Interfaces;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Practice.ViewModels
{
    public class WorkingSoftwareViewModel : ReactiveObject
    {
        private readonly SafetyUiAction _safetyUiAction;
        private readonly IAppInfoManager _appInfoManager;


        public WorkingSoftwareViewModel(SafetyUiAction safetyUiAction,
            IAppInfoManager appInfoManager)
        {
            _safetyUiAction = safetyUiAction;
            _appInfoManager = appInfoManager;
            LoadApps();
        }

        private ObservableCollection<AppIcon> _apps = new ObservableCollection<AppIcon>();

        public ObservableCollection<AppIcon> Apps
        {
            get => _apps;
            set => this.RaiseAndSetIfChanged(ref value, _apps);
        }

        public void LoadApps()
        {
            // 参考 https://stackoverflow.com/questions/2969821/display-icon-in-wpf-image

            Task.Run(async () =>
            {
                await foreach (var icon in _appInfoManager.GetAllIcons())
                {
                    _safetyUiAction.Invoke(() => Apps.Add(icon));
                }

                //if (result.HasException)
                //{

                //}
                //else
                //{
                //    //if (result.Data.Count > 20)
                //    //{
                //    //    foreach (var item in result.Data)
                //    //    {
                //    //        if (result.Data.Count % 20 == 0)
                //    //        {
                //    //            await Task.Delay(1000);
                //    //        }
                //    //        //_safetyUiAction.Invoke(() => Apps.Add(item));
                //    //    }
                //    //}
                //}
            });
        }

    }
}
