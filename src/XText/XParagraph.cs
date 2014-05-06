using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XText
{
    /// <summary>
    /// Represents a paragraph of text
    /// </summary>
    public class XParagraph : XBlock
    {
        public XParagraph(params XInline[] children)
            : base(BlockStyle.Normal, children.Cast<XTextElement>().ToArray())
        {
        }

        public XParagraph(Func<bool> writeIf, params XInline[] children)
            : base(writeIf, BlockStyle.Normal, children.Cast<XTextElement>().ToArray())
        {
        }

        public XParagraph(BlockStyle blockStyle = BlockStyle.Normal, params XInline[] children)
            : base(blockStyle, children.Cast<XTextElement>().ToArray())
        {
        }

        public XParagraph(Func<bool> writeIf, BlockStyle blockStyle = BlockStyle.Normal, params XInline[] children)
            : base(writeIf, blockStyle, children.Cast<XTextElement>().ToArray())
        {
        }

        public void AddChild(XTextElement child)
        {
            Children.Add(child);
        }

        protected override FrameworkElement BuildElementInternal()
        {
            return new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
        }

        protected override void AddChild(FrameworkElement element, UIElement child)
        {
            Debug.WriteLine("XParagraph only supports inline content");
        }

        protected override void AddChild(FrameworkElement element, Inline child)
        {
            ((TextBlock)element).Inlines.Add(child);
        }

        protected override void PostChildrenAdded(FrameworkElement element)
        {
            if (!(((TextBlock)element).Inlines.LastInline is LineBreak))
                ((TextBlock)element).Inlines.Add(new Run(" "));
        }

        public override string ToString()
        {
            var regex = new Regex(@"^ ?(.*?) ?\r?$", RegexOptions.Multiline);
            var indent = BlockStyle == BlockStyle.Indented ? "  " : string.Empty;
            return indent + regex.Replace(string.Join(" ", Children.Select(s => s.ToString())), indent + "$1").Replace("\n", Environment.NewLine).Trim() + "\r\n";
        }

        public override string ToPlainString()
        {
            var regex = new Regex(@"^ ?(.*?) ?\r?$", RegexOptions.Multiline);
            var indent = BlockStyle == BlockStyle.Indented ? "  " : string.Empty;
            return indent + regex.Replace(string.Join(" ", Children.Select(s => s.ToPlainString())), indent + "$1").Replace("\n", Environment.NewLine).Trim() + "\r\n";
        }
    }
}