//TODO: cyclic dependency with XMindAPI.Configuration and XMindAPI.Writers.Configuraiton;
using XMindAPI.Writers.Configuration;
using XMindAPI.Core.Builders;
using XMindAPI.Models;
using XMindAPI.Logging;

namespace XMindAPI.Configuration

{
    public class XMindConfiguration
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        /// <summary>
        /// Configures the write that generated files  will be emitted to.
        /// </summary>
        public XMindWriterConfiguration WriteTo { get; internal set; }

        public string WorkbookName { get; internal set;}
        public XMindConfiguration()
        {
            WriteTo = new XMindWriterConfiguration(this);
        }

        public XMindWorkBook CreateWorkBook(string workbookName, string sourceFileName, bool loadContent = true)
        {
            WorkbookName = workbookName;
            // could be replaced with factory method
            IXMindDocumentBuilder builder = loadContent ? new XMindFileDocumentBuilder(sourceFileName) : new XMindDocumentBuilder();
            var workbook = new XMindWorkBook(this, XMindConfigurationCache.Configuration, builder);
            Logger.Info($"Workbook was created: {workbook}");
            return workbook;
        }

        public XMindWorkBook CreateWorkBook(string workbookName)
        {
            return CreateWorkBook(workbookName, string.Empty, false);
        }
    }
}