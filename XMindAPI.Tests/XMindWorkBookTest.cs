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
    public class XMindWorkBookTest
    {
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
                 .WriteTo
                 .SetFinalizeAction(context => Log.Logger.Information("Finalized"))
                 .Writer(new LoggerWriter()
                         .SetOutput(new LoggerWriterOutputConfig(outputName: "root")))
                 .CreateWorkBook(fileName: "test");
            
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
            //Arrange
            var book = new XMindConfiguration()
                 .WriteTo
                 .Writer(new InMemoryWriter()
                         .SetOutput(new InMemoryWriterOutputConfig(outputName: "root")))
                 .CreateWorkBook(fileName: "test");
            //Act
            book.Save();
            //Assert
        }

        [Test]
        public void Save_CreateEmptyBookWithFileWriterInCaseOfCustomBasePath_Success()
        {
            var book = new XMindConfiguration()
                .WriteTo.Writers(
                    new List<IXMindWriter>{
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig("manifest.xml").SetBasePath("custom-output/META-INF")),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig("meta.xml").SetBasePath("custom-output/")),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig("content.xml").SetBasePath("custom-output/"))
                    })
                .WriteTo.SetWriterBinding(
                    //selected based on OutPutName in IXMindWriterOutputConfig
                    new List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>>{
                        FileWriterUtils.ResolveManifestFile, 
                        FileWriterUtils.ResolveMetaFile,
                        FileWriterUtils.ResolveContentFile
                    }
                )
                .CreateWorkBook(fileName: "test");
            //Act
            book.Save();
            //Assert
            //TODO: add assertions, test IO properly, ideally it unit tests should not leave artifacts
        }

        [Test]
        public void Save_CreateEmptyBookWithDefaultPath_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(defaultSettings: true)
                .CreateWorkBook(fileName: "test");
            //Act
            book.Save();
            //Assert
            //TODO: add assertions, test IO properly, ideally it unit tests should not leave artifacts
        }
    }
}