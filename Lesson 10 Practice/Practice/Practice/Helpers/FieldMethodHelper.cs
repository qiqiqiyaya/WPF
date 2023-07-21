using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Navigation;

namespace Practice.Helpers
{
    public static class FieldMethodHelper
    {
        public static void OpenInBrowser(string? url)
        {
            if (url is not null && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }
    }
}
