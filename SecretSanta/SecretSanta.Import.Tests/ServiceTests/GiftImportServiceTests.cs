using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Import.Services;
using System.IO;

namespace SecretSanta.Import.Tests.ServiceTests
{
    [TestClass]
    public class GiftImportServiceTests
    {
        [TestInitialize]
        public void InitializeGiftImportTests()
        {
            InitializeTestFile1();   
        }

        private void InitializeTestFile1()
        {
            string filepath = System.Environment.CurrentDirectory + @"\TestFile_NoCommaName.txt";

            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("Name: Han Solo");
                    sw.WriteLine("Hyperdrive");
                    sw.WriteLine("Blaster");
                    sw.WriteLine("Credits");
                }
            }
        }

        [TestCleanup]
        public void CleanupTests()
        {
            string cwd = System.Environment.CurrentDirectory;
            File.Delete(cwd + @"\TestFile_NoCommaName.txt");
        }

        [DataTestMethod]
        [DataRow("TestFile_NoCommaName.txt")]
        public void ReadWishlistTest(string filename)
        {
            Wishlist wl = GiftImportService.ReadWishlist(filename);

            Assert.IsNotNull(wl);
            Assert.IsNotNull(wl.User);
            Assert.IsNotNull(wl.Gifts);
            Assert.AreEqual("Han", wl.User.FirstName);
            Assert.AreEqual(3, wl.Gifts.Count);
        }
    }
}
