using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Practice.Helpers;
using Practice.Services;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Practice.ViewModels
{
    public class HomeViewModel : ReactiveObject
    {
        private static readonly SKColor s_blue = new(25, 118, 210);

        public HomeViewModel(SafetyUiAction safetyUiAction)
        {
            var observableValues = new ObservableLimitedLengthQueue<ObservableValue>(15);
            CpuSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = observableValues,
                    Fill = null,
                    Name = "CPU",
                    Stroke = new SolidColorPaint(s_blue, 2),
                    GeometrySize = 10,
                    GeometryStroke = new SolidColorPaint(s_blue, 2),
                }
            };

            Task.Run(function: async () =>
            {
                while (true)
                {
                    var usage = await GetCpuUsageForProcess();
                    var memory = GetPhysicalMemory();
                    safetyUiAction.Invoke(() => observableValues.Enqueue(new ObservableValue(usage)));
                    await Task.Delay(2000);
                }
            });
        }

        public ObservableCollection<ISeries> CpuSeries { get; set; }

        public LabelVisual CpuTitle { get; set; } =
            new LabelVisual
            {
                Text = "CPU 使用率(%)",
                TextSize = 20,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        public ObservableCollection<ISeries> PhysicalMemorySeries { get; set; }

        public LabelVisual PhysicalMemoryTitle { get; set; } =
            new LabelVisual
            {
                Text = "物理内存",
                TextSize = 20,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        public ObservableCollection<ISeries> PrivateMemorySeries { get; set; }

        public LabelVisual PrivateMemoryTitle { get; set; } =
            new LabelVisual
            {
                Text = "专用内存",
                TextSize = 20,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        private readonly Process _currentProcess = Process.GetCurrentProcess();

        private async Task<double> GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = _currentProcess.TotalProcessorTime;

            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = _currentProcess.TotalProcessorTime;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            var cpuUsageTotal = Math.Round(cpuUsedMs / (Environment.ProcessorCount * totalMsPassed) * 100, 1, MidpointRounding.AwayFromZero);
            return cpuUsageTotal;
        }

        private double GetPhysicalMemory()
        {

            var physicalMemory = _currentProcess.WorkingSet64;
            var allocationInMB = _currentProcess.PrivateMemorySize64 / 1024;
            var sss = _currentProcess.WorkingSet64 / 1024;
            var mb = Math.Round((double)physicalMemory / (1024), 1, MidpointRounding.AwayFromZero);
            return mb;
        }
    }
}
