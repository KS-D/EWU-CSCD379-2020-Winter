using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                Assert.AreEqual("title", gift.Title);
            }
        }

    }
}
