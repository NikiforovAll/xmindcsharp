using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XMindAPI.Core;
using XMindAPI.Core.DOM;
using static XMindAPI.Core.DOM.DOMConstants;


namespace XMindAPI.Models
{
    public class XMindSheet : ISheet
    {
        private readonly XElement implementation;

        public string GetId()
        {
            return implementation.Attribute(ATTR_ID).Value;
        }

        public string GetTitle()
        {
            return DOMUtils.GetTextContentByTag(implementation, TAG_TITLE);
        }

        public void SetTitle(string value)
        {
            DOMUtils.SetText(implementation, TAG_TITLE,value);
        }

        public IWorkbook OwnedWorkbook { get; set; }

        public XElement Implementation => implementation;

        public XMindSheet(XElement implementation, XMindWorkBook book)
        {
            this.OwnedWorkbook = book;
            this.implementation = DOMUtils.AddIdAttribute(implementation);
            implementation.Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
        }
        public void AddRelationship(IRelationship relationship)
        {
            throw new NotImplementedException();
        }

        public T GetAdapter<T>(Type adapter)
        {
            throw new NotImplementedException();
        }

        public HashSet<IRelationship> GetRelationships()
        {
            throw new NotImplementedException();
        }

        public ITopic GetRootTopic()
        {
            throw new NotImplementedException();
        }

        public bool HasTitle()
        {
            return !String.IsNullOrEmpty(GetTitle());
        }

        public void RemoveRelationship(IRelationship relationship)
        {
            throw new NotImplementedException();
        }

        public void ReplaceRootTopic(ITopic newRootTopic)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"SHT# Id:{GetId()} ({GetTitle()})";
        }

        public IWorkbook GetParent()
        {
            XNode node = this.Implementation.Parent;
            if(node == (OwnedWorkbook as XMindWorkBook).GetWorkbookElement())
            {
                return OwnedWorkbook;
            }
            return null;
        }

        public int GetIndex()
        {
            throw new NotImplementedException();
        }
    }

}