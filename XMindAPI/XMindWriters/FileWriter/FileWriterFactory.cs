using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using static XMindAPI.Configuration.XMindConfiguration;
using XMindAPI.Configuration;

namespace XMindAPI.Writers
{
    public class FileWriterFactory
    {

        public static List<IXMindWriter> CreateStandardWriters(string basePath)
        {
            var standardOutputs = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            return standardOutputs.Select(o => CreateStandardWriterFactoryMethod(o, basePath)).ToList();
        }
        public static List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> CreateStandardResolvers()
        {
            var standardOutputs = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            return standardOutputs.Select(o => CreateResolverFactoryMethod(o)).ToList();
        }
        public static IXMindWriter CreateStandardWriterFactoryMethod(
            FileWriterStandardOutput standardOutputType, string basePath)
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;

            string fileName = standardOutputType switch
            {
                FileWriterStandardOutput.Manifest => xMindSettings[ManifestLabel],
                FileWriterStandardOutput.Meta => xMindSettings[MetaLabel],
                FileWriterStandardOutput.Content => xMindSettings[ContentLabel],
                _ => throw new InvalidOperationException("CreateWriterFactoryMethod haven't assigned writer")
            };
            bool useDefaultPath = basePath == null;
            IXMindWriter result;

            var writerConfig = new FileWriterOutputConfig(fileName, useDefaultPath);
            if (!useDefaultPath)
            {
                var xmindDefaultFileLocation = XMindConfigurationLoader.Configuration
                    .GetOutputFilesLocations()[fileName];
                writerConfig.SetBasePath(Path.Combine(basePath, xmindDefaultFileLocation));
            }
            result = new FileWriter().SetOutput(writerConfig);
            return result;
        }

        public static Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter> CreateResolverFactoryMethod(FileWriterStandardOutput standardOutputType) => standardOutputType switch
        {
            FileWriterStandardOutput.Manifest =>
                (ctx, writers) => ResolveWriterByOutputName(ctx, writers, ManifestLabel),
            FileWriterStandardOutput.Meta =>
                (ctx, writers) => ResolveWriterByOutputName(ctx, writers, MetaLabel),
            FileWriterStandardOutput.Content =>
                (ctx, writers) => ResolveWriterByOutputName(ctx, writers, ContentLabel),
            _ => throw new InvalidOperationException("CreateResolverFactoryMethod haven't assigned binding")
        };

        private static IXMindWriter ResolveWriterByOutputName(
            XMindWriterContext context,
            List<IXMindWriter> writers,
            string fileLabel)
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            var file = xMindSettings[fileLabel];
            var writerFound = writers.FirstOrDefault(w => context.FileName.Equals(file) && w.GetOutputConfig().OutputName.Equals(file));
            return writerFound;
        }
    }
}
