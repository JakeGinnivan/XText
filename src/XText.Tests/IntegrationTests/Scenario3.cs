namespace XText.Tests.IntegrationTests
{
    public class Scenario3 : InlineScenarioBase
    {
        protected override XInline GetInline()
        {
            return new XSpan("Fred Fibnar", new XRun(""), "", "");
        }
    }
}