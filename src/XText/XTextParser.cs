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

            if (str.IndexOf("\n\n", StringComparison.Ordinal) != -1 || str.EndsWith("\n"))
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

                if (paragraphs.Length == 1)
                    return parsed.Single();
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
            int searchStartIndex = 0;
            do
            {
                formattedInlineMatch = false;
                const string boldDelimiter = "**";
                const string italicDelimiter = "*";
                var boldStart = str.IndexOf(boldDelimiter, searchStartIndex, StringComparison.Ordinal);
                var italicStart = str.IndexOf(italicDelimiter, searchStartIndex, StringComparison.Ordinal);
                var newLineStart = str.IndexOf("\n", searchStartIndex, StringComparison.Ordinal);
                searchStartIndex = 0;

                // Fix escaped characters
                while (italicStart > 0 && str[italicStart - 1] == '\\')
                {
                    // Make sure backslash is not escaped
                    if (italicStart > 1 && str[italicStart - 2] == '\\')
                    {
                        // We are good to fix italicStart
                        var startIndex = italicStart + 1;
                        str = str.Remove(italicStart - 1, 1);
                        if (startIndex >= str.Length)
                        {
                            italicStart = -1;
                            break;
                        }
                        // Remove escape character for *
                        italicStart = str.IndexOf(italicDelimiter, startIndex, StringComparison.Ordinal);
                    }
                    else
                    {
                        str = str.Remove(italicStart - 1, 1);
                        boldStart = str.IndexOf(boldDelimiter, italicStart, StringComparison.Ordinal);
                        newLineStart = str.IndexOf("\n", italicStart, StringComparison.Ordinal);
                        italicStart = str.IndexOf(italicDelimiter, italicStart, StringComparison.Ordinal);
                        break;
                    }
                }

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
                    formattedInlineMatch = true;
                }
                else if (boldStart != -1 && boldStart <= italicStart)
                {
                    try
                    {
                        str = ParseNextInline(str, boldDelimiter, boldStart, inlines, s => new XBold(s));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        searchStartIndex = boldStart + 1;
                    }
                    formattedInlineMatch = true;
                }
                else if (italicStart != -1)
                {
                    try
                    {
                        str = ParseNextInline(str, italicDelimiter, italicStart, inlines, s => new XItalic(s));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        searchStartIndex = italicStart + 1;
                    }
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
            while (delimiterEnd != -1 && delimiterEnd <= str.Length && str[delimiterEnd -1] == '\\')
            {
                delimiterEnd = str.IndexOf(delimiter, delimiterEnd + 1, StringComparison.Ordinal);
            }

            if (start == 0)
            {
                var substring = str.Substring(start + delimiterLength, delimiterEnd - start - delimiterLength);
                inlines.Add(factory(RemoveEscapedMarkdown(substring)));
                str = str.Substring(delimiterEnd + delimiterLength);
            }
            else
            {
                var substring = str.Substring(start + delimiterLength, delimiterEnd - start - delimiterLength);
                inlines.Add(new XRun(str.Substring(0, start)));
                inlines.Add(factory(RemoveEscapedMarkdown(substring)));
                str = str.Substring(delimiterEnd + delimiterLength);
            }
            return str;
        }

        private static string RemoveEscapedMarkdown(string substring)
        {
            return substring.Replace(@"\*", "*");
        }
    }
}