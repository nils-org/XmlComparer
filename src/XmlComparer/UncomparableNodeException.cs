namespace XmlComparer
{
    using System;

    /// <summary>
    /// The nodes supplied are not comparable.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors

    public class UncomparableNodeException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UncomparableNodeException"/> class.
        /// </summary>
        /// <param name="message">The Message.</param>
        public UncomparableNodeException(string message)
            : base(message)
        {
        }
    }
}