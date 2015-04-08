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
                var boldStart = str.IndexOf("**", StringComparison.Ordinal);
                var italicStart = str.IndexOf("*", StringComparison.Ordinal);
                if (boldStart != -1 && boldStart <= italicStart)
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
                } else if (italicStart != -1)
                {
                    var italicEnd = str.IndexOf("*", italicStart + 1, StringComparison.Ordinal);
                    if (italicStart == 0)
                    {
                        var substring = str.Substring(italicStart + 1, italicEnd - italicStart - 1);
                        inlines.Add(new XItalic(substring));
                        str = str.Substring(italicEnd + 1);
                    }
                    else
                    {
                        inlines.Add(new XRun(str.Substring(0, italicStart)));
                        var substring = str.Substring(italicStart + 1, italicEnd - italicStart - 1);
                        inlines.Add(new XItalic(substring));
                        str = str.Substring(italicEnd + 1);
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