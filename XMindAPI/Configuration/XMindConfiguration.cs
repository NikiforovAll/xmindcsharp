//TODO: cyclic dependency with XMindAPI.Configuration and XMindAPI.Writers.Configuraiton;
using XMindAPI.Writers.Configuration;
using XMindAPI.Core.Builders;
using XMindAPI.Models;

namespace XMindAPI.Configuration

{
    public class XMindConfiguration
    {
        public const string ManifestLabel = "output:definition:manifest";
        public const string MetaLabel = "output:definition:meta";
        public const string ContentLabel = "output:definition:content";

        /// <summary>
        /// Configures the write that generated files  will be emitted to.
        /// </summary>
        public XMindWriterConfiguration WriteTo { get; internal set; }

        // public string WorkbookName { get; internal set;}
        public XMindConfiguration()
        {
            WriteTo = new XMindWriterConfiguration(this);
        }

        public XMindWorkBook InitializeWorkBook(string workbookName, string sourceFileName, bool loadContent = true)
        {
            // WorkbookName = workbookName;
            // could be replaced with factory method
            IXMindDocumentBuilder builder = loadContent
                ? new XMindFileDocumentBuilder(sourceFileName)
                : new XMindDocumentBuilder();
            var workbook = new XMindWorkBook(this, builder);
            // Logger.Info($"Workbook was created: {workbook}");
            return workbook;
        }

        public XMindWorkBook CreateWorkBook(string workbookName)
        {
            return InitializeWorkBook(workbookName, string.Empty, false);
        }
    }
}
