using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;

namespace XText.Tests
{
    public class TortoiseImageDiffReporter2 : GenericDiffReporter
    {
        private const string Path = @"C:\tools\tortoisemerge\TortoiseIDiff.exe";

        public TortoiseImageDiffReporter2()
            : base(Path, "/left:\"{0}\" /right:\"{1}\"",
                   "Could not find TortoiseMerge at {0}, please install it (it's part of TortoiseSVN) http://tortoisesvn.net/ "
                       .FormatWith(Path), GetImageFileTypes)
        { }
    }
}