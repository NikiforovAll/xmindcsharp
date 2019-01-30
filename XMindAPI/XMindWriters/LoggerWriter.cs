using System.IO;
using System.Xml.Linq;
using XMindAPI;
using XMindAPI.Logging;

namespace XMindAPI.Writers
{
    public class LoggerWriter : IXMindWriter
    {
         private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        private IXMindWriterOutput _output;

        public LoggerWriter():this(new FileWriterOutput("root"))
        {
        }

        public LoggerWriter(IXMindWriterOutput output)
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
            Logger.Info(
                $"IXMindWriter<LoggerWriter>, OutputName: {_output.OutputName}{System.Environment.NewLine} fileName {file} {System.Environment.NewLine}{document.ToString()}");
        }
    }
}