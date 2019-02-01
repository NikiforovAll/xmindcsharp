using System.IO;
using System.Xml.Linq;
using XMindAPI;
using XMindAPI.Logging;
using System.Collections.Generic;

namespace XMindAPI.Writers
{
    public class InMemoryWriter : IXMindWriter
    {
        private Dictionary<string, XDocument> _documentStorage;
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IXMindWriterOutputConfig _output;

        public Dictionary<string, XDocument> DocumentStorage { get => _documentStorage; private set => _documentStorage = value; }

        public InMemoryWriter(IXMindWriterOutputConfig output)
        {
            SetOutput(output);
        }
        public IXMindWriter SetOutput(IXMindWriterOutputConfig output)
        {
            _output = output;
            return this;
        }

        public void WriteToStorage(XDocument document, string file)
        {
            DocumentStorage.Add(file, document);
        }

        public IXMindWriterOutputConfig GetOutputConfig()
        {
            return _output;
        }
    }
}