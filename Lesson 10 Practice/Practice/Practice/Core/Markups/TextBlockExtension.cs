using System;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Practice.Core.Markups
{
    [MarkupExtensionReturnType(typeof(TextBlock))]
    public class TextBlockExtension : MarkupExtension
    {
        public TextBlockExtension()
        {

        }

        public TextBlockExtension(string text)
        {
            Text = text;
        }

        [ConstructorArgument("text")]
        public string? Text { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var textBlock = new TextBlock
            {
                Text = Text
            };
            return textBlock;
        }
    }
}
