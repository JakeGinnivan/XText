using System;
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
            return new StackPanel { Margin = new Thickness(0, 5, 0, 5) };
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

        protected override void PostChildrenAdded(FrameworkElement element)
        {
        }

        public override string ToString()
        {
            var indent = string.Empty;
            if (BlockStyle == BlockStyle.Indented)
                indent = "  ";

            var stringBuilder = new StringBuilder();
            foreach (var ccWinElement in Children)
            {
                stringBuilder.Append(indent);
                stringBuilder.AppendLine(ccWinElement.ToString());
            }

            return indent + stringBuilder.ToString().Trim();
        }
    }
}