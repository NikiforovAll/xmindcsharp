using System.IO;
using System.Xml.Linq;
using XMindAPI;
using XMindAPI.Logging;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;

namespace XMindAPI.Writers
{
    public class FileWriter : IXMindWriter
    {
        // private Dictionary<string, XDocument> _documentStorage;
        private FileWriterSettings Settings {get; set;}
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IXMindWriterOutputConfig _output;
        private IConfiguration _xMindSettings;

        // public Dictionary<string, XDocument> DocumentStorage { get => _documentStorage; private set => _documentStorage = value; }

        public FileWriter() : this(new FileWriterOutputConfig("root"))
        {
        }

        public FileWriter(IXMindWriterOutputConfig output)
        {
            SetOutput(output);
            // ReadSettingsFromConfiguration();
            _xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            
        }
        public IXMindWriter SetOutput(IXMindWriterOutputConfig output)
        {
            _output = output;
            return this;
        }

        public void WriteToStorage(XDocument document, string file)
        {
            var fileWriterOutput = _output as FileWriterOutputConfig;
            if(fileWriterOutput == null)
            {
                throw new InvalidOperationException();
            }
            var basePath = (_output as FileWriterOutputConfig).Path;
            Directory.CreateDirectory(basePath);
            File.WriteAllText(Path.Combine(basePath, file), document.ToString());
        }

        public IXMindWriterOutputConfig GetOutputConfig()
        {
            //use this validation logic across all writers, refactor code
            if(_output == null || String.IsNullOrEmpty(_output.OutputName))
            {
                throw new InvalidOperationException("IXMindWriter: output is not configured");
            }
            return _output;
        }
    }
}