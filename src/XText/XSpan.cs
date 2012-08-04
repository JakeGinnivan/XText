using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace XText
{
    public class XSpan : XInline
    {
        readonly IList<XInline> children = new List<XInline>();

        public XSpan(params XInline[] children)
            : base(null)
        {
            foreach (var inlineFormattedText in children)
            {
                this.children.Add(inlineFormattedText);
            }
        }

        public XSpan(Func<bool> writeIf, params XInline[] children)
            : base(writeIf, null)
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
    }
}