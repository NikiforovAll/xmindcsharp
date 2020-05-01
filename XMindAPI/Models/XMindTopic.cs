using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using XMindAPI.Core;
using XMindAPI.Core.DOM;
using XMindAPI.Infrastructure.Logging;

using static XMindAPI.Core.DOM.DOMConstants;

namespace XMindAPI.Models
{

    /// <summary>
    ///  Base element of build XMind maps, topics are added to <see cref="XMindWorkBook"/>
    /// </summary>
    public class XMindTopic : ITopic
    {
        public XMindTopic(XElement implementation, XMindWorkBook book)
        {
            OwnedWorkbook = book;
            Implementation = DOMUtils.AddIdAttribute(implementation);
        }

        public IWorkbook OwnedWorkbook { get; set; }
        public XElement Implementation { get; }

        public ITopic Parent => throw new NotImplementedException();

        public ISheet? OwnedSheet { get; set; }

        private readonly TopicType _type = TopicType.Root;
        public TopicType Type { get => _type; set => throw new NotImplementedException(); }
        public bool IsFolded
        {
            get => Implementation.Attribute("branch")?.Value == "folded";
            set
            {
                Implementation.SetAttributeValue("branch", value ? "folded" : null);
            }
        }
        public IList<ITopic> Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // TODO: Add possibility to link topics
        public string? HyperLink
        {
            get => Implementation.Attribute(XName.Get("href"))?.Value;
            set => Implementation.SetAttributeValue(XName.Get("href"), value);
        }

        public void AddLabel(string label)
        {
            DOMUtils.EnsureChildElement(Implementation, TAG_LABELS);
            var labelsTag = Implementation.Element(TAG_LABELS);
            labelsTag.Add(new XElement(TAG_LABEL) { Value = label });
        }
        public void RemoveAllLabels()
        {
            var labelsTag = Implementation.Element(TAG_LABELS);
            labelsTag.RemoveNodes();
        }

        public void RemoveLabel(string label)
        {
            Implementation.Element(TAG_LABELS)
                .Elements(TAG_LABEL)
                .Where(elem => elem.Value
                    .Equals(label, StringComparison.InvariantCultureIgnoreCase))
                .Remove();
        }

        public void SetLabels(ICollection<string> labels)
        {
            DOMUtils.EnsureChildElement(Implementation, TAG_LABELS);
            Implementation.Element(TAG_LABELS)
                .ReplaceNodes(labels.Select(label => new XElement(TAG_LABEL) { Value = label }));
        }

        public HashSet<string> GetLabels() =>
            new HashSet<string>(Implementation.Element(TAG_LABELS)
                .Elements().Select(elem => elem.Value));


        public void AddMarker(string markerId)
        {
            DOMUtils.EnsureChildElement(Implementation, TAG_MARKER_REFS);
            var markersTag = Implementation.Element(TAG_MARKER_REFS);
            markersTag.Add(new XElement(
                TAG_MARKER_REF, new XAttribute(ATTR_MARKER_ID, markerId)));
        }

        public void RemoveMarker(string markerId)
        {
            Implementation.Element(TAG_MARKER_REFS)
                ?.Elements()
                .Where(elem => elem.Attribute(ATTR_MARKER_ID).Value?.Equals(markerId) ?? false)
                .Remove();
        }

        public bool HasMarker(string markerId) => Implementation.Element(TAG_MARKER_REFS)
                ?.Elements()
                ?.Any(elem => elem.Attribute(ATTR_MARKER_ID).Value?.Equals(markerId) ?? false) ?? false;

        public string GetId()
        {
            return Implementation.Attribute(ATTR_ID).Value;
        }

        public string GetTitle()
        {
            return DOMUtils.GetTextContentByTag(Implementation, TAG_TITLE);
        }

        public bool HasTitle() => !string.IsNullOrWhiteSpace(GetTitle());

        public void SetTitle(string value)
        {
            DOMUtils.SetText(Implementation, TAG_TITLE, value);
        }


        public override int GetHashCode()
        {
            // TODO: confirm behavior
            return Implementation.GetHashCode();
        }

        public override string ToString()
        {
            return $"TPC# Id:{GetId()} ({GetTitle()})";
        }

        public void Add(ITopic child, int index = -1, TopicType type = TopicType.Attached)
        {
            if (!(child is XMindTopic childTopic))
            {
                var errorMessage = $"XMindTopic.Add: {nameof(child)} is not valid XMindTopic";
                Logger.Log.Error(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            var typeName = Enum.GetName(type.GetType(), type).ToLower();
            // Override topic type
            // child.Type = type;
            // Add children tag
            DOMUtils.EnsureChildElement(Implementation, TAG_CHILDREN);
            var childrenTag = Implementation.Elements(TAG_CHILDREN).Single();
            XElement? tagTopics = childrenTag.Elements(TAG_TOPICS)
                ?.FirstOrDefault(elem => elem.Attribute(ATTR_TYPE)?.Value == typeName);
            if (tagTopics is null)
            {
                tagTopics = DOMUtils.CreateElement(childrenTag, TAG_TOPICS);
                tagTopics.SetAttributeValue(ATTR_TYPE, typeName);
            }
            var es = DOMUtils.GetChildElementsByTag(tagTopics, TAG_TOPIC).ToList();
            if (index >= 0 && index < es.Count)
            {
                es[index].AddBeforeSelf(childTopic.Implementation);
            }
            else
            {
                tagTopics.Add(childTopic.Implementation);
            }
        }
        public T GetAdapter<T>(Type adapter)
        {
            throw new NotImplementedException();
        }
    }
}
