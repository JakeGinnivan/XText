using System;

namespace XText
{
    public abstract class XTextElement
    {
        readonly Func<bool> shouldBuildElement;

        protected XTextElement(Func<bool> shouldBuildElement)
        {
            this.shouldBuildElement = shouldBuildElement;
        }

        public bool ShouldBuildElement()
        {
            return ((shouldBuildElement != null && shouldBuildElement()) || shouldBuildElement == null);
        }

        public static implicit operator XTextElement(string s)
        {
            return new XSpan(s);
        }
    }
}