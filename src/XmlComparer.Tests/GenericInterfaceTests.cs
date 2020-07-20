using System;
using System.Linq;
using System.Xml;

using FluentAssertions;

using Xunit;

namespace XmlComparer.Tests
{
    public class GenericInterfaceTests
    {
        [Fact]
        public void Given_the_left_node_is_null_It_should_throw()
        {
            var x = @"<foo />".AsXml();
            var sut = new Comparer();

            Action action = () => sut.GetDifferences(null, x).ToList();

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_the_right_node_is_null_It_should_throw()
        {
            var x = @"<foo />".AsXml();
            var sut = new Comparer();

            Action action = () => sut.GetDifferences(x, null).ToList();

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_two_different_node_types_It_should_throw()
        {
            var x = @"<foo />".AsXml(); // document
            var y = x.ChildNodes.Cast<XmlNode>().First(); // foo
            var sut = new Comparer();

            Action action = () => sut.GetDifferences(x, y).ToList();

            action.Should().Throw<UncomparableNodeException>();
        }
    }
}
