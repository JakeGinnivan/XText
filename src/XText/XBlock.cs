using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace XText
{
    public abstract class XBlock : XTextElement
    {

        protected XBlock(BlockStyle blockStyle = BlockStyle.Normal)
            : base(null)
        {
            BlockStyle = blockStyle;
        }

        protected XBlock(Func<bool> writeIf, BlockStyle blockStyle = BlockStyle.Normal)
            : base(writeIf)
        {
            BlockStyle = blockStyle;
        }

        public BlockStyle BlockStyle { get; set; }

        protected abstract FrameworkElement BuildElementInternal();

        public FrameworkElement BuildElement()
        {
            if (ShouldBuildElement())
            {
                var element = BuildElementInternal();

                if (BlockStyle == BlockStyle.Indented)
                {
                    element.Margin = new Thickness(element.Margin.Left + 10, element.Margin.Top, element.Margin.Right,
                                                   element.Margin.Bottom);
                }

                return element;
            }

            return null;
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public override string ToPlainString()
        {
            return ToString(false);
        }

        protected abstract string ToString(bool formatted);

        protected string Indent(string stringToIndent)
        {
            var indentedLines = stringToIndent.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Select(s => "  " + s);
            return string.Join(Environment.NewLine, indentedLines);
        }
    }
}