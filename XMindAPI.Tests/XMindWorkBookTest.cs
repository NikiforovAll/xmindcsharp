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
using System.Collections.Generic;

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
        public void Save_CreateEmptyBookWithLogger_Success()
        {
           var book = new XMindConfiguration()
                .WriteTo
                .SetFinalizeAction(context => Log.Logger.Information("Finalized"))
                .Writer(new LoggerWriter()
                        .SetOutput(new LoggerWriterOutputConfig("root")))
                .CreateWorkBook("test");
            using (TestCorrelator.CreateContext())
            {
                book.Save();
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
        public void Save_CreateEmptyBookWithFileWriter_Success()
        {
            var book = new XMindConfiguration()
                .WriteTo.Writers(
                    new List<IXMindWriter>{
                        new FileWriter().SetOutput(new FileWriterOutputConfig("manifest.xml").SetBasePath("output/META-INF")),
                        new FileWriter().SetOutput(new FileWriterOutputConfig("root.xml").SetBasePath("output/"))
                    })
                .WriteTo.SetWriterBinding(
                    new List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>>{ResolveManifestFile, ResolveOtherFiles}
                )
                .CreateWorkBook("test");
            book.Save();
        }

        //TODO: fix bug !!
        private IXMindWriter ResolveManifestFile(XMindWriterContext context, List<IXMindWriter> writers)
        {
            var file = "manifest.xml";
            return writers.Where(w => context.FileName.Equals(file) && w.GetOutputConfig().OutputName.Equals(file)).FirstOrDefault();
        }
        private IXMindWriter ResolveOtherFiles(XMindWriterContext context, List<IXMindWriter> writers)
        {
             var file = "manifest.xml";
            return writers.Where(w => !context.FileName.Equals(file) || !w.GetOutputConfig().OutputName.Equals(file)).FirstOrDefault();
        }
    }
}   