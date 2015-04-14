using System;
using System.Windows.Controls;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using ApprovalUtilities.Wpf;
using Xunit;

namespace XText.Tests.IntegrationTests
{
    public abstract class InlineScenarioBase
    {
        private static readonly Func<IDisposable> AddAdditionalInfo = ApprovalResults.UniqueForOs;
        protected abstract XInline GetInline();

        [Fact]
        [UseReporter(typeof(KDiffReporter))]
        public void StringRepresentation()
        {
            Approvals.Verify(
                new ApprovalTextWriter(GetInline().ToString()),
                new DerivedClassUnitTestFrameworkNamer("InlineScenarioBase", this),
                Approvals.GetReporter());
        }

        [Fact]
        [UseReporter(typeof(TortoiseImageDiffReporter2), typeof(ClipboardReporter))]
        public void ControlRepresentation()
        {
            var control = new ContentControl
            {
                Content = GetInline().BuildElement()
            };
            WpfApprove(control);
        }

        private void WpfApprove(ContentControl control)
        {
            using (AddAdditionalInfo())
                Approvals.Verify(new ImageWriter(f => WpfUtils.ScreenCapture(control, f)),
                    new DerivedClassUnitTestFrameworkNamer("InlineScenarioBase", this), Approvals.GetReporter());
        }
    }
}