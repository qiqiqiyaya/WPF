using System.Windows;
using System.Windows.Controls;

namespace Practice.Common
{
    public class BlockquoteControl : ContentControl
    {
        static BlockquoteControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlockquoteControl), new FrameworkPropertyMetadata(typeof(BlockquoteControl)));
        }

        /// <summary>
        /// DependencyProperty for <see cref="Title" /> property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(BlockquoteControl),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [Localizability(LocalizationCategory.Text)]
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
    }
}
