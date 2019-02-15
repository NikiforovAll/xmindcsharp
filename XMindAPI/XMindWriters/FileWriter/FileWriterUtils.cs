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
            var writerFound = context.ResolveWriterByOutputName(writers, XMindConfigurationCache.ManifestLabel);
            return writerFound;
        }

        public static IXMindWriter ResolveMetaFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var writerFound = context.ResolveWriterByOutputName(writers, XMindConfigurationCache.MetaLabel);
            return writerFound;
        }

        public static IXMindWriter ResolveContentFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var writerFound = context.ResolveWriterByOutputName(writers, XMindConfigurationCache.ContentLabel);
            return writerFound;
        }

        public static Action<List<XMindWriterContext>> ZipXMindFolder(string xmindFileName)
        {
            var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            var filesToZipLabels = XMindConfigurationCache
                .Configuration
                .GetOutputFilesDefinitions()
                .Values;

            // var fileNames = filesToZipLabels.Select(label => xMindSettings[label]).ToList();

            // var filesToZip = xMindSettings.GetSection("output:files")
            // .GetChildren().Where(
            //     x => fileNames
            //             .Contains(
            //                 x.GetChildren()
            //                     .Where(el => el.Key == "name").Select(el => el.Value).FirstOrDefault()
            //             )
            //         )
            //         .Select(x => (File: x["name"], Path: x["location"]))
            //         .ToList();
            return ctx =>
            {

                using (ZipStorer zip = ZipStorer.Create(
                        Path.Combine(xMindSettings["output:base"], xmindFileName), string.Empty)
                    )
                {
                    var filesToZip = XMindConfigurationCache
                        .Configuration
                        .GetOutputFilesLocations().Where(kvp => filesToZipLabels.Contains(kvp.Key));
                    foreach (var fileToken in filesToZip)
                    {
                        var fullPath = Path.Combine(
                            Environment.CurrentDirectory,
                            XMindConfigurationCache.Configuration.XMindConfigCollection["output:base"],
                            fileToken.Value,
                            fileToken.Key
                        );

                        zip.AddFile(ZipStorer.Compression.Deflate, fullPath, fileToken.Key, string.Empty);
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