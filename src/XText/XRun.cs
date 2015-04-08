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
            : base(() => text)
        {
            this.style = style ?? DefaultStye;
        }

        public XRun(Func<bool> writeIf, string text, string style = null)
            : base(writeIf, () => text)
        {
            this.style = style ?? DefaultStye;
        }

        public XRun(Func<string> text, string style = null)
            : base(text)
        {
            this.style = style ?? DefaultStye;
        }

        public XRun(Func<bool> writeIf, Func<string> text, string style = null)
            : base(writeIf, text)
        {
            this.style = style ?? DefaultStye;
        }

        // Doing this is a breaking change
        // ReSharper disable once IntroduceOptionalParameters.Global
        public XRun(Binding textBinding)
            : this(textBinding, null)
        {
        }

        public XRun(Binding textBinding, string style)
            : base(() => string.Empty)
        {
            this.textBinding = textBinding;
            this.style = style ?? DefaultStye;
        }

        public static string DefaultStye { get; set; }

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

        protected bool Equals(XRun other)
        {
            return 
                base.Equals(other) && 
                Equals(textBinding != null ? textBinding.Path : null, other.textBinding != null ? other.textBinding.Path : null) && 
                string.Equals(style, other.style);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XRun) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (textBinding != null ? textBinding.Path.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (style != null ? style.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(XRun left, XRun right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(XRun left, XRun right)
        {
            return !Equals(left, right);
        }
    }
}