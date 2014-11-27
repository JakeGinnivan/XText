using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace XText
{
    public abstract class XBlock : XTextElement
    {
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
        }

        public BlockStyle BlockStyle { get; set; }

        protected abstract FrameworkElement BuildElementInternal();

        protected virtual void AddChild(FrameworkElement element, UIElement child) { }
        protected virtual void AddChild(FrameworkElement element, Inline child) { }
        protected virtual void AddingChild(FrameworkElement element, UIElement child) { }
        protected virtual void AddingChild(FrameworkElement element, Inline child) { }

        protected abstract void AddChild(StringBuilder stringBuilder, XBlock child, bool formatted);
        protected abstract void AddChild(StringBuilder stringBuilder, XInline child, bool formatted);
        protected abstract void AddingChild(StringBuilder stringBuilder, XBlock child);
        protected abstract void AddingChild(StringBuilder stringBuilder, XInline child);

        public FrameworkElement BuildElement()
        {
            if (ShouldBuildElement())
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
                        var frameworkElement = block.BuildElement();
                        AddingChild(element, frameworkElement);
                        AddChild(element, frameworkElement);
                    }
                    else
                    {
                        var buildElement = ((XInline)child).BuildElement();
                        AddingChild(element, buildElement);
                        AddChild(element, buildElement);
                    }

                }
                return element;
            }

            return null;
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
            if (ShouldBuildElement())
            {
                var stringBuilder = new StringBuilder();

                foreach (var child in Children.Where(o => o.ShouldBuildElement()))
                {
                    var block = child as XBlock;

                    if (block != null)
                    {
                        AddingChild(stringBuilder, block);
                        AddChild(stringBuilder, block, formatted);
                    }
                    else
                    {
                        AddingChild(stringBuilder, (XInline) child);
                        AddChild(stringBuilder, (XInline) child, formatted);
                    }
                }

                return stringBuilder.ToString();
            }

            return string.Empty;
        }
    }
}