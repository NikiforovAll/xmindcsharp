using System.Collections.Generic;
using XMindAPI.Configuration;
using XMindAPI.Writers;

namespace XMindAPI.Extensions
{
    public static class XMindConfigurationExtensions
    {
        public static XMindConfiguration SetUpXMindWithFileWriter(this XMindConfiguration config, bool defaultSettings = true)
        {
            var standardWriters = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            return config
                .WriteTo.Writers(FileWriterFactory.CreateWriters(standardWriters))
                .WriteTo.SetWriterBinding(FileWriterFactory.CreateResolvers(standardWriters));
        }
    }
}