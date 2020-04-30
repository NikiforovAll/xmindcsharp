using System;
using System.IO;
//TODO: cyclic dependency with XMindAPI.Configuration and XMindAPI.Writers.Configuraiton;
using XMindAPI.Writers.Configuration;
using XMindAPI.Core.Builders;
using XMindAPI.Models;
using System.Threading.Tasks;

namespace XMindAPI

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

        /// <summary>
        /// Loads workbook from location. WARNING: currently, book is not de-serialized in XML correctly.
        /// </summary>
        /// <param name="sourceFileLocation">Path to <i>.xmind</i> file</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates new workbook, standard XML assets are generated in-memory.
        /// </summary>
        /// <param name="workbookName">The name of <c>XMindWorkBook</c></param>
        /// <returns></returns>
        public XMindWorkBook CreateWorkBook(string workbookName) =>
            new XMindWorkBook(workbookName, this, new XMindDocumentBuilder());
    }
}
