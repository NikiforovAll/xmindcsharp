using NUnit.Framework;
using System; 
using XMindAPI;

namespace Tests
{
    [TestFixture]
    public class FileBuildTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine("Hello World");
            XMindWorkBook book = new XMindWorkBook("Test");
        }
    }
}