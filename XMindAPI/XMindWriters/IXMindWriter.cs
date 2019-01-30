using System.Xml.Linq;

namespace XMindAPI.Writers
{
    public interface IXMindWriter
    {
        void WriteToStorage(XDocument document, string fileName);
        IXMindWriter SetOutputName(IXMindWriterOutput output);
    }
}