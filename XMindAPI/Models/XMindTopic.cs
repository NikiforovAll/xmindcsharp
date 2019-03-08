using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XMindAPI.Core;
using XMindAPI.Core.DOM;
using XMindAPI.Models;
using static XMindAPI.Core.DOM.DOMConstants;

namespace XMindAPI
{

    /// <summary>
    ///  Base element of build XMind maps, topics are added to <see cref="XMindWorkBook"/>
    /// </summary>
    public class XMindTopic : ITopic
    {
        public XMindTopic(XElement implementation, XMindWorkBook book)
        {
            this.OwnedWorkbook = book;
            this.Implementation = DOMUtils.AddIdAttribute(implementation);
        }

        public IWorkbook OwnedWorkbook { get; }
        public XElement Implementation { get; }

        public void AddLabel(string label)
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

        public bool HasTitle()
        {
            throw new NotImplementedException();
        }

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
    }

}