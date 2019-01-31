using System.IO;
using System.Xml.Linq;
using XMindAPI;
using XMindAPI.Logging;

namespace XMindAPI.Writers
{
    public class LoggerWriter : IXMindWriter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IXMindWriterOutputConfig _output;

        public LoggerWriter() : this(new FileWriterOutputConfig("root"))
        {
        }

        public LoggerWriter(IXMindWriterOutputConfig output)
        {
            SetOutput(output);
        }

        public IXMindWriterOutputConfig Output { get => _output; set => _output = value; }

        public IXMindWriter SetOutput(IXMindWriterOutputConfig output)
        {
            Output = output;
            return this;
        }

        public void WriteToStorage(XDocument document, string file)
        {
            Logger.Info(
                $"IXMindWriter<LoggerWriter>, OutputName: {Output.OutputName}{System.Environment.NewLine} fileName {file} {System.Environment.NewLine}{document.ToString()}");
        }

        public IXMindWriterOutputConfig GetOutputConfig()
        {
            return _output;
        }
    }
}