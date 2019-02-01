namespace XMindAPI.Writers
{
    public class InMemoryWriterOutputConfig : IXMindWriterOutputConfig
    {

        public string OutputName { get; set; }

        public InMemoryWriterOutputConfig(string outputName)
        {
            OutputName = outputName;
        }
    }
}