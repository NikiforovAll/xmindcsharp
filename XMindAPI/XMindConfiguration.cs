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

        public XMindWorkBook CreateWorkBook(string workbookName, string fileName, bool loadContent)
        {
            var workbook = new XMindWorkBook(this, XMindConfigurationCache.Configuration);
            if(loadContent){
                //TODO: add load handling
                // workbook.Load(fileName);
            }
            return workbook;
        }

        public XMindWorkBook CreateWorkBook(string workbookName)
        {
            return CreateWorkBook(workbookName, string.Empty, false);
        }
    }
}