using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XMindAPI.Core;
using XMindAPI.Core.DOM;

namespace XMindAPI.Models
{
    public class XMindSheet : ISheet
    {
        private readonly XElement implementation;

        public string Id => throw new NotImplementedException();

        public string GetTitle()
        {
            throw new NotImplementedException();
        }

        public void SetTitle(string value)
        {
            throw new NotImplementedException();
        }

        public IWorkbook OwnedWorkbook { get; set ; }

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
    }

}