using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            var xTextElements = children.Where(c => c.ShouldBuildElement()).ToList();
            for (var index = 0; index < xTextElements.Count; index++)
            {
                var xElement = xTextElements[index];
                var value = formatted ? xElement.ToString() : xElement.ToPlainString();
                if (BlockStyle == BlockStyle.Indented)
                    value = Indent(value);

                var isLastElement = index == xTextElements.Count - 1;
                if (isLastElement)
                    stringBuilder.Append(value);
                else
                    stringBuilder.AppendLine(value);
            }

            return stringBuilder.ToString();
        }

        protected bool Equals(XSection other)
        {
            return base.Equals(other) && ListEquals(children, other.children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XSection) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (children != null ? children.GetHashCode() : 0);
            }
        }

        public static bool operator ==(XSection left, XSection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(XSection left, XSection right)
        {
            return !Equals(left, right);
        }
    }
}