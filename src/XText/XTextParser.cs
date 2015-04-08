using System;
using System.Collections.Generic;
using System.Linq;

namespace XText
{
    public static class XTextParser
    {
        public static XTextElement Parse(string str)
        {
            str = str.Replace("\r\n", "\n");

            if (str.IndexOf("\n\n", StringComparison.Ordinal) != -1)
            {
                var isLastAParagraph = str.EndsWith("\n");
                var paragraphs = str.TrimEnd('\n').Split(new[] { "\n\n" }, StringSplitOptions.None);
                var parsed = paragraphs.Select(ParseBlock).ToArray();
                for (var i = 0; i < parsed.Length; i++)
                {
                    var isLast = i == parsed.Length - 1;

                    if (isLast && !isLastAParagraph) continue;
                    if (parsed[i] is XInline) parsed[i] = new XParagraph((XInline) parsed[i]);
                }
                return new XSection(parsed);
            }

            return ParseBlock(str);
        }

        private static XTextElement ParseBlock(string str)
        {
            if (str.StartsWith("  "))
            {
                return new XParagraph(BlockStyle.Indented, str.Substring(2));
            }

            return ParseInlineString(str);
        }

        private static XInline ParseInlineString(string str)
        {
            var inlines = new List<XInline>();
            bool formattedInlineMatch;
            do
            {
                formattedInlineMatch = false;
                const string boldDelimiter = "**";
                const string italicDelimiter = "*";
                var boldStart = str.IndexOf(boldDelimiter, StringComparison.Ordinal);
                var italicStart = str.IndexOf(italicDelimiter, StringComparison.Ordinal);
                var newLineStart = str.IndexOf("\n", StringComparison.Ordinal);

                if (newLineStart != -1 && 
                    (newLineStart < boldStart || boldStart == -1) &&
                    (newLineStart < italicStart || italicStart == -1))
                {
                    if (newLineStart == 0)
                    {
                        inlines.Add(new XLineBreak());
                        str = str.Substring(1);
                    }
                    else
                    {
                        inlines.Add(new XRun(str.Substring(0, newLineStart)));
                        str = str.Substring(newLineStart + 1);
                        inlines.Add(new XLineBreak());
                    }
                }
                else if (boldStart != -1 && boldStart <= italicStart)
                {
                    str = ParseNextInline(str, boldDelimiter, boldStart, inlines, s => new XBold(s));
                    formattedInlineMatch = true;
                }
                else if (italicStart != -1)
                {
                    str = ParseNextInline(str, italicDelimiter, italicStart, inlines, s => new XItalic(s));
                    formattedInlineMatch = true;
                }
            } while (formattedInlineMatch);

            if (!string.IsNullOrEmpty(str))
                inlines.Add(new XRun(str));

            if (inlines.Count == 1)
                return inlines[0];

            return new XSpan(inlines.ToArray());
        }

        private static string ParseNextInline(string str, string delimiter, int start, ICollection<XInline> inlines, Func<string, XInline> factory)
        {
            var delimiterLength = delimiter.Length;
            var delimiterEnd = str.IndexOf(delimiter, start + delimiterLength, StringComparison.Ordinal);
            if (start == 0)
            {
                var substring = str.Substring(start + delimiterLength, delimiterEnd - start - delimiterLength);
                inlines.Add(factory(substring));
                str = str.Substring(delimiterEnd + delimiterLength);
            }
            else
            {
                inlines.Add(new XRun(str.Substring(0, start)));
                var substring = str.Substring(start + delimiterLength, delimiterEnd - start - delimiterLength);
                inlines.Add(factory(substring));
                str = str.Substring(delimiterEnd + delimiterLength);
            }
            return str;
        }
    }
}