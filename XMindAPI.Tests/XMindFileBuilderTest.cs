using NUnit.Framework;
using System;
using System.IO;
using XMindAPI;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class XMindDocumentBuilderTest
    {
        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new TestCorrelatorSink())
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        [Test]
        public void CreateDefaultMetaFile_DefaultCreate_Success()
        {

            using (TestCorrelator.CreateContext())
            {
                XMindDocumentBuilder build = new XMindDocumentBuilder();
                var doc = build.CreateDefaultMetaFile();
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle();
                
            }
        }
    }
}   