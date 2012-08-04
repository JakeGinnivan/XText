using System;

namespace XText
{
    public abstract class XElement
    {
        readonly Func<bool> shouldBuildElement;

        protected XElement(Func<bool> shouldBuildElement)
        {
            this.shouldBuildElement = shouldBuildElement;
        }

        public bool ShouldBuildElement()
        {
            return ((shouldBuildElement != null && shouldBuildElement()) || shouldBuildElement == null);
        }

        public static implicit operator XElement(string s)
        {
            return new XRun(s);
        }
    }
}