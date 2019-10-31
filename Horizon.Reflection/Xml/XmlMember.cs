using System;
using System.Collections.Generic;
using System.Xml;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a member node in the generated XML document from C# code.
    /// </summary>
    internal sealed class XmlMember
    {
        /// <summary>
        /// Creates a new instance of <see cref="XmlMember"/>.
        /// </summary>
        /// <param name="xmlNode">XML node.</param>
        internal XmlMember(XmlNode xmlNode)
        {
            Name = GetName(xmlNode);
            Summary = GetSummary(xmlNode);
            Parameters = GetParameters(xmlNode);
        }

        /// <summary>
        /// Member name.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Member summary.
        /// </summary>
        internal string Summary { get; }

        /// <summary>
        /// Member parameters.  This will always be null for all non-method members.
        /// </summary>
        internal IReadOnlyDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Gets the member name from the specified <see cref="XmlNode"/>, if it exists.
        /// </summary>
        /// <param name="xmlNode">XML node.</param>
        /// <returns>Member name.</returns>
        private static string GetName(XmlNode xmlNode)
        {
            try
            {
                return xmlNode.Attributes["name"].Value.Trim();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the member summary from the specified <see cref="XmlNode"/>, if it exists.
        /// </summary>
        /// <param name="xmlNode">XML node.</param>
        /// <returns>Member summary.</returns>
        private static string GetSummary(XmlNode xmlNode)
        {
            try
            {
                return xmlNode["summary"].InnerText.Trim();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Gets the member parameters from the specified <see cref="XmlNode"/>, if it exists.
        /// </summary>
        /// <param name="xmlNode">XML node.</param>
        /// <returns>Member parameters.</returns>
        private static Dictionary<string, string> GetParameters(XmlNode xmlNode)
        {
            try
            {
                var parameters = new Dictionary<string, string>();
                var sibling = xmlNode["param"] as XmlNode;

                while (sibling != null)
                {
                    var name = sibling.Attributes["name"]?.Value;

                    if (name != null)
                    {
                        parameters[name] = sibling.InnerText?.Trim();
                    }

                    sibling = sibling.NextSibling;
                }

                return parameters;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}