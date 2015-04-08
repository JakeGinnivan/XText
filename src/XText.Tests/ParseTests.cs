using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace XText.Tests
{
    public class ParseTests
    {
        [Fact]
        public void Run()
        {
            Verify(new XRun("Text"));
        }

        [Fact]
        public void Bold()
        {
            Verify(new XBold("Text"));
        }

        [Fact]
        public void Italic()
        {
            Verify(new XItalic("Text"));
        }

        [Fact]
        public void SimpleSpans()
        {
            Verify(new XSpan(new XBold("Bold"), "First"));
            Verify(new XSpan("Some", new XBold("Bold"), "Text", new XBold("!")));
            Verify(new XSpan("Some", new XBold("Bold"), "&", new XItalic("Italic"), "Text", new XItalic("1"), new XBold("!")));
        }

        void Verify(XTextElement element)
        {
            var str = element.ToString();

            var restored = XTextParser.Parse(str);

            restored.ShouldBe(element);
        }
    }

    public class XTextParser
    {
        public static XTextElement Parse(string str)
        {
            var inlines = new List<XInline>();

            bool formattedInlineMatch;
            do
            {
                formattedInlineMatch = false;
                var boldDelimiter = "**";
                var italicDelimiter = "*";
                var boldStart = str.IndexOf(boldDelimiter, StringComparison.Ordinal);
                var italicStart = str.IndexOf(italicDelimiter, StringComparison.Ordinal);
                
                if (boldStart != -1 && boldStart <= italicStart)
                {
                    str = ParseNextInline(str, boldDelimiter, boldStart, inlines, s => new XBold(s));
                    formattedInlineMatch = true;
                } else if (italicStart != -1)
                {
                    str = ParseNextInline(str, italicDelimiter, italicStart, inlines, s => new XItalic(s));
                    formattedInlineMatch = true;
                }
            } while (formattedInlineMatch); 

            if (!string.IsNullOrEmpty(str))
                inlines.Add(new XRun(str));

            if (inlines.Count == 1)
                return inlines[0];

            return new XSpan(inlines.ToArray());
        }

        private static string ParseNextInline(string str, string delimiter, int start, ICollection<XInline> inlines, Func<string, XInline> factory)
        {
            var delimiterLength = delimiter.Length;
            var delimiterEnd = str.IndexOf(delimiter, start + delimiterLength, StringComparison.Ordinal);
            if (start == 0)
            {
                var substring = str.Substring(start + delimiterLength, delimiterEnd - start - delimiterLength);
                inlines.Add(factory(substring));
                str = str.Substring(delimiterEnd + delimiterLength);
            }
            else
            {
                inlines.Add(new XRun(str.Substring(0, start)));
                var substring = str.Substring(start + delimiterLength, delimiterEnd - start - delimiterLength);
                inlines.Add(factory(substring));
                str = str.Substring(delimiterEnd + delimiterLength);
            }
            return str;
        }
    }
}