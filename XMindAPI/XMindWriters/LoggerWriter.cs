using System.IO;
using XMindAPI;

namespace XMindAPI.Writers
{
    public class LoggerWriter : IXMindWriter
    {
        private IXMindWriterOutput _output;

        public LoggerWriter():this(new LoggerWriterOutput("root", XMindAPI.XMindOutputType.Log))
        {
            
        }

        public LoggerWriter(IXMindWriterOutput output)
        {
            this._output = output;
        }
        public void SetOutputName(IXMindWriterOutput output)
        {
            throw new System.NotImplementedException();
        }

        public void WriteToStorage(Stream stream)
        {
            //TODO: check - Convert XDocument to MemoryStream
            // https://stackoverflow.com/questions/750198/convert-xdocument-to-stream/11672647
            using (StreamWriter sw = new StreamWriter(stream))
            {
                // Log.Information(sw.ToString());
            }
        }
    }
}