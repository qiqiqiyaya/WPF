using Microsoft.Win32;

namespace Practice.Extensions
{
    public static class RegistryKeyExtensions
    {
        public static string GetString(this RegistryKey registryKey, string? name)
        {
            var obj = registryKey.GetValue(name);
            if (obj == null) return "";
            var value = obj.ToString();

            return value ?? "";
        }
    }
}
