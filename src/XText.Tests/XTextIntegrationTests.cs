using System.Windows.Controls;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using Xunit;

namespace XText.Tests
{
    public class XTextIntegrationTests
    {
        public class Scenario1
        {
            private readonly XSection section;

            public Scenario1()
            {
                section = new XSection(
                    new XParagraph("Some test", new XBold("with bold"), "and stuff"),
                    new XParagraph("Multiple lines", new XLineBreak(), "Next"));
            }

            [Fact]
            [UseReporter(typeof (KDiffReporter))]
            public void StringRepresentation()
            {
                Approvals.Verify(section.ToString());
            }

            [Fact]
            [UseReporter(typeof (TortoiseImageDiffReporter2), typeof(ClipboardReporter))]
            public void ControlRepresentation()
            {
                WpfApprovals.Verify(new ContentControl
                    {
                        Content = section.BuildElement()
                    });
            }
        }

        public class Scenario2
        {
            private readonly XSection section;

            public Scenario2()
            {
                section = new XSection(
                    new XParagraph("Some test", new XBold(" with bold "), "and stuff"),
                    new XParagraph("Multiple lines", new XLineBreak(), "Next"));
            }

            [Fact]
            [UseReporter(typeof(KDiffReporter))]
            public void StringRepresentation()
            {
                Approvals.Verify(section.ToString());
            }

            [Fact]
            [UseReporter(typeof(TortoiseImageDiffReporter2), typeof(ClipboardReporter))]
            public void ControlRepresentation()
            {
                WpfApprovals.Verify(new ContentControl
                {
                    Content = section.BuildElement()
                });
            }
        }

        public class WhiteSpaceScenario
        {
            private readonly XSection section;

            public WhiteSpaceScenario()
            {
                section = new XSection(
                    new XParagraph("Some test ", new XBold(" with bold"), "(", "and stuff", ")"),
                    new XParagraph("Multiple lines", ". Next"),
                    new XSpan("And", "a", "span"));
            }

            [Fact]
            [UseReporter(typeof(KDiffReporter))]
            public void StringRepresentation()
            {
                Approvals.Verify(section.ToString());
            }

            [Fact]
            [UseReporter(typeof(TortoiseImageDiffReporter2), typeof(ClipboardReporter))]
            public void ControlRepresentation()
            {
                WpfApprovals.Verify(new ContentControl
                {
                    Content = section.BuildElement()
                });
            }
        }
    }
}