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
    public class XMindSheet : ISheet
    {
        private XMindWorkBook _ownedWorkbook;

        public string GetId()
        {
            return Implementation.Attribute(ATTR_ID).Value;
        }

        public string GetTitle()
        {
            return DOMUtils.GetTextContentByTag(Implementation, TAG_TITLE);
        }

        public void SetTitle(string value)
        {
            DOMUtils.SetText(Implementation, TAG_TITLE, value);
        }

        public IWorkbook OwnedWorkbook
        {
            get => _ownedWorkbook;
            set => _ownedWorkbook = (XMindWorkBook)value;
        }

        public XElement Implementation { get; }

        public XMindSheet(XElement implementation, XMindWorkBook book)
        {
            OwnedWorkbook = book;
            Implementation = DOMUtils.AddIdAttribute(implementation);
            // implementation.Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
            //creates default topic if needed
            DOMUtils.EnsureChildElement(implementation, TAG_TOPIC);

        }
        public void AddRelationship(IRelationship relationship)
        {
            var container = DOMUtils.EnsureChildElement(Implementation, TAG_RELATIONSHIPS);
            if (!(relationship is XMindRelationship rel))
            {
                var errorMessage = "AddRelationship: Not valid relationship";
                Logger.Log.Error(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            container.Add(rel);
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
            XElement rootTopic = DOMUtils.GetFirstElementByTagName(Implementation, TAG_TOPIC);
            return (ITopic)(OwnedWorkbook as XMindWorkBook).GetAdaptableRegistry().GetAdaptable(rootTopic);
        }

        public bool HasTitle()
        {
            return !string.IsNullOrEmpty(GetTitle());
        }

        public void RemoveRelationship(IRelationship relationship)
        {
            throw new NotImplementedException();
        }

        public void ReplaceRootTopic(ITopic newRootTopic)
        {
            XElement rootTopic = (GetRootTopic() as XMindTopic).Implementation;
            rootTopic.AddAfterSelf((newRootTopic as XMindTopic).Implementation);
            rootTopic.Remove();
        }

        public override string ToString()
        {
            return $"SHT# Id:{GetId()} ({GetTitle()})";
        }

        public IWorkbook GetParent()
        {
            XNode node = this.Implementation.Parent;
            if (node == (OwnedWorkbook as XMindWorkBook).GetWorkbookElement())
            {
                return OwnedWorkbook;
            }
            return null;
        }

        public int GetIndex()
        {
            return GetParent().GetSheets().ToList().IndexOf(this);
        }

        public override int GetHashCode()
        {
            return this.Implementation.GetHashCode();
        }
    }

}
