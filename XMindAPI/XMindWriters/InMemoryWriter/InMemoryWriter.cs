using System.IO;
using System.Xml.Linq;
using XMindAPI;
using XMindAPI.Logging;
using System.Collections.Generic;

namespace XMindAPI.Writers
{
    public class InMemoryWriter : IXMindWriter
    {
        private readonly Dictionary<string, XDocument> _documentStorage = new Dictionary<string, XDocument>();
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IXMindWriterOutputConfig _output;

        public Dictionary<string, XDocument> DocumentStorage { get => _documentStorage;} 

        public InMemoryWriter() : this(new FileWriterOutputConfig("root"))
        {
        }
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