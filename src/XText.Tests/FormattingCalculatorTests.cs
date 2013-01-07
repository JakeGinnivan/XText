using System.Windows.Documents;
using Xunit;
using Xunit.Extensions;

namespace XText.Tests
{
    public class FormattingCalculatorTests
    {
        [Theory]
        [InlineData('\0', false)]
        [InlineData('\n', false)]
        [InlineData('.', true)]
        [InlineData(',', true)]
        [InlineData('?', true)]
        [InlineData(';', true)]
        [InlineData(':', true)]
        [InlineData('a', true)]
        [InlineData('1', true)]
        [InlineData('!', true)]
        [InlineData(' ', false)]
        public void AddsSpaceTheoryDependingOnLastCharacter(char lastCharacter, bool shouldAddSpace)
        {
            var needsSpace = FormattingCalculator.RequiresSpace(lastCharacter, '\0');

            Assert.Equal(shouldAddSpace, needsSpace);
        }

        [Fact]
        public void DoesNotAddsSpaceForLineBreak()
        {
            var needsSpace = FormattingCalculator.RequiresSpace(new LineBreak(), new Run("A"));

            Assert.False(needsSpace);
        }

        [Fact]
        public void DoesNotAddsSpaceForNoLastInline()
        {
            var needsSpace = FormattingCalculator.RequiresSpace(null, new Run("A"));

            Assert.False(needsSpace);
        }

        [Fact]
        public void AddsSpaceForRun()
        {
            var needsSpace = FormattingCalculator.RequiresSpace(new Run("."), new Run("A"));

            Assert.True(needsSpace);
        }

        [Theory]
        [InlineData(')', false)]
        [InlineData('.', false)]
        [InlineData(',', false)]
        [InlineData('?', false)]
        [InlineData(';', false)]
        [InlineData(':', false)]
        [InlineData('a', true)]
        [InlineData('1', true)]
        [InlineData('!', false)]
        [InlineData(' ', false)]
        public void AddsSpaceTheoryDependingOnNext(char nextCharacters, bool shouldAddSpace)
        {
            const char lastCharacter = '.';

            var needsSpace = FormattingCalculator.RequiresSpace(lastCharacter, nextCharacters);

            Assert.Equal(shouldAddSpace, needsSpace);
        }
    }
}