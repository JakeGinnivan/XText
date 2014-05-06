using System;
using System.Windows.Data;
using System.Windows.Documents;

namespace XText
{
    public class XItalic : XInline
    {
        private readonly Binding textBinding;
        public XItalic(string text) : base(text) { }
        public XItalic(Func<bool> writeIf, string text) : base(writeIf, text) { }

        public XItalic(Binding textBinding) : base(string.Empty)
        {
            this.textBinding = textBinding;
        }

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
            if (textBinding != null)
                return "*" + string.Format(textBinding.StringFormat ?? "{0}", string.Concat("{", textBinding.Path.Path, "}")) + "*";

            return "*" + base.ToString() + "*";
        }
    }
}