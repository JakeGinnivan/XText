using System;
using System.Collections.Generic;
using System.Linq;

namespace XText
{
    public abstract class XTextElement
    {
        readonly Func<bool> shouldBuildElement;

        protected XTextElement(Func<bool> shouldBuildElement)
        {
            this.shouldBuildElement = shouldBuildElement ?? (() => true);
        }

        public bool ShouldBuildElement()
        {
            return ((shouldBuildElement != null && shouldBuildElement()) || shouldBuildElement == null);
        }

        public static implicit operator XTextElement(string s)
        {
            return new XRun(s);
        }

        public abstract string ToPlainString();

        protected bool ListEquals<T>(IList<T> l1, IList<T> l2) where T : XTextElement
        {
            if (Equals(l1, l2))
                return true;

            return l1.SequenceEqual(l2);
        }
    }
}