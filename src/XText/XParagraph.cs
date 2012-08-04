using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XText
{
    public class XParagraph : XBlock
    {
        public XParagraph(params XTextElement[] children)
            : base(BlockStyle.Normal, children)
        {
        }

        public XParagraph(Func<bool> writeIf, params XTextElement[] children)
            : base(writeIf, BlockStyle.Normal, children)
        {
        }

        public XParagraph(BlockStyle blockStyle = BlockStyle.Normal, params XTextElement[] children)
            : base(blockStyle, children)
        {
        }

        public XParagraph(Func<bool> writeIf, BlockStyle blockStyle = BlockStyle.Normal, params XTextElement[] children)
            : base(writeIf, blockStyle, children)
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
            ((TextBlock)element).Inlines.Add(child);
        }

        protected override void AddChild(FrameworkElement element, Inline child)
        {
            ((TextBlock)element).Inlines.Add(child);
        }

        protected override void PostChildrenAdded(FrameworkElement element)
        {
            ((TextBlock)element).Inlines.Add(new Run(" "));
        }
    }
}