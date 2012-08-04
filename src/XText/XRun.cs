using System;
using System.Windows;
using System.Windows.Documents;

namespace XText
{
    public class XRun : XInline
    {
        readonly string style;

        public XRun(string text, string style = null)
            : base(text)
        {
            this.style = style;
        }

        public XRun(Func<bool> writeIf, string text, string style = null)
            : base(writeIf, text)
        {
            this.style = style;
        }

        protected override Inline BuildElementInternal()
        {
            var buildElementInternal = new Run(Text);
            if (style != null)
                buildElementInternal.SetResourceReference(FrameworkContentElement.StyleProperty, style);
            return buildElementInternal;
        }
    }
}