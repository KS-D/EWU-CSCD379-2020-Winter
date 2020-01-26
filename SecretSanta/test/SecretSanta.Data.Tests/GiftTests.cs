using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        private const string _Title = "Ring Doorbell";
        private const string _Url = "www.ring.com";
        private const string _Description = "The doorbell that saw too much";
        private readonly Gift _Gift = new Gift(_Title,_Description, _Url, CreateInigo());
        static private User CreateInigo() => new User("Inigo", "Montoya");
        [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(_Gift);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            // Act
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual("Ring Doorbell", gifts[0].Title);
                Assert.AreEqual("www.ring.com", gifts[0].Url);
                Assert.AreEqual("The doorbell that saw too much", gifts[0].Description);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            _ = new Gift(null!, _Description, _Url, CreateInigo());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        { 
            _ = new Gift(_Title, null!, _Url, CreateInigo());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {

            _ = new Gift(_Title, _Description, null!, CreateInigo());
        }
    }
}
