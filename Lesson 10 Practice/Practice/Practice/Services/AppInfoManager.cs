using System;
using Practice.Helpers.IconExtractor;
using Practice.Models;
using Practice.Provider.Interfaces;
using Practice.Services.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Practice.Common;
using Serilog;
using Practice.Extensions;

namespace Practice.Services
{
    public class AppInfoManager : IAppInfoManager
    {
        private readonly IAppInfoProvider _appInfoProvider;
        private readonly ILogger _logger;

        public AppInfoManager(IAppInfoProvider appInfoProvider, ILogger log)
        {
            _logger = log;
            _appInfoProvider = appInfoProvider;
        }

        /// <inheritdoc/>
        public virtual Result<List<AppInfo>> GetAll()
        {
            Result<List<AppInfo>> result = new Result<List<AppInfo>>();
            try
            {
                result.Data = _appInfoProvider.GetAll();
            }
            catch (Exception e)
            {
                _logger.ErrorDetail(e);
                result.SetException(e);
            }

            return result;
        }

        /// <summary>
        /// 获取所有、及其Icon
        /// </summary>
        /// <returns></returns>
        public virtual async IAsyncEnumerable<AppIcon> GetAllIcons()
        {
            var result = GetAll();
            if (result.HasException)
            {
                throw result.Exception!;
            }

            foreach (var item in result.Data)
            {
                await Task.Delay(10);
                var icon = ConvertToIcon(item);
                yield return item.ToIcon(icon);
            }
        }

        /// <inheritdoc/>
        public virtual Result<List<AppInfo>> GetWorkingSoftwareInfo()
        {
            var list = new List<string>() { "visual studio code", "visual studio", "starUML", "microsoft edge",
                "google chrome", "photoshop","oracle vm virtualbox","docker desktop","postman","sql server management",
                "navicat"
            };

            Result<List<AppInfo>> result = new Result<List<AppInfo>>();

            try
            {
                var data = _appInfoProvider.GetList(x =>
                {
                    var name = x.DisplayName.ToLower();
                    // first
                    if (x.DisplayIcon.IsNullOrWhiteSpace() || x.InstallLocation.IsNullOrWhiteSpace())
                    {
                        return false;
                    }

                    if (list.Any(se => name.Contains(se)))
                    {
                        return true;
                    }

                    return false;
                });

                result.Data = data;
            }
            catch (Exception e)
            {
                _logger.ErrorDetail(e);
                result.SetException(e);
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual Result<List<AppIcon>> GetWorkingSoftwareIcon()
        {
            Result<List<AppIcon>> iconResult = new Result<List<AppIcon>>();

            var result = GetWorkingSoftwareInfo();
            if (!result.HasException)
            {
                iconResult.Data = ConvertToAppIcon(result.Data).ToList();
            }
            else
            {
                iconResult.SetException(result.Exception!);
            }

            return iconResult;
        }

        protected virtual IEnumerable<AppIcon> ConvertToAppIcon(List<AppInfo> appInfos)
        {
            return appInfos.Select(s =>
            {
                var icon = ConvertToIcon(s);
                return s.ToIcon(icon);
            });
        }

        protected virtual bool IsIcon(string fileName)
        {
            return Path.GetExtension(fileName).ToLower() == ".ico";
        }

        protected virtual Icon? ConvertToIcon(AppInfo info)
        {
            if (IsIcon(info.DisplayIcon))
            {
                if (!File.Exists(info.DisplayIcon))
                {
                    return null;
                }
                return new Icon(info.DisplayIcon);
            }
            else
            {
                try
                {
                    IconExtractor icon = new IconExtractor(info.DisplayIcon);
                    return icon.GetIcon(0);
                }
                catch
                {
                    return null;
                }

            }
        }
    }
}
