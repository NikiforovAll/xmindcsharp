using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using static XMindAPI.XMindConfiguration;
using XMindAPI.Configuration;
using XMindAPI.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;

namespace XMindAPI.Writers
{
    public class FileWriterFactory
    {

        public static List<IXMindWriter<IXMindWriterOutputConfig>> CreateStandardWriters(string? basePath)
        {
            var standardOutputs = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            return standardOutputs.Select(o => CreateStandardWriterFactoryMethod(o, basePath)).ToList();
        }
        public static List<Func<XMindWriterContext, List<IXMindWriter<IXMindWriterOutputConfig>>, IXMindWriter<IXMindWriterOutputConfig>>> CreateStandardResolvers()
        {
            var standardOutputs = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            return standardOutputs.Select(o => CreateResolverFactoryMethod(o)).ToList();
        }
        public static IXMindWriter<IXMindWriterOutputConfig> CreateStandardWriterFactoryMethod(
            FileWriterStandardOutput standardOutputType, string? basePath)
        {
            IConfiguration xMindSettings = EnsureXMindSettings();
            string fileName = standardOutputType switch
            {
                FileWriterStandardOutput.Manifest => xMindSettings[ManifestLabel],
                FileWriterStandardOutput.Meta => xMindSettings[MetaLabel],
                FileWriterStandardOutput.Content => xMindSettings[ContentLabel],
                _ => throw new InvalidOperationException(
                    "CreateWriterFactoryMethod haven't assigned writer")
            };
            bool useDefaultPath = basePath is null;
            IXMindWriter<IXMindWriterOutputConfig> result;

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

        private static IConfiguration EnsureXMindSettings()
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            if (xMindSettings is null)
            {
                const string errorMessage = "XMindSettings are not provided";
                Logger.Log.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            return xMindSettings;
        }

        public static Func<XMindWriterContext, List<IXMindWriter<IXMindWriterOutputConfig>>, IXMindWriter<IXMindWriterOutputConfig>> CreateResolverFactoryMethod(FileWriterStandardOutput standardOutputType) => standardOutputType switch
        {
            FileWriterStandardOutput.Manifest =>
                (ctx, writers) => ResolveWriterByOutputName(ctx, writers, ManifestLabel),
            FileWriterStandardOutput.Meta =>
                (ctx, writers) => ResolveWriterByOutputName(ctx, writers, MetaLabel),
            FileWriterStandardOutput.Content =>
                (ctx, writers) => ResolveWriterByOutputName(ctx, writers, ContentLabel),
            _ => throw new InvalidOperationException(
                "CreateResolverFactoryMethod haven't assigned binding")
        };

        private static IXMindWriter<IXMindWriterOutputConfig> ResolveWriterByOutputName(
            XMindWriterContext context,
            List<IXMindWriter<IXMindWriterOutputConfig>> writers,
            string fileLabel)
        {
            IConfiguration xMindSettings = EnsureXMindSettings();
            var file = xMindSettings[fileLabel];
            var fileName = context.FileName;
            var writerFound = writers.FirstOrDefault(
                w => !string.IsNullOrWhiteSpace(fileName)
                    && fileName!.Equals(file) // TODO: fix
                    && w.GetOutputConfig().OutputName.Equals(file));
            return writerFound;
        }
    }
}
