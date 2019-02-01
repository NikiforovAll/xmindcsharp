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
                .WriteTo.File("{Date}.log", retainedFileCountLimit: 3)
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
        public void Save_CreateEmptyBookWithFileWriterInCaseOfCustomBasePath_Success()
        {
            var book = new XMindConfiguration()
                .WriteTo.Writers(
                    new List<IXMindWriter>{
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig("manifest.xml").SetBasePath("custom-output/META-INF")),
                        new FileWriter()
                            .SetOutput(new FileWriterOutputConfig("root.xml").SetBasePath("custom-output/"))
                    })
                .WriteTo.SetWriterBinding(
                    new List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>>{
                        FileWriterUtils.ResolveManifestFile, 
                        FileWriterUtils.ResolveOtherFiles
                    }
                )
                .CreateWorkBook("test");
            book.Save();
            //TODO: add assertions, test IO properly, ideally it unit tests should not leave artifacts
        }

        [Test]
        public void Save_CreateEmptyBookWithDefaultPath_Success()
        {
            var book = new XMindConfiguration()
                .WriteTo.Writers(
                    new List<IXMindWriter>{
                        new FileWriter().SetOutput(new FileWriterOutputConfig("manifest.xml", true)),
                        new FileWriter().SetOutput(new FileWriterOutputConfig("default", true))
                    })
                .WriteTo.SetWriterBinding(
                    new List<Func<XMindWriterContext, List<IXMindWriter>, IXMindWriter>>{
                        FileWriterUtils.ResolveManifestFile, 
                        FileWriterUtils.ResolveOtherFiles
                    }
                )
                .CreateWorkBook("test");
            book.Save();
            //TODO: add assertions, test IO properly, ideally it unit tests should not leave artifacts
        }
    }
}   