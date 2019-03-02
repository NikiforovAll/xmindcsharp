using System;
using System.Xml.Linq;
using System.Linq;
using static XMindAPI.Core.DOM.DOMConstants;
namespace XMindAPI.Core.DOM
{
    internal class DOMUtils
    {
        internal static XElement AddIdAttribute(XElement element)
        {
            if(!element.Attributes().Any())
            {
                element.SetAttributeValue(ATTR_ID, Core.GetFactory().CreateId());
                //TODO: setIdAttribute(tag, true) - investigate
            }
            return element;
        }

        internal static XElement GetElementById(XDocument document, string id)
        {
            var element = document.Descendants()
                    .Where(el => el.Attribute("id").Equals(id))
                    .FirstOrDefault();
            return element;
        }
    }
}