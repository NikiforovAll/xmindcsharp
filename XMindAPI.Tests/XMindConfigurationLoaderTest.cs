using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;

using XMindAPI.Configuration;
namespace Tests
{
    [TestFixture]
    public class XMindConfigurationLoaderTest
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
        public void GetOutputLocationsDictionary_Success()
        {
            //TODO: this is not something we want to test since it depends on config
            //Act
            IDictionary<string, string> locations = XMindConfigurationLoader
                .Configuration.GetOutputFilesLocations();
            //Assert
            locations.Should().NotBeEmpty();
        }
    }
}
