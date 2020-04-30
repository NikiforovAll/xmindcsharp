using System;
using System.Xml.Linq;
using XMindAPI.Core;
using XMindAPI.Core.DOM;

namespace XMindAPI.Models
{
    internal class XMindRelationship : IRelationship
    {
        public XMindRelationship(XElement implementation, XMindWorkBook book)
        {
            OwnedWorkbook = book;
            Implementation = DOMUtils.AddIdAttribute(implementation);
        }

        public XElement Implementation { get; }
        public IRelationshipEnd End1 { get; set; }
        public IRelationshipEnd End2 { get; set ; }
        public ISheet OwnedSheet { get; set ; }
        public IWorkbook OwnedWorkbook { get; set; }

        public T GetAdapter<T>(Type adapter)
        {
            throw new NotImplementedException();
        }

        public string GetId()
        {
            throw new System.NotImplementedException();
        }

        public ISheet GetParent()
        {
            throw new NotImplementedException();
        }

        public string GetTitle()
        {
            throw new NotImplementedException();
        }

        public bool HasTitle()
        {
            throw new NotImplementedException();
        }

        public void SetTitle(string value)
        {
            throw new NotImplementedException();
        }
    }
}
