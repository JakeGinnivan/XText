using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Namers.StackTraceParsers;

namespace XText.Tests.IntegrationTests
{
    public class DerivedClassUnitTestFrameworkNamer : UnitTestFrameworkNamer
    {
        private readonly string baseName;
        private readonly object instance;

        public DerivedClassUnitTestFrameworkNamer(string baseName, object instance)
        {
            this.baseName = baseName;
            this.instance = instance;
        }

        public override string Name
        {
            get
            {
                var traceParser = new StackTraceParser();
                traceParser.Parse(Approvals.CurrentCaller.StackTrace);
                return base.Name.Replace(baseName, instance.GetType().Name);
            }
        }
    }
}