using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        private const string ClaimUser = "ksmith";

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
        public async Task Create_Gift_ShouldSetFingerPrintOnInitialSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, ClaimUser));

            var giftId = -1;
            Gift ringGift = SampleData.CreateRingGift();

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Gifts.Add(ringGift);
                await applicationDbContext.SaveChangesAsync();

                giftId = ringGift.Id!.Value;
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(gift);
                Assert.AreEqual(ClaimUser, gift.CreatedBy);
                Assert.AreEqual(ClaimUser, gift.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task Create_Gift_ShouldSetFingerPrintOnUpdateSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, ClaimUser));

            var giftId = -1;
            Gift ringGift = SampleData.CreateRingGift();

            const string updatedTitle = "A New Title";
            const string updatedDescription = "totally different gift";
            const string updatedUser = "TotallyNotKyle";

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Gifts.Add(ringGift);
                await applicationDbContext.SaveChangesAsync();

                giftId = ringGift.Id!.Value;
            }
            
            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, updatedUser));
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift = await applicationDbContext.Gifts.Where(a => a.Id == giftId).SingleOrDefaultAsync();
                gift.Title = updatedTitle;
                gift.Description = updatedDescription;

                await applicationDbContext.SaveChangesAsync();
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(gift);
                Assert.AreEqual(updatedTitle, gift.Title);
                Assert.AreEqual(updatedDescription, gift.Description);
                Assert.AreEqual(ClaimUser, gift.CreatedBy);
                Assert.AreEqual(updatedUser, gift.ModifiedBy);
                Assert.IsTrue(gift.ModifiedOn > gift.CreatedOn);

            }
        }

        [TestMethod]
        public async Task Gift_DeleteAGift_Success()
        {
            Gift ringGift = SampleData.CreateRingGift();
            Gift arduino = SampleData.CreateGiftArduino();

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                applicationDbContext.Gifts.Add(ringGift);
                await applicationDbContext.SaveChangesAsync();
                
                applicationDbContext.Gifts.Add(arduino);
                await applicationDbContext.SaveChangesAsync();

            }
           
            using (var applicationDbContext = new ApplicationDbContext(Options))
            { 
                applicationDbContext.Gifts.Remove(ringGift);
                await applicationDbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                List<Gift> gifts = await dbContext.Gifts.ToListAsync();

                Assert.AreEqual(1, gifts.Count);
                Assert.AreEqual(arduino.Id, gifts[0].Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            _ = new Gift(null!, SampleData.RingDescription, SampleData.RingUrl, SampleData.CreateUserInigo());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        { 
            _ = new Gift(SampleData.RingTitle, null!, SampleData.RingUrl, SampleData.CreateUserInigo());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            _ = new Gift(SampleData.RingTitle, SampleData.RingDescription, null!, SampleData.CreateUserInigo());
        }
    }
}
