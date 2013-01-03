using System;
using System.Windows.Documents;

namespace XText
{
    public class XBold : XInline
    {
        public XBold(string text) : base(text) { }
        public XBold(Func<bool> writeIf, string text) : base(writeIf, text) { }

        protected override Inline BuildElementInternal()
        {
            return new Bold(new Run(Text));
        }

        public static explicit operator XBold(string s)
        {
            return new XBold(s);
        }

        public override string ToString()
        {
            return "**" + base.ToString() + "**";
        }
    }
}