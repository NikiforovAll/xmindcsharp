using NUnit.Framework;
using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using XMindAPI;
using XMindAPI.Models;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using FluentAssertions;
using XMindAPI.Writers.Util;
using XMindAPI.Extensions;
using System.Collections;
using XMindAPI.Core;

namespace Tests
{
    [TestFixture]
    public class XMindWorkBookTest
    {

        private readonly string _customOutputFolderName = Path.Combine(Path.GetTempPath(), "test-output");
        private readonly string[] _files = { "manifest.xml", "meta.xml", "content.xml" };
        private readonly bool _isCleanUpNeeded = true;

        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new TestCorrelatorSink())
                .WriteTo.File("book.test.log", retainedFileCountLimit: 3)
                .CreateLogger();
        }

        [Test]
        public void Save_CreateEmptyBookWithLogWriter_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                 .WriteTo.Writer(new LoggerWriter()
                         .SetOutput(new LoggerWriterOutputConfig(outputName: "root")))
                 .WriteTo.SetFinalizeAction(context => Log.Logger.Information("Finalized"))
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

        public void CreateWorkBook_ReadZippedXMindBookFromFileSystem_Success()
        {

            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(basePath: _customOutputFolderName, zip: true)
                .CreateWorkBook(workbookName: "test");

            var writer = (InMemoryWriter)new InMemoryWriter()
            .SetOutput(new InMemoryWriterOutputConfig(outputName: "root"));
            book.Save();
            var book2 = new XMindConfiguration()
                 .WriteTo
                 .Writer(writer)
                 .CreateWorkBook(sourceFileName: Path.Combine(_customOutputFolderName, "build.xmind"), workbookName: "test2");
            //Act
            book2.Save();
            //Assert
            writer.DocumentStorage.Keys.Should().NotBeEmpty().And
                .HaveCount(3).And
                .BeEquivalentTo("manifest.xml", "meta.xml", "content.xml");
        }

        [Test]

        public void GetPrimarySheet_EmptySheet_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "test");
            //Assert
            book.GetPrimarySheet().Should().NotBeNull("because primary sheet is created by default");
        }

        [Test]
        public void GetSheets_MultipleSheets_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: true)
                .CreateWorkBook(workbookName: "test");

            int numberOfSheets = 2;
            for (int i = 0; i < numberOfSheets; i++)
            {
                book.AddSheet(book.CreateSheet());
            }
            //Act
            var sheets = book.GetSheets();
            //Assert
            sheets.Count().Should().Be(++numberOfSheets, $"{numberOfSheets} were generated");
        }

        [Test]
        public void AddSheet_InsertSheetAsPrimary_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "test");
            var sheet = book.CreateSheet();
            //Act
            book.AddSheet(sheet, 0);
            //Assert
            book.GetPrimarySheet().Should().Be(sheet, "The last book must become primary");
        }

        [Test]
        public void RemoveSheet_RemovePrimarySheet()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "test");
            var primarySheet = book.GetPrimarySheet();
            //Act
            book.RemoveSheet(primarySheet);
            //Assert
            book.GetSheets().Count().Should().Be(0, "because we deleted primary sheet and there are no other sheets");
        }

        [Test]
        public void FindTopic_Default_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "FindTopic_Default_Success");
            //Act
            var topic = book.GetPrimarySheet().GetRootTopic();
            //Assert
            book.FindTopic(topic.GetId(), book)
                .Should().BeOfType<XMindTopic>("because we specify id of a topic")
                .Which.Should().Be(topic);
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