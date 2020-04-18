using System;
using System.Collections.Generic;
using System.Linq;

using static XMindAPI.Configuration.XMindConfiguration;
using XMindAPI.Writers.Util;
using XMindAPI.Configuration;

namespace XMindAPI.Writers
{
    public class FileWriterFactory
    {

        public static List<IXMindWriter> CreateWriters(IEnumerable<FileWriterStandardOutput> standardOutputs, string basePath)
        {
            return standardOutputs.Select(o => CreateWriterFactoryMethod(o, basePath)).ToList();
        }
        public static List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> CreateResolvers(IEnumerable<FileWriterStandardOutput> standardOutputs)
        {
            return standardOutputs.Select(o => CreateResolverFactoryMethod(o)).ToList();
        }
        public static IXMindWriter CreateWriterFactoryMethod(FileWriterStandardOutput standardOutputType, string basePath)
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            string fileName = null;
            bool useDefaultPath = basePath == null;
            IXMindWriter result;
            switch (standardOutputType)
            {

                case FileWriterStandardOutput.Manifest:
                    fileName = xMindSettings[ManifestLabel];
                    break;
                case FileWriterStandardOutput.Meta:
                    fileName = xMindSettings[MetaLabel];
                    break;
                case FileWriterStandardOutput.Content:
                    fileName = xMindSettings[ContentLabel];
                    break;
                default:
                    result = null;
                    break;
            }
            if (fileName == null)
            {
                throw new InvalidOperationException("CreateWriterFactoryMethod haven't assigned writer");
            }
            var writerConfig = new FileWriterOutputConfig(
                fileName,
                useDefaultPath
            );
            if(!useDefaultPath)
            {
                var xmindDefaultFileLocation = XMindConfigurationLoader.Configuration.GetOutputFilesLocations()[fileName];
                writerConfig.SetBasePath(
                    System.IO.Path.Combine(basePath, xmindDefaultFileLocation)
                );
            }
            result = new FileWriter().SetOutput(writerConfig);
            return result;
        }

        public static Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter> CreateResolverFactoryMethod(FileWriterStandardOutput standardOutputType)
        {
            Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter> result;
            switch (standardOutputType)
            {
                case FileWriterStandardOutput.Manifest:
                    result = FileWriterUtils.ResolveManifestFile;
                    break;
                case FileWriterStandardOutput.Meta:
                    result = FileWriterUtils.ResolveMetaFile;
                    break;
                case FileWriterStandardOutput.Content:
                    result = FileWriterUtils.ResolveContentFile;
                    break;
                default:
                    result = null;
                    break;
            }
            if (result == null)
            {
                throw new InvalidOperationException("CreateResolverFactoryMethod haven't assigned binding");
            }
            return result;
        }
    }
}
