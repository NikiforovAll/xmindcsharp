using System.Xml.Linq;

namespace XMindAPI.Writers
{
    public interface IXMindWriter
    {
        void WriteToStorage(XDocument document, string fileName);
        IXMindWriter SetOutput(IXMindWriterOutputConfig output);
        IXMindWriterOutputConfig GetOutputConfig();
    }
}