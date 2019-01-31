namespace XMindAPI.Writers
{
    public class FileWriterOutput : IXMindWriterOutput
    {
        private string _path;
        public string OutputName { get; set; }
        public string Path { get => _path; private set => _path = value; }

        public FileWriterOutput(string outputName)
        {
            OutputName = outputName;
        }

        public IXMindWriterOutput SetBasePath(string path)
        {
            Path = path;
            return this;
        }
    }
}