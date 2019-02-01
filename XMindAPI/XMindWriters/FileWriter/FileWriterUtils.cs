using System.Collections.Generic;
using System.Linq;
namespace XMindAPI.Writers.Util
{
    public static class FileWriterUtils
    {

        public static IXMindWriter ResolveManifestFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var fileLabel = "output:definition:manifest";
            var writerFound = context.ResolveWriterByOutputName(writers, fileLabel);
            return writerFound;
        }
        public static IXMindWriter ResolveMetaFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var fileLabel = "output:definition:meta";
            var writerFound = context.ResolveWriterByOutputName(writers, fileLabel);
            return writerFound;
        }

        public static IXMindWriter ResolveContentFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var fileLabel = "output:definition:content";
            var writerFound = context.ResolveWriterByOutputName(writers, fileLabel);
            return writerFound;
        }
        private static IXMindWriter ResolveWriterByOutputName(
            this XMindWriterContext context,
            List<IXMindWriter> writers,
            string fileLabel)
        {
            var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            var file = xMindSettings[fileLabel];
            var writerFound = writers
                .Where(w => context.FileName.Equals(file) && w.GetOutputConfig().OutputName.Equals(file))
                .FirstOrDefault();
            return writerFound;
        }
    }
}