using System;
using System.Collections.Generic;
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
        private readonly List<XTextElement> children;

        public XSection(BlockStyle blockStyle, params XTextElement[] children)
            : base(blockStyle)
        {
            this.children = children.ToList();
        }

        public XSection(Func<bool> writeIf, BlockStyle blockStyle, params XTextElement[] children)
            : base(writeIf, blockStyle)
        {
            this.children = children.ToList();
        }

        public XSection(params XTextElement[] children)
            : base(BlockStyle.Normal)
        {
            this.children = children.ToList();
        }

        public XSection(Func<bool> writeIf, params XTextElement[] children)
            : base(writeIf, BlockStyle.Normal)
        {
            this.children = children.ToList();
        }

        protected override FrameworkElement BuildElementInternal()
        {
            var buildElementInternal = new StackPanel();
            foreach (var element in children)
            {
                var inline = element as XInline;
                if (inline != null)
                {
                    var buildElement = inline.BuildElement();
                    if (buildElement != null)
                    {
                        buildElementInternal.Children.Add(new TextBlock(buildElement));
                    }
                }
                else
                {
                    var frameworkElement = ((XBlock)element).BuildElement();
                    if (frameworkElement != null)
                    {
                        buildElementInternal.Children.Add(frameworkElement);
                    }
                }
            }
            return buildElementInternal;
        }

        public void AddChild(XBlock child)
        {
            children.Add(child);
        }
        
        protected override string ToString(bool formatted)
        {
            var stringBuilder = new StringBuilder();
            foreach (var xElement in children.Where(c => c.ShouldBuildElement()))
            {
                var value = formatted ? xElement.ToString() : xElement.ToPlainString();
                if (BlockStyle == BlockStyle.Indented)
                    value = Indent(value);

                stringBuilder.AppendLine(value);
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}