using System;
using System.Windows.Data;
using System.Windows.Documents;

namespace XText
{
    public class XBold : XInline
    {
        readonly Binding textBinding;

        public XBold(string text) : base(() => text) { }
        public XBold(Func<bool> writeIf, string text) : base(writeIf, () => text) { }

        public XBold(Func<string> text) : base(text) { }
        public XBold(Func<bool> writeIf, Func<string> text) : base(writeIf, text) { }

        public XBold(Binding textBinding) : base(() => null)
        {
            this.textBinding = textBinding;
        }

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
            if (!ShouldBuildElement()) return string.Empty;
            if (textBinding != null)
                return "**" + string.Format(textBinding.StringFormat ?? "{0}", string.Concat("{", textBinding.Path.Path, "}")) + "**";

            return "**" + base.ToString() + "**";
        }
    }
}