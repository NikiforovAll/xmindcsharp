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
            string basePath = null,
            bool zip = false)
        {
            var standardWriters = new List<FileWriterStandardOutput>{
                FileWriterStandardOutput.Manifest,
                FileWriterStandardOutput.Meta,
                FileWriterStandardOutput.Content
            };
            var result = config
                .WriteTo
                    .Writers(FileWriterFactory.CreateWriters(standardWriters, basePath))
                .WriteTo
                    .SetWriterBinding(FileWriterFactory.CreateResolvers(standardWriters));
            if (zip)
            {
                result.WriteTo
                    .SetFinalizeAction(
                        FileWriterUtils.ZipXMindFolder("build.xmind", basePath)
                    );
            }
            return result;
        }
        public static XMindConfiguration SetUpXMindWithFileWriter(
            this XMindConfiguration config,
            bool useDefaultPath,
            bool zip = false)
        {
            return config.SetUpXMindWithFileWriter(basePath: null, zip: zip);
        }

    }
}