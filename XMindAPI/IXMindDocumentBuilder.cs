using System.Xml.Linq;

internal interface IXMindDocumentBuilder
{
    XDocument CreateMetaFile();
    XDocument CreateManifestFile();

    XDocument CreateContentFile();

    XDocument MetaFile { get; }
    XDocument ManifestFile { get; }
    XDocument ContentFile { get;  }


}