using NUnit.Framework;
using System;
using System.IO;
using XMindAPI;
using FluentAssertions;
using System.Xml.Linq;
using System.Linq;
using Serilog.Sinks.TestCorrelator;
using Serilog;

namespace Tests
{
    [TestFixture]
    public class XMindFileBuilderTest
    {
        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new TestCorrelatorSink())
                // .WriteTo.File("log.txt")
                .CreateLogger();
        }

        [Test]
        public void CreateDefaultMetaFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            XDocument doc = build.CreateDefaultMetaFile();
            doc.ToString().Should().Be("<meta version=\"2.0\" xmlns=\"urn:xmind:xmap:xmlns:meta:2.0\" />");
        }

        [Test]
        public void CreateDefaultManifestFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            var doc = build.CreateDefaultManifestFile();
            doc.Should().NotBeNull();
        }

        [Test]
        public void CreateDefaultContentFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            var doc = build.CreateDefaultContentFile();
            doc.Should().NotBeNull();
        }
    }
}   