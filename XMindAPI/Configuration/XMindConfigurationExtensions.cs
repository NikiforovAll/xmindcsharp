using System;
using System.Collections.Generic;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using XMindAPI.Writers.Util;
namespace XMindAPI.Extensions
{
    public static class XMindConfigurationExtensions
    {
        public static XMindConfiguration WithFileWriter(
            this XMindConfiguration config,
            string? basePath = default,
            bool zip = false)
        {
            var standardWriters = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            var result = config
                .WriteTo.Writers(FileWriterFactory.CreateWriters(standardWriters, basePath))
                .WriteTo.SetWriterBinding(FileWriterFactory.CreateResolvers(standardWriters));
            if (zip)
            {
                throw new NotImplementedException("Need to resolve workbook name from context");
                // result.WriteTo.SetFinalizeAction(
                        // FileWriterUtils.ZipXMindFolder(, basePath));
            }
            return result;
        }
        public static XMindConfiguration WithFileWriter(
            this XMindConfiguration config,
            bool useDefaultPath,
            bool zip = true)
        {
            return config.WithFileWriter(basePath: null, zip: zip);
        }

        public static XMindConfiguration WithInMemoryWriter(
            this XMindConfiguration config
        )
        {
            return config.WriteTo
                .Writer(
                    new InMemoryWriter(
                        new InMemoryWriterOutputConfig($"[in-memory-writer]")));
        }

    }
}
