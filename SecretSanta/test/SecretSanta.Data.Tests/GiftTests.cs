using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        private readonly Gift _Gift =  new Gift
                {
                    Title = "title",
                    Url = "url",
                    Description = "a nice gift",
                    User = new User()

                };

        private const string _UserName = "kyle";

        [TestMethod]
        public async Task Create_Gift_DatabaseShouldSaveGift()
        {
            var giftId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {

                applicationDbContext.Gifts.Add(_Gift);
                await applicationDbContext.SaveChangesAsync();

                giftId = _Gift.Id;
            }

            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(gift);
                Assert.AreEqual(_Gift.Title, gift.Title);
                Assert.AreEqual(_Gift.Description, gift.Description);
                Assert.AreEqual(_Gift.Url, gift.Url);
            }
        }

        [TestMethod]
        public async Task Create_Gift_ShouldSetFingerPrintOnInitialSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, _UserName));

            var giftId = -1;

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Gifts.Add(_Gift);
                await applicationDbContext.SaveChangesAsync();

                giftId = _Gift.Id;
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(gift);
                Assert.AreEqual(_UserName, gift.CreatedBy);
                Assert.AreEqual(_UserName, gift.ModifiedBy);

            }
        }

        [TestMethod]
        public async Task Create_Gift_ShouldSetFingerPrintOnUpdateSave()
        {
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, _UserName));

            var giftId = -1;

            const string updatedTitle = "A New Title";
            const string updatedDescription = "totally different gift";
            const string updatedUser = "TotallyNotKyle";

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                applicationDbContext.Gifts.Add(_Gift);
                await applicationDbContext.SaveChangesAsync();

                giftId = _Gift.Id;
            }
            
            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta => 
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, updatedUser));
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = await applicationDbContext.Gifts.Where(a => a.Id == giftId).SingleOrDefaultAsync();
                gift.Title = updatedTitle;
                gift.Description = updatedDescription;

                await applicationDbContext.SaveChangesAsync();
            }
            
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();
                
                Assert.IsNotNull(gift);
                Assert.AreEqual(updatedTitle, gift.Title);
                Assert.AreEqual(updatedDescription, gift.Description);
                Assert.AreEqual(_UserName, gift.CreatedBy);
                Assert.AreEqual(updatedUser, gift.ModifiedBy);
                Assert.IsTrue(gift.ModifiedOn > gift.CreatedOn);

            }
        }
    }
}
