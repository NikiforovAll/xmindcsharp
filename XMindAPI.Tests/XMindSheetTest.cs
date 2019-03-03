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
using XMindAPI.Extensions;
using System.Collections;
using XMindAPI.Models;
namespace Tests
{
    [TestFixture]
    public class XMindSheetTest
    {
        private readonly bool _isCleanUpNeeded = false;
        private readonly string _xmindOutputFolderName = "xmind-output";
        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new TestCorrelatorSink())
                .WriteTo.File("sheet.test.log", retainedFileCountLimit: 3)
                .CreateLogger();
        }

        [Test]
        public void GetParent_DirectAncestor_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "test");
            //Act
            var parent = book
                .CreateSheet()
                .GetParent();
            //Arrange
            parent.Should().Be((XMindWorkBook)book, $"Sheet as direct ancestor");
        }

        [Test]
        public void SetTitle_DefaultFlow_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .SetUpXMindWithFileWriter(useDefaultPath: true, zip: false)
                .CreateWorkBook(workbookName: "test");
            string title = "Awesome sheet";
            //Act
            var sheet = book
                .CreateSheet();
            sheet.SetTitle(title);
            //Arrange
            book.Save();
            sheet.GetTitle().Should().Be(title, $"Sheet as direct ancestor");
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (_isCleanUpNeeded)
            {
                var xmindOutput = new DirectoryInfo(_xmindOutputFolderName);
                if (xmindOutput.Exists)
                    xmindOutput.Delete(true);
            }
        }
    }
}