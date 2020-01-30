using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        [TestMethod]
        public override async Task FetchAll_RetrievesAllEntities_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<Gift> service = GetService(dbContext);
            (Gift entity, Gift secondEntity) = GetEntities();

            await dbContext.Gifts.AddAsync(entity);
            await dbContext.Gifts.AddAsync(secondEntity);
            await dbContext.SaveChangesAsync();

            List<Gift> entities = await service.FetchAllAsync();

            Gift entityFromDb = entities[0];
            Gift secondEntityFromDb = entities[1];

            AssertEntitiesAreEqual(entity, entityFromDb);
            AssertEntitiesAreEqual(secondEntity, secondEntityFromDb);
            Assert.IsNotNull(entityFromDb.User);
            Assert.IsNotNull(secondEntityFromDb.User);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), 
                        (entityFromDb.User.FirstName,entityFromDb.User.LastName));
            Assert.AreEqual((SampleData.Princess, SampleData.Buttercup),
                        (secondEntityFromDb.User.FirstName,secondEntityFromDb.User.LastName));
        }

        [TestMethod]
        public override async Task Fetch_RetrievesOneEntity_Success()
        {
            using var dbContext = new ApplicationDbContext(Options);
            EntityService<Gift> service = GetService(dbContext);
            (Gift entity, Gift secondEntity) = GetEntities();

            await dbContext.Set<Gift>().AddAsync(entity);
            await dbContext.Set<Gift>().AddAsync(secondEntity);
            await dbContext.SaveChangesAsync();

            Gift entityFromDb = await service.FetchByIdAsync(entity.Id!.Value);

            AssertEntitiesAreEqual(entity, entityFromDb);
            Assert.IsNotNull(entityFromDb.User);
            Assert.AreEqual((SampleData.Inigo, SampleData.Montoya), 
                        (entityFromDb.User.FirstName,entityFromDb.User.LastName));
        }
    }
}
