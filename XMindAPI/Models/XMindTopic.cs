using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        public ISheet OwnedSheet { get; set; }
        public TopicType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsFolded { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IList<ITopic> Children { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddLabel(string label)
        {
            throw new NotImplementedException();
        }

        public void AddLabel(int label)
        {
            throw new NotImplementedException();
        }

        public T GetAdapter<T>(Type adapter)
        {
            throw new NotImplementedException();
        }

        public string GetId()
        {
            return Implementation.Attribute(ATTR_ID).Value;
        }

        public HashSet<string> GetLabels()
        {
            throw new NotImplementedException();
        }

        public string GetTitle()
        {
            return DOMUtils.GetTextContentByTag(Implementation, TAG_TITLE);
        }

        public bool HasTitle() => !string.IsNullOrWhiteSpace(GetTitle());

        public void RemoveAllLabels()
        {
            throw new NotImplementedException();
        }

        public void RemoveLabel(string label)
        {
            throw new NotImplementedException();
        }

        public void SetLabels(ICollection<string> labels)
        {
            throw new NotImplementedException();
        }

        public void SetTitle(string value)
        {
            DOMUtils.SetText(Implementation, TAG_TITLE, value);
        }


        public override int GetHashCode()
        {
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
            var typeKey = "type";
            DOMUtils.EnsureChildElement(Implementation, TAG_CHILDREN);
            var childrenTag = Implementation.Descendants("children")
                .Single();
            XElement? tagTopics = childrenTag.Descendants(TAG_TOPICS)
                ?.FirstOrDefault(elem => elem.Attribute(typeKey)?.Value == typeName);
            if (tagTopics is null)
            {
                tagTopics = DOMUtils.CreateElement(childrenTag, TAG_TOPICS);
                tagTopics.SetAttributeValue(typeKey, typeName);
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
    }

}
