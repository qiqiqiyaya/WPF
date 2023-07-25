using Practice.Extensions;
using Practice.Models;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Controls.Button;

#pragma warning disable CS8602

// ReSharper disable ConditionIsAlwaysTrueOrFalse
#pragma warning disable CS8618
#pragma warning disable CS8601

namespace Practice.Common
{
    [TemplatePart(Name = "PrevButtonTemplateName", Type = typeof(Button))]
    [TemplatePart(Name = "NextButtonTemplateName", Type = typeof(Button))]
    public class Pagination : Selector
    {
        private const string PrevButtonTemplateName = "PART_PREV";
        private const string NextButtonTemplateName = "PART_Next";
        /// <summary>
        /// 更多按钮
        /// </summary>
        private const string MoreButtonContent = "...";

        private Button _prevButton;
        private Button _nextButton;

        /// <summary>
        /// 是否第一次触发事件，第一次初始化时，阻止事件粗发
        /// </summary>
        //public bool IsFirstRaise { get; set; } = true;

        static Pagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(typeof(Pagination)));

            // 重写原数据
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(WrapPanel)));
            template.Seal();
            ItemsPanelProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(template));
        }

        /// <summary>
        ///     The DependencyProperty for RoutedCommand
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(Pagination),
                new FrameworkPropertyMetadata((ICommand)null!));

        /// <summary>
        /// Get or set the Command property
        /// </summary>
        [Bindable(true), Category("Action")]
        [Localizability(LocalizationCategory.NeverLocalize)]
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent(nameof(PageChangedEvent),
            RoutingStrategy.Direct,
            typeof(RoutedPropertyChangedEventArgs<int>),
            typeof(Pagination));

        /// <summary>
        /// PageChanging事件
        /// </summary>
        public event RoutedEventHandler PageChanged
        {
            add => AddHandler(PageChangedEvent, value);
            remove => RemoveHandler(PageChangedEvent, value);
        }

        public static readonly DependencyProperty PageSizeSourceProperty =
            DependencyProperty.Register(nameof(PageSizeSource),
                typeof(int[]),
                typeof(Pagination),
                new PropertyMetadata(new int[5] { 20, 30, 40, 50, 60 }));

        /// <summary>
        /// 每页行数显示
        /// </summary>
        public int[] PageSizeSource
        {
            get => (int[])GetValue(PageSizeSourceProperty);
            set => SetValue(PageSizeSourceProperty, value);
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(nameof(PageSize),
                typeof(int),
                typeof(Pagination),
                new FrameworkPropertyMetadata(20, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PageSizeChange));

        /// <summary>
        /// 每页数据条目个数
        /// </summary>
        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get => (int)GetValue(TotalPageCountProperty);
            set => SetValue(TotalPageCountProperty, value);
        }

        public static readonly DependencyProperty TotalPageCountProperty =
            DependencyProperty.Register(nameof(TotalPageCount),
                typeof(int),
                typeof(Pagination),
                new PropertyMetadata(0));

        /// <summary>
        /// 总条目数
        /// </summary>
        public int Total
        {
            get => (int)GetValue(TotalProperty);
            set => SetValue(TotalProperty, value);
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register(nameof(Total),
                typeof(int),
                typeof(Pagination),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RoutedRaiseEvent));

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageNumber
        {
            get => (int)GetValue(PageNumberProperty);
            set => SetValue(PageNumberProperty, value);
        }

        public static readonly DependencyProperty PageNumberProperty =
            DependencyProperty.Register(nameof(PageNumber),
                typeof(int),
                typeof(Pagination),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RoutedRaiseEvent));

        /// <summary>
        /// 页码按钮的数量，当总页数超过该值时会折叠  大于等于 5 且小于等于 21 的奇数
        /// </summary>
        /// <remarks>
        /// 默认显示 7 个
        /// </remarks>
        public int ButtonCount
        {
            get => (int)GetValue(ButtonCountProperty);
            set => SetValue(ButtonCountProperty, value);
        }

        public static readonly DependencyProperty ButtonCountProperty =
            DependencyProperty.Register(nameof(ButtonCount),
                typeof(int),
                typeof(Pagination),
                new PropertyMetadata(7, ButtonCountChange),
                OnPagerCountPropertyValidate);

        public override void OnApplyTemplate()
        {
            if (Total == 0 || PageNumber == 0)
            {
                SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }

            base.OnApplyTemplate();
            _prevButton = GetTemplateChild(PrevButtonTemplateName) as Button;
            _nextButton = GetTemplateChild(NextButtonTemplateName) as Button;

            Check.NotNull(_prevButton, nameof(_prevButton));
            Check.NotNull(_nextButton, nameof(_nextButton));

            _prevButton.Click += Prev;
            _nextButton.Click += Next;
            _prevButton.SetCurrentValue(IsEnabledProperty, PageNumber != 1);
            _nextButton.SetCurrentValue(IsEnabledProperty, PageNumber != TotalPageCount);

            ButtonRender();
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(PageNumberProperty, PageNumber + 1);
            ButtonRender();
            OtherButtonRender();
        }

        private void Prev(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(PageNumberProperty, PageNumber - 1);
            ButtonRender();
            OtherButtonRender();
        }

        #region 汉化设置

        public static readonly DependencyProperty PageNameFormatProperty =
            DependencyProperty.Register(nameof(PageNameFormat),
                typeof(string),
                typeof(Pagination),
                new PropertyMetadata("页"));

        /// <summary>
        /// 页，名称
        /// </summary>
        public string PageNameFormat
        {
            get => (string)GetValue(PageNameFormatProperty);
            set => SetValue(PageNameFormatProperty, value);
        }

        public static readonly DependencyProperty GotoFormatProperty =
            DependencyProperty.Register(nameof(GotoFormat),
                typeof(string),
                typeof(Pagination),
                new PropertyMetadata("前往"));

        /// <summary>
        /// 前往
        /// </summary>
        public string GotoFormat
        {
            get => (string)GetValue(GotoFormatProperty);
            set => SetValue(GotoFormatProperty, value);
        }

        public static readonly DependencyProperty ContentStringFormatProperty =
            DependencyProperty.Register(nameof(ContentStringFormat),
                typeof(string),
                typeof(Pagination),
                new PropertyMetadata("共{0}条"));

        /// <summary>
        /// 每页函数显示字符串
        /// </summary>
        public string ContentStringFormat
        {
            get => (string)GetValue(ContentStringFormatProperty);
            set => SetValue(ContentStringFormatProperty, value);
        }

        #endregion

        private static void PageSizeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                var newValue = (int)e.NewValue;
                var oldValue = (int)e.OldValue;
                if (newValue > oldValue)
                {
                    var pageCount = (int)Math.Ceiling((double)pagination.Total / pagination.PageSize);
                    if (pagination.PageNumber > pageCount)
                    {
                        pagination.SetValue(PageNumberProperty, pageCount);
                    }
                    else
                    {
                        var pageChanged = new RoutedPropertyChangedEventArgs<int>(pagination.PageNumber, pagination.PageNumber, PageChangedEvent);
                        pagination.RaiseEvent(pageChanged);
                        if (pageChanged.Handled)
                        {
                            return;
                        }

                        if (pagination.Command.CanExecute(pagination.PageNumber))
                        {
                            pagination.Command.Execute(pagination.PageNumber);
                        }
                    }
                }
                else
                {
                    var page = pagination.PageNumber;

                    var pageChanged = new RoutedPropertyChangedEventArgs<int>(page, page, PageChangedEvent);
                    pagination.RaiseEvent(pageChanged);
                    if (pageChanged.Handled)
                    {
                        return;
                    }

                    if (pagination.Command.CanExecute(page))
                    {
                        pagination.Command.Execute(page);
                    }
                }

                pagination.ButtonRender();
                pagination.OtherButtonRender();
            }
        }

        /// <summary>
        /// 按钮数量变更
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ButtonCountChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                pagination.ButtonRender();
            }
        }

        /// <summary>
        /// 当 ButtonCount 变更 ，或者 按钮为 ... 时被点击 ，或者初始化时，按钮需要重新渲染 
        /// </summary>
        public void ButtonRender()
        {
            var pageCount = (int)Math.Ceiling((double)Total / PageSize);
            var currentPage = PageNumber;
            var pagerCount = ButtonCount;

            var startPageIndex = currentPage - pagerCount / 2;
            startPageIndex = startPageIndex < 1 ? 1 : startPageIndex;

            var endPageIndex = startPageIndex + pagerCount - 1;
            endPageIndex = endPageIndex > pageCount ? pageCount : endPageIndex;

            startPageIndex = endPageIndex - pagerCount + 1;
            startPageIndex = startPageIndex < 1 ? 1 : startPageIndex;

            Items.Clear();

            if (startPageIndex > 1)
            {
                AddItem("1");
            }

            if (currentPage >= ((pagerCount - 1) / 2 + 2) && pageCount > 6)
            {
                AddItem(MoreButtonContent, PaginationButtonType.Prev);
                startPageIndex++;
            }

            for (int index = startPageIndex; index < endPageIndex; index++)
            {
                AddItem(index.ToString());
            }

            if (currentPage < (pageCount - (pagerCount - 1) / 2) && pageCount > 6)
            {
                AddItem(MoreButtonContent, PaginationButtonType.Next);
            }

            if (endPageIndex <= pageCount)
            {
                AddItem(pageCount.ToString());
            }
        }

        private static bool OnPagerCountPropertyValidate(object value)
        {
            return int.TryParse(value?.ToString(), out int num) && (num & 1) != 0 && num >= 5 && num <= 21;
        }

        private void OtherButtonRender()
        {
            if (Total == 0 || PageNumber == 0) return;

            var pageCount = (int)Math.Ceiling((double)Total / PageSize);
            SetCurrentValue(TotalPageCountProperty, pageCount);

            _prevButton?.SetCurrentValue(IsEnabledProperty, PageNumber != 1);
            _nextButton?.SetCurrentValue(IsEnabledProperty, PageNumber != TotalPageCount);
        }

        private void AddItem(string content, PaginationButtonType paginationButtonType = PaginationButtonType.Normal)
        {
            bool result = paginationButtonType == PaginationButtonType.Normal
                          && int.TryParse(content, out int num)
                          && num == PageNumber;

            var button = new PaginationButton()
            {
                Content = content,
                PaginationButtonType = paginationButtonType,
            };

            if (result)
            {
                button.IsEnabled = false;
            }
            button.Click += Button_Click;
            Items.Add(button);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is PaginationButton button)
            {
                switch (button.PaginationButtonType)
                {
                    case PaginationButtonType.Prev:
                        SetCurrentValue(PageNumberProperty, Math.Max(1, PageNumber - ButtonCount + 2));
                        break;
                    case PaginationButtonType.Next:
                        SetCurrentValue(PageNumberProperty, Math.Min(TotalPageCount, PageNumber + ButtonCount - 2));
                        break;
                    default:
                        if (int.TryParse(button.Content?.ToString(), out int num))
                        {
                            SetCurrentValue(PageNumberProperty, num);
                        }
                        break;
                }

                ButtonRender();
                OtherButtonRender();
            }
        }

        private static void RoutedRaiseEvent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination pagination)
            {
                if (e.Property == PageNumberProperty)
                {
                    var newValue = (int)e.NewValue;
                    var pageChanged = new RoutedPropertyChangedEventArgs<int>((int)e.OldValue, newValue, PageChangedEvent);
                    pagination.RaiseEvent(pageChanged);
                    if (pageChanged.Handled)
                    {
                        return;
                    }

                    if (pagination.Command.CanExecute(newValue))
                    {
                        pagination.Command.Execute(newValue);
                    }
                }

                pagination.ButtonRender();
                pagination.OtherButtonRender();
                if (pagination.Total > 0 && pagination.PageNumber > 0)
                {
                    pagination.SetValue(VisibilityProperty, Visibility.Visible);
                }
            }
        }
    }

    public class PaginationButton : Button
    {
        public PaginationButtonType PaginationButtonType
        {
            get => (PaginationButtonType)GetValue(PaginationButtonTypeProperty);
            set => SetValue(PaginationButtonTypeProperty, value);
        }

        public static readonly DependencyProperty PaginationButtonTypeProperty =
            DependencyProperty.Register(nameof(PaginationButtonType),
                typeof(PaginationButtonType),
                typeof(PaginationButton),
                new PropertyMetadata(PaginationButtonType.Normal));
    }

    public enum PaginationButtonType
    {
        /// <summary>
        /// 上一页
        /// </summary>
        Prev,
        /// <summary>
        /// 正常页码
        /// </summary>
        Normal,
        /// <summary>
        /// 下一页
        /// </summary>
        Next,
    }
}
