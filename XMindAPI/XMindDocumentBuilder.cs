using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using XMindAPI.Logging;

namespace XMindAPI
{
    internal class XMindDocumentBuilder : IXMindDocumentBuilder
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly IConfiguration _xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;

        protected XDocument _manifestData = null;
        protected XDocument _metaData = null;
        protected XDocument _contentData = null;

        public XDocument MetaFile { get => _metaData; }
        public XDocument ManifestFile { get => _manifestData;}
        public XDocument ContentFile { get => _contentData; }

        public XMindDocumentBuilder()
        {
        }
        public virtual XDocument CreateMetaFile()
        {
            this._metaData = CreateDefaultMetaFile();
            return this._metaData;
        }

        public virtual XDocument CreateManifestFile()
        {
            this._manifestData = CreateDefaultManifestFile();
            return this._manifestData;
        }

        public virtual XDocument CreateContentFile()
        {
            this._contentData = CreateDefaultContentFile();
            return this._contentData;
        }

        private XDocument CreateDefaultMetaFile()
        {
            XDocument metaFile = new XDocument();
            metaFile.Declaration = new XDeclaration("1.0", "UTF-8", "no");
            metaFile.Add(
                new XElement(
                    XNamespace.Get(_xMindSettings["metaNamespace"]) + "meta",
                    new XAttribute("version", "2.0")
                )
            );
            return metaFile;
        }

        private XDocument CreateDefaultManifestFile()
        {
            var manifest = new XDocument();
            manifest.Declaration = new XDeclaration("1.0", "UTF-8", "no");
            var manifestNamespace = XNamespace.Get(_xMindSettings["manifestNamespace"]);
            var manifestFileEntryToken = manifestNamespace + "file-entry";
            XElement rootElement = new XElement(manifestNamespace + "manifest");
            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", "content.xml"),
                    new XAttribute("media-type", "text/xml")
                ));

            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", "META-INF/"),
                    new XAttribute("media-type", "")
                ));

            rootElement.Add(
                new XElement(manifestFileEntryToken,
                    new XAttribute("full-path", "META-INF/manifest.xml"),
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
            XNamespace ns2 = XNamespace.Get(_xMindSettings["standardContentNamespaces:xsl"]);
            XNamespace ns3 = XNamespace.Get(_xMindSettings["standardContentNamespaces:svg"]);
            XNamespace ns4 = XNamespace.Get(_xMindSettings["standardContentNamespaces:xhtml"]);

            content.Add(new XElement(
                XNamespace.Get(_xMindSettings["contentNamespace"]) + "xmap-content",
                new XAttribute(XNamespace.Xmlns + "fo", ns2),
                new XAttribute(XNamespace.Xmlns + "svg", ns3),
                new XAttribute(XNamespace.Xmlns + "xhtml", ns4),
                new XAttribute(XNamespace.Xmlns + "xlink", XNamespace.Get(_xMindSettings["xlinkNamespace"])),
                new XAttribute("version", "2.0")
            ));
            return content;
        }
    }
}