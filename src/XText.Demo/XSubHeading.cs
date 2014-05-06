using System;
using System.Windows;
using System.Windows.Controls;

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
            : base(writeIf, blockStyle, null)
        {
            Text = text;
        }

        protected override FrameworkElement BuildElementInternal()
        {
            var buildElementInternal = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = Text, Margin = new Thickness(0, 5, 0, 5) };
            buildElementInternal.SetResourceReference(FrameworkContentElement.StyleProperty, "H2");
            return buildElementInternal;
        }

        public override string ToString()
        {
            return string.Format("{0}# {1}", BlockStyle == BlockStyle.Indented ? "  " : string.Empty, Text);
        }

        public override string ToPlainString()
        {
            return string.Format("{0} {1}", BlockStyle == BlockStyle.Indented ? "  " : string.Empty, Text);
        }
    }
}