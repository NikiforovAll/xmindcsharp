using System.IO;
using System.Xml.Linq;
using XMindAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XMindAPI.Writers
{
    public class InMemoryWriter : IXMindWriter<IXMindWriterOutputConfig>
    {
        private IXMindWriterOutputConfig _output;

        public Dictionary<string, XDocument> DocumentStorage { get; } = new Dictionary<string, XDocument>();

        public InMemoryWriter() : this(new FileWriterOutputConfig("root"))
        {
        }
        public InMemoryWriter(IXMindWriterOutputConfig output) => _output = output;

        public Task WriteToStorage(XDocument document, string file)
        {
            DocumentStorage.Add(file, document);
            return Task.CompletedTask;
        }

        public IXMindWriterOutputConfig GetOutputConfig()
        {
            return _output;
        }

        public IXMindWriter<IXMindWriterOutputConfig> SetOutput(IXMindWriterOutputConfig output)
        {
            _output = output;
            return this;
        }
    }
}
