using System;
using System.Windows;
using System.Windows.Documents;

namespace XText.Demo
{
    public class MyHighlightedText : XInline
    {
        public MyHighlightedText(string text) : base(text) { }
        public MyHighlightedText(Func<bool> writeIf, string text) : base(writeIf, text) { }

        protected override Inline BuildElementInternal()
        {
            var span = new Span(new Run(Text));
            span.SetResourceReference(FrameworkContentElement.StyleProperty, "HighlightedTextStyle");
            return span;
        }

        public static explicit operator MyHighlightedText(string s)
        {
            return new MyHighlightedText(s);
        }
    }
}