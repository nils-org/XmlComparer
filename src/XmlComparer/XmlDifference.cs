namespace XmlComparer
{
    /// <summary>
    /// A single difference between two xml-Files.
    /// </summary>
    public sealed class XmlDifference
    {
        /// <summary>
        /// Gets or sets the type of the difference.
        /// </summary>
        /// <value>
        /// The type of the difference.
        /// </value>
        public DifferenceType DifferenceType { get; set; }

        /// <summary>
        /// Gets or sets the difference target.
        /// </summary>
        /// <value>
        /// The difference target.
        /// </value>
        public DifferenceSource DifferenceSource { get; set; }

        /// <summary>
        /// Gets or sets the left hint.
        /// </summary>
        /// <value>
        /// The left hint.
        /// </value>
        public string LeftHint { get; set; }

        /// <summary>
        /// Gets or sets the right hint.
        /// </summary>
        /// <value>
        /// The right hint.
        /// </value>
        public string RightHint { get; set; }
    }
}