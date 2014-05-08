using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        protected override void AddingChild(FrameworkElement element, Inline child)
        {
            var textBlock = (TextBlock)element;
            if (FormattingCalculator.RequiresSpace(textBlock.Inlines.LastInline, child))
                textBlock.Inlines.Add(new Run(" "));
        }

        protected override void AddingChild(FrameworkElement element, UIElement child)
        {
            Debug.WriteLine("XParagraph only supports inline content");
        }

        protected override void AddingChild(StringBuilder stringBuilder, XInline child)
        {
            var lastCharacter = stringBuilder.Length > 0 ? stringBuilder[stringBuilder.Length - 1] : '\0';
            if (BlockStyle == BlockStyle.Indented)

            {
                if (stringBuilder.Length == 0 || lastCharacter == '\n')
                    stringBuilder.Append("  ");
            }

            if (child is XLineBreak)
                return;

            if (FormattingCalculator.RequiresSpace(lastCharacter, (child.Text ?? string.Empty).FirstOrDefault()))
                stringBuilder.Append(" ");
        }

        protected override void AddChild(StringBuilder stringBuilder, XBlock child)
        {
            Debug.WriteLine("XParagraph only supports inline content");
        }

        protected override void AddChild(StringBuilder stringBuilder, XInline child)
        {
            stringBuilder.Append(child);
        }

        protected override void AddingChild(StringBuilder stringBuilder, XBlock child)
        {
            Debug.WriteLine("XParagraph only supports inline content");
        }

        public override string ToString()
        {
            return ShouldBuildElement() ? base.ToString() + Environment.NewLine : base.ToString();
        }

        public override string ToPlainString()
        {
            var regex = new Regex(@"^ ?(.*?) ?\r?$", RegexOptions.Multiline);
            var indent = BlockStyle == BlockStyle.Indented ? "  " : string.Empty;
            return indent + regex.Replace(string.Join(" ", Children.Select(s => s.ToPlainString())), indent + "$1").Replace("\n", Environment.NewLine).Trim() + "\r\n";
        }
    }
}