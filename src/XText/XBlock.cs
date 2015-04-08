using System;
using System.Linq;
using System.Windows;

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

        protected bool Equals(XBlock other)
        {
            return BlockStyle == other.BlockStyle;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((XBlock) obj);
        }

        public override int GetHashCode()
        {
            return (int) BlockStyle;
        }

        public static bool operator ==(XBlock left, XBlock right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(XBlock left, XBlock right)
        {
            return !Equals(left, right);
        }
    }
}