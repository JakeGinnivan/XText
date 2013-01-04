using System;
using System.Windows.Documents;

namespace XText
{
    public abstract class XInline : XTextElement
    {
        private string text;

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

        public string Text
        {
            get { return text; }
            private set
            {
                text = value == null ? null : value.Trim();
            }
        }

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

        public override string ToString()
        {
            return ShouldBuildElement() ? Text : string.Empty;
        }
    }
}