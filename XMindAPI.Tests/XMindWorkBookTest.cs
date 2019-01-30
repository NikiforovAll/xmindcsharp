using NUnit.Framework;
using System;
using System.IO;
using XMindAPI;
using FluentAssertions;
using System.Xml.Linq;
using System.Linq;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using XMindAPI.Configuration;
using XMindAPI.Writers;

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
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        [Test]
        public void Save_CreateEmptyBook_Success()
        {
           var book = new XMindConfiguration()
                .WriteTo
                .SetFinalizeAction(context => Log.Logger.Information("Finalized"))
                .Writer(new LoggerWriter()
                        .SetOutputName(new LoggerWriterOutput("root")))
                .CreateWorkBook("test");
            using (TestCorrelator.CreateContext())
            {
                book.Save();
                TestCorrelator.GetLogEventsFromCurrentContext().Should()
                .HaveCount(4, "empty book initialization had failed");
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Where(e => !e.MessageTemplate.ToString().Contains("Finalized", StringComparison.OrdinalIgnoreCase))
                    .All(e => e.MessageTemplate.ToString().Contains("root")).Should().BeTrue();
            }
        }

        [Test]
        public void Save_CreateEmptyBookWithFileLogger_Success()
        {
            var book = new XMindConfiguration()
                .WriteTo
                .Writer(new LoggerWriter()
                        .SetOutputName(new LoggerWriterOutput("root")))
                .CreateWorkBook("test");
        }
    }
}   