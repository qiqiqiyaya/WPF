using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Practice.Core;
using Practice.Extensions;
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

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;
        private readonly SafetyUiActionService _safetyUiActionService;

        public HomeViewModel(SafetyUiActionService safetyUiActionService)
        {
            _safetyUiActionService = safetyUiActionService;
            _cancellationToken = _cancellationTokenSource.Token;

            IntervalAction();
            SystemInformation = GetSystemInformation();
        }

        public void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            IntervalAction();
        }

        public void OnLeave()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private readonly ObservableLimitedLengthQueue<ObservableValue> _cupValues = new ObservableLimitedLengthQueue<ObservableValue>(15);

        public ObservableCollection<ISeries> CpuSeries => new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = _cupValues,
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                Name = "CPU",
                Stroke = new SolidColorPaint(Blue, 1),
                GeometrySize = 5,
                GeometryStroke = new SolidColorPaint(Blue, 1),
            }
        };

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

        private readonly ObservableLimitedLengthQueue<ObservableValue> _physicalMemory = new ObservableLimitedLengthQueue<ObservableValue>(15);

        public ObservableCollection<ISeries> PhysicalMemorySeries => new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = _physicalMemory,
                Fill = null,
                Name = "Physical Memory",
                Stroke = new SolidColorPaint(Blue, 1),
                GeometrySize = 5,
                GeometryStroke = new SolidColorPaint(Blue, 1),
            }
        };

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

        private readonly ObservableLimitedLengthQueue<ObservableValue> _privateMemory = new ObservableLimitedLengthQueue<ObservableValue>(15);

        public ObservableCollection<ISeries> PrivateMemorySeries => new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = _privateMemory,
                Fill = null,
                Name = "Private Memory",
                Stroke = new SolidColorPaint(Blue, 1),
                GeometrySize = 5,
                GeometryStroke = new SolidColorPaint(Blue, 1),
            }
        };

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

        /// <summary>
        /// 当前进程
        /// </summary>
        private readonly Process _currentProcess = Process.GetCurrentProcess();

        /// <summary>
        /// 获取当前进程cpu使用率
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<double> GetCpuUsageForProcess(CancellationToken token)
        {

            var startTime = DateTime.UtcNow;
            var startCpuUsage = _currentProcess.TotalProcessorTime;

            await Task.Delay(500, token);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = _currentProcess.TotalProcessorTime;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            var cpuUsageTotal = Math.Round(cpuUsedMs / (Environment.ProcessorCount * totalMsPassed) * 100, 1, MidpointRounding.AwayFromZero);
            return cpuUsageTotal;
        }

        /// <summary>
        /// 获取当前进程内存使用率
        /// </summary>
        /// <returns></returns>
        private (double, double) GetMemoryUsageForProcess()
        {
            // 大概算出一个数值
            var physicalMemory = _currentProcess.PeakWorkingSet64 / (1024 * 1024);
            var privateMemory = _currentProcess.PrivateMemorySize64 / (1024 * 1024);
            return (physicalMemory, privateMemory);
        }

        /// <summary>
        /// 获取宿主主机系统信息
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 定时执行操作
        /// </summary>
        private void IntervalAction()
        {
            // 交给线程池中线程运行
            Task.Run(function: async () =>
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var usage = await GetCpuUsageForProcess(_cancellationToken);
                    var memory = GetMemoryUsageForProcess();

                    // 计算结果操作交给UI线程操作
                    _safetyUiActionService.Invoke(() =>
                    {
                        _cupValues.Enqueue(new ObservableValue(usage));
                        _physicalMemory.Enqueue(new ObservableValue(memory.Item1));
                        _privateMemory.Enqueue(new ObservableValue(memory.Item2));
                    });
                    await Task.Delay(2500, _cancellationToken);
                }
            }, _cancellationToken).FireAndForget();
        }

        public void Dispose()
        {
            if (!_cancellationTokenSource.IsCancellationRequested) _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _currentProcess.Dispose();
        }
    }
}
