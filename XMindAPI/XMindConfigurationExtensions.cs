using System.Collections.Generic;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using XMindAPI.Writers.Util;
namespace XMindAPI.Extensions
{
    public static class XMindConfigurationExtensions
    {
        public static XMindConfiguration SetUpXMindWithFileWriter(
            this XMindConfiguration config,
            bool defaultSettings = true,
            bool zip = false)
        {
            var standardWriters = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            var result = config.WriteTo.Writers(FileWriterFactory.CreateWriters(standardWriters))
                .WriteTo.SetWriterBinding(FileWriterFactory.CreateResolvers(standardWriters));
            if(zip)
            {
                result.WriteTo.SetFinalizeAction(FileWriterUtils.ZipXMindFolder("build.xmind"));
            }
            return result;
        }
    }
}