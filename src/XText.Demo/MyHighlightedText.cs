using System;
using System.Windows;
using System.Windows.Documents;

namespace XText
{
    public class XHighlighted : XInline
    {
        public XHighlighted(string text) : base(text) { }
        public XHighlighted(Func<bool> writeIf, string text) : base(writeIf, text) { }

        protected override Inline BuildElementInternal()
        {
            var span = new Span(new Run(Text));
            span.SetResourceReference(FrameworkContentElement.StyleProperty, "HighlightedTextStyle");
            return span;
        }

        public static explicit operator XHighlighted(string s)
        {
            return new XHighlighted(s);
        }
    }
}