using System;
using System.Collections.Generic;
using XMindAPI.Writers.Util;
using System.Linq;
namespace XMindAPI.Writers
{
    public class FileWriterFactory
    {

        public static List<IXMindWriter> CreateWriters(IEnumerable<FileWriterStandardOutput> standardOutputs)
        {
           return standardOutputs.Select(o => CreateWriterFactoryMethod(o)).ToList();
        }
        public static List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>> CreateResolvers(IEnumerable<FileWriterStandardOutput> standardOutputs)
        {
           return standardOutputs.Select(o => CreateResolverFactoryMethod(o)).ToList();
        }
        public static IXMindWriter CreateWriterFactoryMethod(FileWriterStandardOutput standardOutputType)
        {
            var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            IXMindWriter result;
            switch (standardOutputType)
            {
                case FileWriterStandardOutput.Manifest:
                    result = new FileWriter().SetOutput(new FileWriterOutputConfig(xMindSettings["output:definition:manifest"], true));
                    break;
                case FileWriterStandardOutput.Meta:
                    result = new FileWriter().SetOutput(new FileWriterOutputConfig(xMindSettings["output:definition:meta"], true));;
                    break;
                case FileWriterStandardOutput.Content:
                    result = new FileWriter().SetOutput(new FileWriterOutputConfig(xMindSettings["output:definition:content"], true));;
                    break;
                default:
                    result = null;
                    break;
            }
            if (result == null)
            {
                throw new InvalidOperationException("CreateWriterFactoryMethod haven't assigned writer");
            }
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