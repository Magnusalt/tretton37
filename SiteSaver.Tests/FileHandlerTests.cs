using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace SiteSaver.Tests
{
    [TestClass]
    public class FileHandlerTests
    {
        private IFileHandler _sut;

        [TestInitialize]
        public void Setup()
        {
            var destination = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _sut = new DiskFileHandler(destination);
        }

        [TestMethod]
        [Ignore("This is an integration test to verify that downloaded contents get saved correctly to file on disk, used for development purpose")]
        public async Task Save_SavesAFileToDisk_WithProvidedContentAndToCorrectPath_VerifiedManually()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(basePath, "tretton37test", "index.html");

            await _sut.Store(filePath, "<body><p>hello world!</p></body>");
        }

        [TestMethod]
        [Ignore("This is an integration test to verify that downloaded contents get saved correctly to file on disk, used for development purpose")]
        public async Task Save_SavesMultipleFilesToDisk_WithProvidedContentAndToCorrectPath_VerifiedManually()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            var saveTasks = new Task[20];

            for (int i = 0; i < 20; i++)
            {
                var filePath = Path.Combine(basePath, "tretton37testmulti", $"index{i}.html");

                saveTasks[i] = _sut.Store(filePath, "<body><p>hello world!</p></body>");
            }

            await Task.WhenAll(saveTasks);
        }
    }
}