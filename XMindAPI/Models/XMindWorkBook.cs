using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;

using XMindAPI.Configuration;
using XMindAPI.Writers;
using XMindAPI.Logging;
using XMindAPI.Core;
using XMindAPI.Core.Builders;

namespace XMindAPI
{
    /// <summary>
    /// XMindWorkBook encapsulates an XMind workbook and methods for performing actions on workbook content. 
    /// </summary>
    public class XMindWorkBook : AbstractWorkbook//IWorkbook
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly XMindConfiguration _globalConfiguration;
        private readonly IXMindDocumentBuilder _documentBuilder;
        internal readonly XMindConfigurationCache _xMindSettings;


        // private string _fileName = null;


        /// <summary>
        /// Creates a new XMind workbook if loadContent is false, otherwise the file content will be loaded.
        /// </summary>
        // /// <param name="loadContent">If true, the current data from the file will be loaded, otherwise an empty workbook will be created.</param>
        internal XMindWorkBook(XMindConfiguration globalConfiguration, XMindConfigurationCache config, IXMindDocumentBuilder builder)
        {
            _xMindSettings = config;
            _globalConfiguration = globalConfiguration;
            _documentBuilder = builder;
            _documentBuilder.CreateMetaFile();
            _documentBuilder.CreateManifestFile();
            _documentBuilder.CreateContentFile();
            //TODO: use builder in order to work with XDocuments, ideally get rid of XML namespace in workbook
        }


        public T getAdapter<T>(Type adapter)
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
        public override void Save()
        {
            var manifestFileName = _xMindSettings.XMindConfigCollection[XMindConfigurationCache.ManifestLabel];
            var metaFileName = _xMindSettings.XMindConfigCollection[XMindConfigurationCache.MetaLabel];
            var contentFileName = _xMindSettings.XMindConfigCollection[XMindConfigurationCache.ContentLabel];

            var files = new Dictionary<string, XDocument>(3)
            {
                [metaFileName] = _documentBuilder.MetaFile,
                [manifestFileName] = _documentBuilder.ManifestFile,
                [contentFileName] = _documentBuilder.ContentFile
            };

            var writerContexts = new List<XMindWriterContext>();
            foreach (var kvp in files)
            {
                var currentWriterContext = new XMindWriterContext()
                {
                    FileName = kvp.Key,
                    FileEntries = new XDocument[1] { kvp.Value }
                };
                var selectedWriters = _globalConfiguration
                    .WriteTo
                    .ResolveWriters(currentWriterContext);
                if (selectedWriters == null)
                {
                    throw new InvalidOperationException("XMindBook.Save: Writer is not selected");
                }

                foreach (var writer in selectedWriters)
                {
                    writer.WriteToStorage(kvp.Value, kvp.Key);
                }
                writerContexts.Add(currentWriterContext);
            }
            _globalConfiguration.WriteTo.FinalizeAction?.Invoke(writerContexts);
        }

        public override IRelationship CreateRelationship(IRelationship rel1, IRelationship rel2)
        {
            throw new NotImplementedException();
        }

        public override IRelationship CreateRelationship()
        {
            throw new NotImplementedException();
        }

        public override ISheet CreateSheet()
        {
            throw new NotImplementedException();
        }

        public override ITopic CreateTopic()
        {
            throw new NotImplementedException();
        }

        public override object FindElement(string id, IAdaptable source)
        {
            throw new NotImplementedException();
        }

        public override ITopic FindTopic()
        {
            throw new NotImplementedException();
        }

        public override object GetElementById(string id)
        {
            throw new NotImplementedException();
        }

        public override ISheet GetPrimarySheet()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ISheet> GetSheets()
        {
            throw new NotImplementedException();
        }

        public override void RemoveSheet(ISheet sheet)
        {
            throw new NotImplementedException();
        }
    }
}
