using System.Linq;
using System.Windows.Documents;

namespace XText
{
    public static class FormattingCalculator
    {
        public static bool RequiresSpace(Inline lastInline, Inline nextInline)
        {
            var requiresSpace = true;
            var nextRun = nextInline as Run;
            var nextInlineFirstCharacter = nextRun == null ? '\0' : (nextRun.Text ?? string.Empty).FirstOrDefault();

            if (lastInline == null)
                requiresSpace = RequiresSpace('\0', nextInlineFirstCharacter);

            var lineBreak = lastInline as LineBreak;
            if (lineBreak != null)
                requiresSpace = RequiresSpace('\n', nextInlineFirstCharacter);

            var run = lastInline as Run;
            if (run != null)
                requiresSpace = RequiresSpace(run.Text.Last(), nextInlineFirstCharacter);

            return requiresSpace;
        }

        public static bool RequiresSpace(char lastCharacter, char nextCharacter)
        {
            if (lastCharacter == '\0' || lastCharacter == '\n' || lastCharacter == ' ' || lastCharacter == '(')
                return false;

            if (nextCharacter == ')' || nextCharacter == '.' || nextCharacter == '?' || nextCharacter == ';' ||
                nextCharacter == ':' || nextCharacter == ',' || nextCharacter == ' ' || nextCharacter == '!')
                return false;

            return true;
        }
    }
}