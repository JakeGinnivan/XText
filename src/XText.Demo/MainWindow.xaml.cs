using System.Windows;

namespace XText.Demo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var frameworkElement =
                new XSection(new XSubHeading("Heading"),
                                new XParagraph("This is an example of what you can do with XText. You can",
                                            new XBold("bold"), "text and do all sorts of nice things like:"),
                                new XParagraph(BlockStyle.Indented, "Indent paragraphs, ", new MyHighlightedText("highlight text"),
                                    new XItalic("Italics"))).
                    BuildElement();

            grid.Children.Add(
                frameworkElement);
        }
    }
}