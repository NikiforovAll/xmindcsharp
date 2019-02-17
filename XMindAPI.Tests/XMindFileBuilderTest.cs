using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;

using NUnit.Framework;
using FluentAssertions;

using Serilog.Sinks.TestCorrelator;
using Serilog;

using XMindAPI;
using XMindAPI.Builders;

namespace Tests
{
    [TestFixture]
    public class XMindFileBuilderTest
    {
        [SetUp]
        public void Setup()
        {
            // Log.Logger = new LoggerConfiguration()
            //     .MinimumLevel.Debug()
            //     .WriteTo.Sink(new TestCorrelatorSink())
            //     .WriteTo.File("log.txt")
            //     .CreateLogger();
        }

        [Test]
        public void CreateDefaultMetaFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            XDocument doc = build.CreateMetaFile();
            doc.ToString().Should().Be("<meta version=\"2.0\" xmlns=\"urn:xmind:xmap:xmlns:meta:2.0\" />");
        }

        [Test]
        public void CreateDefaultManifestFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            var doc = build.CreateManifestFile();
            doc.Should().NotBeNull();
        }

        [Test]
        public void CreateDefaultContentFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            var doc = build.CreateContentFile();
            doc.Should().NotBeNull();
        }
    }
}   