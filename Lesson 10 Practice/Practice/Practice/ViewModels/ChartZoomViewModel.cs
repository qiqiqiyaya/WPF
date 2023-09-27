using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Practice.Extensions;
using Practice.Helpers;
using Practice.Services;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Practice.ViewModels
{
    public class ChartZoomViewModel : ReactiveObject
    {
        private readonly SafetyUiActionService _safetyUiActionService;

        public ChartZoomViewModel(SafetyUiActionService safetyUiActionService)
        {
            _safetyUiActionService = safetyUiActionService;
            Init();
        }

        private readonly ObservableLimitedLengthQueue<ObservableValue> _chartValues = new ObservableLimitedLengthQueue<ObservableValue>(300);
        public ObservableCollection<ISeries> ChartSeries => new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = _chartValues,
                Fill = new SolidColorPaint(SKColors.Blue),
                Name = "value",
                Stroke = new SolidColorPaint(SKColors.Blue, 1),
                GeometrySize = 5,
                GeometryStroke = new SolidColorPaint(SKColors.Blue, 1),
            }
        };

        public LabelVisual ChartTitle { get; set; } =
            new LabelVisual
            {
                Text = "图表缩放测试",
                TextSize = 15,
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
                {
                    SKTypeface = SKFontManager.Default.MatchCharacter("MiSans-Normal", '汉')
                }
            };

        private void Init()
        {
            // 交给线程池中线程运行
            Task.Run(function: async () =>
            {
                int i = 0;
                Random ran = new Random();
                while (i < 300)
                {
                    int number = ran.Next(100);
                    // 计算结果操作交给UI线程操作
                    _safetyUiActionService.Invoke(() =>
                    {
                        _chartValues.Add(new ObservableValue(number));
                    });
                    await Task.Delay(50);
                    i++;
                }
            }).FireAndForget();
        }

        protected void Reset()
        {

        }
    }
}
