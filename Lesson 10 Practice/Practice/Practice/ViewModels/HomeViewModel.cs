using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Practice.Core;
using Practice.Helpers;
using Practice.Models;
using Practice.Services;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Practice.ViewModels
{
    public sealed class HomeViewModel : ReactiveObject, ITabItemMenuChangeAction, IDisposable
    {
        private static readonly SKColor Blue = new(25, 118, 210);

        private readonly PauseTokenSource _pauseTokenSource = new PauseTokenSource();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public HomeViewModel(SafetyUiAction safetyUiAction)
        {
            var pauseToken = _pauseTokenSource.Token;
            var cancellationToken = _cancellationTokenSource.Token;

            var cupValues = new ObservableLimitedLengthQueue<ObservableValue>(15);
            CpuSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = cupValues,
                    Fill =   new SolidColorPaint(SKColors.CornflowerBlue),
                    Name = "CPU",
                    Stroke = new SolidColorPaint(Blue, 1),
                    GeometrySize = 5,
                    GeometryStroke = new SolidColorPaint(Blue, 1),
                }
            };

            var physicalMemory = new ObservableLimitedLengthQueue<ObservableValue>(15);
            PhysicalMemorySeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = physicalMemory,
                    Fill =  null,
                    Name = "Physical Memory",
                    Stroke = new SolidColorPaint(Blue, 1),
                    GeometrySize = 5,
                    GeometryStroke = new SolidColorPaint(Blue, 1),
                }
            };

            var privateMemory = new ObservableLimitedLengthQueue<ObservableValue>(15);
            PrivateMemorySeries = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = privateMemory,
                    Fill =  null,
                    Name = "Private Memory",
                    Stroke = new SolidColorPaint(Blue, 1),
                    GeometrySize = 5,
                    GeometryStroke = new SolidColorPaint(Blue, 1),
                }
            };

            // 交给线程池中线程运行
            Task.Run(function: async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var usage = await GetCpuUsageForProcess();
                    var memory = GetMemoryUsageForProcess();

                    // 计算结果操作交给UI线程操作
                    safetyUiAction.Invoke(() =>
                    {
                        cupValues.Enqueue(new ObservableValue(usage));
                        physicalMemory.Enqueue(new ObservableValue(memory.Item1));
                        privateMemory.Enqueue(new ObservableValue(memory.Item2));
                    });
                    await Task.Delay(2000);
                    if (pauseToken.IsPaused) await pauseToken.WaitAsync();
                }
            });

            SystemInformation = GetSystemInformation();
        }

        public void OnInit()
        {
            _pauseTokenSource.Resume();
        }

        public void OnDestroy()
        {
            _pauseTokenSource.Pause();
        }

        public ObservableCollection<ISeries> CpuSeries { get; set; }

        public LabelVisual CpuTitle { get; set; } =
            new LabelVisual
            {
                Text = "CPU 使用率(%)",
                TextSize = 15,
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        public ObservableCollection<ISeries> PhysicalMemorySeries { get; set; }

        public LabelVisual PhysicalMemoryTitle { get; set; } =
            new LabelVisual
            {
                Text = "物理内存(MB)",
                TextSize = 15,
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        public ObservableCollection<ISeries> PrivateMemorySeries { get; set; }

        public LabelVisual PrivateMemoryTitle { get; set; } =
            new LabelVisual
            {
                Text = "专用内存(MB)",
                TextSize = 15,
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        public SystemInformation SystemInformation { get; }

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

        private (double, double) GetMemoryUsageForProcess()
        {
            // 大概算出一个数值
            var physicalMemory = _currentProcess.WorkingSet64 / (1024 * 1024);
            var privateMemory = _currentProcess.PrivateMemorySize64 / (1024 * 1024);
            return (physicalMemory, privateMemory);
        }

        private SystemInformation GetSystemInformation()
        {
            SystemInformation system = new SystemInformation
            {
                OperationSystem = Environment.OSVersion.ToString(),
                ProcessorArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"),
                ProcessorModel = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"),
                SystemDirectory = Environment.SystemDirectory,
                ProcessorCount = Environment.ProcessorCount,
                UserDomainName = Environment.UserDomainName,
                UserName = Environment.UserName,
                Version = Environment.Version.ToString()
            };
            return system;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _pauseTokenSource.Dispose();
            _currentProcess.Dispose();
        }
    }
}
