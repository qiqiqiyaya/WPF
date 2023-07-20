using Newtonsoft.Json;
using Practice.Properties;
using System.Collections.Generic;
using Practice.Extensions;
using Serilog;

namespace Practice.Services
{
    public class SystemSettingsManager
    {
        private readonly ILogger _logger;
        public SystemSettingsManager(ILogger logger)
        {
            _logger = logger;
        }

        public T? GetSetting<T>(string key)
        {
            Check.NotNull(key, nameof(key));

            var value = Settings.Default[key];
            if (value is string json)
            {
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }

            var ex = new KeyNotFoundException($"The {nameof(key)} can't find from {nameof(Settings)}");
            _logger.ErrorDetail(ex);
            throw ex;
        }

        public void SetSetting<T>(string key, T value)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(value, nameof(value));

            var result = JsonConvert.SerializeObject(value);
            Settings.Default[key] = result;
            Settings.Default.Save();
        }

    }
}
