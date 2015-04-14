namespace XText.Tests.IntegrationTests
{
    public class Scenario1 : BlockScenarioBase
    {
        protected override XBlock GetBlock()
        {
            return new XSection(
                new XParagraph("Some test", new XBold(" with bold "), "and stuff"),
                new XParagraph("Multiple lines", new XLineBreak(), "Next"));
        }
    }
}