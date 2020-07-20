using System.Linq;

using FluentAssertions;

using Xunit;

namespace XmlComparer.Tests
{
    public class ElementTypeTests
    {
        [Fact]
        public void Given_the_same_childnodes_It_should_not_report_any_difference()
        {
            var left = @"<foo><bar /></foo>".AsXml();
            var right = @"<foo><bar /></foo>".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_childnodes_It_should_report_a_change()
        {
            var left = @"<foo><bar /></foo>".AsXml();
            var right = @"<foo><baz /></foo>".AsXml();
            var sut = new Comparer();

            var actual = sut.GetDifferences(left, right);

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.ChildNode);
        }

        [Fact]
        public void Given_the_reordered_childnodes_with_ignore_set_It_should_not_report_a_change()
        {
            var left = @"<foo><bar /><baz /></foo>".AsXml();
            var right = @"<foo><baz /><bar /></foo>".AsXml();
            var sut = new Comparer(ignoreChildNodeOrder: true);

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_an_added_childnode_It_should_report_an_add()
        {
            var left = @"<foo />".AsXml();
            var right = @"<foo><bar /></foo>".AsXml();
            var sut = new Comparer(ignoreChildNodeOrder: true);

            var actual = sut.GetDifferences(left, right);

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Added);
            diff.DifferenceSource.Should().Be(DifferenceSource.ChildNode);
        }

        [Fact]
        public void Given_a_removed_childnode_It_should_report_a_remove()
        {
            var left = @"<foo><bar /></foo>".AsXml();
            var right = @"<foo />".AsXml();
            var sut = new Comparer(ignoreChildNodeOrder: true);

            var actual = sut.GetDifferences(left, right);

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Removed);
            diff.DifferenceSource.Should().Be(DifferenceSource.ChildNode);
        }
    }
}
