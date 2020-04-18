
using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

using XMindAPI.Configuration;
using XMindAPI.Zip;
using System.Threading.Tasks;

namespace XMindAPI.Core.Builders
{
    internal class XMindFileDocumentBuilder : XMindDocumentBuilder
    {
        // protected readonly IConfiguration xMindSettings = XMindConfigurationLoader.Configuration.XMindConfigCollection;
        private readonly string _sourceFileName;
        private bool _isLoaded = false;

        public XMindFileDocumentBuilder(string sourceFileName)
        {
            this._sourceFileName = sourceFileName;
            if (!_isLoaded)
            {
                // TODO: bad approach, IO shouldn't be in ctor
                Load(_sourceFileName);
                _isLoaded = true;
            }
        }
        public override XDocument CreateMetaFile()
        {
            if (this.metaData == null)
            {
                throw new InvalidOperationException("CreateMetaFile: Meta file is not initialized. Invoke Load()");
            }
            return this.metaData;
        }
        public override XDocument CreateManifestFile()
        {
            if (this.manifestData == null)
            {
                throw new InvalidOperationException("CreateMetaFile: Meta file is not initialized. Invoke Load()");
            }
            return this.manifestData;
        }

        public override XDocument CreateContentFile()
        {
            if (this.contentData == null)
            {
                throw new InvalidOperationException("CreateMetaFile: Meta file is not initialized. Invoke Load()");
            }
            return this.contentData;
        }

        /// <summary>
        /// Loads XMind workbook from drive
        /// </summary>
        /// <param name="fileName">file name path </param>
        private void Load(string fileName)
        {
            // TODO: this should be absolute path
            if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                throw new InvalidOperationException($"XMind file {fileName} is not loaded");
            }
            FileInfo xMindFileInfo = new FileInfo(fileName);
            // Logger.Info($"XMindFile loaded: {xMindFileInfo.FullName}");
            if (xMindFileInfo.Extension.ToLower() != ".xmind")
            {
                throw new InvalidOperationException(
                    "Extension of file is not .xmind"
                );
            }
            String tempPath = string.Empty;
            try
            {
                tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempPath);
                string zipFileName = xMindFileInfo.Name.Replace(".xmind", ".zip");
                // Make a temporary copy of the XMind file with a .zip extention for J# zip libraries:
                string tempSourceFileName = Path.Combine(tempPath, zipFileName);
                // Logger.Info($"Read from: {tempSourceFileName}");
                File.Copy(fileName, tempSourceFileName);
                // Make sure the .zip temporary file is not read only
                // TODO: delete it later
                File.SetAttributes(tempSourceFileName, FileAttributes.Normal);
                List<string> fileNamesExtracted = new List<string>(3);
                using (ZipStorer zip = ZipStorer.Open(tempSourceFileName, FileAccess.Read))
                {
                    Dictionary<string, string> locations = XMindConfigurationLoader.Configuration
                        .GetOutputFilesLocations();
                    // Read the central directory collection
                    foreach (ZipStorer.ZipFileEntry entry in zip.ReadCentralDir())
                    {
                        if (locations.TryGetValue(entry.FilenameInZip, out var location))
                        {
                            string fileEntryFullName =
                                Path.Combine(tempPath, location, entry.FilenameInZip);

                            fileNamesExtracted.Add(fileEntryFullName);
                            zip.ExtractFile(entry, fileEntryFullName);
                        }
                    }
                    zip.Close();
                }
                // foreach (var file in fileNamesExtracted)
                // {
                //     Logger.Info($"FileDocumentBuilder.Load: file {file} extracted from zip");
                // }
                var files = XMindConfigurationLoader
                    .Configuration
                .   GetOutputFilesDefinitions();
                var fileLocations = XMindConfigurationLoader
                    .Configuration
                    .GetOutputFilesLocations();

                var manifestFileName = files[XMindConfiguration.ManifestLabel];
                var metaFileName = files[XMindConfiguration.MetaLabel];
                var contentFileName = files[XMindConfiguration.ContentLabel];

                Dictionary<string, XDocument> docs = new Dictionary<string, XDocument>();
                foreach (var fileToken in files)
                {
                    docs.Add(
                        fileToken.Key,
                        XDocument.Parse(File.ReadAllText(Path.Combine(tempPath, fileLocations[fileToken.Value], fileToken.Value)))
                    );
                }
                this.metaData = docs[XMindConfiguration.MetaLabel];
                this.manifestData = docs[XMindConfiguration.ManifestLabel];
                this.contentData = docs[XMindConfiguration.ContentLabel];
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }
    }
}
