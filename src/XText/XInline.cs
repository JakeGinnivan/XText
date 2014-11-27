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
        }

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
            if (ShouldBuildElement())
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
            return ToPlainString();
        }

        public override string ToPlainString()
        {
            return ShouldBuildElement() ? Text : string.Empty;
        }
    }
}