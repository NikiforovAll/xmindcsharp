using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using XMindAPI;

namespace XMindAPI.Writers
{
    public class FileWriter : IXMindWriter
    {
        // private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        // private IConfiguration _xMindSettings;
        internal IXMindWriterOutputConfig _output;

        internal readonly bool _isAutoAddedResolver = false;
        internal readonly FileWriterStandardOutput _fileWriterStandardOutput;
        public FileWriter(FileWriterStandardOutput standardOutput)
        {
            _isAutoAddedResolver = true;
            _fileWriterStandardOutput = standardOutput;
        }
        public FileWriter() : this(new FileWriterOutputConfig("root"))
        {
        }

        public FileWriter(IXMindWriterOutputConfig output)
        {
            SetOutput(output);
            // _xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
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
            var fileFullName = Path.Combine(basePath, file);
            Directory.CreateDirectory(basePath);
            // Logger.Info($"FileWriter.WriteToStorage: writing content to {fileFullName}");
            File.WriteAllText(fileFullName, document.ToString());
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
