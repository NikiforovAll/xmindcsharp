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
    public class XMindTopicTest
    {
        private readonly bool _isCleanUpNeeded = false;
        private readonly string _xmindOutputFolderName = "xmind-output";
        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("topic.test.log", retainedFileCountLimit: 3)
                .CreateLogger();
        }

        [Test]

        public void SetTitle_DefaultFlow_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .WithFileWriter(useDefaultPath: true, zip: true)
                .CreateWorkBook(workbookName: "test");
            string title = "Awesome topic";
            //Act
            var topic = book
                .CreateTopic();
            topic.SetTitle(title);
            //Arrange
            topic.GetTitle().Should().Be(title, $"because title for topic is specified");
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
