using System.Linq;

using FluentAssertions;

using Xunit;

namespace XmlComparer.Tests
{
    public class AttributeTests
    {
        [Fact]
        public void Given_the_same_attributes_It_should_not_report_any_difference()
        {
            var left = @"<foo bar=""baz"" />".AsXml();
            var right = @"<foo bar=""baz"" />".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_one_added_attribute_It_should_report_an_add()
        {
            var left = @"<foo />".AsXml();
            var right = @"<foo bar=""baz"" />".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Added);
            diff.DifferenceSource.Should().Be(DifferenceSource.AttributeName);
        }

        [Fact]
        public void Given_one_removed_attribute_It_should_report_a_remove()
        {
            var left = @"<foo bar=""baz"" />".AsXml();
            var right = @"<foo />".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Removed);
            diff.DifferenceSource.Should().Be(DifferenceSource.AttributeName);
        }

        [Fact]
        public void Given_two_different_attributes_and_set_to_ignore_order_It_should_report_one_add_and_one_remove()
        {
            var left = @"<foo bar=""baz"" />".AsXml();
            var right = @"<foo bim=""bam"" />".AsXml();
            var sut = new Comparer(ignoreAttributeOrder: true);

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().HaveCount(2);
            var add = actual.Single(x => x.DifferenceType == DifferenceType.Added);
            var remove = actual.Single(x => x.DifferenceType == DifferenceType.Removed);
        }

        [Fact]
        public void Given_two_different_attributes_and_set_not_to_ignore_order_It_should_report_one_change()
        {
            var left = @"<foo bar1=""baz"" />".AsXml();
            var right = @"<foo bar2=""baz"" />".AsXml();
            var sut = new Comparer(ignoreAttributeOrder: false);

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.AttributeName);
        }

        [Fact]
        public void Given_an_attribute_with_changed_value_It_should_report_the_difference()
        {
            var left = @"<foo bar=""baz1"" />".AsXml();
            var right = @"<foo bar=""baz2"" />".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.AttributeValue);
        }

        [Fact]
        public void Given_attributes_with_changed_order_ignore_set_It_should_not_report_a_difference()
        {
            var left = @"<foo bar1=""baz1"" bar2=""baz2"" />".AsXml();
            var right = @"<foo bar2=""baz2"" bar1=""baz1"" />".AsXml();
            var sut = new Comparer(ignoreAttributeOrder: true);

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().BeEmpty();
        }
    }
}
