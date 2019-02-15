using NUnit.Framework;
using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using XMindAPI;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using FluentAssertions;
using XMindAPI.Writers.Util;

namespace Tests
{
    [TestFixture]
    public class XMindWorkBookTest
    {

        private readonly string _customOutputFolderName = "test-output";
        private readonly string[] _files = { "manifest.xml", "meta.xml", "content.xml" };
        private readonly bool _isCleanUpNeeded = false;

        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new TestCorrelatorSink())
                .WriteTo.File("test.log", retainedFileCountLimit: 3)
                .CreateLogger();
        }

        [Test]
        public void Save_CreateEmptyBookWithLogWriter_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                 .WriteTo.SetFinalizeAction(context => Log.Logger.Information("Finalized"))
                 .Writer(new LoggerWriter()
                         .SetOutput(new LoggerWriterOutputConfig(outputName: "root")))
                 .CreateWorkBook(workbookName: "test");

            using (TestCorrelator.CreateContext())
            {
                //Act
                book.Save();
                //Assert
                TestCorrelator.GetLogEventsFromCurrentContext()
                .Where(e => e.Level == Serilog.Events.LogEventLevel.Information)
                .Should()
                .HaveCount(4, "empty book initialization had failed");

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Where(e => !e.MessageTemplate.ToString().Contains("Finalized", StringComparison.OrdinalIgnoreCase))
                    .Any(e => e.MessageTemplate.ToString().Contains("root")).Should().BeTrue();
            }
        }

        [Test]
        public void Save_CreateEmptyBookWithInMemoryWriter_Success()
        {
            var writer = (InMemoryWriter)new InMemoryWriter()
                .SetOutput(new InMemoryWriterOutputConfig(outputName: "root"));
            //Arrange
            var book = new XMindConfiguration()
                 .WriteTo
                 .Writer(writer)
                 .CreateWorkBook(workbookName: "test");
            //Act
            book.Save();
            //Assert
            writer.DocumentStorage.Keys.Should().NotBeEmpty().And
                .HaveCount(3).And
                .BeEquivalentTo("manifest.xml", "meta.xml", "content.xml");
        }

        [Test]

        public void ReadZipXMindBookFromFileSystem_Success()
        {
            //TODO: change fluent configuration
            //Arrange
            var book = new XMindConfiguration()
                .WriteTo.SetFinalizeAction(FileWriterUtils.ZipXMindFolder("build.xmind"))
                .Writers(
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
                ).CreateWorkBook(workbookName: "test");
            book.Save();

            var book2 = new XMindConfiguration()
                .CreateWorkBook(workbookName: "test2", fileName:"test-output//build.xmind");
            //Assert
            
            //TODO: assertion for XDocuments
        }
        [OneTimeTearDown]
        public void Cleanup()
        {
            if (_isCleanUpNeeded)
            {
                var customOutput = new DirectoryInfo(_customOutputFolderName);
                if (customOutput.Exists)
                    customOutput.Delete(true);
            }
        }
    }
}