using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace XText
{
    /// <summary>
    /// Represents a paragraph of text
    /// 
    /// Paragraph is a block element
    /// </summary>
    public class XParagraph : XBlock
    {
        private readonly List<XInline> children;

        public XParagraph(params XInline[] children)
            : base(BlockStyle.Normal)
        {
            this.children = children.ToList();
        }

        public XParagraph(Func<bool> writeIf, params XInline[] children)
            : base(writeIf, BlockStyle.Normal)
        {
            this.children = children.ToList();
        }

        public XParagraph(BlockStyle blockStyle = BlockStyle.Normal, params XInline[] children)
            : base(blockStyle)
        {
            this.children = children.ToList();
        }

        public XParagraph(Func<bool> writeIf, BlockStyle blockStyle = BlockStyle.Normal, params XInline[] children)
            : base(writeIf, blockStyle)
        {
            this.children = children.ToList();
        }

        public void AddChild(XInline child)
        {
            children.Add(child);
        }

        protected override FrameworkElement BuildElementInternal()
        {
            var buildElementInternal = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap, 
                Margin = new Thickness(0, 0, 0, 5)
            };
            var buildElement = new XSpan(children.ToArray()).BuildElement();
            buildElementInternal.Inlines.Add(buildElement);
            return buildElementInternal;
        }

        protected override string ToString(bool formatted)
        {
            if (!ShouldBuildElement())
                return string.Empty;

            var span = new XSpan(children.ToArray());
            var spanToString = formatted ? span.ToString() : span.ToPlainString();
            if (BlockStyle == BlockStyle.Indented)
            {
                spanToString = Indent(spanToString);
            }

            return spanToString + Environment.NewLine;
        }
    }
}