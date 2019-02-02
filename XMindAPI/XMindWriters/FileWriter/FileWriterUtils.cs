using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
using System;
using System.IO;

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


        public static Action<List<XMindWriterContext>> ZipXMindFolder(string xmindFileNane)
        {
            var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            var filesToZipLabels = new string[] { "output:definition:meta", "output:definition:manifest", "output:definition:content" };
            var filesToZip = xMindSettings
                .GetSection("output:files")
                .GetChildren()
                .Where(token => xMindSettings.GetSection("output:definition").GetChildren().Select(x => x.Value).Contains(token.Value))
                .Select(token =>
                {
                    var fileName = token["name"];
                    var fileLocation = token["location"];
                    var fileToken = (file: fileName, path: Path.Combine(fileLocation, fileName));
                    return fileToken;
                });

            return ctx =>
            {

                using (ZipStorer zip = ZipStorer.Create(xmindFileNane, string.Empty))
                {
                    foreach (var fileToken in filesToZip)
                    {
                        zip.AddFile(ZipStorer.Compression.Deflate, fileToken.path, fileToken.file, string.Empty);
                    }
                    // zip.AddFile(ZipStorer.Compression.Deflate, "META-INF\\manifest.xml", "manifest.xml", string.Empty);
                    // zip.AddFile(ZipStorer.Compression.Deflate, "meta.xml", "meta.xml", string.Empty);
                    // zip.AddFile(ZipStorer.Compression.Deflate, "content.xml", "content.xml", string.Empty);
                }
            };
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