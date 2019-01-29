using System.IO;

namespace XMindAPI
{
    public interface IXMindWriter
    {
        void WriteToStorage(Stream stream);
        void SetOutputName(IXMindWriterOutput output);
    }
}