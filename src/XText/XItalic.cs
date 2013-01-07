using System;
using System.Windows.Documents;

namespace XText
{
    public class XItalic : XInline
    {
        public XItalic(string text) : base(text) { }
        public XItalic(Func<bool> writeIf, string text) : base(writeIf, text) { }

        protected override Inline BuildElementInternal()
        {
            return new Italic(new Run(Text));
        }

        public static explicit operator XItalic(string s)
        {
            return new XItalic(s);
        }

        public override string ToString()
        {
            return ShouldBuildElement() ? "*" + base.ToString() + "*" : string.Empty;
        }
    }
}