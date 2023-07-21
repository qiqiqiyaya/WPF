using Serilog;
using System;

namespace Practice
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File($"Logs/logs.txt"))
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
