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


        public static Action<List<XMindWriterContext>> ZipXMindFolder(string xmindFileName)
        {
            var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            var filesToZipLabels = new string[] {
                "output:definition:meta",
                "output:definition:manifest",
                "output:definition:content"
            };
            var fileNames = filesToZipLabels.Select(label => xMindSettings[label]).ToList();
            var filesToZip = xMindSettings.GetSection("output:files")
            .GetChildren().Where(
                x => fileNames
                        .Contains(
                            x.GetChildren()
                                .Where(el => el.Key == "name").Select(el => el.Value).FirstOrDefault()
                        )
                    )
                    .Select(x => (File: x["name"], Path: x["location"]))
                    .ToList();
            return ctx =>
            {

                using (ZipStorer zip = ZipStorer.Create(Path.Combine(xMindSettings["output:base"], xmindFileName), string.Empty))
                {

                    foreach (var fileToken in filesToZip)
                    {
                        var fullPath = Path.Combine(
                            Environment.CurrentDirectory,
                            XMindConfigurationCache.Configuration.XMindConfigCollection["output:base"],
                            fileToken.Path,
                            fileToken.File
                        );
                        zip.AddFile(ZipStorer.Compression.Deflate, fullPath, fileToken.File, string.Empty);
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
                .FirstOrDefault(w => context.FileName.Equals(file) && w.GetOutputConfig().OutputName.Equals(file));
            return writerFound;
        }
    }
}