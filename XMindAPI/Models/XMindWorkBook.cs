//TODO: cyclic dependency with XMindAPI.Configuration and XMindAPI.Writers.Configuration;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMindAPI.Configuration;
using XMindAPI.Core;
using XMindAPI.Core.Builders;
using XMindAPI.Core.DOM;
using XMindAPI.Infrastructure;
using XMindAPI.Infrastructure.Logging;
using XMindAPI.Writers;

using static XMindAPI.Core.DOM.DOMConstants;

namespace XMindAPI.Models
{
    /// <summary>
    /// XMindWorkBook encapsulates an XMind workbook and methods for performing actions on workbook content.
    /// </summary>
    public class XMindWorkBook : AbstractWorkbook, INodeAdaptableFactory//IWorkbook
    {
        // https://github.com/xmindltd/xmind/wiki/UsingXmindAPI
        // TODO: add IO
        // void save(OutputStream output);  // save the workbook to the specified OutputStream
        public string Name { get; set; }
        private readonly XMindConfiguration _bookConfiguration;
        private readonly IXMindDocumentBuilder _documentBuilder;

        private readonly NodeAdaptableRegistry _adaptableRegistry;
        internal readonly IConfiguration _xMindSettings;

        private readonly XElement _implementation;

        // private string _fileName = null;

        /// <summary>
        /// Creates a new <see cref="XMindWorkBook"/> if loadContent is false, otherwise the file content will be loaded.
        /// </summary>
        /// <param name="name">Name of the book</param>
        /// <param name="bookConfiguration">Book configuration</param>
        /// <param name="builder">Builder to write based on <see cref="XMindConfiguration"/></param>
        internal XMindWorkBook(string name, XMindConfiguration bookConfiguration, IXMindDocumentBuilder builder)
        {
            Guard.Against.Null(XMindConfigurationLoader.Configuration.XMindConfigCollection, "XMindConfigCollection");

            Name = name;
            _xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            _bookConfiguration = bookConfiguration;
            _documentBuilder = builder;

            _documentBuilder.CreateMetaFile();
            _documentBuilder.CreateManifestFile();
            _documentBuilder.CreateContentFile();

            _implementation = _documentBuilder.ContentFile.Descendants().First();
            _adaptableRegistry = new NodeAdaptableRegistry(_documentBuilder.ContentFile, this);
            //Create default sheet if needed
            //TODO:
            if (DOMUtils.GetFirstElementByTagName(_implementation, TAG_SHEET) == null)
            {
                AddSheet(CreateSheet());
            }
        }

        public override T GetAdapter<T>(Type adapter)
        {
            //TODO: this is point of extension for all adaptees
            // if (IStorage.class.equals(adapter))
            //     return adapter.cast(getStorage());
            // if (IEntryStreamNormalizer.class.equals(adapter))
            //     return adapter.cast(manifest.getStreamNormalizer());
            // if (ICoreEventSource.class.equals(adapter))
            //     return adapter.cast(this);
            // if (adapter.isAssignableFrom(Document.class))
            //     return adapter.cast(implementation);
            // if (adapter.isAssignableFrom(Element.class))
            //     return adapter.cast(getWorkbookElement());
            // if (IMarkerSheet.class.equals(adapter))
            //     return adapter.cast(getMarkerSheet());
            // if (IManifest.class.equals(adapter))
            //     return adapter.cast(getManifest());
            // if (ICoreEventSupport.class.equals(adapter))
            //     return adapter.cast(getCoreEventSupport());
            // if (INodeAdaptableFactory.class.equals(adapter))
            //     return adapter.cast(this);
            // if (INodeAdaptableProvider.class.equals(adapter))
            //     return adapter.cast(getAdaptableRegistry());
            // if (IMarkerRefCounter.class.equals(adapter))
            //     return adapter.cast(getMarkerRefCounter());
            // if (IStyleRefCounter.class.equals(adapter))
            //     return adapter.cast(getStyleRefCounter());
            // if (IWorkbookComponentRefManager.class.equals(adapter))
            //     return adapter.cast(getElementRefCounter());
            // if (IRevisionRepository.class.equals(adapter))
            //     return adapter.cast(getRevisionRepository());
            // if (IWorkbookExtensionManager.class.equals(adapter))
            //     return adapter.cast(getWorkbookExtensionManager());
            return base.GetAdapter<T>(adapter);
        }

        /// <summary>
        /// Save the current XMind workbook file to disk.
        /// </summary>
        public override async Task Save()
        {
            var requestId = $"SaveWorkBook-{SmallGuidGenerator.NewGuid()}";
            Logger.Log.RequestStart(requestId);
            var manifestFileName = _xMindSettings[XMindConfiguration.ManifestLabel];
            var metaFileName = _xMindSettings[XMindConfiguration.MetaLabel];
            var contentFileName = _xMindSettings[XMindConfiguration.ContentLabel];

            var files = new Dictionary<string, XDocument>(3)
            {
                [metaFileName] = _documentBuilder.MetaFile,
                [manifestFileName] = _documentBuilder.ManifestFile,
                [contentFileName] = _documentBuilder.ContentFile
            };
            var writerJobs = new List<Task>(3);
            var writerContexts = new List<XMindWriterContext>();
            foreach (var kvp in files)
            {
                var currentWriterContext = new XMindWriterContext()
                {
                    FileName = kvp.Key,
                    FileEntries = new XDocument[1] { kvp.Value }
                };
                var selectedWriters = _bookConfiguration
                    .WriteTo
                    .ResolveWriters(currentWriterContext);
                if (selectedWriters == null)
                {
                    var errorMessage = "XMindBook.Save: Writer is not selected";
                    Logger.Log.Error(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
                foreach (var writer in selectedWriters)
                {
                    writerJobs.Add(writer.WriteToStorage(kvp.Value, kvp.Key));
                }
                writerContexts.Add(currentWriterContext);
            }
            try
            {
                await Task.WhenAll(writerJobs);
                Logger.Log.RequestPhase(requestId, "WritersCompleted");
                _bookConfiguration.WriteTo.FinalizeAction?.Invoke(writerContexts, this);
                Logger.Log.RequestPhase(requestId, "FinalizerExecuted");
            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);
                throw;
            }
            finally
            {
                Logger.Log.RequestStop(requestId);
            }
        }

        public override IRelationship CreateRelationship(
            IRelationshipEnd rel1, IRelationshipEnd rel2)
        {
            ISheet sheet = rel1.OwnedSheet;
            IRelationship rel = CreateRelationship();
            rel.End1 = rel1;
            rel.End2 = rel2;
            sheet.AddRelationship(rel);
            return rel;
        }

        public override IRelationship CreateRelationship()
        {
            var relationshipElement = new XElement(TAG_RELATIONSHIP);
            var relationship = new XMindRelationship(relationshipElement, this);
            _adaptableRegistry.RegisterByNode(relationship, relationship.Implementation);
            return relationship;
        }

        public override ISheet CreateSheet()
        {
            var sheetElement = new XElement(TAG_SHEET);
            var sheet = new XMindSheet(sheetElement, this);
            _adaptableRegistry.RegisterByNode(sheet, sheet.Implementation);
            return sheet;
        }

        public override void AddSheet(ISheet sheet, int index)
        {
            Logger.Log.DebugTrace($"Add sheet {sheet.GetId()} to {Name}");
            if (!(sheet is XMindSheet impl) || impl.Implementation is null)
            {
                const string errorMessage = "XMindWorkbook.AddSheet: sheet is not correct";
                Logger.Log.Error(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            XElement elementImplementation = impl.Implementation;
            var bookImplementation = GetWorkbookElement();
            if (elementImplementation.Parent is object
                && elementImplementation.Parent != bookImplementation)
            {
                const string errorMessage = "XMindWorkbook.AddSheet: sheet must belong to same document";
                Logger.Log.Error(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            var childElements = DOMUtils.GetChildElementsByTag(bookImplementation, TAG_SHEET);
            if (index >= 0 && index < childElements.Count())
            {
                childElements.Where((e, i) => i == index)
                    .First()
                    .AddBeforeSelf(elementImplementation);
            }
            else
            {
                bookImplementation.Add(elementImplementation);
            }
        }

        /// <summary>
        /// Register topic. Note <see cref="ITopic"/> is not included in DOM of <see cref="XMindWorkBook"/>
        /// </summary>
        /// <returns>Registered XMindTopic</returns>
        public override ITopic CreateTopic()
        {
            var topicElement = new XElement(TAG_TOPIC);
            XMindTopic topic = new XMindTopic(topicElement, this)
            {
                OwnedSheet = GetPrimarySheet()
            };
            Logger.Log.DebugTrace($"Register topic {topic.GetId()} for {Name}");
            _adaptableRegistry.RegisterByNode(topic, topic.Implementation);
            return topic;
        }
        /// <summary>
        /// Register topic. Note <see cref="ITopic"/> is not included in DOM of <see cref="XMindWorkBook"/>

        /// <param name="title">Title to set</param>
        /// <returns>Registered XMindTopic</returns>
        public ITopic CreateTopic(string title)
        {
            var topic = CreateTopic();
            topic.SetTitle(title);
            return topic;
        }

        /// <summary>
        /// Finds elements in WorkBook based on registry
        /// </summary>
        /// <param name="id">Unique Id of element</param>
        /// <param name="source"></param>
        /// <returns></returns>
        public override object? FindElement(string id, IAdaptable source)
        {
            XNode node = source.GetAdapter<XNode>(typeof(XNode));
            if (node == null)
            {
                node = GetWorkbookElement();
            }
            return GetAdaptableRegistry()
                ?.GetAdaptable(id, node.Document);
        }

        /// <summary>
        /// Gets primary sheet
        /// </summary>
        /// <returns></returns>
        public override ISheet GetPrimarySheet()
        {
            XElement primarySheet = DOMUtils.GetFirstElementByTagName(GetWorkbookElement(), TAG_SHEET);
            if (primarySheet == null)
            {
                const string errorMessage = "Primary sheet was not found";
                Logger.Log.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            return (ISheet) GetAdaptableRegistry().GetAdaptable(primarySheet)!;
        }

        /// <summary>
        /// Sheets enumerator
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ISheet> GetSheets()
        {
            return DOMUtils.GetChildList<ISheet>(GetWorkbookElement(), TAG_SHEET, GetAdaptableRegistry());
        }
        public override void RemoveSheet(ISheet sheet)
        {
            if (!(sheet is XMindSheet xmindSheet) || xmindSheet.Implementation is null)
            {
                const string errorMessage = "Implementation was not found";
                Logger.Log.Error(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            XElement elementImplementation = xmindSheet.Implementation;
            var bookImplementation = GetWorkbookElement();
            if (elementImplementation.Parent != bookImplementation)
            {
                // Logger.Warn("XMindWorkbook.RemoveSheet: sheet must belong to same document");
            }
            var childElements = DOMUtils
                .GetChildElementsByTag(bookImplementation, TAG_SHEET).ToList();
            childElements
                .FirstOrDefault(el => el == elementImplementation)?
                .Remove();
        }
        public IAdaptable? CreateAdaptable(XNode node)
        {
            IAdaptable? adaptable = null;
            if (node is XElement e)
            {
                XName nodeName = e.Name;
                switch (nodeName.ToString())
                {
                    case TAG_SHEET:
                        adaptable = new XMindSheet(e, this);
                        break;
                    case TAG_TOPIC:
                        adaptable = new XMindTopic(e, this);
                        break;
                }
            }
            if (adaptable is null)
            {
                Logger.Log.Warning($"XMindWorkbook.CreateAdaptable: adaptable was is not created - {adaptable}");
            }
            return adaptable;
        }

        // public override string ToString()
        // {
        //     return $"Workbook# {_globalConfiguration.WorkbookName}";
        // }

        internal NodeAdaptableRegistry GetAdaptableRegistry()
        {
            return _adaptableRegistry;
        }

        internal XElement GetWorkbookElement()
        {
            return _implementation;
        }
    }
}
