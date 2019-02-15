using NUnit.Framework;
using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using XMindAPI;
using XMindAPI.Extensions;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using XMindAPI.Writers.Util;
using FluentAssertions;
namespace Tests
{
    [TestFixture]
    public class XMindFileWriterTest
    {
        private readonly string _customOutputFolderName = "custom-output";
        private readonly string _xmindOutputFolderName = "xmind-output";
        private readonly string[] _files = { "manifest.xml", "meta.xml", "content.xml" };
        private readonly bool _isCleanUpNeeded = false;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Save_CreateEmptyBookWithFileWriterInCaseOfCustomBasePath_Success()
        {

            var _customOutputFolderName = "custom-output/";
            var book = new XMindConfiguration()
                .WriteTo.Writers(
                    new List<IXMindWriter>{
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig(_files[0])
                                .SetBasePath(Path.Combine(_customOutputFolderName, "META-INF"))),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig(_files[1]).SetBasePath(_customOutputFolderName)),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig(_files[2]).SetBasePath(_customOutputFolderName))
                    })
                .WriteTo.SetWriterBinding(
                    //selected based on OutPutName in IXMindWriterOutputConfig
                    new List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>>{
                        FileWriterUtils.ResolveManifestFile,
                        FileWriterUtils.ResolveMetaFile,
                        FileWriterUtils.ResolveContentFile
                    }
                )
                .CreateWorkBook(workbookName: "test");
            //Act
            book.Save();
            //Assert
            DirectoryInfo di = new DirectoryInfo(_customOutputFolderName);
            di.GetFileSystemInfos("*.xml")
                .Select(fi => fi.Should().BeOfType<FileInfo>().Which.Name.Should().BeOneOf(_files))
                .All(x => true);

        }

        [Test]
        public void Save_CreateEmptyBookWithFileWriterWithDefaultPath_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(defaultSettings: true)
                .CreateWorkBook(workbookName: "test");
            //Act
            book.Save();
            //Assert
            DirectoryInfo di = new DirectoryInfo(_xmindOutputFolderName);
            di.GetFileSystemInfos("*.xml")
                .Select(fi => fi.Should().BeOfType<FileInfo>().Which.Name.Should().BeOneOf(_files))
                .All(x => true);
        }
        [Test]
        public void Save_CreateEmptyBookWithFileWriterWithDefaultPathAndZip_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(defaultSettings: true, zip: true)
                .CreateWorkBook(workbookName: "test");
            //Act
            book.Save();
            //Assert
            DirectoryInfo di = new DirectoryInfo(_xmindOutputFolderName);
            di.GetFileSystemInfos("*.xmind").Should().ContainSingle();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (_isCleanUpNeeded)
            {
                var customOutput = new DirectoryInfo(_customOutputFolderName);
                if (customOutput.Exists)
                    customOutput.Delete(true);
                var xmindOutput = new DirectoryInfo(_xmindOutputFolderName);
                if (xmindOutput.Exists)
                    xmindOutput.Delete(true);
            }
        }
    }
}