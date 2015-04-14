using System;
using System.Windows.Controls;
using System.Windows.Documents;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using ApprovalUtilities.Wpf;
using Xunit;

namespace XText.Tests.IntegrationTests
{
    public abstract class BlockScenarioBase
    {
        private static readonly Func<IDisposable> AddAdditionalInfo = ApprovalResults.UniqueForOs;
        protected abstract XBlock GetBlock();

        [Fact]
        [UseReporter(typeof (KDiffReporter))]
        public void StringRepresentation()
        {
            Approvals.Verify(
                new ApprovalTextWriter(GetBlock().ToString()),
                new DerivedClassUnitTestFrameworkNamer("BlockScenarioBase", this),
                Approvals.GetReporter());
        }

        [Fact]
        [UseReporter(typeof (TortoiseImageDiffReporter2), typeof(ClipboardReporter))]
        public void ControlRepresentation()
        {
            var control = new ContentControl
            {
                Content = GetBlock().BuildElement()
            };
            WpfApprove(control);
        }

        [Fact]
        [UseReporter(typeof (TortoiseImageDiffReporter2), typeof(ClipboardReporter))]
        public void FlowDocRepresentation()
        {
            var control = new ContentControl
            {
                Content = new FlowDocumentScrollViewer
                {
                    Document = new FlowDocument(GetBlock().BuildDocument())
                }
            };
            WpfApprove(control);
        }

        private void WpfApprove(ContentControl control)
        {
            using (AddAdditionalInfo())
                Approvals.Verify(new ImageWriter(f => WpfUtils.ScreenCapture(control, f)),
                    new DerivedClassUnitTestFrameworkNamer("BlockScenarioBase", this), Approvals.GetReporter());
        }
    }
}