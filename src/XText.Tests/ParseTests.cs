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
        public void SimpleSpans()
        {
            Verify(new XSpan(new XBold("Bold"), "First"));
            Verify(new XSpan("Some", new XBold("Bold"), "Text", new XBold("!")));
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
                var boldStart = str.IndexOf("**", StringComparison.Ordinal);
                if (boldStart != -1)
                {
                    var boldEnd = str.IndexOf("**", boldStart + 2, StringComparison.Ordinal);
                    if (boldStart == 0)
                    {
                        var substring = str.Substring(boldStart + 2, boldEnd - boldStart - 2);
                        inlines.Add(new XBold(substring));
                        str = str.Substring(boldEnd + 2);
                    }
                    else
                    {
                        inlines.Add(new XRun(str.Substring(0, boldStart)));
                        var substring = str.Substring(boldStart + 2, boldEnd - boldStart - 2);
                        inlines.Add(new XBold(substring));
                        str = str.Substring(boldEnd + 2);
                    }
                    formattedInlineMatch = true;
                }
            } while (formattedInlineMatch); 

            if (!string.IsNullOrEmpty(str))
                inlines.Add(new XRun(str));

            if (inlines.Count == 1)
                return inlines[0];

            return new XSpan(inlines.ToArray());
        }
    }
}