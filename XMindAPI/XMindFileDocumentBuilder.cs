
using System;
using System.Xml.Linq;

namespace XMindAPI
{
    internal class XMindFileDocumentBuilder : XMindDocumentBuilder
    {
        public override XDocument CreateMetaFile()
        {
            throw new NotImplementedException($"{nameof(CreateMetaFile)}not implemented");
        }
        public override XDocument CreateManifestFile()
        {
            throw new NotImplementedException($"{nameof(CreateManifestFile)} not implemented");
        }

        public override XDocument CreateContentFile()
        {
            throw new NotImplementedException($"{nameof(CreateContentFile)} not implemented");
        }

        //         /// <summary>
        // /// Loads XMind workbook from drive
        // /// </summary>
        // /// <param name="fileName"></param>
        // private void Load(string fileName)
        // {
        //     if (String.IsNullOrEmpty(fileName) || File.Exists(fileName))
        //     {
        //         throw new InvalidOperationException("XMind file is not loaded");
        //     }
        //     FileInfo xMindFileInfo = new FileInfo(fileName);
        //     Logger.Info($"XMindFile loaded: {xMindFileInfo.FullName}");
        //     if (xMindFileInfo.Extension.ToLower() != ".xmind")
        //     {
        //         throw new InvalidOperationException("Extension of file is not .xmind");
        //     }
        //     String tempPath = String.Empty;
        //     try
        //     {
        //         //TODO: consider to move IO dependency out of XMindWorkBook
        //         tempPath = Path.GetTempPath() + Guid.NewGuid() + "\\";
        //         Directory.CreateDirectory(tempPath);
        //         string[] fileNameStrings = xMindFileInfo.Name.Split('.');
        //         string zipFileName = xMindFileInfo.Name.Replace(".xmind", ".zip");
        //         // Make a temporary copy of the XMind file with a .zip extention for J# zip libraries:
        //         string tempSourceFileName = tempPath + zipFileName;
        //         Logger.Info($"Read from: {tempSourceFileName}");
        //         File.Copy(fileName, tempSourceFileName);
        //         // Make sure the .zip temporary file is not read only
        //         // TODO: delete it later
        //         File.SetAttributes(tempSourceFileName, FileAttributes.Normal);
        //         using (ZipStorer zip = ZipStorer.Open(tempSourceFileName, FileAccess.Read))
        //         {
        //             // Read the central directory collection
        //             List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
        //             foreach (ZipStorer.ZipFileEntry entry in dir)
        //             {
        //                 zip.ExtractFile(entry, tempPath +
        //                     (entry.FilenameInZip == "manifest.xml" ? "META-INF\\" : "") +
        //                     entry.FilenameInZip);
        //             }
        //             zip.Close();
        //         }
        //         // TODO: consider to move to IO document builder
        //         _metaData = XDocument.Parse(File.ReadAllText(tempPath + "meta.xml"));
        //         _manifestData = XDocument.Parse(File.ReadAllText(tempPath + "META-INF\\manifest.xml"));
        //         _contentData = XDocument.Parse(File.ReadAllText(tempPath + "content.xml"));
        //     }
        //     finally
        //     {
        //         Directory.Delete(tempPath, true);
        //     }
        // }
    }
}