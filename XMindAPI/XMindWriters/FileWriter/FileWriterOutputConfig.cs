using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

using XMindAPI.Configuration;

namespace XMindAPI.Writers
{
    public class FileWriterOutputConfig : IXMindWriterOutputConfig
    {
        private readonly bool _useDefaultPath;

        public string? Path { get; private set; }
        public string OutputName { get; set; }

        public FileWriterOutputConfig(string outputName)
        {
            OutputName = outputName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outputName">name of the file</param>
        /// <param name="useDefaultPath">build Path based on xmindsettings.json file, basePath (output:base) and file location (output:files:[outputname]) is used</param>
        public FileWriterOutputConfig(string outputName, bool useDefaultPath) : this(outputName)
        {
            var xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
            if (useDefaultPath && xMindSettings is object)
            {
                var basePath = xMindSettings["output:base"];
                Dictionary<string, string> locations = XMindConfigurationLoader.Configuration.GetOutputFilesLocations();
                var path = locations[outputName];
                if (path != null)
                {
                    Path = System.IO.Path.Combine(basePath, path);
                }
            }
            _useDefaultPath = useDefaultPath;
        }

        public IXMindWriterOutputConfig SetBasePath(string path)
        {
            if (_useDefaultPath)
            {
                throw new InvalidOperationException(
                    "Not possible to assign new path, default path in use because of FileWriterOutputConfig.useDefaultPath");
            }
            Path = path;
            return this;
        }
    }
}
