using System.Xml;

namespace XmlComparer.Tests
{
    internal static class StringExtensions
    {
        public static XmlDocument AsXml(this string @this)
        {
            var doc = new XmlDocument();
            doc.LoadXml(@this);
            return doc;
        }
    }
}
