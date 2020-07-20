using System.Linq;

using FluentAssertions;

using Xunit;

namespace XmlComparer.Tests
{
    public class CdataTypeTests
    {
        [Fact]
        public void Given_the_same_comment_It_should_not_report_a_change()
        {
            var left = @"<foo><![CDATA[ bar ]]></foo>".AsXml();
            var right = @"<foo><![CDATA[ bar ]]></foo>".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_comments_It_should_report_a_change()
        {
            var left = @"<foo><![CDATA[ bar ]]></foo>".AsXml();
            var right = @"<foo><![CDATA[ baz ]]></foo>".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right);

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.ChildNode);
        }

        [Fact]
        public void Given_different_comments_with_ignore_set_It_should_not_report_a_change()
        {
            var left = @"<foo><![CDATA[ bar ]]></foo>".AsXml();
            var right = @"<foo><![CDATA[ baz ]]></foo>".AsXml();
            var sut = new Comparer(ignoreCdata: true);

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }
    }
}
