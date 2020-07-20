namespace XmlComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// The XmlComparer. Compares xml and returns differences.
    /// <code>
    /// <![CDATA[
    /// var left = new XmlDocument();
    /// left.Load("C:\temp\demo1.xml");
    ///
    /// var right = new XmlDocument();
    ///     right.Load("C:\temp\demo2.xml");
    ///
    /// var comparer = new XmlComparer.Comparer(
    ///   ignoreAttributeOrder: true,
    ///   ignoreNamespace: true)
    /// var differences = comparer.GetDifferences(left, right);
    /// ]]>
    /// </code>
    /// <para>
    /// See <see cref="Comparer.Comparer(bool, bool, bool, bool, bool, bool)">constructor</see>
    /// for all configuration-properties.
    /// </para>
    /// </summary>
    public class Comparer
    {
        private readonly bool ignoreNamespace;
        private readonly bool ignorePrefix;
        private readonly bool ignoreChildNodeOrder;
        private readonly bool ignoreAttributeOrder;
        private readonly bool ignoreComments;
        private readonly bool ignoreCdata;

        /// <summary>
        /// Initializes a new instance of the <see cref="Comparer"/> class.
        /// </summary>
        /// <param name="ignoreNamespace">if set to <c>true</c> to ignore namespace-uri differences.</param>
        /// <param name="ignorePrefix">if set to <c>true</c> to ignore namespace-prefix differences.</param>
        /// <param name="ignoreChildNodeOrder">if set to <c>true</c> to ignore the orrder of child-nodes.</param>
        /// <param name="ignoreAttributeOrder">if set to <c>true</c> to ignore the order of attributes on a node.</param>
        /// <param name="ignoreComments">if set to <c>true</c> to ignore comments in the xml.</param>
        /// <param name="ignoreCdata">if set to <c>true</c> to ignore CDATA in the xml.</param>
        public Comparer(
            bool ignoreNamespace = false,
            bool ignorePrefix = false,
            bool ignoreChildNodeOrder = false,
            bool ignoreAttributeOrder = false,
            bool ignoreComments = false,
            bool ignoreCdata = false)
        {
            this.ignoreNamespace = ignoreNamespace;
            this.ignorePrefix = ignorePrefix;
            this.ignoreChildNodeOrder = ignoreChildNodeOrder;
            this.ignoreAttributeOrder = ignoreAttributeOrder;
            this.ignoreComments = ignoreComments;
            this.ignoreCdata = ignoreCdata;
        }

        /// <summary>
        /// Gets the differences.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        /// <returns>The list of differences.</returns>
        /// <exception cref="ArgumentNullException">One of the arguments are null.</exception>
        /// <exception cref="UncomparableNodeException">Can not compare left NodeType right.</exception>
        public IEnumerable<XmlDifference> GetDifferences(XmlNode left, XmlNode right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.NodeType != right.NodeType)
            {
                throw new UncomparableNodeException($"Can not compare {Enum.GetName(typeof(XmlNodeType), left.NodeType)} to {Enum.GetName(typeof(XmlNodeType), right.NodeType)}");
            }

            foreach (var diff in GetNamespaceDifferences(left, right))
            {
                yield return diff;
            }

            foreach (var diff in GetAttributeDifferences(left, right))
            {
                yield return diff;
            }

            foreach (var diff in GetChildNodeDifferences(left, right))
            {
                yield return diff;
            }
        }

        private IEnumerable<XmlDifference> GetNamespaceDifferences(XmlNode left, XmlNode right)
        {
            foreach (var diff in GetNamespaceUriDifferences(left, right))
            {
                yield return diff;
            }

            foreach (var diff in GetNamespacePrefixDifferences(left, right))
            {
                yield return diff;
            }
        }

        private IEnumerable<XmlDifference> GetNamespacePrefixDifferences(XmlNode left, XmlNode right)
        {
            if (ignorePrefix)
            {
                yield break;
            }

            var prefixLeft = left.Prefix;
            var prefixRight = right.Prefix;
            var hasPrefixLeft = !string.IsNullOrEmpty(prefixLeft);
            var hasPrefixRight = !string.IsNullOrEmpty(prefixRight);

            if (hasPrefixLeft && !hasPrefixRight)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Removed,
                    DifferenceSource = DifferenceSource.NamespacePrefix,
                    LeftHint = prefixLeft,
                };

                yield break;
            }

            if (!hasPrefixLeft && hasPrefixRight)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Added,
                    DifferenceSource = DifferenceSource.NamespacePrefix,
                    RightHint = prefixRight,
                };

                yield break;
            }

            if (prefixLeft != prefixRight)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Changed,
                    DifferenceSource = DifferenceSource.NamespacePrefix,
                    LeftHint = prefixLeft,
                    RightHint = prefixRight,
                };
            }
        }

        private IEnumerable<XmlDifference> GetNamespaceUriDifferences(XmlNode left, XmlNode right)
        {
            if (ignoreNamespace)
            {
                yield break;
            }

            var nsLeft = left.NamespaceURI;
            var nsRight = right.NamespaceURI;
            var hasLeftNs = !string.IsNullOrEmpty(nsLeft);
            var hasRightNs = !string.IsNullOrEmpty(nsRight);

            if (hasLeftNs && !hasRightNs)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Removed,
                    DifferenceSource = DifferenceSource.NamespaceUri,
                    LeftHint = nsLeft,
                };

                yield break;
            }

            if (!hasLeftNs && hasRightNs)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Added,
                    DifferenceSource = DifferenceSource.NamespaceUri,
                    RightHint = nsRight,
                };

                yield break;
            }

            if (nsLeft != nsRight)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Changed,
                    DifferenceSource = DifferenceSource.NamespaceUri,
                    LeftHint = nsLeft,
                    RightHint = nsRight,
                };
            }
        }

        private IEnumerable<XmlDifference> GetAttributeDifferences(XmlNode left, XmlNode right)
        {
            Func<XmlAttribute, bool> filter = x =>
            {
                // namespaces are attributes, too - ignore them here.
                return x.Prefix != "xmlns" && x.LocalName != "xmlns";
            };

            var leftAttributes = new List<XmlAttribute>();
            if (left.Attributes != null)
            {
                leftAttributes = left.Attributes.Cast<XmlAttribute>().Where(filter).ToList();
            }

            var rightAttributes = new List<XmlAttribute>();
            if (right.Attributes != null)
            {
                rightAttributes = right.Attributes.Cast<XmlAttribute>().Where(filter).ToList();
            }

            while (leftAttributes.Count > 0)
            {
                var l = leftAttributes[0];
                leftAttributes.RemoveAt(0);

                XmlAttribute r = null;
                if (ignoreAttributeOrder)
                {
                    r = rightAttributes.FirstOrDefault(x => x.LocalName == l.LocalName);
                }
                else if (rightAttributes.Count > 0)
                {
                    r = rightAttributes[0];
                }

                if (r == null)
                {
                    yield return new XmlDifference
                    {
                        DifferenceType = DifferenceType.Removed,
                        DifferenceSource = DifferenceSource.AttributeName,
                        LeftHint = l.LocalName,
                    };

                    continue;
                }

                rightAttributes.Remove(r);

                if (l.LocalName != r.LocalName)
                {
                    yield return new XmlDifference
                    {
                        DifferenceType = DifferenceType.Changed,
                        DifferenceSource = DifferenceSource.AttributeName,
                        LeftHint = l.LocalName,
                        RightHint = r.LocalName,
                    };
                }

                if (l.Value != r.Value)
                {
                    yield return new XmlDifference
                    {
                        DifferenceType = DifferenceType.Changed,
                        DifferenceSource = DifferenceSource.AttributeValue,
                        LeftHint = l.LocalName,
                        RightHint = r.LocalName,
                    };
                }
            }

            foreach (var add in rightAttributes)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Added,
                    DifferenceSource = DifferenceSource.AttributeName,
                    RightHint = add.LocalName,
                };
            }
        }

        private IEnumerable<XmlDifference> GetChildNodeDifferences(XmlNode left, XmlNode right)
        {
            // TODO: XmlNodeType.Whitespace ?
            // TODO: XmlNodeType.DocumentType ?
            // TODO: XmlNodeType.Entity ?
            // TODO: XmlNodeType.Notation ?
            // TODO: XmlNodeType.ProcessingInstruction ?
            // TODO: XmlNodeType.XmlDeclaration?
            var allowedNodes = new List<XmlNodeType>(new[]
            {
                XmlNodeType.Element,
                XmlNodeType.Text,
            });

            if (!ignoreComments)
            {
                allowedNodes.Add(XmlNodeType.Comment);
            }
            if (!ignoreCdata)
            {
                allowedNodes.Add(XmlNodeType.CDATA);
            }

            var leftNodes = left.ChildNodes.Cast<XmlNode>().Where(x => allowedNodes.Contains(x.NodeType)).ToList();
            var rightNodes = right.ChildNodes.Cast<XmlNode>().Where(x => allowedNodes.Contains(x.NodeType)).ToList();

            while (leftNodes.Count > 0)
            {
                var l = leftNodes[0];
                leftNodes.RemoveAt(0);

                XmlNode r = null;
                if (ignoreChildNodeOrder && !string.IsNullOrWhiteSpace(l.LocalName))
                {
                    r = rightNodes.FirstOrDefault(x => x.LocalName == l.LocalName);
                }
                else if (rightNodes.Count > 0)
                {
                    r = rightNodes[0];
                }

                if (r == null)
                {
                    yield return new XmlDifference
                    {
                        DifferenceType = DifferenceType.Removed,
                        DifferenceSource = DifferenceSource.ChildNode,
                        LeftHint = l.LocalName,
                    };

                    continue;
                }

                rightNodes.Remove(r);

                IEnumerable<XmlDifference> subDiffs = null;
                switch (l.NodeType)
                {
                    case XmlNodeType.Element:
                        subDiffs = CompareElements(l, r);
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Comment:
                        subDiffs = CompareTexts(l, r);
                        break;

                    default:
                        throw new NotSupportedException($"The node-type {Enum.GetName(typeof(XmlNodeType), l.NodeType)} has no comparison. Please report this bug!");
                }

                if (subDiffs != null)
                {
                    foreach (var d in subDiffs)
                    {
                        yield return d;
                    }
                }
            }

            foreach (var r in rightNodes)
            {
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Added,
                    DifferenceSource = DifferenceSource.ChildNode,
                    RightHint = r.LocalName,
                };
            }
        }

        private IEnumerable<XmlDifference> CompareTexts(XmlNode l, XmlNode r)
        {
            if (l.Value != r.Value)
            {
                // TODO: This can be potentially a long text. Probably show the difference better?!
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Changed,
                    DifferenceSource = DifferenceSource.ChildNode,
                    LeftHint = l.Value,
                    RightHint = r.Value,
                };
            }
        }

        private IEnumerable<XmlDifference> CompareElements(XmlNode l, XmlNode r)
        {
            if (l.LocalName != r.LocalName)
            {
                // happens only when ignoreChildNodeOrder is false..
                yield return new XmlDifference
                {
                    DifferenceType = DifferenceType.Changed,
                    DifferenceSource = DifferenceSource.ChildNode,
                    LeftHint = l.LocalName,
                    RightHint = r.LocalName,
                };
            }

            // Elements have sub-Nodes, so there's recursion here..
            foreach (var d in GetDifferences(l, r))
            {
                yield return d;
            }
        }
    }
}
