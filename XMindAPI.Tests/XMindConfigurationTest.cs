using NUnit.Framework;
using System;
using System.IO;
using XMindAPI.Configuration;
using XMindAPI.Writers;
using XMindAPI;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class XMindConfigurationTest
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
            var config = new XMindConfiguration()
                .WriteTo
                .Writer(new LoggerWriter()
                        .SetOutput(new LoggerWriterOutputConfig("root")));
           
        }
    }
}