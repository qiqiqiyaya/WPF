using Practice.Core;
using Serilog;
using System;

namespace Practice
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            // serilog 设置参考 https://mbarkt3sto.hashnode.dev/logging-to-a-file-using-serilog#heading-configuration

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                // 滚动日志，按天，最多保存最近15天内的日志
                .WriteTo.Async(c => c.File(SystemSettingKeys.GetTxtLogsPath(),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 15))
                .WriteTo.SQLite(SystemSettingKeys.GetSqliteDbPath())
                .CreateLogger();

            Log.Logger.Information("应用程序启动");

            try
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "程序终止运行");
            }
            finally
            {
                Log.Logger.Information("应用程序退出");
                Log.CloseAndFlush();
            }
        }
    }
}
