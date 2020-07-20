using System.Linq;

using FluentAssertions;

using Xunit;

namespace XmlComparer.Tests
{
    public class NamespaceUriTests
    {
        [Fact]
        public void Given_the_same_namespace_It_should_not_report_any_difference()
        {
            var left = @"<foo xmlns=""https://www.namespace1.local"" />".AsXml();
            var right = @"<foo xmlns=""https://www.namespace1.local"" />".AsXml();
            var sut = new Comparer(
                ignoreNamespace: false
            );

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_namespaces_and_set_to_ignore_It_should_not_report_any_difference()
        {
            var left = @"<foo xmlns=""https://www.namespace1.local"" />".AsXml();
            var right = @"<foo xmlns=""https://www.namespace2.local"" />".AsXml();
            var sut = new Comparer(
                ignoreNamespace: true
            );

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_namespaces_and_set_not_to_ignore_It_should_report_the_difference()
        {
            var left = @"<foo xmlns=""https://www.namespace1.local"" />".AsXml();
            var right = @"<foo xmlns=""https://www.namespace2.local"" />".AsXml();
            var sut = new Comparer(
                 ignoreNamespace: false
             );

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.NamespaceUri);
        }

        [Fact]
        public void Given_the_same_namespace_as_prefix_It_should_not_report_any_difference()
        {
            var left = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var right = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var sut = new Comparer(
                ignoreNamespace: false
            );

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_namespaces_as_prefix_and_set_to_ignore_It_should_not_report_any_difference()
        {
            var left = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var right = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var sut = new Comparer(
                ignoreNamespace: true
            );

            var actual = sut.GetDifferences(left, right);

            actual.Should().BeEmpty();
        }

        [Fact]
        public void Given_different_namespaces_as_prefix_It_should_report_a_difference()
        {
            var left = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var right = @"<ns1:foo xmlns:ns1=""https://www.namespace2.local"" />".AsXml();
            var sut = new Comparer(
                 ignoreNamespace: false
             );

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Changed);
            diff.DifferenceSource.Should().Be(DifferenceSource.NamespaceUri);
        }

        [Fact]
        public void Given_an_added_namespace_It_should_report_an_add()
        {
            var left = @"<foo />".AsXml();
            var right = @"<foo xmlns=""https://www.namespace1.local"" />".AsXml();
            var sut = new Comparer(
                 ignoreNamespace: false
             );

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Added);
            diff.DifferenceSource.Should().Be(DifferenceSource.NamespaceUri);
        }

        [Fact]
        public void Given_a_removed_namespace_It_should_report_a_remove()
        {
            var left = @"<foo xmlns=""https://www.namespace1.local"" />".AsXml();
            var right = @"<foo />".AsXml();
            var sut = new Comparer(
                 ignoreNamespace: false
             );

            var actual = sut.GetDifferences(left, right).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Removed);
            diff.DifferenceSource.Should().Be(DifferenceSource.NamespaceUri);
        }

        [Fact]
        public void Given_an_added_namespace_as_prefix_It_should_report_an_add()
        {
            var left = @"<foo />".AsXml();
            var right = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var sut = new Comparer(
                 ignoreNamespace: false
             );

            var actual = sut.GetDifferences(left, right)
                .Where(x => x.DifferenceSource == DifferenceSource.NamespaceUri).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Added);
            diff.DifferenceSource.Should().Be(DifferenceSource.NamespaceUri);
        }

        [Fact]
        public void Given_a_removed_namespace_as_prefix_It_should_report_a_remove()
        {
            var left = @"<ns1:foo xmlns:ns1=""https://www.namespace1.local"" />".AsXml();
            var right = @"<foo />".AsXml();
            var sut = new Comparer(
                 ignoreNamespace: false
             );

            var actual = sut.GetDifferences(left, right)
                .Where(x => x.DifferenceSource == DifferenceSource.NamespaceUri).ToList();

            actual.Should().ContainSingle();
            var diff = actual.Single();
            diff.DifferenceType.Should().Be(DifferenceType.Removed);
            diff.DifferenceSource.Should().Be(DifferenceSource.NamespaceUri);
        }
    }
}
