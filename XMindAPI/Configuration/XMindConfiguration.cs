using System;
using System.IO;
//TODO: cyclic dependency with XMindAPI.Configuration and XMindAPI.Writers.Configuraiton;
using XMindAPI.Writers.Configuration;
using XMindAPI.Core.Builders;
using XMindAPI.Models;
using System.Threading.Tasks;

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

        public XMindWorkBook LoadWorkBookFromLocation(string sourceFileLocation)
        {
            // WorkbookName = workbookName;
            // could be replaced with factory method
            var fi = new FileInfo(sourceFileLocation);
            if (!fi.Exists)
            {
                throw new FileNotFoundException($"{nameof(sourceFileLocation)} is invalid");
            }
            var workbook = new XMindWorkBook(fi.Name, this, new XMindFileDocumentBuilder(sourceFileLocation));
            return workbook;
        }

        public XMindWorkBook CreateWorkBook(string workbookName) =>
            new XMindWorkBook(workbookName, this, new XMindDocumentBuilder());
    }
}
