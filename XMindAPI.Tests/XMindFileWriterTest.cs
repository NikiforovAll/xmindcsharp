using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

using Serilog;
using Serilog.Sinks.TestCorrelator;

using NUnit.Framework;
using FluentAssertions;

using XMindAPI;
using XMindAPI.Extensions;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class XMindFileWriterTest
    {
        private readonly string _customOutputFolderName = "custom-output";
        private readonly string _xmindOutputFolderName = "xmind-output";
        private readonly string[] _files = { "manifest.xml", "meta.xml", "content.xml" };
        private readonly bool _isCleanUpNeeded = true;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Save_CreateEmptyBookWithFileWriterInCaseOfCustomBasePath_Success()
        {

            var book = new XMindConfiguration()
                .WriteTo.Writers(
                    new List<IXMindWriter<IXMindWriterOutputConfig>>{
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig(_files[0])
                                .SetBasePath(Path.Combine(_customOutputFolderName, "META-INF"))),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig(_files[1]).SetBasePath(_customOutputFolderName)),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig(_files[2]).SetBasePath(_customOutputFolderName))
                    })
                .WriteTo.SetWriterBinding(FileWriterFactory.CreateStandardResolvers())
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
                .WithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "test");
            //Act
            book.Save();
            //Assert
            DirectoryInfo di = new DirectoryInfo(_xmindOutputFolderName);
            di.GetFileSystemInfos("*.xml")
                .Select(fi => fi.Should().BeOfType<FileInfo>().Which.Name
                .Should().BeOneOf(_files))
                .All(x => true);
        }
        [Test]
        public async Task Save_CreateEmptyBookWithFileWriterWithDefaultPathAndZip_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .WithFileWriter(useDefaultPath: true, zip: true)
                .CreateWorkBook(workbookName: "test.xmind");
            //Act
            await book.Save();
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
