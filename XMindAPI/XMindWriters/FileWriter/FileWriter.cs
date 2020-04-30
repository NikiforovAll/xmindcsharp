using System;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace XMindAPI.Writers
{
    public class FileWriter : IXMindWriter<FileWriterOutputConfig>
    {
        // private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        // private IConfiguration _xMindSettings;
        internal FileWriterOutputConfig? OutputConfig { get; private set; }

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

        public FileWriter(FileWriterOutputConfig output)
        {
            SetOutput(output);
            // _xMindSettings = XMindConfigurationCache.Configuration.XMindConfigCollection;
        }

        public async Task WriteToStorage(XDocument xmlDocument, string file)
        {
            Guard.Against.Null(OutputConfig, nameof(OutputConfig));
            var basePath = OutputConfig.Path;
            var fileFullName = Path.Combine(basePath, file);
            Directory.CreateDirectory(basePath);
            // Logger.Info($"FileWriter.WriteToStorage: writing content to {fileFullName}");
            using var memoryStream = new MemoryStream();
            using var fileStream = File.Create(fileFullName);
            xmlDocument.Save(memoryStream);
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Flush(true);
        }

        public IXMindWriter<FileWriterOutputConfig> SetOutput(IXMindWriterOutputConfig output)
        {
            if (!(output is FileWriterOutputConfig fileConfig))
            {
                throw new ArgumentException("Please specify correct ${nameof(output)}");
            }
            OutputConfig = fileConfig;
            return this;
        }

        FileWriterOutputConfig IXMindWriter<FileWriterOutputConfig>.GetOutputConfig()
        {
            Guard.Against.Null(OutputConfig, nameof(OutputConfig));
            Guard.Against.NullOrWhiteSpace(OutputConfig.OutputName, "OutputName");
            return OutputConfig;
        }
    }
}
