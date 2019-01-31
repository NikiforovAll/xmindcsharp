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

        private IXMindWriterOutput _output;

        public Dictionary<string, XDocument> DocumentStorage { get => _documentStorage; private set => _documentStorage = value; }

        public InMemoryWriter() : this(new FileWriterOutput("root"))
        {
        }

        public InMemoryWriter(IXMindWriterOutput output)
        {
            SetOutputName(output);
        }
        public IXMindWriter SetOutputName(IXMindWriterOutput output)
        {
            _output = output;
            return this;
        }

        public void WriteToStorage(XDocument document, string file)
        {
            DocumentStorage.Add(file, document);
        }
    }
}