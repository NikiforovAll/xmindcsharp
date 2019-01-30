using System.Collections.Generic;
using System.Xml.Linq;

namespace XMindAPI.Writers
{
    public class XMindWriterContext
    {
        public IEnumerable<XDocument> FileEntries { get; set; }
    }
}