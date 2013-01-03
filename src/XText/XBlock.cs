using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace XText
{
    public abstract class XBlock : XTextElement
    {
        public BlockStyle BlockStyle { get; set; }
        protected readonly IList<XTextElement> Children;

        protected XBlock(BlockStyle blockStyle = BlockStyle.Normal, params XTextElement[] children)
            : base(null)
        {
            BlockStyle = blockStyle;
            Children = children.ToList();
        }

        protected XBlock(Func<bool> writeIf, BlockStyle blockStyle = BlockStyle.Normal, params XTextElement[] children)
            : base(writeIf)
        {
            Children = children.ToList();
            BlockStyle = blockStyle;
            WriteIf = writeIf;
        }

        public Func<bool> WriteIf { get; private set; }

        protected abstract FrameworkElement BuildElementInternal();

        protected virtual void AddChild(FrameworkElement element, UIElement child) { }
        protected virtual void AddChild(FrameworkElement element, Inline child) { }
        protected virtual void PostChildrenAdded(FrameworkElement element) { }

        public FrameworkElement BuildElement()
        {
            if ((WriteIf != null && WriteIf()) || WriteIf == null)
            {
                var element = BuildElementInternal();

                if (BlockStyle == BlockStyle.Indented)
                {
                    element.Margin = new Thickness(element.Margin.Left + 10, element.Margin.Top, element.Margin.Right,
                                                   element.Margin.Bottom);
                }

                foreach (var child in Children.Where(o => o.ShouldBuildElement()))
                {
                    var block = child as XBlock;
                    if (block != null)
                    {
                        AddChild(element, block.BuildElement());
                    }
                    else
                    {
                        AddChild(element, ((XInline)child).BuildElement());
                    }

                    PostChildrenAdded(element);
                }
                return element;
            }

            return null;
        }
    }
}