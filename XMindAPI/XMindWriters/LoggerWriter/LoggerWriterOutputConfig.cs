namespace XMindAPI.Writers
{
    public class LoggerWriterOutputConfig : IXMindWriterOutputConfig
    {

        public string OutputName { get; set; }

        public LoggerWriterOutputConfig(string outputName)
        {
            OutputName = outputName;
        }
    }
}