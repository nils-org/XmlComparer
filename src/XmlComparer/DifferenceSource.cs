namespace XmlComparer
{
    /// <summary>
    /// The source of the difference.
    /// </summary>
    public enum DifferenceSource
    {
        /// <summary>
        /// The namespace-URI-source
        /// </summary>
        NamespaceUri,

        /// <summary>
        /// The namespace-prefix-source
        /// </summary>
        NamespacePrefix,

        /// <summary>
        /// The attribute-name-source
        /// </summary>
        AttributeName,

        /// <summary>
        /// The attribute-value-source
        /// </summary>
        AttributeValue,

        /// <summary>
        /// The childNode-source
        /// </summary>
        ChildNode,
    }
}