namespace XMindAPI.Writers
{
    public class FileWriterOutput : IXMindWriterOutput
    {
        private string _path;
        public string OutputName { get; set; }

        public FileWriterOutput(string outputName)
        {
            OutputName = outputName;
        }

        public IXMindWriterOutput SetBasePath(string path)
        {
            _path = path;
            return this;
        }
    }
}