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
            InitializeTestFile2();
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

        private void InitializeTestFile2()
        {
            string filepath = System.Environment.CurrentDirectory + @"\TestFile_CommaName.txt";

            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("Name: Luke, Skywalker");
                    sw.WriteLine("Lightsaber");
                    sw.WriteLine("X-Wing");
                    sw.WriteLine("Landspeeder");
                }
            }
        }

        [TestCleanup]
        public void CleanupTests()
        {
            string cwd = System.Environment.CurrentDirectory;
            File.Delete(cwd + @"\TestFile_NoCommaName.txt");
            File.Delete(cwd + @"\TestFile_CommaName.txt");
        }

        [DataTestMethod]
        [DataRow("TestFile_NoCommaName.txt", "Han", "Solo", "Hyperdrive", "Blaster", "Credits", 3)]
        [DataRow("TestFile_CommaName.txt", "Luke", "Skywalker", "Lightsaber", "X-Wing", "Landspeeder", 3)]
        public void ReadWishlistTest(string filename, string firstName, string lastName, string line1, string line2,
            string line3, int count)
        {
            Wishlist wl = GiftImportService.ReadWishlist(filename);

            Assert.IsNotNull(wl);
            Assert.IsNotNull(wl.User);
            Assert.IsNotNull(wl.Gifts);
            Assert.AreEqual(firstName, wl.User.FirstName);
            Assert.AreEqual(lastName, wl.User.LastName);
            Assert.AreEqual(line1, wl.Gifts[0].Title);
            Assert.AreEqual(line2, wl.Gifts[1].Title);
            Assert.AreEqual(line3, wl.Gifts[2].Title);
            Assert.AreEqual(count, wl.Gifts.Count);
        }

        [TestMethod]
        public void GetUserFromHeaderTest()
        {
            string header = "Name: Luke Skywalker";
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.AreEqual("Luke", user.FirstName);
            Assert.AreEqual("Skywalker", user.LastName);
        }
    }
}
