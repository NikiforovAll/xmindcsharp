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
        private Dictionary<string, XDocument> _documentStorage;
        private FileWriterSettings Settings {get; set;}
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IXMindWriterOutput _output;
        private IConfiguration _xMindSettings;

        public Dictionary<string, XDocument> DocumentStorage { get => _documentStorage; private set => _documentStorage = value; }

        public FileWriter() : this(new FileWriterOutput("root"))
        {
        }

        public FileWriter(IXMindWriterOutput output)
        {
            SetOutputName(output);
            ReadSettingsFromConfiguration();
            _xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
            
        }
        public IXMindWriter SetOutputName(IXMindWriterOutput output)
        {
            _output = output;
            return this;
        }

        public void WriteToStorage(XDocument document, string file)
        {
            var fileWriterOutput = _output as FileWriterOutput;
            if(fileWriterOutput == null)
            {
                throw new InvalidOperationException();
            }
            //TODO: XMindOut.Resolve(WriterList, condition)
        }

        private void ReadSettingsFromConfiguration()
        {
            Settings = new FileWriterSettings(){
                ManifestLocation = _xMindSettings["output:manifestLocation"],
                MetaDataLocation = _xMindSettings["output:metadataLocation"],
                ContentLocation = _xMindSettings["output:contentLocation"]
            };
        }
    }
}