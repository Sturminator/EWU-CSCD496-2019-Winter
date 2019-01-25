﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Import.Services;
using System;
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
            InitializeTestFile3();
        }

        private void InitializeTestFile1()
        {
            string filepath = System.IO.Path.GetTempPath() + @"\TestFile_NoCommaName.txt";

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
            string filepath = System.IO.Path.GetTempPath() + @"\TestFile_CommaName.txt";

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

        private void InitializeTestFile3()
        {
            string filepath = System.IO.Path.GetTempPath() + @"\TestFile_BlankLines.txt";

            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine("Name: Blank Blankerson");
                    sw.WriteLine("");
                    sw.WriteLine("Not Blank");
                    sw.WriteLine("");
                    sw.WriteLine("Also Not Blank");
                }
            }
        }

        [TestCleanup]
        public void CleanupTests()
        {
            string cwd = System.IO.Path.GetTempPath();
            File.Delete(cwd + @"\TestFile_NoCommaName.txt");
            File.Delete(cwd + @"\TestFile_CommaName.txt");
            File.Delete(cwd + @"\TestFile_BlankLines.txt");
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
        public void ReadWishlistBlankLinesTest()
        {
            Wishlist wl = GiftImportService.ReadWishlist("TestFile_BlankLines.txt");

            Assert.IsNotNull(wl);
            Assert.IsNotNull(wl.User);
            Assert.IsNotNull(wl.Gifts);
            Assert.AreEqual("Blank", wl.User.FirstName);
            Assert.AreEqual("Blankerson", wl.User.LastName);
            Assert.AreEqual("Not Blank", wl.Gifts[0].Title);
            Assert.AreEqual("Also Not Blank", wl.Gifts[1].Title);
            Assert.AreEqual(2, wl.Gifts.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "Filename cannot be null or empty.")]
        public void FileDoesNotExistTest()
        {
            string filename = "TestFile_DoesNotExist.txt";

            Wishlist wl = GiftImportService.ReadWishlist(filename);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Filename cannot be null or empty.")]
        public void FileIsEmpty()
        {
            string filename = "";

            Wishlist wl = GiftImportService.ReadWishlist(filename);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Filename cannot be null or empty.")]
        public void FileIsNull()
        {
            string filename = null;

            Wishlist wl = GiftImportService.ReadWishlist(filename);
        }

        [TestMethod]
        public void GetUserFromHeaderWrongFormatTest()
        {
            string header = "Luke Skywalker";
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserFromHeaderNullTest()
        {
            string header = null;
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserFromHeaderTest()
        {
            string header = "Name: Luke Skywalker";
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.AreEqual("Luke", user.FirstName);
            Assert.AreEqual("Skywalker", user.LastName);
        }

        [TestMethod]
        public void GetUserFromHeaderExtraSpacesTest()
        {
            string header = "Name:    Luke     Skywalker  ";
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.AreEqual("Luke", user.FirstName);
            Assert.AreEqual("Skywalker", user.LastName);
        }

        [TestMethod]
        public void GetUserFromHeaderTooManyNamesTest()
        {
            string header = "Name: Christopher Raeder Sturm";
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserFromHeaderCommaTest()
        {
            string header = "Name: Luke, Skywalker";
            User user = GiftImportService.GetUserFromHeader(header);

            Assert.AreEqual("Luke", user.FirstName);
            Assert.AreEqual("Skywalker", user.LastName);
        }
    }
}