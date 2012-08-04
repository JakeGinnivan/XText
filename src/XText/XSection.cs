using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XText
{
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
    }
}