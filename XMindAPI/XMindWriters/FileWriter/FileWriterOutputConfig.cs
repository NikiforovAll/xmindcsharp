using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using XMindAPI.Logging;

namespace XMindAPI.Writers
{
    public class FileWriterOutputConfig : IXMindWriterOutputConfig
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private string _path;
        public string Path { get => _path; private set => _path = value; }
        public string OutputName { get; set; }

        public FileWriterOutputConfig(string outputName)
        {
            OutputName = outputName;
        }

        public FileWriterOutputConfig(string outputName, bool useDefaultPath):this(outputName)
        {
            var xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            var basePath = xMindSettings["output:base"];
            var sectionGroup = xMindSettings.GetSection("output:files").GetChildren()
                .SelectMany(c => c.GetChildren(), (section, token) => new {section, token})
                .FirstOrDefault(group => group.token.Key == "name" && group.token.Value == outputName);
            var path = sectionGroup?
                .section?
                .GetChildren()?
                .FirstOrDefault(t => t.Key.Equals("location"));
            if(path != null)
            {
                Path = System.IO.Path.Combine(basePath, path.Value);
            }
        }

        public IXMindWriterOutputConfig SetBasePath(string path)
        {
            // TODO: probably good idea to use this logic across all IXMindWriters
            if (!String.IsNullOrEmpty(Path))
            {
                //TODO: consider to use INotifyPropertyChanged for this side effect
                Logger.Warn($"IXMindWriterOutputConfig.Path was overridden: oldValue:{Path}, newValue: {path}");
            }
            Path = path;
            return this;
        }
    }
}