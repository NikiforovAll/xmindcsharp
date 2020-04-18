using System.IO;
using System;
using XMindAPI.Configuration;
using XMindAPI.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace simple
{
    class Program
    {
        private static ILogger<Program> logger;

        static async Task Main(
            string demo = "memory"
        )
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();

            logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();


            switch (demo)
            {
                case "file":
                    await SaveWorkBookToFileSystem();
                    break;
                case "memory":
                    await InMemoryWorkBook();
                    break;
            }
        }

        private async static Task InMemoryWorkBook()
        {
            var book = new XMindConfiguration()
                .WithInMemoryWriter()
                .CreateWorkBook("test.xmind");
            await book.Save();
        }

        private static async Task SaveWorkBookToFileSystem()
        {
            string basePath = Path.Combine(Path.GetTempPath(), "xmind-test");
            var bookName = "test.xmind";
            logger.LogInformation(default(EventId), $"Base path: ${Path.Combine(basePath, bookName)}");
            var book = new XMindConfiguration()
                .WithFileWriter(basePath, true)
                .CreateWorkBook(bookName);
            await book.Save();
        }
    }
}
