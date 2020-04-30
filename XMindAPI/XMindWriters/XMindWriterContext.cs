using System.Collections.Generic;
using System.Xml.Linq;

namespace XMindAPI.Writers
{
    public class XMindWriterContext
    {
        /// <summary>
        /// Provided <see cref="XDocument"/> collection
        /// </summary>
        /// <value></value>
        public IEnumerable<XDocument>? FileEntries { get; set; }
        /// <summary>
        /// Destination file name
        /// </summary>
        /// <value></value>
        public string? FileName { get; set; }
    }
}
