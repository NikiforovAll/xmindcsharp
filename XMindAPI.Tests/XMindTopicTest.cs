using NUnit.Framework;
using System.IO;
using Serilog;
using XMindAPI.Configuration;
using FluentAssertions;
using XMindAPI.Extensions;
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
