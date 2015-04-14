namespace XText.Tests.IntegrationTests
{
    public class WhiteSpaceScenario : BlockScenarioBase
    {
        protected override XBlock GetBlock()
        {
            return new XSection(
                new XParagraph("Some test ", new XBold(" with bold"), "(", "and stuff", ")"),
                new XParagraph("Multiple lines", ". Next"),
                new XSpan("And", "a", "span"));
        }
    }
}