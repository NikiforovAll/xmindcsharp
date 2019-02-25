using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Extensions.Configuration;

using XMindAPI.Logging;
using XMindAPI.Configuration;
namespace XMindAPI.Core.Builders
{
    internal class XMindDocumentBuilder : IXMindDocumentBuilder
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        protected readonly IConfiguration xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;

        protected XDocument manifestData = null;
        protected XDocument metaData = null;
        protected XDocument contentData = null;

        public XDocument MetaFile { get => metaData; }
        public XDocument ManifestFile { get => manifestData;}
        public XDocument ContentFile { get => contentData; }

        public XMindDocumentBuilder()
        {
        }
        public virtual XDocument CreateMetaFile()
        {
            this.metaData = CreateDefaultMetaFile();
            return this.metaData;
        }

        public virtual XDocument CreateManifestFile()
        {
            this.manifestData = CreateDefaultManifestFile();
            return this.manifestData;
        }

        public virtual XDocument CreateContentFile()
        {
            this.contentData = CreateDefaultContentFile();
            return this.contentData;
        }

        private XDocument CreateDefaultMetaFile()
        {
            XDocument metaFile = new XDocument();
            metaFile.Declaration = new XDeclaration("1.0", "UTF-8", "no");
            metaFile.Add(
                new XElement(
                    XNamespace.Get(xMindSettings["metaNamespace"]) + "meta",
                    new XAttribute("version", "2.0")
                )
            );
            return metaFile;
        }

        private XDocument CreateDefaultManifestFile()
        {
            var files = XMindConfigurationCache
                .Configuration
                .GetOutputFilesDefinitions();
            var fileLocations = XMindConfigurationCache
                .Configuration
                .GetOutputFilesLocations();
            var manifest = new XDocument();
            manifest.Declaration = new XDeclaration("1.0", "UTF-8", "no");
            var manifestNamespace = XNamespace.Get(xMindSettings["manifestNamespace"]);
            var manifestFileEntryToken = manifestNamespace + "file-entry";
            XElement rootElement = new XElement(manifestNamespace + "manifest");
            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", files[XMindConfigurationCache.ContentLabel]),
                    new XAttribute("media-type", "text/xml")
                ));

            var manifestFileName = files[XMindConfigurationCache.ManifestLabel];
            var manifestFilePath = fileLocations[manifestFileName];
            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", manifestFilePath),
                    new XAttribute("media-type", "")
                ));

            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", Path.Combine(manifestFilePath, manifestFileName)),
                    new XAttribute("media-type", "text/xml")
                ));

            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", "Thumbnails/"),
                    new XAttribute("media-type", "")
                ));

            manifest.Add(rootElement);
            return manifest;
        }

        private XDocument CreateDefaultContentFile()
        {
            var content = new XDocument();
            XNamespace ns2 = XNamespace.Get(xMindSettings["standardContentNamespaces:xsl"]);
            XNamespace ns3 = XNamespace.Get(xMindSettings["standardContentNamespaces:svg"]);
            XNamespace ns4 = XNamespace.Get(xMindSettings["standardContentNamespaces:xhtml"]);

            content.Add(new XElement(
                XNamespace.Get(xMindSettings["contentNamespace"]) + "xmap-content",
                new XAttribute(XNamespace.Xmlns + "fo", ns2),
                new XAttribute(XNamespace.Xmlns + "svg", ns3),
                new XAttribute(XNamespace.Xmlns + "xhtml", ns4),
                new XAttribute(XNamespace.Xmlns + "xlink", XNamespace.Get(xMindSettings["xlinkNamespace"])),
                new XAttribute("version", "2.0")
            ));
            return content;
        }

        public void AddSheet()
        {
            throw new NotImplementedException();
        }
    }
}