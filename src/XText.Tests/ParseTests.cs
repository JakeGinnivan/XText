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
            var parts = new List<XTextElement>();

            var boldStart = str.IndexOf("**", StringComparison.Ordinal);
            if (boldStart != -1)
            {
                var boldEnd = str.IndexOf("**", boldStart + 2, StringComparison.Ordinal);
                if (boldStart == 0)
                {
                    var substring = str.Substring(boldStart + 2, boldEnd - boldStart - 2);
                    parts.Add(new XBold(substring));
                }
            }

            if (parts.Count == 1)
                return parts[0];

            return new XRun(str);
        }
    }
}