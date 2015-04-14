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

        protected override Block BuildDocumentInternal()
        {
            return new Paragraph(new XSpan(children.ToArray()).BuildElement());
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

        protected bool Equals(XParagraph other)
        {
            return base.Equals(other) && ListEquals(children, other.children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XParagraph) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (children != null ? children.GetHashCode() : 0);
            }
        }

        public static bool operator ==(XParagraph left, XParagraph right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(XParagraph left, XParagraph right)
        {
            return !Equals(left, right);
        }
    }
}