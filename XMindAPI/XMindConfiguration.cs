//TODO: cyclic dependency with XMindAPI.Configuration and XMindAPI.Writers.Configuraiton;
using XMindAPI.Writers.Configuration;

namespace XMindAPI.Configuration

{
    public class XMindConfiguration
    {
        /// <summary>
        /// Configures the write that generated files  will be emitted to.
        /// </summary>
        public XMindWriterConfiguration WriteTo { get; internal set; }
        public XMindConfiguration()
        {
            WriteTo = new XMindWriterConfiguration(this);
        }

        public XMindWorkBook CreateWorkBook(string fileName, bool loadContent)
        {
            return new XMindWorkBook(fileName, loadContent, this);
        }

        public XMindWorkBook CreateWorkBook(string fileName)
        {
            return CreateWorkBook(fileName, false);
        }
    }
}