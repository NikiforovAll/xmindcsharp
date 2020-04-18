using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
using System;
using System.IO;

using XMindAPI.Configuration;
using XMindAPI.Utils;

namespace XMindAPI.Writers.Util
{
    public static class FileWriterUtils
    {
        public static IXMindWriter ResolveManifestFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var writerFound = context.ResolveWriterByOutputName(writers, XMindConfiguration.ManifestLabel);
            return writerFound;
        }

        public static IXMindWriter ResolveMetaFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var writerFound = context.ResolveWriterByOutputName(writers, XMindConfiguration.MetaLabel);
            return writerFound;
        }

        public static IXMindWriter ResolveContentFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var writerFound = context.ResolveWriterByOutputName(writers, XMindConfiguration.ContentLabel);
            return writerFound;
        }

        public static Action<List<XMindWriterContext>> ZipXMindFolder(string xmindFileName, string basePath = null)
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            if (basePath == null)
            {
                basePath = xMindSettings["output:base"];
            }
            var filesToZipLabels = XMindConfigurationLoader
                .Configuration
                .GetOutputFilesDefinitions()
                .Values;
            return ctx =>
            {
                using ZipStorer zip = ZipStorer.Create(Path.Combine(basePath, xmindFileName), string.Empty);
                var filesToZip = XMindConfigurationLoader
                    .Configuration
                    .GetOutputFilesLocations().Where(kvp => filesToZipLabels.Contains(kvp.Key));
                foreach (var fileToken in filesToZip)
                {
                    var fullPath = Path.Combine(
                        Environment.CurrentDirectory,
                        basePath,
                        fileToken.Value,
                        fileToken.Key
                    );

                    zip.AddFile(ZipStorer.Compression.Deflate, fullPath, fileToken.Key, string.Empty);
                }
            };
        }
        private static IXMindWriter ResolveWriterByOutputName(
            this XMindWriterContext context,
            List<IXMindWriter> writers,
            string fileLabel)
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            var file = xMindSettings[fileLabel];
            var writerFound = writers
                .FirstOrDefault(w => context.FileName.Equals(file) && w.GetOutputConfig().OutputName.Equals(file));
            return writerFound;
        }
    }
}
