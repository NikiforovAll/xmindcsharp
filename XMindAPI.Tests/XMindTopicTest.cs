using FluentAssertions;
using NUnit.Framework;
using System.IO;
using XMindAPI.Models;
using XMindAPI.Writers;
using System.Xml.XPath;
using System.Linq;
using static XMindAPI.Core.DOM.DOMConstants;
using XMindAPI;

namespace Tests
{
    [TestFixture]
    public class XMindTopicTest
    {
        private readonly bool _isCleanUpNeeded = false;
        private readonly string _xmindOutputFolderName = "xmind-output";

        public XMindWorkBook WorkBook { get; private set; }

        [SetUp]
        public void Setup()
        {
            // Log.Logger = new LoggerConfiguration()
            //     .MinimumLevel.Debug()
            //     .WriteTo.File("topic.test.log", retainedFileCountLimit: 3)
            //     .CreateLogger();
            WorkBook = new XMindConfiguration()
                .WriteTo
                .Writer(new InMemoryWriter())
                .CreateWorkBook(workbookName: "test");
        }

        [Test]

        public void SetTitle_DefaultFlow_Success()
        {
            //Arrange
            var book = new XMindConfiguration()
                .WithFileWriter(zip: true)
                .CreateWorkBook(workbookName: "test");
            string title = "Awesome topic";
            //Act
            var topic = book
                .CreateTopic(title);
            //Assert
            topic.GetTitle().Should().Be(title, $"because title for topic is specified");
            topic.OwnedSheet.Should().Be(book.GetPrimarySheet(), $"because topics should be added to primary sheet");
            topic.OwnedWorkbook.Should().Be(book, $"because topics are registered in book");
        }

        [Test]
        public void AddChildTopic_SingleAttachedTopic_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            var topic2 = WorkBook.CreateTopic("ChildTopic");
            //Act
            root.Add(topic2);
            //Assert
            root.Implementation.Should().HaveElement("children")
                .Which.Should().HaveElement("topics").Which.Should().HaveElement("topic")
                .Which.Should().HaveAttribute("id", topic2.GetId());
            // .And.HaveAttribute("type", "attached");
        }

        [Test]
        public void AddChildTopics_MultipleInsertedTopics_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            var topic1 = WorkBook.CreateTopic("ChildTopic1");
            var topic2 = WorkBook.CreateTopic("ChildTopic2");
            var topic3 = WorkBook.CreateTopic("ChildTopic3");
            //Act
            root.Add(topic1);
            root.Add(topic2);
            root.Add(topic3, 0);
            //Assert
            root.Implementation.XPathSelectElement("(/children/topics[@type='attached']/topic)[1]")
                .Should().HaveAttribute("id", topic3.GetId());
        }
        [Test]
        public void AddChildTopics_MultipleTopicsAndOneOfThemIsDetached_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            var topic = WorkBook.CreateTopic("ChildTopic");
            var detachedTopic = WorkBook.CreateTopic("ChildTopic");
            //Act
            root.Add(topic);
            root.Add(detachedTopic, type: TopicType.Detached);
            //Assert
            root.Implementation.XPathSelectElement("(/children/topics[@type='detached']/topic)[1]")
                .Should().HaveAttribute("id", detachedTopic.GetId());
        }

        [Test]
        public void AddLabels_MultipleLabels_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            const string label = "test-label";
            root.AddLabel(label);
            root.Implementation.Should().HaveElement("labels")
                .Which.Descendants().Should().ContainSingle();
            root.Implementation.XPathSelectElement("/labels[1]")
                .Should().HaveValue(label, "because label was created with some text");
        }

        [Test]
        public void RemoveLabel_RemoveLastLabel_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            const string label = "test-label";
            //Act
            root.AddLabel(label);
            root.RemoveLabel(label);
            //Assert
            root.Implementation.Should().HaveElement("labels")
                .Which.Descendants().Should().BeEmpty();
        }

        [Test]
        public void SetLabels_NotEmptyCollection_Success()
        {
            //Arrange
            var labelsCollection = new string[] { "l1", "l2" };
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            //Act
            root.SetLabels(labelsCollection);
            //Assert
            root.Implementation.Should().HaveElement("labels")
                .Which.Descendants().Select(elem => elem.Value)
                .Should().BeEquivalentTo(labelsCollection, "because exact collection of labels was set");
            root.GetLabels().Count.Should().Be(2, "because two labels added");
        }

        [Test]
        public void AddMarker_SingleMarker_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            //Act
            root.AddMarker(MAR_priority1);
            //Assert
            root.Implementation.Should().HaveElement(TAG_MARKER_REFS)
                .Which.Should().HaveElement(TAG_MARKER_REF)
                .Which.Should().HaveAttribute(ATTR_MARKER_ID, MAR_priority1);
        }

        [Test]
        public void RemoveMarker_EmptyMarkersAsResult_Success()
        {
            //Arrange
            var root = WorkBook.CreateTopic("Topic") as XMindTopic;
            //Act
            root.AddMarker(MAR_priority1);
            root.RemoveMarker(MAR_priority1);
            //Assert
            root.Implementation.Should().HaveElement(TAG_MARKER_REFS)
                .Which.Descendants().Select(elem => elem.Value)
                .Should().BeEmpty();
            root.HasMarker(MAR_priority1).Should().BeFalse();
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
