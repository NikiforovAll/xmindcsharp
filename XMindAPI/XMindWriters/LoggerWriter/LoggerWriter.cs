using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMindAPI.Writers {
    public class LoggerWriter : IXMindWriter<IXMindWriterOutputConfig> {
        public LoggerWriter () { }

        public IXMindWriterOutputConfig? Output { get; set; }

        public Task WriteToStorage (XDocument document, string file) {
            throw new NotImplementedException ("Logger output is not implemented");
            // Logger.Info(
            //     $"IXMindWriter<LoggerWriter>, OutputName: {Output.OutputName}{System.Environment.NewLine} fileName {file} {System.Environment.NewLine}{document.ToString()}");
        }

        public IXMindWriterOutputConfig GetOutputConfig () {
            return Output;
        }

        public IXMindWriter<IXMindWriterOutputConfig> SetOutput(IXMindWriterOutputConfig output)
        {
            Output = output;
            return this;
        }
    }
}
