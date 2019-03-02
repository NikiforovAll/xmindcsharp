using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static XMindAPI.Core.DOM.DOMConstants;

namespace XMindAPI.Core.DOM
{
    internal class NodeAdaptableRegistry
    {
        private XDocument _defaultDocument;

        private Dictionary<IDKey, IAdaptable> _idMap = new Dictionary<IDKey, IAdaptable>();
        private Dictionary<XNode, IAdaptable> _nodeMap = new Dictionary<XNode, IAdaptable>();

        private IDKey _key = new IDKey();

        public NodeAdaptableRegistry(XDocument defaultDocument)
        {
            this._defaultDocument = defaultDocument;
        }

        public IAdaptable GetAdaptableById(String id, XDocument document)
        {
            return _idMap[getIDKey(id, document)];
        }
        public IAdaptable GetAdaptableByNode(XNode node)
        {
            return _nodeMap[node];
        }
        public IAdaptable GetAdaptable(string id)
        {
            IAdaptable a = GetAdaptable(id, _defaultDocument);
            return a;
        }

        public IAdaptable GetAdaptable(string id, XDocument document)
        {
            IAdaptable a = GetAdaptableById(id, document);
            return a;
        }

        public IAdaptable GetAdaptable(string id, XNode node)
        {
            return _nodeMap[node];
        }

        public void RegisterById(IAdaptable adaptable, string id, XDocument document)
        {
            _idMap.Add(CreateIDKey(id, document), adaptable);
        }

        public void UnregisterById(IAdaptable adaptable, string id, XDocument document)
        {
            IDKey key = getIDKey(id, document);
            if (_idMap.TryGetValue(key, out IAdaptable a) && a.Equals(adaptable))
            {
                _idMap.Remove(key);
            }
        }

        public void RegisterByNode(IAdaptable adaptable, XNode node)
        {
            _nodeMap.Add(node, adaptable);
        }
        public void UnregisterByNode(IAdaptable adaptable, XNode node)
        {
            IAdaptable a = _nodeMap[node];
            if (a == adaptable || (a != null && a.Equals(adaptable)))
            {
                _nodeMap.Remove(node);
            }
        }

        public void Register(IAdaptable adaptable, string id)
        {

        }
        public void Register(IAdaptable adaptable, string id, XDocument document)
        {
            RegisterById(adaptable, id, document);
            XElement element = DOMUtils.GetElementById(document, id);
            if (element != null)
            {
                RegisterByNode(adaptable, element);
            }
        }

        public void Register(IAdaptable adaptable, XNode node)
        {
            RegisterByNode(adaptable, node);
            string id = getId(node);
            if (id != null)
            {
                RegisterById(adaptable, id, node.Document);
            }
        }


        public void Unregister(IAdaptable adaptable, string id)
        {
            Unregister(adaptable, id, _defaultDocument);
        }

        public void Unregister(IAdaptable adaptable, string id, XDocument document)
        {
            UnregisterById(adaptable, id, document);
            XElement element = DOMUtils.GetElementById(document, id);
            if (element != null)
            {
                UnregisterByNode(adaptable, element);
            }
        }

        public void Unregister(IAdaptable adaptable, XNode node)
        {
            UnregisterByNode(adaptable, node);
            String id = getId(node);
            if (id != null)
            {
                UnregisterById(adaptable, id, node.Document);
            }
        }

        private IDKey CreateIDKey(string id, XDocument document)
        {
            return new IDKey() { Id = id, Document = document };
        }

        //TODO: consider to change
        private IDKey getIDKey(string id, XDocument document)
        {
            _key.Id = id;
            _key.Document = document;
            return _key;
        }

        private string getId(XNode node)
        {
            if (node.NodeType == System.Xml.XmlNodeType.Element)
            {
                (node as XElement).Attributes(ATTR_ID).FirstOrDefault();
            }
            return null;
        }
    }
}