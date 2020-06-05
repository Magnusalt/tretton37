using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SiteSaver.Tests
{
    [TestClass]
    public class DiskFileHandlerTests
    {
        private IFileHandler _sut;

        private IFileSystem _fileSystem;

        [TestInitialize]
        public void Setup()
        {
            var destination = "root";
            _fileSystem = Substitute.For<IFileSystem>();
            _sut = new DiskFileHandler(destination, _fileSystem, Substitute.For<ILogger<DiskFileHandler>>());
        }

        [TestMethod]
        public async Task Store_SavesAFileToDisk_WithProvidedContentAndToCorrectPath()
        {
            var filePath = Path.Combine("tretton37test", "index.html");
            var htmlAsBytes = System.Text.Encoding.UTF8.GetBytes("<body><p>hello world!</p></body>");

            await _sut.Store(filePath, htmlAsBytes);

            await _fileSystem.Received().WriteFileToDisk(Path.Combine("root", filePath), htmlAsBytes);
        }

        [TestMethod]
        public async Task Store_AddsIndexDotHtmlToPath_WhenProvidedPathEndsWithDirectorySeparator()
        {
            var filePath = "tretton37test" + Path.DirectorySeparatorChar;
            var htmlAsBytes = System.Text.Encoding.UTF8.GetBytes("<body><p>hello world!</p></body>");

            await _sut.Store(filePath, htmlAsBytes);

            await _fileSystem.Received().WriteFileToDisk(Path.Combine("root", filePath, "index.html"), htmlAsBytes);
        }

        [TestMethod]
        public async Task Store_AddsDotHtmlToPath_WhenProvidedPathHasNoExtension()
        {
            var filePath = "tretton37test";
            var htmlAsBytes = System.Text.Encoding.UTF8.GetBytes("<body><p>hello world!</p></body>");

            await _sut.Store(filePath, htmlAsBytes);

            await _fileSystem.Received().WriteFileToDisk(Path.ChangeExtension(Path.Combine("root", filePath), "html"), htmlAsBytes);
        }
    }
}