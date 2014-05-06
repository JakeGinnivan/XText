using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace XText
{
    public class XRun : XInline
    {
        readonly Binding textBinding;
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

        public XRun(Binding textBinding) : base(string.Empty)
        {
            this.textBinding = textBinding;
        }

        protected override Inline BuildElementInternal()
        {
            Run buildElementInternal;
            if (textBinding != null)
            {
                buildElementInternal = new Run();
                buildElementInternal.SetBinding(Run.TextProperty, textBinding);
            }
            else
                buildElementInternal = new Run(Text);
            if (style != null)
                buildElementInternal.SetResourceReference(FrameworkContentElement.StyleProperty, style);
            return buildElementInternal;
        }

        public override string ToString()
        {
            if (textBinding != null)
                return string.Format(textBinding.StringFormat ?? "{0}", string.Concat("{", textBinding.Path.Path, "}"));

            return base.ToString();
        }
    }
}