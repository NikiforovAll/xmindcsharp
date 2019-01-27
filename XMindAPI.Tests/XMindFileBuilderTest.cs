using NUnit.Framework;
using System;
using System.IO;
using XMindAPI;

namespace Tests
{
    [TestFixture]
    public class XMindDocumentBuilderTest
    {
        internal XMindDocumentBuilder Builder { get; set; }

        [SetUp]
        public void Setup()
        {
            Builder = new XMindDocumentBuilder();
        }

        [Test]
        public void CreateDefaultMetaFile_DefaultCreate_Success()
        {
            XMindDocumentBuilder build = new XMindDocumentBuilder();
            var doc = build.CreateDefaultMetaFile();
            using (StringWriter sw = new StringWriter())
            {
                doc.Save(sw);
                TestContext.WriteLine($"doc: {sw.ToString()}");
            }
        }
    }
}