XText
=====

Allows you to create nicely formatted WPF text from code behind with a syntax similar to the way XDocuments work.

## Example:
    FrameworkElement frameworkElement =
        new XSection(new XSubHeading("Heading"),
                        new XParagraph("This is an example of what you can do with XText. You can",
                                    new XBold("bold"), "text and do all sorts of nice things like:"),
                        new XParagraph(BlockStyle.Indented, "Indent paragraphs, ",
                                    new MyHighlightedText("highlight text"), new XLineBreak(),
                                    new XSection("Nest sections", "Which do not continue on a single line"))).
            BuildElement();

![Example](http://i.imgur.com/m0I64.png)

You can also optionally print out elements:

    bool shouldWrite = true;

    new XParagraph("Some text", ()=>shouldWrite)