using System;
using System.Xml.Linq;

namespace XMindAPI.Writers
{
    public class LoggerWriter : IXMindWriter
    {
        public LoggerWriter() : this(new FileWriterOutputConfig("root"))
        {
        }

        public LoggerWriter(IXMindWriterOutputConfig output)
        {
            SetOutput(output);
        }
        public IXMindWriterOutputConfig Output { get; set; }

        public IXMindWriter SetOutput(IXMindWriterOutputConfig output)
        {
            Output = output;
            return this;
        }

        public void WriteToStorage(XDocument document, string file)
        {
            throw new NotImplementedException("Logger output is not implemented");
            // Logger.Info(
            //     $"IXMindWriter<LoggerWriter>, OutputName: {Output.OutputName}{System.Environment.NewLine} fileName {file} {System.Environment.NewLine}{document.ToString()}");
        }

        public IXMindWriterOutputConfig GetOutputConfig()
        {
            return Output;
        }
    }
}
