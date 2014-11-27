using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace XText
{
    /// <summary>
    /// Useful for grouping a set of inlines and optionally writing them out.
    /// </summary>
    public class XSpan : XInline
    {
        readonly IList<XInline> children = new List<XInline>();

        public XSpan(params XInline[] children)
            : base(() => null)
        {
            foreach (var inlineFormattedText in children)
            {
                this.children.Add(inlineFormattedText);
            }
        }

        public XSpan(Func<bool> writeIf, params XInline[] children)
            : base(writeIf, () => null)
        {
            foreach (var inlineFormattedText in children)
            {
                this.children.Add(inlineFormattedText);
            }
        }

        public void AddChild(XInline child)
        {
            children.Add(child);
        }

        protected override Inline BuildElementInternal()
        {
            var element = new Span();
            foreach (var inlineFormattedText in children)
            {
                var innerElement = inlineFormattedText.BuildElement();
                if (innerElement != null)
                    element.Inlines.Add(innerElement);
            }
            return element;
        }

        public override string ToString()
        {
            return string.Join(" ", children.Where(c => c.ShouldBuildElement()).Select(c => c.ToString()));
        }

        public override string ToPlainString()
        {
            return string.Join(" ", children.Where(c => c.ShouldBuildElement()).Select(c => c.ToString()));
        }
    }
}