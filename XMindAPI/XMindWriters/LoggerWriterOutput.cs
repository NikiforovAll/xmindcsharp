namespace XMindAPI.Writers
{
    public class LoggerWriterOutput : IXMindWriterOutput
    {

        public string OutputName { get; set; }

        public LoggerWriterOutput(string outputName)
        {
            OutputName = outputName;
        }
    }
}