using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

using XMindAPI.Logging;
using XMindAPI.Configuration;

namespace XMindAPI.Writers
{
    public class FileWriterOutputConfig : IXMindWriterOutputConfig
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly bool _useDefaultPath;
        private string _path;
        public string Path { get => _path; private set => _path = value; }
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
        public FileWriterOutputConfig(string outputName, bool useDefaultPath):this(outputName)
        {
            if(useDefaultPath){
                var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
                var basePath = xMindSettings["output:base"];
                Dictionary<string, string> locations = XMindConfigurationCache.Configuration.GetOutputFilesLocations();
                var path = locations[outputName];
                if(path != null)
                {
                    Path = System.IO.Path.Combine(basePath, path);
                }
            }

            this._useDefaultPath = useDefaultPath;
        }

        public IXMindWriterOutputConfig SetBasePath(string path)
        {
            if (this._useDefaultPath){
                throw new InvalidOperationException("Not possible to assign new path, default path in use because of FileWriterOutputConfig.useDefaultPath");
            }
            if(Path != null)
            {
                Logger.Warn($"IXMindWriterOutputConfig.Path was overridden: oldValue:{Path}, newValue: {path}");
            }
            Path = path;
            return this;
        }
    }
}