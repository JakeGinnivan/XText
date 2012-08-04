using System;
using System.Windows.Documents;

namespace XText
{
    public abstract class XInline : XTextElement
    {
        protected XInline(string text)
            : base(null)
        {
            Text = text;
        }

        protected XInline(Func<bool> writeIf, string text)
            : base(writeIf)
        {
            Text = text;
            WriteIf = writeIf;
        }

        public Func<bool> WriteIf { get; private set; }
        public string Text { get; private set; }

        protected abstract Inline BuildElementInternal();

        public Inline BuildElement()
        {
            if ((WriteIf != null && WriteIf()) || WriteIf == null)
            {
                return BuildElementInternal();
            }
            return null;
        }

        public static implicit operator XInline(string s)
        {
            return new XRun(s);
        }
    }
}