using System;
using System.Windows.Data;
using Shouldly;
using Xunit;

namespace XText.Tests
{
    public class ToStringTests
    {
        [Fact]
        public void XBoldToString()
        {
            var bold = new XBold("Text");

            var tostring = bold.ToString();

            Assert.Equal("**Text**", tostring);
        }

        [Fact]
        public void XBoldBindingToString()
        {
            var bold = new XBold(new Binding("Foo"));

            var tostring = bold.ToString();

            Assert.Equal("**{Foo}**", tostring);
        }

        [Fact]
        public void XItalicToString()
        {
            var italic = new XItalic("Text");

            var tostring = italic.ToString();

            Assert.Equal("*Text*", tostring);
        }

        [Fact]
        public void XItalicBindingToString()
        {
            var italic = new XItalic(new Binding("Foo"));

            var tostring = italic.ToString();

            Assert.Equal("*{Foo}*", tostring);
        }

        [Fact]
        public void XLineBreakToString()
        {
            var lineBreak = new XLineBreak();

            var tostring = lineBreak.ToString();

            Assert.Equal(Environment.NewLine, tostring);
        }

        [Fact]
        public void XParagraphToString()
        {
            var paragraph = new XParagraph("Hello", new XLineBreak(), "World");

            var tostring = paragraph.ToString();

            Assert.Equal("Hello\r\nWorld\r\n", tostring);
        }

        [Fact]
        public void XParagraphIndentedToString()
        {
            var paragraph = new XParagraph(BlockStyle.Indented, "Hello", new XLineBreak(), "World");

            var tostring = paragraph.ToString();

            Assert.Equal("  Hello\r\n  World\r\n", tostring);
        }

        [Fact]
        public void XRunToString()
        {
            var run = new XRun("Foo");

            var tostring = run.ToString();

            tostring.ShouldBe("Foo");
        }

        [Fact]
        public void XRunBindingToString()
        {
            var run = new XRun(new Binding("Foo"));

            var tostring = run.ToString();

            tostring.ShouldBe("{Foo}");
        }

        [Fact]
        public void XRunBindingWithStringFormatToString()
        {
            var run = new XRun(new Binding("Foo")
            {
                StringFormat = "This is a {0}!"
            });

            var tostring = run.ToString();

            tostring.ShouldBe("This is a {Foo}!");
        }

        [Fact]
        public void XSectionToString()
        {
            var section = new XSection("Hello", "World");

            var tostring = section.ToString();

            Assert.Equal("Hello\r\nWorld", tostring);
        }

        [Fact]
        public void XSectionWithIndentToString()
        {
            var section = new XSection(BlockStyle.Indented, "Hello", "World");

            var tostring = section.ToString();

            Assert.Equal("  Hello\r\n  World", tostring);
        }

        [Fact]
        public void XSpanToString()
        {
            var span = new XSpan("Hello", "World");

            var tostring = span.ToString();

            Assert.Equal("Hello World", tostring);
        }

        [Fact]
        public void XSpanAfterNewLineBug()
        {
            var toString = new XParagraph(
                "Line 1",
                new XLineBreak(),
                new XSpan(new XRun(() => false, "Foo"), "Hi", "there"))
                .ToString();
            Assert.Equal("Line 1\r\nHi there\r\n", toString);
        }
    }
}