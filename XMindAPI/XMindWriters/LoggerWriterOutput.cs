namespace XMindAPI.Writers
{
    public class LoggerWriterOutput : IXMindWriterOutput
    {

        private string _path;
        public string OutputName { get; set; }
        public XMindOutputType Type { get; set ; }

        public LoggerWriterOutput(string outputName, XMindOutputType type)
        {
            OutputName = outputName;
            Type = type;
        }

        public LoggerWriterOutput SetBasePath(string path)
        {
            _path = path;
            return this;
        }
    }
}