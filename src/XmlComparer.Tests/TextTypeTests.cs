using System.Linq;

using FluentAssertions;

using Xunit;

namespace XmlComparer.Tests
{
    public class TextTypeTests
    {
        [Fact]
        public void Given_the_same_text_It_should_not_report_a_change()
        {
            var left = @"<foo>BAR</foo>".AsXml();
            var right = @"<foo>BAR</foo>".AsXml();
            var sut = new Comparer(ignoreChildNodeOrder: true);

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_texts_It_should_report_a_change()
        {
            var left = @"<foo>BAR</foo>".AsXml();
            var right = @"<foo>BAZ</foo>".AsXml();
            var sut = new Comparer(ignoreChildNodeOrder: true);

            var actual = sut.GetDifferences(left, right);

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.ChildNode);
        }
    }
}
