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
    
## Available Classes
XSection - Top level structure, each child will be on a new line (**StackPanel**)  
XParagraph - Takes a collection of XInlines to format as a paragraph (**TextBlock**)  
XSpan - Takes a collection of inlines, and is an inline itself (useful for optionally writing out a sentance in a paragraph) (**Span**)  
XRun - Inline text, used for implicit conversion between string and XInline (**Run**)  
XBold - Bold Run  
XItalic - Italic Run  
XLineBreak - Flow doc line break  

### Base classes
XTextElement - The base class representing all possible elements  
XInline - Base class representing an inline or flowdoc element  
XBlock - Base class for representing a WPF Control, blocks support indentation