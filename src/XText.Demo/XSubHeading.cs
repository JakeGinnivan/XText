using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XText.Demo
{
    public class XSubHeading : XBlock
    {
        public XSubHeading(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }

        public XSubHeading(Func<bool> writeIf, string text, BlockStyle blockStyle = BlockStyle.Normal)
            : base(writeIf, blockStyle)
        {
            Text = text;
        }

        protected override FrameworkElement BuildElementInternal()
        {
            var buildElementInternal = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = Text, Margin = new Thickness(0, 5, 0, 5) };
            buildElementInternal.SetResourceReference(FrameworkContentElement.StyleProperty, "H2");
            return buildElementInternal;
        }

        protected override Block BuildDocumentInternal()
        {
            return new Paragraph(new Run(Text)
            {
                FontSize = 16
            });
        }

        protected override string ToString(bool formatted)
        {
            var format = formatted ? "{0}# {1}" : "{0} {1}";
            return string.Format(format, BlockStyle == BlockStyle.Indented ? "  " : string.Empty, Text);
        }
    }
}