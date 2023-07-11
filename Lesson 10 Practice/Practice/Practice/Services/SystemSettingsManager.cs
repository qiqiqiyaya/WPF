using Newtonsoft.Json;
using Practice.Properties;
using System.Collections.Generic;
using Practice.Extensions;


namespace Practice.Services
{
    public class SystemSettingsManager
    {
        public SystemSettingsManager()
        {

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

            throw new KeyNotFoundException($"The {nameof(key)} can't find from {nameof(Settings)}");
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
