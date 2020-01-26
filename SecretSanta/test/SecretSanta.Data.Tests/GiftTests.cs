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
       [TestMethod]
        public async Task Gift_CanBeSavedToDatabase()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.Gifts.Add(SampleData.CreateRingGift());
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
            _ = new Gift(null!, SampleData.RingDescription, SampleData.RingUrl, SampleData.CreateInigo());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        { 
            _ = new Gift(SampleData.RingTitle, null!, SampleData.RingUrl, SampleData.CreateInigo());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {

            _ = new Gift(SampleData.RingTitle, SampleData.RingDescription, null!, SampleData.CreateInigo());
        }
    }
}
