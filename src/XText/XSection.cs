using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XText
{
    /// <summary>
    /// Represents a list of elements, each child will be on it's own line
    /// </summary>
    public class XSection : XBlock
    {
        public XSection(BlockStyle blockStyle, params XTextElement[] children)
            : base(blockStyle, children)
        {
        }

        public XSection(Func<bool> writeIf, BlockStyle blockStyle, params XTextElement[] children)
            : base(writeIf, blockStyle, children)
        {
        }

        public XSection(params XTextElement[] children)
            : base(BlockStyle.Normal, children)
        {
        }

        public XSection(Func<bool> writeIf, params XTextElement[] children)
            : base(writeIf, BlockStyle.Normal, children)
        {
        }

        protected override FrameworkElement BuildElementInternal()
        {
            return new StackPanel();
        }

        public void AddChild(XBlock child)
        {
            Children.Add(child);
        }

        protected override void AddChild(FrameworkElement element, UIElement child)
        {
            ((StackPanel)element).Children.Add(child);
        }

        protected override void AddChild(FrameworkElement element, Inline child)
        {
            ((StackPanel)element).Children.Add(new TextBlock(child));
        }

        protected override void AddChild(StringBuilder stringBuilder, XBlock child, bool formatted)
        {
            stringBuilder.AppendLine(formatted ? child.ToString() : child.ToPlainString());
        }

        protected override void AddChild(StringBuilder stringBuilder, XInline child, bool formatted)
        {
            stringBuilder.AppendLine(formatted ? child.ToString() : child.ToPlainString());
        }

        protected override void AddingChild(StringBuilder stringBuilder, XBlock child)
        {
        }

        protected override void AddingChild(StringBuilder stringBuilder, XInline child)
        {
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public override string ToPlainString()
        {
            return ToString(false);
        }

        private string ToString(bool formatted)
        {
            var indent = string.Empty;
            if (BlockStyle == BlockStyle.Indented)
                indent = "  ";

            var stringBuilder = new StringBuilder();
            foreach (var xElement in Children.Where(c => c.ShouldBuildElement()))
            {
                stringBuilder.Append(indent);
                stringBuilder.AppendLine(formatted ? xElement.ToString() : xElement.ToPlainString());
            }

            return indent + stringBuilder.ToString().Trim();
        }
    }
}