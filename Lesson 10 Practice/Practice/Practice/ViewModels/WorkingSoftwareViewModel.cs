using System.Collections.Generic;
using Microsoft.Win32;
using Practice.Extensions;
using Practice.Models;
using Practice.Services.interfaces;
using Prism.Services.Dialogs;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Practice.Services;

namespace Practice.ViewModels
{
    public class WorkingSoftwareViewModel : ReactiveObject
    {
        private const string AppsPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

        private readonly SafetyUiAction _safetyUiAction;

        public WorkingSoftwareViewModel(SafetyUiAction safetyUiAction)
        {
            _safetyUiAction = safetyUiAction;
            LoadApps();
        }

        private ObservableCollection<AppInfo> _apps = new ObservableCollection<AppInfo>();
        public ObservableCollection<AppInfo> Apps => _apps;


        public void LoadApps()
        {
            Task.Run(() =>
            {
                RegistryKey? appsRegistryKey = Registry.LocalMachine.OpenSubKey(AppsPath);
                if (appsRegistryKey == null) return;

                var subKeyNames = appsRegistryKey.GetSubKeyNames();
                var data = new List<AppInfo>();
                foreach (var key in subKeyNames)
                {
                    RegistryKey? subRegistryKey = appsRegistryKey.OpenSubKey(key);
                    if (subRegistryKey == null) return;

                    using (subRegistryKey)
                    {
                        var app = new AppInfo();

                        app.DisplayName = subRegistryKey.GetString("DisplayName");
                        app.Publisher = subRegistryKey.GetString("Publisher");
                        app.InstallLocation = subRegistryKey.GetString("InstallLocation");
                        app.HelpLink = subRegistryKey.GetString("HelpLink");
                        app.DisplayIcon = subRegistryKey.GetString("DisplayIcon");
                        data.Add(app);
                    }
                }

                var aa = data.FirstOrDefault(x => x.DisplayName.Contains("WeChat"));
                _safetyUiAction.Invoke(() => _apps.AddRange(data));
            });
        }

        private void GetApp(string subKeyName)
        {

        }
    }
}
