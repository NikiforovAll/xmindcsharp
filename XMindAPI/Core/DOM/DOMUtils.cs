
#nullable disable
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using static XMindAPI.Core.DOM.DOMConstants;

namespace XMindAPI.Core.DOM
{
    internal class DOMUtils
    {

        internal static XElement AddIdAttribute(XElement element)
        {
            if (!element.Attributes().Any())
            {
                element.SetAttributeValue(ATTR_ID, Core.GetFactory().CreateId());
                //TODO: setIdAttribute(tag, true) - investigate
            }
            return element;
        }

        internal static XElement GetElementById(XDocument document, string id)
        {
            var result = document.Descendants()
                .Where(el => el.Attributes("id").FirstOrDefault()?.Equals(id) ?? false)
                .FirstOrDefault();
            if (result == null)
            {
                result = document.Descendants().FirstOrDefault(el => GetElementById(el, id) != null);
            }
            return result;
        }

        internal static XElement GetElementById(XElement element, string id)
        {
            XElement result = null;
            foreach (var item in element.Descendants())
            {
                result = item.Attributes("id").FirstOrDefault()?.Equals(id) ?? false ? item : null;
                result = GetElementById(item, id);
            }
            return result;
        }


        internal static List<T> GetChildList<T>(XElement element, string childTag, NodeAdaptableRegistry registry) where T : IAdaptable
        {
            List<T> result = new List<T>();
            foreach (var item in GetChildElementsByTag(element, childTag))
            {
                IAdaptable adaptable = registry.GetAdaptable(item);
                if (adaptable != null)
                {
                    result.Add((T)adaptable);
                }
            }
            return result;
        }

        internal static IEnumerable<XElement> GetChildElementsByTag(XNode element, string tagName)
        {
            return ((XElement)element).Elements(tagName);
        }

        internal static XElement GetFirstElementByTagName(XNode element, string tagName)
        {
            return GetChildElementsByTag(element, tagName).FirstOrDefault();
        }

        internal static string GetTextContentByTag(XElement element, string tagName)
        {
            XElement item = GetFirstElementByTagName(element, tagName);
            return item?.Value;
        }

        internal static void SetText(XNode node, string tagName, string textContent)
        {
            XNode textNode = GetFirstElementByTagName(node, tagName);
            if (textNode != null)
            {
                if (textContent == null)
                {
                    textNode.Remove();
                }
                else
                {
                    var element = (XElement)textNode;
                    element.Value = textContent;
                }
            }
            else
            {
                //TODO: strange behavior - investigate
                var element = (XElement)node;
                element.Add(new XElement(tagName, textContent));
            }
        }
        internal static XElement CreateText(XNode parent, string tagName, string content)
        {
            XElement element = CreateElement(parent, tagName);
            element.Value = content;
            return element;
        }
        internal static XNode FindTextNode(XNode node)
        {
            return node.Ancestors()
            .Where(el => el.NodeType == System.Xml.XmlNodeType.Text)
            .FirstOrDefault();
        }

        internal static XElement CreateElement(XNode node, string tagName)
        {
            XDocument doc = node.NodeType == System.Xml.XmlNodeType.Document ?
                node as XDocument :
                node.Document;
            //TODO: differs from Java implementation
            var innerElement = new XElement(tagName);
            (node as XElement)?.Add(innerElement);
            return innerElement;
        }

        internal static XElement EnsureChildElement(XNode parent, string tagName)
        {
            XElement element;
            if (parent.NodeType == System.Xml.XmlNodeType.Document)
            {
                element = parent.Parent;
            }
            else
            {
                element = GetFirstElementByTagName(parent, tagName);
            }
            if (element == null)
            {
                // Logger.Info($"EnsureChildElement.CreateElement: item {tagName}was created for {parent.NodeType}");
                CreateElement(parent, tagName);
            }
            return element;
        }
    }
}
