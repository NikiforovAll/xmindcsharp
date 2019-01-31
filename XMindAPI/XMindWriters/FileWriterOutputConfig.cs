namespace XMindAPI.Writers
{
    public class FileWriterOutputConfig : IXMindWriterOutputConfig
    {
        private string _path;
        public string OutputName { get; set; }
        public string Path { get => _path; private set => _path = value; }

        public FileWriterOutputConfig(string outputName)
        {
            OutputName = outputName;
        }

        public IXMindWriterOutputConfig SetBasePath(string path)
        {
            Path = path;
            return this;
        }
    }
}