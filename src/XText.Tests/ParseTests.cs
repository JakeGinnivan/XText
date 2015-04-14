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
        public void EscapeActualStars()
        {

            Verify(new XBold("2 * 3"));
        }

        void Verify(XTextElement element)
        {
            var str = element.ToString();

            var restored = XTextParser.Parse(str);

            restored.ShouldBe(element);
        }
    }
}