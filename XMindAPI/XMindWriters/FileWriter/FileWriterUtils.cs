using System.Collections.Generic;
using System.Linq;
namespace XMindAPI.Writers.Util
{
    public class FileWriterUtils
    {
        public static IXMindWriter ResolveManifestFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var file = "manifest.xml";
            var writerFound = writers
                .Where(w => context.FileName.Equals(file) && w.GetOutputConfig().OutputName.Equals(file))
                .FirstOrDefault();
            return writerFound;
        }
        public static IXMindWriter ResolveOtherFiles(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var file = "manifest.xml";
            if (context.FileName == file)
                return null;
            var writerFound = writers
                .Where(w => w.GetOutputConfig().OutputName != file)
                .FirstOrDefault();
            return writerFound;
        }
    }
}