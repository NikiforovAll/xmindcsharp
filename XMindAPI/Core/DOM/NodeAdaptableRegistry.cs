using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static XMindAPI.Core.DOM.DOMConstants;

namespace XMindAPI.Core.DOM
{
    internal class NodeAdaptableRegistry
    {
        private readonly XDocument _defaultDocument;
        private readonly INodeAdaptableFactory _factory;
        private readonly Dictionary<IDKey, IAdaptable> _idMap = new Dictionary<IDKey, IAdaptable>();
        private readonly Dictionary<XNode, IAdaptable> _nodeMap = new Dictionary<XNode, IAdaptable>();

        // private IDKey _key = new IDKey();

        public NodeAdaptableRegistry(XDocument defaultDocument, INodeAdaptableFactory factory)
        {
            _defaultDocument = defaultDocument;
            _factory = factory;
        }

        public IAdaptable? GetAdaptableById(String id, XDocument document)
        {
            // Logger.Info($"NodeAdaptableRegistry.GetAdaptableById: Getting element by Id: {id}");
            if (!_idMap.TryGetValue(CreateIDKey(id, document), out var result))
                return null;
            return result;
        }
        public IAdaptable GetAdaptableByNode(XNode node)
        {
            // Logger.Info($"NodeAdaptableRegistry.GetAdaptableById: Getting element by Node: {node.NodeType}");
            return _nodeMap[node];
        }
        public IAdaptable? GetAdaptable(string id) => GetAdaptable(id, _defaultDocument);

        public IAdaptable? GetAdaptable(string id, XDocument document)
        {
            IAdaptable? a = GetAdaptableById(id, document);
            if (a == null)
            {
                XElement element = DOMUtils.GetElementById(document, id);
                if (element != null)
                {
                    a = GetAdaptableByNode(element);
                    if (a == null)
                    {
                        a = _factory.CreateAdaptable(element);
                    }
                    if (a != null)
                    {
                        RegisterByNode(a, element);
                        RegisterById(a, id, document);
                    }
                }
            }
            return a;
        }
        public IAdaptable? GetAdaptable(XNode node)
        {
            if (node is null)
                return null;
            if (!_nodeMap.TryGetValue(node, out IAdaptable? a))
            {
                a = _factory.CreateAdaptable(node);
                if (a != null)
                {
                    RegisterByNode(a, node);
                    var id = GetId(node);
                    if (id != null)
                    {
                        RegisterById(a, id, node.Document);
                    }
                }
            }
            return a;
        }
        public void RegisterById(IAdaptable adaptable, string id, XDocument document)
        {
            // Logger.Info($"NodeAdaptableRegistry.RegisterById: item was registered {adaptable}");
            _idMap.Add(CreateIDKey(id, document), adaptable);
        }

        public void UnregisterById(IAdaptable adaptable, string id, XDocument document)
        {
            IDKey key = CreateIDKey(id, document);
            if (_idMap.TryGetValue(key, out IAdaptable a) && a.Equals(adaptable))
            {
                // Logger.Info($"NodeAdaptableRegistry.UnregisterById: item was unregistered {adaptable}");
                _idMap.Remove(key);
            }
        }

        public void RegisterByNode(IAdaptable adaptable, XNode node)
        {
            // Logger.Info($"NodeAdaptableRegistry.RegisterByNode: item was registered {adaptable}");
            _nodeMap.Add(node, adaptable);
        }
        public void UnregisterByNode(IAdaptable adaptable, XNode node)
        {
            IAdaptable a = _nodeMap[node];
            if (a == adaptable || (a != null && a.Equals(adaptable)))
            {
                // Logger.Info($"NodeAdaptableRegistry.UnregisterByNode: item was unregistered {adaptable}");
                _nodeMap.Remove(node);
            }
        }

        public void Register(IAdaptable adaptable, string id)
        {
            Register(adaptable, id, _defaultDocument);
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
            var id = GetId(node);
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
            var id = GetId(node);
            if (id != null)
            {
                UnregisterById(adaptable, id, node.Document);
            }
        }

        private IDKey CreateIDKey(string id, XDocument document)
        {
            return new IDKey(document, id);
        }

        private string? GetId(XNode node)
        {
            if (node.NodeType == System.Xml.XmlNodeType.Element)
            {
                XElement? xElement = node as XElement;
                return xElement?.Attributes(ATTR_ID)
                    ?.FirstOrDefault()?.Value;
            }
            return null;
        }
    }
}
