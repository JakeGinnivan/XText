using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace XText
{
    /// <summary>
    /// Useful for grouping a set of inlines and optionally writing them out.
    /// </summary>
    public class XSpan : XInline
    {
        readonly IList<XInline> children = new List<XInline>();

        public XSpan(params XInline[] children)
            : base(() => null)
        {
            foreach (var inlineFormattedText in children)
            {
                this.children.Add(inlineFormattedText);
            }
        }

        public XSpan(Func<bool> writeIf, params XInline[] children)
            : base(writeIf, () => null)
        {
            foreach (var inlineFormattedText in children)
            {
                this.children.Add(inlineFormattedText);
            }
        }

        protected override Inline BuildElementInternal()
        {
            var element = new Span();
            foreach (var child in children.Where(o => o.ShouldBuildElement()))
            {
                var buildElement = child.BuildElement();
                AddingChild(element, buildElement);
                AddChild(element, buildElement);
            }
            return element;
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public override string ToPlainString()
        {
            return ToString(false);
        }

        private string ToString(bool formatted)
        {
            if (ShouldBuildElement())
            {
                var stringBuilder = new StringBuilder();

                foreach (var child in children.Where(o => o.ShouldBuildElement()))
                {
                    AddingChild(stringBuilder, child);
                    AddChild(stringBuilder, child, formatted);
                }

                return stringBuilder.ToString();
            }

            return string.Empty;
        }

        private void AddChild(Span element, Inline child)
        {
            element.Inlines.Add(child);
        }

        private void AddingChild(Span element, Inline child)
        {
            if (FormattingCalculator.RequiresSpace(element.Inlines.LastInline, child))
                element.Inlines.Add(new Run(" "));
        }

        private void AddingChild(StringBuilder stringBuilder, XInline child)
        {
            var lastCharacter = stringBuilder.Length > 0 ? stringBuilder[stringBuilder.Length - 1] : '\0';

            if (child is XLineBreak)
                return;

            if (FormattingCalculator.RequiresSpace(lastCharacter, (child.Text ?? string.Empty).FirstOrDefault()))
                stringBuilder.Append(" ");
        }

        private void AddChild(StringBuilder stringBuilder, XInline child, bool formatted)
        {
            stringBuilder.Append(formatted ? child.ToString() : child.ToPlainString());
        }
    }
}