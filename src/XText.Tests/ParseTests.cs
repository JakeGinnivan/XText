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
            Verify(new XRun("Text*"));
            Verify(new XRun("Text*Text"));
            Verify(new XRun(@"Text\*"));
            Verify(new XRun(@"Text\*Text"));
            Verify(new XRun(@"Text\*\*Text"));
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
        public void IndentedParagraph()
        {
            Verify(new XParagraph(BlockStyle.Indented, "Indented Text"));
        }

        [Fact]
        public void NewLine()
        {
            Verify(new XSpan("Multiline", new XLineBreak(), "Text"));
            Verify(new XSpan(new XLineBreak(), "Start with newline"));
        }

        [Fact]
        public void SimpleSpans()
        {
            Verify(new XSpan(new XBold("Bold"), "First"));
            Verify(new XSpan("Some", new XBold("Bold"), "Text", new XBold("!")));
            Verify(new XSpan("Some", new XBold("Bold"), "&", new XItalic("Italic"), "Text", new XItalic("1"), new XBold("!")));
        }

        [Fact]
        public void TwoParagraphsInASection()
        {
            Verify(new XSection(new XParagraph("First paragraph"), "Inline second"));
            Verify(new XSection(new XParagraph("First paragraph"), new XParagraph("Second paragraph")));
        }

        [Fact]
        public void FormattedLines()
        {
            Verify(new XParagraph(new XBold("Line 1"), new XLineBreak(), new XBold("Line 2")));
            Verify(new XParagraph(new XBold("Line 1"), new XLineBreak(), new XBold("Line 2*")));
            Verify(new XParagraph(new XBold("Line 1"), new XLineBreak(), new XBold("Line 2*1234")));
            Verify(new XParagraph(new XBold("Line 1"), new XLineBreak(), new XBold("Line 2\\*1234")));
        }

        [Fact]
        public void EscapeActualStars()
        {
            Verify(new XBold("2 * 3"));
            Verify(new XBold("2 \\* 3"));
        }

        [Fact]
        public void NonMatchedText()
        {
            Verify("2*3", "2\\*3");
            Verify("2*", "2\\*");
            Verify("2**3*4**", "2 **3\\*4**");
            Verify("2**3\\*4**", "2 **3\\*4**");
            Verify("\\***3\\***", "\\* **3\\***");
            const string str = "Para 1\r\n\r\nPara 2: 2\\*3";
            Verify(str, str);
        }

        void Verify(string str, string expected)
        {
            var element = XTextParser.Parse(str);

            var backToString = element.ToString();

            backToString.ShouldBe(expected);
        }

        void Verify(XTextElement element)
        {
            var str = element.ToString();

            var restored = XTextParser.Parse(str);

            restored.ShouldBe(element);
        }
    }
}