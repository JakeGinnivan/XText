using System;
using System.Windows.Documents;

namespace XText
{
    public abstract class XInline : XTextElement
    {
        private readonly Func<string> text;
        
        protected XInline(Func<string> text)
            : base(null)
        {
            this.text = text;
        }

        protected XInline(Func<bool> writeIf, Func<string> text)
            : base(writeIf)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                try
                {
                    var t = text();
                    return t == null ? null : t.Trim();
                }
                catch (NullReferenceException)
                {
                    return null;
                }
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
            return ToPlainString().Replace("*", @"\*");
        }

        public override string ToPlainString()
        {
            return ShouldBuildElement() ? Text : string.Empty;
        }

        protected bool Equals(XInline other)
        {
            return Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XInline) obj);
        }

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
        }

        public static bool operator ==(XInline left, XInline right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(XInline left, XInline right)
        {
            return !Equals(left, right);
        }
    }
}