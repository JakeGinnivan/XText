using System;
using System.Windows.Data;
using System.Windows.Documents;

namespace XText
{
    public class XItalic : XInline
    {
        private readonly Binding textBinding;
        public XItalic(string text) : base(() => text) { }
        public XItalic(Func<bool> writeIf, string text) : base(writeIf, () => text) { }
        public XItalic(Func<string> text) : base(text) { }
        public XItalic(Func<bool> writeIf, Func<string> text) : base(writeIf, text) { }

        public XItalic(Binding textBinding) : base(() => string.Empty)
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
            if (!ShouldBuildElement()) return string.Empty;
            
            if (textBinding != null)
                return "*" + string.Format(textBinding.StringFormat ?? "{0}", string.Concat("{", textBinding.Path.Path, "}")) + "*";

            return "*" + base.ToString() + "*";
        }

        protected bool Equals(XItalic other)
        {
            return
                base.Equals(other) &&
                Equals(textBinding != null ? textBinding.Path : null, other.textBinding != null ? other.textBinding.Path : null);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XItalic) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (textBinding != null ? textBinding.GetHashCode() : 0);
            }
        }

        public static bool operator ==(XItalic left, XItalic right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(XItalic left, XItalic right)
        {
            return !Equals(left, right);
        }
    }
}