using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Data;
using SecretSanta.Data.Tests;


namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftServiceTests : EntityServiceBaseTest<Gift>
    {
        public override (Gift entity, Gift secondEntity) GetEntities()
        {
            return (SampleData.CreateRingGift(), SampleData.CreateGiftArduino());
        }

        public override EntityService<Gift> GetService(ApplicationDbContext dbContext)
        {
            return new GiftService(dbContext, Mapper);
        }

        public override void AssertEntitiesAreEqual(Gift expected, Gift actual)
        {
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Url, actual.Url);
        }

        public override Gift UpdateEntity(Gift entity, string update)
        {
            entity.Description = update;
            return entity;
        }
    }
}
